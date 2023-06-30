using Livros.API.Extensions;
using Livros.Application.Applications;
using Livros.Application.Interfaces;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Core.Notificacoes;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Livros.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            //services.AddScoped<ILivroApplication, LivroApplication>();
            //services.AddScoped<ILivroRepository, LivroRepository>();
            //services.AddScoped<ILivroService, LivroService>();

            services.AddScoped<IEmailApplication, EmailApplication>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUser, AspNetUser>();
            services.AddScoped<INotifier, Notefier>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}