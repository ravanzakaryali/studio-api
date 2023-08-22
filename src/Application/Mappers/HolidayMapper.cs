namespace Space.Application.Mappers;

public class HolidayMapper : Profile
{
    public HolidayMapper()
    {
        CreateMap<Holiday, HolidayResponseDto>();
        CreateMap<CreateHolidayCommand, Holiday>();
        CreateMap<UpdateHolidayRequestDto, Holiday>();
        
    }
}
