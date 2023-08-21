using Livros.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livros.Infrastructure.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable(name: "Usuarios");

            builder.Property(p => p.Nome)
                   .IsRequired()
                   .HasColumnName("Nome")
                   .HasMaxLength(150)
                   .HasColumnType("varchar(150)");

            //builder.Property(p => p.Sobrenome)
            //       .HasColumnName("Sobrenome")
            //       .HasMaxLength(150)
            //       .HasColumnType("varchar(150)");

            builder.Property(p => p.Genero)
                   .HasMaxLength(50)
                   .HasColumnName("Genero")
                   .HasColumnType("varchar(50)");

            builder.Property(p => p.Status)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("Status")
                   .HasColumnType("varchar(50)")
                   .HasDefaultValue("Ativo");

            builder.Property(u => u.UserName)
                   .HasMaxLength(128)
                   .HasColumnType("varchar(128)");

            builder.Property(u => u.NormalizedUserName)
                   .HasMaxLength(128)
                   .HasColumnType("varchar(128)");

            builder.Property(u => u.Email)
                   .HasMaxLength(128)
                   .HasColumnType("varchar(128)");

            builder.Property(u => u.NormalizedEmail)
                   .HasMaxLength(128)
                   .HasColumnType("varchar(128)");

            builder.Property(u => u.PasswordHash)
                   .HasMaxLength(1000)
                   .HasColumnType("varchar(1000)");
        }
    }
}