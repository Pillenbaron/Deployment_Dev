using System.Globalization;
using System.Windows.Controls;

namespace awinta.Deployment_NET.Validation
{
    class NumericValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            var Number = value as string;
            int Output;

            if (value is string && string.IsNullOrWhiteSpace(Number) && int.TryParse(Number, out Output))
            {

                return new ValidationResult(true, null);

            }
            else
            {

                return new ValidationResult(false, "Eingabe ist keine gültige Zahl!");

            }

        }
    }
}
