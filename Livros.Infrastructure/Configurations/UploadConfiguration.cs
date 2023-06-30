using Livros.Domain.Entities;
using Livros.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livros.Infrastructure.Configurations
{
    public class UploadConfiguration : ConfigurationUploadIFormFileBase<Imagem>
    {
        public override void Configure(EntityTypeBuilder<Imagem> builder)
        {
            tableName = "Imagens";

            base.Configure(builder);
        }
    }
}