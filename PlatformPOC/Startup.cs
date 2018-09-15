using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PlatformContracts;
using PlatformImplementation;
using PlatformPOC.PlatformContracts;
using PlatformPOC.PlatformImplementation;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace PlatformPOC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //https://www.darylcumbo.net/serilog-vs-nlog-benchmarks/
            var log = new LoggerConfiguration()
                .WriteTo.File("Log.txt", buffered: true, flushToDiskInterval: TimeSpan.FromMilliseconds(1000))
                .CreateLogger();
            services.AddSingleton(log);

            services.AddSingleton<IPlatform, Platform>();
            services.AddSingleton<IPlatformMetrics, PlatformMetrics>();

            // Load Services from external assembly
            // TODO: Load assembly location from configuration
            var assembly = Assembly.LoadFile(@"G:\Code\Source\projects\netCorePOC\TestService\bin\Debug\netcoreapp2.0\TestService.dll");
            foreach (var serviceMethod in assembly.ExportedTypes)
            {
                services.AddTransient(typeof(IServiceMethod), serviceMethod.UnderlyingSystemType);
            }

            var part = new AssemblyPart(assembly);
            services.AddMvc()
                .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(part));

            services.AddTransient<PlatformMiddleware>();

            // Configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                // only for testing
                cfg.RequireHttpsMetadata = false;
                cfg.Authority = "http://localhost:8180/auth/realms/SpringBootKeycloak";
                cfg.IncludeErrorDetails = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = "http://localhost:8180/auth/realms/SpringBootKeycloak",
                    ValidateLifetime = true
                };

            });            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<PlatformMiddleware>();

            app.UseMvc();                        
        }

        private static Action<MetricsWebHostOptions> Configure()
        {
            return options =>
            {


                //options.TrackingMiddlewareOptions = middlewareOptions => { middlewareOptions.IgnoredHttpStatusCodes = new[] { 500 }; };
            };
        }
    }
}
