using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Estante;
using Livros.Application.Dtos.Lista;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Interfaces
{
    public interface IEstanteApplication : IApplicationBase<Estante, ViewEstanteDto, PostEstanteDto, PutEstanteDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Estante, ViewEstanteDto>> GetPaginationAsync(ParametersEstante parametersEstante);

        bool ExisteId(Guid id);

        bool ExisteNomePostDto(PostEstanteDto postEstanteDto);

        bool ExisteNomePutDto(PutEstanteDto putEstanteDto);
    }
}