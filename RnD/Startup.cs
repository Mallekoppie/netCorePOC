using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RnD
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
            services.AddMvc();

            /*services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                //options.DefaultSignInScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })*/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            /*  .AddOpenIdConnect(options =>
              {
                  //options.Authority = "http://localhost:8180/auth/realms/SpringBootKeycloak";
                  options.ClientId = "netcore";
                  //options.ClientSecret = "e0c09ec8-0d8d-448b-b2d4-690de421803c";
                  options.RequireHttpsMetadata = false; // For local development
                  //options.MetadataAddress = "http://localhost:8180/auth/realms/SpringBootKeycloak/.well-known/openid-configuration";
                  //options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

              })*/
             .AddJwtBearer(options =>
             {
                 //options.Authority = "http://localhost:8180/auth/realms/SpringBootKeycloak";
                 options.Audience = "netcore";
                 //options.Challenge = "WWW-Authenticate";
                 options.RequireHttpsMetadata = false; // For local development
                 //options.MetadataAddress = "http://localhost:8180/auth/realms/SpringBootKeycloak/.well-known/openid-configuration";
                 //options.SaveToken = true;
                 //options.RefreshOnIssuerKeyNotFound = true;                
                 options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>("http://localhost:8180/auth/realms/SpringBootKeycloak/.well-known/openid-configuration",
                     new OpenIdConnectConfigurationRetriever(), new HttpDocumentRetriever() { RequireHttps = false });
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = false,
                     ValidateIssuerSigningKey = false,
                     ValidIssuer = "http://localhost:8180/auth/realms/SpringBootKeycloak",
                     ValidAudience = "netcore",
                     RequireSignedTokens = false,
                 };

                 options.IncludeErrorDetails = true;

             });
            /*.AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = false,
                     ValidateIssuerSigningKey = false,
                     ValidIssuer = "http://localhost:8180/auth/realms/SpringBootKeycloak",
                     ValidAudience = "netcore"
                 };
             });*/

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseAuthentication();
        }
    }
}
