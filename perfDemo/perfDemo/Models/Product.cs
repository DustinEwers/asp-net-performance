using System.ComponentModel.DataAnnotations;

namespace perfDemo.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}