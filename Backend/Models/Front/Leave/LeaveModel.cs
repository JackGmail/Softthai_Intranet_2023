using System.Text.Json;
using Extensions.Common.STResultAPI;
using ST.INFRA.Common;
using static Extensions.Systems.AllClass;
using ST.INFRA;
using Extensions.Common.STExtension;

namespace ST_API.Models
{
    public class cLeaveInitDataRequest : ResultAPI
    {
        public int nLeave { get; set; }
        public int nLate { get; set; }
        public int nWFH { get; set; }
        public int nLeaveWithCert { get; set; }
        public int nSickLeave { get; set; }
        public int nBusinessLeave { get; set; }
        public int nHolidayLeave { get; set; }
        public int nMaternityLeave { get; set; }
        public int nOrdinationLeave { get; set; }
        public int nBirthLeave { get; set; }
        public int nUnpaidLeave { get; set; }
        public int nCompensationWork { get; set; }
        public string? sImage1 { get; set; }
        public string? sImage2 { get; set; }
        public string? sImage3 { get; set; }
        public string? sImage4 { get; set; }
        public string? sImage5 { get; set; }
        public string? sImage6 { get; set; }
        public string? sImage7 { get; set; }
        public string? sImage8 { get; set; }
        public List<cCalendarRequest>? lstData { get; set; }
    }

    public class cReturnImportExcelRequest : ResultAPI
    {
        public List<cObjectImportExcelRequest>? lstData { get; set; }
    }

    public class cLeaveTableRequest : STGrid.Option
    {
        public int? nID { get; set; }
    }

    public class cEditLeaveRequest
    {
        public List<cLeaveSummaryRequest> lstLeaveDate { get; set; }
        // public List<cLeaveSummary> lstSummaryLeave { get; set; }
    }

    public class cLeaveRightsRequest : STGrid.PaginationData
    {
        public List<string>? lstID { get; set; }
        public string? sNameTH { get; set; }
        public int? nTypeID { get; set; }
        public int? nStatusID { get; set; }
        public string? sEmpTypeID { get; set; }
    }

    public class cResultTableLeaveRequest : Pagination
    {
        public List<cLeaveDetailRequest>? lstData { get; set; }
        public List<cSelectOptionleave>? lstLeaveType { get; set; }
        public List<cLeaveSummaryRequest>? lstSummaryLeave { get; set; }
        public cSelectRequest[]? lstSelectEmpType { get; set; }
        public cSelectRequest[]? lstPosition { get; set; }
        public List<cLeaveSummaryRequest>? sID { get; set; }
        public List<cEditLeaveDateleaveRequest>? arrLeave { get; set; }
        public int? nSick { get; set; }
        public int? nBusiness { get; set; }
        public int? nHoliday { get; set; }
        public int? nBirthDate { get; set; }
        public int? nMaternity { get; set; }
        public int? nNoPaid { get; set; }
        public int? nCompensationWork { get; set; }
        // public string sID { get; set; }
    }

    public class cTabelWorkListRequest : Pagination
    {
        public TableDataWorkListRequest[] arrData { get; set; } = { };
    }

    public class cReqTableWorkListRequest : STGrid.Option
    {
        public string[] arrTypeID { get; set; } = { };
        public string[] arrStatusID { get; set; } = { };
        public int nTypeFilterDate { get; set; } = 0;
        public double? dStartFilterDate { get; set; }
        public double? dEndFilterDate { get; set; }
        public bool isApproveListPage { get; set; } = false;
        public string[] arrEmpID { get; set; } = { };
    }

    public class ApproverDataRequest
    {
        public string sID { get; set; }
        public int nNo { get; set; }
        public string sName { get; set; }
        public string sPosition { get; set; }
        public string sStatus { get; set; }
        public string sEmployeeID { get; set; }
        public bool isHr { get; set; }
    }

