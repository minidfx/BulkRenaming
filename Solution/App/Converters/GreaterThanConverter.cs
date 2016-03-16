using System;
using System.Collections.Generic;
using System.Linq;

using Windows.UI.Xaml.Data;

namespace BulkRenaming.Converters
{
    public sealed class GreaterThanConverter : IValueConverter
    {
        #region Interface Implementations

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var enumerable = value as IEnumerable<object>;
            var greaterThan = ushort.Parse((string) parameter);

            if (value != null)
            {
                return enumerable.Count() > greaterThan;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}