using CascadePass.Core.Common.Data.Csv;

namespace CascadePass.Core.Common.Tests.Data.Csv
{
    public class TestCsvProvider : CsvProvider
    {
        public void SetIsWorking(bool isWorking)
        {
            this.IsWorking = isWorking;
        }
    }
}
