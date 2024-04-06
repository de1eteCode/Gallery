using System.Text.Json.Serialization;

namespace WebApi.Models;

/// <summary>
/// Модель представления ответа ошибки для REST API
/// </summary>
[Serializable]
public class ErrorResponseVm
{
    /// <summary>
    /// Тип ошибки
    /// </summary>
    [JsonPropertyName("error")]
    public string Error { get; set; }

    /// <summary>
    /// Сообщение ошибки
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }
}