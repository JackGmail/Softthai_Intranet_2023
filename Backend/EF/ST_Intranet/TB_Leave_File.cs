using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Leave_File
{
    public int nLeaveFileID { get; set; }

    /// <summary>
    /// รหัสใบคำขอ
    /// </summary>
    public int nRequestLeaveID { get; set; }

    public string sPath { get; set; } = null!;

    public string sSystemFilename { get; set; } = null!;

    public string sFilename { get; set; } = null!;

    public int nOrder { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsDelete { get; set; }
}
