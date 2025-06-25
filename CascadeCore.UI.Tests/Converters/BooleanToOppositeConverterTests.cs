using CascadePass.CascadeCore.UI.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CascadeCore.UI.Tests.Converters
{
    [TestClass]
    public class BooleanToOppositeConverterTests
    {
        [TestMethod]
        public void Convert_TrueReturnsFalse()
        {
            var converter = new BooleanToOppositeConverter();
            var result = converter.Convert(true, null, null, null);
            bool testResult = (bool)result;

            Assert.IsFalse(testResult);
        }

        [TestMethod]
        public void Convert_FalseReturnsTrue()
        {
            var converter = new BooleanToOppositeConverter();
            var result = converter.Convert(false, null, null, null);
            bool testResult = (bool)result;

            Assert.IsTrue(testResult);
        }

        [TestMethod]
        public void ConvertBack_TrueReturnsFalse()
        {
            var converter = new BooleanToOppositeConverter();
            var result = converter.ConvertBack(true, null, null, null);
            bool testResult = (bool)result;

            Assert.IsFalse(testResult);
        }

        [TestMethod]
        public void ConvertBack_FalseReturnsTrue()
        {
            var converter = new BooleanToOppositeConverter();
            var result = converter.ConvertBack(false, null, null, null);
            bool testResult = (bool)result;

            Assert.IsTrue(testResult);
        }
    }
}
