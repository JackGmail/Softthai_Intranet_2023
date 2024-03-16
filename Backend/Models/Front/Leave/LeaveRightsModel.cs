using System.Text.Json;
using Extensions.Common.STResultAPI;
using ST.INFRA.Common;
using static Extensions.Systems.AllClass;
using ST.INFRA;
using Backend.Models;
using ResultAPI = Backend.Models.ResultAPI;
using Backend.Models.Front.Project;

namespace ST_API.Models
{
    public class cLeaveInitData : ResultAPI
    {
        public int nLeave { get; set; }
        public int nLate { get; set; }
        public int nWFH { get; set; }
        public int nLeaveWithCert { get; set; }
        public List<cCalendar>? lstData { get; set; }
        public LeaveRightsTotalRequest[] arrLeaveTotal { get; set; }
    }

    public class LeaveRightsTotalRequest
    {
        public string sLeaveTypeName { get; set; }
        public string sID { get; set; }
        public string? sFileLink { get; set; }
        public int nLeaveCount { get; set; }
        public int nID { get; set; }
    }

    public class cReturnImportExcel : ResultAPI
    {
        public List<cObjectImportExcel>? lstData { get; set; }
    }

    public class cLeaveTable : STGrid.PaginationData
    {
        public int? nID { get; set; }
    }

    public class cEditLeave
    {
        public List<cLeaveSummary>? lstLeaveDate { get; set; }
        public List<string>? Year { get; set; }
        // public List<cLeaveSummary> lstSummaryLeave { get; set; }
    }

    public class cLeaveRights : STGrid.PaginationData
    {
        public List<string>? lstID { get; set; }
        public string? sNameTH { get; set; }
        public int? nTypeID { get; set; }
        public int? nStatusID { get; set; }
        public List<string>? sEmpTypeID { get; set; }
        public List<string>? Year { get; set; }
        public List<string>? lstEmpName { get; set; }
    }

    public class cResultTableLeave : Pagination
    {
        public List<cLeaveDetail>? lstData { get; set; }
        public List<cLeaveDetail>? lstHoliday { get; set; }
        public List<cLeaveDetail>? lstActivity { get; set; }
        public List<cLeaveHomeTable>? lstDataHome { get; set; }
        public cLeaveHomeTable[]? arrDataHome { get; set; }
        public List<cSelectOption>? lstLeaveType { get; set; }
        public List<cLeaveSummary>? lstSummaryLeave { get; set; }
        public List<cLeaveSummary>? query { get; set; }        
        public cSelect[]? lstEmployeeName { get; set; }
        public cSelect[]? lstYear { get; set; }
        public cSelect[]? lstSelectEmpType { get; set; }
        public List<cLeaveSummary>? sID { get; set; }
        public cLeaveHome[] arrHome { get; set; }
    }

    public class cLeaveHome
    {
        public int nLeaveID { get; set; }
        public int nCount { get; set; }
        public string sName { get; set; }
    }

    public class cTabelWorkList : Pagination
    {
        public TableDataWorkList[] arrData { get; set; } = { };
    }

    public class cReqTableWorkList : STGrid.PaginationData
    {
        public string[] arrTypeID { get; set; } = { };
        public string[] arrStatusID { get; set; } = { };
        public int nTypeFilterDate { get; set; } = 0;
        public double? dStartFilterDate { get; set; }
        public double? dEndFilterDate { get; set; }
        public bool isApproveListPage { get; set; } = false;
        public string[] arrEmpID { get; set; } = { };
    }

    public class ApproverData
    {
        public string sID { get; set; }
        public int nNo { get; set; }
        public string sName { get; set; }
        public string sPosition { get; set; }
        public string sStatus { get; set; }
        public string sEmployeeID { get; set; }
        public bool isHr { get; set; }
    }

    public class TableDataWorkList
    {
        public string sID { get; set; }
        public string sCreateDate { get; set; } = "";
        public DateTime dCreateDate { get; set; }
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
    }

    public class cLeaveSummary
    {
        public List<cLeaveDetail> lstLeaveDetail { get; set; } = new List<cLeaveDetail>();
        public string? sID { get; set; }
        public string? sFullNameTH { get; set; }
        public string? sNameTH { get; set; }
        public string? sName { get; set; }
        public string? sSurname { get; set; }
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
        public int? nEmpType { get; set; }
        public int? No { get; set; }
        public int Year { get; set; }
        // public int nLeaveTypeID { get; set; }
    }

    public class cLeaveDetail : STGrid.PaginationData
    {
        public int nLeaveType { get; set; }
        public string? sID { get; set; }
        public int nID { get; set; }
        public string? sNameTH { get; set; }
        public string? sNameEN { get; set; }
        public string? sSurNameTH { get; set; }
        public string? sSurNameEN { get; set; }
        public string? sPosition { get; set; }
        public string? sLeaveDate { get; set; }
        public DateTime dEventStart { get; set; }
        public DateTime dEventEnd { get; set; }
        public string sStartLeave
        {
            get
            {
                // return dEventStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                return dEventStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_dd);
            }
        }
        public string sEndLeave
        {
            get
            {
                return dEventEnd.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
            }
        }
        public int? nEmployeeID { get; set; }
        public int? nLeaveTypeID { get; set; }
        public string? sLeaveUse { get; set; }
        public int? nLeaveSummaryID { get; set; }
        public string? sLeaveData { get; set; }
        public string? sUse { get; set; }
        public int Year { get; set; }
        public string? sLeaveTypeName { get; set; }
        public int? nLeaveCount { get; set; }
        public string? sPer { get; set; }
        public string? sActivity { get; set; }
    }

