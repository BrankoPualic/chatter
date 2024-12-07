using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Users;

public class UserLoginLog : BaseDomain
{
	public UserLoginLog(Guid userId)
	{
		UserId = userId;
		CreatedOn = DateTime.UtcNow;
	}

	public Guid UserId { get; set; }

	public DateTime CreatedOn { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }
}