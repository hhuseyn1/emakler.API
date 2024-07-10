using BusinessLayer.Exceptions;
using DataAccessLayer.Exceptions;
using EntityLayer.Entities;
using System.Net;

namespace EMakler.PROAPI.Middlewares; 

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;
        var errorResponse = new ErrorResponse
        {
            Message = "An unexpected error occurred.",
            Detail = exception.Message
        };

        switch (exception)
        {
            case EntityNotFoundException entityNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = entityNotFoundException.Message;
                break;
            case DatabaseConnectionException databaseConnectionException:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = databaseConnectionException.Message;
                break;
            case BusinessRuleException businessRuleException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = businessRuleException.Message;
                break;
            case ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = validationException.Message;
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        _logger.LogError(exception, $"Error occurred: {errorResponse.Message}");
        return context.Response.WriteAsJsonAsync(errorResponse);
    }
}

