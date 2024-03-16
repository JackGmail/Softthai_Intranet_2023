using Extensions.Common.STResultAPI;
using ST.INFRA;
using ST.INFRA.Common;

namespace Backend.Models.Back.Permission
{

    public class cResultOption : Extensions.Common.STResultAPI.ResultAPI
    {
        public List<cSelectOption>? lstUserType { get; set; }
        public List<cSelectOption>? lstUserGroup { get; set; }
    }


    public class UserPerModelRequest
    {
        public string sUserID { get; set; } = "";
    }
    /// <summary>
    /// AutoComplete
    /// </summary>
    public class cOptionAutoComplete
    {
        public string? value { get; set; }
        public string? label { get; set; }
        public string? sNameTH { get; set; }
        public string? sSureNameTH { get; set; }
        public string? sNameEN { get; set; }
        public string? sSureNameEN { get; set; }
        public string? sPosition { get; set; }
        public string? sDivision { get; set; }
        public string? sEmail { get; set; }
        public string? sPhoneNumber { get; set; }
    }
    /// <summary>
    /// requese Table
    /// </summary>
    public class cReqUserTable : STGrid.PaginationData
    {
        public string? sSearch { get; set; }
        public int? nStatus { get; set; }
    }
    /// <summary>
    /// Data Table
    /// </summary>
    public class cResultUserTable : Pagination
    {
        public List<ObjectUser>? lstData { get; set; }
    }
    /// <summary>
    /// Object Table
    /// </summary>
    public class ObjectUser
    {
        public string sID { get; set; } = string.Empty;
        public string nEmployeeID { get; set; } = string.Empty;
        public int? nNo { get; set; }
        public string? sName { get; set; }
        public string? sNameTH { get; set; }
        public string? sSurnameTH { get; set; }
        public string? sNameEN { get; set; }
        public string? sSurnameEN { get; set; }
        public string? sSureName { get; set; }
        public string? sGroup { get; set; }
        public DateTime dUpdate { get; set; }
        public string sUpdate
        {
            get
            {
                return dUpdate.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmmss, ST.INFRA.Enum.CultureName.th_TH);
            }
        }
        public int? nUpdateBy { get; set; }
        public string? sUpdateBy { get; set; }
        public bool IsActive { get; set; }
        public string sStatus
        {
            get
            {
                return IsActive ? "ใช้งาน" : "ไม่ใช้งาน";
            }
        }
        public int nPermission { get; set; }
        public List<TablePermission>? lstPermission { get; set; }
    }
    /// <summary>
    /// Object Form
    /// </summary>
    public class cReturnDataUserPerForm : Extensions.Common.STResultAPI.ResultAPI
    {
        public string? sUserType { get; set; }
        public string? sUserGroup{ get; set; }
        public objEmployee? objEmployeeID { get; set; }
        public string? sName { get; set; }
        public string? sSurename { get; set; }
        public string? sEmail { get; set; }
        public string? sTelephone { get; set; }
        public string? sPosition { get; set; }
        public bool IsActive { get; set; }
        public List<TablePermission>? lstPermission { get; set; }

    }
    /// <summary>
    /// ObjEmployeeID
    /// </summary>
    public class objEmployee
    {
        public string? value { get; set; }
        public string? label { get; set; }
    }
    /// <summary>
    /// Param SaveDataUser
    /// </summary>
    public class cResultDataUserPer : ResultAPI
    {
        public string? sEmpID { get; set; }
        public string? nUserGroup { get; set; }
        public bool IsActive { get; set; }
        public List<TablePermission>? lstPermission { get; set; }

    }
    /// <summary>
    /// List ID Requese Delete
    /// </summary>
    public class cReqDeleteUser
    {
        public List<string> lstDelete { get; set; } = new List<string>();
    }
    public class ClassTableUser
    {
        public string? sID { get; set; }
        public string nEmployeeID { get; set; } = "";
        public bool IsActive { get; set; }

    }
}
