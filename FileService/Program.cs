using FileService.Models;
using FileService.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDatabase>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Database"), optNpgsql =>
    {
        optNpgsql.MigrationsAssembly(typeof(ApplicationDatabase).Assembly.GetName().Name);
    });

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

await MigrateDatabase(app);


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();


async Task MigrateDatabase(WebApplication webApplication)
{
    await using var scope = webApplication.Services.CreateAsyncScope();

    var db = scope.ServiceProvider.GetRequiredService<ApplicationDatabase>();

    await db.Database.MigrateAsync();
}