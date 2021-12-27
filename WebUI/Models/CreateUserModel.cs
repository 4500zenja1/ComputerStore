using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class CreateUserModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}