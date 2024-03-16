namespace Backend.Models.Authentication
{
    public class ParamJWT
    {
        public int nUserID { get; set; }
        public string sFullName { get; set; } = string.Empty;
        public string sImageUser { get; set; } = string.Empty;
        public string sFullNameTH { get; set; } = string.Empty;
        public string sPosition { get; set; } = string.Empty;
        public string sEmail { get; set; } = string.Empty;
        public List<int> lstUserPositionID { get; set; } = new List<int>();
        public List<int> lstUserRoleID { get; set; } = new List<int>();
        public List<int> lstUserGroupID { get; set; } = new List<int>();
        public List<MenuPermisson> lstMenuPrms { get; set; } = new List<MenuPermisson>();
    }
    public class UserAccount : ParamJWT
    {

    }

    public class ParamLogin
    {
        /// <summary>
        /// Username
        /// </summary>
        public string? sUsername { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        public string? sPWD { get; set; }
        /// <summary>
        /// Security Code
        /// </summary>
        public string? sSecureCode { get; set; }
        /// <summary>
        /// AD = Login AD
        /// </summary>
        public string? sMode { get; set; }
    }
    /// <summary>
    /// ส่งค่ากลับ
    /// </summary>
    public class ResultLogin
    {
        public string? sToken { get; set; }
        public string? sFullnameTH { get; set; }
        public string? sNickname { get; set; }
        public string? sPosition { get; set; }
        public string? sEmail { get; set; }
        public string? sGroup { get; set; }
    }

    public class ParamUserPermisson
    {
        public int? nUserID { get; set; }
        public List<int> lstUserRoleID { get; set; } = new List<int>();
        public List<int> lstUserGroupID { get; set; } = new List<int>();
    }
    public class ResultUserPermisson
    {
        public List<MenuPermisson> lstMenuPermission { get; set; } = new List<MenuPermisson>();

    }

    public class MenuPermisson
    {
        public int nMenuID { get; set; }
        public int nPermission { get; set; }
    }
    public class ParamMenuPermisson
    {
        public string sRoute { get; set; } = string.Empty;
    }
    public class ResultMenuPermisson
    {
        public int nPermission { get; set; } = 0;
    }
    public class AccountLine
    {
        public string? sUsername {get;set;}
        public string? sUserID { get; set; }
        public string? sGUID { get; set; }
    }
    public class STEnCrypt
    {
        public string? sValue { get; set; }
    }
}
