using Egzaminas.Entities;

namespace Egzaminas.Interfaces
{
    public interface IJwtService
    {
        string GetJwtToken(string username, string role, string accountId);
    }
}