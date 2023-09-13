using Livros.Domain.Entities;
using Livros.Domain.Entities.Base;
using Livros.Domain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Livros.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<Usuario>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Editora> Editoras { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Idioma> Idiomas { get; set; }
        public DbSet<Imagem> Imagens { get; set; }
        public DbSet<Estante> Estantes { get; set; }
        public DbSet<Obra> Obras { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Volume> Volumes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(EntityBase).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(CreateFilterExpression(entityType.ClrType));
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        private static LambdaExpression CreateFilterExpression(Type type)
        {
            ParameterExpression lambdaParam = Expression.Parameter(type);
            BinaryExpression lambdaBody = Expression.NotEqual(
                Expression.Property(lambdaParam, nameof(EntityBase.Status)),
                Expression.Constant(EStatus.Excluido.ToString()));

            return Expression.Lambda(lambdaBody, lambdaParam);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (EntityEntry entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("AlteradoEm").CurrentValue = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}