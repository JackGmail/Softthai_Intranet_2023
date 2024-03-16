using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_ConfigType
{
    /// <summary>
    /// Config Type ID
    /// </summary>
    public int nConfigTypeID { get; set; }

    /// <summary>
    /// Config Type Name
    /// </summary>
    public string? sConfigTypeName { get; set; }

    /// <summary>
    /// Sort Order
    /// </summary>
    public int? nSortOrder { get; set; }

    /// <summary>
    /// Is set Admin digital can mangement  ? ture,false
    /// </summary>
    public bool IsManageByUser { get; set; }

    /// <summary>
    /// 1 = Active, 0 = Inactive
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Parent ID
    /// </summary>
    public int? nParentID { get; set; }

    public virtual ICollection<TM_Config> TM_Config { get; set; } = new List<TM_Config>();
}
