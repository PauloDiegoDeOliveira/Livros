using Livros.API.Controllers;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Estante;
using Livros.Application.Dtos.Lista;
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
    [Route("/v{version:apiVersion}/estantes")]
    [ApiController]
    public class EstanteController : MainController
    {
        private readonly IEstanteApplication estanteApplication;
        private readonly ILogger<EstanteController> logger;

        public EstanteController(IEstanteApplication estanteApplication,
                                 INotifier notifier,
                                 ILogger<EstanteController> logger,
                                 IUser user) : base(notifier, user)
        {
            this.estanteApplication = estanteApplication;
            this.logger = logger;
        }

        /// <summary>
        /// Retorna todas as estantes, com opções de filtro e paginação de dados.
        /// </summary>
        /// <param name="parametersEstante"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Estante, ViewEstanteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersEstante parametersEstante)
        {
            logger.LogWarning("Foi requisitado as estantes.");

            ViewPagedListDto<Estante, ViewEstanteDto> estantes = await estanteApplication.GetPaginationAsync(parametersEstante);
            if (estantes is null || !estantes.Pagina.Any())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Estantes encontradas.");
            }

            return CustomResponse(estantes);
        }

        /// <summary>
        /// Insere uma estante.
        /// </summary>
        /// <param name="postEstanteDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewEstanteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] PostEstanteDto postEstanteDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@postEstanteDto}", postEstanteDto);

            ViewEstanteDto inserido;
            using (Operation.Time("Tempo de adição de uma estante."))
            {
                logger.LogWarning("Foi requisitado a inserção de uma estante.");
                inserido = await estanteApplication.PostAsync(postEstanteDto);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Estante criada com sucesso!");
            }

            return CustomResponse(inserido);
        }

        /// <summary>
        /// Altera uma estante.
        /// </summary>
        /// <param name="putEstanteDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewEstanteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] PutEstanteDto putEstanteDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@putEstanteDto}", putEstanteDto);

            ViewEstanteDto atualizado;
            using (Operation.Time("Tempo de atualização de uma estante."))
            {
                logger.LogWarning("Foi requisitado a atualização de uma estante.");
                atualizado = await estanteApplication.PutAsync(putEstanteDto);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Estante atualizada com sucesso!");
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Exclui uma estante.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewEstanteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewEstanteDto removido = await estanteApplication.PutStatusAsync(id, EStatus.Excluido);
            if (removido is null)
            {
                NotifyError("Nenhuma estante foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto removido {@removido} ", removido);

            if (IsValidOperation())
            {
                NotifyWarning("Estante excluída com sucesso!");
            }

            return CustomResponse(removido);
        }

        /// <summary>
        /// Altera o status de uma estante.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewEstanteDto), StatusCodes.Status200OK)]
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

            ViewEstanteDto atualizado;
            using (Operation.Time("Tempo de atualização do status de uma estante."))
            {
                logger.LogWarning("Foi requisitado a atualização do status de uma estante.");
                atualizado = await estanteApplication.PutStatusAsync(putStatusDto.Id, putStatusDto.Status);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            switch (atualizado.Status)
            {
                case EStatus.Ativo:
                    NotifyWarning("Estante atualizada para ativo com sucesso!");
                    break;

                case EStatus.Inativo:
                    NotifyWarning("Estante atualizada para inativo com sucesso!");
                    break;

                case EStatus.Excluido:
                    NotifyWarning("Estante atualizada para excluído com sucesso!");
                    break;

                default:
                    NotifyWarning("Status atualizado com sucesso.");
                    break;
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Retorna detalhes de uma estante.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>Id: B9D01043-B103-42A1-AE3C-08DAD14324A9</remarks>
        [HttpGet("detalhes/{id:guid}")]
        [ProducesResponseType(typeof(ViewEstanteDetalhesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdDetailsAsync(Guid id)
        {
            ViewEstanteDetalhesDto viewEstanteDetalhesDto = await estanteApplication.GetByIdDetalhesAsync(id);

            if (viewEstanteDetalhesDto is null)
            {
                return CustomResponse(ModelState);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Foi requisitado detalhes de uma estante. {@viewEstanteDetalhesDto}", viewEstanteDetalhesDto);

            if (IsValidOperation())
            {
                NotifyWarning("Detalhes de uma estante.");
            }

            return CustomResponse(viewEstanteDetalhesDto);
        }
    }
}