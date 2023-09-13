using Livros.Domain.Entities;
using Livros.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livros.Infrastructure.Configurations
{
    public class EstanteConfiguration : ConfigurationBase<Estante>
    {
        public override void Configure(EntityTypeBuilder<Estante> builder)
        {
            tableName = "Estantes";

            base.Configure(builder);

            builder.Property(l => l.Nome)
                   .IsRequired()
                   .HasColumnName("Nome")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)");
        }
    }
}