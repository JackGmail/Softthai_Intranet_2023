using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_LoginType
{
    /// <summary>
    /// Login Type ID
    /// </summary>
    public int nLoginTypeID { get; set; }

    /// <summary>
    /// Login Type Name
    /// </summary>
    public string sLoginTypeName { get; set; } = null!;

    /// <summary>
    /// Url
    /// </summary>
    public string? sUrl { get; set; }

    /// <summary>
    /// 1 = Active, 0 = Inactive
    /// </summary>
    public bool? IsActive { get; set; }
}
