namespace Livros.Application.Utilities.ConversorHora
{
    public class HashedPassword
    {
        public string Password { get; set; }

        public void ChangePassword(string password)
        {
            Password = password;
        }
    }
}