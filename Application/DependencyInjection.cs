using AutoMapper.EquivalencyExpression;
using BookManagement.Core.Application.Infrastructure.Validation;
using BookManagement.Core.Application.Profiles;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BookManagement.Core.Application
{
    public static class DependencyIjection
    {
        public static void AddAplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(cfg => cfg.AddCollectionMappers(), typeof(BookProfile));

            AssemblyScanner.FindValidatorsInAssembly(typeof(RequestValidatorBehavior<,>).Assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidatorBehavior<,>));
        }
    }
}
