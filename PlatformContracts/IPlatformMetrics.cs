using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlatformContracts
{
    public interface IPlatformMetrics
    {
        void IncrementTotalRequests();
        void IncrementTotalSuccessRequests();
        void IncrementError400Requests();
        void IncrementError500Requests();

        Task TrackSlaSelf(Func<Task> action);
        Task TrackSlaRemote(string remoteName, Func<Task> action);
    }
}
