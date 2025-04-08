using System.ComponentModel.DataAnnotations;

namespace ModelValidationsExample.CustomValidators
{
    // Single field validation
    public class MinimumYearValidatorAttribute : ValidationAttribute
    {
        public int MinimumYear { get; set; } = 2000;
        public string DefaultErrorMessage { get; set; } = "{0} should be less than Jan 01, {1}";
        public MinimumYearValidatorAttribute() { }
        public MinimumYearValidatorAttribute(int minimumYear)
        {
            MinimumYear = minimumYear;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value != null)
            {
                DateTime date = (DateTime)value;
                if(date.Year >= MinimumYear)
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, validationContext.DisplayName, MinimumYear)); // Mandatory for passing argument {0} in ErrorMessage
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            return null;
        }
    }
}
