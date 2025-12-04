using System;

namespace CascadePass.Core.Common.Data.Csv
{
    /// <summary>
    /// Provides a common base for CSV parsing and writing operations.
    /// Manages <see cref="CsvOptions"/> and exposes events for option changes
    /// or ignored attempts to change options.
    /// </summary>
    public abstract class CsvProvider
    {
        private CsvOptions csvOptions;

        /// <summary>
        /// Occurs when <see cref="CsvOptions"/> is successfully changed to a new instance.
        /// </summary>
        public event EventHandler<CsvOptionsChangedEventArgs> OptionsChanged;

        /// <summary>
        /// Occurs when an attempt to change <see cref="CsvOptions"/> is ignored.
        /// This can happen if the provider is currently working and live changes are not allowed,
        /// or if a null value was supplied.
        /// </summary>
        public event EventHandler<OptionsChangeIgnoredEventArgs> OptionsChangeIgnored;

        /// <summary>
        /// Gets or sets the options used for CSV operations.
        /// <para>
        /// If not explicitly set, defaults to <see cref="CsvOptions.Default"/>.
        /// </para>
        /// <para>
        /// Setting this property to <c>null</c> or attempting to change it while
        /// <see cref="IsWorking"/> is <c>true</c> and <see cref="AllowLiveOptionsChange"/> is <c>false</c>
        /// will be ignored, and <see cref="OptionsChangeIgnored"/> will be raised.
        /// </para>
        /// </summary>
        public CsvOptions CsvOptions
        {
            get => this.csvOptions ??= CsvOptions.Default;
            set
            {
                bool illegal = this.IsWorking && !this.AllowLiveOptionsChange;
                bool empty = value is null;

                if (illegal || empty)
                {
                    OptionsChangeIgnoredEventArgs.ReasonType reason = illegal ?
                        OptionsChangeIgnoredEventArgs.ReasonType.OptionsChangeNotAllowedWhileWorking
                        : OptionsChangeIgnoredEventArgs.ReasonType.NullOptionsNotAllowed;

                    this.OnOptionsChangeIgnored(value, reason);
                    return;
                }

                var oldOptions = this.csvOptions;
                this.csvOptions = value;

                // Fire event if options actually changed
                if (!ReferenceEquals(oldOptions, this.csvOptions))
                {
                    this.OnOptionsChanged(oldOptions, this.csvOptions);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether live changes to <see cref="CsvOptions"/>
        /// are allowed while the provider is working.
        /// </summary>
        public bool AllowLiveOptionsChange { get; set; }

        /// <summary>
        /// Gets a value indicating whether the provider is currently performing
        /// a CSV operation (such as parsing or writing).
        /// </summary>
        public bool IsWorking { get; internal set; }

        /// <summary>
        /// Raises the <see cref="OptionsChangeIgnored"/> event with the specified attempted options and reason.
        /// </summary>
        /// <param name="attemptedOptions">The options that were attempted to be set.</param>
        /// <param name="reason">The reason the change was ignored.</param>
        protected void OnOptionsChangeIgnored(CsvOptions attemptedOptions, OptionsChangeIgnoredEventArgs.ReasonType reason)
        {
            this.OptionsChangeIgnored?.Invoke(this, new OptionsChangeIgnoredEventArgs(attemptedOptions, reason));
        }

        /// <summary>
        /// Raises the <see cref="OptionsChanged"/> event with the specified old and new options.
        /// </summary>
        /// <param name="oldOptions">The previous options instance.</param>
        /// <param name="newOptions">The new options instance.</param>
        private void OnOptionsChanged(CsvOptions oldOptions, CsvOptions newOptions)
        {
            this.OptionsChanged?.Invoke(this, new CsvOptionsChangedEventArgs(oldOptions, newOptions));
        }
    }
}
