using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Maximum of 10 digits")]
        [MinLength(10)]
        public string PhoneNumber { get; set; }
    }
}
