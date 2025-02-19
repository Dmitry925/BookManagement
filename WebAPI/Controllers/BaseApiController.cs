using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace BookManagement.Presentation.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        private IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected Task<object?> SendAsync(object request)
        {
            return Mediator.Send(request);
        }
    }
}
