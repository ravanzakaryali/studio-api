namespace Space.Application.Mappers;

public class ClassSessionMapper : Profile
{
    public ClassSessionMapper()
    {
        CreateMap<UpdateClassSessionRequestDto, ClassTimeSheet>()
            .ForMember(c => c.Date, opt => opt.MapFrom(cs => cs.ClassSessionDate));
    }
}
