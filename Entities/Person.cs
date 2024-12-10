using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Egzaminas.Entities
{
    public class Person
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string PersonCode { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PersonImageBytes { get; set; }

        [ForeignKey(nameof(Account))]
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        [ForeignKey(nameof(Address))]
        public Guid AddressId { get; set; }
        public Address Address { get; set; } = null!;
    }
}
