using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformContracts
{
    public interface IPlatformMetrics
    {
        void IncrementTotalRequests();
        void IncrementTotalSuccessRequests();
        void IncrementError400Requests();
        void IncrementError500Requests();
    }
}
