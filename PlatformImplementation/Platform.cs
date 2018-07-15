using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PlatformContracts;
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
        IConfiguration _configuration;
        IPlatformMetrics _metrics;

        public Platform(Logger logger, IConfiguration configuration, IPlatformMetrics metrics)
        {
            doNotUseSerilogger = logger;
            _logger = GetLogger(typeof(Platform));
            _configuration = configuration;
            _metrics = metrics;
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

        public string GetConfigurarion(string key)
        {
            return _configuration[key];
        }

        public IPlatformMetrics GetMetrics()
        {
            return _metrics;
        }
    }
}
