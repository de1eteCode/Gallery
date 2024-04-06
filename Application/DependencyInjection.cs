using System.Reflection;
using Application.Common.Behaviors;
using AutoMapper.EquivalencyExpression;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddAutoMapper(conf =>
        {
            conf.AddCollectionMappers();
        }, Assembly.GetExecutingAssembly());
        serviceCollection.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
        serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        serviceCollection.AddBehaviors();

        return serviceCollection;
    }

    private static IServiceCollection AddBehaviors(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}