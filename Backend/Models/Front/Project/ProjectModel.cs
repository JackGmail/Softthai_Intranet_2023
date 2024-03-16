

using Extensions.Common.STExtension;
using Extensions.Common.STResultAPI;
using ST.INFRA.Common;
using static Extensions.Systems.AllClass;

namespace Backend.Models.Front.Project
{
    public class cResultProject : ResultAPI
    {
        public List<cSelectOption>? lstRedio { get; set; }
        public List<cSelectOption>? lstPosition { get; set; }
        public List<cSelectOption>? lstCustomer { get; set; }
        public List<cSelectOptionCustomer>? lstContractPoint { get; set; }
        public List<cSelectOption>? lstProject { get; set; }
        public List<cMasterDataProcess>? lstConfirmProcessSub { get; set; }
        public List<cMasterDataProcessMain>? lstConfirmProcessMain { get; set; }
        public List<ObjectResultProcess> lstDataALL { get; set; } = new List<ObjectResultProcess>();


    }
    public class cReqProject
    {
        public string? sID { get; set; }
    }

    public class cProject : ResultAPI
    {
        public string? sID { get; set; }
        public int? nParentID { get; set; }
        public int nCustomerID { get; set; }
        public string? sProjectName { get; set; }
        public string? sProjectAbbr { get; set; }
        public string? sIntroduce { get; set; }
        public int nProjectTypeID { get; set; }
        public int? nProjectID { get; set; }
        public List<cProjectMember> lstMemberData { get; set; } = new List<cProjectMember>();
        public List<cProjectContractPoint> lstContactData { get; set; } = new List<cProjectContractPoint>();
        public List<cProjectContractPoint> lstAllDataContact { get; set; } = new List<cProjectContractPoint>();
        public List<cProjectMember> lstAllMemberData { get; set; } = new List<cProjectMember>();
    }


    public class cProjectContractPoint
    {
        public string? sID { get; set; }
        public int nRow { get; set; } = 0;
        public int nProjectContractPointID { get; set; } 
        public bool IsDel { get; set; }
        public int? nProjectID { get; set; }
        public string ? sName { get; set; }
        public string? sEmail { get; set; }
        public string? sTel { get; set; }
        public string? sCustomerName { get; set; }
        public int nCustomerID { get; set; }
        public int nContactPointID { get; set; }


    }
    public class cProjectMember
    {
        public string? sID { get; set; }
        public int nRow { get; set; } = 0;
        public int nPositionID { get; set; } = 0;
        public string sPositionName { get; set; } = string.Empty;
        public int nEmployeeID { get; set; } = 0;
        public string sEmployeeName { get; set; } = string.Empty;
        public string sPosition { get; set; } = string.Empty;
        public bool IsDel { get; set; }
        public bool IsStatus { get; set; }
        public int? nProjectID { get; set; }

    }
    public class cProcessTable : STGrid.PaginationData
    {
        public int? nParentID { get; set; }
        public string sID { get; set; }
        public List<ObjectResultProcess> arrRows { get; set; }
        public int nDataLength { get; set; }
        public int nPageIndex { get; set; }
    }

    public class cProjectData : ResultAPI
    {
        public string? sID { get; set; }
        public int nContractPointID { get; set; }
        public string sEmail { get; set; }
        public string sName { get; set; }
        public string sTelephone { get; set; }
    }

    public class EmployeeData
    {
        public string label { get; set; }
        public string value { get; set; }
        public string? nEmployeeID { get; set; }
        public string? sFirstname { get; set; }
        public string? sLastName { get; set; }
    }


    public class cProcess : ResultAPI
    {
        public string? sID { get; set; }
        public int nMasterProcessID { get; set; }
        public int? nProcessID { get; set; }
        public int nProjectID { get; set; }
        public int nOrder { get; set; }
        public decimal nManhour { get; set; }
        public bool isManhour { get; set; }

    }

    public class cSelectOptionCustomer
    {
        public string? label { get; set; }
        public string? value { get; set; }
        public int? nCustomerID { get; set; }
    }
    public class cMasterDataProcess
    {
        public string? label { get; set; }
        public string? value { get; set; }
        public int? nParentID { get; set; }
        public string? sNote { get; set; }
        public int? nMasterProcessTypeID { get; set; }
        public int nOrder { get; set; }
    }

    public class ObjectResultProcess
    {
        public int nNo { get; set; }
        public int nID { get; set; }
        public string sID { get; set; }
        public int nMasterProcessID { get; set; }
        public int? nParentID { get; set; }
        public string? sMasterProcessName { get; set; }
        public string? sNote { get; set; }
        public int nMasterProcessTypeID { get; set; }
        public bool IsStandProcess { get; set; }
        public bool isActive { get; set; }
        public int nOrder { get; set; }
        public List<int>? lstOrder { get; set; }
        public DateTime dUpdate { get; set; }
    }
    public class cResultProcessTable : Pagination_New
    {
        public List<ObjectResultProcess>? lstData { get; set; }
        //public List<cOrderOptions>? lstOrderOptions { get; set; }
    }
    public class Pagination_New : ResultAPI
    {
        public int nDataLength { get; set; } //nDataCountAll
        public int nPageIndex { get; set; }
        public int nPageSize { get; set; }
        public int nSkip { get; set; } //nSkipData
        public int nTake { get; set; } //nTakeData
        public int nStartIndex { get; set; } //nStartItemIndex

        //public string sID { get; set; }
        public List<ObjectResultProcess> arrRows { get; set; }
        public string sSortExpression { get; set; }
        public string sSortDirection { get; set; }
    }

    public class cMasterDataProcessMain
    {
        public string? label { get; set; }

