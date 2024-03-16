using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Project_ProcessManday
{
    public int nMandayID { get; set; }

    public int nProcessID { get; set; }

    public int nPositionID { get; set; }

    public decimal? nManday { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public virtual TB_Project_Process nProcess { get; set; } = null!;
}
