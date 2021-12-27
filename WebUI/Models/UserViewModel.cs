using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class UserViewModel
    {
        public class CreateUserViewModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class LoginViewModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class RoleEditModel
        {
            public AppRole Role { get; set; }
            public IEnumerable<AppUser> Members { get; set; }
            public IEnumerable<AppUser> NonMembers { get; set; }
        }

        public class RoleModificationModel
        {
            [Required]
            public string RoleName { get; set; }
            public string[] IdsToAdd { get; set; }
            public string[] IdsToDelete { get; set; }
        }
    }
}