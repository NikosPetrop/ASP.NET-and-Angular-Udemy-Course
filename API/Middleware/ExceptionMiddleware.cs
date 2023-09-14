using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware
{
    /// <summary> This needs to be called in order to go to next (if any) middleware. </summary>
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    /// <summary> Check if we run in development or production environment </summary>
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    /// <summary> This has to be called InvokeAsync to be called from framework, as middleware. </summary>
    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            // Outputs error into console/terminal
            _logger.LogError(ex, ex.Message);

            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // In production env, we want to "hide" stacktrace
            var response = _env.IsDevelopment()
                ? new APIException(ctx.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new APIException(ctx.Response.StatusCode, ex.Message, "Internal Server Error");

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await ctx.Response.WriteAsync(json);
        }
    }
}
