using System.Data;
using System.IO;
using System.Linq;

namespace CascadePass.Core.Common.Data.Csv
{
    public class CsvWriter : CsvProvider
    {
        public string LineEnding { get; set; }

        internal string EffectiveLineEnding => this.LineEnding ?? (this.CsvOptions.LineEndings.Length > 0 ? this.CsvOptions.LineEndings[0] : "\n");

        public static string Write(DataTable table, CsvOptions options)
        {
            CsvWriter writer = new()
            {
                CsvOptions = options
            };

            return writer.Write(table);
        }

        public string Write(DataTable table)
        {
            using var sw = new StringWriter();
            this.WriteInternal(table, sw);
            return sw.ToString();
        }

        public void Write(DataTable table, Stream stream)
        {
            using var sw = new StreamWriter(stream);
            this.WriteInternal(table, sw);
            sw.Flush();
        }

        private void WriteInternal(DataTable table, TextWriter writer)
        {
            this.IsWorking = true;
            string lineEnding = this.EffectiveLineEnding;

            // headers
            if (this.CsvOptions.FirstRowAsHeader)
            {
                string headerLine = string.Join(
                    this.CsvOptions.Separator,
                    table.Columns.Cast<DataColumn>().Select(c => EscapeField(c.ColumnName))
                );
                writer.Write(headerLine);
                writer.Write(lineEnding);
            }

            // rows
            foreach (DataRow row in table.Rows)
            {
                string line = string.Join(
                    this.CsvOptions.Separator,
                    row.ItemArray.Select(field => EscapeField(field?.ToString() ?? string.Empty))
                );

                writer.Write(line);
                writer.Write(lineEnding);
            }

            this.IsWorking = false;
        }

        private string EscapeField(string field)
        {
            bool mustQuote = field.Contains(this.CsvOptions.Separator)
                          || field.Contains('\"')
                          || field.Contains(this.EffectiveLineEnding);

            if (mustQuote)
            {
                string escaped = field.Replace("\"", "\"\"");
                return $"\"{escaped}\"";
            }

            return field;
        }
    }
}
