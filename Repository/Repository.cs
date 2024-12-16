using Egzaminas.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Egzaminas.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AccountsDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AccountsDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public virtual T? Get(Guid id)
        {
            return _dbSet.Find(id);
        }
        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
