using AutoMapper;
using BookManagement.Core.Application.DTOs.UserDTOs;
using BookManagement.Core.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Features.UserFeatures.Queries
{
    public class GetAllUsersQuery
    {
        public class Request : IRequest<Response>
        {
            
        }
        
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;

            public Handler(UserManager<User> userManager, IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var response = _mapper.Map<Response>(await _userManager.Users.AsNoTracking().ToListAsync());

                return response;
            }
        }

        public class Response : List<UserDetailsDto>
        {
        }
    }
}
