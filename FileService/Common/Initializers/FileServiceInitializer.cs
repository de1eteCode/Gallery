using System.Runtime.CompilerServices;

namespace FileService.Common.Initializers;

/// <summary>
/// Класс статической инициализации сборки
/// </summary>
/// <remarks>
/// https://stackoverflow.com/questions/69961449/net6-and-datetime-problem-cannot-write-datetime-with-kind-utc-to-postgresql-ty/70142836#70142836
/// </remarks>
public static class FileServiceInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}