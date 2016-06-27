using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace perfDemo.Models
{
    public class Order
    {
        [Key]
        public int OrderNumber { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }


        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }

        public Customer Customer { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}