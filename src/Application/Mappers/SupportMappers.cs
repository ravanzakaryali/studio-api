using System.Net.NetworkInformation;

namespace Space.Application.Mappers;

public class SupportMappers : Profile
{
    public SupportMappers()
    {
        CreateMap<Support, GetSupportResponseDto>()
            .ForMember(c => c.Images, opt => opt.MapFrom(c => c.SupportImages));
        CreateMap<SupportImage, GetFileResponse>()
            .ForMember(c => c.Path, opt => opt.MapFrom(c => "https://studioapi.code.az/api/images/" + c.FileName));

        CreateMap<SupportCategory, GetSupportCategory>();

        CreateMap<User, GetSupportUser>();

    }
}
