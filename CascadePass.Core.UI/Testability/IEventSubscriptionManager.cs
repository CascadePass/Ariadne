using System;

namespace CascadePass.Core.UI
{
    public interface IEventSubscriptionManager
    {
        bool Subscribe<T>(T source, string eventName, Delegate handler) where T : class;
        bool Unsubscribe<T>(T source) where T : class;
        bool Unsubscribe<T>(T source, string eventName) where T : class;
        void UnsubscribeAll();
    }
}