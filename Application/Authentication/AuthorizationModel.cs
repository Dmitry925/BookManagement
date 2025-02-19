using System;
using System.Collections.Generic;

namespace BookManagement.Core.Application.Authentication
{
    public class AuthorizationModel
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
