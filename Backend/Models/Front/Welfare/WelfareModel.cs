using Extensions.Common.STResultAPI;
using ST.INFRA.Common;
using static Extensions.Systems.AllClass;

namespace Backend.Models
{
    public class cDentalResult : Extensions.Common.STResultAPI.ResultAPI
    {
        public List<cOptionSelect>? lstData { get; set; }
        public string? sEmpName { get; set; }
        public string? sEmpPosition { get; set; }
        public int? nConditionAmount { get; set; }
        public cobjHR? objHR { get; set; }
        public string sRequestName { get; set; }
        public int? nRemainsMoney { get; set; }
    }

    public class cobjHR
    {
        public string sID { get; set; }
        public int nNo { get; set; }
        public string sName { get; set; }
        public string sPosition { get; set; }
        public string sAction { get; set; }
    }

    public class cDentalTableResult : Pagination
    {
        public List<cDentalDetail> lstDentalRequest { get; set; }
        public cDentalDetail objEdit { get; set; }
        public List<cMedRequestHistoryLog>? lstHistoryLog { get; set; }
        public string? sRequestName { get; set; }
        public int? nStatus { get; set; }
        public bool? IsHR { get; set; }
        public List<ItemFileData>? fFileImage { get; set; } = new List<ItemFileData> { };
    }

    // public class cDentalTable : STGrid.Option
    public class cDentalTable : STGrid.PaginationData
    {
        public string? sID { get; set; }
    }

    public class cOptionSelect
    {
        public string value { get; set; }
        public string label { get; set; }
        public bool? check { get; set; }
        public bool? disabled { get; set; }
    }

    public class cEmpData
    {
        public string? sEmpName { get; set; }
        public string? sEmpPosition { get; set; }
    }

    public class cDentalDetail
    {
        public string[]? sDentalType { get; set; }
        public string? sID { get; set; }
        public int? nEmpID { get; set; }
        // public int? nRequestDentalID { get; set; }
        public string? sFullNameTH { get; set; }
        public string? sPositionTH { get; set; }
        public string? sStatusTH { get; set; }
        // public string dDentalService { get; set; }
        public DateTime? dDentalService { get; set; }
        public decimal? nMoneyBalance { get; set; }
        public decimal? nRequest { get; set; }
        public decimal? nTotal { get; set; }
        public decimal? nConditionAmount { get; set; }
        // public int nDentalType { get; set; }
        public string? sHospitalName { get; set; }
        public string? sName { get; set; }
        public string? sPosition { get; set; }
        // public List<string>? sProve { get; set; } //upload file
        public int? nRequestType { get; set; }
        public int? nStatusID { get; set; } //2 == submit | 1 == draft
        public string? sDentalTypeName { get; set; }
        public int? nCheck1 { get; set; }
        public int? nCheck2 { get; set; }
        public int? nCheck3 { get; set; }
        public string? sComment { get; set; }
        public List<ItemFileData>? fFile { get; set; } = new List<ItemFileData> { };
        public bool? IsActionFromLine { get; set; } = false;
        public string? sGUID { get; set; }
    }

    public class cMedRequestHistoryLog
    {
        public string sName { get; set; }
        public string? sPosition { get; set; }
        public string sComment { get; set; }
        public string sStatus { get; set; }
        public string sActionMode { get; set; }
        public string sActionDate { get; set; }
    }

    public class cReqWelfareApprove
    {
        public string? sID { get; set; }
        public int? nStatusID { get; set; }
        public string? sComment { get; set; }
        public bool? IsActionFromLine { get; set; } = false;
        public string? sGUID { get; set; }
    }
}