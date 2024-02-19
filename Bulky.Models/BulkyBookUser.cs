﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class BulkyBookUser : IdentityUser
    {
        [Required]
        public string BulkyBookUserName {  get; set; }

        public string? UserStreetAddress {  get; set; }
        public string? UserCity { get; set;}
        public string? UserState {  get; set; }
        public string? UserPostalCode { get; set;}
    }
}
