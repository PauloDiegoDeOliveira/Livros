using System.Text.RegularExpressions;

namespace Livros.Application.Utilities.Image
{
    public static class InterpretadorDataUriImagem
    {
        private static readonly Regex PadraoDataUri = new(@"^data\:(?<mimeType>image\/(?<imageType>png|tiff|jpg|gif|jpeg|svg+xml|svg));base64,(?<data>[A-Z0-9\+\/\=]+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private static readonly Dictionary<string, string> MapeamentoExtensoes = new()
        {
            { "svg+xml", "svg" }
        };

        public static string ObterExtensaoDoBase64(string base64String)
        {
            Match match = ObterMatchBase64(base64String);
            if (match == null)
            {
                return null;
            }

            return ConverterExtensao(match.Groups["imageType"].Value);
        }

        public static string ExtrairStringBase64(string base64String)
        {
            Match match = ObterMatchBase64(base64String);
            if (match == null)
            {
                return null;
            }

            return match.Groups["data"].Value;
        }

        private static Match ObterMatchBase64(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
            {
                return null;
            }

            Match match = PadraoDataUri.Match(base64String);
            if (!match.Success)
            {
                return null;
            }

            return match;
        }

        private static string ConverterExtensao(string extensao)
        {
            if (MapeamentoExtensoes.ContainsKey(extensao))
            {
                return MapeamentoExtensoes[extensao];
            }

            return extensao;
        }
    }
}