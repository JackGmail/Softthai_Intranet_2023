using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_HolidayYear
{
    public int nHolidayYearID { get; set; }

    /// <summary>
    /// ปี
    /// 
    /// </summary>
    public int nYear { get; set; }

    /// <summary>
    /// จำนวนวันหยุดประจำปี (วัน)
    /// </summary>
    public decimal nHoliday { get; set; }

    /// <summary>
    /// จำนวนกิจกรรมประจำปี (วัน)
    /// </summary>
    public decimal nActivity { get; set; }

    public int nOrder { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
