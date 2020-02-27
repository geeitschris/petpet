using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace petpet.Models {
    public class User {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MinLength (2)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType (DataType.Password)]
        [MinLength (8)]
        public string Password { get; set; }

        [Required]
        [Display (Name = "Password")]
        [Compare ("Password", ErrorMessage = "Passwords must match.")]
        [NotMapped]
        public string cPassword { get; set; }
        public int Balance { get; set; } = 5;
        public Pet Pet { get; set; }

        public List<Mail> UserMail { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}