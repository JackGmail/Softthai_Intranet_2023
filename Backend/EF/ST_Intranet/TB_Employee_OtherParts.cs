using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_OtherParts
{
    public int nOtherPartsID { get; set; }

    /// <summary>
    /// รหัสตารางพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// อื่น ๆ ระบุ
    /// </summary>
    public string? sOther { get; set; }

    public string? sSourcesofJob { get; set; }

    /// <summary>
    /// โรคร้ายแรง  true = เคย , false = ไม่เคย
    /// </summary>
    public bool IsSeriousDisease { get; set; }

    /// <summary>
    /// ถ้าเคย ระบุชื่อโรค
    /// </summary>
    public string? sNameDisease { get; set; }

    /// <summary>
    /// เคยสมัครงานมาก่อน true = เคย , false = ไม่เคย
    /// </summary>
    public bool IsApplyforWork { get; set; }

    /// <summary>
    /// วันที่ที่เคยสมัครงาน
    /// </summary>
    public DateTime? dWhen { get; set; }

    /// <summary>
    /// แนะนำตัว
    /// </summary>
    public string? sIntroduce { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
