using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_WF_Dental_File
{
    /// <summary>
    /// รหัสไฟล์เอกสารท้นตกรรม
    /// </summary>
    public int nDentalFileID { get; set; }

    /// <summary>
    /// รหัสใบคำขอเบิกค่าท้นตกรรม (TB_Request_WF_Dental)
    /// </summary>
    public int nRequestDentalID { get; set; }

    public string sPath { get; set; } = null!;

    public string sSystemFilename { get; set; } = null!;

    public string sFilename { get; set; } = null!;

    public int nOrder { get; set; }

    public string dCreate { get; set; } = null!;

    public string nCreateBy { get; set; } = null!;

    public string dUpdate { get; set; } = null!;

    public string nUpdateBy { get; set; } = null!;

    public string? dDelete { get; set; }

    public string? nDeleteBy { get; set; }

    public string IsDelete { get; set; } = null!;
}
