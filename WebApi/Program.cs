using Application;
using Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WebApi;
using WebApi.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
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
