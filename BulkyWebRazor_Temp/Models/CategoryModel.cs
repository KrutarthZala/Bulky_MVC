using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyBookWebRazor_Temp.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string CategoryName { get; set; }

        [DisplayName("Category Display Order")]
        [Range(1, 100, ErrorMessage = "The field Category Display Order must be between 1-100.")]
        public int CategoryOrder { get; set; }
    }
}
