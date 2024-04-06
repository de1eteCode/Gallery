using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi;

public static class SwaggerSetup
{
    /// <summary>
    /// Метод добавления конфигурация для swagger
    /// </summary>
    /// <param name="configuration">Интерфейс конфигурации</param>
    /// <param name="services">Интерфейс для подключения сервисов к сборке</param>
    public static void AddSwagger(IConfiguration configuration, IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc(configuration["Version"], new OpenApiInfo
            {
                Title = configuration["ProjectName"],
                Version = configuration["Version"]
            });

            SetupSwaggerDocs(option);
        });
    }

    /// <summary>
    /// Настройка определения путей для xml документации
    /// </summary>
    /// <param name="swaggerGenOptions"></param>
    private static void SetupSwaggerDocs(SwaggerGenOptions swaggerGenOptions)
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        var xmlFile = $"{assemblyName}.xml";
        var xmlFileApplication = xmlFile.Replace("WebApi", "Application");
        var baseDirectory = AppContext.BaseDirectory;
        var baseDirectoryApplication = baseDirectory.Replace(assemblyName, "Application");
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        var xmlPathApplication = Path.Combine(baseDirectoryApplication, xmlFileApplication);
        swaggerGenOptions.IncludeXmlComments(xmlPath);
        swaggerGenOptions.IncludeXmlComments(xmlPathApplication);
        swaggerGenOptions.CustomSchemaIds(type => type.FullName);
    }
}