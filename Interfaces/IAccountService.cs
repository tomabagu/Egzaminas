using Egzaminas.Entities;

namespace Egzaminas.Interfaces
{
    public interface IAccountService
    {
        Account ChangeUserName(string username, string password, string newUsername);
        Account SignupNewAccount(string username, string password);
    }
}