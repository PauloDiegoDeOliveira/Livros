using Livros.Domain.Entities;
using Livros.Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livros.Infrastructure.Configurations
{
    public class VolumeConfiguration : ConfigurationBase<Volume>
    {
        public override void Configure(EntityTypeBuilder<Volume> builder)
        {
            tableName = "Volumes";

            base.Configure(builder);

            builder.Property(v => v.Titulo)
                   .IsRequired()
                   .HasColumnName("Titulo")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)");

            builder.Property(v => v.Anotacao)
                   .IsRequired()
                   .HasColumnName("Anotacao")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)");
        }
    }
}