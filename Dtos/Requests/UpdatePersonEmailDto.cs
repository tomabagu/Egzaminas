using Egzaminas.Validators;
using System.ComponentModel.DataAnnotations;

namespace Egzaminas.Dtos.Requests
{
    public class UpdatePersonEmailDto
    {
        public Guid PersonId { get; set; }
        [EmailAddress]
        [EmailDomainValidator]
        public string PersonEmail { get; set; } = null!;
    }
}
