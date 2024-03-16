using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_Provinces
{
    public int nProvinceID { get; set; }

    public string? sProvinceNameTH { get; set; }

    public string? sProvinceShort { get; set; }

    public string? sProvinceNameEN { get; set; }

    public int? geography_id { get; set; }
}
