namespace api.Middlewares;

public sealed class FirstMiddleware
{
    private readonly RequestDelegate _next;

    public FirstMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
    }
}

// https://www.codeproject.com/Articles/5337511/Request-Response-Logging-Middleware-ASP-NET-Core
