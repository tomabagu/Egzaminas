using Egzaminas.Validators;

namespace Egzaminas.Dtos.Requests
{
    public class UpdatePersonCodeDto
    {
        public Guid PersonId { get; set; }
        [PersonCodeValidator]
        [NotNullOrWhiteSpace]
        public string PersonCode { get; set; } = null!;
    }
}
