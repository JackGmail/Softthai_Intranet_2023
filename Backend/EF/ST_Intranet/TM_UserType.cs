using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_UserType
{
    /// <summary>
    /// User Type ID
    /// </summary>
    public int nUserTypeID { get; set; }

    /// <summary>
    /// User Type Name
    /// </summary>
    public string sUserTypeName { get; set; } = null!;

    /// <summary>
    /// Sort Order
    /// </summary>
    public int? nSortOrder { get; set; }

    /// <summary>
    /// 1 = Active, 0 = Inactive
    /// </summary>
    public bool? IsActive { get; set; }
}
