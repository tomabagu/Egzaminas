using Egzaminas.Entities;

namespace Egzaminas.Interfaces
{
    public interface IAccountService
    {
        Account SignupNewAccount(string username, string password);
    }
}