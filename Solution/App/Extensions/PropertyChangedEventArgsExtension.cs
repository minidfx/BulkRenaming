using System;
using System.ComponentModel;
using System.Linq.Expressions;

using Caliburn.Micro;

namespace App.Extensions
{
    public static class PropertyChangedEventArgsExtension
    {
        #region All other members

        public static bool IsProperty<TProperty>(this PropertyChangedEventArgs source,
                                                 Expression<Func<TProperty>> property)
        {
            return source.PropertyName == property.GetMemberInfo().Name;
        }

        #endregion
    }
}