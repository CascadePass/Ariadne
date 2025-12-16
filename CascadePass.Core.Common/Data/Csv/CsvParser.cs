using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;

namespace CascadePass.Core.Common.Data.Csv
{
    /// <summary>
    /// Provides functionality to parse CSV data into structured objects.
    /// </summary>
    /// <remarks>
    /// The <see cref="CsvParser"/> reads CSV input according to the rules defined in
    /// <see cref="CsvOptions"/> and produces both a strongly-typed column list and a
    /// <see cref="DataTable"/> representation for tabular access.
    /// </remarks>
    public class CsvParser : CsvProvider
    {
        /// <summary>
        /// Gets or sets the collection of columns discovered in the parsed CSV data.
        /// </summary>
        /// <remarks>
        /// Each <see cref="CsvColumn"/> represents metadata about a column, such as its
        /// name, index, and inferred type. This collection is populated after parsing.
        /// </remarks>
        public List<CsvColumn> Columns { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DataTable"/> containing the parsed CSV rows.
        /// </summary>
        /// <remarks>
        /// The <see cref="DataTable"/> provides a tabular representation of the CSV data,
        /// with rows and columns accessible via standard ADO.NET APIs. This property is
        /// populated after parsing completes.
        /// </remarks>
        public DataTable Table { get; set; }


        public static CsvParser Parse(string rawText, CsvOptions csvOptions)
        {
            CsvParser parser = new()
            {
                CsvOptions = csvOptions,
            };

            parser.Parse(rawText);
            return parser;
        }

        /// <summary>
        /// Parses raw CSV text into a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="rawText">
        /// The raw CSV content as a single string. This should include all rows and
        /// columns, with delimiters, quotes, and line endings defined by the
        /// associated <see cref="CsvOptions"/>.
        /// </param>
        /// <returns>
        /// A <see cref="DataTable"/> containing the parsed CSV data, with rows and
        /// columns populated according to the configured <see cref="CsvOptions"/>.
        /// </returns>
        /// <remarks>
        /// - The parser respects the delimiter, quote, escape, and line ending rules
        ///   defined in <see cref="CsvOptions"/>.
        /// - If <see cref="CsvOptions.FirstRowAsHeader"/> is <c>true</c>, the first row
        ///   will be used to name the columns in the <see cref="DataTable"/>.
        /// - If <see cref="CsvOptions.PreserveWhitespace"/> is <c>false</c>, leading and
        ///   trailing whitespace in fields will be trimmed.
        /// </remarks>
        /// <example>
        /// <code>
        /// var options = CsvOptions.Default;
        /// var parser = new CsvParser(options);
        /// var table = parser.Parse("Name,Age\nAlice,30\nBob,25");
        ///
        /// // Access data
        /// Console.WriteLine(table.Rows[0]["Name"]); // Outputs "Alice"
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="rawText"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        /// Thrown if the CSV content is malformed and cannot be parsed.
        /// </exception>
        public DataTable Parse(string rawText)
        {
            #region Guard clauses

            ArgumentNullException.ThrowIfNullOrWhiteSpace(rawText, nameof(rawText));

            if (string.IsNullOrEmpty(this.CsvOptions.Delimiter))
            {
                throw new InvalidOperationException("CSV Separator cannot be null or empty.");
            }

            if (this.CsvOptions.LineEndings is null || this.CsvOptions.LineEndings.Length == 0 || this.CsvOptions.LineEndings.Any(e => string.IsNullOrEmpty(e)))
            {
                throw new InvalidOperationException("CSV LineEndings cannot be null or empty.");
            }

            #endregion

            this.Columns = [];
            this.Table = new();

            this.IsWorking = true;
            var lines = rawText.Split(this.CsvOptions.LineEndings, StringSplitOptions.None)
                                    .SkipWhile(string.IsNullOrWhiteSpace)
                                    .Reverse()
                                    .SkipWhile(string.IsNullOrWhiteSpace)
                                    .Reverse()
                                    .ToArray();

            if (lines.Length > 0)
            {
                var headers = this.SplitCsvLine(lines[0], this.CsvOptions.Delimiter);

                int columnIndex = 0;
                foreach (string header in headers)
                {
                    CsvColumn csvColumn = new() { Name = this.GenerateColumnName(header, ++columnIndex) };

                    this.Columns.Add(csvColumn);
                    this.Table.Columns.Add(csvColumn.Name);
                }

                for (int i = (this.CsvOptions.FirstRowAsHeader ? 1 : 0); i < lines.Length; i++)
                {
                    var fields = this.SplitCsvLine(lines[i], this.CsvOptions.Delimiter);

                    if (fields.Count == headers.Count)
                    {
                        DataRow row = this.Table.NewRow();
                        for (int j = 0; j < headers.Count; j++)
                        {
                            row[j] = this.GetValue(fields[j]);
                        }

                        this.Table.Rows.Add(row);
                    }
                }
            }


            for (int colIndex = 0; colIndex < this.Table.Columns.Count; colIndex++)
            {
                // We're going to need a way to scan the first 50 rows or something because
                // scanning the entire dataset could be slow for large files.  For now, though,
                // this is fine.
                var values = this.Table.Rows.Cast<DataRow>().Select(r => r[colIndex]?.ToString() ?? string.Empty);
                var column = this.Columns[colIndex];

                column.DataType = InferDataType(values, out bool allowBlanks);
                column.AllowBlank = allowBlanks;
            }

            this.IsWorking = false;
            return this.Table;
        }

        internal string GenerateColumnName(string firstData, int columnIndex)
        {
            string name = string.Empty;

            if(this.CsvOptions.FirstRowAsHeader)
            {
                name = firstData;
            }
            else if (this.DesiredColumns.Count >= columnIndex && !string.IsNullOrWhiteSpace(this.DesiredColumns[columnIndex].Name))
            {
                name = this.DesiredColumns[columnIndex].Name;
            }

            string resultingName = name;
            int columnWithNameCount = 0;
            while (this.Columns.Any(c => c.Name == resultingName))
            {
                resultingName = $"{name}{++columnWithNameCount}";
            }

            return resultingName;
        }

        internal string GetValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            string result = this.CsvOptions.PreserveWhitespace ? value : value.Trim();

            return result;
        }

