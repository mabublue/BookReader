using Amazon;
using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using Domain.Services.Telemetry;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Api.HostedServices
{
    public class TelemetryPushService : BackgroundService
    {
        private readonly ILogger<TelemetryPushService> _logger;
        private readonly RegionEndpoint _region = RegionEndpoint.APSoutheast2;

        public TelemetryPushService(ILogger<TelemetryPushService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting {nameof(TelemetryPushService)}");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var kvp = GlobalTelemetryQueue.GetInstance().DequeueDatum();

                    if (kvp.Key == null || kvp.Value == null) //No work to do, goto sleep
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                        continue;
                    }

                    using var client = new AmazonCloudWatchClient(_region);
                    var request = new PutMetricDataRequest
                    {
                        MetricData = new List<MetricDatum>
                                                   {
                                                       kvp.Value
                                                   },
                        Namespace = kvp.Key
                    };
                    await client.PutMetricDataAsync(request, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed logging CloudWatch metric");
                }
            }
        }
    }
}
