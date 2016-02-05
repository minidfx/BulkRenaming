using System;
using System.Text;

using Windows.UI.Xaml.Data;

namespace App.Converters
{
    public class FuturResultConverter : IValueConverter
    {
        #region Interface Implementations

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var valueAsString = value as string;
            if (string.IsNullOrEmpty(valueAsString))
            {
                return string.Empty;
            }

            if (valueAsString.Trim() == string.Empty)
            {
                return "<empty>";
            }

            // Use StringBuilder to avoid performance issue
            var sb = new StringBuilder();

            sb.Append("->");
            sb.Append(valueAsString);

            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}