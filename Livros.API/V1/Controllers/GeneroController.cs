using Livros.API.Controllers;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Genero;
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
    [Route("/v{version:apiVersion}/generos")]
    [ApiController]
    public class GeneroController : MainController
    {
        private readonly IGeneroApplication generoApplication;
        private readonly ILogger<GeneroController> logger;

        public GeneroController(IGeneroApplication generoApplication,
                                INotifier notifier,
                                ILogger<GeneroController> logger,
                                IUser user) : base(notifier, user)
        {
            this.generoApplication = generoApplication;
            this.logger = logger;
        }

        /// <summary>
        /// Retorna todos os gêneros, com opções de filtro e paginação de dados.
        /// </summary>
        /// <param name="parametersGenero"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Genero, ViewGeneroDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersGenero parametersGenero)
        {
            string logMessage = parametersGenero?.PalavraChave != null
                 ? $"Em gêneros foi buscado a palavra chave: {parametersGenero.PalavraChave}"
                 : "Foi requisitado os gêneros.";

            logger.LogWarning(logMessage);

            ViewPagedListDto<Genero, ViewGeneroDto> generos = await generoApplication.GetPaginationAsync(parametersGenero);
            if (generos is null || !generos.Pagina.Any())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Gêneros encontrados.");
            }

            return CustomResponse(generos);
        }

        /// <summary>
        /// Insere um gênero.
        /// </summary>
        /// <param name="postGeneroDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewGeneroDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] PostGeneroDto postGeneroDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@postGeneroDto}", postGeneroDto);

            ViewGeneroDto inserido;
            using (Operation.Time("Tempo de adição de um gênero."))
            {
                logger.LogWarning("Foi requisitado a inserção de um gênero.");
                inserido = await generoApplication.PostAsync(postGeneroDto);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Gênero criado com sucesso!");
            }

            return CustomResponse(inserido);
        }

        /// <summary>
        /// Altera um gênero.
        /// </summary>
        /// <param name="putGeneroDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewGeneroDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] PutGeneroDto putGeneroDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@putGeneroDto}", putGeneroDto);

            ViewGeneroDto atualizado;
            using (Operation.Time("Tempo de atualização de um gênero."))
            {
                logger.LogWarning("Foi requisitado a atualização de um gênero.");
                atualizado = await generoApplication.PutAsync(putGeneroDto);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Gênero atualizado com sucesso!");
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Exclui um gênero.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewGeneroDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewGeneroDto removido = await generoApplication.PutStatusAsync(id, EStatus.Excluido);
            if (removido is null)
            {
                NotifyError("Nenhum gênero foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto removido {@removido} ", removido);

            if (IsValidOperation())
            {
                NotifyWarning("Gênero excluído com sucesso!");
            }

            return CustomResponse(removido);
        }

        /// <summary>
        /// Altera o status de um gênero.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewGeneroDto), StatusCodes.Status200OK)]
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

            ViewGeneroDto atualizado;
            using (Operation.Time("Tempo de atualização do status de um gênero."))
            {
                logger.LogWarning("Foi requisitado a atualização do status de um gênero.");
                atualizado = await generoApplication.PutStatusAsync(putStatusDto.Id, putStatusDto.Status);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            switch (atualizado.Status)
            {
                case EStatus.Ativo:
                    NotifyWarning("Gênero atualizado para ativo com sucesso!");
                    break;

                case EStatus.Inativo:
                    NotifyWarning("Gênero atualizado para inativo com sucesso!");
                    break;

                case EStatus.Excluido:
                    NotifyWarning("Gênero atualizado para excluído com sucesso!");
                    break;

                default:
                    NotifyWarning("Status atualizado com sucesso.");
                    break;
            }

            return CustomResponse(atualizado);
        }
    }
}