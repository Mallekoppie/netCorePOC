using App.Metrics;
using App.Metrics.Counter;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformImplementation
{
    public static class MetricsRegistry
    {
        public static CounterOptions SuccessfullCalls => new CounterOptions
        {
            Name = "Success Requests",
            MeasurementUnit = Unit.Calls
        };
    }
}
