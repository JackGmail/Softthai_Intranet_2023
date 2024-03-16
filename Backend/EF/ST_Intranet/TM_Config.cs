using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_Config
{
    /// <summary>
    /// Config ID
    /// </summary>
    public int nConfigID { get; set; }

    /// <summary>
    /// Config Type ID Ref. Table: TM_ConfigType
    /// </summary>
    public int nConfigTypeID { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string sConfigName { get; set; } = null!;

    /// <summary>
    /// String Value
    /// </summary>
    public string? sValue { get; set; }

    /// <summary>
    /// Number Value
    /// </summary>
    public decimal? nValue { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? sDescription { get; set; }

    /// <summary>
    /// Sort Order
    /// </summary>
    public int? nSortOrder { get; set; }

    public virtual TM_ConfigType nConfigType { get; set; } = null!;
}
