using Livros.Domain.Entities;
using Livros.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livros.Infrastructure.Configurations
{
    public class ImagemConfiguration : ConfigurationUploadIFormFileBase<Imagem>
    {
        public override void Configure(EntityTypeBuilder<Imagem> builder)
        {
            tableName = "Imagens";

            base.Configure(builder);

            builder.Property(i => i.TipoUpload)
                   .HasColumnName("TipoUpload")
                   .HasMaxLength(100)
                   .HasColumnType("varchar(100)")
                   .HasDefaultValue("Imagem");
        }
    }
}