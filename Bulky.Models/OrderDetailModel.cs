using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
    public class OrderDetailModel
    {
        [Key]
        public int OrderDetailID { get; set; }

        [Required]
        public int OrderHeaderID { get; set; }
        [ForeignKey("OrderHeaderID")]
        [ValidateNever]
        public OrderHeaderModel OrderHeader { get; set; }

        [Required]
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [ValidateNever]
        public ProductModel Product { get; set; }

        public int ProductCount { get; set; }
        public double ProductPrice { get; set; }
    }
}
