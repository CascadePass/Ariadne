using CascadePass.Core.UI.Converters;

namespace CascadePass.Core.UI.Tests.Converters
{
    [TestClass]
    public class EqualityConverterTests
    {
        [TestMethod]
        public void Convert_ShouldReturnTrueWhenSameObjectIsSupplied()
        {
            object obj = new();
            var converter = new EqualityConverter();
            var result = (bool)converter.Convert(obj, null, obj, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Convert_ShouldReturnFalseWhenDifferentInstancesAreSupplied()
        {
            object obj1 = new(), obj2 = new();
            var converter = new EqualityConverter();
            var result = (bool)converter.Convert(obj1, null, obj2, null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Convert_ShouldReturnTrueWhenSameNumberIsSupplied()
        {
            var converter = new EqualityConverter();
            var result = (bool)converter.Convert(14, null, 14, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Convert_ShouldReturnFalseWhenDifferentNumbersAreSupplied()
        {
            var converter = new EqualityConverter();
            var result = (bool)converter.Convert(14, null, 141, null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Convert_ShouldReturnTrueWhenSameStringIsSupplied()
        {
            var converter = new EqualityConverter();
            var result = (bool)converter.Convert("Shuksan", null, "Shuksan", null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Convert_ShouldReturnFalseWhenDifferentStringsAreSupplied()
        {
            var converter = new EqualityConverter();
            var result = (bool)converter.Convert("Baker", null, "Rainier", null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Convert_ShouldReturnTrueWhenSameBoolIsSupplied()
        {
            var converter = new EqualityConverter();
            var result = (bool)converter.Convert(true, null, true, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Convert_ShouldReturnFalseWhenDifferentBoolsAreSupplied()
        {
            var converter = new EqualityConverter();
            var result = (bool)converter.Convert(true, null, false, null);

            Assert.IsFalse(result);
        }
    }
}
