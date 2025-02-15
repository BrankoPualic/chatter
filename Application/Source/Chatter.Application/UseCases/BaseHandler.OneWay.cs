namespace Chatter.Application.UseCases;

public abstract class BaseHandler<TRequest> : BaseHandlerProcess, IRequestHandler<TRequest>
	where TRequest : IRequest
{
	protected readonly IDatabaseContext _db;
	protected readonly IIdentityUser _currentUser;

	protected BaseHandler()
	{ }

	protected BaseHandler(IDatabaseContext db) : this() => _db = db;

	protected BaseHandler(IDatabaseContext db, IIdentityUser currentUser) : this(db) => _currentUser = currentUser;

	public abstract Task Handle(TRequest request, CancellationToken cancellationToken);
}