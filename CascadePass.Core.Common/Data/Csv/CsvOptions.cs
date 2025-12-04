namespace CascadePass.Core.Common.Data.Csv
{
    public class CsvOptions
    {
        public string[] LineEndings { get; set; }

        public string Separator { get; set; }

        public bool PreserveWhitespace { get; set; }

        public bool FirstRowAsHeader { get; set; }

        public bool IsUseable =>
            !string.IsNullOrWhiteSpace(this.Separator) &&
            this.LineEndings != null && this.LineEndings.Length > 0
        ;

        public static CsvOptions Default => new()
        {
            LineEndings = ["\r\n", "\n", "\r",],
            Separator = ",",
            PreserveWhitespace = false,
            FirstRowAsHeader = true,
        };
    }
}
