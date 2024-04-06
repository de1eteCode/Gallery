using System.Net;
using Application.Common.Interfaces;

namespace Application.Common.Exceptions;

/// <summary>
/// Ошибка: Доступ запрещен
/// </summary>
public class ForbiddenException : Exception, IRestException
{
    /// <inheritdoc />
    public int Code => (int)HttpStatusCode.Forbidden;

    public ForbiddenException(Exception innerException)
        : base("Доступ запрещен", innerException)
    {
    }

    public ForbiddenException()
        : this(null)
    {
    }
}