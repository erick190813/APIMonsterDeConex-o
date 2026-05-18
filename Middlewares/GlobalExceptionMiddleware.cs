using System.Net;
using System.Text.Json;

namespace PokeApiBackend.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[FALHA INTERNA - POKEAPI]: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorPayload = new
            {
                StatusCode = context.Response.StatusCode,
                Timestamp = DateTime.UtcNow,
                Message = "Falha no barramento interno do PokeApi Backend. A requisição falhou.",
                SystemErrorCode = "POKEAPI_CORE_FAIL",
                TechnicalDetails = exception.Message
            };

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(errorPayload, jsonOptions));
        }
    }
}