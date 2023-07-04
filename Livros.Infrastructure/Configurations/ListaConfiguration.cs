using Livros.Domain.Entities;
using Livros.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livros.Infrastructure.Configurations
{
    public class ListaConfiguration : ConfigurationBase<Lista>
    {
        public override void Configure(EntityTypeBuilder<Lista> builder)
        {
            tableName = "Listas";

            base.Configure(builder);

            builder.Property(l => l.Nome)
                   .IsRequired()
                   .HasColumnName("Nome")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)");

            builder.Property(l => l.Publico)
                   .IsRequired()
                   .HasColumnName("Publico")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)")
                   .HasDefaultValue("Publico");
        }
    }
}