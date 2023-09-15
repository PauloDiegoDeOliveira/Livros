using AutoMapper;
using Livros.Application.Applications.Base;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Estante;
using Livros.Application.Dtos.Lista;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Applications
{
    public class EstanteApplication : ApplicationBase<Estante, ViewEstanteDto, PostEstanteDto, PutEstanteDto, PutStatusDto>, IEstanteApplication
    {
        private readonly IEstanteService estanteService;

        public EstanteApplication(IEstanteService estanteService,
                                  INotifier notifier,
                                  IMapper mapper) : base(estanteService, notifier, mapper)
        {
            this.estanteService = estanteService;
        }

        public async Task<ViewPagedListDto<Estante, ViewEstanteDto>> GetPaginationAsync(ParametersEstante parametersEstante)
        {
            PagedList<Estante> estantes = await estanteService.GetPaginationAsync(parametersEstante);
            if (estantes is null)
            {
                return null;
            }

            return new ViewPagedListDto<Estante, ViewEstanteDto>(estantes, mapper.Map<List<ViewEstanteDto>>(estantes));
        }

        public bool ExisteId(Guid id)
        {
            return estanteService.ExisteId(id);
        }

        public bool ExisteNomePostDto(PostEstanteDto postEstanteDto)
        {
            Estante estante = mapper.Map<Estante>(postEstanteDto);
            bool consulta = estanteService.ExisteNome(estante);

            return mapper.Map<bool>(consulta);
        }

        public bool ExisteNomePutDto(PutEstanteDto putEstanteDto)
        {
            Estante estante = mapper.Map<Estante>(putEstanteDto);
            bool consulta = estanteService.ExisteNome(estante);

            return mapper.Map<bool>(consulta);
        }
    }
}