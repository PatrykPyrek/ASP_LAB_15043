using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestContent = new StringBuilder();
        requestContent.AppendLine("\n");
        requestContent.AppendLine("=== Request Info ===");
        requestContent.AppendLine($"Method = {context.Request.Method.ToUpper()}");
        requestContent.AppendLine($"Path = {context.Request.Path}");
        requestContent.AppendLine("-- Headers --");

        foreach (var (headerKey, headerValue) in context.Request.Headers)
        {
            requestContent.AppendLine($"Header = {headerKey}, Value = {headerValue}");
        }

        if (context.Request.ContentLength > 0)
        {
            context.Request.EnableBuffering();
            using var requestReader = new StreamReader(context.Request.Body, leaveOpen: true);
            var content = await requestReader.ReadToEndAsync();
            requestContent.AppendLine($"Body = {content}");
            context.Request.Body.Position = 0; 
        }
        else
        {
            requestContent.AppendLine("Body = <empty>");
        }

        _logger.LogInformation(requestContent.ToString());
        await _next(context);
    }
}
