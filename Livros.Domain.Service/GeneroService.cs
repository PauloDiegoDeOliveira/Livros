using Livros.Domain.Core.Interfaces.Repositories;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;
using Livros.Domain.Service.Base;

namespace Livros.Domain.Service
{
    public class GeneroService : ServiceBase<Genero>, IGeneroService
    {
        private readonly IGeneroRepository generoRepository;

        public GeneroService(IGeneroRepository generoRepository,
                              INotifier notifier) : base(generoRepository, notifier)
        {
            this.generoRepository = generoRepository;
        }

        public async Task<PagedList<Genero>> GetPaginationAsync(ParametersGenero parametersGenero)
        {
            return await generoRepository.GetPaginationAsync(parametersGenero);
        }

        public bool ExisteId(Guid id)
        {
            return generoRepository.ExisteId(id);
        }

        public bool ExisteNome(Genero genero)
        {
            return generoRepository.ExisteNome(genero);
        }
    }
}