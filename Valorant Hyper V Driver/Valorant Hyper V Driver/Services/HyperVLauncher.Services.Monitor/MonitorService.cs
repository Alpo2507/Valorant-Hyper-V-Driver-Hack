using HyperVLauncher.Contracts.Models;
using HyperVLauncher.Contracts.Interfaces;

using HyperVLauncher.Providers.Tracing;

namespace HyperVLauncher.Services.Monitor
{
    public class MonitorService
    {
        private readonly IHyperVProvider _hyperVProvider;
        private readonly ITrayIpcProvider _trayIpcProvider;
        private readonly IShortcutProvider _shortcutProvider;
        private readonly ISettingsProvider _settingsProvider;
        private readonly CancellationToken _cancellationToken;

        public MonitorService(
            IHyperVProvider hyperVProvider,
            ITrayIpcProvider trayIpcProvider,
            ISettingsProvider settingsProvider,
            IShortcutProvider shortcutProvider,
            CancellationToken cancellationToken)
        {
            _hyperVProvider = hyperVProvider;
            _trayIpcProvider = trayIpcProvider;
            _shortcutProvider = shortcutProvider;
            _settingsProvider = settingsProvider;
            _cancellationToken = cancellationToken;
        }

        public async Task Run()
        {
            await CheckForInvalidShortcuts();

            _hyperVProvider.StartVirtualMachineCreatedMonitor(_cancellationToken);
            _hyperVProvider.StartVirtualMachineDeletedMonitor(_cancellationToken);

            _hyperVProvider.OnVirtualMachineCreated = OnVirtualMachineCreated;
            _hyperVProvider.OnVirtualMachineDeleted = OnVirtualMachineDeleted;
        }

        private async Task CheckForInvalidShortcuts()
        {
            var appSettings = await _settingsProvider.Get(true);

            if (!appSettings.AutoDeleteShortcuts)
            {
                return;
            }

            Tracer.Info("Checking for invalid shortcuts...");

            var vmList = _hyperVProvider.GetVirtualMachineList().ToList();

            var vmIdsToDelete = new HashSet<string>();

            foreach (var shortcut in appSettings.Shortcuts)
            {
                if (vmList.FirstOrDefault(x => x.Id == shortcut.VmId) is null)
                {
                    vmIdsToDelete.Add(shortcut.VmId);
                }
            }

            foreach (var vmIdToDelete in vmIdsToDelete)
            {
                await _settingsProvider.DeleteVirtualMachineShortcuts(
                    vmIdToDelete, 
                    _trayIpcProvider, 
                    _shortcutProvider);
            }
        }

        public async Task OnVirtualMachineCreated(VirtualMachine vm)
        {
            Tracer.Info($"New Virtual Machine detected: {vm.Id} - {vm.Name}");

            var appSettings = await _settingsProvider.Get(true);

            if (appSettings.AutoCreateShortcuts)
            {
                await _settingsProvider.ProcessCreateShortcut(
                    vm.Id,
                    vm.Name,
                    _trayIpcProvider,
                    _shortcutProvider);
            }
            else if (appSettings.NotifyOnNewVm)
            {
                await _trayIpcProvider.SendShowShortcutPromptNotif(
                    vm.Id,
                    vm.Name);
            }
        }

        public async Task OnVirtualMachineDeleted(VirtualMachine vm)
        {
            Tracer.Info($"Deleted Virtual Machine detected: {vm.Id} - {vm.Name}");

            var appSettings = await _settingsProvider.Get(true);

            if (appSettings.AutoDeleteShortcuts)
            {
                await _settingsProvider.DeleteVirtualMachineShortcuts(
                    vm.Id,
                    _trayIpcProvider,
                    _shortcutProvider);
            }
        }
    }
}
