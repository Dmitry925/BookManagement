using BookManagement.Core.Domain.Enums;

namespace BookManagement.Core.Application.DTOs.UserDTOs
{
    public class SignUpUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public BaseRole Role { get; set; }
    }
}
