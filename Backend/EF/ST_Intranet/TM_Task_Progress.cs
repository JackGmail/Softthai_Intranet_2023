using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_Task_Progress
{
    public int nProgressID { get; set; }

    public string? sProgressName { get; set; }

    public string? sDescription { get; set; }

    /// <summary>
    /// true = StandProcess , false = OtherProcess
    /// </summary>
    public bool IsRequiredDesc { get; set; }

    /// <summary>
    /// true = StandProcess , false = OtherProcess
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// วันที่สร้างรายการ
    /// </summary>
    public DateTime dCreate { get; set; }

    /// <summary>
    /// ผู้สร้างรายการ
    /// </summary>
    public int nCreateBy { get; set; }

    /// <summary>
    /// วันที่แก้ไขรายการ
    /// </summary>
    public DateTime dUpdate { get; set; }

    /// <summary>
    /// ผู้แก้ไขรายการ
    /// </summary>
    public int nUpdateBy { get; set; }

    /// <summary>
    /// true = ลบ , false = ยังไม่ถูกลบ
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// วันที่ลบรายการ
    /// </summary>
    public DateTime? dDelete { get; set; }

    /// <summary>
    /// ผู้ที่ลบรายการ
    /// </summary>
    public int? nDeleteBy { get; set; }

    public int nOrder { get; set; }
}
