using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

//Not needed anymore
namespace ImpactChallenge.WebApi.Filters
{
    public class ExecutionTimeFilter : Attribute, IActionFilter
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
            var dateTimeUtcNow = DateTime.UtcNow;
            context.HttpContext.Items["TraceId"] = traceId;
            context.HttpContext.Items["DateTimeUtcNow"] = dateTimeUtcNow;
            
            _logger.LogInformation("Trace ID: {traceId} | DateTime Utc: {dateTimeUtcNow} | Execution started for {controller}.{action}", traceId, dateTimeUtcNow, controller, action);
            _stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();
            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            var controller = context.Controller.GetType().Name;
            var action = context.ActionDescriptor.DisplayName;
            var traceId = context.HttpContext.Items["TraceId"] as string;
            var dateTimeUtcNow = context.HttpContext.Items["DateTimeUtcNow"] as DateTime?;
            _logger.LogInformation("Trace ID: {traceId} | DateTime Utc: {dateTimeUtcNow} | Execution time for {controller}.{action}: {elapsedMilliseconds} ms", traceId, dateTimeUtcNow, controller, action, elapsedMilliseconds);
        }
    }
}
