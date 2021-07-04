using Domain.Extensions;
using Domain.Handlers.Commands;
using Domain.Handlers.Queries;
using Domain.Services.Telemetry;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog.Core.Enrichers;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class CommandTelemetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        private readonly ITelemetryService _telemetryService;

        public CommandTelemetryBehavior(ILogger<CommandTelemetryBehavior<TRequest, TResponse>> logger, ITelemetryService telemetryService)
        {
            _logger = logger;
            _telemetryService = telemetryService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                var response = await next();
                sw.Stop();
                using (CreateLogContext(request, sw, response))
                {
                    if (IsQuery(request))
                    {
                        _logger.Log(LogLevel.Information, $"QUERY Executed in {sw.ElapsedMilliseconds} milliseconds: {request.GetType().Name}");
                        _telemetryService.IncrementCounter(MetricNameSpace.QueryExecution, CounterNames.QueryExecution.Success);
                        _telemetryService.PutTimeMs(MetricNameSpace.QueryExecution, CounterNames.QueryExecution.ExecutionTime, sw.ElapsedMilliseconds);
                    }
                    else
                    {
                        _logger.Log(LogLevel.Information, $"COMMAND Executed in {sw.ElapsedMilliseconds} milliseconds: {request.GetType().Name}");
                        _telemetryService.IncrementCounter(MetricNameSpace.CommandExecution, CounterNames.CommandExecution.Success);
                        _telemetryService.PutTimeMs(MetricNameSpace.CommandExecution, CounterNames.CommandExecution.ExecutionTime, sw.ElapsedMilliseconds);
                    }
                }

                using (CreateLogContext(request, sw, response))
                {
                    _logger.LogInformation("Handled {Command}", typeof(TRequest).FullName);
                }

                return response;
            }
            catch (Exception ex)
            {
                sw.Stop();

                using (CreateLogContextForException(request, sw))
                {
                    if (IsQuery(request))
                    {
                        _logger.Log(LogLevel.Information, $"QUERY {request.GetType().Name} Error: {ex.Message}");
                        _telemetryService.IncrementCounter(MetricNameSpace.QueryExecution, CounterNames.QueryExecution.Exception);
                    }
                    else
                    {
                        _logger.Log(LogLevel.Information, $"COMMAND {request.GetType().Name} Error: {ex.Message}");
                        _telemetryService.IncrementCounter(MetricNameSpace.CommandExecution, CounterNames.CommandExecution.Exception);
                    }
                }

                using (CreateLogContextForException(request, sw))
                {
                    _logger.LogError(ex, "{Message}", ex.Message);
                }

                throw;
            }
        }

        private bool IsQuery(TRequest request)
        {
            return request.GetType().HasImplementedRawGeneric(typeof(IQuery<>));
        }

        private static IDisposable CreateLogContext(TRequest request, Stopwatch sw, TResponse response)
        {
            var outcome = "Unknown";
            var commandResponse = response as CommandResponse;
            if (commandResponse != null)
                outcome = commandResponse.IsSuccess ? "Success" : "Failure";

            return LogContext.Push(new PropertyEnricher("ExecutionTimeMs", sw.ElapsedMilliseconds),
                                   new PropertyEnricher("Command", request),
                                   new PropertyEnricher("BreadcrumbCorrelationId", Guid.NewGuid().ToString()),
                                   new PropertyEnricher("CommandShortName", request.GetType().Name),
                                   new PropertyEnricher("Success", (response as CommandResponse)?.IsSuccess),
                                   new PropertyEnricher("ValidationErrors", (response as CommandResponse)?.ValidationErrors),
                                   new PropertyEnricher("Outcome", outcome)
                                  );
        }

        private static IDisposable CreateLogContextForException(TRequest request, Stopwatch sw)
        {
            return LogContext.Push(new PropertyEnricher("ExecutionTimeMs", sw.ElapsedMilliseconds),
                                   new PropertyEnricher("Command", request),
                                   new PropertyEnricher("Outcome", "Exception")
                                  );
        }
    }
}
