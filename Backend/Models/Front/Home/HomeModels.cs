using Extensions.Common.STResultAPI;
using ST.INFRA.Common;
using ST.INFRA;
using static Extensions.Systems.AllClass;
namespace ST_API.Models.IHomeService
{
    public class cResultDataHome : Pagination
    {
        public List<cMeetingRoomHome>? lstMeeting { get; set; }
        public List<cMeetingRoomHome>? lstMeetingNew { get; set; }
        public List<cDataHolidayHome>? lstDataHoliday { get; set; }
        public List<cHolidayHome>? lstDataHolidayTotal { get; set; }
        public List<cDataHolidayHome>? lstDataActivity { get; set; }
        public List<cHolidayHome>? lstDataActivityTotal { get; set; }
        public string? sYear { get; set; }
    }

    public class cMeetingRoomHome : ResultAPI
    {
        public List<cMeetingDetialHome>? lstMeetingDetail { get; set; }
        public List<cMeetingDetialHome>? lstMeetingDetailNew { get; set; }
        public List<cHolidayHome>? lstDataHoliday { get; set; }
        public string? sDayTH { get; set; }
        public string? sDayEN { get; set; }
    }
    public class cDataHolidayHome : ResultAPI
    {
        public List<cHolidayHome>? lstDataActivity { get; set; }
        public List<cHolidayHome>? lstDataHoliday { get; set; }
        public string? sDayTH { get; set; }
        public string? sDayEN { get; set; }
    }


    public class cMeetingDetialHome
    {
        public int nMeetingID { get; set; }
        public string? stitle { get; set; }
        public string? sRoom { get; set; }
        public string? sRoomcode { get; set; }
        public string? sDay { get; set; }
        public string? sMonth { get; set; }
        public string? sTimeStart { get; set; }
        public string? sTimeEnd { get; set; }
    }
    public class cHolidayHome
    {
        public int nHolidayID { get; set; }
        public int nYear { get; set; }
        public string? stitle { get; set; }
        public string? sDay { get; set; }
        public string? sMonth { get; set; }

    }

    #region
    /// <summary>
    /// 
    /// </summary>
    public class ResultBanner : ResultAPI
    {
        /// <summary>
        /// 
        /// </summary>
        public List<BannerData>? lstBanner { get; set; }
    }
    /// <summary>
    /// </summary>
    public class BannerData
    {
        /// <summary>
        /// </summary>
        public string? sUrl { get; set; }
        /// <summary>
        /// </summary>
        public ItemFileData? fFileBaner { get; set; }
        /// <summary>
        /// </summary>
        public string? sTextInfo { get; set; }
        /// <summary>
        /// </summary>
        public string? sBannerName { get; set; }
        /// <summary>
        /// </summary>
        public string? sColor { get; set; }
        /// <summary>
        /// </summary>
        public string? sID { get; set; }
        /// <summary>
        /// </summary>
        public int nOrder { get; set; }
        /// <summary>
        /// </summary>
        public int nOrderPin { get; set; }
        /// <summary>
        /// </summary>
        public decimal? nSize { get; set; }
        /// <summary>
        /// </summary>
        public DateTime dLastUpdate { get; set; }
    }
    #endregion
    public class ResultInit : Backend.Models.ResultAPI
    {
        public string sPosition { get; set; } = string.Empty;
        public string HeadDate { get; set; } = string.Empty;
    }
    public class ResultOpt : Backend.Models.ResultAPI
    {
        public List<Backend.Models.Option> lstProject { get; set; } = new();
        public List<Backend.Models.Option> lstEmployee { get; set; } = new();
        public List<Backend.Models.Option> lstLeaveType { get; set; } = new();
    }
    public class Option
    {
        public string? label { get; set; }
        public string? value { get; set; }
    }

    public class ParamViewManager : STGrid.PaginationData
    {
        public string? sDateStart { get; set; } = string.Empty;
        public string? sDateEnd { get; set; } = string.Empty;
        public string? sProject { get; set; } = string.Empty;
        public string? sEmployee { get; set; } = string.Empty;
        public string? sTypeLeave { get; set; } = string.Empty;
        public string? sTypeWFH { get; set; } = string.Empty;
    }

    public class ResultTaskManager : Pagination
    {
        public List<Backend.Models.TaskItem> arrData { get; set; } = new();
        public List<Backend.Models.TaskItem> arrFullData { get; set; } = new();
    }

    public class ResultOTManager : Pagination
    {
        public List<OTItem> arrData { get; set; } = new();
        public List<OTItem> arrFullData { get; set; } = new();
    }
    public class OTItem
    {
        public string sID { get; set; } = string.Empty;
        public string sEncryptID { get; set; } = string.Empty;
        public string? sTypeOT { get; set; }
        public string? sEmployeeName { get; set; }
        public string? sDetail { get; set; }
        public string? sOTDate { get; set; }
        public decimal? nPlan { get; set; }
        public decimal? nActual { get; set; }
        public string? sApproveBy { get; set; }
        public string? sStatus { get; set; }
        public int? nStatusID { get; set; }
    }
    public class ResultWFHManager : Pagination
    {
        public List<WFHItem> arrData { get; set; } = new();
        public List<WFHItem> arrFullData { get; set; } = new();
    }
    public class WFHItem
    {
        public string sID { get; set; } = string.Empty;
        public string sEncryptID { get; set; } = string.Empty;
        public string? sType { get; set; }
        public string? sEmployeeName { get; set; }
        public string? sRequestDate { get; set; }
        public string? sWFHDate { get; set; }
        public string? sApproveBy { get; set; }
        public string? sStatus { get; set; }
        public int? nFlowProcessID { get; set; }
        public string? sDescription { get; set; }
    }

    public class ResultLeaveManager : Pagination
    {
        public List<LeaveItem> arrData { get; set; } = new();
        public List<LeaveItem> arrFullData { get; set; } = new();
    }
    public class LeaveItem
    {
        public string sID { get; set; } = string.Empty;
        public string sEncryptID { get; set; } = string.Empty;
        public string? sCreateDate { get; set; }
        public string? sTypeLeave { get; set; }
        public string? sEmployeeName { get; set; }
        public string? sDetail { get; set; }
        public string? sStartLeave { get; set; }
        public string? sEndLeave { get; set; }
        public string? sLeaveUse { get; set; }
        public int? nStatus { get; set; }
        public string? sStatus { get; set; }
        public bool isEnableEdit { get; set; }
        public bool isEnableApprove { get; set; }
    }

}