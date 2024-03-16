using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_BannerType_Test
{
    public int nBannerTypeID { get; set; }

    public string sBannerTypeName { get; set; } = null!;

    public int? nOrder { get; set; }

    public bool IsActive { get; set; }
}
