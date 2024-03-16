using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Project_Plan
{
    public int nPlanID { get; set; }

    public int nProjectID { get; set; }

    public int? nParentID { get; set; }

    public string? sParentID { get; set; }

    public string? sChildID { get; set; }

    public string? sDetail { get; set; }

    public int nTypeID { get; set; }

    public int? nTypeTaskID { get; set; }

    public int nProposalID { get; set; }

    public DateTime? dPlanStart { get; set; }

    public DateTime? dPlanEnd { get; set; }

    public decimal? nPlanManhour { get; set; }

    public decimal? nDevelopManhour { get; set; }

    public decimal? nTesterManhour { get; set; }

    public decimal? nPlanProgress { get; set; }

    public decimal? nDevelopProgress { get; set; }

    public decimal? nTesterProgress { get; set; }

    public decimal? nActualProgress { get; set; }

    public decimal? nWeight { get; set; }

    public decimal? nWeightPlan { get; set; }

    public int nLevel { get; set; }

    public int nPlanStatusID { get; set; }

    public bool IsDelayPlan { get; set; }

    public bool IsDelayManhour { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public int nPlanTypeID { get; set; }

    public int nProgressTypeID { get; set; }
}
