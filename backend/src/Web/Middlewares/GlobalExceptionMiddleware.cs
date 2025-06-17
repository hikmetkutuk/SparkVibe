using Application.Interfaces;

namespace Web.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception caught in middleware: {ex.Message}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = new
        {
            context.Response.StatusCode,
            exception.Message,
            exception.StackTrace
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}