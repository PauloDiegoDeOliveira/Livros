namespace Livros.Application.Utilities.Paths
{
    public class URLsUpload
    {
        public static readonly Dictionary<string, EstruturaUrl> Urls = new()
        {
            { "UsuarioDesenvolvimento", new EstruturaUrl { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Usuario\\Imagens", DNS = "https://Livros/Arquivos/Usuario/Imagens", SPLIT = "Livros/"} },
            { "UsuarioHomologacao", new EstruturaUrl { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Usuario\\Imagens", DNS = "https://Livros/Usuario/Imagens", SPLIT = "Livros/"} },
            { "UsuarioProducao", new EstruturaUrl { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Usuario\\Imagens", DNS = "https://Livros/Usuario/Imagens", SPLIT = "Livros/"} },

            { "ObraDesenvolvimento", new EstruturaUrl { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Obra\\Imagens", DNS = "https://Livros/Arquivos/Obra/Imagens", SPLIT = "Livros/"} },
            { "ObraHomologacao", new EstruturaUrl { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Obra\\Imagens", DNS = "https://Livros/Arquivos/Obra/Imagens", SPLIT = "Livros/"} },
            { "ObraProducao", new EstruturaUrl { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Obra\\Imagens", DNS = "https://Livros/Arquivos/Obra/Imagens", SPLIT = "Livros/"} },
        };
    }
}