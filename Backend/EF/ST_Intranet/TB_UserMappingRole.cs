using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_UserMappingRole
{
    /// <summary>
    /// Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// Refer.TM_UserRole
    /// </summary>
    public int nUserRoleID { get; set; }
}
