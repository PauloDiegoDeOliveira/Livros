using Livros.Domain.Enums;

namespace Livros.Application.Dtos.Usuario
{
    public class ViewUsuarioDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public EGenero Genero { get; set; }
        public string Email { get; set; }
        public bool Notificar { get; set; }
        public EStatus Status { get; set; }
    }
}