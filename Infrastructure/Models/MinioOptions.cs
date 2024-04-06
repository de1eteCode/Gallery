namespace Infrastructure.Models;

/// <summary>
/// Настройки для Minio
/// </summary>
public class MinioOptions
{
    /// <summary>
    /// Наименование бакета
    /// </summary>
    public string BucketName { get; init; }

    /// <summary>
    /// Эндпоинт Minio API
    /// </summary>
    public string ApiEndpoint { get; init; }

    /// <summary>
    /// Ключ доступа
    /// </summary>
    public string AccessKey { get; init; }

    /// <summary>
    /// Секретный ключ
    /// </summary>
    public string SecretKey { get; init; }
    
    /// <summary>
    /// Использование SSL
    /// </summary>
    public bool Secure { get; init; }
}