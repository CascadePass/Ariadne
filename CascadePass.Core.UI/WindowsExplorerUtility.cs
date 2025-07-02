using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CascadePass.Core.UI
{
    public static class WindowsExplorerUtility
    {
        public static bool LaunchFile(string filename)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = filename,
                UseShellExecute = true
            };

            return WindowsExplorerUtility.StartProcess(startInfo);
        }

        public static bool BrowseToFolder(string path)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = path,
                UseShellExecute = true
            };

            return WindowsExplorerUtility.StartProcess(startInfo);
        }

        public static bool BrowseToFile(string filePath)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"/select,\"{filePath}\"",
                UseShellExecute = true
            };

            return WindowsExplorerUtility.StartProcess(startInfo);
        }

        private static bool StartProcess(ProcessStartInfo psi)
        {
            try
            {
                Process.Start(psi);
            }
            catch (Win32Exception) { return false; }
            catch (ObjectDisposedException) { return false; }
            catch (InvalidOperationException) { return false; }

            return true;
        }
    }
}