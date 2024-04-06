using System.Net;
using Application.Common.Interfaces;

namespace Application.Common.Exceptions;

/// <summary>
/// Ошибка: Не найден объект
/// </summary>
public class NotFoundException : Exception, IRestException
{
    /// <inheritdoc />
    public int Code => (int)HttpStatusCode.NotFound;
    
    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string name, object key)
        : this($"Объект \"{name}\" ({key}) не был найден.")
    {
    }
}