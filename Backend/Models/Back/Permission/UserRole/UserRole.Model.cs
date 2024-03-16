
using Extensions.Common.STResultAPI;
using ST.INFRA;
using ST.INFRA.Common;

namespace Backend.Models
{
    public class cUserRoleTable : STGrid.PaginationData
    {
        public int? nStatus { get; set; }
        public string? sUserRoleName { get; set; }
        public List<string> lstRemove { get; set; } = new List<string>();
    }
    public class cResultUserRoleTable : Pagination
    {
        public List<ObjectResultUserRole>? lstData { get; set; }
    }
    public class ObjectResultUserRole
    {
        public int? No { get; set; }
        public int nID { get; set; }
        public string? sID { get; set; }
        public string? sUserRoleName { get; set; }
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
        public bool? IsCanDelete { get; set; }
        public bool IsActive { get; set; }
        public string sActive
        {
            get
            {
                return IsActive ? "ใช้งาน" : "ไม่ใช้งาน";
            }
        }
        public int nPermission { get; set; }
        public List<TablePermission>? lstPermission { get; set; }
    }
    public class CUserRoleData : ResultAPI
    {
        /// <summary>
        /// </summary>
        public string? sID { get; set; } = "";
    }

    public class UserRoleModelRequest
    {
        public string sRoleID { get; set; } = "";
    }
    public class UserRoleModel : ResultAPI
    {
        //public string sID { get; set; } 
        public string? sRoleID { get; set; }
        public string? sUserRoleName { get; set; }
        public string? sDrescription { get; set; }
        public bool? IsActive { get; set; }
        public List<TablePermission>? lstPermission { get; set; }
    }
    #region  Group
    public class cResultUserGroup : ResultAPI
    {
        public List<cSelectOption>? lstRoles { get; set; }
        public List<cSelectOption>? lstRolesSH { get; set; }
    }
    public class cUserGroupTable : STGrid.PaginationData
    {
        public int? nStatus { get; set; }
        public string? sGroup { get; set; }
        public int? nRoleID { get; set; }
        public List<string> lstRemove { get; set; } = new List<string>();
    }
    public class cResultUserGroupTable : Pagination
    {
        public List<ObjectResultUserGroup>? lstData { get; set; }
    }
    public class ObjectResultUserGroup
    {
        public int? No { get; set; }
        public int nID { get; set; }
        public string? sID { get; set; }
        public string? sGroup { get; set; }
        public string? sRole { get; set; }
        public int? nRoleID { get; set; }
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
        public bool? IsCanDelete { get; set; }
        public bool IsStatus { get; set; }
        public string sStatus
        {
            get
            {
                return IsStatus ? "ใช้งาน" : "ไม่ใช้งาน";
            }
        }
        public int nPermission { get; set; }
        public List<TablePermission>? lstPermission { get; set; }
    }
    public class UserGroupModelRequest
    {
        public string sUserGroupID { get; set; } = "";
    }
    public class UserGroupModel : ResultAPI
    {
        public string? sUserGroupID { get; set; }
        public string? sRoleID { get; set; }
        public string sGroup { get; set; }
        public string? sDescription { get; set; }
        public bool? IsActive { get; set; }
        public List<TablePermission>? lstPermission { get; set; }
    }

    public class cUserGroupOrder
    {
        public string? sID { get; set; }
        public string? sSubProcessID { get; set; }
        public string? sMasterProcessName { get; set; }
        public int nOrder { get; set; }

    }

    public class ClassTableUserRole : ResultAPI
    {
        public string? sID { get; set; }
        public int? nUserRoleID { get; set; }
        public bool IsActive { get; set; }

    }
    public class ClassTableGroup : ResultAPI
    {
        public string? sID { get; set; }
        public int? nUserGroupID { get; set; }
        public bool IsActive { get; set; }

    }
    public class UserModelRequest
    {
        public string sGroupID { get; set; } = "";
    }
    #endregion
}
