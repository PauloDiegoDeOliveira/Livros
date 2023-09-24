using FluentValidation;
using FluentValidation.AspNetCore;
using Livros.Application.Dtos.Editora;
using Livros.Application.Dtos.Estante;
using Livros.Application.Dtos.Genero;
using Livros.Application.Dtos.Idioma;
using Livros.Application.Dtos.Obra;
using Livros.Application.Dtos.Volume;
using Livros.Application.Validators.Autor;
using Livros.Application.Validators.Estante;
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
            services.AddValidatorsFromAssemblyContaining<PostAutorValidator>();
            services.AddValidatorsFromAssemblyContaining<PutAutorValidator>();

            services.AddValidatorsFromAssemblyContaining<PostEditoraDto>();
            services.AddValidatorsFromAssemblyContaining<PutEditoraDto>();

            services.AddValidatorsFromAssemblyContaining<PostGeneroDto>();
            services.AddValidatorsFromAssemblyContaining<PutGeneroDto>();

            services.AddValidatorsFromAssemblyContaining<PostIdiomaDto>();
            services.AddValidatorsFromAssemblyContaining<PutIdiomaDto>();

            services.AddValidatorsFromAssemblyContaining<PostObraDto>();
            services.AddValidatorsFromAssemblyContaining<PutObraDto>();

            services.AddValidatorsFromAssemblyContaining<PostVolumeDto>();
            services.AddValidatorsFromAssemblyContaining<PutVolumeDto>();

            services.AddValidatorsFromAssemblyContaining<PostEstanteDto>();
            services.AddValidatorsFromAssemblyContaining<PutEstanteDto>();

            services.AddFluentValidationAutoValidation();
        }
    }
}