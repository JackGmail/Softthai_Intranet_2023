using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Project_UpdateProgress
{
    public int nTaskID { get; set; }

    public int nPlanID { get; set; }

    public int nEmployeeID { get; set; }

    public DateTime dAction { get; set; }

    public decimal nManhour { get; set; }

    public decimal nProgress { get; set; }

    public string? sDescription { get; set; }

    public string? sDelayDescription { get; set; }

    public int nCalculateTypeID { get; set; }

    public int nWorkTypeID { get; set; }

    public int nWorkPlaceTypeID { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
