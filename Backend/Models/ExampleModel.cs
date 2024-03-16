using System.Text.Json;
using Extensions.Common.STResultAPI;

namespace Backend.Models.ExampleModel
{
    public class cExampleData : ResultAPI
    {
        public int? nFormID { get; set; }

        public string? sInputEmail { get; set; }

        public string? sInputPassword { get; set; }

        public string? sInputTH { get; set; }

        public string? sInputEN { get; set; }

        public decimal? nInputNumber { get; set; }

        public string? sInputNumberScientific { get; set; }

        public int? nSelectID { get; set; }

        public DateTime? dDay { get; set; }

        public int? nMonth { get; set; }

        public DateTime? dYearMonth { get; set; }

        public int? nYear { get; set; }

        public int? nQuarter { get; set; }

        public DateTime? dDayStart { get; set; }

        public DateTime? dDayEnd { get; set; }

        public int? nMonthStart { get; set; }

        public int? nMonthEnd { get; set; }

        public DateTime? dYearMonthStart { get; set; }

        public DateTime? dYearMonthEnd { get; set; }

        public int? nYearStart { get; set; }

        public int? nYearEnd { get; set; }
        public int? nQuarterStart { get; set; }
        public int? nQuarterEnd { get; set; }

        public DateTime? dDateTime { get; set; }

        public DateTime? dDateTimeStart { get; set; }

        public DateTime? dDateTimeEnd { get; set; }
    }
}
