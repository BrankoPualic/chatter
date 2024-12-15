namespace Chatter.Application.UseCases;

public abstract class BaseHandler<TRequest, TResponse> : BaseHandlerProcess, IRequestHandler<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	protected readonly IDatabaseContext _db;
	protected readonly IMapper _mapper;
	protected readonly IIdentityUser _currentUser;
	protected readonly ILogger _logger;

	protected BaseHandler()
	{ }

	protected BaseHandler(IDatabaseContext db) : this() => _db = db;

	protected BaseHandler(IDatabaseContext db, IIdentityUser currentUser) : this(db) => _currentUser = currentUser;

	protected BaseHandler(IDatabaseContext db, IMapper mapper) : this(db) => _mapper = mapper;

	protected BaseHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper) : this(db, currentUser) => _mapper = mapper;

	protected BaseHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper, ILogger logger) : this(db, currentUser, mapper) => _logger = logger;

	public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}