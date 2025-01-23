using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalogo.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
      private  readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> filter)
        {
            _logger = filter;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception,"Ocorreu um erro interno. Status code:500");
            context.Result = new ObjectResult("Ocorreu um erro ao tratar sua solicitaçao.")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
