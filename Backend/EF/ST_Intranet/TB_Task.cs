using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Task
{
    public int nTaskID { get; set; }

    public int? nParentID { get; set; }

    public int nProjectID { get; set; }

    /// <summary>
    /// 1 = Task , 2 = WFH
    /// </summary>
    public int nTypeRequest { get; set; }

    public DateTime dTask { get; set; }

    public string sDescription { get; set; } = null!;

    public decimal? nPlan { get; set; }

    public decimal? nPlanProcess { get; set; }

    public decimal? nActual { get; set; }

    public decimal? nActualProcess { get; set; }

    public string? sDescriptionDelay { get; set; }

    public int nTaskTypeID { get; set; }

    public int nTaskStatusID { get; set; }

    public int nEmployeeID { get; set; }

    public int? nProgressID { get; set; }

    public int nPlanTypeID { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsComplete { get; set; }
}
