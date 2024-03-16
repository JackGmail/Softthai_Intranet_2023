using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_DataType
{
    public int nDatatype_ID { get; set; }

    public string? sNameTH { get; set; }

    public string? sNameEng { get; set; }

    public string? sDescription { get; set; }

    public bool IsAdd { get; set; }

    public bool IsOrder { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public bool IsAllowManage { get; set; }

    public bool IsHasParent { get; set; }

    public int? nParentDataTypeID { get; set; }
}