    public class TableDataWorkListRequest
    {
        public string sID { get; set; }
        public string sCreateDate { get; set; } = "";
        public DateTime dCreateDate { get; set; }
        public DateTime? dUpdate { get; set; }
        public string sFullName { get; set; } = "";
        public string sType { get; set; } = "";
        public string sStartLeave { get; set; } = "";
        public DateTime dStartLeave { get; set; }
        public string sEndLeave { get; set; } = "";
        public DateTime dEndLeave { get; set; }
        public string sLeaveUse { get; set; } = "";
        public string sStatus { get; set; } = "";
        public int nStatus { get; set; } = 0;
        public bool isEnableEdit { get; set; }
        public bool isEnableApprove { get; set; }
        public bool? isDelete { get; set; }
    }

    public class cLeaveSummaryRequest
    {
        public List<cLeaveDetailRequest> lstLeaveDetail { get; set; }
        public string? sID { get; set; }
        public string? sNameTH { get; set; }
        public DateTime dEventStart { get; set; }
        public DateTime dEventEnd { get; set; }
        public string sStartLeave
        {
            get
            {
                return dEventStart.ToString("dd/MM/yyyy");
            }
        }
        public string sEndLeave
        {
            get
            {
                return dEventEnd.ToString("dd/MM/yyyy");
            }
        }
        public string? sEmpType { get; set; }
        public string? sEmpYear { get; set; }
        public int? nEmployeeID { get; set; }
        public int? No { get; set; }
        public int? nLeaveTypeID { get; set; }
        public string? sLeaveDate { get; set; }
        public string? sLeaveData { get; set; }
    }

    public class cLeaveDetailRequest
    {
        public int nLeaveType { get; set; }
        public string? sID { get; set; }
        public string? sNameTH { get; set; }
        public string? sNameEN { get; set; }
        public string? sSurNameTH { get; set; }
        public string? sSurNameEN { get; set; }
        public string? sPosition { get; set; }
        public string? sLeaveDate { get; set; }
        public DateTime dEventStart { get; set; }
        public DateTime dEventEnd { get; set; }
        public int? nEmployeeID { get; set; }
        public int? nLeaveTypeID { get; set; }
        public string? sLeaveUse { get; set; }
        public int? nLeaveSummaryID { get; set; }
        public string? sLeaveData { get; set; }
    }

    public class cCalendarRequest
    {
        public string id { get; set; }
        public string groupId { get; set; }
        public DateTime dStart { get; set; }
        public DateTime dEnd { get; set; }
        public string title { get; set; }
        public string backgroundColor { get; set; }
        public string? textColor { get; set; }
        public bool? IsMeetingRoom { get; set; }
    }

    public class cFormInitRequest : ResultAPI
    {
        public DateTime[] arrHoliday { get; set; } = { };
        public LeaveTotalRequest[] arrLeaveTotal { get; set; } = { };
        public cSelectOptionleave[] arrOption { get; set; } = { };
        public cSelectOptionleave[] arrOptionStatus { get; set; } = { };
        public List<cSelectOptionleave>? lstDataEmployee { get; set; }
        public string? sTypeID { get; set; }
        public bool? isEmergency { get; set; }
        public DateTime? dStart { get; set; }
        public DateTime? dEnd { get; set; }
        public string? sReason { get; set; }
        public decimal? nUse { get; set; }
        public List<ItemFileData>? arrFile { get; set; } = new List<ItemFileData> { };
        public bool isRequestor { get; set; } = false;
        public int? nStatus { get; set; }
        public List<ApproverDataRequest> arrApprover { get; set; } = new List<ApproverDataRequest>();
        public cSelectOptionleave[] arrOptionEmployee { get; set; } = { };
        public bool isHr { get; set; } = false;
        public bool isLead { get; set; } = false;
        public List<string>? lstSelectUser { get; set; }
        public List<string>? lstStatus { get; set; }
        public string? sEmployeeID { get; set; }
        public List<string>? lstSelectUserID { get; set; }
        public int? nFocusStep { get; set; }
        public List<cStepperleleave>? lstStepper { get; set; }
    }
    public class cStepperleleave
    {
        public string? sLabel { get; set; }
        public string? sActionBy { get; set; }
        public string? sDateAction { get; set; }
    }

