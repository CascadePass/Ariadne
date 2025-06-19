using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CascadePass.CascadeCore.UI.Tests
{
    [TestClass]
    public class RegistryProviderTests
    {
        #region GetValue Tests

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\r")]
        [DataRow("\n")]
        [DataRow("\t")]
        [DataRow("\r\n")]
        public void GetValue_ThrowsForEmptyKeyName(string verionOfEmpty)
        {
            var registryProvider = new RegistryProvider();
            Assert.ThrowsException<ArgumentException>(() => registryProvider.GetValue(verionOfEmpty, "AnyValue"));
        }

        [TestMethod]
        public void GetValue_ReturnsNullAndRaisesEvent_WhenKeyDoesNotExist()
        {
            var registryProvider = new RegistryProvider();
            string keyName = "NonExistentKey";
            string valueName = "NonExistentValue";

            var result = registryProvider.GetValue(keyName, valueName);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetValue_RaisesRegistryAccessedEvent()
        {
            var registryProvider = new RegistryProvider();
            string keyName = "DoesNotMatter";
            string valueName = "DoesNotMatter";
            RegistryAccessEventArgs receivedArgs = null;

            registryProvider.RegistryAccessed += (sender, args) =>
            {
                receivedArgs = args;
            };

            var result = registryProvider.GetValue(keyName, valueName);

            Assert.IsNull(result);
            Assert.IsNotNull(receivedArgs);
            Assert.AreEqual(RegistryAccessType.Read, receivedArgs.AccessType);
            Assert.AreEqual(keyName, receivedArgs.KeyName);
            Assert.AreEqual(valueName, receivedArgs.ValueName);
        }

        [TestMethod]
        public void GetValue_RaisesRegistryAccessedEvent_OnlyOnce()
        {
            var registryProvider = new RegistryProvider();
            string keyName = "ArbitraryKey";
            string valueName = "ArbitraryValue";
            int eventCount = 0;

            registryProvider.RegistryAccessed += (sender, args) =>
            {
                eventCount++;
            };

            var result = registryProvider.GetValue(keyName, valueName);

            Assert.IsNull(result);
            Assert.AreEqual(1, eventCount, "Expected only one RegistryAccessed event to be raised.");
        }

        [TestMethod]
        public async Task GetValue_RaisesRegistryAccessedAsyncEvent()
        {
            var registryProvider = new RegistryProvider();
            string keyName = "AsyncKey";
            string valueName = "AsyncValue";

            RegistryAccessEventArgs receivedArgs = null;
            var tcs = new TaskCompletionSource<bool>();

            registryProvider.RegistryAccessedAsync += async (sender, args) =>
            {
                receivedArgs = args;
                tcs.SetResult(true);
                await Task.CompletedTask;
            };

            var result = registryProvider.GetValue(keyName, valueName);

            await Task.WhenAny(tcs.Task, Task.Delay(500)); // Wait for async event or timeout

            Assert.IsNull(result);
            Assert.IsTrue(tcs.Task.IsCompleted, "Async event did not complete in time.");
            Assert.IsNotNull(receivedArgs);
            Assert.AreEqual(RegistryAccessType.Read, receivedArgs.AccessType);
            Assert.AreEqual(keyName, receivedArgs.KeyName);
            Assert.AreEqual(valueName, receivedArgs.ValueName);
        }

        [TestMethod]
        public async Task GetValue_RaisesRegistryAccessedAsyncEvent_OnlyOnce()
        {
            var registryProvider = new RegistryProvider();
            string keyName = "AsyncKey_Single";
            string valueName = "AsyncValue_Single";

            int asyncCount = 0;
            var tcs = new TaskCompletionSource<bool>();

            registryProvider.RegistryAccessedAsync += async (sender, args) =>
            {
                Interlocked.Increment(ref asyncCount);
                tcs.SetResult(true);
                await Task.CompletedTask;
            };

            var result = registryProvider.GetValue(keyName, valueName);

            await Task.WhenAny(tcs.Task, Task.Delay(500));

            Assert.IsNull(result);
            Assert.IsTrue(tcs.Task.IsCompleted, "Async event did not fire.");
            Assert.AreEqual(1, asyncCount, "Expected only one async event invocation.");
        }

        #endregion

        #region SetValue Tests

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\r")]
        [DataRow("\n")]
        [DataRow("\t")]
        [DataRow("\r\n")]
        public void SetValue_ThrowsForEmptyKeyName(string verionOfEmpty)
        {
            var registryProvider = new RegistryProvider();
            Assert.ThrowsException<ArgumentException>(() => registryProvider.SetValue(verionOfEmpty, "AnyValueName", "ValueTheCallerWantsToSaveToTheRegistry", RegistryValueKind.String));
        }


        [TestMethod]
        public void SetValue_ReturnsNullAndRaisesEvent_WhenKeyDoesNotExist()
        {
            var registryProvider = new RegistryProvider();
            string keyName = "NonExistentKey";
            string valueName = "NonExistentValue";
            string valueToSet = "ValueTheCallerWantsToSaveToTheRegistry";

            var result = registryProvider.SetValue(keyName, valueName, valueToSet, RegistryValueKind.String);
            registryProvider.DeleteValue(keyName, valueName);

            Assert.IsTrue(result);
        }

        #endregion
    }
}
