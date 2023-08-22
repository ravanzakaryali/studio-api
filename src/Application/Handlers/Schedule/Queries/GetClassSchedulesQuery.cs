using System;

namespace Space.Application.Handlers;

public record GetClassSchedulesQuery() : IRequest<IEnumerable<GetClassSchedulesResponseDto>>;


internal class GetClassSchedulesQueryHandler : IRequestHandler<GetClassSchedulesQuery, IEnumerable<GetClassSchedulesResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;

    public GetClassSchedulesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<IEnumerable<GetClassSchedulesResponseDto>> Handle(GetClassSchedulesQuery request, CancellationToken cancellationToken)
    {

        var roomSchedules = await _unitOfWork.RoomScheduleRepository.GetAllAsync(q => q.Year == 2023, tracking: false, "Room","Class.Program");
        var classes = await _unitOfWork.ClassRepository.GetAllAsync(q => q.IsActive, tracking: false, "Room","Program");

        var response = new List<GetClassSchedulesResponseDto>();

        foreach (var item in classes)
        {
            var classSchedules = roomSchedules.Where(q => q.ClassId == item.Id).OrderBy(q => q.GeneralDate);

            var rooms = classSchedules.Select(q => q.Room?.Id).Distinct();

            foreach (var room in rooms)
            {
                var startDate = classSchedules.Where(q => q.RoomId == room).OrderBy(q => q.GeneralDate)?.Take(1).FirstOrDefault()?.GeneralDate;
                var endDate = classSchedules.Where(q => q.RoomId == room).OrderByDescending(q => q.GeneralDate)?.Take(1).FirstOrDefault()?.GeneralDate;

                GetClassSchedulesResponseDto model = new GetClassSchedulesResponseDto();
                model.ClassColor = item.Program?.Color;
                model.ClassName = item.Name;
                model.RoomName = item.Room?.Name;
                model.StartDate = startDate;
                model.EndDate = endDate;
                model.ClassId = item.Id;

                response.Add(model);

            }


        }

        return response;
    }
}




