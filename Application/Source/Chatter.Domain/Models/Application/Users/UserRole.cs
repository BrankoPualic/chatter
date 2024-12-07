using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatter.Domain.Models.Application.Users;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
public class UserRole
{
	public Guid UserId { get; set; }

	public eSystemRole RoleId { get; set; }

	[ForeignKey(nameof(UserId))]
	public User User { get; set; }
}