using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace CascadePass.Core.UI
{
    public class RegistryProvider : IRegistryProvider
    {
        private readonly RegistryHive hive;

        public delegate void RegistryAccessedHandler(object sender, RegistryAccessEventArgs e);

        public event EventHandler<RegistryAccessEventArgs> RegistryAccessed;

        #region Constructors

        public RegistryProvider()
        {
            this.hive = RegistryHive.CurrentUser;
        }

        public RegistryProvider(RegistryHive targetHive)
        {
            this.hive = targetHive;
        }

        #endregion

        private RegistryKey OpenKey(string keyName, bool writable)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            return RegistryKey.OpenBaseKey(hive, RegistryView.Default).OpenSubKey(keyName, writable);
        }

        public object GetValue(string keyName, string valueName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            try
            {
                using var key = this.OpenKey(keyName, writable: false);
                object value = key?.GetValue(valueName);

                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Read));
                return value;
            }
            catch (Exception ex)
            {
                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Read, ex));
                return null;
            }
        }

        public bool SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            try
            {
                using var key = RegistryKey.OpenBaseKey(hive, RegistryView.Default).CreateSubKey(keyName);
                key?.SetValue(valueName, value, valueKind);

                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Write));
                return true;
            }
            catch (Exception ex)
            {
                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Write, ex));
                return false;
            }
        }

        public bool DeleteValue(string keyName, string valueName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            try
            {
                using var key = this.OpenKey(keyName, writable: true);
                key?.DeleteValue(valueName, throwOnMissingValue: false);

                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Delete));
                return true;
            }
            catch (Exception ex)
            {
                this.OnRegistryAccessed(new(this.hive, keyName, valueName, RegistryAccessType.Delete, ex));
                return false;
            }
        }

        public string[] GetSubKeyNames(string keyName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            try
            {
                using var key = this.OpenKey(keyName, writable: false);
                var result = key?.GetSubKeyNames() ?? [];

                this.OnRegistryAccessed(new(this.hive, keyName, RegistryAccessType.EnumerateKeys));
                return result;
            }
            catch (Exception ex)
            {
                this.OnRegistryAccessed(new(this.hive, keyName, RegistryAccessType.EnumerateKeys, ex));
                return null;
            }
        }

        public string[] GetValueNames(string keyName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException(null, nameof(keyName));
            }

            try
            {
                using var key = this.OpenKey(keyName, writable: false);
                var result = key?.GetValueNames() ?? [];

                this.OnRegistryAccessed(new(this.hive, keyName, RegistryAccessType.EnumerateValues));
                return result;
            }
            catch (Exception ex)
            {
                this.OnRegistryAccessed(new(this.hive, keyName, RegistryAccessType.EnumerateValues, ex));
                return null;
            }
        }

        protected virtual void OnRegistryAccessed(RegistryAccessEventArgs e)
        {
            this.RegistryAccessed?.Invoke(this, e);
            this.OnRegistryAccessedAsync(e);
        }

        protected virtual void OnRegistryAccessedAsync(RegistryAccessEventArgs e)
        {
            var handlers = this.RegistryAccessed;
            if (handlers != null)
            {
                foreach (EventHandler<RegistryAccessEventArgs> handler in handlers.GetInvocationList())
                {
                    _ = Task.Run(() => handler(this, e));
                }
            }
        }
    }
}
