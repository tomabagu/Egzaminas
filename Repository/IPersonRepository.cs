using Egzaminas.Entities;

namespace Egzaminas.Repository
{
    public interface IPersonRepository
    {
        Person? Get(Guid accountId);
        Guid Update(Person model);
    }
}