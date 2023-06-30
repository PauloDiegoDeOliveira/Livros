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
            ConfigureControllers(services);
            RegisterValidators(services);
            ConfigureGlobalValidationOptions();
        }

        private static void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(config =>
                {
                    config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    config.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddJsonOptions(conf =>
                {
                    conf.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        }

        private static void ConfigureGlobalValidationOptions()
        {
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pt-BR");
        }

        private static void RegisterValidators(IServiceCollection services)
        {
            //services.AddValidatorsFromAssemblyContaining<PostLivroValidator>();
            //services.AddValidatorsFromAssemblyContaining<PutLivroValidator>();

            services.AddFluentValidationAutoValidation();
        }
    }
}