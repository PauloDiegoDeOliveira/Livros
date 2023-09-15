using AutoMapper;
using Livros.Application.Applications.Base;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Idioma;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Applications
{
    public class IdiomaApplication : ApplicationBase<Idioma, ViewIdiomaDto, PostIdiomaDto, PutIdiomaDto, PutStatusDto>, IIdiomaApplication
    {
        private readonly IIdiomaService idiomaService;

        public IdiomaApplication(IIdiomaService idiomaService,
                                 INotifier notifier,
                                 IMapper mapper) : base(idiomaService, notifier, mapper)
        {
            this.idiomaService = idiomaService;
        }

        public async Task<ViewPagedListDto<Idioma, ViewIdiomaDto>> GetPaginationAsync(ParametersIdioma parametersIdioma)
        {
            PagedList<Idioma> idiomas = await idiomaService.GetPaginationAsync(parametersIdioma);

            if (idiomas is null)
            {
                return null;
            }

            return new ViewPagedListDto<Idioma, ViewIdiomaDto>(idiomas, mapper.Map<List<ViewIdiomaDto>>(idiomas));
        }

        public bool ExisteId(Guid id)
        {
            return idiomaService.ExisteId(id);
        }

        public bool ExisteNomePostDto(PostIdiomaDto postIdiomaDto)
        {
            Idioma idioma = mapper.Map<Idioma>(postIdiomaDto);
            bool consulta = idiomaService.ExisteNome(idioma);
            return mapper.Map<bool>(consulta);
        }

        public bool ExisteNomePutDto(PutIdiomaDto putIdiomaDto)
        {
            Idioma idioma = mapper.Map<Idioma>(putIdiomaDto);
            bool consulta = idiomaService.ExisteNome(idioma);
            return mapper.Map<bool>(consulta);
        }
    }
}