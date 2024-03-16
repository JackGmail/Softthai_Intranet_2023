using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_LeaveSummary
{
    public int nLeaveSummaryID { get; set; }

    /// <summary>
    /// รหัสตารางพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// รหัสประเภทการลา Refer.TB_LeaveType
    /// </summary>
    public int nLeaveTypeID { get; set; }

    public int nYear { get; set; }

    /// <summary>
    /// จำนวนวันลาเริ่มต้นของปี
    /// </summary>
    public decimal nQuantity { get; set; }

    /// <summary>
    /// ยกยอดจากปีที่แล้ว(กรณีประเภทการลาที่มีสมทบ)
    /// </summary>
    public decimal nTransferred { get; set; }

    /// <summary>
    /// วันลาที่เปลี่ยนเป็นเงิน
    /// </summary>
    public decimal nIntoMoney { get; set; }

    /// <summary>
    /// จำนวนวันลาที่ใช้ไปแล้ว
    /// </summary>
    public decimal nLeaveUse { get; set; }

    public decimal nLeaveRemain { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
