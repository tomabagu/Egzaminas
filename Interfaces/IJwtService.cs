using Egzaminas.Entities;

namespace Egzaminas.Interfaces
{
    public interface IJwtService
    {
        string GetJwtToken(Account account);
    }
}