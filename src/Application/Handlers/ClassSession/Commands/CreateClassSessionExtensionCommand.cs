﻿using Microsoft.AspNetCore.Http;
using Space.Domain.Entities;

namespace Space.Application.Handlers;

public class CreateClassSessionExtensionCommand : IRequest
{
    public CreateClassSessionExtensionCommand(double hours, int classId, DateOnly? startDate)
    {
        Hours = hours;
        ClassId = classId;
        StartDate = startDate;
    }

    public DateOnly? StartDate { get; set; }
    public double Hours { get; set; }
    public int ClassId { get; set; }

}
public class CreateClassSessionExtensionCommandHandler : IRequestHandler<CreateClassSessionExtensionCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateClassSessionExtensionCommandHandler(
        IUnitOfWork unitOfWork, ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateClassSessionExtensionCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.ClassId)
            .Include(c => c.ClassSessions)
            .Include(c => c.Session)
            .ThenInclude(c => c.Details)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.ClassId);



        List<DateOnly> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();
        List<ClassSession> classSessions = new();

        DateOnly startDate = request.StartDate ?? @class.ClassSessions.MaxBy(c => c.Date)!.Date;
        int startDayOfWeek = (int)startDate.DayOfWeek;
        int count = 0;
        double totalHour = request.Hours;


        //Todo: Code Review
        while (totalHour > 0)
        {
            foreach (var session in @class.Session.Details.OrderBy(c => c.DayOfWeek))
            {
                var daysToAdd = ((int)session.DayOfWeek - (int)startDayOfWeek + 7) % 7;
                int numSelectedDays = @class.Session.Details.Count();

                int hour = (session.EndTime - session.StartTime).Hours;
                DateOnly dateTime = startDate.AddDays(count * 7 + daysToAdd);
                Console.WriteLine(dateTime);

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
                        StartTime = session.StartTime,
                        EndTime = session.EndTime,
                        RoomId = @class.RoomId,
                        TotalHours = hour,
                        Date = dateTime,
                    });
                    if (session.Category != ClassSessionCategory.Lab)
                        totalHour -= hour;
                    if (totalHour <= 0) break;
                }
            }
            count++;
        }
        @class.EndDate = classSessions.MaxBy(c => c.Date)?.Date;

        await _spaceDbContext.ClassSessions.AddRangeAsync(classSessions, cancellationToken);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
