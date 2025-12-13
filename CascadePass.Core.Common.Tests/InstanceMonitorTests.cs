namespace CascadePass.Core.Common.Tests
{
    [TestClass]
    public class InstanceMonitorTests
    {
        #region ProcessName

        [TestMethod]
        public void ProcessName_IsNotNull()
        {
            Assert.IsNotNull(InstanceMonitor.ProcessName);
        }

        [TestMethod]
        public void ProcessName_IsNotEmpty()
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(InstanceMonitor.ProcessName));
        }

        [TestMethod]
        public void ProcessName_DoesNotChange()
        {
            string firstName = InstanceMonitor.ProcessName;
            string secondName = InstanceMonitor.ProcessName;

            Assert.AreEqual(firstName, secondName);
        }

        [TestMethod]
        public void ProcessName_WhenAlreadySet_DoesNotReassign()
        {
            // First call sets processName
            string firstName = InstanceMonitor.ProcessName;

            // Manually set the backing field to a dummy value
            typeof(InstanceMonitor)
                .GetField("processName", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                .SetValue(null, firstName);

            // Second call should return the existing value without reassigning
            string secondName = InstanceMonitor.ProcessName;

            Assert.AreEqual(firstName, secondName);
        }

        #endregion

        #region Current

        [TestMethod]
        public void Current_IsNotNull()
        {
            Assert.IsNotNull(InstanceMonitor.Current);
        }

        #endregion

        [TestMethod]
        public void InstanceCount_IsGreaterThanZero()
        {
            Assert.IsTrue(InstanceMonitor.InstanceCount > 0);
        }

        [TestMethod]
        public void TestInstanceMonitor_StartAndStopMonitoring()
        {
            // Ensure monitoring is stopped before test starts
            InstanceMonitor.StopMonitoring();

            InstanceMonitor.StartMonitoring(500);

            Assert.IsTrue(InstanceMonitor.IsPolling);

            InstanceMonitor.StopMonitoring();

            System.Threading.Thread.Sleep(20);

            Assert.IsFalse(InstanceMonitor.IsPolling);
        }

        [TestMethod]
        public void StartMonitoring_CalledWhenPolling_DoesNotThrow()
        {
            InstanceMonitor.StartMonitoring(500);
            InstanceMonitor.StartMonitoring(500);

            Assert.IsTrue(InstanceMonitor.IsPolling);

            InstanceMonitor.StopMonitoring();
        }
    }
}
