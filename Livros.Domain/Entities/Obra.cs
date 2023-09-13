using Livros.Domain.Entities.Base;

namespace Livros.Domain.Entities
{
    public class Obra : EntityBase
    {
        public string UsuarioId { get; set; }
        public Guid IdiomaId { get; set; }
        public Guid ImagemId { get; set; }
        public string Titulo { get; set; }
        public string Anotacao { get; set; }
        public int AvaliacaoTotal { get; set; }
        public decimal PrecoTotal { get; set; }
        public int PaginaTotal { get; set; }
        public bool Concluido { get; set; }
        public bool VolumeUnico { get; set; }
        public string Tipo { get; set; }

        public Usuario Usuario { get; set; }
        public Idioma Idioma { get; set; }
        public Imagem Imagem { get; set; }
        public List<Editora> Editoras { get; set; }
        public List<Lista> Listas { get; set; }
        public List<Volume> Volumes { get; set; }
        public List<Autor> Autores { get; set; }
        public List<Genero> Generos { get; set; }
    }
}