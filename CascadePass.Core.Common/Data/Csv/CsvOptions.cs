namespace CascadePass.Core.Common.Data.Csv
{
    /// <summary>
    /// Provides configuration options for CSV parsing and writing.
    /// </summary>
    /// <remarks>
    /// This class defines how CSV data is interpreted and written, including delimiters,
    /// line endings, quoting, escaping, and header handling.
    /// </remarks>
    public class CsvOptions
    {
        /// <summary>
        /// Gets or sets the set of line endings recognized in the CSV input.
        /// </summary>
        /// <remarks>
        /// Common values include <see cref="WindowsLineEnding"/>, <see cref="UnixLineEnding"/>,
        /// and <see cref="MacOldLineEnding"/>. Multiple endings can be specified to support
        /// mixed-platform files.
        /// </remarks>
        public string[] LineEndings { get; set; }

        /// <summary>
        /// Gets or sets the delimiter string used to separate fields.
        /// </summary>
        /// <example>
        /// For standard CSV files, use ",". For TSV files, use "\t".
        /// </example>
        public string Delimiter { get; set; }

        /// <summary>
        /// Gets or sets the character used to quote fields containing delimiters or line breaks.
        /// </summary>
        /// <remarks>
        /// Typically a double quote (<c>"</c>).
        /// </remarks>
        public string QuoteCharacter { get; set; }

        /// <summary>
        /// Gets or sets the character used to escape quotes inside quoted fields.
        /// </summary>
        /// <remarks>
        /// Commonly a backslash (<c>\</c>).
        /// </remarks>
        public string EscapeCharacter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether leading and trailing whitespace
        /// in fields should be preserved.
        /// </summary>
        /// <remarks>
        /// If <c>false</c>, whitespace is trimmed during parsing.
        /// </remarks>
        public bool PreserveWhitespace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the first row of the CSV
        /// should be treated as a header row.
        /// </summary>
        /// <remarks>
        /// If <c>true</c>, the parser will use the first row to name columns.
        /// </remarks>
        public bool FirstRowAsHeader { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CsvOptions"/> instance
        /// is usable for parsing or writing.
        /// </summary>
        /// <remarks>
        /// Returns <c>true</c> if a delimiter is defined and at least one line ending
        /// is specified.
        /// </remarks>
        public bool IsUseable =>
            !string.IsNullOrWhiteSpace(this.Delimiter) &&
            this.LineEndings != null &&
            this.LineEndings.Length > 0;

        /// <summary>
        /// Gets a default <see cref="CsvOptions"/> instance with common settings.
        /// </summary>
        /// <remarks>
        /// Defaults to comma delimiter, double-quote quoting, backslash escaping,
        /// CRLF/LF/CR line endings, no whitespace preservation, and first row as header.
        /// </remarks>
        public static CsvOptions Default => new()
        {
            LineEndings = ["\r\n", "\n", "\r"],
            Delimiter = ",",
            QuoteCharacter = "\"",
            EscapeCharacter = "\\",
            PreserveWhitespace = false,
            FirstRowAsHeader = true,
        };

        /// <summary>
        /// Represents the Windows line ending (<c>\r\n</c>).
        /// </summary>
        public static readonly string[] WindowsLineEnding = ["\r\n"];

        /// <summary>
        /// Represents the Unix line ending (<c>\n</c>).
        /// </summary>
        public static readonly string[] UnixLineEnding = ["\n"];

        /// <summary>
        /// Represents the legacy Mac OS line ending (<c>\r</c>).
        /// </summary>
        public static readonly string[] MacOldLineEnding = ["\r"];
    }
}
