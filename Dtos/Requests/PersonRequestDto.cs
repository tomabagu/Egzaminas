using Egzaminas.Validators;
using System.ComponentModel.DataAnnotations;

namespace Egzaminas.Dtos.Requests
{
    public class PersonRequestDto
    {
        [NotNullOrWhiteSpace]
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        [PersonCodeValidator]
        [NotNullOrWhiteSpace]
        public string PersonCode { get; set; } = null!;
        
        [EmailAddress]
        [EmailDomainValidator]
        public string Email { get; set; } = null!;
        [AllowedFileExtensionValidator([".jpg"])]
        [MaxFileSize(5 * 1024 * 1024)] //5mb
        public IFormFile ProfilePicture { get; set; }
    }
}
