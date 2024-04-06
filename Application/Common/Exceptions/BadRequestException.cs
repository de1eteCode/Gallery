using System.Net;
using Application.Common.Interfaces;

namespace Application.Common.Exceptions;

/// <summary>
/// Ошибка: Неверный запрос
/// </summary>
public class BadRequestException : Exception, IRestException
{
    /// <inheritdoc />
    public int Code => (int)HttpStatusCode.Forbidden;

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public BadRequestException(string message)
        : this(message, null)
    {
    }
}