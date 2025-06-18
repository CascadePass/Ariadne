using Microsoft.Win32;

namespace CascadePass.CascadeCore.UI
{
    public class RegistryProvider : IRegistryProvider
    {
        public object GetValue(string keyName, string valueName)
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName);

            key?.Flush();
            return key?.GetValue(valueName);
        }
    }
}
