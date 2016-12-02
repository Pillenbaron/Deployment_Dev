using System;
using System.Globalization;
using System.Windows.Data;

namespace awinta.Deployment_NET.Converter
{
    internal class UriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;
            Uri deployPath = null;

            if (string.IsNullOrWhiteSpace(path) && Uri.TryCreate(path, UriKind.Absolute, out deployPath))
            {

                return deployPath;

            }
            else
            {

                return null;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var uri = value as Uri;

            return uri != null ? uri.AbsolutePath : string.Empty;
        }
    }
}
