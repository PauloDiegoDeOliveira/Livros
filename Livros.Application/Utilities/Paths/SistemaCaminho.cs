using Livros.Domain.Enums;
using Newtonsoft.Json;

namespace Livros.Application.Utilities.Paths
{
    public static class SistemaCaminho
    {
        public static Dictionary<string, Dictionary<string, string>> Caminhos { get; private set; }

        public static async Task CarregarURLsBase64()
        {
            string json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Base64URLsUpload.json"));
            Caminhos = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            await Task.CompletedTask;
        }

        public static async Task<bool> ValidarURLs(string nomeCaminho, EAmbiente ambiente)
        {
            string chave = nomeCaminho + ambiente.ToString();

            if (!Caminhos.ContainsKey(chave) ||
                !Caminhos[chave].ContainsKey("IP") ||
                !Caminhos[chave].ContainsKey("DNS") ||
                !Caminhos[chave].ContainsKey("SPLIT"))
            {
                return false;
            }

            await Task.CompletedTask;

            return true;
        }

        public static async Task<Dictionary<string, string>> ObterURLs(string nomeCaminho, EAmbiente ambiente)
        {
            string chave = nomeCaminho + ambiente.ToString();

            if (!Caminhos.ContainsKey(chave))
            {
                return null;
            }

            await Task.CompletedTask;

            return Caminhos[chave];
        }
    }
}