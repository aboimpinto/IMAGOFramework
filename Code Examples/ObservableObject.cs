// <copyright company="Roche Diagnostics AG">
// Copyright (c) Roche Diagnostics AG. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Roche.LabCore.ComponentModel
{
    /// <summary>
    /// Base class for observable Objects. It implements the <see cref="INotifyPropertyChanged"/> interface. Use
    /// this as a base class for ViewModels to notify the bound View of changes.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Allows triggering the <see cref="PropertyChanged"/> event using
        /// a lambda expression, thus avoiding strings. Keep in mind that
        /// using this method comes with a performance penalty, so don't use
        /// it for frequently updated properties that cause a lot of events
        /// to be fired.
        /// </summary>
        /// <param name="propertyExpression">Expression pointing to a given
        /// property.</param>
        protected virtual void OnPropertyChanged(LambdaExpression propertyExpression)
        {
            Guard.ArgumentIsNotNull(propertyExpression, () => propertyExpression);

            // the cast will always succeed if properly used
            MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
            string propertyName = memberExpression.Member.Name;
            this.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Allows triggering the <see cref="PropertyChanged"/> event using
        /// a lambda expression, thus avoiding strings. Keep in minde that
        /// using this method comes with a performance penalty, so don't use
        /// it for frequently updated properties that cause a lot of events
        /// to be fired.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="propertyExpression">Expression pointing to a given
        /// property.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Keep API.")]
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            Guard.ArgumentIsNotNull(propertyExpression, () => propertyExpression);

            // the cast will always succeed if properly used
            MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
            string propertyName = memberExpression.Member.Name;
            this.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for
        /// a given property.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler listener = this.PropertyChanged;
            if (listener != null)
            {
                listener(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for
        /// a set of properties.
        /// </summary>
        /// <param name="propertyNames">Provides the names of the changed properties.</param>
        protected void OnPropertyChanged(params string[] propertyNames)
        {
            Guard.ArgumentIsNotNull(propertyNames, () => propertyNames);
            foreach (string propertyName in propertyNames)
            {
                this.OnPropertyChanged(propertyName);
            }
        }
    }

    /// <summary>
    /// Provides an <see langword="abstract"/> base class for any object that provides property change notifications.
    /// This base class is not intended for ViewModels to update Views only. It provides an additional subscription
    /// mechanism that can be used to notify any object of property changes. Use the PropertyObserver to notify changes.
    /// </summary>
    /// <typeparam name="T">The type of the the object whose property changes should be observed.</typeparam>
    [DataContract(Name = "ObservableObject", Namespace = Contracts.Namespace)]
    public abstract class ObservableObject<T> : INotifyPropertyChanged
        where T : ObservableObject<T>
    {
        private PropertyObserver<T> propertyObserver;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Allows triggering the <see cref="PropertyChanged"/> event using
        /// a lambda expression, thus avoiding strings. Keep in mind that
        /// using this method comes with a performance penalty, so don't use
        /// it for frequently updated properties that cause a lot of events
        /// to be fired.
        /// </summary>
        /// <param name="propertyExpression">Expression pointing to a given
        /// property.</param>
        protected virtual void OnPropertyChanged(LambdaExpression propertyExpression)
        {
            Guard.ArgumentIsNotNull(propertyExpression, () => propertyExpression);

            // the cast will always succeed if properly used
            MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
            string propertyName = memberExpression.Member.Name;
            this.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Allows triggering the <see cref="PropertyChanged"/> event using
        /// a lambda expression, thus avoiding strings. Keep in minde that
        /// using this method comes with a performance penalty, so don't use
        /// it for frequently updated properties that cause a lot of events
        /// to be fired.
        /// </summary>
        /// <typeparam name="TProperty">Type of the property.</typeparam>
        /// <param name="propertyExpression">Expression pointing to a given
        /// property.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Keep API.")]
        protected virtual void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            Guard.ArgumentIsNotNull(propertyExpression, () => propertyExpression);

            // the cast will always succeed if properly used
            MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
            string propertyName = memberExpression.Member.Name;
            this.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for
        /// a given property.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyObserver.NotifyChanged(propertyName);

            // Call UI notification: old style.
            PropertyChangedEventHandler listener = this.PropertyChanged;
            if (listener != null)
            {
                listener(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for
        /// a set of properties.
        /// </summary>
        /// <param name="propertyNames">Provides the names of the changed properties.</param>
        protected void OnPropertyChanged(params string[] propertyNames)
        {
            Guard.ArgumentIsNotNull(propertyNames, () => propertyNames);
            foreach (string propertyName in propertyNames)
            {
                this.OnPropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableObject{T}"/> class.
        /// </summary>
        protected ObservableObject()
        {
            this.propertyObserver = new PropertyObserver<T>((T)this);
        }

        /// <summary>
        /// Gets the <see cref="PropertyObserver"/> that manages subscribtions tp property change events.
        /// </summary>
        /// <value>The property <see cref="PropertyObserver"/> that manages subscribtions tp property change events.</value>
        protected PropertyObserver<T> PropertyObserver
        {
            get
            {
                if (this.propertyObserver == null)
                {
                    this.propertyObserver = new PropertyObserver<T>((T)this);
                }

                return this.propertyObserver;
            }
        }

        /// <summary>
        /// Subscribes to changes in the property referenced in the given <paramref name="propertySelector"/> with the given <paramref name="action"/> delegate.
        /// </summary>
        /// <typeparam name="TResult">The type of the property.</typeparam>
        /// <param name="propertySelector">A lambda expression that accesses a property, such as <c>x => x.Name</c> (where the type of x is <typeparamref name="T"/>).</param>
        /// <param name="action">The callback action to invoke when the given property changes.</param>
        /// <returns>A subscription token that can be disposed if notification should no longer occur.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="propertySelector"/> is null.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="propertySelector"/> does not represent a property access.</exception>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Expression must be nested with a generic type.")]
        public IDisposable SubscribeChanged<TResult>(Expression<Func<T, TResult>> propertySelector, Action<T> action)
        {
            return this.PropertyObserver.SubscribeChanged(propertySelector, action);
        }
    }
}