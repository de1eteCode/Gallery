using System.Net;
using System.Text.Json;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using WebApi.Models;

namespace WebApi.Utils;

public static class ExceptionHandlerExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var error = context.Features.Get<IExceptionHandlerPathFeature>()!.Error;

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = error switch
                {
                    IRestException applicationException => applicationException.Code,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(
                    new ErrorResponseVm
                    {
                        Error = error.GetType().Name,
                        Message = error.Message
                    }));
            });
        });
    }
}