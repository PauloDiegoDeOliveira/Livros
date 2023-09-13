using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Genero;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Interfaces
{
    public interface IGeneroApplication : IApplicationBase<Genero, ViewGeneroDto, PostGeneroDto, PutGeneroDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Genero, ViewGeneroDto>> GetPaginationAsync(ParametersGenero parametersGenero);

        bool ExisteId(Guid id);

        bool ExisteNomePostDto(PostGeneroDto postGeneroDto);

        bool ExisteNomePutDto(PutGeneroDto putGeneroDto);
    }
}