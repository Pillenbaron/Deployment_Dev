using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace awinta.Deployment_NET.Converter
{
    class VersionConverter : System.Windows.Data.IValueConverter
    {

        private const string RegexExpression = @"^.*?(?<Hauptversion>\d+)\.(?<Nebenversion>\d+)\.(?<Buildnummer>\d+)\.(?<Revision>\d+).*?$";
        private const string hauptversion = "Hauptversion";
        private const string nebenversion = "Nebenversion";
        private const string buildnummer = "Buildnummer";
        private const string revision = "Revision";

        public Solution.Model.VersionData GetVersionNumber(object value)
        {

            var text = value as string;


            if (text != null)
            {

                RegexOptions options = RegexOptions.Multiline;
                Solution.Model.VersionData output = null;

                MatchCollection matches = Regex.Matches(text, RegexExpression, options);
                foreach (Match match in matches)
                {

                    output = new Solution.Model.VersionData()
                    {
                        Hauptversion = System.Convert.ToInt16(match.Groups[hauptversion].Value),
                        Nebenversion = System.Convert.ToInt16(match.Groups[nebenversion].Value),
                        Buildnummer = System.Convert.ToInt16(match.Groups[buildnummer].Value),
                        Revision = System.Convert.ToInt16(match.Groups[revision].Value)
                    };

                    return output;

                }
            }

            return null;

        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var Version = value as Solution.Model.VersionData;

            if (Version != null) { return Version.ToString(); }

            return string.Empty;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetVersionNumber(value);
        }
    }
}
