using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Api
{
    internal class ErrorHandlingMiddleware
    {
        private readonly ILogger _log = new Logger(typeof(ErrorHandlingMiddleware));
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _log.Error("Error thrown in request.", ex);
            var code = HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.StatusCode = (int)code;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }
    }
}