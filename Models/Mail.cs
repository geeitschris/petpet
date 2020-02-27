using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace petpet.Models {
    public class Mail {
        [Key]
        public int MailId { get; set; }
        public string Subject { get; set; } = "No Subject";

        [Required]
        public string Content { get; set; }
        public int UserId { get; set; }
        public User Author { get; set; }
        public string AuthorName { get; set; }

        [Required]
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}