using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Request_Leave
{
    /// <summary>
    /// รหัสใบคำขอ
    /// </summary>
    public int nRequestLeaveID { get; set; }

    /// <summary>
    /// TM_RequestType .nRequestTypeID = 1
    /// </summary>
    public int nRequestTypeID { get; set; }

    /// <summary>
    /// รหัสพนักงาน
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// ประเภทการลา
    /// </summary>
    public int nLeaveTypeID { get; set; }

    /// <summary>
    /// ลาแบบฉุกเฉิน true = ใช่ , false = ไม่ใช่
    /// </summary>
    public bool IsEmergency { get; set; }

    /// <summary>
    /// วันเวลาเริ่มต้น
    /// </summary>
    public DateTime dStartDateTime { get; set; }

    /// <summary>
    /// วันเวลาสิ้นสุด
    /// </summary>
    public DateTime dEndDateTime { get; set; }

    /// <summary>
    /// เหตุผลการลา
    /// </summary>
    public string? sReason { get; set; }

    /// <summary>
    /// สถานะใบคำร้อง
    /// </summary>
    public int nStatusID { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsDelete { get; set; }

    /// <summary>
    /// จำนวนวันลาที่ใช้ไปแล้ว
    /// </summary>
    public decimal nLeaveUse { get; set; }
}
