using Egzaminas.Dtos.Requests;
using Egzaminas.Dtos.Results;
using Egzaminas.Entities;
using Egzaminas.Interfaces;
using Egzaminas.Services;
using System;

namespace Egzaminas.Mappers
{
    public class PersonMapper : IPersonMapper
    {
        public PersonResultDto Map(Person entity)
        {
            return new PersonResultDto
            {
                PersonId = entity.PersonId.ToString(),
                Name = entity.Name,
                Surname = entity.Surname,
                PersonCode = entity.PersonCode,
                Email = entity.Email,
                ProfilePicture = Convert.ToBase64String(entity.PersonImageBytes),
                AccountId = entity.AccountId.ToString(),
                AddressId = entity.AddressId.ToString()

            };
        }
        public Person Map(PersonRequestDto dto)
        {
            return new Person
            {
                Name = dto.Name!,
                Surname = dto.Surname,
                PersonCode = dto.PersonCode,
                Email = dto.Email,
                PersonImageBytes = ImageService.ResizeImage(dto.ProfilePicture)
            };
        }

        public byte[] ConvertToByteArray(IFormFile file)
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
        }

        public AllAccountDataResultDto Map(Account account)
        {
            return new AllAccountDataResultDto
            {
                Person = account.Person != null ? Map(account.Person): null,
                Address = account.Person?.Address != null ? new AddressResultDto
                {
                    AddressId = account.Person.Address.AddressId.ToString(),
                    Street = account.Person.Address.Street,
                    City = account.Person.Address.City,
                    Number = account.Person.Address.Number,
                }: null
            };
        }

    }
}
