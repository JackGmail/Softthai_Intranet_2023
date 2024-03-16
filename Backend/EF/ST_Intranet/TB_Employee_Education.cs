using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_Education
{
    /// <summary>
    /// รหัสตาราง
    /// </summary>
    public int nEducationID { get; set; }

    /// <summary>
    /// รหัสตารางพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// ระดับการศึกษา Refer.TM_Data
    /// </summary>
    public int nEducational_Level { get; set; }

    /// <summary>
    /// ระดับการศึกษาอื่น ๆ 
    /// </summary>
    public string? sOther { get; set; }

    /// <summary>
    /// สถานบันการศึกษา
    /// </summary>
    public string? sAcademy { get; set; }

    /// <summary>
    /// สาขาวิชา
    /// </summary>
    public string? sMajor { get; set; }

    /// <summary>
    /// ปีที่เริ่มศึกษา
    /// </summary>
    public int? nEducationStart { get; set; }

    /// <summary>
    /// ปีที่จบการศึกษา
    /// </summary>
    public int? nEducationEnd { get; set; }

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
