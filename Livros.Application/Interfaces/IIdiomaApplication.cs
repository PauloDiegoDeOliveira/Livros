using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Idioma;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Interfaces
{
    public interface IIdiomaApplication : IApplicationBase<Idioma, ViewIdiomaDto, PostIdiomaDto, PutIdiomaDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Idioma, ViewIdiomaDto>> GetPaginationAsync(ParametersIdioma parametersIdioma);

        bool ExisteId(Guid id);

        bool ExisteNomePostDto(PostIdiomaDto postIdiomaDto);

        bool ExisteNomePutDto(PutIdiomaDto putIdiomaDto);
    }
}