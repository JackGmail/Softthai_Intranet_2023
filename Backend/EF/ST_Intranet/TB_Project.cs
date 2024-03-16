using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Project
{
    public int nProjectID { get; set; }

    public int? nParentID { get; set; }

    public string sProjectCode { get; set; } = null!;

    public string sProjectName { get; set; } = null!;

    public string? sProjectAbbr { get; set; }

    public string? sIntroduce { get; set; }

    public int nProjectTypeID { get; set; }

    public int? nProcessID { get; set; }

    public decimal? nProcessPlan { get; set; }

    public decimal? nProcessActual { get; set; }

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
    /// วันที่ลบรายการ
    /// </summary>
    public DateTime? dDelete { get; set; }

    /// <summary>
    /// ผู้ที่ลบรายการ
    /// </summary>
    public int? nDeleteBy { get; set; }

    /// <summary>
    /// true = ลบ , false = ยังไม่ถูกลบ
    /// </summary>
    public bool IsDelete { get; set; }
}
