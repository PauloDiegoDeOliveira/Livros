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

            UploadBase64Methods<Obra> uploadClass = new();
            await uploadClass.UploadImagem(obra.CaminhoFisico, base64string);

            return mapper.Map<ViewObraDto>(await obraService.PostAsync(obra));
        }

        public async Task<ViewObraDto> PutAsync(PutObraDto putObraDto, string caminhoFisico, string caminhoAbsoluto, string splitRelativo, string base64string, string extensao)
        {
            Obra obra = mapper.Map<Obra>(putObraDto);

            var serviceResult = await obraService.GetByIdAsync(putObraDto.Id, false);
            if (serviceResult == null || !serviceResult.IsSuccess)
            {
                return null;
            }

            Obra consulta = serviceResult.Value;

            if (!string.IsNullOrWhiteSpace(base64string))
            {
                UploadBase64Methods<Obra> uploadClass = new();
                await uploadClass.DeleteImage(consulta);

                obra.PolulateInformations(await PathCreator.CreateIpPath(caminhoFisico),
                                          await PathCreator.CreateAbsolutePath(caminhoAbsoluto),
                                          await PathCreator.CreateRelativePath(await PathCreator.CreateAbsolutePath(caminhoAbsoluto), splitRelativo), extensao);

                await uploadClass.UploadImagem(obra.CaminhoFisico, base64string);
            }
            else
            {
                obra.PutInformations(consulta);
            }

            return mapper.Map<ViewObraDto>(await obraService.PutAsync(obra));
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