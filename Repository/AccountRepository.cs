using Egzaminas.Entities;
using Egzaminas.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Egzaminas.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountsDbContext _context;

        public AccountRepository(AccountsDbContext context)
        {
            _context = context;
        }

        public Guid SaveAccount(Account model)
        {
            _context.Accounts.Add(model);
            _context.SaveChanges();
            return model.AccountId;
        }
        public Account? GetAccount(string userName)
        {
            return _context.Accounts.FirstOrDefault(x => x.UserName == userName);
        }

        public Account? GetAccountByGuid(Guid id)
        {
            return _context.Accounts.Include(a => a.Person).ThenInclude(p => p.Address).FirstOrDefault(x => x.AccountId == id);
        }
        public void Delete(Account account)
        {
            if (account.Person != null)
            {
                _context.Persons.Remove(account.Person);
            }
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }

        public ICollection<Account> GetAllAccounts()
        {
            return _context.Accounts.Where(a => a.Role != "Admin").ToList();
        }
    }
}
