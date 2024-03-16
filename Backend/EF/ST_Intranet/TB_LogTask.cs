using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_LogTask
{
    /// <summary>
    /// Log ID
    /// </summary>
    public int nLogID { get; set; }

    /// <summary>
    /// Task Name
    /// </summary>
    public string sTaskName { get; set; } = null!;

    /// <summary>
    /// Start Date
    /// </summary>
    public DateTime dStartDate { get; set; }

    /// <summary>
    /// End Date
    /// </summary>
    public DateTime? dEndDate { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? sDescription { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public string sStatus { get; set; } = null!;

    /// <summary>
    /// Erroe Message
    /// </summary>
    public string? sErrorMsg { get; set; }
}
