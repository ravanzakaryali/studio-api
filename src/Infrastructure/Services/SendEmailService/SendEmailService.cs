using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Infrastructure.Services.SendEmailService
{
	public class SendEmailService : BackgroundService
	{
		private readonly IUnitOfWork _unitOfWork;
		readonly ISpaceDbContext _spaceDbContext;

		public SendEmailService(IUnitOfWork unitOfWork, ISpaceDbContext spaceDbContext)
		{
			_unitOfWork = unitOfWork;
			_spaceDbContext = spaceDbContext;
		}

		private async Task SendEmail()
		{
			var roleIdentifier = "Mentor";
			var allClasses = _spaceDbContext.Classes
					.Include(c => c.ClassSessions)
						.ThenInclude(cs => cs.AttendancesWorkers)
							.ThenInclude(aw => aw.Worker)
					.Include(c => c.ClassModulesWorkers)
						.ThenInclude(cw => cw.Worker)
					.Select(c => new
					{
						ClassName = c.Name,
						MentorName = c.ClassModulesWorkers.First(cw => cw.Role!.Name == roleIdentifier).Worker.Name,
						MentorEmail = c.ClassModulesWorkers.First(cw => cw.Role!.Name == roleIdentifier).Worker.Email,
						Sessions = c.ClassSessions.Select(cs => new
						{
							SessionStartTime = cs.StartTime,
							SessionEndTime = cs.EndTime,
							Date = cs.Date,
							SessionTotalHours = cs.TotalHour,
						}).ToList()
					}).ToList();

			foreach (var classs in allClasses)
			{
				foreach (var classSession in classs.Sessions)
				{
					if (classSession.Date.Day == DateTime.Now.Day)
					{
						var currentTime = TimeOnly.FromDateTime(DateTime.Now);

						if (classSession.SessionEndTime - currentTime <= TimeSpan.FromMinutes(30))
						{
							string subject = $" Xatırlatma: {classs.ClassName}";
							string body = $"Zəhmət olmasa davamiyyeti doldurun " +
								$"Dərsin bitməsinə 15 dəqiqə qalir";
							await _unitOfWork.EmailService.SendMessageAsync(body, classs.MentorEmail, "EmailNotificationTemplate.html", subject);
						}
					}
				}
			}
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await SendEmail();
				await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

			}
		}
	}
}
