using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_Position_History
{
    public int PositionHistoryID { get; set; }

    /// <summary>
    /// Refer.TB_Employee_Position
    /// </summary>
    public int nEmpPositionID { get; set; }

    /// <summary>
    /// รหัสตารางพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// ระดับตำแหน่ง Refer.TM_Data
    /// </summary>
    public int nLevelPosition { get; set; }

    public int? nOrder { get; set; }

    /// <summary>
    /// รหัสตำแหน่งงาน Refer.TB_Position
    /// </summary>
    public int nPositionID { get; set; }

    /// <summary>
    /// วันที่เริ่มต้น
    /// </summary>
    public DateTime? dStartDate { get; set; }

    /// <summary>
    /// วันที่สิ้นสุด
    /// </summary>
    public DateTime? dEndDate { get; set; }

    /// <summary>
    /// รหัสตำแหน่งงาน Refer.TB_Position
    /// </summary>
    public int nPromotePositionID { get; set; }

    public string? sRemark { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
