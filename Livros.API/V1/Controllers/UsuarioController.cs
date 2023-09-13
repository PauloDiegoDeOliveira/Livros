using Livro.Identity.Constants;
using Livros.API.Controllers;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Usuario;
using Livros.Application.Dtos.Usuario.Autenticacao;
using Livros.Application.Interfaces;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Enums;
using Livros.Domain.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerilogTimings;

namespace Livros.API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/usuario")]
    [ApiController]
    public class UsuarioController : MainController
    {
        private readonly IUsuarioApplication usuarioApplication;
        private readonly ILogger<AutenticacaoController> logger;

        public UsuarioController(IUsuarioApplication usuarioApplication,
                                 ILogger<AutenticacaoController> logger,
                                 INotifier notifier,
                                 IUser user) : base(notifier, user)
        {
            this.usuarioApplication = usuarioApplication;
            this.logger = logger;
        }

        /// <summary>
        /// Retorna todos os usuários, com opções de filtro e paginação de dados.
        /// </summary>
        /// <param name="parametersUsuario"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("usuario")]
        [ProducesResponseType(typeof(ViewPaginacaoUsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersUsuario parametersUsuario)
        {
            logger.LogWarning("Foi requisitado os usuários.");

            ViewPaginacaoUsuarioDto usuarios = await usuarioApplication.GetPaginationAsync(parametersUsuario);
            if (usuarios is null || !usuarios.Pagina.Any())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Usuários encontrados.");
            }

            return CustomResponse(usuarios);
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

        /// <summary>
        /// Atualiza um usuário.
        /// </summary>
        /// <param name="putCadastroUsuarioDto"></param>
        /// <returns></returns>
        [HttpPut("atualiza-usuario")]
        [Authorize(Policy = Policies.HorarioComercial)]
        //[ClaimsAuthorizeAttribute(ClaimTypes.Autorizacao, PermissionTypes.Inserir)]
        [ProducesResponseType(typeof(ViewUsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Atualizar([FromBody] PutCadastroUsuarioDto putCadastroUsuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            ViewUsuarioDto inserido = await usuarioApplication.UpdateUsuario(putCadastroUsuarioDto);
            if (inserido is null)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Usuário atualizado com sucesso!");
            }

            return CustomResponse(inserido);
        }

        /// <summary>
        /// Exclui um usuário.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir o mesmo será removido permanentemente da base.</remarks>
        [Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewUsuarioDto removido = await usuarioApplication.DeleteAsync(id);
            if (removido is null)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto removido {@removido} ", removido);

            if (IsValidOperation())
            {
                NotifyWarning("Usuário excluído com sucesso!");
            }

            return CustomResponse(removido);
        }

        /// <summary>
        /// Altera o status.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewUsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutStatusAsync([FromForm] PutStatusDto putStatusDto)
        {
            if (putStatusDto.Status == 0)
            {
                NotifyError("Nenhum status selecionado!");
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@putStatusDto}", putStatusDto);

            ViewUsuarioDto atualizado;
            using (Operation.Time("Tempo de atualização do status de um upload."))
            {
                logger.LogWarning("Foi requisitada a atualização do status de um usuário.");
                atualizado = await usuarioApplication.PutStatusAsync(putStatusDto.Id, putStatusDto.Status);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                switch (atualizado.Status)
                {
                    case EStatus.Ativo:
                        NotifyWarning("Usuário atualizado para ativo com sucesso.");
                        break;

                    case EStatus.Inativo:
                        NotifyWarning("Usuário atualizado para inativo com sucesso.");
                        break;

                    case EStatus.Excluido:
                        NotifyWarning("Usuário atualizado para excluído com sucesso.");
                        break;

                    default:
                        NotifyWarning("Status atualizado com sucesso.");
                        break;
                }
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Altera a senha de um usuário autenticado.
        /// </summary>
        /// <param name="postAlterarSenhaUsuarioDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("alterar-senha")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AlterarSenha([FromBody] PostAlterarSenhaUsuarioDto postAlterarSenhaUsuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@postAlterarSenhaUsuarioDto}", postAlterarSenhaUsuarioDto);

            postAlterarSenhaUsuarioDto.UsuarioId = UsuarioId;

            bool atualizado;
            using (Operation.Time("Tempo de atualização de um usuário autenticado."))
            {
                logger.LogWarning("Foi requisitada a atualização de um usuário autenticado.");
                atualizado = await usuarioApplication.AlterarSenha(postAlterarSenhaUsuarioDto);
            }

            if (!atualizado)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Senha alterada com sucesso!");
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Envia um e-mail com o token para resetar a senha.
        /// </summary>
        /// <param name="postConfirmacaoEmailDto"></param>
        /// <returns></returns>
        [HttpPost("resetar-senha-email")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EnviarEmailResetarSenha([FromForm] PostEmailDto postConfirmacaoEmailDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            if (string.IsNullOrEmpty(postConfirmacaoEmailDto.Email))
            {
                NotifyError("O campo e-mail não pode ser vazio.");
                return CustomResponse(ModelState);
            }

            bool atualizado = await usuarioApplication.EnviarEmailResetarSenha(postConfirmacaoEmailDto);
            if (!atualizado)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Enviamos um <i>e-mail</i> para realização da alteração de senha.");
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Recebe os dados com o token para realizar o reset de senha.
        /// </summary>
        /// <param name="postResetarSenhaDto"></param>
        /// <returns></returns>
        [HttpPost("resetar-senha")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetarSenha([FromBody] PostResetarSenhaDto postResetarSenhaDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            if (string.IsNullOrEmpty(postResetarSenhaDto.Token) && postResetarSenhaDto.UsuarioId == Guid.Empty)
            {
                NotifyError("Os dados não podem ser vazios.");
                return CustomResponse(ModelState);
            }

            bool atualizado = await usuarioApplication.ResetarSenha(postResetarSenhaDto);
            if (!atualizado)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Alteração realizada com sucesso!");
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Confirma um e-mail de usuário cadastrado por meio do token.
        /// </summary>
        /// <param name="postConfirmacaoEmailDto"></param>
        /// <returns></returns>
        [HttpPost("confirmar-cadastro")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmarEmail([FromBody] PostConfirmacaoEmailDto postConfirmacaoEmailDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            if (string.IsNullOrEmpty(postConfirmacaoEmailDto.Token) && postConfirmacaoEmailDto.UsuarioId == Guid.Empty)
            {
                NotifyError("Os dados não podem ser vazios.");
                return CustomResponse(ModelState);
            }

            bool atualizado = await usuarioApplication.ConfimarEmail(postConfirmacaoEmailDto);
            if (!atualizado)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Verificação realizada com sucesso!");
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Renvia um e-mail de confirmação de conta.
        /// </summary>
        /// <param name="postConfirmacaoEmailDto"></param>
        /// <returns></returns>
        [HttpPost("reenviar-confirmacao-email")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReenviarConfirmacaoEmail([FromForm] PostEmailDto postConfirmacaoEmailDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            if (string.IsNullOrEmpty(postConfirmacaoEmailDto.Email))
            {
                NotifyError("O campo e-mail não pode ser vazio.");
                return CustomResponse(ModelState);
            }

            bool atualizado = await usuarioApplication.ReenviarConfirmacaoEmail(postConfirmacaoEmailDto);
            if (!atualizado)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Verificação reenviada com sucesso!");
            }

            return CustomResponse(atualizado);
        }
    }
}