using System.Collections.Generic;
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

        public virtual ICollection<ProductOption> ProductOptions { get; set; }
    }

    public class ProductOption
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public ProductOptionType Type { get; set; }

        public string Value { get; set; }
        
        public virtual  Product Product { get; set; }
    }

    public enum ProductOptionType
    {
        Color, 
        Size,
    }
}