using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_Subdistrict
{
    public string sSubDistrictID { get; set; } = null!;

    public int nZip_code { get; set; }

    public string sSubdistrictNameTH { get; set; } = null!;

    public string sSubdistrictNameEN { get; set; } = null!;

    public int nDistrictID { get; set; }
}
