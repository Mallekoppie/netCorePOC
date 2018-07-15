using App.Metrics;
using App.Metrics.Timer;
using PlatformContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlatformImplementation
{
    public class PlatformMetrics : IPlatformMetrics
    {
        IMetrics _metrics;

        public PlatformMetrics(IMetrics metrics)
        {
            _metrics = metrics;
        }

        public void IncrementError400Requests()
        {
            _metrics.Measure.Meter.Mark(MetricsRegistry.Error400Requests);
        }

        public void IncrementError500Requests()
        {
            _metrics.Measure.Meter.Mark(MetricsRegistry.Error500Requests);
        }

        public void IncrementTotalRequests()
        {
            _metrics.Measure.Counter.Increment(MetricsRegistry.TotalRequests);
        }

        public void IncrementTotalSuccessRequests()
        {
            _metrics.Measure.Meter.Mark(MetricsRegistry.SuccessfullRequests);
        }

        public async Task TrackSlaSelf(Action action)
        {
            using (_metrics.Measure.Timer.Time(MetricsRegistry.ExecutionTimeSelfResource))
            {
                await Task.Run(action);
            }
        }
    }
}
