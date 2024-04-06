namespace Infrastructure.Common.Interfaces;

/// <summary>
/// Создатель бакетов в Minio
/// </summary>
public interface IS3MinioBucketSeeder
{
    /// <summary>
    /// Проверка и создание бакета
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task SeedBucket(CancellationToken cancellationToken = default);
}