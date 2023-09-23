using Livros.Domain.Core.Interfaces.Repositories;
using Livros.Domain.Core.Interfaces.Services;
using Livros.Domain.Core.Notificacoes;
using Livros.Domain.Entities;
using Livros.Domain.Enums;
using Livros.Domain.Pagination;
using Livros.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Livros.Infrastructure.Data.Repositories
{
    public class EstanteRepository : RepositoryBase<Estante>, IEstanteRepository
    {
        private readonly IUser user;

        public EstanteRepository(AppDbContext appDbContext,
                                 INotifier notifier,
                                 ILogger<RepositoryBase<Estante>> logger,
                                 IConfiguration configuration,
                                 IUser user) : base(appDbContext, notifier, logger, configuration)
        {
            this.user = user;
        }

        public async Task<PagedList<Estante>> GetPaginationAsync(ParametersEstante parametersEstante)
        {
            return await TryCatch(async () =>
            {
                IQueryable<Estante> estantes = appDbContext.Estantes
                                .Where(e => e.UsuarioId == user.GetUserId().ToString())
                                .AsNoTracking();

                if (parametersEstante.Id != null)
                {
                    estantes = estantes.Where(e => parametersEstante.Id.Contains(e.Id));
                }

                if (parametersEstante.PalavraChave != null)
                {
                    estantes = estantes.Where(e => e.Nome.ToLower().Trim().Contains(parametersEstante.PalavraChave.ToLower().Trim()));
                }

                if (parametersEstante.Status != 0)
                {
                    estantes = estantes.Where(e => e.Status == parametersEstante.Status.ToString());
                }

                if (!estantes.Any())
                {
                    AddNotification("Nenhum objeto foi encontrado.", ENotificationType.Warning);
                }
                else if (estantes.Count() > 1)
                {
                    if (parametersEstante.Ordenar != 0)
                    {
                        switch (parametersEstante.Ordenar)
                        {
                            case EOrdenar.Crescente:
                                estantes = estantes.OrderBy(e => e.Nome);
                                break;

                            case EOrdenar.Decrescente:
                                estantes = estantes.OrderByDescending(e => e.Nome);
                                break;

                            case EOrdenar.Novos:
                                estantes = estantes.OrderByDescending(e => e.CriadoEm);
                                break;

                            case EOrdenar.Antigos:
                                estantes = estantes.OrderBy(e => e.CriadoEm);
                                break;
                        }
                    }
                    else
                    {
                        estantes = estantes.OrderBy(e => e.CriadoEm);
                    }
                }

                return await Task.FromResult(PagedList<Estante>.ToPagedList(estantes, parametersEstante.NumeroPagina, parametersEstante.ResultadosExibidos));
            });
        }

        private void DefinirUsuarioId(Estante estante)
        {
            estante.UsuarioId = user.GetUserId().ToString();
        }

        public override async Task<Estante> PostAsync(Estante estante)
        {
            DefinirUsuarioId(estante);
            estante.Nome = await ObterNomeIncrementado(estante.Nome);

            return await base.PostAsync(await InserirObrasAsync(estante));
        }

        private async Task<string> ObterNomeIncrementado(string nomeBase)
        {
            if (!await EstanteExisteParaUsuario(nomeBase))
            {
                return nomeBase;
            }

            string nomeNovo;
            int incremento = 2;

            do
            {
                nomeNovo = $"{nomeBase} ({incremento})";
                incremento++;
            }
            while (await EstanteExisteParaUsuario(nomeNovo));

            return nomeNovo;
        }

        private async Task<bool> EstanteExisteParaUsuario(string nome)
        {
            return await appDbContext.Estantes.AsNoTracking()
                            .AnyAsync(e => e.Nome.ToLower() == nome.ToLower()
                                    && e.Status != EStatus.Excluido.ToString()
                                    && e.UsuarioId == user.GetUserId().ToString());
        }

        private async Task<Estante> InserirObrasAsync(Estante estante)
        {
            List<Obra> obras = await appDbContext.Obras
                 .Where(obra => estante.Obras.Select(e => e.Id).Contains(obra.Id))
                 .ToListAsync();

            if (obras.Count > 50)
            {
                AddNotification("A estante não pode ter mais de 50 obras.");
            }

            estante.Obras = obras;

            return estante;
        }

        public override async Task<Estante> PutAsync(Estante estante)
        {
            DefinirUsuarioId(estante);
            estante.Nome = await ObterNomeIncrementadoParaPut(estante.Nome, estante.Id);

            return await base.PutAsync(await AtualizarEstanteAsync(estante));
        }

        private async Task<string> ObterNomeIncrementadoParaPut(string nomeBase, Guid estanteId)
        {
            if (!await EstanteExisteParaOutroUsuario(nomeBase, estanteId))
            {
                return nomeBase;
            }

            string nomeNovo;
            int incremento = 2;

            do
            {
                nomeNovo = $"{nomeBase} ({incremento})";
                incremento++;
            }
            while (await EstanteExisteParaOutroUsuario(nomeNovo, estanteId));

            return nomeNovo;
        }

        private async Task<bool> EstanteExisteParaOutroUsuario(string nome, Guid obraId)
        {
            return await appDbContext.Estantes.AsNoTracking()
                .AnyAsync(p => p.Nome.ToLower() == nome.ToLower()
                            && p.Status != EStatus.Excluido.ToString()
                            && p.UsuarioId == user.GetUserId().ToString()
                            && p.Id != obraId);
        }

        private async Task<Estante> AtualizarEstanteAsync(Estante estante)
        {
            Estante estanteConsultado = await appDbContext.Estantes
                         .Include(e => e.Obras)
                         .FirstOrDefaultAsync(e => e.Id == estante.Id);

            if (estanteConsultado == null)
            {
                AddNotification("Nenhuma estante foi encontrada com o id informado.");
                return null;
            }

            appDbContext.Entry(estanteConsultado).CurrentValues.SetValues(estante);
            await AtualizarObraAsync(estante, estanteConsultado);

            return estanteConsultado;
        }

        private async Task AtualizarObraAsync(Estante estante, Estante estanteConsultado)
        {
            estanteConsultado.Obras.Clear();
            foreach (Obra obra in estante.Obras)
            {
                Obra obraConsultado = await appDbContext.Obras.FindAsync(obra.Id);
                estanteConsultado.Obras.Add(obraConsultado);
            }
        }

        public bool ExisteId(Guid id)
        {
            return appDbContext.Estantes.Any(e => e.Id == id);
        }

        public bool ExisteNome(Estante estante)
        {
            if (estante.Id == Guid.Empty)
            {
                return appDbContext.Estantes.AsNoTracking()
                             .Any(e => e.Nome.ToLower() == estante.Nome.ToLower());
            }
            else
            {
                return appDbContext.Estantes.AsNoTracking()
                            .Any(e => e.Nome.ToLower() == estante.Nome.ToLower()
                                && e.Id != estante.Id);
            }
        }
    }
}