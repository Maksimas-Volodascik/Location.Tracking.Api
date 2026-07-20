using Newtonsoft.Json;
using System.Net;

namespace Location.Tracking.Api.Middleware
{
    public class LoggingMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public LoggingMiddleware(ILogger<LoggingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;

            _logger.LogInformation("Logging initialized");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex}");
                HandleException(ex, context);
            }
            finally
            {
                await _next(context);
            }
        }

        private Task HandleException(Exception ex, HttpContext context)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
