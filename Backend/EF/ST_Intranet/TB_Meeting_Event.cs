using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Meeting_Event
{
    public long nEventID { get; set; }

    public int nMeetingID { get; set; }

    public DateTime dEventStart { get; set; }

    public DateTime dEventEnd { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsDelete { get; set; }
}
