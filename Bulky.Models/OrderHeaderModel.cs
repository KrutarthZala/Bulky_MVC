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
    public class OrderHeaderModel
    {
        [Key]
        public int OrderHeaderID { get; set; }

        public string BulkyBookUserID { get; set; }
        [ForeignKey("BulkyBookUserID")]
        [ValidateNever]
        public BulkyBookUser BulkyBookUser { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public double OrderTotal { get; set; }

        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? OrderTrackingNumber { get; set; }
        public string? OrderCarrier { get; set; }

        public DateTime PaymentDate { get; set; }
        public DateOnly PaymentDueDate { get; set; }

        public string? PaymentSessionID {  get; set; }
        public string? PaymentIntentID { get; set; }


        [Required]
        public string UserName {  get; set; }

        [Required]
        public string UserPhoneNumber { get; set; }

        [Required]
        public string UserStreetAddress { get; set; }

        [Required]
        public string UserCity { get; set; }

        [Required]
        public string UserState { get; set; }

        [Required]
        public string UserPostalCode { get; set; }

    }
    
}
