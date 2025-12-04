using System;

namespace CascadePass.Core.Common.Data.Csv
{
    /// <summary>
    /// Provides data for the <see cref="CsvProvider.OptionsChangeIgnored"/> event.
    /// Contains the attempted options and the reason the change was ignored.
    /// </summary>
    public class OptionsChangeIgnoredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsChangeIgnoredEventArgs"/> class.
        /// </summary>
        /// <param name="attemptedOptions">The options that were attempted to be set.</param>
        /// <param name="reason">The reason the change was ignored.</param>
        public OptionsChangeIgnoredEventArgs(CsvOptions attemptedOptions, ReasonType reason)
        {
            this.AttemptedOptions = attemptedOptions;
            this.Reason = reason;
        }

        /// <summary>
        /// Gets the options instance that was attempted to be set.
        /// May be <c>null</c> if the caller attempted to assign a null value.
        /// </summary>
        public CsvOptions AttemptedOptions { get; }

        /// <summary>
        /// Gets the reason the attempted change was ignored.
        /// </summary>
        public ReasonType Reason { get; }

        /// <summary>
        /// Enumerates the possible reasons why an options change was ignored.
        /// </summary>
        public enum ReasonType
        {
            /// <summary>
            /// The reason could not be determined.
            /// </summary>
            Unknown,

            /// <summary>
            /// The provider was working and live changes were not allowed.
            /// </summary>
            OptionsChangeNotAllowedWhileWorking,

            /// <summary>
            /// A null options value was supplied, which is not permitted.
            /// </summary>
            NullOptionsNotAllowed,
        }
    }
}
