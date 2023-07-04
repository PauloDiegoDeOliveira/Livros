using Livros.Domain.Entities;
using Livros.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livros.Infrastructure.Configurations
{
    public class GeneroConfiguration : ConfigurationBase<Genero>
    {
        public override void Configure(EntityTypeBuilder<Genero> builder)
        {
            tableName = "Generos";

            base.Configure(builder);

            builder.Property(g => g.Nome)
                   .IsRequired()
                   .HasColumnName("Nome")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)");
        }
    }
}