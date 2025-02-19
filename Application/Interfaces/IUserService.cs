using BookManagement.Core.Application.Authentication;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Interfaces
{
    public interface IUserService
    {
        Task<AuthorizationModel> GetTokenAsync(AuthenticationModel model);
    }
}
