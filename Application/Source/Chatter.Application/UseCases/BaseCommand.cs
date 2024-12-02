namespace Chatter.Application.UseCases;

public class BaseCommand : IRequest
{ }

public class BaseCommand<TResponse> : IRequest<ResponseWrapper<TResponse>>
{ }