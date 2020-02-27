using System;
using System.ComponentModel.DataAnnotations;

namespace petpet.Models {
    public class Comment {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string Content { get; set; }
        public int UserId { get; set; }
        public User Author { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}