using Livros.Domain.Entities;
using Livros.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livros.Infrastructure.Configurations
{
    public class ObraConfiguration : ConfigurationBase<Obra>
    {
        public override void Configure(EntityTypeBuilder<Obra> builder)
        {
            tableName = "Obras";

            base.Configure(builder);

            builder.Property(o => o.Titulo)
                   .IsRequired()
                   .HasColumnName("Titulo")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)");

            builder.Property(o => o.Anotacao)
                   .IsRequired()
                   .HasColumnName("Anotacao")
                   .HasMaxLength(5000)
                   .HasColumnType("varchar(5000)");

            builder.Property(o => o.Tipo)
                   .IsRequired()
                   .HasColumnName("Tipo")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)")
                   .HasDefaultValue("Obra");
        }
    }
}