using AutoMapper;
using Livros.Application.Applications.Base;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Obra;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces;
using Livros.Application.Utilities.Image;
using Livros.Application.Utilities.Paths;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Applications
{
    public class ObraApplication : ApplicationBase<Obra, ViewObraDto, PostObraDto, PutObraDto, PutStatusDto>, IObraApplication
    {
        private readonly IObraService obraService;

        public ObraApplication(IObraService obraService,
                               INotifier notifier,
                               IMapper mapper) : base(obraService, notifier, mapper)
        {
            this.obraService = obraService;
        }

        public async Task<ViewPagedListDto<Obra, ViewObraDto>> GetPaginationAsync(ParametersObra parametersObra)
        {
            PagedList<Obra> obras = await obraService.GetPaginationAsync(parametersObra);
            if (obras is null)
            {
                return null;
            }

            return new ViewPagedListDto<Obra, ViewObraDto>(obras, mapper.Map<List<ViewObraDto>>(obras));
        }

        public async Task<ViewObraDto> PostAsync(PostObraDto postObraDto, string caminhoFisico, string caminhoAbsoluto, string splitRelativo, string base64string, string extensao)
        {
            Obra obra = mapper.Map<Obra>(postObraDto);

            obra.PolulateInformations(await PathCreator.CreateIpPath(caminhoFisico),
                                      await PathCreator.CreateAbsolutePath(caminhoAbsoluto),
                                      await PathCreator.CreateRelativePath(await PathCreator.CreateAbsolutePath(caminhoAbsoluto), splitRelativo), extensao);

            UploadBase64Methods<Obra> uploadBase64Methods = new();
            await uploadBase64Methods.UploadImagem(obra.CaminhoFisico, base64string);

            return mapper.Map<ViewObraDto>(await obraService.PostAsync(obra));
        }

        public async Task<ViewObraDto> PutAsync(PutObraDto putObraDto, string caminhoFisico, string caminhoAbsoluto, string splitRelativo, string base64string, string extensao)
        {
            var serviceResult = await obraService.GetByIdAsync(putObraDto.Id, false);
            if (serviceResult == null || !serviceResult.IsSuccess)
            {
                return null;
            }

            Obra consulta = serviceResult.Value;
            Obra obra = mapper.Map<Obra>(putObraDto);

            if (!string.IsNullOrWhiteSpace(base64string))
            {
                await TratarAtualizacaoDeImagem(consulta, obra, caminhoFisico, caminhoAbsoluto, splitRelativo, base64string, extensao);
            }
            else if (base64string == null)
            {
                UploadBase64Methods<Obra> uploadBase64Methods = new();
                await uploadBase64Methods.DeleteImage(consulta);

                consulta.NomeArquivo = null;
                consulta.CaminhoAbsoluto = null;
                consulta.CaminhoRelativo = null;
                consulta.CaminhoFisico = null;
                consulta.HoraEnvio = null;
                obra.PutInformations(consulta);
            }
            else if (base64string == "")
            {
                obra.PutInformations(consulta);
            }

            return mapper.Map<ViewObraDto>(await obraService.PutAsync(obra));
        }

        private async Task TratarAtualizacaoDeImagem(Obra consulta, Obra obra, string caminhoFisico, string caminhoAbsoluto, string splitRelativo, string base64string, string extensao)
        {
            UploadBase64Methods<Obra> uploadBase64Methods = new();
            await uploadBase64Methods.DeleteImage(consulta);

            obra.PolulateInformations(await PathCreator.CreateIpPath(caminhoFisico),
                                      await PathCreator.CreateAbsolutePath(caminhoAbsoluto),
                                      await PathCreator.CreateRelativePath(await PathCreator.CreateAbsolutePath(caminhoAbsoluto), splitRelativo), extensao);

            await uploadBase64Methods.UploadImagem(obra.CaminhoFisico, base64string);
        }

        public bool ExisteId(Guid id)
        {
            return obraService.ExisteId(id);
        }

        public bool ExisteNomePostDto(PostObraDto postObraDto)
        {
            Obra obra = mapper.Map<Obra>(postObraDto);
            bool consulta = obraService.ExisteNome(obra);

            return mapper.Map<bool>(consulta);
        }

        public bool ExisteNomePutDto(PutObraDto putObraDto)
        {
            Obra obra = mapper.Map<Obra>(putObraDto);
            bool consulta = obraService.ExisteNome(obra);

            return mapper.Map<bool>(consulta);
        }
    }
}