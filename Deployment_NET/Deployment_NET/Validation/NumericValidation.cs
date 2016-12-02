using System.Globalization;
using System.Windows.Controls;

namespace awinta.Deployment_NET.Validation
{
    internal class NumericValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            var number = value as string;
            int output;

            var s = value as string;
            if (s != null && string.IsNullOrWhiteSpace(number) && int.TryParse(number, out output))
            {

                return new ValidationResult(true, null);

            }
            return new ValidationResult(false, "Eingabe ist keine gültige Zahl!");
        }
    }
}
