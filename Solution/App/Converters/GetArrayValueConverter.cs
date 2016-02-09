﻿using System;
using System.Linq;

using Windows.UI.Xaml.Data;

namespace App.Converters
{
    public sealed class GetArrayValueConverter : IValueConverter
    {
        #region Interface Implementations

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var values = value as string[];
            if (values == null)
            {
                throw new ArgumentException();
            }

            var indexArray = uint.Parse((string) parameter);
            if (indexArray >= values.Count())
            {
                return string.Empty;
            }

            return values[indexArray];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}