using Livros.Domain.Enums;

namespace Livros.Application.Utilities.Paths
{
    public static class PathCreator
    {
        public static PathResponse CreateIpFolderPath(string ipPath)
        {
            string folderPath = GetDateBasedFolderPath(ipPath, $@"\");

            if (Path.IsPathRooted(folderPath))
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                return new PathResponse(true, folderPath);
            }

            PathResponse failResponse = new();
            failResponse.AdicionarErro("Erro ao criar as pastas de upload de imagem.");

            return failResponse;
        }

        public static string CreateAbsoluteFolderPath(string absolutePath)
        {
            return GetDateBasedFolderPath(absolutePath, $@"/");
        }

        public static PathResponse CreateRelativeFolderPath(string absolutePath, string lastPart)
        {
            string[] splits = absolutePath.Split(new[] { lastPart }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (splits.Length < 2)
            {
                PathResponse failResponse = new();
                failResponse.AdicionarErro("Erro ao criar o caminho relativo.");

                return failResponse;
            }

            return new PathResponse(true, splits[1]);
        }

        public static string GetDateBasedFolderPath(string externalPath, string pathSeparator)
        {
            return $"{externalPath}{pathSeparator}{DateInformations.GetSplitData(EData.Year)}{pathSeparator}{DateInformations.GetSplitData(EData.Month)}{pathSeparator}{DateInformations.GetSplitData(EData.Day)}{pathSeparator}";
        }
    }
}