using System.Linq.Expressions;

namespace Egzaminas.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T? Get(Guid id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}