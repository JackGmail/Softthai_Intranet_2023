using System.Text.Json;
using Backend.Models.Back.Permission;
using Extensions.Common.STResultAPI;

namespace Backend.Models
{
    //    /// <summary>
    //    /// ItemBreadCrumb
    //    /// </summary>
    //    public class cItemBreadCrumb
    //    {
    //        /// <summary>
    //        /// Key
    //        /// </summary>
    //        public string? Key { get; set; }
    //        /// <summary>
    //        /// Item Name
    //        /// </summary>
    //        public string? ItemName { get; set; }
    //        /// <summary>
    //        /// Icon
    //        /// </summary>
    //        public string? sIcon { get; set; }
    //        /// <summary>
    //        /// URL
    //        /// </summary>
    //        public string? sURL { get; set; }
    //        /// <summary>
    //        /// Level
    //        /// </summary>
    //        public int? nLevel { get; set; }
    //        /// <summary>
    //        /// Show in Display
    //        /// </summary>
    //        public bool IsDisplay { get; set; }
    //    }

    //    /// <summary>
    //    /// Bredcrumb
    //    /// </summary>
    //    public class clsBredcrumb : ResultAPI
    //    {
    //        /// <summary>
    //        /// lstBremcrumb
    //        /// </summary>
    //        public List<cItemBreadCrumb>? lstBremcrumb { get; set; }

    //    }

    //    /// <summary>
    //    /// Bredcrumb
    //    /// </summary>
    //    public class ResponseMenu : ResultAPI
    //    {
    //        /// <summary>
    //        /// List Menu
    //        /// </summary>
    //        public List<cItemMenu>? lstMenu { get; set; }

    //    }

    //    /// <summary>
    //    /// ItemMenu
    //    /// </summary>
    //    public class cItemMenu
    //    {
    //        /// <summary>
    //        /// Key
    //        /// </summary>
    //        public string? sKey { get; set; }
    //        /// <summary>
    //        /// Menu ID
    //        /// </summary>
    //        public string? sMenuID { get; set; }
    //        /// <summary>
    //        /// Menu Name
    //        /// </summary>
    //        public string? sMenuName { get; set; }
    //        /// <summary>
    //        /// URL
    //        /// </summary>
    //        public string? sURL { get; set; }
    //        /// <summary>
    //        /// Icon
    //        /// </summary>
    //        public string? sIcon { get; set; }
    //        /// <summary>
    //        /// Level
    //        /// </summary>
    //        public int? nLevel { get; set; }
    //        /// <summary>
    //        /// Show in Display
    //        /// </summary>
    //        public bool IsDisplay { get; set; }
    //        /// <summary>
    //        /// List Children
    //        /// </summary>
    //        public List<cItemMenu>? lstChildren { get; set; }
    //        public string? sParentID { get; set; }
    //        public int nMenuID { get; set; }
    //        public int? nParentID { get; set; }
    //    }

    public class TablePermission
    {
        public int nNo { get; set; }
        public int nOrder { get; set; }
        public int nMenuID { get; set; }
        public int? nMenuHeadID { get; set; }
        public int? nHighPerm { get; set; }
        public string? sID { get; set; }
        public string? sMenuName { get; set; }
        public bool isHasSub { get; set; }
        public bool isSubMenu { get; set; }
        public bool isFontEnd { get; set; }
        public bool isManagePRM { get; set; }
        public bool isDisable { get; set; }
        public bool isReadOnly { get; set; }
        public bool isEnable { get; set; }
        public int? nPermission { get; set; }
        public int? nDisabledMode { get; set; }
        public List<MenuAllModel>? lstSubMenu { get; set; }
    }
    public class cPermission : ResultAPI
    {
        public List<TablePermission>? lstPermission { get; set; }
    }

}
