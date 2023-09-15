using Livros.Domain.Core.Interfaces.Repositories;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;
using Livros.Domain.Service.Base;

namespace Livros.Domain.Service
{
    public class EstanteService : ServiceBase<Estante>, IEstanteService
    {
        private readonly IEstanteRepository estanteRepository;

        public EstanteService(IEstanteRepository estanteRepository,
                              INotifier notifier) : base(estanteRepository, notifier)
        {
            this.estanteRepository = estanteRepository;
        }

        public async Task<PagedList<Estante>> GetPaginationAsync(ParametersEstante parametersEstante)
        {
            return await estanteRepository.GetPaginationAsync(parametersEstante);
        }

        public bool ExisteId(Guid id)
        {
            return estanteRepository.ExisteId(id);
        }

        public bool ExisteNome(Estante estante)
        {
            return estanteRepository.ExisteNome(estante);
        }
    }
}