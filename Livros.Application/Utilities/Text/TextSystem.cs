using Newtonsoft.Json;

namespace Livros.Application.Utilities.Text
{
    public static class TextSystem
    {
        private static List<TextObject> Texts = new();

        public static void GetTextJson()
        {
            string json = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Returns.json");
            Texts = JsonConvert.DeserializeObject<List<TextObject>>(json);
        }

        public static async Task<string> GetText(int id)
        {
            TextObject result = Texts.Find(x => x.Id == id);
            if (result is null)
            {
                return "Id de retorno não encontrado.";
            }

            await Task.CompletedTask;

            return result.Text;
        }
    }
}