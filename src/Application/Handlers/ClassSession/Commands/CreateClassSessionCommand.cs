using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Space.Domain.Entities;
using System;
using System.Reflection;

namespace Space.Application.Handlers;

public class CreateClassSessionCommand : IRequest
{
    public Guid ClassId { get; set; }
    public IEnumerable<CreateClassSessionDto> Sessions { get; set; } = null!;
}

internal class CreateClassSessionCommandHandler : IRequestHandler<CreateClassSessionCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IClassRepository _classRepository;
    readonly IClassSessionRepository _classSessionRepository;
    readonly IHolidayRepository _holidayRepository;

    public CreateClassSessionCommandHandler(IUnitOfWork unitOfWork, IClassRepository classRepository, IClassSessionRepository sessionRepository, IHolidayRepository holidayRepository)
    {
        _unitOfWork = unitOfWork;
        _classRepository = classRepository;
        _classSessionRepository = sessionRepository;
        _holidayRepository = holidayRepository;
    }

    public async Task Handle(CreateClassSessionCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _classRepository.GetAsync(request.ClassId, tracking: false, "Session.Details", "Program.Modules.SubModules")
            ?? throw new NotFoundException(nameof(Class), request.ClassId);
        _classSessionRepository.RemoveRange(await _classSessionRepository.GetAllAsync(cr => cr.ClassId == @class.Id));


        if (@class.StartDate == null || @class.RoomId == null)
            throw new Exception("Class start date or room null");

        List<CreateClassSessionDto> sessions = request.Sessions.ToList();
        List<DayOfWeek> selectedDays = sessions.Select(c => c.DayOfWeek).ToList();

        List<DateTime> holidayDates = await _holidayRepository.GetDatesAsync();


        List<ClassSession> classSessions = _classSessionRepository.GenerateSessions(
                                                                                       @class.Program.TotalHours,
                                                                                       sessions,
                                                                                       @class.StartDate.Value,
                                                                                       holidayDates,
                                                                                       @class.Id,
                                                                                       @class.RoomId.Value);

        @class.EndDate = classSessions.Max(c => c.Date).Date;
        await _classSessionRepository.AddRangeAsync(classSessions);
    }
}