using Livros.Domain.Enums;

namespace Livros.Application.Utilities.Paths
{
    public static class ExtrairData
    {
        public static string ExtrairComponente(EData data)
        {
            DateTime dataAtual = DateTime.Now;

            return data switch
            {
                EData.Year => dataAtual.Year.ToString(),
                EData.Month => dataAtual.Month.ToString("00"),
                EData.Day => dataAtual.Day.ToString("00"),
                _ => null,
            };
        }
    }
}