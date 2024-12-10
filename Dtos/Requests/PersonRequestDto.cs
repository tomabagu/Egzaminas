using Egzaminas.Validators;
using System.ComponentModel.DataAnnotations;

namespace Egzaminas.Dtos.Requests
{
    public record PersonRequestDto
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        [PersonCodeValidator]
        public string PersonCode { get; set; } = null!;
        
        [EmailAddress]
        [EmailDomainValidator]
        public string Email { get; set; } = null!;

        public IFormFile ProfilePicture { get; set; }
    }
}
