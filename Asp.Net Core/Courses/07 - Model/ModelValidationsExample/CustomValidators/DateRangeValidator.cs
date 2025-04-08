using System.ComponentModel.DataAnnotations;
using System.Reflection; // inspect own metadata at runtime, like properties, methods, etc.

namespace ModelValidationsExample.CustomValidators
{
    public class DateRangeValidator : ValidationAttribute
    {
        public string OtherPropertyName { get; set; }
        public DateRangeValidator(string otherPropertyName)
        {
            OtherPropertyName = otherPropertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) // validationContext is the class object (Person in this case) which uses it
        {
            if(value != null)
            {
                // get to_date
                DateTime to_date = Convert.ToDateTime(value);

                // get from_date
                PropertyInfo? otherProperty = validationContext.ObjectType // select otherpropertyname from objecttype (class) list
                    .GetProperty(OtherPropertyName);
                if (otherProperty != null)
                {
                    DateTime from_date = Convert.ToDateTime(otherProperty.GetValue(validationContext.ObjectInstance));
                    if (to_date < from_date)
                    {
                        return new ValidationResult(ErrorMessage, new string[] { OtherPropertyName, validationContext.MemberName }); // member name represents the name of data which uses this validation rule
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
                return null;
            }


            return null;
        }
    }
}
