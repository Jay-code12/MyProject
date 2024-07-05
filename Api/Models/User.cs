using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Api.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FIrstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow; 
    }
}
