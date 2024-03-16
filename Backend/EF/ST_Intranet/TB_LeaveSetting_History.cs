using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_LeaveSetting_History
{
    public int nLeaveSettingHisID { get; set; }

    /// <summary>
    /// รหัสการตั้งค่า Refer.TB_LeaveSetting
    /// </summary>
    public int LeaveSettingID { get; set; }

    /// <summary>
    /// ประเภทการลา Refer.TB_LeaveType
    /// </summary>
    public int nLeaveTypeID { get; set; }

    /// <summary>
    /// TM_Data.nDatatype_ID = 7
    /// </summary>
    public int nEmployeeTypeID { get; set; }

    public int nOrder { get; set; }

    /// <summary>
    /// อายุงานเเริ่มต้น (เดือน)
    /// </summary>
    public int nWorkingAgeStart { get; set; }

    /// <summary>
    /// อายุงานสิ้นสุด (เดือน)
    /// </summary>
    public int? nWorkingAgeEnd { get; set; }

    /// <summary>
    /// สิทธิ์การลา(วัน)
    /// </summary>
    public decimal nLeaveRights { get; set; }

    /// <summary>
    /// ต้องคิดสัดส่วนหรือไม่ true = ต้องคิด , false = ไม่ต้อง
    /// </summary>
    public bool IsProportion { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
