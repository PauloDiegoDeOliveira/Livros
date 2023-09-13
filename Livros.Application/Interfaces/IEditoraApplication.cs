using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Editora;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces.Base;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Interfaces
{
    public interface IEditoraApplication : IApplicationBase<Editora, ViewEditoraDto, PostEditoraDto, PutEditoraDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Editora, ViewEditoraDto>> GetPaginationAsync(ParametersEditora parametersEditora);

        bool ExisteId(Guid id);

        bool ExisteNomePostDto(PostEditoraDto postEditoraDto);

        bool ExisteNomePutDto(PutEditoraDto putEditoraDto);
    }
}