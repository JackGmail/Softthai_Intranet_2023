using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Meeting_Flow
{
    public int nMeetingFlowID { get; set; }

    public int nMeetingID { get; set; }

    public int nStatusID { get; set; }

    public string? sRemark { get; set; }

    public DateTime dAction { get; set; }

    public int nActionBy { get; set; }
}
