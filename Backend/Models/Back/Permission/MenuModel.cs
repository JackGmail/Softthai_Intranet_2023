namespace Backend.Models.Back.Permission
{
    #region  
    public class MenuAllModel
    {
        public string sID { get; set; } = "";
        public int nMenuID { get; set; }
        public int? nParentID { get; set; }
        public string? sMenuName { get; set; }
        public bool isFontEnd { get; set; }
        public bool IsSetPRMS { get; set; }
        public string? sIcon { get; set; }
        public string? sRoute { get; set; }
        public bool? IsActive { get; set; }
        public int? nLevel { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsView { get; set; }
        public decimal? nOrder { get; set; }
        public bool IsShowBreadcrumb { get; set; }
        public bool IsManage { get; set; }
        public bool IsDisable { get; set; }
        public bool IsSetPermission { get; set; }
        public int nMenuType { get; set; }
        public List<MenuAllModel>? lstSubMenu { get; set; }
    }
    public class MenuModel
    {
        public string sID { get; set; } = "";
        public int nMenuID { get; set; }
        public int? nParentID { get; set; }
        public string? sMenuName { get; set; }
        public bool isFontEnd { get; set; }
        public bool IsSetPRMS { get; set; }
        public string? sIcon { get; set; }
        public string? sRoute { get; set; }
        public bool? IsActive { get; set; }
        public int? nLevel { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsView { get; set; }
        public decimal? nOrder { get; set; }
        public bool IsShowBreadcrumb { get; set; }
        public bool IsManage { get; set; }
        public bool IsDisable { get; set; }
        public bool IsSetPermission { get; set; }
        public int nMenuType { get; set; }
    }


    #endregion
}