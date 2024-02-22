using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class CompanyModel
    {
        [Key]
        public int CompanyID { get; set; }

        [Required]
        [DisplayName("Company Name")]
        public string? CompanyName { get; set; }

        [DisplayName("Street Address")]
        public string? CompanyStreetAddress { get; set; }

        [DisplayName("City")]
        public string? CompanyCity { get; set; }

        [DisplayName("State")]
        public string? CompanyState { get; set; }

        [DisplayName("Postal Code")]
        public string? CompanyPostalCode { get; set; }

        [DisplayName("Phone Number")]
        public string? CompanyPhoneNumber { get; set; }

    }
}
