namespace Livros.Application.Dtos.Usuario.Autenticacao
{
    public class ViewLoginDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool Notificar { get; set; }
    }
}