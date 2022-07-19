using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BussinessObject.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        
        public int ProductId { get; set; }
        
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = " This field id required.")]
        [MaxLength(50, ErrorMessage = "Max length is 50."), MinLength(10, ErrorMessage = "Min length is 10.")]
        public string ProductName { get; set; }
        [Required]
        public string Weight { get; set; }
        [Range(1, 10000000)]
        public decimal UnitPrice { get; set; }
        [Required]
        [Range(0, 1000 , ErrorMessage = "This field is reuqired from 0 to 1000")]
        public int UnitsInStock { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }   
}
