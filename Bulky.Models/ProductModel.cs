using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class ProductModel
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [DisplayName("Book Title")]
        public string? ProductTitle { get; set; }
        
        [Required]
        [DisplayName("Book Description")]
        public string? ProductDescription { get; set; }

        [Required]
        [DisplayName("Book ISBN")]
        public string? ProductISBN { get; set; }

        [Required]
        [DisplayName("Book Author")]
        public string? ProductAuthor { get; set; }

        [Required]
        [DisplayName("List Price")]
        [Range(1, 1000)]
        public double ProductListPrice { get; set; }

        [Required]
        [DisplayName("Price for 1-50")]
        [Range(1, 1000)]
        public double ProductPrice { get; set; }

        [Required]
        [DisplayName("Price for 50+")]
        [Range(1, 1000)]
        public double ProductPrice50 { get; set; }

        [Required]
        [DisplayName("Price for 100+")]
        [Range(1, 1000)]
        public double ProductPrice100 { get; set; }

    }
}
