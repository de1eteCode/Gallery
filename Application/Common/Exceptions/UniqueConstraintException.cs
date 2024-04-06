using System.Net;
using Application.Common.Interfaces;

namespace Application.Common.Exceptions;

/// <summary>
/// Нарушение уникальности ключа
/// </summary>
public class UniqueConstraintException : Exception, IRestException
{
    /// <inheritdoc />
    public int Code => (int)HttpStatusCode.Conflict;
    
    public UniqueConstraintException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    
    public UniqueConstraintException(string message)
        : this(message, null)
    {
    }
}