using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_ContractPoint
{
    public int nContractPointID { get; set; }

    public int nCustomerID { get; set; }

    /// <summary>
    /// ชื่อภาษาไทย
    /// </summary>
    public string? sNameTH { get; set; }

    /// <summary>
    /// นามสกุลภาษาไทย
    /// </summary>
    public string? sSurnameTH { get; set; }

    /// <summary>
    /// ชื่อภาษาอังกฤษ
    /// </summary>
    public string? sNameEN { get; set; }

    /// <summary>
    /// นามสกุลภาษาอังกฤษ
    /// </summary>
    public string? sSurnameEN { get; set; }

    /// <summary>
    /// ชื่อเล่น
    /// </summary>
    public string? sNickname { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? sEmail { get; set; }

    /// <summary>
    /// เบอร์ติดต่อ
    /// </summary>
    public string? sTelephone { get; set; }

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
