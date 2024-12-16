using Egzaminas.Interfaces;
using System.Security.Cryptography;

namespace Egzaminas.Services
{
    public class LoginService : ILoginService
    {
        private readonly IAccountRepository _repository;

        public LoginService(IAccountRepository repository)
        {
            _repository = repository;
        }
        public bool Login(string username, string password, out string role, out string? accountId)
        {
            var account = _repository.GetAccount(username);
            if (account == null)
            {
                role = "";
                accountId = null;
                return false;
            }
            role = account.Role;
            accountId = account.AccountId.ToString();
            if (VerifyPasswordHash(password, account.PasswordHash, account.PasswordSalt))
            {
                return true;
            }
            return false;
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
