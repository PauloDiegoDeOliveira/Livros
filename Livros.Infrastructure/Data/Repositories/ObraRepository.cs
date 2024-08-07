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
                        .Include(o => o.Volumes.OrderBy(v => v.Ordem))
                        .Include(o => o.Autores)
                        .Include(o => o.Editora)
                        .Include(o => o.Generos)
                        .Include(o => o.Idiomas)
                        .AsNoTracking();

                if (parametersObra.EditoraId != Guid.Empty)
                {
                    obras = obras.Where(o => o.EditoraId == parametersObra.EditoraId);
                }

                if (parametersObra.VolumeUnico)
                {
                    obras = obras.Where(o => o.VolumeUnico);
                }

                if (parametersObra.IdiomaId != null)
                {
                    obras = obras.Where(o => o.Idiomas.Any(o => parametersObra.IdiomaId.Contains(o.Id)));
                }

                if (parametersObra.AutorId != null)
                {
                    obras = obras.Where(o => o.Autores.Any(o => parametersObra.AutorId.Contains(o.Id)));
                }

                if (parametersObra.GeneroId != null)
                {
                    obras = obras.Where(o => o.Generos.Any(o => parametersObra.GeneroId.Contains(o.Id)));
                }

                if (parametersObra.Tipo != 0)
                {
                    obras = obras.Where(o => o.Tipo == parametersObra.Tipo.ToString());
                }

                if (parametersObra.DataInicio.HasValue && !parametersObra.DataFim.HasValue)
                {
                    obras = obras.Where(o => o.CriadoEm.Date >= parametersObra.DataInicio.Value.Date);
                }
                else if (!parametersObra.DataInicio.HasValue && parametersObra.DataFim.HasValue)
                {
                    obras = obras.Where(o => o.CriadoEm.Date <= parametersObra.DataFim.Value.Date);
                }
                else if (parametersObra.DataInicio.HasValue && parametersObra.DataFim.HasValue)
                {
                    obras = obras.Where(o => o.CriadoEm.Date >= parametersObra.DataInicio.Value.Date && o.CriadoEm.Date <= parametersObra.DataFim.Value.Date);
                }

                if (parametersObra.Id != null)
                {
                    obras = obras.Where(o => parametersObra.Id.Contains(o.Id));
                }

                if (parametersObra.PalavraChave != null)
                {
                    obras = obras.Where(o => o.Nome.ToLower().Trim().Contains(parametersObra.PalavraChave.ToLower().Trim()));
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
                                obras = obras.OrderBy(o => o.Nome);
                                break;

                            case EOrdenar.Decrescente:
                                obras = obras.OrderByDescending(o => o.Nome);
                                break;

                            case EOrdenar.Novos:
                                obras = obras.OrderByDescending(o => o.CriadoEm);
                                break;

                            case EOrdenar.Antigos:
                                obras = obras.OrderBy(o => o.CriadoEm);
                                break;
                        }
                    }
                }

                return await Task.FromResult(PagedList<Obra>.ToPagedList(obras, parametersObra.NumeroPagina, parametersObra.ResultadosExibidos));
            });
        }

        private void DefinirUsuarioId(Obra obra)
        {
            obra.UsuarioId = user.GetUserId().ToString();
        }

        private void AtualizarOrdemVolume(IList<Volume> volumes)
        {
            int numero = 1;
            foreach (Volume volume in volumes)
            {
                volume.Ordem = numero++;
            }
        }

        public override async Task<Obra> PostAsync(Obra obra)
        {
            DefinirUsuarioId(obra);
            AtualizarOrdemVolume(obra.Volumes);
            await InserirIdiomasAsync(obra);
            await InserirGenerosAsync(obra);
            await InserirAutoresAsync(obra);

            return await base.PostAsync(obra);
        }

        private async Task InserirIdiomasAsync(Obra obra)
        {
            List<Idioma> idiomasConsultados = new();
            foreach (Idioma idioma in obra.Idiomas)
            {
                Idioma idiomaConsultado = await appDbContext.Idiomas.FindAsync(idioma.Id);
                idiomasConsultados.Add(idiomaConsultado);
            }

            obra.ListaIdiomas(idiomasConsultados);
        }

        private async Task InserirGenerosAsync(Obra obra)
        {
            List<Genero> generosConsultados = new();
            foreach (Genero genero in obra.Generos)
            {
                Genero generoConsultado = await appDbContext.Generos.FindAsync(genero.Id);
                generosConsultados.Add(generoConsultado);
            }

            obra.ListaGeneros(generosConsultados);
        }

        private async Task InserirAutoresAsync(Obra obra)
        {
            List<Autor> autoresConsultados = new();
            foreach (Autor autor in obra.Autores)
            {
                Autor autorConsultado = await appDbContext.Autores.FindAsync(autor.Id);
                autoresConsultados.Add(autorConsultado);
            }

            obra.ListaAutores(autoresConsultados);
        }

        public override async Task<Obra> PutAsync(Obra obra)
        {
            DefinirUsuarioId(obra);
            AtualizarOrdemVolume(obra.Volumes);

            Obra obraConsultado = await appDbContext.Obras
                        .Include(o => o.Volumes)
                        .Include(o => o.Idiomas)
                        .Include(o => o.Generos)
                        .Include(o => o.Autores)
                        .FirstOrDefaultAsync(o => o.Id == obra.Id);

            appDbContext.Entry(obraConsultado).CurrentValues.SetValues(obra);

            UpdateVolumeAsync(obra, obraConsultado);
            await AtualizarIdiomasAsync(obra, obraConsultado);
            await AtualizarAutoresAsync(obra, obraConsultado);
            await AtualizarGenerosAsync(obra, obraConsultado);

            await SaveChangesAsync();

            return obra;
        }

        private static void UpdateVolumeAsync(Obra obra, Obra obraConsultado)
        {
            obraConsultado.Volumes.Clear();
            obraConsultado.Volumes = obra.Volumes;
        }

        private async Task AtualizarIdiomasAsync(Obra obra, Obra obraConsultado)
        {
            obraConsultado.Idiomas.Clear();
            foreach (Idioma idioma in obra.Idiomas)
            {
                Idioma idiomaConsultado = await appDbContext.Idiomas.FindAsync(idioma.Id);
                obraConsultado.Idiomas.Add(idiomaConsultado);
            }
        }

        private async Task AtualizarAutoresAsync(Obra obra, Obra obraConsultado)
        {
            obraConsultado.Autores.Clear();
            foreach (Autor autor in obra.Autores)
            {
                Autor autorConsultado = await appDbContext.Autores.FindAsync(autor.Id);
                obraConsultado.Autores.Add(autorConsultado);
            }
        }

        private async Task AtualizarGenerosAsync(Obra obra, Obra obraConsultado)
        {
            obraConsultado.Generos.Clear();
            foreach (Genero genero in obra.Generos)
            {
                Genero generoConsultado = await appDbContext.Generos.FindAsync(genero.Id);
                obraConsultado.Generos.Add(generoConsultado);
            }
        }

        public async Task<Obra> GetByIdDetalhesAsync(Guid obraId)
        {
            return await appDbContext.Obras
                     .Include(o => o.Volumes.OrderBy(v => v.Ordem))
                     .Include(o => o.Editora)
                     .Include(o => o.Generos)
                     .Include(o => o.Autores)
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
                .Where(o => o.Nome.ToLower() == obra.Nome.ToLower()
                       && o.UsuarioId == user.GetUserId().ToString()
                       && o.Status != EStatus.Excluido.ToString());

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
    }
}