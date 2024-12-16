using System.ComponentModel.DataAnnotations;

namespace Egzaminas.Validators
{
    public class UsernameValidatorAttribute : ValidationAttribute
    {
        public int MinimumLength { get; set; } = 4;
        public int MaximumLength { get; set; } = 60;
        public bool RequireUppercase { get; set; } = true;

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("username is required.");
            }

            var username = value.ToString();

            if (string.IsNullOrEmpty(username))
            {
                return new ValidationResult("username is required.");
            }

            if (username.Length < MinimumLength)
            {
                return new ValidationResult($"username must be at least {MinimumLength} characters long.");
            }

            if (username.Length > MaximumLength)
            {
                return new ValidationResult($"username must be at most {MaximumLength} characters long.");
            }

            if (RequireUppercase && !username.Any(char.IsUpper))
            {
                return new ValidationResult("username must contain at least one uppercase letter.");
            }

            return ValidationResult.Success;
        }
    }
}
