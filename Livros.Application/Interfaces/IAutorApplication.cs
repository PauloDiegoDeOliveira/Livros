using Livros.Application.Dtos.Autor;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Interfaces
{
    public interface IAutorApplication : IApplicationBase<Autor, ViewAutorDto, PostAutorDto, PutAutorDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Autor, ViewAutorDto>> GetPaginationAsync(ParametersAutor parametersAutor);

        bool ExisteId(Guid id);

        bool ExisteNomePostDto(PostAutorDto postAutorDto);

        bool ExisteNomePutDto(PutAutorDto putAutorDto);
    }
}