using FileService.Models;
using FileService.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDatabase>(opt =>
{
    opt.UseInMemoryDatabase("db");

#if DEBUG
    opt.EnableSensitiveDataLogging();
#endif
});

builder.Services.Configure<FileStoreOptions>(builder.Configuration.GetSection("FileStore"));

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();