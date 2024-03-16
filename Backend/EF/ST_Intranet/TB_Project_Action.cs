using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Project_Action
{
    public int nActionID { get; set; }

    public int nPlanID { get; set; }

    public int nEmployeeID { get; set; }

    public decimal nManhour { get; set; }

    public int nCalculateTypeID { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
