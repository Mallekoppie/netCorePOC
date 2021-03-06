﻿using Microsoft.AspNetCore.Mvc;
using PlatformContracts;
using PlatformPOC.PlatformContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestService
{
    [Produces("application/json")]
    [Route("api/Test")]// Should be in format Subsystem/Service/Version
    public class TestService : Controller, IServiceMethod
    {
        IPlatform _platform;
        IPlatformLogger _logger;
        IPlatformMetrics _metrics;

        public TestService(IPlatform platform)
        {
            _platform = platform;
            _logger = platform.GetLogger(typeof(TestService));
            _metrics = _platform.GetMetrics();
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

        [HttpGet("error")]
        public void CreateErrorToTrackMetrics()
        {
            throw new Exception("Test exception");
        }

        [HttpGet("remote")]
        public async Task<string> CallRemote()
        {
            using (HttpClient client = new HttpClient())
            {
                String textResponse = "";

                await _metrics.TrackSlaRemote("testServer", async () => 
                {
                    var result = await client.GetAsync("http://localhost:10000/returnjsonBody");                    

                    textResponse = await result.Content.ReadAsStringAsync();
                });
                
                return textResponse;
            }
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
