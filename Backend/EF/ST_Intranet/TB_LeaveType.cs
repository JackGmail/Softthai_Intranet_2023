using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_LeaveType
{
    public int nLeaveTypeID { get; set; }

    /// <summary>
    /// รหัสอ้างอิง
    /// </summary>
    public string? sLeaveTypeCode { get; set; }

    /// <summary>
    /// ชื่อประเภทการลา
    /// </summary>
    public string LeaveTypeName { get; set; } = null!;

    /// <summary>
    /// ลำดับ
    /// </summary>
    public int nOrder { get; set; }

    public int sSex { get; set; }

    /// <summary>
    /// สมทบได้ไม่เกิน
    /// </summary>
    public decimal? nAssociate { get; set; }

    /// <summary>
    /// กำหนดลาล่วงหน้า (วัน)
    /// </summary>
    public decimal? nAdvanceLeave { get; set; }

    /// <summary>
    /// จำนวนวันที่สามารถลาติดต่อกันได้สูงสุด (วัน)
    /// </summary>
    public decimal? nMaximum { get; set; }

    /// <summary>
    /// เปลี่ยนวันหยุดเป็นเงินได้หรือไม่ true = ได้, false = ไม่ได้
    /// </summary>
    public bool? IsChangeIntoMoney { get; set; }

    /// <summary>
    /// เงื่อนไขการลา
    /// </summary>
    public string? sCondition { get; set; }

    /// <summary>
    /// true = ใช้งาน , false = ไม่ใช้งาน
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// วันที่สร้างรายการ
    /// </summary>
    public DateTime dCreate { get; set; }

    /// <summary>
    /// ผู้สร้างรายการ
    /// </summary>
    public int nCreateBy { get; set; }

    /// <summary>
    /// วันที่แก้ไขรายการ
    /// </summary>
    public DateTime dUpdate { get; set; }

    /// <summary>
    /// ผู้แก้ไขรายการ
    /// </summary>
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
}
