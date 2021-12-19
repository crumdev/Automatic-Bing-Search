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
        private readonly Methods _webworker;

        public Worker(ILogger<Worker> logger, IOptions<AutomationOptions> options)
        {
            _logger = logger;
            _webworker = new Methods(options);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _webworker.SearchWebInDesktopMode();
            _webworker.SearchWebInMobileMode();
            _webworker.CleanUp();
            await Task.Delay(1000, stoppingToken);
        }
    }
}
