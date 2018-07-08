using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlatformImplementation;
using PlatformPOC.PlatformContracts;
using PlatformPOC.PlatformImplementation;
using Serilog;

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

            // Load Services from external assembly
            // TODO: Load assembly location from configuration
            var assembly = Assembly.LoadFile(@"G:\Code\Source\projects\PlatformPOC\TestService\bin\Debug\netcoreapp2.0\TestService.dll");
            foreach (var serviceMethod in assembly.ExportedTypes)
            {
                services.AddTransient(typeof(IServiceMethod), serviceMethod.UnderlyingSystemType);                
            }

            var part = new AssemblyPart(assembly);
            services.AddMvc()
                .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(part));

            services.AddTransient<PlatformMiddleware>();
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
    }
}
