using Microsoft.AspNetCore.Identity;
using System;

namespace BookManagement.Core.Domain.Models
{
    public class User : IdentityUser<Guid>
    {
    }
}
