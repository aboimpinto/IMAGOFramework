using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace IMAGO.Framework.MVVM
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region Public Events 
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Propertected Properties 
        protected PropertyChangedEventHandler PropertyChangedHandler
        {
            get
            {
                return PropertyChanged;
            }
        }
        #endregion

        #region Virtual Methods 
        public virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression) 
        {
            //var handler = PropertyChanged;
            //if (handler != null)
            //{
                var propertyName = GetPropertyName(propertyExpression);
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}
        }
        #endregion

        #region Private Methods 
        protected string GetPropertyName<T>(Expression<Func<T>> propertyExpression) 
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }

            return property.Name;
        }
        #endregion
    }
}
