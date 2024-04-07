using Application;
using Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WebApi;
using WebApi.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(opt =>
    {
        // Ремарка на будущее
        // По возможности переделать на авто ответы
        // https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.2#automatic-http-400-responses
        opt.SuppressModelStateInvalidFilter = true;
    });
SwaggerSetup.AddSwagger(builder.Configuration, builder.Services);

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = null;
});

var app = builder.Build();

await app.Services.MigrateDb();
await app.Services.SeedS3();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();
app.MapControllers();

app.Run();
