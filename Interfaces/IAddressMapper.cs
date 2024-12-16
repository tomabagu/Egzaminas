using Egzaminas.Dtos.Requests;
using Egzaminas.Dtos.Results;
using Egzaminas.Entities;

namespace Egzaminas.Interfaces
{
    public interface IAddressMapper
    {
        Address Map(AddressRequestDto dto);
        AddressResultDto Map(Address dto);
        void Project(Address address, AddressRequestDto req);
    }
}