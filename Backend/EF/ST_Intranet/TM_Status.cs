using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_Status
{
    /// <summary>
    /// รหัสสถานะ
    /// </summary>
    public int nStatusID { get; set; }

    /// <summary>
    /// รหัสประเภทใบคำขอ Refer.TM_RequestType
    /// </summary>
    public int nRequestTypeID { get; set; }

    /// <summary>
    /// ชื่อสถานะ
    /// </summary>
    public string? sStatusName { get; set; }

    /// <summary>
    /// สถานะต่อไป
    /// </summary>
    public int? nNextStatusID { get; set; }

    public string? sNextStatusName { get; set; }

    public string? sDescription { get; set; }

    public int? nOrder { get; set; }
}
