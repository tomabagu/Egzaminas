using Egzaminas.Validators;

namespace Egzaminas.Dtos.Requests
{
    public class UpdateAddressNumber
    {
        public Guid AddressId { get; set; }
        [NotNullOrWhiteSpace]
        public string AddressNumber { get; set; }
    }
}
