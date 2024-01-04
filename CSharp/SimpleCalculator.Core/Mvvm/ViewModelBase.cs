using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace SimpleCalculator.Core.Mvvm
{
    /// <summary>
    /// The simple base view-model that implements <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => this.propertyChangedInternalEvent += value;
            remove => this.propertyChangedInternalEvent -= value;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        private PropertyChangedEventHandler propertyChangedInternalEvent;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a notification when a property value changes.
        /// </summary>
        public IObservable<PropertyChangedEventArgs> PropertyChanged
            => Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => (s, e) => h(e),
                h => this.propertyChangedInternalEvent += h,
                h => this.propertyChangedInternalEvent -= h);

        #endregion

        #region Protected Methods

        /// <summary>
        /// Sets the property and notify property changed
        /// </summary>
        /// <param name="backingStore">The backing field of the property.</param>
        /// <param name="value">The setting value.</param>
        /// <param name="onChanged">The action that is called after the property value has been changed.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns><c>true</c> if the value was changed, <c>false</c> if the existing value matched the desired value.</returns>
        protected virtual bool SetProperty<T>(ref T backingStore, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged();
            this.RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Sets the property and notify property changed
        /// </summary>
        /// <param name="backingStore">The backing field of the property.</param>
        /// <param name="value">The setting value.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns><c>true</c> if the value was changed, <c>false</c> if the existing value matched the desired value.</returns>
        protected virtual bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            this.RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Raises property changed event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notifies that a property is changed.
        /// </summary>
        /// <param name="e">The property changed event args.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.propertyChangedInternalEvent?.Invoke(this, e);
        }

        #endregion
    }
}
