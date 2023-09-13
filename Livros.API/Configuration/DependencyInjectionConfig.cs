using Livros.API.Extensions;
using Livros.Application.Applications;
using Livros.Application.Interfaces;
using Livros.Domain.Core.Interfaces.Repositories;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Core.Notificacoes;
using Livros.Domain.Service;
using Livros.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Livros.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioApplication, UsuarioApplication>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped<IObraApplication, ObraApplication>();
            services.AddScoped<IObraRepository, ObraRepository>();
            services.AddScoped<IObraService, ObraService>();

            services.AddScoped<IEditoraApplication, EditoraApplication>();
            services.AddScoped<IEditoraRepository, EditoraRepository>();
            services.AddScoped<IEditoraService, EditoraService>();

            services.AddScoped<IObraApplication, ObraApplication>();
            services.AddScoped<IObraRepository, ObraRepository>();
            services.AddScoped<IObraService, ObraService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUser, AspNetUser>();
            services.AddScoped<INotifier, Notefier>();

            services.AddScoped<IEmailApplication, EmailApplication>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}