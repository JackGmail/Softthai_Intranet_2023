using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Banner
{
    public int nBannerID { get; set; }

    public string sBannerName { get; set; } = null!;

    public string? sNote { get; set; }

    public DateTime? sStartDate { get; set; }

    public DateTime? sEndDate { get; set; }

    public bool IsSetDate { get; set; }

    public bool IsActive { get; set; }

    public string sPath { get; set; } = null!;

    public string sSystemFileName { get; set; } = null!;

    public string sFileName { get; set; } = null!;

    public int nOrder { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsDelete { get; set; }
}
