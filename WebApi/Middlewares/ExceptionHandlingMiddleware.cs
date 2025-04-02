using System.Net;
using System.Text.Json;
using Domain.Shared;

namespace WebApi.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            string jsonResp;
            switch (ex)
            {
                case DataValidationException validationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ValidationErrorModel validationError = new(validationException.Code, validationException.ExMessage, validationException?.Details, validationException?.Fields);
                    jsonResp = JsonSerializer.Serialize(validationError, new JsonSerializerOptions
                    {
                        IncludeFields = true
                    });
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(jsonResp);
                    break;
                case NotFoundException notFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    ErrorModel notFoundError = new(notFoundException.Code, notFoundException.ExMessage);
                    jsonResp = JsonSerializer.Serialize(notFoundError);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(jsonResp);
                    break;
                case ServerException serverException:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    ErrorModel serverError = new(serverException.Code, serverException.ExMessage);
                    jsonResp = JsonSerializer.Serialize(serverError);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(jsonResp);
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    ErrorModel generalError = new("500", "Ocorreu um erro inesperado.");
                    jsonResp = JsonSerializer.Serialize(generalError);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(jsonResp);
                    break;
            }

        }
    }
}

