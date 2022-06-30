using System.ComponentModel.DataAnnotations;

namespace backend_task3.Models
{
    public class Register
    {
        [Required, UniqueName]

        public string Username { get; set; }

        [Required, EmailAddress, UniqueEmail]



        public string Email { get; set; }

        [Required]
        [Minimamage(18)]

        
        
        public string Birthday { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
