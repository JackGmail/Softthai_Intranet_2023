using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_District
{
    public int nDistrictID { get; set; }

    public string sDistrictNameTH { get; set; } = null!;

    public string sDistrictNameEN { get; set; } = null!;

    public int nProvinceID { get; set; }
}
