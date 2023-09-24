using Livros.API.Controllers;
using Livros.Application.Dtos.Autor;
using Livros.Application.Dtos.Base;
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
    [Route("/v{version:apiVersion}/autores")]
    [ApiController]
    public class AutorController : MainController
    {
        private readonly IAutorApplication autorApplication;
        private readonly ILogger<AutorController> logger;

        public AutorController(IAutorApplication autorApplication,
                                INotifier notifier,
                                ILogger<AutorController> logger,
                                IUser user) : base(notifier, user)
        {
            this.autorApplication = autorApplication;
            this.logger = logger;
        }

        /// <summary>
        /// Retorna todos os autores, com opções de filtro e paginação de dados.
        /// </summary>
        /// <param name="parametersAutor"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Autor, ViewAutorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersAutor parametersAutor)
        {
            string logMessage = parametersAutor?.PalavraChave != null
                 ? $"Em autores foi buscado a palavra chave: {parametersAutor.PalavraChave}"
                 : "Foi requisitado os autores.";

            logger.LogWarning(logMessage);

            ViewPagedListDto<Autor, ViewAutorDto> autores = await autorApplication.GetPaginationAsync(parametersAutor);

            if (autores is null || !autores.Pagina.Any())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Autores encontrados.");
            }

            return CustomResponse(autores);
        }

        /// <summary>
        /// Insere um autor.
        /// </summary>
        /// <param name="postAutorDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewAutorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] PostAutorDto postAutorDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@postAutorDto}", postAutorDto);

            ViewAutorDto inserido;
            using (Operation.Time("Tempo de adição de um autor."))
            {
                logger.LogWarning("Foi requisitado a inserção de um autor.");
                inserido = await autorApplication.PostAsync(postAutorDto);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Autor criado com sucesso!");
            }

            return CustomResponse(inserido);
        }

        /// <summary>
        /// Altera um autor.
        /// </summary>
        /// <param name="putAutorDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewAutorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] PutAutorDto putAutorDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@putAutorDto}", putAutorDto);

            ViewAutorDto atualizado;
            using (Operation.Time("Tempo de atualização de um autor."))
            {
                logger.LogWarning("Foi requisitado a atualização de um autor.");
                atualizado = await autorApplication.PutAsync(putAutorDto);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Autor atualizado com sucesso!");
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Exclui um autor.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Ao excluir o mesmo será alterado para status 3 excluído.</remarks>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ViewAutorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            ViewAutorDto removido = await autorApplication.PutStatusAsync(id, EStatus.Excluido);
            if (removido is null)
            {
                NotifyError("Nenhum autor foi encontrado com o id informado.");
                return CustomResponse(ModelState);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto removido {@removido} ", removido);

            if (IsValidOperation())
            {
                NotifyWarning("Autor excluído com sucesso!");
            }

            return CustomResponse(removido);
        }

        /// <summary>
        /// Altera o status de um autor.
        /// </summary>
        /// <param name="putStatusDto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(typeof(ViewAutorDto), StatusCodes.Status200OK)]
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

            ViewAutorDto atualizado;
            using (Operation.Time("Tempo de atualização do status de um autor."))
            {
                logger.LogWarning("Foi requisitado a atualização do status de um autor.");
                atualizado = await autorApplication.PutStatusAsync(putStatusDto.Id, putStatusDto.Status);
            }

            if (atualizado is null)
            {
                return CustomResponse(ModelState);
            }

            switch (atualizado.Status)
            {
                case EStatus.Ativo:
                    NotifyWarning("Autor atualizado para ativo com sucesso!");
                    break;

                case EStatus.Inativo:
                    NotifyWarning("Autor atualizado para inativo com sucesso!");
                    break;

                case EStatus.Excluido:
                    NotifyWarning("Autor atualizado para excluído com sucesso!");
                    break;

                default:
                    NotifyWarning("Status atualizado com sucesso.");
                    break;
            }

            return CustomResponse(atualizado);
        }
    }
}