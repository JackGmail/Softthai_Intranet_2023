using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_Language
{
    public int nLanguageID { get; set; }

    /// <summary>
    /// รหัสตารางพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// ลำดับ
    /// </summary>
    public int nOrder { get; set; }

    /// <summary>
    /// ภาษา Refer.TM_Data
    /// </summary>
    public int nLanguage { get; set; }

    /// <summary>
    /// ภาษาอื่น ๆ 
    /// </summary>
    public string? sOther { get; set; }

    /// <summary>
    /// ระดับการพูด Refer.TM_Data
    /// </summary>
    public int? nSpeaking { get; set; }

    /// <summary>
    /// ระดับการเขียน Refer.TM_Data
    /// </summary>
    public int? nWriting { get; set; }

    /// <summary>
    /// ระดับการอ่าน Refer.TM_Data
    /// </summary>
    public int? nReading { get; set; }

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
