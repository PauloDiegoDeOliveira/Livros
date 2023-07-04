using Livros.Domain.Entities;
using Livros.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livros.Infrastructure.Configurations
{
    public class IdiomaConfiguration : ConfigurationBase<Idioma>
    {
        public override void Configure(EntityTypeBuilder<Idioma> builder)
        {
            tableName = "Idiomas";

            base.Configure(builder);

            builder.Property(i => i.Nome)
                   .IsRequired()
                   .HasColumnName("Nome")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)");

            builder.Property(i => i.Alias)
                   .IsRequired()
                   .HasColumnName("Alias")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)");
        }
    }
}