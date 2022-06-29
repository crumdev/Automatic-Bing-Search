using System;
using System.Threading;
using System.Threading.Tasks;
using Automation.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Automation
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMethods _webworker;

        public Worker(ILogger<Worker> logger, IOptions<AutomationOptions> options, IMethods webworker)
        {
            _logger = logger;
            _webworker = webworker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _webworker.SearchWebInDesktopMode();
            _webworker.SearchWebInMobileMode();
            _logger.LogInformation("Worker finished at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
