using Extensions.Common.STResultAPI;
using ST.INFRA.Common;

namespace Backend.Models
{
    public class cInitData : Extensions.Common.STResultAPI.ResultAPI
    {
        public cSelectOption[] lstProject { get; set; }
        public cSelectOption[] lstProcess { get; set; }
        public cSelectOption[] lstApproveByCreate { get; set; }
        public cSelectOption[] lstApproveBy { get; set; }
        public cSelectOption[] lstCondition { get; set; }
        public cSelectOption[] lstProjectTask { get; set; }
        public List<string>? lstConditionList { get; set; }
        public string? sRequester { get; set; }
    }
    public class cOTData : Extensions.Common.STResultAPI.ResultAPI
    {
        public string? sID { get; set; }
        public string? sProjectID { get; set; }
        public string? sProcessID { get; set; }
        public string? sApproveBy { get; set; }
        public string? sRequester { get; set; }
        public string? sTopic { get; set; }
        public DateTime? dPlanDateTime { get; set; }
        public decimal? nEstimateHour { get; set; }
        public List<string>? lstCondition { get; set; }
        public string? sOtherRemark { get; set; }
        public List<cTaskDetail>? lstTask { get; set; }
        public cSelectOption[]? lstProjectTask { get; set; }
        public int? nStatusID { get; set; }
        public string? sComment { get; set; }
        public string? sApprovedBy { get; set; }
        public string? sApprovedDate { get; set; }
        public DateTime? dActionStartResult { get; set; }
        public DateTime? dActionEndResult { get; set; }
        public bool? IsClosedOT { get; set; }
        public bool? IsApprover { get; set; }
        public bool? IsRequester { get; set; }
        public bool? IsActionFromLine { get; set; } = false;
        public string? sGUID { get; set; }
        public List<cDataExportOT>? lstData { get; set; }
        public List<cHistoryLog>? lstHistoryLog { get; set; }
    }
    public class cTaskDetail
    {
        public int? nID { get; set; }
        public string? sTaskID { get; set; }
        public string? sTaskDesc { get; set; }
        public bool? IsTaskCompleted { get; set; }
        public decimal? nOTHourResult { get; set; }
        public string? sReasonResult { get; set; }

    }
    public class cReqDataOT
    {
        public string? sID { get; set; }
        public string? sComment { get; set; }
        public string? sProjectID { get; set; }
        public int? nStatusID { get; set; }
        public bool? IsActionFromLine { get; set; } = false;
        public string? sGUID { get; set; }
    }
    public class cReqTableOT : STGrid.PaginationData
    {
        public List<string>? lstProjectID { get; set; }
        public DateTime? dRequestStart { get; set; }
        public DateTime? dRequestEnd { get; set; }
        public List<string>? lstRequesterID { get; set; }
        public List<string>? lstStatusID { get; set; }
    }
    public class cResultTableOT : Pagination
    {
        public List<cDataTableOT>? lstData { get; set; }
        public List<cSelectOption>? lstStatus { get; set; }
        public List<cSelectOption>? lstProject { get; set; }
        public List<cSelectOption>? lstRequester { get; set; }
        public List<string>? lstSelectUser { get; set; }
    }
    public class cDataTableOT
    {
        public int? nRequestOTID { get; set; }
        public string? sID { get; set; }
        public int? No { get; set; }
        public DateTime? dCreate { get; set; }
        public DateTime? dPlanDateTime { get; set; }
        public int? nRequester { get; set; }
        public string? sRequester { get; set; }
        public string? sCreate { get; set; }
        public string? sPlanDate { get; set; }
        public int? nProjectID { get; set; }
        public string? sProject { get; set; }
        public string? sTopic { get; set; }
        public string? sType { get; set; }
        public decimal? nHour { get; set; }
        public string? sHour { get; set; }
        public string? sApproveBy { get; set; }
        public int? nStatusID { get; set; }
        public string? sStatus { get; set; }
        public DateTime? dUpdate { get; set; }
        public cSelectOption[] lstCondition { get; set; }
        public bool? IsRequester { get; set; }
        public bool? IsApprover { get; set; }
        public string? sDescription { get; set; }
        public string? sReason { get; set; }
        public decimal? nHourHoliday { get; set; }
        public decimal? nHourNormal { get; set; }
        public string? sActionTime { get; set; }
        public DateTime? dStartActionDateTime { get; set; }
        public DateTime? dEndActionDateTime { get; set; }
        public List<cDataTableOT>? lstReason { get; set; }
        public List<cDataTableOT>? lstTask { get; set; }
        public string? sActionStartTime { get; set; }
        public string? sActionEndTime { get; set; }
        public bool? IsHoliday { get; set; }

    }

    public class cExportExcelOT : Pagination
    {
        public byte[] objFile { get; set; }
        public string sFileType { get; set; }
        public string? sFileName { get; set; }
    }

    public class cDataExportOT
    {
        public string? sEmployeeID { get; set; }
        public string? sRequester { get; set; }
        public decimal? nSumHourHoliday { get; set; }
        public decimal? nSumHourNormal { get; set; }
        public List<cDataTableOT>? lstDataDetail { get; set; }

    }

}
