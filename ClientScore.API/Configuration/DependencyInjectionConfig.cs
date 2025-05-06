using ClientScore.Application.Interfaces;
using ClientScore.Application.Services;
using ClientScore.Infrastructure.Context;
using ClientScore.Infrastructure.Interfaces;
using Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Cors.Infrastructure;
using ClientScore.Application.Validator;
using ClientScore.Application.DTOs;

namespace ClientScore.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped<IScoreService, ScoreService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            services.AddScoped<IValidator<ClienteRequestDto>, ClienteValidator>();
            return services;
        }
    }
}
