﻿using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Meter;
using App.Metrics.Timer;
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

        public static TimerOptions ExecutionTimeSelfResource => new TimerOptions
        {
            Name = "SLA Self Method Execution ms",
            DurationUnit = TimeUnit.Milliseconds,
            MeasurementUnit = Unit.Requests,
            RateUnit = TimeUnit.Milliseconds
        };

        public static TimerOptions ExecutionTimeRemote => new TimerOptions
        {
            Name = "SLA Remote Method Execution ms: ",
            DurationUnit = TimeUnit.Milliseconds,
            MeasurementUnit = Unit.Requests,
            RateUnit = TimeUnit.Milliseconds
        };
    }
}
