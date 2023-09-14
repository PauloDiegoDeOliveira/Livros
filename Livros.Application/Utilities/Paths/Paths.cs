﻿namespace Livros.Application.Utilities.Paths
{
    public class Paths
    {
        public static readonly Dictionary<string, Url> Urls = new()
        {
            { "UsuarioDesenvolvimento", new Url { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Usuario\\Imagens", DNS = "https://Livros/Arquivos/Usuario/Imagens", SPLIT = "Livros/"} },
            { "UsuarioHomologacao", new Url { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Usuario\\Imagens", DNS = "https://Livros/Usuario/Imagens", SPLIT = "Livros/"} },
            { "UsuarioProducao", new Url { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Usuario\\Imagens", DNS = "https://Livros/Usuario/Imagens", SPLIT = "Livros/"} },

            { "ObraDesenvolvimento", new Url { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Obra\\Imagens", DNS = "https://Livros/Arquivos/Obra/Imagens", SPLIT = "Livros/"} },
            { "ObraHomologacao", new Url { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Obra\\Imagens", DNS = "https://Livros/Arquivos/Obra/Imagens", SPLIT = "Livros/"} },
            { "ObraProducao", new Url { IP = "\\\\192.168.103.20\\Sites\\Sistemas\\Livros\\Arquivos\\Obra\\Imagens", DNS = "https://Livros/Arquivos/Obra/Imagens", SPLIT = "Livros/"} },
        };
    }
}