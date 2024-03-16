using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Request_OT
{
    public int nRequestOTID { get; set; }

    /// <summary>
    /// รหัสประเภทใบคำขอ (TM_RequestType)
    /// </summary>
    public int nRequestTypeID { get; set; }

    /// <summary>
    /// 0 = Normal, 1 = Holiday
    /// </summary>
    public bool IsHoliday { get; set; }

    public int nProjectID { get; set; }

    public int nProcessID { get; set; }

    public int nApproveBy { get; set; }

    public string sTopic { get; set; } = null!;

    public DateTime dPlanDateTime { get; set; }

    public decimal nEstimateHour { get; set; }

    public DateTime? dStartActionDateTime { get; set; }

    public DateTime? dEndActionDateTime { get; set; }

    /// <summary>
    /// 0 = Request, 1 = Result
    /// </summary>
    public int? nActionHour { get; set; }

    public int nStepID { get; set; }

    public int nStatusID { get; set; }

    public string? sNoteApprover { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsDelete { get; set; }
}
