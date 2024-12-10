using Egzaminas.Dtos.Requests;
using Egzaminas.Entities;
using Egzaminas.Interfaces;

namespace Egzaminas.Mappers
{
    public class AccountMapper : IAccountMapper
    {
        private readonly IAccountService _service;

        public AccountMapper(IAccountService service)
        {
            _service = service;
        }

        public Account Map(AccountRequestDto dto)
        {
            _service.CreatePasswordHash(dto.Password!, out var passwordHash, out var passwordSalt);
            return new Account
            {
                UserName = dto.UserName!,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = dto.Role!
            };
        }

    }
}
