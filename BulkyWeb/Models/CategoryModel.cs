using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; }
        public int CategoryOrder { get; set; }
    }
}
