using System;

namespace awinta.Deployment_NET.Service
{
    internal static class ValidationService
    {

        private const string UrlErrorMessage = "Kein Gültiger Pfad!";
        private const string NumericErrorMessage = "Eingabe ist keine gültige Zahl!";

        #region Validation

        public static string ValidateUri(string value)
        {

            var path = value;
            Uri deployPath;

            if (string.IsNullOrWhiteSpace(path) && Uri.TryCreate(path, UriKind.Absolute, out deployPath))
            {

                return string.Empty;

            }
            return UrlErrorMessage;
        }

        public static string ValidateNumeric(string value)
        {

            var number = value;
            int output;

            if (string.IsNullOrWhiteSpace(number) && int.TryParse(number, out output))
            {

                return string.Empty;

            }
            return NumericErrorMessage;
        }

        #endregion

    }
}
