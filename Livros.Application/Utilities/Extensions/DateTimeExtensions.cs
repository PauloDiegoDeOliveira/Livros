using System.Globalization;

namespace Livros.Application.Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static string DataFormatoBrasileiro(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}