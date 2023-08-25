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

        IEnumerable<RoomSchedule> roomSchedules = await _unitOfWork.RoomScheduleRepository.GetAllAsync(q => q.Year == 2023, tracking: false);
        IEnumerable<Class> classes = await _unitOfWork.ClassRepository.GetAllAsync(q => q.IsActive, tracking: false, "Room", "Program");

        List<GetClassSchedulesResponseDto> response = new();

        foreach (Class item in classes)
        {
            IOrderedEnumerable<RoomSchedule> classSchedules = roomSchedules.Where(q => q.ClassId == item.Id).OrderBy(q => q.GeneralDate);

            IEnumerable<Guid?> rooms = classSchedules.Select(q => q.RoomId).Distinct();

            foreach (Guid? room in rooms)
            {
                DateTime? startDate = classSchedules.Where(q => q.RoomId == room).OrderBy(q => q.GeneralDate)?.Take(1).FirstOrDefault()?.GeneralDate;
                DateTime? endDate = classSchedules.Where(q => q.RoomId == room).OrderByDescending(q => q.GeneralDate)?.Take(1).FirstOrDefault()?.GeneralDate;

                GetClassSchedulesResponseDto model = new()
                {
                    ClassColor = item.Program?.Color ?? "",
                    ClassName = item.Name,
                    RoomName = item.Room?.Name ?? "",
                    StartDate = startDate,
                    EndDate = endDate,
                    ClassId = item.Id
                };
                response.Add(model);
            }
        }

        return response;
    }
}




