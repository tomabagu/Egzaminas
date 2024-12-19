using Egzaminas.Dtos.Results;
using Egzaminas.Entities;
using Egzaminas.Interfaces;

namespace Egzaminas.Mappers
{
    public class AccountsMapper : IAccountsMapper
    {
        public AccountsResultDto Map(Account entity)
        {
            return new AccountsResultDto
            {
                AccountId = entity.AccountId,
                Username = entity.UserName
            };
        }

        public ICollection<AccountsResultDto> Map(ICollection<Account> entities)
        {
            return entities.Select(entity => new AccountsResultDto
            {
                AccountId = entity.AccountId,
                Username = entity.UserName
            }).ToList();
        }
    }
}
