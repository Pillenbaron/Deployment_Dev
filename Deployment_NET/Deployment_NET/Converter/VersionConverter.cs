using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace awinta.Deployment_NET.Converter
{
    class VersionConverter : System.Windows.Data.IValueConverter
    {

        private const string RegexExpression = @"^.*?(?<Hauptversion>\d+)\.(?<Nebenversion>\d+)\.(?<Buildnummer>\d+)\.(?<Revision>\d+).*?$";
        private const string Hauptversion = "Hauptversion";
        private const string Nebenversion = "Nebenversion";
        private const string Buildnummer = "Buildnummer";
        private const string Revision = "Revision";

        public Solution.Model.VersionData GetVersionNumber(object value)
        {

            var text = value as string;


            if (text != null)
            {

                RegexOptions options = RegexOptions.Multiline;

                MatchCollection matches = Regex.Matches(text, RegexExpression, options);
                foreach (Match match in matches)
                {
                    var output = new Solution.Model.VersionData()
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


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var version = value as Solution.Model.VersionData;

            return version?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetVersionNumber(value);
        }
    }
}
