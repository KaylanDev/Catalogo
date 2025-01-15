using Catalogo.Migrations;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Catalogo.Models.Extensions
{
    public static class ApiExceptionMiddlewareExtensions
    {
        public static void ConfigureExeptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
               {
                   context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                   context.Response.ContentType = "application/json";
                   var contextFeature = context.Features.Get<ExceptionHandlerFeature>();
                   if (contextFeature != null)
                   {
                       await context.Response.WriteAsync(new ErrorDetals()
                       {
                           StatusCode = context.Response.StatusCode,
                           Message = contextFeature.Error.Message,
                           Trace = contextFeature.Error.StackTrace
                       }.ToString());
                   }
                });

            });
        }
    }
}
