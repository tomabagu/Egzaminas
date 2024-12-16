namespace Egzaminas.Interfaces
{
    public interface ILoginService
    {
        bool Login(string username, string password, out string role, out string accountId);
    }
}