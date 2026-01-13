namespace TodoApi.Middleware
{
  public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Try to process the request
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); // If it fails, handle it here
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        var response = new 
        {
            StatusCode = 500,
            Message = "Internal Server Error. Please try again later.",
            Detailed = exception.Message // Remove this line in Production!
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}
  
}