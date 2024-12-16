using System.ComponentModel.DataAnnotations;

namespace Egzaminas.Validators
{
    public class AllowedFileExtensionValidatorAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public AllowedFileExtensionValidatorAttribute(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult($"Allowed extensions: {string.Join(" ", _allowedExtensions)}");
                }
            }
            return ValidationResult.Success;
        }
    }
}
