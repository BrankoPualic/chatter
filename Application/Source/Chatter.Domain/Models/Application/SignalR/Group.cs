using System.ComponentModel.DataAnnotations;

namespace Chatter.Domain.Models.Application.SignalR;

public class Group
{
	public Group()
	{ }

	public Group(Guid chatId) => ChatId = chatId;

	[Key]
	public Guid ChatId { get; set; }

	public ICollection<Connection> Connections { get; set; } = [];
}