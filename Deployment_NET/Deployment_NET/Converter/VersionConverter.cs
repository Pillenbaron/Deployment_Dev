using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using awinta.Deployment_NET.Data;

namespace awinta.Deployment_NET.Converter
{
    internal class VersionConverter : IValueConverter
    {
        private const string RegexExpression =
            @"^.*?(?<Hauptversion>\d+)\.(?<Nebenversion>\d+)\.(?<Buildnummer>\d+)\.(?<Revision>\d+).*?$";

        private const string Hauptversion = "Hauptversion";
        private const string Nebenversion = "Nebenversion";
        private const string Buildnummer = "Buildnummer";
        private const string Revision = "Revision";


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var version = value as VersionData;

            return version?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetVersionNumber(value);
        }

        public VersionData GetVersionNumber(object value)
        {
            var text = value as string;


            if (text != null)
            {
                var options = RegexOptions.Multiline;

                var matches = Regex.Matches(text, RegexExpression, options);
                foreach (Match match in matches)
                {
                    var output = new VersionData
                    {
                        Hauptversion = System.Convert.ToInt16(match.Groups[Hauptversion].Value),
                        Nebenversion = System.Convert.ToInt16(match.Groups[Nebenversion].Value),
                        Buildnummer = System.Convert.ToInt16(match.Groups[Buildnummer].Value),
                        Revision = System.Convert.ToInt16(match.Groups[Revision].Value)
                    };

                    return output;
                }
            }

            return null;
        }
    }
}