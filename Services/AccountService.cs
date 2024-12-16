using System.Security.Cryptography;
using System.Text;
using Egzaminas.Entities;
using Egzaminas.Interfaces;

namespace Egzaminas.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }
        public Account SignupNewAccount(string username, string password)
        {
            var account = CreateAccount(username, password);
            _repository.SaveAccount(account);
            return account;
        }
        private Account CreateAccount(string username, string password)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var account = new Account
            {
                UserName = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User"
            };
            return account;
        }
        public Account ChangeUserName(string username, string password, string newUsername)
        {
            var account = _repository.GetAccount(username);
            if (VerifyPasswordHash(password, account.PasswordHash, account.PasswordSalt))
            {
                account.UserName = newUsername;
                _repository.SaveAccount(account);
                return account;
            }

            return null;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
