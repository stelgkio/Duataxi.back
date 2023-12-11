using AuthServer.Extensions;
using AuthServer.Infrastructure.Data.Identity;
using AuthServer.Infrastructure.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Consul;
using DuaTaxi.Common;
using DuaTaxi.Common.Consul;
using DuaTaxi.Common.Dispatchers;
using DuaTaxi.Common.Jaeger;
using DuaTaxi.Common.Mvc;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Common.Swagger;
using DuaTaxi.DBContext.Data.Context;
using DuaTaxi.Entities.Core.Models;
using IdentityServer4;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Net;
using System.Reflection;

namespace AuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private static readonly string[] Headers = new[] { "X-Operation", "X-Resource", "X-Total-Count" };
        public IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc();
            services.AddConsul();
            services.AddJaeger();
            services.AddOpenTracing();



            services.AddDbContext<DuaTaxiDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddIdentity<AppUser, IdentityRole>(options=> {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<DuaTaxiDbContext>()
                .AddDefaultTokenProviders();

    
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = _builder => _builder.UseSqlServer(Configuration.GetConnectionString("Default"));
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30; // interval in seconds
                })                
                //.AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<AppUser>()
                .AddJwtBearerClientAuthentication();


            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options => {
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;
                options.Audience = "resourceapi";
                options.TokenValidationParameters = new TokenValidationParameters() {
                    ClockSkew = TimeSpan.FromMinutes(0),
                    //Na kanw mia dokimh me auto.
                    //-----RoleClaimType= "role"

                };
            });


            //services.AddAuthentication(options => {
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer("Bearer", o => {
            //    o.Authority = "http://localhost:5000";
            //    o.Audience = "resourceapi";
            //    o.RequireHttpsMetadata = false;
            //    o.TokenValidationParameters = new TokenValidationParameters() {
            //        ClockSkew = TimeSpan.FromMinutes(0),
            //        //Na kanw mia dokimh me auto.
            //        //-----RoleClaimType= "role"

            //    };
            //});




            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "<insert here>";
                    options.ClientSecret = "<insert here>";
                });
               

            services.AddTransient<IProfileService, IdentityClaimsProfileService>();

            // services.AddCors(options => options.AddPolicy("AllowOrigin", p => p.WithOrigins("https://webeasytravel.firebaseapp.com/")
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()));

            services.AddCors(options => options.AddPolicy("AllowAll", 
                p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders(Headers)));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // services.AddHttpsRedirection(options =>
            // {
            //     options.HttpsPort = 5550;
            // });

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                    .AsImplementedInterfaces();
            builder.Populate(services);
            builder.AddRabbitMq();
            builder.AddDispatchers();

            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
          IApplicationLifetime applicationLifetime, IConsulClient client,
          IStartupInitializer startupInitializer)
            {
            if (env.IsDevelopment() || env.EnvironmentName == "local")
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                    }
                });
            });

            //var serilog = new LoggerConfiguration()
            //    .MinimumLevel.Verbose()
            //    .Enrich.FromLogContext()
            //    .WriteTo.File(@"authserver_log.txt");

            //loggerFactory.WithFilter(new FilterLoggerSettings
            //    {
            //        { "IdentityServer4", LogLevel.Debug },
            //        { "Microsoft", LogLevel.Warning },
            //        { "System", LogLevel.Warning },
            //    }).AddSerilog(serilog.CreateLogger());

            app.UseStaticFiles();
            app.UseCors("AllowAll");
          //  app.UseHttpsRedirection();
            app.UseIdentityServer();
            app.UseServiceId();
            app.UseErrorHandler();
            app.UseRabbitMq();

            app.UseMvc();
            app.UseAllForwardedHeaders();
            app.UseAuthentication();

            

            var consulServiceId = app.UseConsul();
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                client.Agent.ServiceDeregister(consulServiceId);
                Container.Dispose();
            });

            startupInitializer.InitializeAsync();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
