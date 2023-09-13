using AutoMapper;
using Livros.Application.Applications.Base;
using Livros.Application.Dtos.Base;
using Livros.Application.Dtos.Editora;
using Livros.Application.Dtos.Pagination;
using Livros.Application.Interfaces;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;

namespace Livros.Application.Applications
{
    public class EditoraApplication : ApplicationBase<Editora, ViewEditoraDto, PostEditoraDto, PutEditoraDto, PutStatusDto>, IEditoraApplication
    {
        private readonly IEditoraService editoraService;

        public EditoraApplication(IEditoraService editoraService,
                                  INotifier notifier,
                                  IMapper mapper) : base(editoraService, notifier, mapper)
        {
            this.editoraService = editoraService;
        }

        public async Task<ViewPagedListDto<Editora, ViewEditoraDto>> GetPaginationAsync(ParametersEditora parametersEditora)
        {
            PagedList<Editora> editoras = await editoraService.GetPaginationAsync(parametersEditora);
            if (editoras is null)
            {
                return null;
            }

            return new ViewPagedListDto<Editora, ViewEditoraDto>(editoras, mapper.Map<List<ViewEditoraDto>>(editoras));
        }

        public bool ExisteId(Guid id)
        {
            return editoraService.ExisteId(id);
        }

        public bool ExisteNomePostDto(PostEditoraDto postEditoraDto)
        {
            Editora editora = mapper.Map<Editora>(postEditoraDto);
            bool consulta = editoraService.ExisteNome(editora);

            return mapper.Map<bool>(consulta);
        }

        public bool ExisteNomePutDto(PutEditoraDto putEditoraDto)
        {
            Editora editora = mapper.Map<Editora>(putEditoraDto);
            bool consulta = editoraService.ExisteNome(editora);

            return mapper.Map<bool>(consulta);
        }
    }
}