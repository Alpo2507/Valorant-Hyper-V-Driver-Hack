using HyperVLauncher.Contracts.Constants;
using HyperVLauncher.Contracts.Interfaces;

using HyperVLauncher.Providers.Ipc;
using HyperVLauncher.Providers.Path;
using HyperVLauncher.Providers.HyperV;
using HyperVLauncher.Providers.Tracing;
using HyperVLauncher.Providers.Settings;
using HyperVLauncher.Providers.Shortcut;

using HyperVLauncher.Services.Monitor;

var pathProvider = new PathProvider(GeneralConstants.ProfileName);
pathProvider.CreateDirectories();

TracingProvider.Init(pathProvider.GetTracingPath(), "Monitor");

Tracer.Info("Starting Virtual Machine monitor service...");

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IHyperVProvider, HyperVProvider>();
        services.AddSingleton<ISettingsProvider, SettingsProvider>();
        services.AddSingleton<IShortcutProvider, ShortcutProvider>();

        services.AddSingleton<IPathProvider>(provider => pathProvider);
        services.AddSingleton<ITrayIpcProvider>(provider => new IpcProvider(GeneralConstants.TrayIpcPipeName));
    });

if (Environment.UserInteractive)
{
    await hostBuilder
        .RunConsoleAsync();
}
else
{
    await hostBuilder
        .UseWindowsService()
        .Build()
        .RunAsync();
}

Tracer.Info("Stopped Virtual Machine monitor service.");

