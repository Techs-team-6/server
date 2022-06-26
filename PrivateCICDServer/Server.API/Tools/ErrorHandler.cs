using System.Net;
using System.Text.Json;
using Domain.Tools;

namespace Server.API.Tools;

public class ErrorHandler
{
    private readonly RequestDelegate _next;

    public ErrorHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch
            {
                AuthException e => (int) HttpStatusCode.Unauthorized,
                EntityNotFoundException e => (int) HttpStatusCode.NotFound,
                InvalidTokenException e => (int) HttpStatusCode.BadRequest,
                ServiceException e => (int) HttpStatusCode.BadRequest,
                _ => (int) HttpStatusCode.InternalServerError
            };
            
          await response.WriteAsync( JsonSerializer.Serialize(new ErrorDetails(error?.Message ?? "Error has no description")));
        }
    }
}