        public string? value { get; set; }
        public string sID { get; set; }
        public cResultProcessTable? lstDataProcessSub { get; set; }
    }

    public class cProjectTable : STGrid.PaginationData
    {
        public string? sProjectName { get; set; }
        public string? sCustomer { get; set; }
        public int? nProjectID { get; set; }

        public List<string>? lstRemove { get; set; } = new List<string>();
    }
    public class cResultProjectTable : Pagination
    {
        public List<ObjectResultProject>? lstData { get; set; }
    }
    public class ObjectResultProject
    {
        public int? No { get; set; }
        public int nID { get; set; }
        public string sID { get; set; }
        public int? nCustomerID { get; set; }
        public int? nProjectID { get; set; }
        public string? sProjectName { get; set; }
        public string? sCustomerName { get; set; }
        public DateTime? dStart { get; set; }
        public string? sStartDate { get; set; }
        public DateTime? dEnd { get; set; }
        public string? sEndDate { get; set; }
        public DateTime dUpdate { get; set; }
        public string? sUpdate { get; set; }
    }
    #region sun
    public class cProjectTOR : ResultAPI
    {
        public List<ObjectProjectTOR> lstDataTOR { get; set; }
    }
    public class ObjectProjectTOR
    {
        public string sProjectID { get; set; }
        public string sID { get; set; }
        public string? sCode { get; set; }
        public string? sDetail { get; set; }
        public bool? IsDel { get; set; }
    }
    #region ExportExcel
    public class cExportExcelTOR
    {
        public string? sFileName { get; set; }
        public string sProjectID { get; set; }
    }
    public class cReturnExportExcelTOR
    {
        public byte[] objFile { get; set; }
        public string sFileType { get; set; }
        public string? sFileName { get; set; }
    }
    #endregion
    #region ImportExcel
    public class reqImportTOR : ResultAPI
    {
        public List<ItemFileData>? fFile { get; set; }
        public string? nMasterID { get; set; }
    }
    public class cReturnImportExcelTOR : ResultAPI
    {
        public List<ObjectProjectTOR> lstProjectTOR { get; set; }
    }
    #endregion




    #endregion
    /// <summary>
    /// </summary>
    public class objOptionPlanTest : ResultAPI
    {
        /// <summary>
        /// </summary>
        public List<objOption> listProject { get; set; } = new List<objOption>();
        /// <summary>
        /// </summary>
        public List<string> listProjectTest { get; set; } = new List<string>();
        /// <summary>
        /// </summary>
        public List<objOption> listTester { get; set; } = new List<objOption>();
        /// <summary>
        /// </summary>
        public List<string> listsTester { get; set; } = new List<string>();
    }


    /// <summary>
    /// </summary>
    public class resultDynamicTable : Pagination
    {
        /// <summary>
        /// </summary>
        public List<dynamicTable> listProject { get; set; } = new List<dynamicTable>();
        /// <summary>
        /// </summary>
        //public List<ProjectPlaning.cActionBy>? lstActionBy { get; set; }

    }

    /// <summary>
    /// </summary>
    public class dynamicTable
    {
        /// <summary>
        /// </summary>
        public string sName { get; set; } = "";
        /// <summary>
        /// </summary>
        public List<objDynamicTable> listProjectSub { get; set; } = new List<objDynamicTable>();

    }
    /// <summary>
    /// </summary>
    public class objDynamicTable
    {
        public int? sID { get; set; }
        /// <summary>
        /// </summary>
        public string sDevelopTask { get; set; } = "";

        /// <summary>
        /// </summary>
        public decimal? nProgress { get; set; }
        /// <summary>
        /// </summary>
        public string? sRemark { get; set; }
        /// <summary>
        /// </summary>
        public string? sPlanTestDate { get; set; }
        /// <summary>
        /// </summary>
        public string sPlanManhour { get; set; } = "";
        /// <summary>
        /// </summary>
        public string sActualTest { get; set; } = "";
        /// <summary>
        /// </summary>
        //public List<ProjectPlaning.cActionBy>? lstActionBy { get; set; }
        /// <summary>
        /// </summary>
        public string sTestBy { get; set; } = "";
        /// <summary>
        /// </summary>
        public string sUpdateBy { get; set; } = "";
        /// <summary>
        /// </summary>
        public int nProjectID { get; set; }
        /// <summary>
        /// </summary>
        public DateTime? dStartDev { get; set; }
        /// <summary>
        /// </summary>
        public DateTime? dEndDev { get; set; }
        /// <summary>
        /// </summary>
        public string sDevelopDate
        {
            get
            {
                return dStartDev.ToStringFromDate("dd/MM/yyyy", "th-TH") + " - " + dEndDev.ToStringFromDate("dd/MM/yyyy", "th-TH");
            }
        }
    }
    /// <summary>
    /// </summary>
    public class clsFilterProject
    {
        /// <summary>
        /// </summary>
        public int? nDataLength { get; set; }
        /// <summary>
        /// </summary>
        public int? nPageSize { get; set; }
        /// <summary>
        /// </summary>
        public int? nPageIndex { get; set; }
        /// <summary>
        /// </summary>
        public string? sSortExpression { get; set; }
        /// <summary>
        /// </summary>
        public string? sSortDirection { get; set; }
        /// <summary>
        /// </summary>
        public bool? isASC { get; }
        /// <summary>
        /// </summary>
        public bool? isDESC { get; }

        /// <summary>
        /// </summary>
        public string? sTask { get; set; }
        /// <summary>
        /// </summary>
        public string? sProject { get; set; }
        /// <summary>
        /// </summary>
        public string? sTester { get; set; }
        /// <summary>
        /// </summary>
        public string? sStartDate { get; set; }
        /// <summary>
        /// </summary>
        public string? sEndDate { get; set; }
    }
}
