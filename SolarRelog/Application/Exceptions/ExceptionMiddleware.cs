﻿namespace SolarRelog.Application.Exceptions;

public class ExceptionMiddleware
{    
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }   
       
    public async Task InvokeAsync(HttpContext httpContext, ILogger logger)
    {
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex, logger);
        }
    }
                     
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        object? jsonResult;

        switch (exception)
        {
            case AppException:
                logger.LogError(exception, exception.Message);
                response.StatusCode = StatusCodes.Status400BadRequest;
                jsonResult = new { Error = exception.Message };
                break;
            case EntityNotFoundException:
                logger.LogError(exception, exception.Message);
                response.StatusCode = StatusCodes.Status404NotFound;
                jsonResult = new { Error = exception.Message };
                break;
            default:
                logger.LogCritical(exception, exception.Message);
                response.StatusCode = StatusCodes.Status500InternalServerError;
                jsonResult = new { Error = exception.Message };
                break;
        }
        
        await response.WriteAsJsonAsync(jsonResult);
    }
}    
