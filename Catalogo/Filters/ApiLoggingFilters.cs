using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalogo.Filters
{
    public class ApiLoggingFilters : IActionFilter
    {

        readonly ILogger<ApiLoggingFilters> _logger;

        public ApiLoggingFilters(ILogger<ApiLoggingFilters> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //executa dps da action

            _logger.LogInformation("#########################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"{context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("#########################################");

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //executa antes da action
            _logger.LogInformation("#########################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"{context.ModelState.IsValid}");
            _logger.LogInformation("#########################################");
        }
    }
}
