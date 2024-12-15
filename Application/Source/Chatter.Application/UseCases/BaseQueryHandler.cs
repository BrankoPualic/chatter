namespace Chatter.Application.UseCases;

public abstract class BaseQueryHandler<TQuery, TResponse> : BaseHandler<TQuery, ResponseWrapper<TResponse>>
	where TQuery : BaseQuery<TResponse>
{
	protected BaseQueryHandler()
	{ }

	protected BaseQueryHandler(IDatabaseContext db) : base(db)
	{
	}

	protected BaseQueryHandler(IDatabaseContext db, IIdentityUser currentUser) : base(db, currentUser)
	{
	}

	protected BaseQueryHandler(IDatabaseContext db, IMapper mapper) : base(db, mapper)
	{
	}

	protected BaseQueryHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper) : base(db, currentUser, mapper)
	{
	}

	protected BaseQueryHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper, ILogger logger) : base(db, currentUser, mapper, logger)
	{
	}
}