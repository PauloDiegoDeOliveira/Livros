using Livros.API.Controllers;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Idioma;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Enums;
using Livros.Domain.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerilogTimings;

namespace Livros.API.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/idiomas")]
    [ApiController]
    public class IdiomaController : MainController
    {
        private readonly IIdiomaApplication idiomaApplication;
        private readonly ILogger<IdiomaController> logger;

        public IdiomaController(IIdiomaApplication idiomaApplication,
                                INotifier notifier,
                                ILogger<IdiomaController> logger,
                                IUser user) : base(notifier, user)
        {
            this.idiomaApplication = idiomaApplication;
            this.logger = logger;
        }

        /// <summary>
        /// Retorna todos os idiomas, com opções de filtro e paginação de dados.
        /// </summary>
        /// <param name="parametersIdioma"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Idioma, ViewIdiomaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersIdioma parametersIdioma)
        {
            logger.LogWarning("Foi requisitado os idiomas.");

            ViewPagedListDto<Idioma, ViewIdiomaDto> idiomas = await idiomaApplication.GetPaginationAsync(parametersIdioma);

            if (idiomas is null || !idiomas.Pagina.Any())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Idiomas encontrados.");
            }

            return CustomResponse(idiomas);
        }

        /// <summary>
        /// Insere um idioma.
        /// </summary>
        /// <param name="postIdiomaDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewIdiomaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] PostIdiomaDto postIdiomaDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@postIdiomaDto}", postIdiomaDto);

            ViewIdiomaDto inserido;
            using (Operation.Time("Tempo de adição de um idioma."))
            {
                logger.LogWarning("Foi requisitado a inserção de um idioma.");
                inserido = await idiomaApplication.PostAsync(postIdiomaDto);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Idioma criado com sucesso!");
            }

            return CustomResponse(inserido);
        }

        /// <summary>
        /// Altera um idioma.
        /// </summary>
        /// <param name="putIdiomaDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewIdiomaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] PutIdiomaDto putIdiomaDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@putIdiomaDto}", putIdiomaDto);

            ViewIdiomaDto atualizado;
            using (Operation.Time("Tempo de atualização de um idioma."))
            {
                logger.LogWarning("Foi requisitado a atualização de um idioma.");
                atualizado = await idiomaApplication.PutAsync(putIdiomaDto);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Idioma atualizado com sucesso!");
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Exclui um idioma.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewIdiomaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewIdiomaDto removido = await idiomaApplication.PutStatusAsync(id, EStatus.Excluido);
            if (removido is null)
            {
                NotifyError("Nenhum idioma foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto removido {@removido} ", removido);

            if (IsValidOperation())
            {
                NotifyWarning("Idioma excluído com sucesso!");
            }

            return CustomResponse(removido);
        }

        /// <summary>
        /// Altera o status de um Idioma.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewIdiomaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutStatusAsync([FromBody] PutStatusDto putStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            if (putStatusDto.Status == 0)
            {
                NotifyError("Nenhum status selecionado.");
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@putStatusDto}", putStatusDto);

            ViewIdiomaDto atualizado;
            using (Operation.Time("Tempo de atualização do status de um idioma."))
            {
                logger.LogWarning("Foi requisitado a atualização do status de um idioma.");
                atualizado = await idiomaApplication.PutStatusAsync(putStatusDto.Id, putStatusDto.Status);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            switch (atualizado.Status)
            {
                case EStatus.Ativo:
                    NotifyWarning("Idioma atualizado para ativo com sucesso!");
                    break;

                case EStatus.Inativo:
                    NotifyWarning("Idioma atualizado para inativo com sucesso!");
                    break;

                case EStatus.Excluido:
                    NotifyWarning("Idioma atualizado para excluído com sucesso!");
                    break;

                default:
                    NotifyWarning("Status atualizado com sucesso.");
                    break;
            }

            return CustomResponse(atualizado);
        }
    }
}