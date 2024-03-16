using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_Image
{
    public int nEmployeeImageID { get; set; }

    public int nEmployeeID { get; set; }

    /// <summary>
    /// รหัสประเภทรูป TM_Data.nDatatype_ID = 24
    /// </summary>
    public int? nImageType { get; set; }

    public string? sPath { get; set; }

    /// <summary>
    /// ชื่อใหม่
    /// </summary>
    public string? sSystemFileName { get; set; }

    /// <summary>
    /// ชื่อเก่า
    /// </summary>
    public string? sFileName { get; set; }

    public int? nOrder { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
