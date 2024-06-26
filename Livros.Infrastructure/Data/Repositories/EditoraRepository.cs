﻿using Livros.Domain.Core.Interfaces.Repositories;
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
    public class EditoraRepository : RepositoryBase<Editora>, IEditoraRepository
    {
        private readonly IUser user;

        public EditoraRepository(AppDbContext appDbContext,
                                 INotifier notifier,
                                 ILogger<RepositoryBase<Editora>> logger,
                                 IConfiguration configuration,
                                 IUser user) : base(appDbContext, notifier, logger, configuration)
        {
            this.user = user;
        }

        public async Task<PagedList<Editora>> GetPaginationAsync(ParametersEditora parametersEditora)
        {
            return await TryCatch(async () =>
            {
                IQueryable<Editora> editoras = appDbContext.Editoras.Where(e => e.UsuarioId == user.GetUserId().ToString())
                    .Include(e => e.Obras.Where(e => e.UsuarioId == user.GetUserId().ToString()))
                    .AsNoTracking();

                if (parametersEditora.Id != null)
                {
                    editoras = editoras.Where(e => parametersEditora.Id.Contains(e.Id));
                }

                if (parametersEditora.PalavraChave != null)
                {
                    editoras = editoras.Where(e => e.Nome.ToLower().Trim().Contains(parametersEditora.PalavraChave.ToLower().Trim()));
                }

                if (parametersEditora.Status != 0)
                {
                    editoras = editoras.Where(e => e.Status == parametersEditora.Status.ToString());
                }

                if (parametersEditora.QuantidadeObras != 0)
                {
                    switch (parametersEditora.QuantidadeObras)
                    {
                        case EQuantidadeObras.Crescente:
                            editoras = editoras.OrderBy(e => e.Obras.Count());
                            break;

                        case EQuantidadeObras.Decrescente:
                            editoras = editoras.OrderByDescending(e => e.Obras.Count());
                            break;
                    }
                }

                if (!editoras.Any())
                {
                    AddNotification("Nenhum objeto foi encontrado.", ENotificationType.Warning);
                }
                else if (editoras.Count() > 1)
                {
                    if (parametersEditora.Ordenar != 0)
                    {
                        switch (parametersEditora.Ordenar)
                        {
                            case EOrdenar.Crescente:
                                editoras = editoras.OrderBy(e => e.Nome);
                                break;

                            case EOrdenar.Decrescente:
                                editoras = editoras.OrderByDescending(e => e.Nome);
                                break;

                            case EOrdenar.Novos:
                                editoras = editoras.OrderByDescending(e => e.CriadoEm);
                                break;

                            case EOrdenar.Antigos:
                                editoras = editoras.OrderBy(e => e.CriadoEm);
                                break;
                        }
                    }
                }

                return await Task.FromResult(PagedList<Editora>.ToPagedList(editoras, parametersEditora.NumeroPagina, parametersEditora.ResultadosExibidos));
            });
        }

        private void DefinirUsuarioId(Editora editora)
        {
            editora.UsuarioId = user.GetUserId().ToString();
        }

        public override async Task<Editora> PostAsync(Editora editora)
        {
            DefinirUsuarioId(editora);
            return await base.PostAsync(editora);
        }

        public override async Task<Editora> PutAsync(Editora editora)
        {
            DefinirUsuarioId(editora);
            return await base.PutAsync(editora);
        }

        public bool ExisteId(Guid id)
        {
            return appDbContext.Editoras.Any(e => e.Id == id);
        }

        public bool ExisteNome(Editora editora)
        {
            IQueryable<Editora> query = appDbContext.Editoras.AsNoTracking()
                 .Where(o => o.Nome.ToLower() == editora.Nome.ToLower()
                       && o.UsuarioId == user.GetUserId().ToString()
                       && o.Status != EStatus.Excluido.ToString());

            if (editora.Id != Guid.Empty)
            {
                query = query.Where(e => e.Id != editora.Id);
            }

            return query.Any();
        }
    }
}