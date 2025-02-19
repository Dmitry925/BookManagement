using System.ComponentModel.DataAnnotations;

namespace BookManagement.Core.Application.Authentication
{
    public class AuthenticationModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
