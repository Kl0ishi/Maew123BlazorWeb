using System.Globalization;

namespace Maew123.Web.Utilities
{
    public class SaleFilterHelper
    {
        public SaleFilterResultDto Model { get; set; }

        // Get specific sales of the whole year
        public List<CartsDto> GetSalesOfYear(int year)
        {
            return Model.Carts.Where(s => s.OrderDate.Year == year).ToList();
        }

        // Get specific sales of the whole year
        public List<CartsDto> GetSalesOfAllYears()
        {
            return Model.Carts.ToList();
        }

        // Get specific sales of the whole month
        public List<CartsDto> GetSalesOfWholeMonth(int year, int month)
        {
            return Model.Carts.Where(s => s.OrderDate.Year == year && s.OrderDate.Month == month).ToList();
        }

        // Get specific sales of the whole week
        public List<CartsDto> GetSalesOfWholeWeek(int year, int month, int week)
        {
            // Calculate the start and end dates of the week
            DateTime startDate = FirstDateOfWeekISO8601(year, month, week);
            DateTime endDate = startDate.AddDays(6);

            // Filter sales within the week and for the specified year and month
            return Model.Carts.Where(s => s.OrderDate.Year == year &&
                                           s.OrderDate.Month == month &&
                                           s.OrderDate >= startDate &&
                                           s.OrderDate <= endDate)
                              .ToList();
        }

        // Helper method to get the start date of a week based on the year and week number
        private DateTime FirstDateOfWeekISO8601(int year, int month, int week)
        {
            DateTime jan1 = new DateTime(year, month, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = week;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
    }
}
