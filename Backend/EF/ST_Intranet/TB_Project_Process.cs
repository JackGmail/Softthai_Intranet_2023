using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Project_Process
{
    public int nProcessID { get; set; }

    public int nMasterProcessID { get; set; }

    public int nProjectID { get; set; }

    public decimal nManhour { get; set; }

    /// <summary>
    /// Require Manhour
    /// </summary>
    public bool IsManhour { get; set; }

    /// <summary>
    /// true = ใช้งาน , false = ไม่ใช้งาน
    /// </summary>
    public bool IsActive { get; set; }

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

    public virtual ICollection<TB_Project_ProcessManday> TB_Project_ProcessManday { get; set; } = new List<TB_Project_ProcessManday>();
}
