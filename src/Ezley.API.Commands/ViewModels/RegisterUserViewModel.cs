using System;
using System.Collections.Generic;
using Ezley.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ezley.API.Commands.ViewModels
{
    public class RegisterUserViewModel:IValidatableObject
    {
        [Required]
        public Guid? Id { get; set; }
        
        // Auth0 User fields:
        [Required]
        [EmailAddress]
        public string EmailUserName { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password min length is 8 characters.")]
        public string Password { get; set; }
        
        // User fields
       // public PersonName PersonName { get; set; }
       // public DisplayName DisplayName { get; set; }
       // public Address Address { get; set; }
       // public Phone Phone { get; set; }
       // public Email Email { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
           
            // 1) check for number
            if(!Password.Any(Char.IsNumber))
            {
                result.Add( new ValidationResult(
                    $"Password must contain at least one number.",
                    new[] { nameof(Password)}));
            }
            // 2) check for special char
            char[] special = new char[] {'!', '@', '#', '$', '%', '^', '&', '*'}; // Shift + (1-8)
            if (!Password.Any(c => special.Contains(c)))
            {
                result.Add( new ValidationResult(
                    $"Password must contain at least one special character (!@#$%^&*).",
                    new[] { nameof(Password)}));
            }
            // 3) check for capital
            if (!Password.Any(Char.IsUpper))
            {
                result.Add( new ValidationResult(
                    $"Password must contain at least one capital letter.",
                    new[] { nameof(Password)}));
            }
            // 4) check for lower
            if (!Password.Any(Char.IsLower))
            {
                result.Add( new ValidationResult(
                    $"Password must contain at least one lower-case letter.",
                    new[] { nameof(Password)}));
            }
            return result;
            
        }
    }
}