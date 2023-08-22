namespace Space.Application.Mappers;

public class ClassSessionMapper : Profile
{
    public ClassSessionMapper()
    {
        CreateMap<UpdateClassSessionRequestDto, ClassSession>()
            .ForMember(c => c.Date, opt => opt.MapFrom(cs => cs.ClassSessionDate));
    }
}
