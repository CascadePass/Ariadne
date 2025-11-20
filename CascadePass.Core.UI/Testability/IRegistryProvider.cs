using Microsoft.Win32;
using System;

namespace CascadePass.Core.UI
{
    public interface IRegistryProvider
    {
        event EventHandler<RegistryAccessEventArgs> RegistryAccessed;

        bool DeleteValue(string keyName, string valueName);
        string[] GetSubKeyNames(string keyName);
        object GetValue(string keyName, string valueName);
        string[] GetValueNames(string keyName);
        bool SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind);
    }
}
