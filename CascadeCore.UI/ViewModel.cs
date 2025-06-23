using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CascadePass.CascadeCore.UI
{
    public abstract class ViewModel : Observable, INotifyDataErrorInfo
    {
        private bool hasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        /// <summary>
        /// Gets or sets a value indicating whether there are errors in the
        /// information the ViewModel is showing.
        /// </summary>
        public bool HasErrors
        {
            get => this.hasErrors;
            internal set => this.SetPropertyValue(ref this.hasErrors, value, nameof(this.HasErrors));
        }

        public virtual IEnumerable GetErrors(string propertyName) => null;

        protected void OnErrorsChanged(object sender, DataErrorsChangedEventArgs args)
        {
            this.ErrorsChanged?.Invoke(this, args);
        }
    }

    public class EventSubscriptionManager : IDisposable
    {
        private readonly List<Action> _unsubscribers = new();

        public void Subscribe<TEventArgs>(
            Action<EventHandler<TEventArgs>> subscribe,
            Action<EventHandler<TEventArgs>> unsubscribe,
            EventHandler<TEventArgs> handler) where TEventArgs : EventArgs
        {
            subscribe(handler);
            _unsubscribers.Add(() => unsubscribe(handler));
        }

        public void Dispose()
        {
            foreach (var unsubscribe in _unsubscribers)
            {
                unsubscribe();
            }
            _unsubscribers.Clear();
        }
    }


}
