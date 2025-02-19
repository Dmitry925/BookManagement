using AutoMapper;
using BookManagement.Core.Application.DTOs.UserDTOs;
using BookManagement.Core.Application.Features.UserFeatures.Commands;
using BookManagement.Core.Domain.Models;

namespace BookManagement.Core.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDetailsDto>();

            CreateMap<SignUpUserCommand.Request, User>();
        }
    }
}
