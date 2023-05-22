using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace ImpactChallenge.WebApi.Filters
{
    public class ExecutionTimeFilter : IActionFilter
    {
        private Stopwatch _stopwatch;
        private readonly ILogger<ExecutionTimeFilter> _logger;

        public ExecutionTimeFilter(ILogger<ExecutionTimeFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller.GetType().Name;
            var action = context.ActionDescriptor.DisplayName;
            var traceId = Guid.NewGuid().ToString();
            context.HttpContext.Items["TraceId"] = traceId;
            _logger.LogTrace("Trace ID: {TraceId} | Execution started for {Controller}.{Action}", controller, action, traceId);
            _stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();
            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            var controller = context.Controller.GetType().Name;
            var action = context.ActionDescriptor.DisplayName;
            var traceId = context.HttpContext.Items["TraceId"] as string;
            _logger.LogInformation("Trace ID: {TraceId} | Execution time for {Controller}.{Action}: {ElapsedMilliseconds} ms", controller, action, elapsedMilliseconds, traceId);
        }
    }
}
