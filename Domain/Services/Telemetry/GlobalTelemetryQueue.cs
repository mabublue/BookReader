using Amazon.CloudWatch.Model;
using Domain.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Domain.Services.Telemetry
{
    public class GlobalTelemetryQueue
    {
        private const int MaxItems = 10000;
        private readonly ConcurrentQueue<KeyValuePair<string, MetricDatum>> _items;

        private GlobalTelemetryQueue()
        {
            _items = new ConcurrentQueue<KeyValuePair<string, MetricDatum>>();
        }

        private static GlobalTelemetryQueue _instance;
        public static GlobalTelemetryQueue GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GlobalTelemetryQueue();
            }
            return _instance;
        }

        public void EnqueueDatum(string metricNameSpace, MetricDatum datum)
        {
            Throw.IfNullOrEmpty(metricNameSpace, nameof(metricNameSpace));
            Throw.IfNull(datum, nameof(datum));

            _items.Enqueue(new KeyValuePair<string, MetricDatum>(metricNameSpace, datum));

            while (_items.Count > MaxItems)
            {
                // ReSharper disable once UnusedVariable
                _items.TryDequeue(out var outObj);
            }
        }

        public KeyValuePair<string, MetricDatum> DequeueDatum()
        {
            _items.TryDequeue(out var item);
            return item;
        }
    }
}
