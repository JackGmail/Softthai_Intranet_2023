using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Image_Leave
{
    public int nImageID { get; set; }

    /// <summary>
    /// ประเภทการลา
    /// </summary>
    public int nLeaveTypeID { get; set; }

    /// <summary>
    /// รูปภาพ/parth
    /// </summary>
    public string sImageParh { get; set; } = null!;

    /// <summary>
    /// ชื่อใหม่
    /// </summary>
    public string sSystemName { get; set; } = null!;

    /// <summary>
    /// ชื่อเก่า
    /// </summary>
    public string sExpireName { get; set; } = null!;

    public int? nOrder { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
