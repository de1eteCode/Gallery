using System.Diagnostics;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;

    public PerformanceBehavior(ILogger<TRequest> logger)
    {
        _timer = new Stopwatch();

        _logger = logger;
    }

    public async ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        MessageHandlerDelegate<TRequest, TResponse> next)
    {
        _timer.Start();

        var response = await next(request, cancellationToken);

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogWarning($"Long Running Request: {requestName}; " +
                               $"{elapsedMilliseconds} milliseconds;");
        }

        return response;
    }
}