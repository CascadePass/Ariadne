using System;
using System.Threading;

namespace CascadePass.CascadeCore.UI.Tests
{
    [TestClass]
    public class EventSubscriptionManagerTests
    {
        private class Publisher
        {
            public event EventHandler SomethingHappened;

            public void Raise() => SomethingHappened?.Invoke(this, EventArgs.Empty);
        }

        private bool wasCalled;
        private void Handler(object sender, EventArgs e) => wasCalled = true;

        [TestMethod]
        public void Subscribe_ShouldAttachEventHandler()
        {
            var manager = new EventSubscriptionManager();
            var publisher = new Publisher();
            wasCalled = false;

            var success = manager.Subscribe(publisher, nameof(Publisher.SomethingHappened), new EventHandler(Handler));
            publisher.Raise();

            Assert.IsTrue(success);
            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        public void Unsubscribe_ShouldDetachSpecificHandler()
        {
            var manager = new EventSubscriptionManager();
            var publisher = new Publisher();
            wasCalled = false;

            manager.Subscribe(publisher, nameof(Publisher.SomethingHappened), new EventHandler(Handler));
            manager.Unsubscribe(publisher, nameof(Publisher.SomethingHappened));

            publisher.Raise();
            Assert.IsFalse(wasCalled);
        }

        [TestMethod]
        public void UnsubscribeAll_ShouldDetachAllHandlers()
        {
            var manager = new EventSubscriptionManager();
            var publisher1 = new Publisher();
            var publisher2 = new Publisher();
            wasCalled = false;

            manager.Subscribe(publisher1, nameof(Publisher.SomethingHappened), new EventHandler(Handler));
            manager.Subscribe(publisher2, nameof(Publisher.SomethingHappened), new EventHandler(Handler));
            manager.UnsubscribeAll();

            publisher1.Raise();
            publisher2.Raise();

            Assert.IsFalse(wasCalled);
        }

        [TestMethod]
        public void Subscribe_NullInputs_ShouldReturnFalse()
        {
            var manager = new EventSubscriptionManager();

            var result1 = manager.Subscribe<object>(null, "SomeEvent", new EventHandler(Handler));
            var result2 = manager.Subscribe(new object(), null, new EventHandler(Handler));
            var result3 = manager.Subscribe(new object(), "SomeEvent", null);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void MultipleSubscriptions_ShouldEachBeInvoked_Independently()
        {
            var manager = new EventSubscriptionManager();
            var publisher = new Publisher();
            int callCount = 0;

            void Handler1(object s, EventArgs e) => callCount++;
            void Handler2(object s, EventArgs e) => callCount++;

            manager.Subscribe(publisher, nameof(Publisher.SomethingHappened), new EventHandler(Handler1));
            manager.Subscribe(publisher, nameof(Publisher.SomethingHappened), new EventHandler(Handler2));

            publisher.Raise();

            Thread.Sleep(100); // Allow time for event handlers to be invoked
            Assert.AreEqual(2, callCount);

            manager.UnsubscribeAll();
            //manager.Unsubscribe(publisher, nameof(Publisher.SomethingHappened));
            callCount = 0;
            publisher.Raise();

            Thread.Sleep(150); // Allow time for event handlers to be invoked
            Assert.AreEqual(0, callCount);
        }

        [TestMethod]
        public void ExpiredReference_ShouldBeCleanedUp()
        {
            var manager = new EventSubscriptionManager();

            void SubscribeTemporary()
            {
                var tempPublisher = new Publisher();
                manager.Subscribe(tempPublisher, nameof(Publisher.SomethingHappened), new EventHandler((s, e) => { }));
            }

            SubscribeTemporary();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Call UnsubscribeAll to trigger cleanup
            manager.UnsubscribeAll();

            // No assert here—just ensure that no exceptions are thrown and internal cleanup occurs
        }

    }

}
