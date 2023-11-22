using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Infrastructure.Services.SendEmailService
{
	public class EmailServiceLauncher : IHostedService
	{
		private readonly IServiceProvider _serviceProvider;

		public EmailServiceLauncher(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			var scope = _serviceProvider.CreateScope();
			var sendEmailService = scope.ServiceProvider.GetRequiredService<SendEmailService>();
			await sendEmailService.StartAsync(cancellationToken);

		}
		public async Task StopAsync(CancellationToken cancellationToken)
		{
			var scope = _serviceProvider.CreateScope();
			var sendEmailService = scope.ServiceProvider.GetRequiredService<SendEmailService>();
			await sendEmailService.StopAsync(cancellationToken);

		}
	}
}
