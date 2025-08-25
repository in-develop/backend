using Microsoft.AspNetCore.Mvc.Filters;

namespace TeamChallenge.Filters
{
    public class ValidationFilter : IActionFilter
    {
        private readonly ILogger<ValidationFilter> _logger;

        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // This is called after the action has executed; no logging needed here for this purpose.
        }
    }
}
