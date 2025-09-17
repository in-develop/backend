using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TeamChallenge.Models.Responses;

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
            if (context.ActionArguments.TryGetValue("id", out var id) &&
                ((id is int idInt &&
                idInt <= 0) ||
                (id is string idStr &&
                string.IsNullOrEmpty(idStr))))
            {
                _logger.LogWarning("Invalid ID: {Id}", id);
                context.Result = new BadRequestObjectResult(
                    new BadRequestResponse("Invalid ID"));
                
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult result)
            {
                if(result.Value is IResponse response)
                {
                    result.StatusCode = response.StatusCode;
                }
            }
        }
    }
}
