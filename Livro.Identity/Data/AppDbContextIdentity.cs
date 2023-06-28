using Livro.Identity.Entitys;
using Livros.Domain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Livro.Identity.Data
{
    public class AppDbContextIdentity : IdentityDbContext<Usuario>
    {
        public AppDbContextIdentity(DbContextOptions<AppDbContextIdentity> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContextIdentity).Assembly);
            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.Entity<Usuario>().HasQueryFilter(u => u.Status != EStatus.Excluido.ToString());
        }
    }
}