using Livros.Domain.Core.Notificacoes;

namespace Livros.Domain.Core.Interfaces.Services
{
    public interface INotifier
    {
        List<Notification> GetAllNotifications();

        void AddNotification(Notification notificacao);

        bool HasNotification();

        bool HasAnyError();
    }
}