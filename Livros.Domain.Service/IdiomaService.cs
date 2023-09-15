using Livros.Domain.Core.Interfaces.Repositories;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;
using Livros.Domain.Service.Base;

namespace Livros.Domain.Service
{
    public class IdiomaService : ServiceBase<Idioma>, IIdiomaService
    {
        private readonly IIdiomaRepository idiomaRepository;

        public IdiomaService(IIdiomaRepository idiomaRepository,
                              INotifier notifier) : base(idiomaRepository, notifier)
        {
            this.idiomaRepository = idiomaRepository;
        }

        public async Task<PagedList<Idioma>> GetPaginationAsync(ParametersIdioma paramtersIdioma)
        {
            return await idiomaRepository.GetPaginationAsync(paramtersIdioma);
        }

        public bool ExisteId(Guid id)
        {
            return idiomaRepository.ExisteId(id);
        }

        public bool ExisteNome(Idioma idioma)
        {
            return idiomaRepository.ExisteNome(idioma);
        }
    }
}