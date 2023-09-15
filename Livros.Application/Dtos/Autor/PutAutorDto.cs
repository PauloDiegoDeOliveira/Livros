namespace Livros.Application.Dtos.Autor
{
    public class PutAutorDto : PostAutorDto
    {
        public Guid Id { get; set; }
        public string UsuarioId { get; set; }
    }
}