using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_Data
{
    public int nData_ID { get; set; }

    public int nDatatypeID { get; set; }

    public string? sNameTH { get; set; }

    public string? sNameEng { get; set; }

    public string? sAbbr { get; set; }

    public int nOrder { get; set; }

    public int? nParentData_ID { get; set; }

    public string? sDescription { get; set; }

    public int nCreateBy { get; set; }

    public DateTime? dCraeteDate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dUpdate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }
}
