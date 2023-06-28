namespace Livros.Domain.Core.Notificacoes
{
    public class Notification
    {
        public ENotificationType NotificationType { get; }

        public string Message { get; }

        public Notification(string message)
        {
            Message = message;
            NotificationType = ENotificationType.Error;
        }

        public Notification(string message, ENotificationType notificationType)
        {
            Message = message;
            NotificationType = notificationType;
        }
    }
}