using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformPOC.PlatformContracts
{
    public interface IPlatformLogger
    {
        void LogDebug(string message);

        void LogInformation(string message);

        void LogWarning(string message);

        void LogError(string message);

        void LogFatal(string message);
    }
}
