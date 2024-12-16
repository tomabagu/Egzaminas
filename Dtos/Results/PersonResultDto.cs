using Egzaminas.Validators;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Egzaminas.Dtos.Results
{
    public class PersonResultDto
    {
        public string PersonId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string PersonCode { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ProfilePicture { get; set; }
        public string? AccountId { get; set; }
        public string? AddressId { get; set; }
    }
}
