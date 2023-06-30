using Livros.Domain.Enums;

namespace Livros.Application.Utilities.Paths
{
    public static class DateInformations
    {
        public static string GetSplitData(EData data)
        {
            DateTime datevalue = DateTime.Now;

            return data switch
            {
                EData.Year => datevalue.Year.ToString(),
                EData.Month => datevalue.Month.ToString(),
                EData.Day => datevalue.Day.ToString(),
                _ => null,
            };
        }
    }
}