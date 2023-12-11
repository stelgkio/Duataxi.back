using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DuaTaxi.Common.Mongo;
using DuaTaxi.Common.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DuaTaxi.Common.Dispatchers;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Common;
using DuaTaxi.Service.TaxiApi.Entities;
using Consul;
using DuaTaxi.Service.TaxiApi.Messages.Commands;
using DuaTaxi.Common.Jaeger;
using DuaTaxi.Common.Consul;
using DuaTaxi.Services.TaxiApi.Messages.Commands;
using DuaTaxi.Services.TaxiApi.Entities.Models;
using DuaTaxi.Common.RestEase;
using DuaTaxi.Services.TaxiApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DuaTaxi.Services.TaxiApi.Framework;
using DuaTaxi.Common.Redis;
using DuaTaxi.Services.TaxiApi.Hubs;

namespace DuaTaxi.Service.TaxiApi
{
    public class Startup
    {
        private static readonly string[] Headers = new[] { "X-Operation", "X-Resource", "X-Total-Count" };
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer Container { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCustomMvc();
            services.AddInitializers(typeof(IMongoDbInitializer));
            services.AddConsul();
            services.AddJaeger();
            services.AddOpenTracing();
            services.RegisterServiceForwarder<IPaymentService>("payment-service");
            // services.AddTransient<IMetricsRegistry, MetricsRegistry>();

            AddSignalR(services);


            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => {
                o.Authority = "http://localhost:5000";
                o.Audience = "resourceapi";
                o.RequireHttpsMetadata = false;
            });

           
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ApiReader", policy => policy.RequireClaim("scope", "api.read"));
            //    options.AddPolicy("Consumer", policy => policy.RequireClaim(ClaimTypes.Role, "consumer"));
            //});


            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(Startup).Assembly)
                .AsImplementedInterfaces();
            builder.Populate(services);
            builder.AddDispatchers();
            builder.AddMongo();
            builder.AddMongoRepository<Customer>("Customers");
            builder.AddMongoRepository<TaxiDriverStatus>("TaxiDriverStatus");
            builder.AddMongoRepository<ActiveDrivers>("ActiveDrivers");
            builder.AddMongoRepository<DriversMapPosition>("DriversMapPosition");

            builder.AddRabbitMq();

            Container = builder.Build();

            return new AutofacServiceProvider(Container);

        }

        private void AddSignalR(IServiceCollection services)
        {
            var options = Configuration.GetOptions<SignalrOptions>("signalr");
            services.AddSingleton(options);
            var builder = services.AddSignalR();
            if (!options.Backplane.Equals("redis", StringComparison.InvariantCultureIgnoreCase)) {
                return;
            }
            var redisOptions = Configuration.GetOptions<RedisOptions>("redis");
            builder.AddRedis(redisOptions.ConnectionString);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SignalrOptions signalrOptions,
           IApplicationLifetime applicationLifetime, IStartupInitializer initializer, IConsulClient consulClient)
        {
            if (env.IsDevelopment() || env.EnvironmentName == "local") {
                app.UseDeveloperExceptionPage();
            }

            //  app.UseErrorHandler();
            app.UseMvc();
            app.UseServiceId();
            app.UseRabbitMq()
                .SubscribeCommand<CreateTaxiDriver>()
                .SubscribeCommand<CreateTaxiDriverCustomer>()
                .SubscribeCommand<DeleteTaxiDriverCustomer>()
                .SubscribeCommand<InitializeTaxiDriver>()
                .SubscribeCommand<TaxiDriverActivation>()
                .SubscribeCommand<TaxiDriverMapPosition>();
            //.SubscribeCommand<CreateDiscount>(onError: (cmd, ex)
            //    => new CreateDiscountRejected(cmd.CustomerId, ex.Message, "customer_not_found"))
            //.SubscribeEvent<CustomerCreated>(@namespace: "customers")
            //.SubscribeEvent<OrderCompleted>(@namespace: "orders");
            var serviceId = app.UseConsul();
            
            app.UseSignalR(routes => {
                routes.MapHub<TaxiDriverHub>($"/{signalrOptions.Hub}");
            });

            applicationLifetime.ApplicationStopped.Register(() => {
                consulClient.Agent.ServiceDeregister(serviceId);
                Container.Dispose();
            });
            initializer.InitializeAsync();

        }
    }
}
