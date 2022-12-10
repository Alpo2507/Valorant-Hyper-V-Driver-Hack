
namespace HyperVLauncher.Services.Monitor
{
    public class Worker : BackgroundService
    {
        private Task? _monitorTask;

        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _monitorTask = Task.Run(async () =>
            {
                var monitorService = ActivatorUtilities
                    .CreateInstance<MonitorService>(
                        _serviceProvider,
                        cancellationToken);

                await monitorService.Run();
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);

            if (_monitorTask is not null)
            {
                await _monitorTask;
            }
        }
    }
}
