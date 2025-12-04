using CascadePass.Core.Common.Data.Csv;

namespace CascadePass.Core.Common.Tests.Data.Csv
{
    [TestClass]
    public class CsvProviderTests
    {
        [TestMethod]
        public void CsvOptions_NotNull_ByDefault()
        {
            Assert.IsNotNull(new TestCsvProvider().CsvOptions);
        }

        [TestMethod]
        public void CsvOptions_CannotSetNull()
        {
            var provider = new TestCsvProvider();

            if (provider.CsvOptions is null)
            {
                Assert.Inconclusive();
            }

            provider.CsvOptions = null;
            Assert.IsNotNull(provider.CsvOptions);
        }

        [TestMethod]
        public void CsvOptions_EventFires_WhenNullIsSet()
        {
            var provider = new TestCsvProvider();

            if (provider.CsvOptions is null)
            {
                Assert.Inconclusive();
            }

            bool eventFired = false;

            provider.OptionsChangeIgnored += (o, e) =>
            {
                eventFired = true;
            };


            // The event has been subscribed to, now trigger it:
            provider.CsvOptions = null;
            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void CsvOptions_EventIdentifiesReason_WhenNullIsSet()
        {
            var provider = new TestCsvProvider();

            if (provider.CsvOptions is null)
            {
                Assert.Inconclusive();
            }

            bool eventFired = true;
            provider.OptionsChangeIgnored += (o, e) =>
            {
                eventFired = true;
                Assert.AreEqual(OptionsChangeIgnoredEventArgs.ReasonType.NullOptionsNotAllowed, e.Reason);
            };


            // The event has been subscribed to, now trigger it:
            provider.CsvOptions = null;
            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void CsvOptions_CannotSetWhileWorking()
        {
            var provider = new TestCsvProvider();
            var currentOptions = provider.CsvOptions;
            var mustReject = new CsvOptions();

            provider.SetIsWorking(true);
            provider.CsvOptions = mustReject;

            Assert.AreEqual(currentOptions, provider.CsvOptions);
            Assert.AreNotEqual(mustReject, provider.CsvOptions);
        }

        [TestMethod]
        public void CsvOptions_EventFires_WhenSetWhileWorking()
        {
            var provider = new TestCsvProvider();
            var currentOptions = provider.CsvOptions;
            var mustReject = new CsvOptions();

            bool eventFired = false;

            provider.OptionsChangeIgnored += (o, e) =>
            {
                eventFired = true;
            };

            provider.AllowLiveOptionsChange = false;
            provider.SetIsWorking(true);
            provider.CsvOptions = mustReject;


            // The event has been subscribed to, now trigger it:
            provider.CsvOptions = mustReject;
            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void CsvOptions_EventIdentifiesReason_WhenSetWhileWorking()
        {
            var provider = new TestCsvProvider();
            var currentOptions = provider.CsvOptions;
            var mustReject = new CsvOptions();

            bool eventFired = false;

            provider.OptionsChangeIgnored += (o, e) =>
            {
                eventFired = true;
                Assert.AreEqual(OptionsChangeIgnoredEventArgs.ReasonType.OptionsChangeNotAllowedWhileWorking, e.Reason);
            };

            provider.AllowLiveOptionsChange = false;
            provider.SetIsWorking(true);
            provider.CsvOptions = mustReject;


            // The event has been subscribed to, now trigger it:
            provider.CsvOptions = mustReject;
            Assert.IsTrue(eventFired);
        }
    }
}
