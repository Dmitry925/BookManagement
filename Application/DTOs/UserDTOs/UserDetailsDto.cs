using BookManagement.Core.Domain.Enums;
using System;

namespace BookManagement.Core.Application.DTOs.UserDTOs
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public BaseRole Role { get; set; }
    }
}
