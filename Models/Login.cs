using System.ComponentModel.DataAnnotations;

namespace petpet.Models {
    public class Login {
        [Required]
        [EmailAddress]
        [Display (Name = "Email")]
        public string lemail { get; set; }

        [Required]
        [DataType (DataType.Password)]
        [Display (Name = "Password")]
        public string lpassword { get; set; }
    }
}