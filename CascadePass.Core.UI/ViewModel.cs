using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace CascadePass.Core.UI
{
    public abstract class ViewModel : Observable, INotifyDataErrorInfo
    {
        private ConcurrentDictionary<string, ObservableCollection<object>> errors = new();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Gets a value indicating whether there are errors in the information
        /// the ViewModel is showing.
        /// </summary>
        /// <remarks>
        /// Use <see cref="AddError(string, string)"/>, <see cref="RemoveError(string, string)"/>,
        /// <see cref="ClearErrors(string)"/>, and <see cref="ClearErrors()"/> to manage the errors
        /// for properties in the ViewModel.
        /// </remarks>
        public bool HasErrors => this.errors.Count > 0;

        /// <summary>
        /// Adds an error associated with a specific property to the error collection.
        /// </summary>
        /// <remarks>If the specified property does not already have an associated error list, a new list
        /// is created.  If the error is not already present in the list for the specified property, it is
        /// added,  and the <see cref="OnErrorsChanged"/> event is raised to notify listeners of the change.</remarks>
        /// <param name="propertyName">The name of the property for which the error is being added. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="error">The error to associate with the specified property. Cannot be <see langword="null"/> or empty.</param>
        public virtual void AddError(string propertyName, object error)
        {
            if (!this.errors.TryGetValue(propertyName, out var errorList))
            {
                errorList = [];
                this.errors[propertyName] = errorList;
            }
            if (!errorList.Contains(error))
            {
                errorList.Add(error);
                this.OnErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Removes an error associated with a specific property from the error collection.
        /// </summary>
        /// <param name="propertyName">The name of the property for which the error is being added. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="error">The error message to associate with the specified property. Cannot be <see langword="null"/> or empty.</param>
        public virtual void RemoveError(string propertyName, object error)
        {
            if (this.errors.TryGetValue(propertyName, out var errorList))
            {
                if (errorList.Contains(error))
                {
                    errorList.Remove(error);
                    this.OnErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
                }
            }
        }

        /// <summary>
        /// Clears all validation errors from the current instance.
        /// </summary>
        /// <remarks>This method removes all errors and raises the <see cref="OnErrorsChanged"/> event    
        /// to notify listeners that the error state has changed.</remarks>
        public virtual void ClearErrors()
        {
            this.errors.Clear();
            this.OnErrorsChanged(this, new DataErrorsChangedEventArgs(null));
        }

        /// <summary>
        /// Clears all validation errors for the specified property or all properties if no property name is provided.
        /// </summary>
        /// <remarks>This method raises the <see cref="ErrorsChanged"/> event to notify listeners that the
        /// validation errors have been updated.</remarks>
        /// <param name="propertyName">The name of the property for which to clear validation errors. If <see langword="null"/> or empty, clears
        /// errors for all properties.</param>
        public virtual void ClearErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                this.errors.Clear();
                this.OnErrorsChanged(this, new DataErrorsChangedEventArgs(null));
            }
            else if (this.errors.TryRemove(propertyName, out var errorList))
            {
                errorList.Clear();
                this.OnErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Retrieves the validation errors associated with a specified property or all properties.
        /// </summary>
        /// <remarks>This method is typically used to retrieve validation errors for data binding or user
        /// input validation scenarios.</remarks>
        /// <param name="propertyName">The name of the property for which to retrieve validation errors.  If <see langword="null"/> or an empty
        /// string, errors for all properties are returned.</param>
        /// <returns>An <see cref="IEnumerable"/> containing the validation errors for the specified property,  or all validation
        /// errors if <paramref name="propertyName"/> is <see langword="null"/> or empty.  Returns <see
        /// langword="null"/> if no errors exist for the specified property.</returns>
        public virtual IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return this.errors.Values.SelectMany(e => e).ToList();
            }

            if (this.errors.TryGetValue(propertyName, out var errorList))
            {
                return errorList;
            }

            return null;
        }


        protected void OnErrorsChanged(object sender, DataErrorsChangedEventArgs args)
        {
            this.ErrorsChanged?.Invoke(this, args);
            this.OnPropertyChanged(nameof(this.HasErrors));
        }
    }
}
