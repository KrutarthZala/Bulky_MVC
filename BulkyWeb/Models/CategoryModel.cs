using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [DisplayName("Category Display Order")]
        public int CategoryOrder { get; set; }
    }
}
