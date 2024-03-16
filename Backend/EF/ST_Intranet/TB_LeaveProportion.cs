using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_LeaveProportion
{
    public int LeaveProportionID { get; set; }

    /// <summary>
    /// Refer. TB_LeaveType
    /// </summary>
    public int nLeaveTypeID { get; set; }

    /// <summary>
    /// ประเภทพนักงาน
    /// </summary>
    public int nEmployeeTypeID { get; set; }

    public decimal nJan { get; set; }

    public decimal nFeb { get; set; }

    public decimal nMar { get; set; }

    public decimal nApr { get; set; }

    public decimal nMay { get; set; }

    public decimal nJun { get; set; }

    public decimal nJul { get; set; }

    public decimal nAug { get; set; }

    public decimal nSep { get; set; }

    public decimal nOct { get; set; }

    public decimal nNov { get; set; }

    public decimal nDec { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
