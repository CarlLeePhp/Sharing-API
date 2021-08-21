using System.ComponentModel.DataAnnotations;

namespace Sharing_API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
    }
}
