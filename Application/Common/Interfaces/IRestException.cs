namespace Application.Common.Interfaces;

/// <summary>
/// Rest ошибка сервиса
/// </summary>
public interface IRestException
{
    /// <summary>
    /// Http код
    /// </summary>
    public int Code { get; }
}