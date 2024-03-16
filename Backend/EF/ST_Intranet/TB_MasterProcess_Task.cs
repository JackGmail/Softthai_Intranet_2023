using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_MasterProcess_Task
{
    public int nMasterProcessTaskID { get; set; }

    public int nMasterProcessID { get; set; }

    public string sMasterProcessTaskName { get; set; } = null!;

    public int nOrder { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsDelete { get; set; }
}
