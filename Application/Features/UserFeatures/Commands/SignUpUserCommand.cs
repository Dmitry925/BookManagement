using AutoMapper;
using BookManagement.Core.Application.DTOs.UserDTOs;
using BookManagement.Core.Application.Infrastructure.Exceptions;
using BookManagement.Core.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Features.UserFeatures.Commands
{
    public class SignUpUserCommand
    {
        public class Request : SignUpUserDto, IRequest<Response>
        {
            
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {                    
                RuleFor(v => v.UserName)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("UserName mustn't be empty!");

                RuleFor(v => v.Password)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Password mustn't be empty or null!");
            }
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
                if (_userManager.Users.Any(x => x.UserName == request.UserName))
                {
                    throw new AlreadyExistsException(request.UserName);
                }

                var user = _mapper.Map<User>(request);
                user.Id = Guid.NewGuid();

                var createdUser = await _userManager.CreateAsync(user, request.Password);
                if(createdUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, request.Role.ToString());
                }
                else
                {
                    throw new ValidationException(string.Join("\n", createdUser.Errors.Select(x => "\nCode: " + x.Code + "\nDescription: " + x.Description)));
                }

                return new Response
                {
                    UserId = user.Id
                };
            }
        }

        public class Response
        {
            public Guid UserId { get; set; }
        }
    }
}
