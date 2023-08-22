using System;
using Space.Domain.Entities;

namespace Space.Application.Handlers;
public record GetRoomScheduleSessionsQuery() : IRequest<IEnumerable<GetRoomScheduleSessionsResponseDto>>;


internal class GetRoomScheduleSessionsQueryHandler : IRequestHandler<GetRoomScheduleSessionsQuery, IEnumerable<GetRoomScheduleSessionsResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;

    public GetRoomScheduleSessionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetRoomScheduleSessionsResponseDto>> Handle(GetRoomScheduleSessionsQuery request, CancellationToken cancellationToken)
    {

        var roomScheduleQuery = await _unitOfWork.RoomScheduleRepository
                  .GetAllAsync(q => q.Year == 2023, tracking: false, "Class.Session");

        var rooms = await _unitOfWork.RoomRepository.GetAllAsync();
        var classSessions = await _unitOfWork.ClassSessionRepository.GetAllAsync();


        var response = new List<GetRoomScheduleSessionsResponseDto>();

        foreach (var item in rooms)
        {

            var model = new GetRoomScheduleSessionsResponseDto();
            model.RoomName = item.Name;


            var weekScheduleSessions = new List<WeekScheduleSessions>();

            for (int i = 0; i <= 52; i++)
            {
                WeekScheduleSessions weekScheduleModel = new WeekScheduleSessions();
                weekScheduleModel.WeekOfYear = i + 1;

                var getRoomScheduleSessionsByWeekDtos = new List<GetRoomScheduleSessionsByWeekDto>();


                var roomSchedules = roomScheduleQuery.Where(q => (q.GeneralDate.DayOfYear / 7) == i && q.RoomId == item.Id);



                foreach (var roomSchedule in roomSchedules)
                {

                    var roomScheduleSessionsByWeekDto = new GetRoomScheduleSessionsByWeekDto();
                    roomScheduleSessionsByWeekDto.ClassName = roomSchedule.Class?.Name;
                    roomScheduleSessionsByWeekDto.SessionName = roomSchedule.Class?.Session?.Name;

                    if(!getRoomScheduleSessionsByWeekDtos.Any(q => q.ClassName == roomSchedule.Class?.Name && q.SessionName == roomSchedule.Class?.Session?.Name))
                    getRoomScheduleSessionsByWeekDtos.Add(roomScheduleSessionsByWeekDto);


                }

                weekScheduleModel.GetRoomScheduleSessionsByWeeks = getRoomScheduleSessionsByWeekDtos;

                weekScheduleSessions.Add(weekScheduleModel);


            }

            model.WeekScheduleSessions = weekScheduleSessions;
            response.Add(model);

        }

        return response;
    }
}



