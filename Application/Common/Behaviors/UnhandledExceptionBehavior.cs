using Mediator;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        MessageHandlerDelegate<TRequest, TResponse> next)
    {
        try
        {
            return await next(request, cancellationToken);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            var message = ex.Message;
            var stackTrace = ex.StackTrace ?? string.Empty;
            var innerExceptionMessage = ex.InnerException?.Message ?? string.Empty;

            _logger.LogError($"Unhandled Exception: {requestName};{message};{stackTrace};" +
                             $"{innerExceptionMessage}");

            throw;
        }
    }
}