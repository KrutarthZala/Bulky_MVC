using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class ShoppingCartModel
    {
        [Key]
        public int CartID { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [ValidateNever]
        public ProductModel Product { get; set; }

        [Range(1, 1000, ErrorMessage ="Please enter value between 1 and 100")]
        public int ProductCount { get; set; }

        public string BulkyBookUserID { get; set; }
        [ForeignKey("BulkyBookUserID")]
        public BulkyBookUser BulkyBookUser { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
