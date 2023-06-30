using Livro.Identity.Constants;
using Livros.API.Controllers;
using Livros.Application.Dtos.Usuario;
using Livros.Application.Dtos.Usuario.Autenticacao;
using Livros.Application.Interfaces;
using Livros.Domain.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Livros.API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/autenticacao")]
    [ApiController]
    public class AutenticacaoController : MainController
    {
        private readonly IUsuarioApplication usuarioApplication;
        private readonly ILogger<AutenticacaoController> logger;

        public AutenticacaoController(IUsuarioApplication usuarioApplication,
                                      ILogger<AutenticacaoController> logger,
                                      INotifier notifier,
                                      IUser user) : base(notifier, user)
        {
            this.usuarioApplication = usuarioApplication;
            this.logger = logger;
        }

        /// <summary>
        /// Autentica um usuário.
        /// </summary>
        /// <param name="usuarioLogin"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ViewLoginDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] PostLoginUsuarioDto usuarioLogin)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            ViewLoginDto resultado = await usuarioApplication.Login(usuarioLogin);
            if (resultado is null)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Login realizado com sucesso!");
            }

            return CustomResponse(resultado);
        }

        /// <summary>
        /// Cadastra um usuário.
        /// </summary>
        /// <param name="usuarioCadastro"></param>
        /// <returns></returns>
        [HttpPost("cadastro-usuario")]
        [Authorize(Policy = Policies.HorarioComercial)]
        //[ClaimsAuthorizeAttribute(ClaimTypes.Autorizacao, PermissionTypes.Inserir)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Cadastrar([FromBody] PostCadastroUsuarioDto usuarioCadastro)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            bool inserido = await usuarioApplication.CadastrarUsuario(usuarioCadastro);

            if (!inserido)
            {
                return CustomResponse(ModelState);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            NotifyWarning("Usuário cadastrado com sucesso!");

            return CustomResponse(inserido);
        }
    }
}