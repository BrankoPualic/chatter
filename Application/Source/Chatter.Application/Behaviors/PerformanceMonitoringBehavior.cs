using System.Diagnostics;

namespace Chatter.Application.Behaviors;

public class PerformanceMonitoringBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger<PerformanceMonitoringBehavior<TRequest, TResponse>> _logger;
	private const int THRESHOLD = 5000;

	public PerformanceMonitoringBehavior(ILogger<PerformanceMonitoringBehavior<TRequest, TResponse>> logger)
	{
		_logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var timer = Stopwatch.StartNew();

		var response = await next();

		timer.Stop();

		var elapsedMilliseconds = timer.ElapsedMilliseconds;

		if (elapsedMilliseconds > THRESHOLD)
		{
			_logger.LogWarning($"Long Running Request: {typeof(TRequest).Name} {elapsedMilliseconds} ms");
		}

		return response;
	}
}