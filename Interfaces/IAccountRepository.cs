using Egzaminas.Entities;

namespace Egzaminas.Interfaces
{
    public interface IAccountRepository
    {
        void Delete(Account account);
        Account? GetAccount(string username);
        Account? GetAccountByGuid(Guid id);
        ICollection<Account> GetAllAccounts();
        Guid SaveAccount(Account account);
    }
}