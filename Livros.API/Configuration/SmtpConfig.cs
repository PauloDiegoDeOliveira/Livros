using Livros.Application.Dtos.Smtp;

namespace Livros.API.Configuration
{
    public static class SmtpConfig
    {
        public static void AddSMTPConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
        }
    }
}