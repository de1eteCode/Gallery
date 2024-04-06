using Application.Common.Interfaces;
using Infrastructure.Common.Interfaces;
using Infrastructure.Models;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddSingleton(TimeProvider.System);

        serviceCollection.AddScoped<IFileService, FileService>();
        serviceCollection.Configure<UploadFileOptions>(configuration.GetSection("UploadFileOptions"));
        
        serviceCollection.AddDb(configuration);
        serviceCollection.AddMinioS3(configuration);

        return serviceCollection;
    }

    private static void AddDb(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<CreatedSaveChangesInterceptor>();
        serviceCollection.AddScoped<SoftDeleteSaveChangesInterceptor>();
        
        serviceCollection.AddDbContext<IApplicationDbContext, ApplicationDbContext>((sp, opt) =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("Postgres"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

            opt.AddInterceptors(sp.GetRequiredService<CreatedSaveChangesInterceptor>());
            opt.AddInterceptors(sp.GetRequiredService<SoftDeleteSaveChangesInterceptor>());
        });
    }

    private static void AddMinioS3(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var options = configuration.GetSection("Minio").Get<MinioOptions>() ?? throw new Exception();
        
        serviceCollection.Configure<MinioOptions>(configuration.GetSection("Minio"));
        serviceCollection.AddScoped<IS3Service, S3Service>();
        serviceCollection.AddScoped<IS3MinioBucketSeeder, S3Service>();

        serviceCollection.AddMinio(client =>
        {
            client.WithEndpoint(options.ApiEndpoint)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL(options.Secure);
        });
    }

    public static async Task MigrateDb(this IServiceProvider serviceCollection)
    {
        await using var scope = serviceCollection.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (context.Database.IsNpgsql())
            await context.Database.MigrateAsync();
    }

    public static async Task SeedS3(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var s3Seeder = scope.ServiceProvider.GetRequiredService<IS3MinioBucketSeeder>();

        await s3Seeder.SeedBucket();
    }
}