using System;

namespace BookManagement.Core.Application.Infrastructure.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string title)
            : base($"{title} was not found.")
        {

        }
    }
}
