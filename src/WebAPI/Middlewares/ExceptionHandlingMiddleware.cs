using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Space.WebAPI.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private IUnitOfWork? _unitOfWork;
    private ICurrentUserService? _currentUserService;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        try
        {

            await _next(httpContext);
        }
        catch (DateTimeException ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)ex.HttpStatusCode;

            TimeErrorResponse timeError = new TimeErrorResponse()
            {
                Message = ex.Message,
                Time = ex.Time,
            };
            string json = JsonConvert.SerializeObject(timeError, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            await httpContext.Response.WriteAsync(json);
            unitOfWork.TelegramService.SendMessage(json);

        }
        catch (AlreadyExistsException ex)
        {
            ErrorResponse error = await HandleExceptionAsync(httpContext, ex, ex.HttpStatusCode);
        }
        catch (NotFoundException ex)
        {
            ErrorResponse error = await HandleExceptionAsync(httpContext, ex, ex.HttpStatusCode);
        }
        catch (UnauthorizedAccessException ex)
        {
            ErrorResponse error = await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Unauthorized);
        }
        catch (ValidationException ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            ValidationErrorResponse response = new()
            {
                Title = "Validation error",
                Errors = ex.Errors
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToArray())
            };
            string json = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            await httpContext.Response.WriteAsync(json);
        }
        catch (SqlException ex)
        {
            ErrorResponse error = await HandleExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            ErrorResponse error = await HandleExceptionAsync(httpContext, ex);
            _logger.LogError(ex, $"Request {httpContext.Request?.Method}: {httpContext.Request?.Path.Value} failed Error: {@error}", error);
        }
    }
    private async Task<ErrorResponse> HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? message = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        ErrorResponse response = new()
        {
            Title = message ?? exception.Message,
        };
        string json = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
        await context.Response.WriteAsync(json);
        string? email = _currentUserService.Email;

        string sendMessage =
            $"StatusCode: {context.Response.StatusCode},\n\n " +
            $"Message: '{response.Title}',\n\n " +
            $"Login User: {email}, \n\n" +
            $"Endpoint {context.Request.Method}: {context.Request.Path} \n\n";
        _unitOfWork!.TelegramService.SendMessage(sendMessage);
        return response;
    }
}
public static class ExceptionHandlerMiddelwareExtensions
{

    public static IApplicationBuilder UseExceptionMiddelware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}