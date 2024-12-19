using Egzaminas.Entities;
using Microsoft.EntityFrameworkCore;

namespace Egzaminas
{
    public class AccountsDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public AccountsDbContext(DbContextOptions<AccountsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
               .HasOne(a => a.Person)
               .WithOne(p => p.Account)
               .HasForeignKey<Person>(p => p.AccountId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Address)
                .WithOne(a => a.Person)
                .HasForeignKey<Person>(p => p.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
