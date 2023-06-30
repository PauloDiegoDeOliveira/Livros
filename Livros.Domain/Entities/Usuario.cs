using Microsoft.AspNetCore.Identity;

namespace Livros.Domain.Entities
{
    public class Usuario : IdentityUser
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime Nascimento { get; set; }
        public string Genero { get; set; }
        public bool Notificar { get; set; }
        public long VersaoToken { get; set; }
        public string Status { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public DateTimeOffset? UltimoAcesso { get; set; }

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