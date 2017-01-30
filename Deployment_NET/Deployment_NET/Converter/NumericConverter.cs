using System;
using System.Globalization;
using System.Windows.Data;

namespace awinta.Deployment_NET.Converter
{
    internal class NumericConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return value.ToString();
            return string.Empty;

            //return (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = value as string;
            int output;

            if (value is string && !string.IsNullOrWhiteSpace(number) && int.TryParse(number, out output))
                return value;
            return null;
        }
    }
}