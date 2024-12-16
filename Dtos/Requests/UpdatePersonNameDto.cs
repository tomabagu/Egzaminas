using Egzaminas.Validators;

namespace Egzaminas.Dtos.Requests
{
    public class UpdatePersonNameDto
    {
        public Guid PersonId { get; set; }
        [NotNullOrWhiteSpace]
        public string PersonName { get; set; }
    }
}
