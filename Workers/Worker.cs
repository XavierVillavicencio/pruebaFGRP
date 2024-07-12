using Microsoft.EntityFrameworkCore;
using pruebaFGRP.Data;
using pruebaFGRP.Interfaces;

namespace pruebaFGRP
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IConfigData _configData;
        private string workerServiceKey = "WorkerServiceMaintenanceInterval";

        public Worker(ILogger<Worker> logger, IConfigData configData)
        {
            _logger = logger;
            _configData = configData;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    await _configData.GetConfigVar(workerServiceKey);
                }
                
            }
        }
    }
}
