using Egzaminas.Entities;
using Egzaminas.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Egzaminas.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(AccountsDbContext context) : base(context) { }
    }
}
