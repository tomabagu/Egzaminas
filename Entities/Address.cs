using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Egzaminas.Entities
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
       
    }
}
