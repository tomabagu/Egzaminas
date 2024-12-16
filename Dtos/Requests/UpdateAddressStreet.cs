using Egzaminas.Validators;

namespace Egzaminas.Dtos.Requests
{
    public class UpdateAddressStreet
    {
        public Guid AddressId { get; set; }
        [NotNullOrWhiteSpace]
        public string AddressStreet { get; set; }
    }
}
