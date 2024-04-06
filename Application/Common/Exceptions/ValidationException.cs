using System.Net;
using Application.Common.Interfaces;
using FluentValidation.Results;

namespace Application.Common.Exceptions;

/// <summary>
/// Ошибки в данных запроса
/// </summary>
public class ValidationException : Exception, IRestException
{
    /// <inheritdoc />
    public int Code => (int)HttpStatusCode.BadRequest;
    
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this(string.Join("; ", failures.Select(i => $"{i.PropertyName}: {i.ErrorMessage}")))
    {
        var failureGroups = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

        foreach (var failureGroup in failureGroups)
        {
            var propertyName = failureGroup.Key;
            var propertyFailures = failureGroup.ToArray();

            Errors.Add(propertyName, propertyFailures);
        }
    }
}