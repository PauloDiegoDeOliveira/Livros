using Livros.Domain.Core.Interfaces.Services;

namespace Livros.Domain.Core.Notificacoes
{
    public class Notefier : INotifier
    {
        private readonly List<Notification> notifications;

        public Notefier()
        {
            notifications = new List<Notification>();
        }

        public void AddNotification(Notification notificacao)
        {
            notifications.Add(notificacao);
        }

        public void AddListNotification(List<Notification> notification)
        {
            notifications.AddRange(notification);
        }

        public List<Notification> GetAllNotifications()
        {
            return notifications;
        }

        public bool HasNotification()
        {
            return notifications.Any();
        }

        public bool HasAnyError()
        {
            return notifications.Where(x => x.NotificationType == ENotificationType.Error).Any();
        }
    }
}