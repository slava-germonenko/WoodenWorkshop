using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using WoodenWorkshop.Common.Core.Exceptions;
using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.PublicApi.Web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly JsonSerializerOptions _serializationOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
    };
    
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IWebHostEnvironment environment)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (CoreLogicException e)
        {
            await WriteResponseAsync(context, HttpStatusCode.BadRequest, BaseError.FromException(e));
        }
        catch (Exception e)
        {
            var apiResponse = environment.IsDevelopment()
                ? new DevError { ErrorCode = 0, Message = e.Message, StackTrace = e.StackTrace }
                : new BaseError{ ErrorCode = 0, Message = "Произошла непредвиденная ошибка! Если ошибка повторяется – пожалуйста, свяжитесь с администрацией." };
            await WriteResponseAsync(context, HttpStatusCode.InternalServerError, apiResponse);
        }
    }
    
    private async Task WriteResponseAsync(HttpContext context, HttpStatusCode code, object? body)
    {
        context.Response.StatusCode = (int)code;
        if (body is not null)
        {
            var serializedBodyBytes = JsonSerializer.SerializeToUtf8Bytes(body, _serializationOptions);
            await context.Response.Body.WriteAsync(serializedBodyBytes);
        }
    }
}