using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace perfDemo.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        public string FirstName { get; set; }

        [MaxLength(250)]
        public string LastName { get; set; }

        [MaxLength(250)]
        public string Address1 { get; set; }

        [MaxLength(250)]
        public string Address2 { get; set; }

        [MaxLength(250)]
        public string City { get; set; }

        [MaxLength(250)]
        public string State { get; set; }

        [MaxLength(10)]
        public string PostalCode { get; set; } 

        public virtual ICollection<Order> Orders { get; set; }
    }
}