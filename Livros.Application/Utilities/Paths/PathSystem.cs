using Livros.Domain.Enums;

namespace Livros.Application.Utilities.Paths
{
    public static class PathSystem
    {
        public static Paths Paths { get; set; }

        public static bool ValidateURLs(string pathName, EAmbiente ambiente)
        {
            string key = $"{pathName}{ambiente}";
            if (!Paths.Urls.TryGetValue(key, out var url) || string.IsNullOrEmpty(url?.IP) || string.IsNullOrEmpty(url?.DNS) || string.IsNullOrEmpty(url?.SPLIT))
            {
                return false;
            }

            return true;
        }

        public static Url GetURLs(string pathName, EAmbiente ambiente)
        {
            string key = $"{pathName}{ambiente}";
            Paths.Urls.TryGetValue(key, out Url url);

            return url;
        }
    }
}