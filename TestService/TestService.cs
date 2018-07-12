using Microsoft.AspNetCore.Mvc;
using PlatformPOC.PlatformContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestService
{
    [Produces("application/json")]
    [Route("api/Test")]// Should be in format Subsystem/Service/Version
    public class TestService : Controller, IServiceMethod
    {
        IPlatform _platform;
        IPlatformLogger _logger;

        public TestService(IPlatform platform)
        {
            this._platform = platform;
            _logger = platform.GetLogger(typeof(TestService));
        }

        [HttpGet("/config")]
        public string GetConfig()
        {
            return $"Config retrieved: {_platform.GetConfigurarion("CustomConfigSection:SomeConfiguration")}";
        }

        
        [HttpGet("claims", Name = "GetClaims")]
        public IEnumerable<string> GetClaims()
        {
            // Theere must be a better way
            var authorizationHeader = HttpContext.Request.Headers.FirstOrDefault(c => c.Key.ToLower() == "authorization");
            var claims = _platform.GetTokenClaims(authorizationHeader.Value);

            List<String> claimTypes = new List<string>();

            return claims.Select(c=> c.Type).ToList();
        }

        // GET: api/Test
        [HttpGet]        
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Test/5
        [HttpGet("id/{id}", Name = "GetId")]        
        public string Get(int id)
        {
            if (id == 2)
            {
                _logger.LogInformation("Calling platform for testValue");
                return _platform.GetValueForTesting();
            }
            else
            {
                return "value";
            }

        }

        [HttpGet("model", Name = "GetModel")]
        public JsonResult GetModel()
        {
            var model = new TestModel { Name = "test name", Surname = "Barnard" };
            return new JsonResult(model);
        }

        // POST: api/Test
        [HttpPost]
        public void Post([FromBody]string value)
        {
            _logger.LogInformation("Entered Post");
        }

        // PUT: api/Test/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            _logger.LogInformation("Entered Put");
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogInformation("Entered Delete");
        }

        public bool Authorise(string token)
        {
            if (token.Contains("deny"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateRequestSchema(string token, string request)
        {
            if (!String.IsNullOrWhiteSpace(token) && !String.IsNullOrWhiteSpace(request))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
