using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_Menu
{
    /// <summary>
    /// PK TM_Menu
    /// </summary>
    public int nMenuID { get; set; }

    /// <summary>
    /// Parent MenuID , Refer Table TM_Menu
    /// </summary>
    public int? nParentID { get; set; }

    /// <summary>
    /// 1 = Frontend , 2 = Backend
    /// </summary>
    public int nMenuType { get; set; }

    /// <summary>
    /// ชื่อเมนู
    /// </summary>
    public string sMenuName { get; set; } = null!;

    public int nLevel { get; set; }

    public string sRoute { get; set; } = null!;

    public string? sIcon { get; set; }

    /// <summary>
    /// 1 = แสดงที่เมนู Bar , 0 = ไม่แสดงที่เมนู Bar
    /// </summary>
    public bool IsDisplay { get; set; }

    /// <summary>
    /// 1 = แสดงให้กำหนด Permission , 0 = ไม่แสดงให้กำหนด Permission 
    /// </summary>
    public bool IsSetPermission { get; set; }

    /// <summary>
    /// 1 = แสดงให้กำหนด Permission Disable , 0 = ไม่แสดงให้กำหนด Permission Disable
    /// </summary>
    public bool IsDisable { get; set; }

    /// <summary>
    /// 1 = แสดงให้กำหนด Permission View , 0 = ไม่แสดงให้กำหนด Permission View
    /// </summary>
    public bool IsView { get; set; }

    /// <summary>
    /// 1 = แสดงให้กำหนด Permission Manage , 0 = ไม่แสดงให้กำหนด Permission Manage
    /// </summary>
    public bool IsManage { get; set; }

    /// <summary>
    /// 1 = แสดงใน Breadcrum , 0 = ไม่แสดงใน Breadcrum
    /// </summary>
    public bool IsShowBreadcrumb { get; set; }

    /// <summary>
    /// 1 = ใช้งาน , 0 = ไม่ใช้งาน
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// ลำดับการแสดงผล
    /// </summary>
    public decimal nOrder { get; set; }
}
