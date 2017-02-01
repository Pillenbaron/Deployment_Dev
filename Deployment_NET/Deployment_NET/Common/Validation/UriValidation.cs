using System;
using System.Globalization;
using System.Windows.Controls;

namespace awinta.Deployment_NET.Common.Validation
{
    public class UriValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var path = value as string;
            Uri deployPath;

            if (string.IsNullOrWhiteSpace(path) && Uri.TryCreate(path, UriKind.Absolute, out deployPath))
                return new ValidationResult(true, null);
            return new ValidationResult(false, "Kein Gültiger Pfad!");
        }
    }
}