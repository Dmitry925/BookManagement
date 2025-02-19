using BookManagement.Core.Application.Authentication;
using BookManagement.Core.Application.Infrastructure.Exceptions;
using BookManagement.Core.Application.Interfaces;
using BookManagement.Core.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Infrastructure.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly JWT _jwt;

        public UserService(UserManager<User> userManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        public async Task<AuthorizationModel> GetTokenAsync(AuthenticationModel model)
        {
            var authenticationModel = new AuthorizationModel();
            var user = await _userManager.FindByNameAsync(model.UserName);
            
            if(user == null)
            {
                throw new NotFoundException(model.UserName);
            }
            
            if(await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authenticationModel.UserId = user.Id;
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Roles = roleList.ToList();

                return authenticationModel;
            }

            throw new ValidationException($"Incorrect credentials for user {user.UserName}.");
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            for(int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);


            return jwtSecurityToken;
        }
    }
}
