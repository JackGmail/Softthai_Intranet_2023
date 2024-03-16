using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_Salary_History
{
    /// <summary>
    /// รหัสตาราง
    /// </summary>
    public int nSalaryID { get; set; }

    /// <summary>
    /// รหัสตารางพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// วันที่เริ่มต้น
    /// </summary>
    public DateTime dStartDate { get; set; }

    /// <summary>
    /// วันที่สิ้นสุด
    /// </summary>
    public DateTime dEndDate { get; set; }

    /// <summary>
    /// เงินเดือน
    /// </summary>
    public decimal nSalary { get; set; }

    /// <summary>
    /// การจัดเรียง
    /// </summary>
    public int nOrder { get; set; }

    /// <summary>
    /// วันที่สร้างรายการ
    /// </summary>
    public DateTime dCreate { get; set; }

    /// <summary>
    /// true = ใช้งาน , false = ไม่ใช้งาน
    /// </summary>
    public bool IsActive { get; set; }

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
