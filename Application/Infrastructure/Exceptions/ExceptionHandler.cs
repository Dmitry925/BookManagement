using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Infrastructure.Exceptions
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _rDelegate;

        public ExceptionHandler(RequestDelegate rDelegate)
        {
            _rDelegate = rDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _rDelegate(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                var response = new { message = ex.Message };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";
                var response = new { message = ex.Message };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (AlreadyExistsException ex)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                context.Response.ContentType = "application/json";
                var response = new { message = ex.Message };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var response = new { message = "An error occurred while processing your request." };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
