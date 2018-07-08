using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PlatformPOC.PlatformContracts;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace PlatformPOC.PlatformImplementation
{
    public class Platform : IPlatform
    {
        IPlatformLogger _logger;
        Logger doNotUseSerilogger; // Do not use directly

        public Platform(Logger logger)
        {
            doNotUseSerilogger = logger;
            _logger = GetLogger(typeof(Platform));
        }

        public bool ConfigureAuthorization(string tennantId, List<string> groups)
        {
            throw new NotImplementedException();
        }

        public IPlatformLogger GetLogger(Type type)
        {
            return PlatformLogger.CreateLogger(type, doNotUseSerilogger);
        }

        public IEnumerable<System.Security.Claims.Claim> GetTokenClaims(string token)
        {
            if (token.Contains(' '))
            {
                token = token.Substring(token.IndexOf(' ') + 1);
            }

            var jwtToken = new JwtSecurityToken(token);

            return jwtToken.Claims;
        }

        public string GetValueForTesting()
        {
            return "Test value returned!";
        }

        static string _issuer = string.Empty;
        static List<SecurityKey> _signingTokens = null;
        DateTime _stsMetadataRetrievalTime = DateTime.MinValue;
        // TODO: Our tenant must be read from config
        static string _tenantId = "94742033-57a8-49b3-81aa-e72832a0aee7";
        static string _stsDiscoveryEndpoint = $"https://login.microsoftonline.com/{_tenantId}/v2.0/.well-known/openid-configuration";// TODO: Must be retrieved from config and our tenant?
        ConfigurationManager<OpenIdConnectConfiguration> _configManager = new ConfigurationManager<OpenIdConnectConfiguration>(_stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());
        TokenValidationParameters validationParameters = null;
        private bool initialised = false;

        private TokenValidationParameters GetConfig()
        {
            if (initialised == false)
            {
                // TODO: periodically update the signing keys
                // First call of the service will be slow
                OpenIdConnectConfiguration config = _configManager.GetConfigurationAsync().Result; // TODO: This is blocking. It is bad and it should feel bad
                                                                                                   //config.JwksUri might need to be updated to get through the firewall
                _issuer = config.Issuer;

                _signingTokens = config.SigningKeys.ToList();
                _stsMetadataRetrievalTime = DateTime.UtcNow;

                validationParameters = new TokenValidationParameters
                {
                    ValidAudience = "https://graph.microsoft.com", // TODO: Must be read from config
                    ValidIssuer = "https://sts.windows.net/94742033-57a8-49b3-81aa-e72832a0aee7/", // TODO: Must read from config
                    IssuerSigningKeys = _signingTokens,
                    ValidateAudience = false, // These are development settings
                    ValidateLifetime = false,
                    ValidateActor = false,
                    ValidateIssuer = false,
                    ValidateTokenReplay = false,
                    ValidateIssuerSigningKey = false
                };
            }

            return validationParameters;
        }

        public bool ValidateOAuth2Token(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                // cut out the bearer part
                if (token.Contains(" "))
                {
                    var split = token.Split(' ');
                    token = split[1];
                }

                if (!handler.CanReadToken(token))
                {
                    return false;
                }

                SecurityToken outToken = null;
                var claimsPrinciple = handler.ValidateToken(token, GetConfig(), out outToken);

                JwtSecurityToken jwtToken = outToken as JwtSecurityToken;

                if (jwtToken == null)
                {
                    // TODO: Add Logging
                    return false;
                }

                // TODO: Get correct tenant from config
                var correctTenant = jwtToken.Claims.Any(c => c.Type == "tid" && c.Value == "94742033-57a8-49b3-81aa-e72832a0aee7");

                if (!correctTenant)
                {
                    return false;
                }

                return true;

            } //TODO: specific catch exceptions for expiry and so on 
            catch (Exception ex)
            {
                // TODO: Log exception details
                // Unknown exception occurred

                return false;
            }
        }

        public bool ValidateWellFormedJson(string body)
        {
            try
            {
                var result = JsonConvert.DeserializeObject(body);

                if (result == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (JsonException ex)
            {
                // TODO: Add logging
                return false;
            }
            catch (Exception ex)
            {
                // TODO: Add logging
                return false;
            }
        }

        public bool ValidateWellFormedXml(string body)
        {
            throw new NotImplementedException();
        }
    }
}
