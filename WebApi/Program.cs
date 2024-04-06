using Application;
using Infrastructure;
using ReactCoreTemplate.WebApi.Utils;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
SwaggerSetup.AddSwagger(builder.Configuration, builder.Services);

var app = builder.Build();

await app.Services.MigrateDb();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();
app.MapControllers();

app.Run();
