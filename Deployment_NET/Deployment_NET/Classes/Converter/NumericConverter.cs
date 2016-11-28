﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace awinta.Deployment_NET.Converter
{
    internal class NumericConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Number = value as string;
            int Output;

            if (value is string && string.IsNullOrWhiteSpace(Number) && int.TryParse(Number, out Output))
            {

                return value;

            }
            else
            {

                return null;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value;
        }
    }
}