    public class cLeaveHomeTable
    {
        public int nLeaveType { get; set; }
        public string? sID { get; set; }
        public int nID { get; set; }
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
        public string? sUse { get; set; }
        public int Year { get; set; }
        public string? sLeaveTypeName { get; set; }
        public int? nLeaveCount { get; set; }
        public string? sPer { get; set; }
    }

    public class cCalendar
    {
        public string sID { get; set; }
        public string groupId { get; set; }
        public DateTime dEventStart { get; set; }
        public DateTime dEventEnd { get; set; }
        public string start
        {
            get
            {
                return dEventStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
            }
        }
        public string end
        {
            get
            {
                return dEventEnd.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
            }
        }
        public string title { get; set; }
        public string backgroundColor { get; set; }
        public string? textColor { get; set; }
        public bool? IsMeetingRoom { get; set; }
        public int? nStatus { get; set; }
        public bool allDay { get; set; }
    }

    public class cFormInit : ResultAPI
    {
        public DateTime[] arrHoliday { get; set; } = { };
        public LeaveTotal[] arrLeaveTotal { get; set; } = { };
        public cSelectOption[] arrOption { get; set; } = { };
        public cSelectOption[] arrOptionStatus { get; set; } = { };
        public string? sTypeID { get; set; }
        public bool? isEmergency { get; set; }
        public DateTime? dStart { get; set; }
        public DateTime? dEnd { get; set; }
        public string? sReason { get; set; }
        public decimal? nUse { get; set; }
        public ItemFileData[]? arrFile { get; set; } = { };
        public bool isRequestor { get; set; } = false;
        public int? Status { get; set; }
        public List<ApproverData> arrApprover { get; set; } = new List<ApproverData>();
        public cSelectOption[] arrOptionEmployee { get; set; } = { };
        public bool isHr { get; set; } = false;
        public bool isLead { get; set; } = false;
    }

    public class cLeaveLogs : ResultAPI
    {
        public LeaveLogsData[] arrData { get; set; } = { };
    }

    public class LeaveLogsData
    {
        public string sLogName { get; set; }
        public string sPosition { get; set; }
        public string sComment { get; set; }
        public string sStatus { get; set; }
        public string sActionDate { get; set; }
        public string sImgPath { get; set; }
        public int Status { get; set; }

    }
    public class LeaveTotal
    {
        public string sLeaveTypeName { get; set; }
        public decimal dLeaveTotal { get; set; }
        public decimal dLeaveRemain { get; set; }
        public decimal dLeaveMax { get; set; }
        public string sID { get; set; }
    }
    public class cReqInitForm
    {
        public string sID { get; set; } = "";
    }

    public class cFormReq
    {
        public string sTypeID { get; set; } = "";
        public bool isEmergency { get; set; } = false;
        public double dStart { get; set; } = 0.00;
        public double dEnd { get; set; } = 0.00;
        public string sReason { get; set; } = "";
        public string sComment { get; set; } = "";
        public int nMode { get; set; }// = (int)Enum.EnumLeave.SaveMode.SaveDraft;
        public string? sID { get; set; } = "";
        public decimal nUse { get; set; } = 0;
        public ItemFileData[] arrFile { get; set; } = { };
    }

    public class cSelect
    {
        public string? value { get; set; }
        public string? label { get; set; }
    }

    public class cObjectImportExcel
    {
        public List<cLeaveDetail> lstLeaveDetail { get; set; }
        public string? sID { get; set; }
        public string? sNameTH { get; set; }
        public string? sName { get; set; }
        public string? sSurname { get; set; }
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
        public int? nEmpType { get; set; }
        public int? No { get; set; }
        public int? nLeaveTypeID { get; set; }
        public string? sLeaveDate { get; set; }
        public string? sLeaveData { get; set; }
        public int Year { get; set; }
    }

    public class cLeaveImportExcel
    {
        public string? sFileName { get; set; }
        public string? sPath { get; set; }
        public string? sSysFileName { get; set; }
    }

    public class cLeaveExport
    {
        public List<cLeaveSummary>? lstData { get; set; }
        public List<cSelectOption>? lstType { get; set; }
    }

    public class cExportExcel
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

    public class cEditLeaveDate
    {
        public List<cLeaveDetail>? lstLeaveDetail { get; set; }
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

    public class reqImport : ResultAPI
    {
        public List<ItemFileData>? fFile { get; set; }
        public string? nMasterID { get; set; }
    }

    public class cImport
    {
        public string? sSystem { get; set; }
        //public string? sFileName_Sys { get; set; }
        //public string? sCheckSystemcode { get; set; }

        public string? sFileName { get; set; }
        public string? sPath { get; set; }
        public string? sSysFileName { get; set; }
    }

    public class cReturnImportExcelLeave : ResultAPI
    {
        public List<cLeaveSummary> lstPrjLeave { get; set; }
    }
}