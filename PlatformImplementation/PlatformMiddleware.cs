using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        public PlatformMiddleware(IPlatform platform, IServiceMethod service)
        {
            _platform = platform;
            _service = service;
            _logger = platform.GetLogger(typeof(PlatformMiddleware));
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
            /*
            // Platform Validate Token
            var authorizationHeader = context.Request.Headers.FirstOrDefault(c => c.Key.ToLower() == "authorization");

            //If there is no header then reject the call
            if (authorizationHeader.Key == null)
            {
                _logger.LogError($"{context.Request.Path} does not have a Authorization header");
                // TODO: Add specific logging
                await ReturnUnAuthorized(context);

                return;
            }

            // If the token is not valid reject the call
            if (_platform.ValidateOAuth2Token(authorizationHeader.Value) == false)
            {
                // TODO: Add specific logging
                _logger.LogError("Token validation failed");
                await ReturnUnAuthorized(context);

                return;
            }

            // Service authorise
            if (_service.Authorise(authorizationHeader.Value) == false)
            {
                await ReturnUnAuthorized(context);

                return;
            }
            */
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
        }
    }
}
