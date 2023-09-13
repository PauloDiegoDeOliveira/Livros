using Livros.API.Controllers;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Editora;
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
    //[Authorize]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/editoras")]
    [ApiController]
    public class EditoraController : MainController
    {
        private readonly IEditoraApplication editoraApplication;
        private readonly ILogger<EditoraController> logger;

        public EditoraController(IEditoraApplication editoraApplication,
                                 INotifier notifier,
                                 ILogger<EditoraController> logger,
                                 IUser user) : base(notifier, user)
        {
            this.editoraApplication = editoraApplication;
            this.logger = logger;
        }

        /// <summary>
        /// Retorna todas as editoras com filtro e paginação de dados.
        /// </summary>
        /// <param name="parametersEditora"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Editora, ViewEditoraDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersEditora parametersEditora)
        {
            logger.LogWarning("Foi requisitado as obras.");

            ViewPagedListDto<Editora, ViewEditoraDto> editoras = await editoraApplication.GetPaginationAsync(parametersEditora);
            if (editoras is null || !editoras.Pagina.Any())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Editoras encontradas.");
            }

            return CustomResponse(editoras);
        }

        /// <summary>
        /// Insere uma editora.
        /// </summary>
        /// <param name="postEditoraDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewEditoraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] PostEditoraDto postEditoraDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@postEditoraDto}", postEditoraDto);

            ViewEditoraDto inserido;
            using (Operation.Time("Tempo de adição de uma editora."))
            {
                logger.LogWarning("Foi requisitado a inserção de uma editora.");
                inserido = await editoraApplication.PostAsync(postEditoraDto);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Editora criada com sucesso!");
            }

            return CustomResponse(inserido);
        }

        /// <summary>
        /// Altera uma editora.
        /// </summary>
        /// <param name="putEditoraDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewEditoraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] PutEditoraDto putEditoraDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@putEditoraDto}", putEditoraDto);

            ViewEditoraDto atualizado;
            using (Operation.Time("Tempo de atualização de uma editora."))
            {
                logger.LogWarning("Foi requisitado a atualização de uma editora.");
                atualizado = await editoraApplication.PutAsync(putEditoraDto);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Editora atualizada com sucesso!");
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Exclui uma editora.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewEditoraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewEditoraDto removido = await editoraApplication.PutStatusAsync(id, EStatus.Excluido);
            if (removido is null)
            {
                NotifyError("Nenhuma editora foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto removido {@removido} ", removido);

            if (IsValidOperation())
            {
                NotifyWarning("Editora excluída com sucesso!");
            }

            return CustomResponse(removido);
        }

        /// <summary>
        /// Altera o status de uma editora.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewEditoraDto), StatusCodes.Status200OK)]
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

            ViewEditoraDto atualizado;
            using (Operation.Time("Tempo de atualização do status de uma obra."))
            {
                logger.LogWarning("Foi requisitado a atualização do status de uma obra.");
                atualizado = await editoraApplication.PutStatusAsync(putStatusDto.Id, putStatusDto.Status);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            switch (atualizado.Status)
            {
                case EStatus.Ativo:
                    NotifyWarning("Editora atualizada para ativo com sucesso!");
                    break;

                case EStatus.Inativo:
                    NotifyWarning("Editora atualizada para inativo com sucesso!");
                    break;

                case EStatus.Excluido:
                    NotifyWarning("Editora atualizada para excluído com sucesso!");
                    break;

                default:
                    NotifyWarning("Status atualizado com sucesso.");
                    break;
            }

            return CustomResponse(atualizado);
        }
    }
}