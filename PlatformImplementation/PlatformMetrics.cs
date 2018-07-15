using App.Metrics;
using PlatformContracts;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
