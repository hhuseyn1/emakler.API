using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Text.Json;

namespace BusinessLayer.Exception;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
        System.Exception exception, 
        CancellationToken cancellationToken)
    {
        Log.Error(exception, exception.Message);

        var details = new ProblemDetails()
        {
            Detail = "API Error",
            Instance = "API",
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "Error",
            Type = "Server Error"
        };

        var response = JsonSerializer.Serialize(details);
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsync(response, cancellationToken);

        return true;
    }
}
