using AutoMapper;
using Livros.Application.Applications.Base;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Obra;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces;
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