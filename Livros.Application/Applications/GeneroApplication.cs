using AutoMapper;
using Livros.Application.Applications.Base;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Genero;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Applications
{
    public class GeneroApplication : ApplicationBase<Genero, ViewGeneroDto, PostGeneroDto, PutGeneroDto, PutStatusDto>, IGeneroApplication
    {
        private readonly IGeneroService generoService;

        public GeneroApplication(IGeneroService generoService,
                                 INotifier notifier,
                                 IMapper mapper) : base(generoService, notifier, mapper)
        {
            this.generoService = generoService;
        }

        public async Task<ViewPagedListDto<Genero, ViewGeneroDto>> GetPaginationAsync(ParametersGenero parametersGenero)
        {
            PagedList<Genero> generos = await generoService.GetPaginationAsync(parametersGenero);
            if (generos is null)
            {
                return null;
            }

            return new ViewPagedListDto<Genero, ViewGeneroDto>(generos, mapper.Map<List<ViewGeneroDto>>(generos));
        }

        public bool ExisteId(Guid id)
        {
            return generoService.ExisteId(id);
        }

        public bool ExisteNomePostDto(PostGeneroDto postGeneroDto)
        {
            Genero genero = mapper.Map<Genero>(postGeneroDto);
            bool consulta = generoService.ExisteNome(genero);

            return mapper.Map<bool>(consulta);
        }

        public bool ExisteNomePutDto(PutGeneroDto putGeneroDto)
        {
            Genero genero = mapper.Map<Genero>(putGeneroDto);
            bool consulta = generoService.ExisteNome(genero);

            return mapper.Map<bool>(consulta);
        }
    }
}