using BookManagement.Core.Domain.Enums;
using BookManagement.Core.Domain.Models;
using System;
using System.Collections.Generic;

namespace BookManagement.Infrastructure.Persistence.Seeds
{
    public static class DemoUsers
    {
        public static Dictionary<BaseRole, List<User>> DemoUsersList { get; set; }

        public static string DefaultPassword { get; set; } = @"Pa$$w0rd.";

        static DemoUsers()
        {
            DemoUsersList = new Dictionary<BaseRole, List<User>>
            {
                {
                    BaseRole.Admin,
                    new List<User>
                    {
                        new User
                        {
                            Id = Guid.NewGuid(),
                            UserName = "Admin1"
                        },
                        new User
                        {
                            Id = Guid.NewGuid(),
                            UserName = "Admin2"
                        }
                    }
                },
                { 
                    BaseRole.User,
                    new List<User>
                    {
                        new User
                        {
                            Id = Guid.NewGuid(),
                            UserName = "User1"
                        },
                        new User
                        {
                            Id = Guid.NewGuid(),
                            UserName = "User2"
                        }
                    }
                }
            };
        }
    }
}
