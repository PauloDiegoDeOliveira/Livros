using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Livros.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            //services.AddScoped<ILivroApplication, LivroApplication>();
            //services.AddScoped<ILivroRepository, LivroRepository>();
            //services.AddScoped<ILivroService, LivroService>();

            //services.AddScoped<IEmailApplication, EmailApplication>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddScoped<IUser, AspNetUser>();
            //services.AddScoped<INotifier, Notefier>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}