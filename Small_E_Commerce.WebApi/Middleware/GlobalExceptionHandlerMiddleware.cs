using System.Net;
using Microsoft.EntityFrameworkCore;
using Small_E_Commerce.Products;

namespace Small_E_Commerce.WebApi.Middleware;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (DomainException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (DbUpdateException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.InnerException?.Message });
            logger.LogInformation("DbUpdateException: {message}", ex.ToString());
        }

        catch (IllegalStateTransitionException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.InnerException?.Message });
            logger.LogInformation("IllegalStateTransitionException: {message}", ex.ToString());
        }
        

        catch (Exception ex)
        {
            httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });
            logger.LogError("Exception: {message}", ex.ToString());
        }
    }
}