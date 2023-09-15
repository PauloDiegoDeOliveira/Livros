using Livros.Domain.Core.Interfaces.Repositories;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Entities;
using Livros.Domain.Pagination;
using Livros.Domain.Service.Base;

namespace Livros.Domain.Service
{
    public class ObraService : ServiceBase<Obra>, IObraService
    {
        private readonly IObraRepository obraRepository;

        public ObraService(IObraRepository obraRepository,
                           INotifier notifier) : base(obraRepository, notifier)
        {
            this.obraRepository = obraRepository;
        }

        public async Task<PagedList<Obra>> GetPaginationAsync(ParametersObra parametersObra)
        {
            return await obraRepository.GetPaginationAsync(parametersObra);
        }

        public async Task<Obra> GetByIdDetalhesAsync(Guid obraId)
        {
            return await obraRepository.GetByIdDetalhesAsync(obraId);
        }

        public bool ExisteId(Guid id)
        {
            return obraRepository.ExisteId(id);
        }

        public bool ExisteNome(Obra obra)
        {
            return obraRepository.ExisteNome(obra);
        }
    }
}