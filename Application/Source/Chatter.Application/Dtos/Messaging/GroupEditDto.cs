using Chatter.Application.Dtos.Users;
using Chatter.Domain.Models;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.Dtos.Messaging;

public class GroupEditDto : IBaseDomain
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public List<UserLightDto> Members { get; set; } = [];

	public void ToModel(Chat model, IDatabaseContext db)
	{
		model.Id = Id == Guid.Empty ? Guid.NewGuid() : Id;
		model.GroupName = Name;
		model.IsGroup = true;

		var missingMembers = model.Members
			.Where(_ => !Members.Any(m => m.Id == _.UserId))
			.ToList();

		var newMembers = Members
			.Where(_ => !model.Members.Any(m => m.UserId == _.Id))
			.Select(_ => new ChatMember { UserId = _.Id, RoleId = eChatRole.Member })
			.ToList();

		model.Members.RemoveRange(missingMembers);
		model.Members.AddRange(newMembers);

		db.ChatMembers.RemoveRange(missingMembers);
	}
}