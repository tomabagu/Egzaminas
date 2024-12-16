using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Egzaminas.Entities
{
    public class Person
    {
        public Guid PersonId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string PersonCode { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PersonImageBytes { get; set; }
        public Guid? AccountId { get; set; }
        public Account? Account { get; set; }
        public Guid? AddressId { get; set; }
        public Address? Address { get; set; }
    }
}
