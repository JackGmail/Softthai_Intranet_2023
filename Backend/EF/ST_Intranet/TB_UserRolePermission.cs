using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_UserRolePermission
{
    /// <summary>
    /// Refer.TM_UserRole
    /// </summary>
    public int nUserRoleID { get; set; }

    /// <summary>
    /// Refer.TM_Menu
    /// </summary>
    public int nMenuID { get; set; }

    /// <summary>
    /// Permission 2=Full Control, 1=Read Only, 0=Disable
    /// </summary>
    public int nPermission { get; set; }
}
