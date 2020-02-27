using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace petpet.Models {
    public class Pet {
        [Key]
        public int PetId { get; set; }

        [Required]
        [MinLength (2, ErrorMessage = "Name must be at least 2 characters")]
        public string PetName { get; set; }
        public int PetValue { get; set; }
        public int PetLevel { get; set; }

        public double PetExperience { get; set; }
        public int PetHappiness { get; set; }
        public int PetHunger { get; set; }
        public bool isAdult { get; set; } = false;

        public string PetPicture { get; set; }

        [Required]
        [MaxLength (250)]
        public string PetBio { get; set; }
        public List<Comment> PetComments { get; set; }
        public int UserId { get; set; }
        public User Creator { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}