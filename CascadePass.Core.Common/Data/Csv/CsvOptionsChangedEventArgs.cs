using System;

namespace CascadePass.Core.Common.Data.Csv
{
    /// <summary>
    /// Provides data for the <see cref="CsvProvider.OptionsChanged"/> event.
    /// Contains both the old and new <see cref="CsvOptions"/> instances.
    /// </summary>
    public class CsvOptionsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvOptionsChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldOptions">The previous options instance.</param>
        /// <param name="newOptions">The new options instance.</param>
        public CsvOptionsChangedEventArgs(CsvOptions oldOptions, CsvOptions newOptions)
        {
            OldOptions = oldOptions;
            NewOptions = newOptions;
        }

        /// <summary>
        /// Gets the previous options instance before the change occurred.
        /// </summary>
        public CsvOptions OldOptions { get; }

        /// <summary>
        /// Gets the new options instance applied to the provider.
        /// </summary>
        public CsvOptions NewOptions { get; }
    }
}
