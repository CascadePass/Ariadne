using CascadePass.CascadeCore.UI.Converters;
using System;

namespace CascadePass.CascadeCore.UI.Tests.Converters
{
    [TestClass]
    public class FileSizeFormatConverterTests
    {
        [TestMethod]
        public void IsNullConverter_Int32()
        {
            var converter = new FileSizeFormatConverter();
            var result = (string)converter.Convert((Int32)1, null, null, null);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
            Assert.AreEqual("1 B", result);
        }

        [TestMethod]
        public void IsNullConverter_Int16()
        {
            var converter = new FileSizeFormatConverter();
            var result = (string)converter.Convert((Int16)1, null, null, null);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
            Assert.AreEqual("1 B", result);
        }

        [TestMethod]
        public void IsNullConverter_1()
        {
            var converter = new FileSizeFormatConverter();
            var result = (string)converter.Convert((long)1, null, null, null);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
            Assert.AreEqual("1 B", result);
        }

        [TestMethod]
        public void IsNullConverter_1KB()
        {
            var converter = new FileSizeFormatConverter();
            var result = (string)converter.Convert((long)1024, null, null, null);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
            Assert.AreEqual("1.00 KB", result);
        }

        [TestMethod]
        public void IsNullConverter_1MB()
        {
            var converter = new FileSizeFormatConverter();
            var result = (string)converter.Convert((long)1024 * (long)1024, null, null, null);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
            Assert.AreEqual("1.00 MB", result);
        }

        [TestMethod]
        public void IsNullConverter_1GB()
        {
            var converter = new FileSizeFormatConverter();
            var result = (string)converter.Convert((long)1024 * (long)1024 * (long)1024, null, null, null);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
            Assert.AreEqual("1.00 GB", result);
        }
    }
}
