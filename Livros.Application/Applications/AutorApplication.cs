using AutoMapper;
using Livros.Application.Applications.Base;
using Livros.Application.Dtos.Autor;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Applications
{
    public class AutorApplication : ApplicationBase<Autor, ViewAutorDto, PostAutorDto, PutAutorDto, PutStatusDto>, IAutorApplication
    {
        private readonly IAutorService autorService;

        public AutorApplication(IAutorService autorService,
                                 INotifier notifier,
                                 IMapper mapper) : base(autorService, notifier, mapper)
        {
            this.autorService = autorService;
        }

        public async Task<ViewPagedListDto<Autor, ViewAutorDto>> GetPaginationAsync(ParametersAutor parametersAutor)
        {
            PagedList<Autor> autores = await autorService.GetPaginationAsync(parametersAutor);

            if (autores is null)
            {
                return null;
            }

            return new ViewPagedListDto<Autor, ViewAutorDto>(autores, mapper.Map<List<ViewAutorDto>>(autores));
        }

        public bool ExisteId(Guid id)
        {
            return autorService.ExisteId(id);
        }

        public bool ExisteNomePostDto(PostAutorDto postAutorDto)
        {
            Autor autor = mapper.Map<Autor>(postAutorDto);
            bool consulta = autorService.ExisteNome(autor);
            return mapper.Map<bool>(consulta);
        }

        public bool ExisteNomePutDto(PutAutorDto putAutorDto)
        {
            Autor autor = mapper.Map<Autor>(putAutorDto);
            bool consulta = autorService.ExisteNome(autor);
            return mapper.Map<bool>(consulta);
        }
    }
}