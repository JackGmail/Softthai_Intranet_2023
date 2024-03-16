using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_LeaveChange
{
    public int nID { get; set; }

    public int nLeaveTypeID { get; set; }

    public int nEmployeeID { get; set; }

    public DateTime dWorkStart { get; set; }

    public DateTime dWorkNow { get; set; }

    public decimal nQuantity { get; set; }

    public decimal nIntoMoney { get; set; }

    public decimal? nLeaveRemain { get; set; }

    public int nYear { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
