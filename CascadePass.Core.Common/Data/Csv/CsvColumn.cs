namespace CascadePass.Core.Common.Data.Csv
{
    public class CsvColumn
    {
        public string Name { get; set; }

        public SupportedDataType DataType { get; set; }

        public bool IsGrowthSeries { get; set; }

        public bool AllowBlank { get; set; }
    }
}
