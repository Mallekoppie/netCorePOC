using System;
using System.Collections.Generic;

namespace PlatformPOC.PlatformContracts
{
    public interface IPlatform
    {
        //Get the logger for the platform you are running on.
        //I just used a microsoft contract. We will ultimately implement our own contract
        IPlatformLogger GetLogger(Type type);

        // Configure which tennant is allowed to issue tokens and which groups services must be 
        // a member of to get global access to data
        bool ConfigureAuthorization(String tennantId, List<String> groups);

        // Get the claims from the supplied token
        IEnumerable<System.Security.Claims.Claim> GetTokenClaims(String token);

        String GetValueForTesting();

        bool ValidateWellFormedJson(String body);

        bool ValidateWellFormedXml(String body);

        string GetConfigurarion(string key);
    }
}
