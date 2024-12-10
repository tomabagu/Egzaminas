using Egzaminas.Entities;

namespace Egzaminas.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AccountsDbContext _context;

        public Guid Update(Person model)
        {
            _context.Persons.Add(model);
            _context.SaveChanges();
            return model.Id;
        }
        public Person? Get(Guid accountId)
        {
            if (accountId == null)
                throw new ArgumentNullException(nameof(accountId));

            return _context.Persons.FirstOrDefault(x => x.AccountId == accountId);
        }

    }
}
