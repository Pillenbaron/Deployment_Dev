using System;
using System.Globalization;
using System.Windows.Controls;

namespace awinta.Deployment_NET.Validation
{
    public class UriValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            var Path = value as string;
            Uri DeployPath = null;

            if (string.IsNullOrWhiteSpace(Path) && Uri.TryCreate(Path, UriKind.Absolute, out DeployPath))
            {

                return new ValidationResult(true, null);

            }
            else
            {

                return new ValidationResult(false, "Kein Gültiger Pfad!");

            }

        }
    }
}
