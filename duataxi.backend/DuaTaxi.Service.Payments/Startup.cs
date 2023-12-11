using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Consul;
using DuaTaxi.Common;
using DuaTaxi.Common.Consul;
using DuaTaxi.Common.Dispatchers;
using DuaTaxi.Common.Jaeger;
using DuaTaxi.Common.Mongo;
using DuaTaxi.Common.Mvc;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Service.Payments.Entities.Models;
using DuaTaxi.Service.Payments.Messages.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DuaTaxi.Service.Payments
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCustomMvc();
            services.AddInitializers(typeof(IMongoDbInitializer));
            services.AddConsul();
            services.AddJaeger();
            services.AddOpenTracing();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", cors =>
                        cors.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithExposedHeaders(Headers));
            });
            //services.RegisterServiceForwarder<IOperationsService>("operations-service");
            //services.RegisterServiceForwarder<ITaxiService>("taxiapi-service");
            // services.RegisterServiceForwarder<IOrdersService>("orders-service");
            //services.AddTransient<IMetricsRegistry, MetricsRegistry>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = "http://localhost:5000";
                o.Audience = "resourceapi";
                o.RequireHttpsMetadata = false;
            });


            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(Startup).Assembly)
                .AsImplementedInterfaces();
            builder.Populate(services);
            builder.AddDispatchers();
            builder.AddMongo();
            builder.AddMongoRepository<Payment>("Payments");
          
            builder.AddRabbitMq();

            Container = builder.Build();

            return new AutofacServiceProvider(Container);

        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
           IApplicationLifetime applicationLifetime, IStartupInitializer initializer
            ,IConsulClient consulClient
            )
        {
            if (env.IsDevelopment() || env.EnvironmentName == "local")
            {
                app.UseDeveloperExceptionPage();
            }

            initializer.InitializeAsync();
            app.UseMvc();
            app.UseRabbitMq()
             .SubscribeCommand<CreateFirstPayment>();
            //.SubscribeCommand<CreateDiscount>(onError: (cmd, ex)
            //    => new CreateDiscountRejected(cmd.CustomerId, ex.Message, "customer_not_found"))
            //.SubscribeEvent<CustomerCreated>(@namespace: "customers")
            //.SubscribeEvent<OrderCompleted>(@namespace: "orders");
            var serviceId = app.UseConsul();

            applicationLifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceId);
                Container.Dispose();
            });
            initializer.InitializeAsync();
        }
    }
}
