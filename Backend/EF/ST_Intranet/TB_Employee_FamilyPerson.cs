using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_FamilyPerson
{
    /// <summary>
    /// รหัสตาราง
    /// </summary>
    public int nFamilyPersonID { get; set; }

    /// <summary>
    /// รหัสตารางพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// ลำดับ
    /// </summary>
    public int nOrder { get; set; }

    /// <summary>
    /// ชื่อ
    /// </summary>
    public string? sName { get; set; }

    /// <summary>
    /// นามสกุล
    /// </summary>
    public string? sSureName { get; set; }

    /// <summary>
    /// ความสัมพันธ์ Refer.TM_Data
    /// </summary>
    public int nRelationship { get; set; }

    /// <summary>
    /// อายุ
    /// </summary>
    public int? nAge { get; set; }

    /// <summary>
    /// อาชีพ
    /// </summary>
    public string? sOccupation { get; set; }

    /// <summary>
    /// สถานที่ทำงาน
    /// </summary>
    public string? sWorkplace { get; set; }

    /// <summary>
    /// ตำแหน่ง
    /// </summary>
    public string? sPosition { get; set; }

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
