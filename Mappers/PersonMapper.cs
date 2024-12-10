using Egzaminas.Dtos.Requests;
using Egzaminas.Entities;
using Egzaminas.Interfaces;

namespace Egzaminas.Mappers
{
    public class PersonMapper : IPersonMapper
    {
        public Person Map(PersonRequestDto dto)
        {
            return new Person
            {
                Name = dto.Name!,
                Surname = dto.Surname,
                PersonCode = dto.PersonCode,
                Email = dto.Email,
                PersonImageBytes = ConvertToByteArray(dto.ProfilePicture)
            };
        }

        private static byte[] ConvertToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public void Project(Person person, PersonRequestDto req)
        {
            person.Name = req.Name;
            person.Surname = req.Surname;
            person.PersonCode = req.PersonCode;
            person.Email = req.Email;
            person.PersonImageBytes = ConvertToByteArray(req.ProfilePicture);
            person.AccountId = req.AccountId;
        }
    }
}
