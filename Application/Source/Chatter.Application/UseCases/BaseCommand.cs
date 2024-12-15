namespace Chatter.Application.UseCases;

public class BaseCommand : IRequest<ResponseWrapper>
{ }

public class BaseCommand<TResponse> : IRequest<ResponseWrapper<TResponse>>
{ }