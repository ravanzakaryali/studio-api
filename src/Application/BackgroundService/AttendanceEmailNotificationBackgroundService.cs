using DocumentFormat.OpenXml.Math;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

            List<ClassSession> classSessions = await dbContext.ClassSessions
                            .Where(c => c.Date == dateNow)
                            .ToListAsync(stoppingToken);

            List<ClassSession> classSessionsEnd = classSessions
                            .Where(c => c.EndTime.Hour == hour)
                            .ToList();

            foreach (ClassSession classSession in classSessionsEnd)
            {
                if (classSession.Status == ClassSessionStatus.Cancelled)
                {
                    continue;
                }

                if (classSession.ClassTimeSheetId == null)
                {
                    ClassModulesWorker? classModulesWorker = await dbContext.ClassModulesWorkers
                            .Include(c => c.Class)
                            .Include(c => c.Role)
                            .Where(c => c.ClassId == classSession.ClassId && c.StartDate <= dateNow && c.EndDate >= dateNow && c.Role!.Name == "Mentor")
                            .FirstOrDefaultAsync(stoppingToken);

                    if (classModulesWorker == null)
                    {
                        continue;
                    }

                    Worker? worker = await dbContext.Workers
                        .Where(c => c.Id == classModulesWorker.WorkerId)
                        .FirstOrDefaultAsync(stoppingToken);

                    if (worker == null)
                    {
                        continue;
                    }

                    ClassModulesWorker? classModulesWorkerMuellim = await dbContext.ClassModulesWorkers
                                                .Include(c => c.Class)
                                                .Include(c => c.Role)
                                                .Where(c => c.ClassId == classSession.ClassId && c.StartDate <= dateNow && c.EndDate >= dateNow && c.Role!.Name == "Muellim")
                                                .FirstOrDefaultAsync(stoppingToken);

                    if (classModulesWorkerMuellim == null)
                    {
                        continue;
                    }
                    Worker? workerMuellim = await dbContext.Workers
                            .Where(c => c.Id == classModulesWorkerMuellim.WorkerId)
                            .FirstOrDefaultAsync(stoppingToken);

                    if (workerMuellim == null)
                    {
                        continue;
                    }

                    // unitOfWorkService.TelegramService.SendMessage($"Davamiyyət {DateTime.Now.ToString("dddd, dd MMMM yyyy")} : {classModulesWorker.Class.Name} \n Mentor: {worker.Name} {worker.Email}");
                    // unitOfWorkService.TelegramService.SendMessage($"Davamiyyət {DateTime.Now.ToString("dddd, dd MMMM yyyy")}: {classModulesWorkerMuellim.Class.Name} \n Müellim: {workerMuellim.Name} {workerMuellim.Email}");

                    // await unitOfWorkService.EmailService.SendMessageAsync("https://studio.code.az", classModulesWorker.Class.Name, worker.Name ?? "", worker.Email, "EmailAttendanceTemplate.html", "Studio - Davamiyyət");
                    // await unitOfWorkService.EmailService.SendMessageAsync("https://studio.code.az", classModulesWorkerMuellim.Class.Name, workerMuellim.Name ?? "", workerMuellim.Email, "EmailAttendanceTemplate.html", "Studio - Davamiyyət");

                }
            }
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
