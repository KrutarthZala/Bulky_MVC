using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyBook_WebAPI.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; }
        public string? ProductTitle { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductISBN { get; set; }
        public string? ProductAuthor { get; set; }

        [Range(1, 1000)]
        public double ProductListPrice { get; set; }

        [Range(1, 1000)]
        public double ProductPrice { get; set; }

        [Range(1, 1000)]
        public double ProductPrice50 { get; set; }

        [Range(1, 1000)]
        public double ProductPrice100 { get; set; }

        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public CategoryModel? Category { get; set; }

        public string? ProductImageURL { get; set; }
    }
}
