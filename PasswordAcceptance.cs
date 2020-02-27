using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace petpet {
    public class PasswordAcceptance : ValidationAttribute {
        protected override ValidationResult IsValid (object value, ValidationContext validationContext) {
            if (value == null) {
                return new ValidationResult ("Password must be at least 8 characters containing 1 letter, 1 number, and 1 special character.");
            }
            Regex regex = new Regex (@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");
            Match match = regex.Match (value.ToString ());
            if (!match.Success) {
                return new ValidationResult ("Password must be at least 8 characters containing 1 letter, 1 number, and 1 special character.");
            }
            return ValidationResult.Success;
        }
    }
}