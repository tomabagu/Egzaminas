using Egzaminas.Dtos.Requests;
using Egzaminas.Entities;

namespace Egzaminas.Interfaces
{
    public interface IPersonMapper
    {
        Person Map(PersonRequestDto dto);
        void Project(Person person, PersonRequestDto req);
    }
}