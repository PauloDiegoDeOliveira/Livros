using Livros.Domain.Enums;

namespace Livros.Application.Utilities.Paths
{
    public static class PathCreator
    {
        public static async Task<string> CreateIpPath(string ipPath)
        {
            string path = await DataFolders(ipPath, $@"\");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            await Task.CompletedTask;

            return path;
        }

        public static async Task<string> CreateAbsolutePath(string absolutePath)
        {
            return await DataFolders(absolutePath, $@"/");
        }

        public static async Task<string> CreateRelativePath(string absolutePath, string lastPart)
        {
            string[] splits = absolutePath.Split(new[] { lastPart }, 2, StringSplitOptions.RemoveEmptyEntries);
            await Task.CompletedTask;

            return splits[1];
        }

        public static async Task<string> DataFolders(string externalPath, string charType)
        {
            string path = externalPath
                          + charType
                          + DateInformations.GetSplitData(EData.Year)
                          + charType
                          + DateInformations.GetSplitData(EData.Month)
                          + charType
                          + DateInformations.GetSplitData(EData.Day)
                          + charType;

            await Task.CompletedTask;

            return path;
        }
    }
}