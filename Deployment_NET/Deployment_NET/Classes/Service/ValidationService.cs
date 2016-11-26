using System;

namespace awinta.Deployment_NET.Service
{
    internal sealed class ValidationService
    {

        private const string urlErrorMessage = "Kein Gültiger Pfad!";
        private const string numericErrorMessage = "Eingabe ist keine gültige Zahl!";

        private ValidationService() { }

        #region Validation

        public static string ValidateUri(string Value)
        {

            var Path = Value as string;
            Uri DeployPath = null;

            if (string.IsNullOrWhiteSpace(Path) && Uri.TryCreate(Path, UriKind.Absolute, out DeployPath))
            {

                return string.Empty;

            }
            else
            {

                return urlErrorMessage;

            }

        }

        public static string ValidateNumeric(string Value)
        {

            var Number = Value as string;
            int Output;

            if (Value is string && string.IsNullOrWhiteSpace(Number) && int.TryParse(Number, out Output))
            {

                return string.Empty;

            }
            else
            {

                return numericErrorMessage;

            }

        }

        #endregion

    }
}
