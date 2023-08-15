namespace ProjetoTransactionApi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await LogError(context, ex, statusCode: 500).ConfigureAwait(true);
            }
        }
        private async Task LogError(HttpContext context, Exception ex, int statusCode)
        {
            _logger.LogError(ex, "<{traceIdentifier} | {message}>", context.TraceIdentifier, ex.Message);

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = @"application/json";

                await context.Response.WriteAsync($"{{\"error\": \"{ex.Message}\"}}").ConfigureAwait(true);
            }
        }
    }
}
