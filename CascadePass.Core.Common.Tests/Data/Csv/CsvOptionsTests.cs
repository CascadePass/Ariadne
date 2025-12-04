using CascadePass.Core.Common.Data.Csv;

namespace CascadePass.Core.Common.Tests.Data.Csv
{
    [TestClass]
    public class CsvOptionsTests
    {
        [TestMethod]
        public void Default_IsNotNull()
        {
            Assert.IsNotNull(CsvOptions.Default);
        }

        [TestMethod]
        public void Default_IsUseable()
        {
            Assert.IsTrue(CsvOptions.Default.IsUseable);
        }
    }
}
