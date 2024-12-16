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
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var exists = _context.Accounts.Any(x => x.UserName == model.UserName);
            if (exists)
                throw new ArgumentException("Username already exists");

            _context.Accounts.Add(model);
            _context.SaveChanges();
            return model.AccountId;
        }
        public Account? GetAccount(string userName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            return _context.Accounts.FirstOrDefault(x => x.UserName == userName);
        }

        public Account? GetAccountByGuid(Guid id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return _context.Accounts.Include(a => a.Person).ThenInclude(p => p.Address).FirstOrDefault(x => x.AccountId == id);
        }
        public void Delete(Account account)
        {
            if (account != null)
            {
                if (account.Person != null)
                {
                    if (account.Person.Address != null)
                    {
                        _context.Addresses.Remove(account.Person.Address);
                    }
                    _context.Persons.Remove(account.Person);
                }
                _context.Accounts.Remove(account);
                _context.SaveChanges();
            }

        }
    }
}
