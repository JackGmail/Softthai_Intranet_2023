using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_TimeStemp
{
    public int nTimeStampID { get; set; }

    public int nEmployeeID { get; set; }

    public string? sLocation { get; set; }

    public DateTime dTimeDate { get; set; }

    public DateTime dTimeStartDate { get; set; }

    public DateTime? dTimeEndDate { get; set; }

    public bool IsDelay { get; set; }

    public int nMinute { get; set; }

    public int nMinuteByHR { get; set; }

    public string? sComment { get; set; }
}
