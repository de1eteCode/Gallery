using Application.Common.Exceptions;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Behaviors;

public class DbExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    public async ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        MessageHandlerDelegate<TRequest, TResponse> next)
    {
        try
        {
            return await next(request, cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            var innerExceptionMessage = ex.InnerException?.Message ?? string.Empty;

            // Нарушение ограничение внешнего ключа
            if (innerExceptionMessage.StartsWith(DbExceptionCode.ForeignKeyConstraint))
                throw new ForeignKeyConstraintException(innerExceptionMessage);
            // Нарушение уникальности
            if (innerExceptionMessage.StartsWith(DbExceptionCode.UniqueConstraint))
                throw new UniqueConstraintException(innerExceptionMessage);

            throw;
        }
    }

    private static class DbExceptionCode
    {
        public static string ForeignKeyConstraint => "23503";
        public static string UniqueConstraint => "23505";
    }
}