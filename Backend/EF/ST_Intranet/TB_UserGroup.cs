using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_UserGroup
{
    public int nUserGroupID { get; set; }

    /// <summary>
    /// Refer. TM_UserRole
    /// </summary>
    public int nUserRoleID { get; set; }

    public string sUserGroupName { get; set; } = null!;

    public string? sUserGroupDescription { get; set; }

    public int? nOrder { get; set; }

    public bool IsActive { get; set; }

    /// <summary>
    /// วันที่สร้างรายการ
    /// </summary>
    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    /// <summary>
    /// true = ลบ , false = ยังไม่ถูกลบ
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// วันที่ลบรายการ
    /// </summary>
    public DateTime? dDelete { get; set; }

    /// <summary>
    /// ผู้ที่ลบรายการ
    /// </summary>
    public int? nDeleteBy { get; set; }

    public bool IsCanDelete { get; set; }
}
