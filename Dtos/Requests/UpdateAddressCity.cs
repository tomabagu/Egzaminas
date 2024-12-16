using Egzaminas.Validators;

namespace Egzaminas.Dtos.Requests
{
    public class UpdateAddressCity
    {
        public Guid AddressId { get; set; }
        [NotNullOrWhiteSpace]
        public string AddressCity { get; set; }
    }
}
