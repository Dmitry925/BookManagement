using System;

namespace BookManagement.Core.Application.Infrastructure.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string name)
            : base($"{name} already exists.")
        {
            
        }
    }
}
