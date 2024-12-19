using System.Security.Cryptography;
using System.Text;
using Egzaminas.Entities;
using Egzaminas.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            var exists = _repository.GetAccount(username);
            if (exists != null)
            {
                throw new ArgumentException("Username already exists");
            }
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
