using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddSingleton(TimeProvider.System);
        
        AddDb(serviceCollection, configuration);

        return serviceCollection;
    }

    private static void AddDb(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<CreatedSaveChangesInterceptor>();
        
        serviceCollection.AddDbContext<IApplicationDbContext, ApplicationDbContext>((sp, opt) =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("Postgres"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

            opt.AddInterceptors(sp.GetRequiredService<CreatedSaveChangesInterceptor>());
        });
    }

    public static async Task MigrateDb(this IServiceProvider serviceCollection)
    {
        await using var scope = serviceCollection.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (context.Database.IsNpgsql())
            await context.Database.MigrateAsync();
    }
}