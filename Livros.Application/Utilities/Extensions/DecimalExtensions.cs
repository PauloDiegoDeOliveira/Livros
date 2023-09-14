using System.Globalization;

namespace Livros.Application.Utilities.Extensions
{
    public static class DecimalExtensions
    {
        public static string MoedaBrasileira(this decimal value)
        {
            return value.ToString("C2", new CultureInfo("pt-BR"));
        }
    }
}