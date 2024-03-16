using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_UserMappingGroup
{
    public int nEmployeeID { get; set; }

    /// <summary>
    /// Refer.TM_UserGroup
    /// </summary>
    public int nUserGroupID { get; set; }
}
