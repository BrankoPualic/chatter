namespace Chatter.Domain.Models;

public abstract class BaseDomain : IBaseDomain
{
	public Guid Id { get; set; }

	public bool IsNew => Id.Equals(default);
}
