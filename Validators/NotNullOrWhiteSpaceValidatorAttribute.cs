using System.ComponentModel.DataAnnotations;

namespace Egzaminas.Validators
{
    public class NotNullOrWhiteSpaceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult($"{validationContext.DisplayName} cannot be null or whitespace.");
            }

            return ValidationResult.Success;
        }
    }
}
