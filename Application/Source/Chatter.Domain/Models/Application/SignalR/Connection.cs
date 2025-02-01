using System.ComponentModel.DataAnnotations;

namespace Chatter.Domain.Models.Application.SignalR;

public class Connection
{
	public Connection()
	{ }

	public Connection(string connectionId, Guid userId)
	{
		ConnectionId = connectionId;
		UserId = userId;
	}

	[Key]
	public string ConnectionId { get; set; }

	public Guid UserId { get; set; }
}