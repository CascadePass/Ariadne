using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace CascadePass.Core.Common
{
    public static class InstanceMonitor
    {
        private static string processName;
        private static int lastCount;
        private static Timer timer;

        public static event EventHandler<int> InstanceCountChanged;
        public static event EventHandler InstanceStarted;
        public static event EventHandler InstanceExited;

        public static string ProcessName => InstanceMonitor.processName ??= Process.GetCurrentProcess()?.ProcessName;

        public static Process Current => Process.GetCurrentProcess();

        public static Process[] Other =>
            Process.GetProcessesByName(InstanceMonitor.ProcessName)
                   .Where(p => p.Id != InstanceMonitor.Current.Id)
                   .ToArray();

        public static int InstanceCount => InstanceMonitor.lastCount = Process.GetProcessesByName(InstanceMonitor.ProcessName).Length;

        public static bool IsMultipleInstancesRunning => InstanceMonitor.InstanceCount > 1;

        public static bool IsOnlyInstance => InstanceMonitor.InstanceCount == 1;

        public static bool IsPolling { get; private set; }

        public static void StartMonitoring(int intervalMs = 1000)
        {
            if (InstanceMonitor.IsPolling)
            {
                return;
            }

            InstanceMonitor.IsPolling = true;
            lastCount = Process.GetProcessesByName(ProcessName).Length;
            timer ??= new Timer(CheckInstances, null, 0, intervalMs);
        }

        public static void StopMonitoring()
        {
            timer?.Dispose();
            timer = null;
            InstanceMonitor.IsPolling = false;
        }

        private static void CheckInstances(object state)
        {
            int currentCount = Process.GetProcessesByName(InstanceMonitor.ProcessName).Length;

            if (currentCount != lastCount)
            {
                InstanceCountChanged?.Invoke(typeof(InstanceMonitor), currentCount);

                if (currentCount > lastCount)
                    InstanceStarted?.Invoke(typeof(InstanceMonitor), EventArgs.Empty);
                else if (currentCount < lastCount)
                    InstanceExited?.Invoke(typeof(InstanceMonitor), EventArgs.Empty);

                lastCount = currentCount;
            }
        }
    }
}
