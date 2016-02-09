using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace App.Converters
{
    public sealed class NullVisibilityConverter : IValueConverter
    {
        #region Interface Implementations

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}