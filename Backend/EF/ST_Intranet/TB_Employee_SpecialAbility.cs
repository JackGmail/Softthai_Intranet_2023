using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_SpecialAbility
{
    public int nSpecialAbilityID { get; set; }

    /// <summary>
    /// รหัสตารางพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// ประเภทความสามารถพิเศษ Refer.TM_Data
    /// </summary>
    public int nSpecialAbilityTypeID { get; set; }

    public int? nOrder { get; set; }

    public string? sOther { get; set; }

    /// <summary>
    /// TM_Data.nData_ID = 93 ได้  TM_Data.nData_ID = 97 ไม่ได้
    /// </summary>
    public int? nCan { get; set; }

    public string? sDrescription { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
