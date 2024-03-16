using Extensions.Common.STResultAPI;
using ST.INFRA.Common;
using ST.INFRA;
using static Extensions.Systems.AllClass;
namespace ST_API.Models.ITypeleaveService
{

    public class cReqTableTypeleave : STGrid.PaginationData
    {
        public string? sStatus { get; set; }
        public string? sLeaveTypeCode { get; set; }
        public string? sLeaveTypeName { get; set; }

    }



    public class cResultTableTypeLeave : Pagination
    {
        public List<cTableTypeleave>? lstData { get; set; }
        public List<cTableLeaveSummary> lstDataSummary { get; set; }
        public List<cTableLeaveSummary> lstDataSummaryEdit { get; set; }

    }
    public class ResultTypeleave : ResultAPI
    {
        public List<SelectOption>? lstDataSex { get; set; }
        public List<SelectOption>? lstDataEmployeeType { get; set; }
        public List<cTypeleaveData>? lstOrder { get; set; }
        public List<cTypeleaveData>? lstOrder1 { get; set; }
        public List<cTypeleaveData>? lstOrder2 { get; set; }
        public List<ItemFileData>? fFileImage { get; set; } = new List<ItemFileData> { };
        public TypeleaveData objEdit { get; set; }
        public List<SelectOption>? lstDataleaveType { get; set; }
        public List<SelectOption>? lstDataleaveTypeAll { get; set; }
        public List<SelectOption>? lstDataEmployee { get; set; }
        public List<SelectOption>? lstOptionYear { get; set; }
        public List<cTableLeaveSummary> lstData { get; set; }
        public List<cTableLeaveSummary> lstDataRequest { get; set; }
        public List<cTableLeaveSummary> lstDataSummary { get; set; }
        public AlreadyDTO Data { get; set; }
        public List<string>? lstSelectUser { get; set; }
        public List<string>? lstleaveTypeAll { get; set; }


    }
    public class SelectOption
    {
        public string? label { get; set; }
        public string? value { get; set; }
        public bool isActive { get; set; }

    }
    public class TypeleaveData
    {
        public string? sID { get; set; }

        public int nLeaveID
        {
            get
            {
                return !string.IsNullOrEmpty(sID) ? sID.DecryptParameter().ToInt() : 0;
            }
        }
        public int nLeaveTypeID { get; set; }
        public string sLeaveTypeCode { get; set; }
        public string sLeaveTypeName { get; set; }
        public int nOrder { get; set; }
        public int nSex { get; set; }
        public decimal? nAssociate { get; set; }
        public decimal? nAdvanceLeave { get; set; }
        public decimal? nMaximum { get; set; }
        public int nChangeIntoMoney { get; set; }
        public string? sCondition { get; set; }
        public bool isActive { get; set; }
        public DateTime dCreate { get; set; }
        public int nCreateBy { get; set; }
        public DateTime dUpdate { get; set; }
        public int nUpdateBy { get; set; }
        public DateTime? dDelete { get; set; }
        public int? nDeleteBy { get; set; }
        public bool isDelete { get; set; }

        public int LeaveProportionID { get; set; }
        public int nEmployeeTypeID { get; set; }

        public decimal? nJan { get; set; }

        public decimal? nFeb { get; set; }

        public decimal? nMar { get; set; }

        public decimal? nApr { get; set; }

        public decimal? nMay { get; set; }

        public decimal? nJun { get; set; }

        public decimal? nJul { get; set; }

        public decimal? nAug { get; set; }

        public decimal? nSep { get; set; }

        public decimal? nOct { get; set; }

        public decimal? nNov { get; set; }

        public decimal? nDec { get; set; }
        public bool isCheckBoxAllow { get; set; }

        public List<cTypeleaveData> lstDataType { get; set; }
        public List<ItemFileData>? fFile { get; set; } = new List<ItemFileData> { };

    }
    public class cTypeleaveData
    {
        public int nWorkingAgeStart { get; set; }
        public int nWorkingAgeEnd { get; set; }
        public decimal nLeaveRights { get; set; }
        public bool isDataType { get; set; }
        public int nOrder { get; set; }
        public bool isProportion { get; set; }
        public int nEmployeeTypeID { get; set; }
        public bool isChk { get; set; }
        public int nLeaveSettingID { get; set; }
        public bool IsDelete { get; set; }
        public List<SelectOption>? lstDataEmployeeType { get; set; }
    }

    public class cTableTypeleave
    {
        public string? sID { get; set; }
        public int nLeaveTypeID { get; set; }
        public string? sStatus { get; set; }
        public string? sLeaveTypeCode { get; set; }
        public string? sLeaveTypeName { get; set; }
        public DateTime dUpdate { get; set; }
        public bool isActive { get; set; }
        public int nStatus { get; set; }

    }
    public class cParam
    {
        public string? sID { get; set; }

        public int nLeaveID
        {
            get
            {
                return !string.IsNullOrEmpty(sID) ? sID.DecryptParameter().ToInt() : 0;
            }
        }

    }
    public class cTableLeaveSummary

