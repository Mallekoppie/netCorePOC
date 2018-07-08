using System;

namespace PlatformPOC.PlatformContracts
{
    public interface IServiceMethod
    {
        bool Authorise(String token);
        // The 'Object' type must be changed to a generic base HTTP request class
        bool ValidateRequestSchema(String token, String request);        
    }
}
