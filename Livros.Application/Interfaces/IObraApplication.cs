using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Obra;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Interfaces
{
    public interface IObraApplication : IApplicationBase<Obra, ViewObraDto, PostObraDto, PutObraDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Obra, ViewObraDto>> GetPaginationAsync(ParametersObra parametersObra);

        bool ExisteId(Guid id);

        bool ExisteNomePostDto(PostObraDto postObraDto);

        bool ExisteNomePutDto(PutObraDto putObraDto);
    }
}