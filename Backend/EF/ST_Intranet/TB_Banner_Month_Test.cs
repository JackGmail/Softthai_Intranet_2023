using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Banner_Month_Test
{
    public int nBannerMonthID { get; set; }

    public int nBannerID { get; set; }

    public int nMonth { get; set; }

    public int nValue { get; set; }

    public DateTime dCreateDate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdateDate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }
}
