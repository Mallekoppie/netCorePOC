using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace PlatformPOC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }


        /*
        public static IWebHost BuildWebHost(string[] args) =>
            
        WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
                */

        public static IMetricsRoot Metrics { get; set; }

        public static IWebHost BuildWebHost(string[] args)
        {
            Metrics = AppMetrics.CreateDefaultBuilder()
                    .OutputMetrics.AsPrometheusPlainText()
                    .OutputMetrics.AsPrometheusProtobuf()
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
                            .UseStartup<Startup>()
                            .UseUrls("http://localhost:10010/")
                            .Build();
        }

    }
}
