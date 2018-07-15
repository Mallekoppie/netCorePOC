using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;
using PlatformContracts;
using PlatformPOC.PlatformContracts;
using Prometheus;
using Prometheus.Advanced;

namespace PlatformImplementation
{
    public class PlatformMiddleware : IMiddleware
    {
        private IPlatform _platform;
        private IServiceMethod _service;
        private IPlatformLogger _logger;
        private IPlatformMetrics _metrics;

        public PlatformMiddleware(IPlatform platform, IServiceMethod service, IPlatformMetrics metrics)
        {
            _platform = platform;
            _service = service;
            _logger = platform.GetLogger(typeof(PlatformMiddleware));
            _metrics = metrics;        
        }

        private async Task ReturnUnAuthorized(HttpContext context)
        {
            context.Response.StatusCode = 401;

            await context.Response.WriteAsync("UnAuthorized");// Maybe a generic message could be returned?

            return;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path == "/metrics")
            {
                await next(context);

                return;
                
            }

            _metrics.IncrementTotalRequests();
           
            // Platform validate wellformed input            
            using (StreamReader reader = new StreamReader(context.Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                if (!String.IsNullOrWhiteSpace(context.Request.ContentType))
                {
                    bool isWellFormedJson = false;
                    if (!String.IsNullOrWhiteSpace(body))
                    {
                        switch (context.Request.ContentType.ToLower())
                        {
                            case "application/json": //The is no constant for JSON. There is a RFC to have it added
                                isWellFormedJson = _platform.ValidateWellFormedJson(body);
                                break;
                            case MediaTypeNames.Application.Soap:
                            case MediaTypeNames.Text.Xml:
                                //Not yet implemented.
                                break;
                            default:
                                // Unsupport Content-Type
                                break;
                        }

                        if (isWellFormedJson == false)
                        {
                            context.Response.StatusCode = 400;
                            await context.Response.WriteAsync("Bad Request");

                            return;
                        }

                        // Service input value validation
                        //var serviceAuthorized = _service.ValidateRequestSchema(authorizationHeader.Value, body);

                        //if (serviceAuthorized == false)
                        //{
                          //  context.Response.StatusCode = 400; //Something else for invalid schema access
                            //await context.Response.WriteAsync("Invalid Content");

                            //return;
                        //}
                    }
                }
            }

            // Service method invocation - DONE

            // Service does logging - DONE



            await next(context);

            TrackMetrics(context);
        }

        private void TrackMetrics(HttpContext context)
        {
            
            var status = context.Response.StatusCode;

            if (status >= 200 && status < 300)
            {
                _metrics.IncrementTotalSuccessRequests();
            }

            if (status >= 400 && status < 500)
            {
                _metrics.IncrementError400Requests();
            }

            if (status > 500)
            {
                _metrics.IncrementError500Requests();
            }
        }

    }
}
