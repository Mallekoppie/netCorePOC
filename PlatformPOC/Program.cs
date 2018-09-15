using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PlatformPOC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IMetricsRoot Metrics { get; set; }

        public static IWebHost BuildWebHost(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            Metrics = AppMetrics.CreateDefaultBuilder()
                    .OutputMetrics.AsPrometheusPlainText()
                    .OutputMetrics.AsPrometheusProtobuf()
                    .Configuration.ReadFrom(configuration)
                    .Build();

            return WebHost.CreateDefaultBuilder(args)
                            .ConfigureMetrics(Metrics)
                            .UseMetrics(
                                options =>
                                {
                                    options.EndpointOptions = endpointsOptions =>
                                    {
                                        endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter(new MetricsPrometheusOptions());
                                        endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter(new MetricsPrometheusOptions());
                                    };
                                })
                                .UseMetricsWebTracking()
                            .UseStartup<Startup>()
                            .UseUrls("http://localhost:10010/")
                            .Build();
        }

    }
}
