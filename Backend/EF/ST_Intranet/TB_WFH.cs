using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_WFH
{
    public int nWFHID { get; set; }

    public DateTime dWFH { get; set; }

    public decimal nManhour { get; set; }

    public int nApproveBy { get; set; }

    public bool? IsEmergency { get; set; }

    public bool IsOnsite { get; set; }

    public int nFlowProcessID { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public virtual ICollection<TB_WFHFlow> TB_WFHFlow { get; set; } = new List<TB_WFHFlow>();

    public virtual ICollection<TB_WFHFlowHistory> TB_WFHFlowHistory { get; set; } = new List<TB_WFHFlowHistory>();

    public virtual ICollection<TB_WFHTask> TB_WFHTask { get; set; } = new List<TB_WFHTask>();
}
