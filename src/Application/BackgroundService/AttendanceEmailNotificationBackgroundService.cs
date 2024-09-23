using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class NotificationBackgroundService : BackgroundService
{
    private static IServiceScopeFactory? _serviceProvider;

    public NotificationBackgroundService(IServiceScopeFactory serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using IServiceScope scope = _serviceProvider!.CreateScope();
            using ISpaceDbContext dbContext = scope.ServiceProvider.GetRequiredService<ISpaceDbContext>();
            using IUnitOfWork unitOfWorkService = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            DateOnly dateNow = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(4));
            int hour = DateTime.UtcNow.AddHours(4).Hour;
            int minute = DateTime.UtcNow.AddHours(4).Minute;

            List<Class> classes = await dbContext.Classes
                .Include(c => c.Session)
                .ThenInclude(s => s.Details)
                .ToListAsync(stoppingToken);

            List<DateOnly> holidayService = await unitOfWorkService.HolidayService.GetDatesAsync();

            List<ClassTimeSheet> classTimeSheets = await dbContext.ClassTimeSheets
                .Where(cts => cts.Date == dateNow)
                .ToListAsync(stoppingToken);

            List<ClassModulesWorker> classModulesWorkers = await dbContext.ClassModulesWorkers
                .Include(c => c.Class)
                .Include(c => c.Role)
                .Where(c => c.StartDate <= dateNow && c.EndDate >= dateNow)
                .ToListAsync(stoppingToken);

            List<Worker> workers = await dbContext.Workers.ToListAsync(stoppingToken);

            foreach (Class classEntity in classes)
            {
                DateOnly startDateClass = classEntity.StartDate;
                DateOnly endDateClass = classEntity.EndDate ?? dateNow;

                List<DateOnly> relevantDates = new();

                for (DateOnly date = startDateClass; date <= endDateClass; date = date.AddDays(1))
                {
                    DayOfWeek dayOfWeek = date.DayOfWeek;
                    bool hasSession = classEntity.Session.Details.Any(sd => sd.DayOfWeek == dayOfWeek);
                    if (hasSession && !holidayService.Contains(date))
                    {
                        relevantDates.Add(date);
                    }
                }

                if (!relevantDates.Contains(dateNow))
                {
                    continue;
                }
                SessionDetail? session = classEntity.Session.Details.Where(sd => sd.DayOfWeek == dateNow.DayOfWeek).FirstOrDefault();
                if (session == null && holidayService.Contains(dateNow))
                {
                    continue;
                }
                if (session?.EndTime.Hour != hour)
                {
                    continue;
                }
                ClassTimeSheet? classTimeSheet = classTimeSheets
                    .Where(cts => cts.ClassId == classEntity.Id).FirstOrDefault();

                if (classTimeSheet == null)
                {
                    ClassModulesWorker? mentor = classModulesWorkers
                        .Where(c => c.ClassId == classEntity.Id && c.StartDate <= dateNow && c.EndDate >= dateNow && c.Role!.Name == "mentor")
                        .FirstOrDefault();

                    if (mentor == null)
                    {
                        continue;
                    }

                    Worker? worker = workers
                        .Where(c => c.Id == mentor.WorkerId)
                        .FirstOrDefault();

                    if (worker == null)
                    {
                        continue;
                    }

                    ClassModulesWorker? teacher = classModulesWorkers
                        .Where(c => c.ClassId == classEntity.Id && c.StartDate <= dateNow && c.EndDate >= dateNow && c.Role!.Name == "muellim")
                        .FirstOrDefault();

                    if (teacher == null)
                    {
                        continue;
                    }

                    Worker? workerTeacher = workers
                        .Where(c => c.Id == teacher.WorkerId)
                        .FirstOrDefault();

                    if (workerTeacher == null)
                    {
                        continue;
                    }

                    unitOfWorkService.TelegramService.SendMessage($"Davamiyyət {DateTime.Now:dddd, dd MMMM yyyy}: {teacher.Class.Name} \n Müellim: {workerTeacher.Name} {workerTeacher.Email}");

                    await unitOfWorkService.EmailService.SendMessageAsync("https://studio.code.az", mentor.Class.Name, worker.Name ?? "", worker.Email, "EmailAttendanceTemplate.html", "Studio - Davamiyyət");
                    await unitOfWorkService.EmailService.SendMessageAsync("https://studio.code.az", teacher.Class.Name, workerTeacher.Name ?? "", workerTeacher.Email, "EmailAttendanceTemplate.html", "Studio - Davamiyyət");
                }
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}