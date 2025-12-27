namespace CascadePass.Core.Common.Data.Csv
{
    /// <summary>
    /// Represents a column definition within a CSV schema, including its name,
    /// data type, validation rules, and optional growth‑series behavior.
    /// </summary>
    public class CsvColumn
    {
        /// <summary>
        /// Gets or sets the column name as it appears in the CSV file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the expected data type for values in this column.
        /// Used to validate and parse incoming CSV data.
        /// </summary>
        public SupportedDataType DataType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column represents a
        /// growth series, where each row's value is derived from a growth rate.
        /// </summary>
        public bool IsGrowthSeries { get; set; }

        /// <summary>
        /// Gets or sets the growth rate applied when <see cref="IsGrowthSeries"/> is true.
        /// Represents the percentage or numeric increment used to compute series values.
        /// </summary>
        public int GrowthRate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether blank or empty values are permitted
        /// for this column during CSV parsing or validation.
        /// </summary>
        public bool AllowBlank { get; set; }
    }

}
