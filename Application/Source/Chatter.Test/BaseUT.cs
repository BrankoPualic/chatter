using AutoMapper;
using Chatter.Common;
using Chatter.DependencyRegister;
using Chatter.Domain;
using Chatter.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public abstract class BaseUT
{
	private readonly IServiceProvider _serviceProvider;

	protected BaseUT()
	{
		var services = new ServiceCollection();

		services.AddLogging();
		services.AddDbContext<DatabaseContext>();
		services.AllAplicationServices();
		services.AddSingleton(MockIdentityUser());

		_serviceProvider = services.BuildServiceProvider();
	}

	protected TService Get<TService>() where TService : notnull
	{
		return _serviceProvider.GetRequiredService<TService>();
	}

	protected IIdentityUser CurrentUser => Get<IIdentityUser>();

	protected IMapper Mapper => Get<IMapper>();

	protected DatabaseContext db => Get<DatabaseContext>();

	private static IIdentityUser MockIdentityUser()
	{
		var mock = new Mock<IIdentityUser>();
		mock.Setup(_ => _.Id).Returns(Guid.Parse(Constants.SYSTEM_USER));
		mock.Setup(_ => _.Email).Returns("sysadmin@chatter.com");
		mock.Setup(_ => _.Username).Returns("admin");
		mock.Setup(_ => _.Roles).Returns([eSystemRole.SystemAdministrator]);

		return mock.Object;
	}
}