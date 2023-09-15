using Livros.Domain.Core.Interfaces.Repositories;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;
using Livros.Domain.Service.Base;

namespace Livros.Domain.Service
{
    public class AutorService : ServiceBase<Autor>, IAutorService
    {
        private readonly IAutorRepository autorRepository;

        public AutorService(IAutorRepository autorRepository,
                              INotifier notifier) : base(autorRepository, notifier)
        {
            this.autorRepository = autorRepository;
        }

        public async Task<PagedList<Autor>> GetPaginationAsync(ParametersAutor parametersAutor)
        {
            return await autorRepository.GetPaginationAsync(parametersAutor);
        }

        public bool ExisteId(Guid id)
        {
            return autorRepository.ExisteId(id);
        }

        public bool ExisteNome(Autor autor)
        {
            return autorRepository.ExisteNome(autor);
        }
    }
}