using Extensions.Common.STExtension;
using Extensions.Common.STResultAPI;
using ST.INFRA.Common;
namespace ST_API.Models.IHolidayService
{
    #region HolidayParameter
    public class cHolidayYear : Pagination
    {
        public int? nID { get; set; }
        public int nHolidayDayID { get; set; }
        public int nYear { get; set; }
        public decimal nTotalHoliday { get; set; }
        public decimal nTotalActivity { get; set; }
        public int nOrder { get; set; }
        public List<cRequestYear>? lstnYear { get; set; }
        public List<cHolidayDetails>? lstHolidayDetails { get; set; }
    }
    public class cHolidayDetails : STGrid.Pagination
    {
        public int? nID { get; set; }
        public string? sID { get; set; }
        public DateTime dHolidayDate { get; set; }
        public string? sHolidayDate { get; set; }
        public string? sHolidayName { get; set; } = null!;
        public bool? IsActivity { get; set; }
        public string? sActivity { get; set; }
        public string? sDescription { get; set; }
    }

    public class cReqHolidayYear : STGrid.PaginationData
    {
        public string? sID { get; set; }
        public int? nID { get; set; }
        public int? nHolidayDayID { get; set; }
        public int? nYear { get; set; }
        public decimal? nTotalHoliday { get; set; }
        public decimal? nTotalActivity { get; set; }
        public int? nOrder { get; set; }
        public List<int>? lstnYear { get; set; }
        public List<cHolidayDetails>? lstHolidayDetails { get; set; }
    }
    #endregion

    #region HolidayTable
    public class cReturnHolidayList : Pagination
    {
        public List<cFilterHolidayYear>? lstHolidayYear { get; set; }
    }
    public class cFilterHolidayYear : Pagination
    {
        public int? nYear { get; set; }
        public decimal? nTotalHoliday { get; set; }
        public decimal? nTotalActivity { get; set; }
        public List<cHolidayData>? lstData { get; set; }

    }
    public class cFilterHolidayDetails : Pagination
    {
        public DateTime dHolidayDate { get; set; }
        public string? sHolidayDate
        {
            get
            {
                return dHolidayDate.ToStringFromDate();
            }
        }
        public string sHolidayName { get; set; }
        public bool? IsActivity { get; set; }
        public string? sActivity
        {
            get
            {
                return IsActivity.HasValue ? IsActivity.Value ? "กิจกรรมบริษัท" : "วันหยุดประจำปี" : "";
            }
        }
    }

    public class cFilterHolidayDayTable : Pagination
    {
        public int? nYear { get; set; } = null!;
        public int? nTotalHoliday { get; set; } = null!;
        public int? nTotalActivity { get; set; }
        public string? sNameActivity {get; set;}
        public List<cHolidayData>? lstData { get; set; }

    }

    #endregion

    public class cHolidayData : STGrid.PaginationData
    {
        public int nID { get; set; }
        public string? sID { get; set; } = null!;
        public string? sHolidayDate { get; set; } = null!;
        public string? sHolidayName { get; set; } = null!;
        public string? sActivity { get; set; } = null!;
        public bool? IsActivity { get; set; } = null!;
        public string? sDescription { get; set; } = null!;
        public DateTime? dHolidayDate { get; set; } = null!;
        public bool? IsDelete {get; set;}

    }
    #region DuplicateYearList
    public class cRequestYear
    {
        public int? value { get; set; } = null!;
        public string? label { get; set; } = null!;

    }
    public class cYearList
    {
        public int nOldYear { get; set; }
        public List<int> lstNewYear { get; set; }
        public decimal nTotalHoliday { get; set; }
        public decimal nTotalActivity { get; set; }
        public List<cHolidayYear>? lstData { get; set; }
        public List<cHolidayDetails>? lstHolidayDetails { get; set; }

    }
    #endregion

    #region Export PDF
    public class cExportOutput
    {
        public byte[]? objFile { get; set; }
        public string? sFileType { get; set; }
        public string? sFileName { get; set; }
        public List<cHolidayData>? lstHolidayItem { get; set; }
    }

    #endregion

       public class cRemoveTableHoliday : STGrid.PaginationData
    {
        public List<string>? lstID { get; set; } = new List<string>();
    }
}
