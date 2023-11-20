using Azure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Space.WebAPI.Middlewares;

/// <summary>
/// Exception handling middleware
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private IUnitOfWork? _unitOfWork;
    private ICurrentUserService? _currentUserService;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate representing the next middleware in the pipeline.</param>
    /// <param name="logger">The logger used for logging exceptions and messages.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// This middleware handles exceptions in your ASP.NET Core application and provides appropriate HTTP responses.
    /// </summary>
    /// <param name="httpContext">The context information for the HTTP request.</param>
    /// <param name="unitOfWork">The unit of work service used for performing database operations.</param>
    /// <param name="currentUserService">The current user service containing user information.</param>
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

            TimeErrorResponse response = new()
            {
                Message = ex.Message,
                Time = ex.Time,
            };
            string json = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            Serilog.Log.Error(ex, "Request {RequestMethod}: {RequestPath} failed Error: {ResponseTitle}, Ip Address: {IpAdress}, Login user {UserName}",
                                   httpContext.Request?.Method, httpContext.Request?.Path.Value, response.Message, httpContext.Connection.LocalIpAddress,
                                   httpContext.User?.Identity?.IsAuthenticated != null || true ? httpContext.User?.Identity?.Name : null);

            string? email = _currentUserService.Email;
            string sendMessage =
                $"StatusCode: {httpContext.Response.StatusCode},\n\n " +
                $"Message: '{response.Message}',\n\n " +
                $"Login User: {email}, \n\n" +
                $"Endpoint {httpContext.Request?.Method}: {httpContext.Request?.Path} \n\n";
            _unitOfWork!.TelegramService.SendMessage(sendMessage);



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
        catch (InvalidCredentialsException ex)
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

            string? email = _currentUserService.Email;

            Serilog.Log.Error(ex, "Request {RequestMethod}: {RequestPath} failed Error: {ResponseTitle}, Ip Address: {IpAdress}, Login user {UserName}", httpContext.Request?.Method, httpContext.Request?.Path.Value, response.Title, httpContext.Connection.RemoteIpAddress, httpContext.User?.Identity?.IsAuthenticated != null || true ? httpContext.User?.Identity?.Name : null);

            string sendMessage =
                $"StatusCode: {httpContext.Response.StatusCode},\n\n " +
                $"Message: '{response.Title}',\n\n " +
                $"Login User: {email}, \n\n" +
                $"Endpoint {httpContext.Request?.Method}: {httpContext.Request?.Path} \n\n";

            _unitOfWork!.TelegramService.SendMessage(sendMessage);

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

    /// <summary>
    /// Handles exceptions that occur during request processing and sends an error response.
    /// </summary>
    /// <param name="httpContext">The current <see cref="HttpContext"/>.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="statusCode">The HTTP status code to be set in the response (default is InternalServerError).</param>
    /// <param name="message">An optional custom error message to include in the response (default is exception message).</param>
    /// <returns>An <see cref="ErrorResponse"/> containing error details.</returns>
    private async Task<ErrorResponse> HandleExceptionAsync(HttpContext httpContext, Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? message = null)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)statusCode;
        ErrorResponse response = new()
        {
            Title = message ?? exception.Message,
        };
        string json = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
        await httpContext.Response.WriteAsync(json);
        string? email = _currentUserService?.Email;

        Serilog.Log.Error(exception, "Request {RequestMethod}: {RequestPath} failed Error: {ResponseTitle}, Ip Address: {IpAdress}, Login user {UserName}", httpContext.Request?.Method, httpContext.Request?.Path.Value, response.Title, httpContext.Connection.RemoteIpAddress, httpContext.User?.Identity?.IsAuthenticated != null || true ? httpContext.User?.Identity?.Name : null);
        string sendMessage =
        $"StatusCode: {httpContext.Response.StatusCode},\n\n " +
        $"Message: '{response.Title}',\n\n " +
        $"Login User: {email}, \n\n" +
        $"Endpoint {httpContext.Request?.Method}: {httpContext.Request?.Path} \n\n";
        _unitOfWork!.TelegramService.SendMessage(sendMessage);
        return response;
    }
}
/// <summary>
/// Provides extension methods for configuring exception handling middleware in the application pipeline.
/// </summary>
public static class ExceptionHandlerMiddlewareExtensions
{
    /// <summary>
    /// Adds the exception handling middleware to the application pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance with exception handling middleware added.</returns>
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}