using Service.Ultis;

namespace InteractiveFloor.Middlewares
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred");

                context.Response.ContentType = "application/json";

                var statusCode = ex is CustomException customEx ? customEx.StatusCode : StatusCodes.Status500InternalServerError;
                context.Response.StatusCode = statusCode;

                var response = new
                {
                    StatusCode = statusCode,
                    ex.Message,
                    Detailed = ex.ToString()
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