    {
        public string? sID { get; set; }
        public int nSummaryID
        {
            get
            {
                return !string.IsNullOrEmpty(sID) ? sID.DecryptParameter().ToInt() : 0;
            }
        }

        public int nLeaveSummaryID { get; set; }
        public int nEmployeeID { get; set; }
        public string sEmployeeName { get; set; }
        public int nLeaveTypeID { get; set; }
        public string sYear { get; set; }
        public decimal nQuantity { get; set; }
        public decimal nTransferred { get; set; }
        public decimal? nIntoMoney { get; set; }
        public decimal nLeaveUse { get; set; }
        public decimal nLeaveRemain { get; set; }
        public DateTime dUpdate { get; set; }
        public string sLeaveTypeName { get; set; }
        public List<LeaveSummeryData> lstLeaveDetail { get; set; }
        public int nMonth { get; set; }
        public string sMonth { get; set; }
        public decimal nLeaveUseTotal { get; set; }
        public List<SelectOption>? lstDataleaveType { get; set; }
        public List<SelectOption>? lstDataEmployee { get; set; }
        public string? sLeaveTypeID { get; set; }
        public string? sEmployeeID { get; set; }
        public int nYear { get; set; }
        public string sYearWork { get; set; }

    }

    public class cReqTableLeaveSummary : STGrid.PaginationData
    {
        public string? sID { get; set; }
        public int nEmployeeID { get; set; }
        public int nLeaveTypeID { get; set; }
        public string? sEmployeeName { get; set; }
        public int nYear { get; set; }
        public string? sLeaveTypeName { get; set; }
        public int nIntoMoney { get; set; }
        public int nLeaveRemain { get; set; }
        public string? sYearWork { get; set; }
        public List<string>? lstLeaveType { get; set; }
        public List<string>? lstEmployeeID { get; set; }
    }
    public class cParamSummary : cReqTableLeaveSummary
    {

        public string? sID { get; set; }
        public int nEmployeeID { get; set; }

        public int nLeaveSummaryID
        {
            get
            {
                return !string.IsNullOrEmpty(sID) ? sID.DecryptParameter().ToInt() : 0;
            }
        }
    }

    public class cSaveSummary

    {
        public string? sID { get; set; }
        public int nSummaryID
        {
            get
            {
                return !string.IsNullOrEmpty(sID) ? sID.DecryptParameter().ToInt() : 0;
            }
        }
        public int nEmployeeID { get; set; }
        public int nLeaveTypeID { get; set; }
        public decimal nIntoMoney { get; set; }
        public decimal nLeaveRemain { get; set; }
        public decimal nQuantity { get; set; }

    }
    public class cExportExcelSummary : Pagination
    {
        public byte[] objFile { get; set; }
        public string sFileType { get; set; }
        public string? sFileName { get; set; }
    }
    public class cParamExport
    {
        public int nYear { get; set; }
        public int nEmployeeID { get; set; }
        public int nLeaveTypeID { get; set; }
        public int nIntoMoney { get; set; }
        public int nLeaveRemain { get; set; }
        public string? sYearWork { get; set; }
    }

    public class AlreadyDTO
    {
        public bool IsAlready { get; set; }
        public int nWorkingAgeStart { get; set; }


    }
    public class ParamCheck
    {
        public int nWorkingAgeStart { get; set; }
        public int nWorkingAgeEnd { get; set; }
        public int nEmployeeTypeID { get; set; }
        public string sLeaveTypeCode { get; set; }
    }

    public class LeaveSummeryData
    {
        public int nLeaveSummaryID { get; set; }
        public string sEmployeeName { get; set; }
        public int nEmployeeID { get; set; }
        public int nLeaveTypeID { get; set; }
        public decimal nLeaveUse { get; set; }
        public string sLeaveTypeName { get; set; }
        public int nMonth { get; set; }
        public string sMonth { get; set; }
        public decimal nLeaveRemain { get; set; }
        public decimal nLeaveUseTotal { get; set; }


    }
    public class cParamSearch
    {
        public int nEmployeeID { get; set; }
        public int nLeaveTypeID { get; set; }
        public int nNumber { get; set; }
        public List<string>? lstLeaveType { get; set; }
        public List<string>? lstLeaveTypereq { get; set; }
        public List<string>? lstLeaveTypeemp { get; set; }
        public List<string>? lstEmployeeID { get; set; }
        public int nYear { get; set; }
        public string? sStartreq { get; set; }
        public string? sEndreq { get; set; }
        public int nYearEmp { get; set; }

    }
    public class cRemoveTableLeave : STGrid.PaginationData
    {
        public List<string>? lstID { get; set; } = new List<string>();
    }
    public class cSysTypeleave
    {
        public List<string>? lstEmployee { get; set; }
        public int nYear { get; set; }
    }

    public class cSysleaveSummary
    {

        public int nEmployeeID { get; set; }
        public int nLeaveTypeID { get; set; }
        public decimal nLeaveRights { get; set; }

    }

}
