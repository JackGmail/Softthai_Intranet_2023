using ST.INFRA;
using ST.INFRA.Common;
using static Extensions.Systems.AllClass;
using Extensions.Common.STResultAPI;
using Extensions.Common.STExtension;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Backend.Models
{
    public class cGetFilter : STGrid.PaginationData
    {
        public string? sID { get; set; }
        public List<string>? lstEmployee { get; set; }
        public string? objYear { get; set; }
        public DateTime? dRequestStart { get; set; }
        public DateTime? dRequestEnd { get; set; }
    }

    public class cGetDataForExcel : STGrid.PaginationData
    {
        public string? objYear { get; set; }
        public DateTime? dRequestStart { get; set; }
        public DateTime? dRequestEnd { get; set; }
    }

    public class cDataTable : Pagination
    {
        public List<cEmployeeList> lstData { get; set; }
        public List<cEmpWorkSchedule> lstEmpData { get; set; }
        public string sEmpID { get; set; }
        public int? nCalculateLateTime { get; set; }
        public decimal? nCalculateOT { get; set; }
        public int? nCalculateWFH { get; set; }
        public string? sCalculateLeave { get; set; }
    }

    public class cInitDataWorkSchedule : ResultAPI
    {
        public List<cSelectOption>? lstEmployee { get; set; }
        public List<cEmpWorkSchedule>? lstEmployeeData { get; set; }
        public List<cSelectOption>? lstYear { get; set; }
    }

    public class cEmployeeData
    {
        public string? sID { get; set; }
    }

    public class cSaveEmployeeData
    {
        public string? sID { get; set; }
        public List<cEmpWorkSchedule>? lstEmpData { get; set; }
    }

    public class cExportRequest
    {
        public List<cEmployeeList>? lstWorkShcedule { get; set; }
        public string? sFileName { get; set; }
        public string? objYear { get; set; }
        public DateTime? dRequestStart { get; set; }
        public DateTime? dRequestEnd { get; set; }
    }

    public class cEmployeeList
    {
        public string sID { get; set; }
        public int? No { get; set; }
        public int nEmployeeID { get; set; }
        public string sEmpName { get; set; }
        public string? sLeaveRequest { get; set; }
        public string? sOTRequest { get; set; }
        public string? sWFHRequest { get; set; }
        public DateTime dlatestEdit { get; set; }
        public string? slatestEdit
        {
            get
            {
                return dlatestEdit.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_dd);
            }
        }
        public DateTime? dWorkStart { get; set; }
        public string? sWorkStart
        {
            get
            {
                return dWorkStart?.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_dd);
            }
        }
        public DateTime dDateWork { get; set; }
        public DateTime dTimeStartWork { get; set; }
        public string? sLateTime { get; set; }
        public string? sNickName { get; set; }
    }

    public class cEmpWorkSchedule
    {
        public string? sID { get; set; }
        public string? sComment { get; set; }
        public DateTime? dStartTime { get; set; }
        public DateTime? dEndTime { get; set; }
        public string? sLateTime { get; set; }
        public DateTime? dTimeDate { get; set; }
        public int? No { get; set; }
        public int nEmployeeID { get; set; }
        public string? sEmployeeID { get; set; }
        public string? sEmpName { get; set; }
        public DateTime dlatestEdit { get; set; }
        public string? slatestEdit
        {
            get
            {
                return dlatestEdit.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_dd);
            }
        }
        public string? sEmployeeCode { get; set; }
        public string? sPosition { get; set; }
        public string? sPhone { get; set; }
        public string? sEmail { get; set; }
        public DateTime? dWorkStart { get; set; }
        public string? sWorkStart
        {
            get
            {
                return dWorkStart?.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_dd);
            }
        }
        public string? sEmployeeType { get; set; }
        public string? sWorkYear { get; set; }
        public int nEmployeeTypeID { get; set; }
        public string? sOTRequestHours { get; set; }
        public string? sWHFCount { get; set; }
        public DateTime dDateWork { get; set; }
        public string? sDateWork { get; set; }
        public DateTime dTimeStartWork { get; set; }
        public DateTime dTimeEndWork { get; set; }
        public string? sTimeStartWork { get; set; }
        public string? sTimeEndWork { get; set; }
        public string? sLeaveTime { get; set; }
        public string? sCalculateWFH { get; set; }
        public decimal? nCalculateOT { get; set; }
        public string? sCalculateLeave { get; set; }
        public string? sCalculateLateTime { get; set; }
    }

    public class cLeaveRequest
    {
        public decimal nLeaveUse { get; set; }
        public int? nLeaveTypeID { get; set; }
        public int? nIDLeave { get; set; }
        public DateTime dlatestEdit { get; set; }
        public string? slatestEdit
        {
            get
            {
                return dlatestEdit.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_dd);
            }
        }
        public DateTime? dWorkStart { get; set; }
        public string? sWorkStart
        {
            get
            {
                return dWorkStart?.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_dd);
            }
        }
        // public string sLeaveTime { get; set; }
        public DateTime? dStartDateTime { get; set; }
        public DateTime? dEndDateTime { get; set; }
        public string sLeaveTypeName { get; set; }
    }

    public class cExport : ResultAPI
    {
        public byte[] objFile { get; set; }
        public string sFileType { get; set; }
        public string? sFileName { get; set; }
    }
}