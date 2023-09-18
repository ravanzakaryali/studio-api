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

    public CreateClassSessionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateClassSessionCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _unitOfWork.ClassRepository.GetAsync(request.ClassId, tracking: false, "Session.Details", "Program.Modules.SubModules")
            ?? throw new NotFoundException(nameof(Class), request.ClassId);
        _unitOfWork.ClassSessionRepository.RemoveRange(await _unitOfWork.ClassSessionRepository.GetAllAsync(cr => cr.ClassId == @class.Id));

        List<ClassSession> classSessions = new();

        if (@class.StartDate == null || @class.RoomId == null)
            throw new Exception("Class start date or room null");

        DateTime startDate = @class.StartDate.Value;
        var sessions = request.Sessions.ToList();
        int startDayOfWeek = (int)@class.StartDate.Value.DayOfWeek;
        List<DayOfWeek> selectedDays = sessions.Select(c => c.DayOfWeek).ToList();

        IEnumerable<Holiday> holidays = await _unitOfWork.HolidayRepository.GetAllAsync();
        List<DateTime> holidayDates = new();
        foreach (Holiday holiday in holidays)
        {
            for (DateOnly date = holiday.StartDate; date <= holiday.EndDate; date = date.AddDays(1))
            {
                holidayDates.Add(date.ToDateTime(new TimeOnly(0, 0)));
            }
        }
        var total2 = @class.Program.TotalHours;
        int count = 0;
        while (total2 > 0)
        {
            foreach (var session in sessions.OrderBy(c => c.DayOfWeek))
            {
                var daysToAdd = ((int)session.DayOfWeek - (int)startDayOfWeek + 7) % 7;
                int numSelectedDays = sessions.Count;

                int hour = (session.End - session.Start).Hours;

                DateTime dateTime = startDate.AddDays(count * 7 + daysToAdd);

                if (holidayDates.Contains(dateTime))
                {
                    continue;
                }

                if (hour != 0)
                {
                    classSessions.Add(new ClassSession()
                    {
                        Category = session.Category,
                        ClassId = @class.Id,
                        StartTime = session.Start,
                        EndTime = session.End,
                        RoomId = @class.RoomId,
                        TotalHour = hour,
                        Date = dateTime
                    });
                    if (session.Category != ClassSessionCategory.Lab)
                        total2 -= hour;
                    if (total2 <= 0)
                        break;

                }
            }
            count++;
        }


        @class.EndDate = classSessions.Max(c => c.Date).Date;

        await _unitOfWork.ClassSessionRepository.AddRangeAsync(classSessions);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}