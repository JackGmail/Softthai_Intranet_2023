using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_RequestType
{
    /// <summary>
    /// รหัสประเภทใบคำขอ
    /// </summary>
    public int nRequestTypeID { get; set; }

    /// <summary>
    /// ชื่อประเภทใบคำขอ
    /// </summary>
    public string nRequestTypeName { get; set; } = null!;

    /// <summary>
    /// รายละเอียดประเภทใบคำขอ
    /// </summary>
    public string? sDrescription { get; set; }

    public bool IsActive { get; set; }
}
