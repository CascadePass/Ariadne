using CascadePass.CascadeCore.UI.Converters;

namespace CascadePass.CascadeCore.UI.Tests.Converters
{
    [TestClass]
    public class IsNullConverterTests
    {
        [TestMethod]
        public void IsNullConverter_ReturnsTrueForNull() {
            var converter = new IsNullConverter();
            var result = (bool)converter.Convert(null, null, null, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNullConverter_ReturnsTrueForNullObject()
        {
            var converter = new IsNullConverter();
            var result = (bool)converter.Convert((object)null, null, null, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNullConverter_ReturnsFalseForObject()
        {
            var converter = new IsNullConverter();
            var result = (bool)converter.Convert(new object(), null, null, null);

            Assert.IsFalse(result);
        }
    }
}
