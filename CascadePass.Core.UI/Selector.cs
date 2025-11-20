using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace CascadePass.Core.UI
{
    public class Selector<T> : Observable
    {
        private ObservableCollection<T> available;
        private T selected;
        private bool? isValueType;

        public event EventHandler<SelectionChangingEventArgs<T>> SelectionChanging;
        public event EventHandler<SelectionChangedEventArgs<T>> SelectionChanged;

        #region Constructors

        public Selector()
        {
            this.available = [];
            this.Dispatcher = Application.Current?.Dispatcher;
        }

        public Selector(ObservableCollection<T> values)
        {
            this.available = values;
            this.Dispatcher = Application.Current?.Dispatcher;
        }

        public Selector(ObservableCollection<T> values, T initiallySelected)
        {
            this.available = values;
            this.selected = initiallySelected;
            this.Dispatcher = Application.Current?.Dispatcher;
        }

        public Selector(ObservableCollection<T> values, InitialSelectionPolicy initialSelection)
        {
            this.available = values;
            this.Dispatcher = Application.Current?.Dispatcher;

            if (initialSelection == InitialSelectionPolicy.First && values.Count > 0)
            {
                this.SelectFirst();
            }
            else if (initialSelection == InitialSelectionPolicy.Last && values.Count > 0)
            {
                this.SelectLast();
            }
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public bool AllowNullSelection { get; set; }

        [XmlIgnore]
        public bool AllowEmptySelection { get; set; }

        [XmlIgnore]
        public ReselectionPolicy ReselectionPolicy { get; set; }

        [XmlIgnore]
        public UnlistedSelectionPolicy UnlistedSelectionPolicy { get; set; }

        [XmlIgnore]
        public Dispatcher Dispatcher { get; set; }

        [XmlIgnore]
        public ObservableCollection<T> Available
        {
            get => this.available;
            set => this.SetPropertyValue(ref this.available, value, nameof(this.Available));
        }

        public T Selected
        {
            get => this.selected;
            set
            {
                #region Guard Clauses

                if (!this.AllowNullSelection && value is null)
                {
                    return;
                }

                if (EqualityComparer<T>.Default.Equals(this.selected, value))
                {
                    return;
                }

                #endregion

                #region Not in list

                // ComboBox with IsEditable="true" will cause this.

                if (!this.Available.Contains(value))
                {
                    switch (this.UnlistedSelectionPolicy)
                    {
                        case UnlistedSelectionPolicy.None:
                        case UnlistedSelectionPolicy.Allow:
                            break;

                        case UnlistedSelectionPolicy.Throw:
                            throw new InvalidOperationException("Selected item is not in the Available list.");

                        case UnlistedSelectionPolicy.Add:
                            this.Add(value);
                            break;
                    }
                }

                #endregion

                var original = this.Selected;

                SelectionChangingEventArgs<T> beforeChangeArgs = new(original, value);
                this.OnSelectionChanging(beforeChangeArgs);

                if (beforeChangeArgs.Cancelled)
                {
                    return;
                }


                // INotifyPropertyChanged is rased here:
                this.SetPropertyValue(ref this.selected, value, nameof(this.Selected));


                SelectionChangedEventArgs<T> afterChangeArgs = new(original, value);
                this.OnSelectionChanged(afterChangeArgs);
            }
        }

        [XmlIgnore]
        public bool HasSelection => !EqualityComparer<T>.Default.Equals(this.Selected, default);

        [XmlIgnore]
        public int SelectedIndex => this.Available?.IndexOf(this.Selected) ?? -1;

        [XmlIgnore]
        public bool IsValueType => this.isValueType ??= typeof(T).IsValueType;

        [XmlIgnore]
        public bool IsReferenceType => !this.IsValueType;

        #endregion

        #region Methods

        #region Manage Selection

        public bool ClearSelection()
        {
            if (!this.AllowNullSelection && this.IsReferenceType)
            {
                return false;
            }

            if (!this.AllowEmptySelection && this.IsValueType)
            {
                return false;
            }

            this.Selected = default;
            return true;
        }

        public bool SelectFirst()
        {
            if (this.Available is null || this.Available.Count == 0)
            {
                return false;
            }
            this.Selected = this.Available[0];
            return true;
        }

        public bool SelectLast()
        {
            if (this.Available is null || this.Available.Count == 0)
            {
                return false;
            }
            this.Selected = this.Available[^1];
            return true;
        }

        public bool SelectPrevious()
        {
            if (this.Available is null || this.Available.Count == 0)
            {
                return false;
            }
            int currentIndex = this.SelectedIndex;
            int previousIndex = (currentIndex - 1 + this.Available.Count) % this.Available.Count;
            this.Selected = this.Available[previousIndex];
            return true;
        }

        public bool SelectNext()
        {
            if (this.Available is null || this.Available.Count == 0)
            {
                return false;
            }

            int currentIndex = this.SelectedIndex;
            int nextIndex = (currentIndex + 1) % this.Available.Count;
            this.Selected = this.Available[nextIndex];
            return true;
        }

        public bool SelectWhere(Func<T, bool> predicate)
        {
            if (this.Available is null) return false;
            var match = this.Available.FirstOrDefault(predicate);
            if (match == null && !this.AllowNullSelection) return false;
            this.Selected = match;
            return true;
        }

        public bool SelectIf(Func<T, bool> predicate, T fallback)
        {
            if (this.Available is null) return false;
            var match = this.Available.FirstOrDefault(predicate);
            this.Selected = match ?? fallback;
            return true;
        }

        public bool SelectRandom()
        {
            if (this.Available is null || this.Available.Count == 0)
            {
                return false;
            }

            // This means we create a new Random instance each time we call this method.
            // Which is expensive, and probably lower quality randomness.
            //
            // This method is intended for unit tests and app demos, not for real-world
            // use.  It's almost never a good idea to select something randomly in a UI.
            // Optimizing would add a little bit of complexity for no real gain.

            var random = new Random();
            var currentlySelected = this.SelectedIndex;
            int newIndex;

            // Guard against selecting the same item again if there's more than one item.
            do
            {
                newIndex = random.Next(this.Available.Count);
            } while (newIndex == currentlySelected && this.Available.Count > 1);

            this.Selected = this.Available[newIndex];

            return true;
        }

        #endregion

        #region Manage Available

        public void Add(T item)
        {
            if (this.Dispatcher is null || this.Dispatcher.CheckAccess())
            {
                this.Available.Add(item);
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this.Available.Add(item);
            });
        }

        public void Remove(T item)
        {
            if (this.Dispatcher == null || this.Dispatcher.CheckAccess())
            {
                bool wasSelected = EqualityComparer<T>.Default.Equals(this.Selected, item);

                if (wasSelected)
                {
                    switch (this.ReselectionPolicy)
                    {
                        case ReselectionPolicy.Next:
                            SelectNext(); break;
                        case ReselectionPolicy.Previous:
                            SelectPrevious(); break;
                            // First/Last/None/LeaveUnchanged handled after removal
                    }
                }

                this.Available.Remove(item);

                if (wasSelected)
                {
                    switch (this.ReselectionPolicy)
                    {
                        case ReselectionPolicy.First:
                            SelectFirst(); break;
                        case ReselectionPolicy.Last:
                            SelectLast(); break;
                        case ReselectionPolicy.None:
                            ClearSelection(); break;
                        case ReselectionPolicy.LeaveUnchanged:
                            break;
                    }
                }
                return;
            }

            this.Dispatcher.Invoke(() => Remove(item)); // reuse same logic inside dispatcher
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (this.Dispatcher is null || this.Dispatcher.CheckAccess())
            {
                foreach (var item in items)
                {
                    this.Available.Add(item);
                }

                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                foreach (var item in items) this.Available.Add(item);
            });
        }

        public void Clear()
        {
            if (this.Dispatcher is null || this.Dispatcher.CheckAccess())
            {
                this.Available.Clear();

                // Important.  This may or may not do anything, depending on the
                // properties: AllowNullSelection, AllowEmptySelection.

                this.ClearSelection();

                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this.Clear();
            });
        }

        #endregion

        protected virtual void OnSelectionChanging(SelectionChangingEventArgs<T> e)
        {
            this.SelectionChanging?.Invoke(this, e);
        }

        protected virtual void OnSelectionChanged(SelectionChangedEventArgs<T> e)
        {
            this.SelectionChanged?.Invoke(this, e);
        }

        #endregion
    }

    public class SelectionChangingEventArgs<T> : EventArgs
    {
        public bool Cancelled { get; set; }
        public T CurrentValue { get; }
        public T ProposedValue { get; }

        public SelectionChangingEventArgs(T current, T proposed)
        {
            CurrentValue = current;
            ProposedValue = proposed;
        }
    }

    public class SelectionChangedEventArgs<T> : EventArgs
    {
        public T CurrentValue { get; }
        public T ProposedValue { get; }

        public SelectionChangedEventArgs(T current, T proposed)
        {
            CurrentValue = current;
            ProposedValue = proposed;
        }
    }

    public enum InitialSelectionPolicy
    {
        None,
        First,
        Last,
    }

    public enum ReselectionPolicy
    {
        None,
        First,
        Last,
        Next,
        Previous,
        LeaveUnchanged,
    }

    public enum UnlistedSelectionPolicy
    {
        None,
        Allow,
        Throw,
        Add,
    }
}
