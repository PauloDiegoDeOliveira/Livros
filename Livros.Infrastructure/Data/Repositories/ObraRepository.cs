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
    public class ObraRepository : RepositoryBase<Obra>, IObraRepository
    {
        private readonly IUser user;

        public ObraRepository(AppDbContext appDbContext,
                              INotifier notifier,
                              ILogger<RepositoryBase<Obra>> logger,
                              IConfiguration configuration,
                              IUser user) : base(appDbContext, notifier, logger, configuration)
        {
            this.user = user;
        }

        public async Task<PagedList<Obra>> GetPaginationAsync(ParametersObra parametersObra)
        {
            return await TryCatch(async () =>
            {
                IQueryable<Obra> obras = appDbContext.Obras.Where(o => o.UsuarioId == user.GetUserId().ToString())
                        .Include(o => o.Volumes)
                        .AsNoTracking();

                if (parametersObra.Id != null)
                {
                    obras = obras.Where(o => parametersObra.Id.Contains(o.Id));
                }

                if (parametersObra.PalavraChave != null)
                {
                    obras = obras.Where(o => o.Titulo.ToLower().Trim().Contains(parametersObra.PalavraChave.ToLower().Trim()));
                }

                if (parametersObra.Status != 0)
                {
                    obras = obras.Where(o => o.Status == parametersObra.Status.ToString());
                }

                if (!obras.Any())
                {
                    AddNotification("Nenhum objeto foi encontrado.", ENotificationType.Warning);
                }
                else if (obras.Count() > 1)
                {
                    if (parametersObra.Ordenar != 0)
                    {
                        switch (parametersObra.Ordenar)
                        {
                            case EOrdenar.Crescente:
                                obras = obras.OrderBy(o => o.Titulo);
                                break;

                            case EOrdenar.Decrescente:
                                obras = obras.OrderByDescending(o => o.Titulo);
                                break;

                            case EOrdenar.Novos:
                                obras = obras.OrderByDescending(o => o.CriadoEm);
                                break;

                            case EOrdenar.Antigos:
                                obras = obras.OrderBy(o => o.CriadoEm);
                                break;
                        }
                    }
                    else
                    {
                        obras = obras.OrderBy(o => o.CriadoEm);
                    }
                }

                return await Task.FromResult(PagedList<Obra>.ToPagedList(obras, parametersObra.NumeroPagina, parametersObra.ResultadosExibidos));
            });
        }

        private void DefinirUsuarioId(Obra obra)
        {
            obra.UsuarioId = user.GetUserId().ToString();
        }

        public override async Task<Obra> PostAsync(Obra obra)
        {
            DefinirUsuarioId(obra);
            await InsertIdiomasAsync(obra);

            return await base.PostAsync(obra);
        }

        private async Task InsertIdiomasAsync(Obra obra)
        {
            List<Idioma> idiomasConsultados = new();

            foreach (Idioma idioma in obra.Idiomas)
            {
                Idioma idiomaConsultado = await appDbContext.Idiomas.FindAsync(idioma.Id);
                idiomasConsultados.Add(idiomaConsultado);
            }

            obra.ListaIdiomas(idiomasConsultados);
        }

        public override async Task<Obra> PutAsync(Obra obra)
        {
            Obra obraConsultado = await appDbContext.Obras
                        .Include(o => o.Volumes)
                        .Include(o => o.Idiomas)
                        .FirstOrDefaultAsync(o => o.Id == obra.Id);

            appDbContext.Entry(obraConsultado).CurrentValues.SetValues(obra);

            DefinirUsuarioId(obra);
            UpdateVolumeAsync(obra, obraConsultado);
            await UpdateIdiomasAsync(obra, obraConsultado);

            await SaveChangesAsync();

            return obra;
        }

        private static void UpdateVolumeAsync(Obra obra, Obra obraConsultado)
        {
            obraConsultado.Volumes.Clear();
            obraConsultado.Volumes = obra.Volumes;
        }

        private async Task UpdateIdiomasAsync(Obra obra, Obra obraConsultado)
        {
            obraConsultado.Idiomas.Clear();
            foreach (Idioma idioma in obra.Idiomas)
            {
                Idioma idiomaConsultado = await appDbContext.Idiomas.FindAsync(idioma.Id);
                obraConsultado.Idiomas.Add(idiomaConsultado);
            }
        }

        public async Task<Obra> GetByIdDetalhesAsync(Guid obraId)
        {
            return await appDbContext.Obras
                     .Include(o => o.Volumes)
                     .Include(o => o.Editora)
                     .Include(o => o.Genero)
                     .Include(o => o.Autor)
                     .Include(o => o.Estantes)
                     .AsNoTracking()
                     .FirstOrDefaultAsync(o => o.Id == obraId);
        }

        public bool ExisteId(Guid id)
        {
            return appDbContext.Obras.Any(o => o.Id == id);
        }

        public bool ExisteNome(Obra obra)
        {
            IQueryable<Obra> query = appDbContext.Obras.AsNoTracking()
                .Where(o => o.Titulo.ToLower() == obra.Titulo.ToLower());

            if (obra.Id != Guid.Empty)
            {
                query = query.Where(o => o.Id != obra.Id);
            }

            return query.Any();
        }

        public bool ExisteVolumeId(Guid id)
        {
            return appDbContext.Volumes.Any(v => v.Id == id);
        }

        public bool ExisteNomeVolume(Volume volume)
        {
            IQueryable<Volume> query = appDbContext.Volumes.AsNoTracking()
                .Where(v => v.Titulo.ToLower() == volume.Titulo.ToLower());

            if (volume.Id != Guid.Empty)
            {
                query = query.Where(v => v.Id != volume.Id);
            }

            return query.Any();
        }
    }
}