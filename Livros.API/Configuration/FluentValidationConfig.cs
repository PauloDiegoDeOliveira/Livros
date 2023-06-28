using FluentValidation;
using FluentValidation.AspNetCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Livros.API.Configuration
{
    public static class FluentValidationConfig
    {
        public static void AddFluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            //services.AddValidatorsFromAssemblyContaining<PostLivroValidator>();
            //services.AddValidatorsFromAssemblyContaining<PostUsuarioValidator>();

            services.AddFluentValidationAutoValidation();

            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pt-BR");
        }
    }
}