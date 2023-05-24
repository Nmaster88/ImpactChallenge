using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace ImpactChallenge.WebApi.Middleware
{

public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var elapsedMilliseconds = Stopwatch.StartNew();
            var endpoint = context.GetEndpoint(); 
            var traceId = Guid.NewGuid().ToString();
            var dateTimeUtcNow = DateTime.UtcNow;

            try
            {
                // Log the start of the request
                _logger.LogInformation("Trace ID: {traceId} | DateTime Utc: {dateTimeUtcNow} | Execution started for endpoint: {endpoint}", traceId, dateTimeUtcNow, endpoint);

                await _next(context);

                elapsedMilliseconds.Stop();
                // Log the completion of the request
                _logger.LogInformation("Trace ID: {traceId} | DateTime Utc: {dateTimeUtcNow} | Execution time for endpoint {endpoint} | elapsedMilliseconds: {elapsedMilliseconds} ms", traceId, dateTimeUtcNow, endpoint,elapsedMilliseconds);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occurred
                _logger.LogError("Trace ID: {traceId} | An error occurred: {ex.Message}", traceId, ex.Message);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }

}
