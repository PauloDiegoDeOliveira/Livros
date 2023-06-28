using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Core.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Livros.API.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotifier notifier;
        public readonly IUser user;
        protected Guid UsuarioId { get; set; }
        protected bool UsuarioAutenticado { get; set; }
        protected IEnumerable<string> UsuarioClaims { get; set; }

        protected MainController(INotifier notifier,
                                 IUser user)
        {
            this.notifier = notifier;
            this.user = user;

            if (user.IsAuthenticated())
            {
                UsuarioId = user.GetUserId();
                UsuarioAutenticado = true;
                UsuarioClaims = user.GetUserClaims();
            }
        }

        protected bool IsValidOperation()
        {
            return !notifier.HasAnyError();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(new
                {
                    mensagem = notifier.GetAllNotifications().Select(n => n.Message),
                    sucesso = true,
                    dados = result
                });
            }

            return BadRequest(new
            {
                sucesso = false,
                erros = notifier.GetAllNotifications().Select(n => n.Message)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                NotifyInvalidModels(modelState);

            return CustomResponse();
        }

        protected void NotifyInvalidModels(ModelStateDictionary modelState)
        {
            IEnumerable<ModelError> errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (ModelError error in errors)
            {
                string errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMsg);
            }
        }

        protected void NotifyError(string message)
        {
            notifier.AddNotification(new Notification(message));
        }

        protected void NotificarErro(List<string> messageList)
        {
            foreach (string erro in messageList)
            {
                NotifyError(erro);
            }
        }

        protected void NotifyWarning(string message)
        {
            notifier.AddNotification(new Notification(message, ENotificationType.Warning));
        }
    }
}