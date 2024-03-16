using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Request_WF_Privatecar_File
{
    public int nRequestFileID { get; set; }

    /// <summary>
    /// รหัสใบคำขอ
    /// </summary>
    public int nRequestPrivatecarID { get; set; }

    public string sPath { get; set; } = null!;

    public string sSystemFilename { get; set; } = null!;

    public string sFilename { get; set; } = null!;

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsDelete { get; set; }
}
