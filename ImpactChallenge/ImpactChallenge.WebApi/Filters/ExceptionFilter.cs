using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

//Not needed anymore
namespace ImpactChallenge.WebApi.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            //TODO: take care of the different kind of exceptions that arise

            if (context.Exception is ArgumentException)
            {
                // Handle ArgumentException
                _logger.LogError("Argument exception occurred", context.Exception);
                context.Result = new BadRequestResult();
            }
            else
            {
                // Handle other exceptions
                _logger.LogError("Unhandled exception occurred", context.Exception);
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }



            // Prevent the exception from being re-thrown
            context.ExceptionHandled = true;
        }
    }
}
