using BookManagement.Core.Application.Features.BookFeatures.Commands;
using BookManagement.Core.Application.Features.BookFeatures.Queries;
using BookManagement.Core.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookManagement.Presentation.WebAPI.Controllers
{
    public class BookController : BaseApiController
    {
        [HttpPost, Authorize()]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand.Request request)
        {
            return Ok(await SendAsync(request));
        }

        [HttpPost("bulk"), Authorize()]
        public async Task<IActionResult> CreateBooksBulk([FromBody] CreateBooksBulkCommand.Request request)
        {
            return Ok(await SendAsync(request));
        }

        [HttpGet, Authorize()]
        public async Task<IActionResult> GetAllBooks([FromQuery] GetAllBooksPagedQuery.Request request)
        {
            var response = await SendAsync(request);
            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }

        [HttpGet("{Id:Guid}"), Authorize()]
        public async Task<IActionResult> GetBookById([FromRoute] GetBookByIdQuery.Request request)
        {
            return Ok(await SendAsync(request));
            
        }

        [HttpGet("{Title}"), Authorize()]
        public async Task<IActionResult> GetBookByTitle([FromRoute] GetBookByTitleQuery.Request request)
        {
            return Ok(await SendAsync(request));
            
        }

        [HttpDelete("{Id:Guid}"), Authorize(Roles = nameof(BaseRole.Admin))]
        public async Task<IActionResult> DeleteBook([FromRoute] SoftDeleteBookCommand.Request request)
        {
            await SendAsync(request);
            return NoContent();
        }

        [HttpDelete("bulk"), Authorize(Roles = nameof(BaseRole.Admin))]
        public async Task<IActionResult> DeleteBooksBulk([FromBody] SoftDeleteBooksBulkCommand.Request request)
        {
            await SendAsync(request);
            return NoContent();
        }

        [HttpPut("{Id:Guid}"), Authorize(Roles = nameof(BaseRole.Admin))]
        public async Task<IActionResult> UpdateSource([FromRoute] Guid Id, [FromBody] UpdateBookCommand.Request request)
        {
            request.Id = Id;

            return Ok(await SendAsync(request));
        }
    }
}
