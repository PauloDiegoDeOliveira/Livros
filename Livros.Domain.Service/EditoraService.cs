using Livros.Domain.Core.Interfaces.Repositories;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;
using Livros.Domain.Service.Base;

namespace Livros.Domain.Service
{
    public class EditoraService : ServiceBase<Editora>, IEditoraService
    {
        private readonly IEditoraRepository editoraRepository;

        public EditoraService(IEditoraRepository editoraRepository,
                              INotifier notifier) : base(editoraRepository, notifier)
        {
            this.editoraRepository = editoraRepository;
        }

        public async Task<PagedList<Editora>> GetPaginationAsync(ParametersEditora parametersEditora)
        {
            return await editoraRepository.GetPaginationAsync(parametersEditora);
        }

        public bool ExisteId(Guid id)
        {
            return editoraRepository.ExisteId(id);
        }

        public bool ExisteNome(Editora editora)
        {
            return editoraRepository.ExisteNome(editora);
        }
    }
}