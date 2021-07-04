using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using Domain.Utils;
using System;

namespace Domain.Services.Telemetry
{
    public interface ITelemetryService
    {
        void IncrementCounter(MetricNameSpace metricNamespace, string counterName);
        void PutValue(MetricNameSpace metricNamespace, string counterName, double value);
        void PutTimeMs(MetricNameSpace metricNamespace, string counterName, double timeMs);

        void IncrementDeprecatedFunctionality(string eventName);
    }

    public class TelemetryService : ITelemetryService
    {
        private readonly string _environmentName = ConfigurationManager.EnvironmentName;

        public void PutTimeMs(MetricNameSpace metricNamespace, string counterName, double timeMs)
        {
            var metric = new MetricDatum
            {
                MetricName = counterName,
                Unit = StandardUnit.Milliseconds,
                Value = timeMs,
                TimestampUtc = DateTime.UtcNow
            };

            GlobalTelemetryQueue.GetInstance().EnqueueDatum(MetricNames.LibraryNamespace(_environmentName, metricNamespace), metric);

        }

        public void IncrementDeprecatedFunctionality(string eventName)
        {
            PutValue(MetricNameSpace.DeprecatedFunctionalityUsage, eventName, 1);
        }


        public void IncrementCounter(MetricNameSpace metricNamespace, string counterName)
        {
            PutValue(metricNamespace, counterName, 1);
        }

        public void PutValue(MetricNameSpace metricNamespace, string eventName, double value)
        {
            var metric = new MetricDatum
            {
                MetricName = eventName,
                Unit = StandardUnit.None,
                Value = value,
                TimestampUtc = DateTime.UtcNow
            };

            GlobalTelemetryQueue.GetInstance().EnqueueDatum(MetricNames.LibraryNamespace(_environmentName, metricNamespace), metric);
        }
    }
}
