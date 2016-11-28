using System;
using System.Globalization;
using System.Windows.Data;

namespace awinta.Deployment_NET.Converter
{
    internal class UriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Path = value as string;
            Uri DeployPath = null;

            if (string.IsNullOrWhiteSpace(Path) && Uri.TryCreate(Path, UriKind.Absolute, out DeployPath))
            {

                return new Uri(Path);

            }
            else
            {

                return null;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Uri = value as Uri;

            if (Uri != null) return Uri.AbsolutePath;
            return string.Empty;

        }
    }
}
