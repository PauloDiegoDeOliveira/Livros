using Livros.API.Controllers;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Obra;
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
    [Route("/v{version:apiVersion}/obras")]
    [ApiController]
    public class ObraController : MainController
    {
        private readonly IObraApplication obraApplication;
        private readonly ILogger<ObraController> logger;

        public ObraController(IObraApplication obraApplication,
                              INotifier notifier,
                              ILogger<ObraController> logger,
                              IUser user) : base(notifier, user)
        {
            this.obraApplication = obraApplication;
            this.logger = logger;
        }

        /// <summary>
        /// Retorna todas as obras com filtro e paginação de dados.
        /// </summary>
        /// <param name="parametersObra"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Obra, ViewObraDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersObra parametersObra)
        {
            logger.LogWarning("Foi requisitado as obras.");

            ViewPagedListDto<Obra, ViewObraDto> obras = await obraApplication.GetPaginationAsync(parametersObra);

            if (obras is null || !obras.Pagina.Any())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Obras encontradas.");
            }

            return CustomResponse(obras);
        }

        /// <summary>
        /// Insere uma obra.
        /// </summary>
        /// <param name="postObraDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewObraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] PostObraDto postObraDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            logger.LogWarning("Objeto recebido {@postObraDto}", postObraDto);

            ViewObraDto inserido;
            using (Operation.Time("Tempo de adição de uma obra."))
            {
                logger.LogWarning("Foi requisitado a inserção de uma obra.");
                inserido = await obraApplication.PostAsync(postObraDto);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Obra criada com sucesso!");
            }

            return CustomResponse(inserido);
        }

        /// <summary>
        /// Altera uma obra.
        /// </summary>
        /// <param name="putObraDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewObraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] PutObraDto putObraDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@putObraDto}", putObraDto);

            ViewObraDto atualizado;
            using (Operation.Time("Tempo de atualização de uma obra."))
            {
                logger.LogWarning("Foi requisitado a atualização de uma obra.");
                atualizado = await obraApplication.PutAsync(putObraDto);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Obra atualizada com sucesso!");
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Exclui uma obra.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewObraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewObraDto removido = await obraApplication.PutStatusAsync(id, EStatus.Excluido);
            if (removido is null)
            {
                NotifyError("Nenhuma obra foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto removido {@removido} ", removido);

            if (IsValidOperation())
            {
                NotifyWarning("Obra excluída com sucesso!");
            }

            return CustomResponse(removido);
        }

        /// <summary>
        /// Altera o status de uma obra.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewObraDto), StatusCodes.Status200OK)]
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

            ViewObraDto atualizado;
            using (Operation.Time("Tempo de atualização do status de uma obra."))
            {
                logger.LogWarning("Foi requisitado a atualização do status de uma obra.");
                atualizado = await obraApplication.PutStatusAsync(putStatusDto.Id, putStatusDto.Status);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            switch (atualizado.Status)
            {
                case EStatus.Ativo:
                    NotifyWarning("Participante atualizado para ativo com sucesso!");
                    break;

                case EStatus.Inativo:
                    NotifyWarning("Participante atualizado para inativo com sucesso!");
                    break;

                case EStatus.Excluido:
                    NotifyWarning("Participante atualizada para excluído com sucesso!");
                    break;

                default:
                    NotifyWarning("Status atualizado com sucesso.");
                    break;
            }

            return CustomResponse(atualizado);
        }
    }
}