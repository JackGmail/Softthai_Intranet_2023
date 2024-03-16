using Backend.Models;
using Extensions.Common.STResultAPI;
using ST.INFRA.Common;

namespace Backend.Models
{
    public class cInitAllowanceData : Extensions.Common.STResultAPI.ResultAPI
    {
        public cSelectOption[] lstProject { get; set; }
        public cSelectOption[] lstAllowanceType { get; set; }
        public cSelectOption[] lstVehicle { get; set; }
        public cSelectOption[] lstStartType { get; set; }
        public string sUserName { get; set; }
        public string sPosition { get; set; }
        public cUserHR objHR { get; set; }
    }
    public class cUserHR
    {
        public string sID { get; set; }
        public int nNo { get; set; }
        public string sName { get; set; }
        public string sPosition { get; set; }
        public string sAction { get; set; }
    }
    public class cAllowanceData : Extensions.Common.STResultAPI.ResultAPI
    {
        public string? sID { get; set; }
        public string? sProjectID { get; set; }
        public string? sDesc { get; set; }
        public int nAllowanceTypeID { get; set; }
        public DateTime? dStartDate_StartTime { get; set; }
        public DateTime? dStartDate_EndTime { get; set; }
        public DateTime? dEndDate_StartTime { get; set; }
        public DateTime? dEndDate_EndTime { get; set; }
        public decimal? nSumAllowanceDay { get; set; }
        public decimal? nSumAllowanceMoney { get; set; }
        public string? sComment { get; set; }
        public int nStatusID { get; set; }
        public bool? IsRequester { get; set; }
        public bool? IsApprover { get; set; }
        public string? sUserName { get; set; }
        public string? sPosition { get; set; }
        public List<cHistoryLog>? lstHistoryLog { get; set; }
    }
    public class cHistoryLog
    {
        public string sName { get; set; }
        public string? sPosition { get; set; }
        public string sComment { get; set; }
        public string sStatus { get; set; }
        public string sActionMode { get; set; }
        public string sActionDate { get; set; }
        public string? sPathImg { get; set; }
    }
    public class cReqDataAllowance
    {
        public string? sID { get; set; }
    }
    public class cReqTableWelfare : STGrid.PaginationData
    {
        public List<string>? lstFormID { get; set; }
        public List<string>? lstRequesterID { get; set; }
        public List<string>? lstStatusID { get; set; }
        public DateTime? dRequestStart { get; set; }
        public DateTime? dRequestEnd { get; set; }
    }
    public class cResultTableWelfare : Pagination
    {
        public List<cDataTableWelfare>? lstData { get; set; }
        public int? nUserID { get; set; }
        public bool? IsApprover { get; set; }
        public List<cSelectOption>? lstForm { get; set; }
        public List<cSelectOption>? lstRequester { get; set; }
        public List<cSelectOption>? lstStatus { get; set; }
    }
    public class cDataTableWelfare
    {
        public string? sID { get; set; }
        public int? No { get; set; }
        public DateTime? dRequestDate { get; set; }
        public string? sRequestDate { get; set; }
        public string? sForm { get; set; }
        public string? sAmount { get; set; }
        public decimal? nRequester { get; set; }
        public string? sRequester { get; set; }
        public string? sWaitingBy { get; set; }
        public string? sStatus { get; set; }
        public int? nStatusID { get; set; }
        public DateTime? dUpdate { get; set; }
        public bool? IsRequester { get; set; }
        public bool? IsApprover { get; set; }
        public int? nRequestTypeID { get; set; }
        public string? sComment { get; set; }
    }
    public class cReqApprove
    {
        public string? sID { get; set; }
        public int? nStatusID { get; set; }
        public string? sComment { get; set; }
        public bool? IsActionFromLine { get; set; } = false;
        public string? sGUID { get; set; }
    }
}
