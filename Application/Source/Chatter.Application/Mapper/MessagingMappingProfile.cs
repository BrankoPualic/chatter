using Chatter.Application.Dtos.Messaging;
using Chatter.Domain.Models.Application.Messaging;

namespace Chatter.Application.Mapper;

public class MessagingMappingProfile : AutoMapperProfile
{
	public MessagingMappingProfile()
	{
		CreateMap<Message, MessageDto>()
			.ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Status))
			.ForMember(dest => dest.IsEditable, opt => opt.MapFrom(src => src.IsEditable()));
		CreateMap<Attachment, AttachmentDto>();
		CreateMap<Chat, ChatDto>();
		CreateMap<Chat, ChatLightDto>();
	}
}