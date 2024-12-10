using Egzaminas.Dtos.Requests;
using Egzaminas.Entities;
using Egzaminas.Interfaces;

namespace Egzaminas.Mappers
{
    public class AddressMapper : IAddressMapper
    {
        public Address Map(AddressRequestDto dto)
        {
            return new Address
            {
                City = dto.City,
                Street = dto.Street,
                Number = dto.Number,
            };
        }
    }
}
