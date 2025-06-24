using System;
using System.Collections.Generic;

namespace CascadePass.CascadeCore.UI
{
    public class EventSubscriptionManager
    {
        private readonly Dictionary<object, List<Delegate>> handlers;

        public EventSubscriptionManager() {
            this.handlers = [];
        }

        public bool Subscribe<T>(T source, string eventName, Delegate handler) where T : class
        {
            if (source == null || handler == null || string.IsNullOrEmpty(eventName))
            {
                return false;
            }

            var eventInfo = typeof(T).GetEvent(eventName);
            if (eventInfo == null)
            {
                return false;
            }

            eventInfo.AddEventHandler(source, handler);

            if (!handlers.TryGetValue(source, out var list))
            {
                handlers[source] = list = [];
            }

            list.Add(handler);
            return true;
        }

        public bool Unsubscribe<T>(T source, string eventName) where T : class
        {
            if (source == null || !handlers.TryGetValue(source, out var foundHandlers)) return false;

            var eventInfo = typeof(T).GetEvent(eventName);
            if (eventInfo == null) return false;

            foreach (var handler in foundHandlers)
                eventInfo.RemoveEventHandler(source, handler);

            this.handlers.Remove(source);
            return true;
        }

        public bool Unsubscribe<T>(T source) where T : class
        {
            if (source == null || !handlers.TryGetValue(source, out var foundHandlers))
                return false;

            var type = typeof(T);
            bool unsubscribed = false;
            foreach (var handler in foundHandlers)
            {
                // Try to find the event that this handler is attached to
                var method = handler.Method;
                // Find all events on the type
                foreach (var eventInfo in type.GetEvents())
                {
                    // Check if the handler's method matches the event's handler type
                    if (eventInfo.EventHandlerType == handler.GetType() || eventInfo.EventHandlerType == handler.Method.DeclaringType)
                    {
                        try
                        {
                            eventInfo.RemoveEventHandler(source, handler);
                            unsubscribed = true;
                        }
                        catch { /* ignore errors for mismatched handlers */ }
                    }
                }
            }
            handlers.Remove(source);
            return unsubscribed;
        }

        public void UnsubscribeAll()
        {
            foreach (var (source, handlers) in handlers)
            {
                var type = source.GetType();
                foreach (var handler in handlers)
                {
                    var eventInfo = type.GetEvent(handler.Method.Name);
                    eventInfo?.RemoveEventHandler(source, handler);
                }
            }

            handlers.Clear();
        }
    }

}
