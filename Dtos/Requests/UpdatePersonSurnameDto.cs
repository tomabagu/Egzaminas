using Egzaminas.Validators;

namespace Egzaminas.Dtos.Requests
{
    public class UpdatePersonSurnameDto
    {
        public Guid PersonId { get; set; }
        [NotNullOrWhiteSpace]
        public string PersonSurname { get; set; }
    }
}
