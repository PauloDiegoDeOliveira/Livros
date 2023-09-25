using Livros.API.Controllers;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Obra;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces;
using Livros.Application.Utilities.Image;
using Livros.Application.Utilities.Paths;
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
        private readonly EAmbiente eAmbiente;

        public ObraController(IObraApplication obraApplication,
                              INotifier notifier,
                              ILogger<ObraController> logger,
                              IUser user,
                              IWebHostEnvironment webHostEnvironment) : base(notifier, user)
        {
            this.obraApplication = obraApplication;
            this.logger = logger;
            this.eAmbiente = webHostEnvironment.IsProduction()
                                  ? EAmbiente.Producao
                                  : webHostEnvironment.IsEnvironment("Homologation")
                                      ? EAmbiente.Homologacao
                                      : EAmbiente.Desenvolvimento;
        }

        /// <summary>
        /// Retorna todas as obras, com opções de filtro e paginação de dados.
        /// </summary>
        /// <param name="parametersObra"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ViewPagedListDto<Obra, ViewObraDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] ParametersObra parametersObra)
        {
            string logMessage = parametersObra?.PalavraChave != null
                     ? $"Em obras foi buscado a palavra chave: {parametersObra.PalavraChave}"
                     : "Foi requisitado as obras.";

            logger.LogWarning(logMessage);

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
        /// Insere um obra.
        /// </summary>
        /// <param name="postObraDto"></param>
        /// <param name="eDiretorio"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ViewObraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] PostObraDto postObraDto, EDiretorio eDiretorio)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@postObraDto}", postObraDto);

            if (!await UrlsSaoValidos(eDiretorio))
            {
                NotifyWarning("Diretório não encontrado.");
                return CustomResponse(ModelState);
            }

            var (base64String, extensao) = ExtrairInformacoesImagem(postObraDto.ImagemBase64);
            if (base64String is null || extensao is null)
            {
                NotifyWarning("Extensão não suportada ou texto não se encontra em base64.");
                return CustomResponse(ModelState);
            }

            ViewObraDto inserido = await AdicionarObra(postObraDto, eDiretorio, base64String, extensao);

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            NotifyWarning("Obra criada com sucesso!");

            return CustomResponse(inserido);
        }

        private async Task<bool> UrlsSaoValidos(EDiretorio eDiretorio)
        {
            return await GerenciadorDeCaminhos.ValidarURLs(eDiretorio.ToString(), eAmbiente);
        }

        private (string, string) ExtrairInformacoesImagem(string imagemBase64)
        {
            string obterExtensaoDoBase64 = InterpretadorDataUriImagem.ObterExtensaoDoBase64(imagemBase64);
            string ExtrairStringBase64 = InterpretadorDataUriImagem.ExtrairStringBase64(imagemBase64);

            return (ExtrairStringBase64, obterExtensaoDoBase64);
        }

        private async Task<ViewObraDto> AdicionarObra(PostObraDto postObraDto, EDiretorio eDiretorio, string base64String, string extensao)
        {
            ViewObraDto inserido;
            Dictionary<string, string> Urls = await GerenciadorDeCaminhos.ObterURLs(eDiretorio.ToString(), eAmbiente);

            using (Operation.Time("Tempo de adição de uma obra."))
            {
                logger.LogWarning("Foi requisitado a inserção de uma obra.");
                inserido = await obraApplication.PostAsync(postObraDto, Urls["IP"], Urls["DNS"], Urls["SPLIT"], base64String, extensao);
            }

            return inserido;
        }

        /// <summary>
        /// Altera uma obra.
        /// </summary>
        /// <param name="putObraDto"></param>
        /// <param name="eDiretorio"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ViewObraDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] PutObraDto putObraDto, EDiretorio eDiretorio)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Objeto recebido {@putObraDto}", putObraDto);

            if (!string.IsNullOrWhiteSpace(putObraDto.ImagemBase64))
            {
                return await AtualizarComImagem(putObraDto, eDiretorio);
            }
            else if (putObraDto.ImagemBase64 == "")
            {
                return await AtualizarSemAlterarImagem(putObraDto);
            }
            else if (putObraDto.ImagemBase64 == null)
            {
                return await AtualizarRemovendoImagem(putObraDto);
            }

            return CustomResponse(ModelState);
        }

        private async Task<IActionResult> AtualizarRemovendoImagem(PutObraDto putObraDto)
        {
            ViewObraDto obraAtualizada = await obraApplication.PutAsync(putObraDto, null, null, null, null, null);

            if (obraAtualizada is null)
            {
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Obra atualizada e imagem excluída com sucesso!");
            }

            return CustomResponse(obraAtualizada);
        }

        private async Task<IActionResult> AtualizarComImagem(PutObraDto putObraDto, EDiretorio eDiretorio)
        {
            if (!await GerenciadorDeCaminhos.ValidarURLs(eDiretorio.ToString(), eAmbiente))
            {
                NotifyWarning("Diretório não encontrado.");
                return CustomResponse(ModelState);
            }

            var urls = await GerenciadorDeCaminhos.ObterURLs(eDiretorio.ToString(), eAmbiente);

            string obterExtensaoDoBase64 = InterpretadorDataUriImagem.ObterExtensaoDoBase64(putObraDto.ImagemBase64);
            string extrairStringBase64 = InterpretadorDataUriImagem.ExtrairStringBase64(putObraDto.ImagemBase64);

            if (obterExtensaoDoBase64 is null || extrairStringBase64 is null)
            {
                NotifyWarning("Extensão não suportada ou texto não se encontra em base64.");
                return CustomResponse(ModelState);
            }

            ViewObraDto obraAtualizada = await obraApplication.PutAsync(putObraDto, urls["IP"], urls["DNS"], urls["SPLIT"], extrairStringBase64, obterExtensaoDoBase64);

            if (obraAtualizada is null)
            {
                NotifyWarning("Nenhuma obra foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Obra e imagem atualizadas com sucesso!");
            }

            return CustomResponse(obraAtualizada);
        }

        private async Task<IActionResult> AtualizarSemAlterarImagem(PutObraDto putObraDto)
        {
            ViewObraDto obraAtualizada;

            using (Operation.Time("Tempo de atualização de uma obra sem alterar a imagem."))
            {
                logger.LogWarning("Foi requisitado a atualização de uma obra, mas sem alterar a imagem existente.");

                obraAtualizada = await obraApplication.PutAsync(putObraDto, "", "", "", "", "");
            }

            if (obraAtualizada is null)
            {
                NotifyWarning("Nenhuma obra foi encontrada com o id informado.");
                return CustomResponse(ModelState);
            }

            if (IsValidOperation())
            {
                NotifyWarning("Obra atualizada com sucesso, e a imagem permaneceu inalterada!");
            }

            return CustomResponse(obraAtualizada);
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
                    NotifyWarning("Obra atualizado para ativo com sucesso!");
                    break;

                case EStatus.Inativo:
                    NotifyWarning("Obra atualizado para inativo com sucesso!");
                    break;

                case EStatus.Excluido:
                    NotifyWarning("Obra atualizada para excluído com sucesso!");
                    break;

                default:
                    NotifyWarning("Status atualizado com sucesso.");
                    break;
            }

            return CustomResponse(atualizado);
        }

        /// <summary>
        /// Retorna detalhes de uma obra.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>Id: B9D01043-B103-42A1-AE3C-08DAD14324A9</remarks>
        [HttpGet("detalhes/{id:guid}")]
        [ProducesResponseType(typeof(ViewObraDetalhesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdDetalhesAsync(Guid id)
        {
            ViewObraDetalhesDto viewObraDetalhesDto = await obraApplication.GetByIdDetalhesAsync(id);
            if (viewObraDetalhesDto is null)
            {
                return CustomResponse(ModelState);
            }

            if (!IsValidOperation())
            {
                return CustomResponse(ModelState);
            }

            logger.LogWarning("Foi requisitado detalhes de uma obra. {@viewObraDetalhesDto}", viewObraDetalhesDto);

            if (IsValidOperation())
            {
                NotifyWarning("Detalhes de uma obra.");
            }

            return CustomResponse(viewObraDetalhesDto);
        }
    }
}