using Egzaminas.Dtos.Requests;
using Egzaminas.Dtos.Results;
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

        public AddressResultDto Map(Address dto)
        {
            return new AddressResultDto
            {
                AddressId = dto.AddressId.ToString(),
                City = dto.City,
                Street = dto.Street,
                Number = dto.Number,
            };
        }

        public void Project(Address address, AddressRequestDto req)
        {
            address.City = req.City;
            address.Street = req.Street;
            address.Number = req.Number;
        }
    }
}
