using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Meter;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformImplementation
{
    public static class MetricsRegistry
    {
        public static CounterOptions TotalRequests => new CounterOptions
        {
            Name = "Total Requests",
            MeasurementUnit = Unit.Calls
        };

        public static MeterOptions SuccessfullRequests => new MeterOptions
        {
            Name = "Successfull Requests",
            MeasurementUnit = Unit.Calls
        };

        public static MeterOptions Error400Requests => new MeterOptions
        {
            Name = "Error 400 Requests",
            MeasurementUnit = Unit.Calls
        };

        public static MeterOptions Error500Requests => new MeterOptions
        {
            Name = "Error 500 Requests",
            MeasurementUnit = Unit.Calls
        };
    }
}
