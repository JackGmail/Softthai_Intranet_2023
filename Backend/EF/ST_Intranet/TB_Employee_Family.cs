using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_Family
{
    /// <summary>
    /// รหัสตาราง
    /// </summary>
    public int nFamilyID { get; set; }

    /// <summary>
    /// รหัสตารางพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// จำนวนบุตร
    /// </summary>
    public int? nChilden { get; set; }

    /// <summary>
    /// จำนวนพี่น้อง รวมพนักงาน
    /// </summary>
    public int? nSiblings { get; set; }

    /// <summary>
    /// จำนวนพี่ชาย น้องชาย
    /// </summary>
    public int? nBrother { get; set; }

    /// <summary>
    /// จำนวนพี่สาว น้องสาว
    /// </summary>
    public int? nSister { get; set; }

    /// <summary>
    /// เป็นบุตรคนที่
    /// </summary>
    public int? nChildPosition { get; set; }

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
