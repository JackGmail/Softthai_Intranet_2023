using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_Allergy
{
    public int nAllergyID { get; set; }

    public int nEmployeeID { get; set; }

    /// <summary>
    /// ประเภทการแพ้/โรคประจำตัว
    /// </summary>
    public int nAllergyType { get; set; }

    /// <summary>
    /// ชื่อ
    /// </summary>
    public string nAllergyName { get; set; } = null!;

    /// <summary>
    /// อาการ
    /// </summary>
    public string? nDrescription { get; set; }

    public int nOrder { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
