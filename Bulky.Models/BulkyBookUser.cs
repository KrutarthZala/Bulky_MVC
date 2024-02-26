using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
    public class BulkyBookUser : IdentityUser
    {
        [Required]
        public string? BulkyBookUserName {  get; set; }
        public string? UserStreetAddress {  get; set; }
        public string? UserCity { get; set;}
        public string? UserState {  get; set; }
        public string? UserPostalCode { get; set;}

        public int? CompanyID { get; set;}
        [ForeignKey("CompanyID")]
        [ValidateNever]
        public CompanyModel Company { get; set; }
    }
}
