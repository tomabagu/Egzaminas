using Egzaminas.Dtos.Requests;
using Egzaminas.Dtos.Results;
using Egzaminas.Entities;

namespace Egzaminas.Interfaces
{
    public interface IPersonMapper
    {
        byte[] ConvertToByteArray(IFormFile file);
        Person Map(PersonRequestDto dto);
        PersonResultDto Map(Person entity);
        AllAccountDataResultDto Map(Account account);
        void Project(Person person, PersonRequestDto req);
    }
}