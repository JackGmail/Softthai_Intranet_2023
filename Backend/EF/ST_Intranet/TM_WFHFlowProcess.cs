using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

/// <summary>
/// ตารางเก็บ WorkFlow WFH
/// </summary>
public partial class TM_WFHFlowProcess
{
    /// <summary>
    /// รหัสตาราง
    /// </summary>
    public int nFlowProcessID { get; set; }

    /// <summary>
    /// ชื่อ Process Flow
    /// </summary>
    public string sProcess { get; set; } = null!;

    /// <summary>
    /// ชื่อ Action
    /// </summary>
    public string sAction { get; set; } = null!;

    /// <summary>
    /// ลำดับรายการ
    /// </summary>
    public int nOrder { get; set; }

    /// <summary>
    /// true = มีการใช้งาน , false ไม่มีการใช้งาน
    /// </summary>
    public bool IsActive { get; set; }
}