        internal List<string> SplitCsvLine(string line, string separator)
        {
            List<string> fields = [];
            StringBuilder current = new();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        // Escaped quote
                        current.Append('"');
                        i++; // skip the second quote
                    }
                    else
                    {
                        // Toggle quote state
                        inQuotes = !inQuotes;
                    }
                }
                else if (!inQuotes && c.ToString() == separator)
                {
                    // End of field
                    fields.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            fields.Add(current.ToString());
            return fields;
        }

        internal SupportedDataType InferDataType(IEnumerable<string> values, out bool allowBlank)
        {
            allowBlank = false;

            bool allInt = true, allFloat = true, allDate = true, allTime = true, allDateTime = true, allBool = true;

            foreach (var value in values)
            {
                string v = this.CsvOptions.PreserveWhitespace ? value : value.Trim();

                if (string.IsNullOrEmpty(v))
                {
                    allowBlank = true;
                    continue;
                }

                if (!int.TryParse(v, out _)) allInt = false;
                if (!double.TryParse(v, out _)) allFloat = false;

                if (!DateTime.TryParse(v, out var dt))
                {
                    allDate = false; allTime = false; allDateTime = false;
                }
                else
                {
                    if (dt.TimeOfDay.TotalSeconds > 0) allDate = false;
                    if (dt.Date != DateTime.MinValue.Date) allTime = false;
                }

                if (!bool.TryParse(v, out _)) allBool = false;
            }

            if (allInt) return SupportedDataType.Integer;
            if (allFloat) return SupportedDataType.Float;
            if (allDateTime) return SupportedDataType.DateTime;
            if (allDate) return SupportedDataType.Date;
            if (allTime) return SupportedDataType.Time;
            if (allBool) return SupportedDataType.Boolean;

            return SupportedDataType.Text;
        }
    }
}
