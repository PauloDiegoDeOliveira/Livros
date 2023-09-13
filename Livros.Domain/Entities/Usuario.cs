using Microsoft.AspNetCore.Identity;

namespace Livros.Domain.Entities
{
    public class Usuario : IdentityUser
    {
        public string Nome { get; set; }

        //public string Sobrenome { get; set; }

        public DateTime? DataNascimento { get; set; }
        public string Genero { get; set; }
        public bool Notificar { get; set; }
        public long VersaoToken { get; set; }
        public string Status { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public DateTimeOffset? UltimoAcesso { get; set; }

        public IList<Autor> Autores { get; set; }
        public IList<Editora> Editoras { get; set; }
        public IList<Genero> Generos { get; set; }
        public IList<Estante> Listas { get; set; }
        public IList<Obra> Obras { get; set; }

        public void AlteraAlteradoEm()
        {
            AlteradoEm = DateTime.Now;
        }

        public void AlteraUltimoAcesso()
        {
            UltimoAcesso = DateTimeOffset.Now;
        }
    }
}