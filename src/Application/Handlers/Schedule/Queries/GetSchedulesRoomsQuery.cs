using Microsoft.VisualBasic;
using System;
using System.Globalization;

namespace Space.Application.Handlers;

public record GetSchedulesRoomsQuery(int? Year) : IRequest<IEnumerable<GetSchedulesRoomsResponseDto>>;


internal class GetSchedulesRoomQueryHandler : IRequestHandler<GetSchedulesRoomsQuery, IEnumerable<GetSchedulesRoomsResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetSchedulesRoomQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetSchedulesRoomsResponseDto>> Handle(GetSchedulesRoomsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Room> rooms = await _unitOfWork.RoomRepository.GetAllAsync(includes: "RoomSchedules");
        List<GetSchedulesRoomsResponseDto> response = new();

        foreach (Room room in rooms)
        {
            GetSchedulesRoomsResponseDto responseRoom = new GetSchedulesRoomsResponseDto()
            {
                Room = new GetRoomResponseDto()
                {
                    Capacity = room.Capacity ?? 0,
                    Id = room.Id,
                    Name = room.Name,
                    Type = room.Type
                }
            };
            for (int i = 1; i <= 12; i++)
            {
                int roomSchedulesHours = room.RoomSchedules.Where(c => c.DayOfMonth == i && c.Year == request.Year).ToList().Sum(c => DateTime.Parse(c.EndDate).Hour - DateTime.Parse(c.StartDate).Hour);
                int daysInApril = DateTime.DaysInMonth(request.Year ?? new DateTime().Year, i) * 13;
                int value = roomSchedulesHours * 100 / daysInApril;
                responseRoom.OccupancyRates.Add(new OccupancyRate()
                {
                    MonthOfYear = i,
                    Value = value
                });
            }
            response.Add(responseRoom);
        }
        return response;
    }
}
