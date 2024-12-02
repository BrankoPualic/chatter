namespace Chatter.Application.UseCases;

public class BaseQuery<TResponse> : IRequest<ResponseWrapper<TResponse>>
{ }