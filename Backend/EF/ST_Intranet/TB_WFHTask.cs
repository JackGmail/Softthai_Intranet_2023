using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

/// <summary>
/// ตารางเก็บ Task WFH
/// </summary>
public partial class TB_WFHTask
{
    /// <summary>
    /// รหัสตาราง
    /// </summary>
    public int nTaskID { get; set; }

    /// <summary>
    /// รหัสตาราง
    /// </summary>
    public int? nWFHID { get; set; }

    /// <summary>
    /// อ้างอิง TM_Data Column nTypeID = 31
    /// </summary>
    public int nPlanType { get; set; }

    public int nPlanID { get; set; }

    /// <summary>
    /// ลำดับรายการ
    /// </summary>
    public int nOrder { get; set; }

    /// <summary>
    /// วันที่สร้าง
    /// </summary>
    public DateTime dCreate { get; set; }

    /// <summary>
    /// รหัสผู้สร้าง
    /// </summary>
    public int nCreateBy { get; set; }

    /// <summary>
    /// วันที่แก้ไข
    /// </summary>
    public DateTime dUpdate { get; set; }

    /// <summary>
    /// รหัสผู้ที่แก้ไข
    /// </summary>
    public int nUpdateBy { get; set; }

    /// <summary>
    /// true = รายการถูกลบ , false = รายการยังไม่ถูกลบ
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// วันที่ลบ
    /// </summary>
    public DateTime? dDelete { get; set; }

    /// <summary>
    /// รหัสผู้ลบ
    /// </summary>
    public int? nDeleteBy { get; set; }

    public virtual TB_WFH? nWFH { get; set; }
}
