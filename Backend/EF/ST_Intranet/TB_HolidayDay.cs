using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_HolidayDay
{
    public int nHolidayDayID { get; set; }

    public int nYear { get; set; }

    /// <summary>
    /// วันที่
    /// </summary>
    public DateTime dDate { get; set; }

    /// <summary>
    /// ชื่อวันหยุด
    /// </summary>
    public string sHolidayName { get; set; } = null!;

    /// <summary>
    /// คำอธิบาย
    /// </summary>
    public string? sDrescription { get; set; }

    /// <summary>
    /// เป็นวันหยุดกิจกรรม true = ใช่ , false = ไม่ใช่
    /// </summary>
    public bool IsActivity { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
