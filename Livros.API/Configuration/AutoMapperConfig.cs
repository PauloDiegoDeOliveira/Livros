using Livros.Application.Mappings;

namespace Livros.API.Configuration
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UsuarioMappingProfile));
        }
    }
}