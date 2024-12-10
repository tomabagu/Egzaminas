using Egzaminas.Dtos.Requests;
using Egzaminas.Entities;

namespace Egzaminas.Interfaces
{
    public interface IAddressMapper
    {
        Address Map(AddressRequestDto dto);
    }
}