using Livros.Domain.Entities;
using Livros.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livros.Infrastructure.Configurations
{
    public class EditoraConfiguration : ConfigurationBase<Editora>
    {
        public override void Configure(EntityTypeBuilder<Editora> builder)
        {
            tableName = "Editoras";

            base.Configure(builder);

            builder.Property(e => e.Nome)
                   .IsRequired()
                   .HasColumnName("Nome")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)");
        }
    }
}