    public class cLeaveLogsRequest : ResultAPI
    {
        public LeaveLogsDataRequest[] arrData { get; set; } = { };
        public int? nStatus { get; set; }
    }

    public class LeaveLogsDataRequest
    {
        public string sLogName { get; set; }
        public string sPosition { get; set; }
        public string sComment { get; set; }
        public string sStatus { get; set; }
        public string sActionDate { get; set; }
        public string sImgPath { get; set; }
        public int nStatus { get; set; }
        public string sActionTime { get; set; }

    }
    public class LeaveTotalRequest
    {
        public string sLeaveTypeName { get; set; }
        public decimal dLeaveTotal { get; set; }
        public decimal dLeaveRemain { get; set; }
        public decimal dLeaveMax { get; set; }
        public string sID { get; set; }
        public string? sFileLink { get; set; }
        public int nLeaveCount { get; set; }
        public LeaveTotalRequest[] arrLeaveTotal { get; set; }
        public List<LeaveTotalRequest> lstLeaveTotal { get; set; }
    }
    public class cReqInitFormRequest
    {
        public string sID { get; set; } = "";
    }

    public class cFormReqRequest
    {
        public string sTypeID { get; set; } = "";
        public bool isEmergency { get; set; } = false;
        public double dStart { get; set; } = 0.00;
        public double dEnd { get; set; } = 0.00;
        public string sReason { get; set; } = "";
        public string? sComment { get; set; } = "";
        public int nMode { get; set; } = (int)Backend.Enum.EnumLeave.SaveMode.SaveDraft;
        public string? sID { get; set; } = "";
        public decimal nUse { get; set; } = 0;
        public ItemFileData[] arrFile { get; set; } = { };
        public bool? IsActionFromLine { get; set; } = false;
        public string? sGUID { get; set; }
        public string? sEmployeeID { get; set; }
    }

    public class cSelectRequest
    {
        public string? value { get; set; }
        public string? label { get; set; }
    }

    public class cObjectImportExcelRequest
    {
        public string? sID { get; set; }
        public string sCode { get; set; }
        public string? sDetail { get; set; }
    }

    public class cLeaveImportExcelRequest
    {
        public string? sFileName { get; set; }
        public string? sPath { get; set; }
        public string? sSysFileName { get; set; }
    }

    public class cLeaveExportRequest
    {
        public List<cLeaveSummary>? lstData { get; set; }
        public List<cSelectOptionleave>? lstType { get; set; }
    }

    public class cExportExcelRequest : Pagination
    {
        public byte[] objFile { get; set; }
        public string sFileType { get; set; }
        public string? sFileName { get; set; }
    }

    // public class cEditLeaveDate
    // {
    //     // public  List<cLeaveSummary>? lstLeaveSummary { get; set; }
    //     // public List<cLeaveSummary>? lstSummaryLeave { get; set; }
    //    public List<cLeaveDetail> lstLeaveDetail { get; set; }
    // }

    public class cEditLeaveDateleaveRequest
    {
        public List<cLeaveDetailRequest>? lstLeaveDetail { get; set; }
        public string? sID { get; set; }
        public string? sNameTH { get; set; }
        public DateTime dEventStart { get; set; }
        public DateTime dEventEnd { get; set; }
        public string sStartLeave
        {
            get
            {
                return dEventStart.ToString("dd/MM/yyyy");
            }
        }
        public string sEndLeave
        {
            get
            {
                return dEventEnd.ToString("dd/MM/yyyy");
            }
        }
        public string? sEmpType { get; set; }
        public string? sEmpYear { get; set; }
        public int? nEmployeeID { get; set; }
        public int? No { get; set; }
        public int? nLeaveTypeID { get; set; }
    }
    public class cSelectOptionleave
    {
        public string? label { get; set; }

        public string? value { get; set; }
    }
}