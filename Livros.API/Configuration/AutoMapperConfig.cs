using Livros.Application.Mappings;

namespace Livros.API.Configuration
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UsuarioMappingProfile),
                                   typeof(ObraMappingProfile),
                                   typeof(EditoraMappingProfile),
                                   typeof(GeneroMappingProfile),
                                   typeof(AutorMappingProfile),
                                   typeof(IdiomaMappingProfile));
        }
    }
}