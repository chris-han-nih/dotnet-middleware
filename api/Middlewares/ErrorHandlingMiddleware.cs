using System.Net;
using System.Text.Json;

namespace api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            httpContext.Items["ExceptionOccurred"] = true;
            await HandleExceptionAsync(httpContext);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            code = context.Response.StatusCode,
            message = "Internal Server Error",
        };

        var responseStr = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(responseStr);
    }
}
