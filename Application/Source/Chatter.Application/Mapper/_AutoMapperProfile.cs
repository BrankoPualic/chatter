namespace Chatter.Application.Mapper;

public abstract class AutoMapperProfile : Profile
{
	protected AutoMapperProfile()
	{
		// Data
		CreateMap(typeof(PagingResult<>), typeof(PagingResultDto<>))
			.ForMember("Data", opt => opt.MapFrom(src => src.GetType().GetProperty("Data").GetValue(src)))
			.ForMember("Total", opt => opt.MapFrom(src => src.GetType().GetProperty("Total").GetValue(src)));
		CreateMap<Blob, BlobDto>()
			.ForLookup(_ => _.Type, _ => _.TypeId);
	}
}