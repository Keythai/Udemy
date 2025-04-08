using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ModelValidationsExample.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace ModelValidationsExample.Models
{
    // Validate multiple fields
    public class Person : IValidatableObject // IValidatableObject is not reusable but can achieve fast and simple data validation
    {
        [Required(ErrorMessage = "{0} cannot be empty or null.")] // {0} is the name of the property
        [Display (Name = "Person Name")] // Change the displayed name of the property
        [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} characters.")] // {1} is maximum length (first argument), {2} is minimum length (second argument)
        [RegularExpression("^[A-Za-z .]+$", ErrorMessage = "{0} would contain only alphabets, space and dot(.)")]
        public string? PersonName { get; set; }
        [EmailAddress(ErrorMessage = "{0} is not a valid email address.")]
        public string? Email { get; set; }
        [Phone(ErrorMessage = "{0} should contain 10 digits.")]
        //[ValidateNever] // Temporarily disable validation
        public string? Phone { get; set; }
        [Required(ErrorMessage = "{0} cannot be empty or null.")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "{0} cannot be empty or null.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]  // Compare 2 properties, returns error message if they don't match
        public string? ConfirmPassword { get; set; }

        [Range(0, 999.99, ErrorMessage = "{0} should be between ${1} and ${2}")] // Range is applied to numbers (int, double, etc.)
        public double? Price { get; set; }

        [MinimumYearValidator(2005)]
        //[BindNever] // Prevents the property from being bound to the request
        public DateTime? DateOfBirth { get; set; }

        public DateTime? FromDate { get; set; }
        [DateRangeValidator("FromDate", ErrorMessage = "FromDate should be earlier than ToDate.")]
        public DateTime? ToDate { get; set; }
        public int? Age { get; set; }

        public List<string?> Tags { get; set; } = new List<string?>();
        public override string ToString()
        {
            return $"PersonName: {PersonName}, Email: {Email}, Phone: {Phone}, Password: {Password}, ConfirmPassword: {ConfirmPassword}, Price: {Price}";
        }

        // IValidatableObject method
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DateOfBirth.HasValue == false && Age.HasValue == false)
            {
                yield return new ValidationResult("Either DateOfBirth or Age is required.", new string[] { "DateOfBirth", "Age" });
            }
        }
    }
}
