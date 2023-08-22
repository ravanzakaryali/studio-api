namespace Space.Application.Mappers;

public class RoomMappers : Profile
{
	public RoomMappers()
	{
		CreateMap<CreateRoomCommand, Room>();
		CreateMap<UpdateRoomRequestDto, Room>();
		CreateMap<Room, CreateRoomResponseDto>();
		CreateMap<Room, GetRoomResponseDto>();
    }
}
