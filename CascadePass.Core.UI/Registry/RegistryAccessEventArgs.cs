using Microsoft.Win32;
using System;

namespace CascadePass.Core.UI
{
    /// <summary>
    /// Provides data for events related to Windows Registry access operations, 
    /// including key and value names, access type, outcome, and any associated exception.
    /// </summary>
    public class RegistryAccessEventArgs : EventArgs
    {
        #region Constructors

        internal RegistryAccessEventArgs(
            RegistryHive hive,
            string keyName,
            RegistryAccessType accessType)
        {
            this.Hive = hive;
            this.KeyName = keyName;
            this.AccessType = accessType;
        }

        internal RegistryAccessEventArgs(
            RegistryHive hive,
            string keyName,
            string valueName,
            RegistryAccessType accessType)
        {
            this.Hive = hive;
            this.KeyName = keyName;
            this.ValueName = valueName;
            this.AccessType = accessType;
        }

        internal RegistryAccessEventArgs(
            RegistryHive hive,
            string keyName,
            RegistryAccessType accessType,
            Exception exception)
        {
            this.Hive = hive;
            this.KeyName = keyName;
            this.AccessType = accessType;
            this.Exception = exception;
        }

        internal RegistryAccessEventArgs(
            RegistryHive hive,
            string keyName,
            string valueName,
            RegistryAccessType accessType,
            Exception exception)
        {
            this.Hive = hive;
            this.KeyName = keyName;
            this.ValueName = valueName;
            this.AccessType = accessType;
            this.Exception = exception;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the registry hive involved in the operation.
        /// </summary>
        public RegistryHive Hive { get; }

        /// <summary>
        /// Gets the name of the registry key involved in the operation.
        /// </summary>
        public string KeyName { get; }

        /// <summary>
        /// Gets the name of the value within the registry key that was accessed or modified.
        /// </summary>
        public string ValueName { get; }

        /// <summary>
        /// Gets the type of registry access operation that was performed.
        /// </summary>
        public RegistryAccessType AccessType { get; }

        /// <summary>
        /// Gets a value indicating whether the registry operation completed without throwing an exception.
        /// </summary>
        public bool WasSuccessful => this.Exception == null;

        /// <summary>
        /// Gets the exception thrown during the registry operation, if any. 
        /// Returns <c>null</c> if the operation was successful.
        /// </summary>
        public Exception Exception { get; }

        #endregion
    }
}
