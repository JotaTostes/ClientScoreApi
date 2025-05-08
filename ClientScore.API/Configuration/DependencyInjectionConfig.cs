using ClientScore.Application.Interfaces;
using ClientScore.Application.Services;
using ClientScore.Infrastructure.Context;
using ClientScore.Infrastructure.Interfaces;
using Infrastructure.Repositories;
using FluentValidation;
using ClientScore.Application.Validator;
using ClientScore.Application.DTOs;

namespace ClientScore.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            //DbContext
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

            //Services
            services.AddScoped<IScoreService, ScoreService>();
            services.AddScoped<IClienteService, ClienteService>();

            //Repositories
            services.AddScoped<IClienteRepository, ClienteRepository>();

            //Validators
            services.AddScoped<IValidator<ClienteRequestDto>, ClienteValidator>();
            return services;
        }
    }
}
