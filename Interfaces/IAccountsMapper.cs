using Egzaminas.Dtos.Results;
using Egzaminas.Entities;

namespace Egzaminas.Interfaces
{
    public interface IAccountsMapper
    {
        AccountsResultDto Map(Account entity);
        ICollection<AccountsResultDto> Map(ICollection<Account> entities);
    }
}