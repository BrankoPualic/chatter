using System.Diagnostics;

namespace Chatter.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

	public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
		=> _logger = logger;

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var timer = Stopwatch.StartNew();

		_logger.LogInformation($"Handling {typeof(TRequest).Name}");

		var response = await next();

		timer.Stop();

		_logger.LogInformation($"Handled {typeof(TResponse).Name} in {timer.ElapsedMilliseconds} ms");

		return response;
	}
}