using Livros.Domain.Enums;
using Newtonsoft.Json;

namespace Livros.Application.Utilities.Paths
{
    public static class PathSystem
    {
        public static Dictionary<string, Dictionary<string, string>> Paths { get; private set; }

        public static async Task GetBase64URLsUpload()
        {
            string json = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Base64URLsUpload.json");
            Paths = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            await Task.CompletedTask;
        }

        public static async Task<bool> ValidateURLs(string pathName, EAmbiente eAmbiente)
        {
            string key = pathName + eAmbiente.ToString();

            if (!Paths.ContainsKey(key) || !Paths[key].ContainsKey("IP") || !Paths[key].ContainsKey("DNS") || !Paths[key].ContainsKey("SPLIT"))
            {
                return false;
            }

            await Task.CompletedTask;

            return true;
        }

        public static async Task<Dictionary<string, string>> GetURLs(string pathName, EAmbiente eAmbiente)
        {
            string key = pathName + eAmbiente.ToString();

            if (!Paths.ContainsKey(key))
            {
                return null;
            }

            await Task.CompletedTask;

            return Paths[key];
        }
    }
}