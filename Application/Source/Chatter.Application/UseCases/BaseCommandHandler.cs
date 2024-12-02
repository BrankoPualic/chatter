namespace Chatter.Application.UseCases;

public abstract class BaseCommandHandler<TCommand> : BaseHandler<TCommand>
	where TCommand : BaseCommand
{
	protected BaseCommandHandler()
	{
	}

	protected BaseCommandHandler(IDatabaseContext db) : base(db)
	{
	}

	protected BaseCommandHandler(IDatabaseContext db, IIdentityUser currentUser) : base(db, currentUser)
	{
	}
}

public abstract class BaseCommandHandler<TCommand, TResponse> : BaseHandler<TCommand, TResponse>
	where TCommand : BaseCommand<TResponse>
{
	protected BaseCommandHandler()
	{
	}

	protected BaseCommandHandler(IDatabaseContext db) : base(db)
	{
	}

	protected BaseCommandHandler(IDatabaseContext db, IIdentityUser currentUser) : base(db, currentUser)
	{
	}

	protected BaseCommandHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper) : base(db, currentUser, mapper)
	{
	}

	protected BaseCommandHandler(IDatabaseContext db, IIdentityUser currentUser, IMapper mapper, ILogger logger) : base(db, currentUser, mapper, logger)
	{
	}
}