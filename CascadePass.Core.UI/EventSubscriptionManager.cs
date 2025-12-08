using System;
using System.Collections.Generic;

namespace CascadePass.Core.UI
{
    public class EventSubscriptionManager : IEventSubscriptionManager
    {
        private readonly object syncRoot;
        private readonly Dictionary<WeakReference<object>, List<HandlerInfo>> handlers;

        public EventSubscriptionManager() {
            this.syncRoot = new();
            this.handlers = [];
        }

        private class HandlerInfo
        {
            public string EventName { get; set; }
            public Delegate Handler { get; set; }
        }

        public bool Subscribe<T>(T source, string eventName, Delegate handler) where T : class
        {
            if (source == null || handler == null || string.IsNullOrEmpty(eventName))
                return false;

            var eventInfo = typeof(T).GetEvent(eventName);
            if (eventInfo == null || !eventInfo.EventHandlerType.IsAssignableFrom(handler.GetType()))
                return false;

            eventInfo.AddEventHandler(source, handler);
            var weakRef = new WeakReference<object>(source);

            lock (syncRoot)
            {
                CleanupDeadReferences();

                if (!handlers.TryGetValue(weakRef, out var list))
                {
                    list = new List<HandlerInfo>();
                    handlers[weakRef] = list;
                }

                list.Add(new HandlerInfo { EventName = eventName, Handler = handler });
            }

            return true;
        }

        public bool Unsubscribe<T>(T source, string eventName) where T : class
        {
            if (source == null)
                return false;

            WeakReference<object> weakRef;
            List<HandlerInfo> foundHandlers;

            lock (syncRoot)
            {
                CleanupDeadReferences();
                weakRef = FindWeakReference(source);
                if (weakRef == null || !handlers.TryGetValue(weakRef, out foundHandlers))
                    return false;
            }

            var eventInfo = typeof(T).GetEvent(eventName);
            if (eventInfo == null)
                return false;

            foundHandlers.RemoveAll(h =>
            {
                if (h.EventName == eventName)
                {
                    eventInfo.RemoveEventHandler(source, h.Handler);
                    return true;
                }
                return false;
            });

            lock (syncRoot)
            {
                if (foundHandlers.Count == 0 && weakRef != null)
                    handlers.Remove(weakRef);
            }

            return true;
        }

        public bool Unsubscribe<T>(T source) where T : class
        {
            if (source == null)
                return false;

            WeakReference<object> weakRef;
            List<HandlerInfo> foundHandlers;

            lock (syncRoot)
            {
                CleanupDeadReferences();
                weakRef = FindWeakReference(source);
                if (weakRef == null || !handlers.TryGetValue(weakRef, out foundHandlers))
                    return false;
            }

            var type = typeof(T);
            foreach (var info in foundHandlers)
            {
                var eventInfo = type.GetEvent(info.EventName);
                eventInfo?.RemoveEventHandler(source, info.Handler);
            }

            lock (syncRoot)
            {
                if (weakRef != null)
                    handlers.Remove(weakRef);
            }

            return true;
        }

        public void UnsubscribeAll()
        {
            lock (syncRoot)
            {
                CleanupDeadReferences();

                foreach (var entry in handlers)
                {
                    if (entry.Key.TryGetTarget(out var target))
                    {
                        var type = target.GetType();
                        foreach (var info in entry.Value)
                        {
                            var eventInfo = type.GetEvent(info.EventName);
                            eventInfo?.RemoveEventHandler(target, info.Handler);
                        }
                    }
                }

                handlers.Clear();
            }
        }

        private void CleanupDeadReferences()
        {
            var deadRefs = new List<WeakReference<object>>();

            foreach (var kvp in handlers)
            {
                if (!kvp.Key.TryGetTarget(out _))
                    deadRefs.Add(kvp.Key);
            }

            foreach (var deadRef in deadRefs)
            {
                handlers.Remove(deadRef);
            }
        }

        private WeakReference<object> FindWeakReference(object target)
        {
            foreach (var weakRef in handlers.Keys)
            {
                if (weakRef.TryGetTarget(out var obj) && ReferenceEquals(obj, target))
                    return weakRef;
            }
            return null;
        }
    }
}
