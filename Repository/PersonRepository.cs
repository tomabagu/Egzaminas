using Egzaminas.Entities;
using Egzaminas.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Egzaminas.Repository
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(AccountsDbContext context) : base(context) { }

        override public Person? Get(Guid id)
        {
            return _context.Persons.Include(a => a.Address).FirstOrDefault(x => x.PersonId == id);
        }

    }
}
