namespace api.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var originalResponseBodyStream = httpContext.Response.Body;

        using var responseBody = new MemoryStream();
        httpContext.Response.Body = responseBody;

        try
        {
            await _next(httpContext);
        }
        finally
        {
            httpContext.Response.Body = originalResponseBodyStream;

            if (!httpContext.Items.ContainsKey("ExceptionOccurred"))
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(responseBody).ReadToEndAsync();
                _logger.LogInformation("Response Body: {ResponseBody}", text);
            }
                
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalResponseBodyStream);
        }
    }
}
