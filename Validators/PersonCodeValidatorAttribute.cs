using System.ComponentModel.DataAnnotations;

namespace Egzaminas.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PersonCodeValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var personCode = value as string;
            if (!string.IsNullOrEmpty(personCode))
            {

                foreach (char c in personCode)
                {
                    if (!char.IsDigit(c))
                    {
                        return new ValidationResult("Person code can be only digits [0-9]");
                    }
                }

                if (personCode.Length != 11)
                    return new ValidationResult("Person code length must be 11.");
            }

            return ValidationResult.Success!;
        }
    }
}
