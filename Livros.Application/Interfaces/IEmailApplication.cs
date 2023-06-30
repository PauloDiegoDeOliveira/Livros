using Livros.Application.Dtos.Smtp;

namespace Livros.Application.Interfaces
{
    public interface IEmailApplication
    {
        public Task SendEmail(UserEmailOptions userEmailOptions, string emailTemplate);
    }
}