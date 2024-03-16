using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Floor
{
    public int nFloorID { get; set; }

    public string? sFloorName { get; set; }

    /// <summary>
    /// จำนวนคนที่รองรับ
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// ลำดับรายการ
    /// </summary>
    public int nOrder { get; set; }

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
}
