using System.Net;
using Application.Common.Interfaces;

namespace Application.Common.Exceptions;

/// <summary>
/// Нарушение внешнего ключа
/// </summary>
public class ForeignKeyConstraintException : Exception, IRestException
{
    /// <inheritdoc />
    public int Code => (int)HttpStatusCode.Conflict;
    
    public ForeignKeyConstraintException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    
    public ForeignKeyConstraintException(string message)
        : this(message, null)
    {
    }
}