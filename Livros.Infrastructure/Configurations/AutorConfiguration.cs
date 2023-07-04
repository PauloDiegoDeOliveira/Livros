using Livros.Domain.Entities;
using Livros.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livros.Infrastructure.Configurations
{
    public class AutorConfiguration : ConfigurationBase<Autor>
    {
        public override void Configure(EntityTypeBuilder<Autor> builder)
        {
            tableName = "Autores";

            base.Configure(builder);

            builder.Property(a => a.Nome)
                   .IsRequired()
                   .HasColumnName("Nome")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)");
        }
    }
}