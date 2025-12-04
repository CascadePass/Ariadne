using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CascadePass.Core.Common.Data.Csv
{
    public class CsvParser : CsvProvider
    {
        public List<CsvColumn> Columns { get; set; }

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

        public DataTable Parse(string rawText)
        {
            #region Guard clauses

            ArgumentNullException.ThrowIfNullOrWhiteSpace(rawText, nameof(rawText));

            if (string.IsNullOrEmpty(this.CsvOptions.Separator))
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
                var headers = this.SplitCsvLine(lines[0], this.CsvOptions.Separator);

                int columnIndex = 0;
                foreach (string header in headers)
                {
                    CsvColumn csvColumn = new() { Name = this.CsvOptions.FirstRowAsHeader ? header : $"Column {++columnIndex}" };

                    int columnWithNameCount = 0;
                    while (this.Columns.Any(c => c.Name == csvColumn.Name))
                    {
                        csvColumn.Name = $"{header}{++columnWithNameCount}";
                    }

                    this.Columns.Add(csvColumn);
                    this.Table.Columns.Add(csvColumn.Name);
                }

                for (int i = (this.CsvOptions.FirstRowAsHeader ? 1 : 0); i < lines.Length; i++)
                {
                    var fields = this.SplitCsvLine(lines[i], this.CsvOptions.Separator);

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
