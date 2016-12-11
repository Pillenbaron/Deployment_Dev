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

        public void SampleRegexUsage()
        {
            RegexOptions options = RegexOptions.Multiline;
            string input = @"asdasd1234.0234.0432.0080asdadsad";

            MatchCollection matches = Regex.Matches(input, RegexExpression, options);
            foreach (Match match in matches)
            {
                Console.WriteLine(match.Value);

                Console.WriteLine("Hauptversion:" + match.Groups[hauptversion].Value);

                Console.WriteLine("Nebenversion:" + match.Groups[nebenversion].Value);

                Console.WriteLine("Buildnummer:" + match.Groups[buildnummer].Value);

                Console.WriteLine("Revision:" + match.Groups[revision].Value);

            }
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Version = value as Solution.Model.VersionData;

            if (Version != null) { return Version.ToString(); }

            return string.Empty;
        }
    }
}
