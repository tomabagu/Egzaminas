using Egzaminas.Validators;
using System.ComponentModel.DataAnnotations;

namespace Egzaminas.Dtos.Requests
{
    public class AccountRequestDto
    {
        [UsernameValidator]
        public string? UserName { get; set; }

        [PasswordValidator]
        public string? Password { get; set; }
    }
}
