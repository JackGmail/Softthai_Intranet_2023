using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Request_WF_Allowance
{
    /// <summary>
    /// รหัสใบคำขอเบี้ยเลี้ยง
    /// </summary>
    public int nRequestAllowanceID { get; set; }

    /// <summary>
    /// รหัสประเภทใบคำขอ (TM_RequestType)
    /// </summary>
    public int nRequestTypeID { get; set; }

    /// <summary>
    /// รหัสพนักงาน
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// รหัสโครงการ
    /// </summary>
    public int nProjectID { get; set; }

    public string? sDescription { get; set; }

    /// <summary>
    /// รหัสประเภทเบี้ยเลี้ยง ( แบบค้างคืน=TM_Data.nData_ID=122/ แบบไปกลับ =TM_Data.nData_ID=123)
    /// </summary>
    public int nAllowanceTypeID { get; set; }

    public DateTime? dStartDate_StartTime { get; set; }

    public DateTime? dStartDate_EndTime { get; set; }

    public DateTime? dEndDate_StartTime { get; set; }

    public DateTime? dEndDate_EndTime { get; set; }

    /// <summary>
    /// รวมวันที่เบิกเบี้ยเลี้ยง
    /// </summary>
    public decimal? nSumDay { get; set; }

    public decimal? nSumMoney { get; set; }

    /// <summary>
    /// คอมเม้นจากผู้อนุมัติ
    /// </summary>
    public string? sComment { get; set; }

    public int nStatusID { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsDelete { get; set; }
}
