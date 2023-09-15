using Livros.Domain.Enums;

namespace Livros.Application.Dtos.Idioma
{
    public class ViewIdiomaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Alias { get; set; }
        public EStatus Status { get; set; }
    }
}