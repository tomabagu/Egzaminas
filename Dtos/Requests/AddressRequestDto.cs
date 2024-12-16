using Egzaminas.Validators;
using System.ComponentModel.DataAnnotations;

namespace Egzaminas.Dtos.Requests
{
    public class AddressRequestDto
    {
        [NotNullOrWhiteSpace]
        public string City { get; set; }
        [NotNullOrWhiteSpace]
        public string Street { get; set; }
        [NotNullOrWhiteSpace]
        public string Number { get; set; }
    }
}
