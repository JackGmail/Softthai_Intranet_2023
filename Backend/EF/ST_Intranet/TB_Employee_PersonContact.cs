using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_PersonContact
{
    public int nPersonContactID { get; set; }

    /// <summary>
    /// รหัสพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// ประเภทผู้ตือต่อของพนักงาน Refer.TM_Data
    /// </summary>
    public int nContactType { get; set; }

    public int? nOrder { get; set; }

    /// <summary>
    /// ชื่อผู้ติดต่อ
    /// </summary>
    public string sName { get; set; } = null!;

    /// <summary>
    /// นามสกุลผู้ติดต่อ
    /// </summary>
    public string? sSurename { get; set; }

    /// <summary>
    /// ประเภทความสัมพันธ์ Refer.TM_Data
    /// </summary>
    public int? nRelationship { get; set; }

    public string? sAddress { get; set; }

    /// <summary>
    /// บ้านเลขที่
    /// </summary>
    public string? sPresentAddress { get; set; }

    /// <summary>
    /// หมู่ที่
    /// </summary>
    public string? sMoo { get; set; }

    /// <summary>
    /// ถนน
    /// </summary>
    public string? sRoad { get; set; }

    /// <summary>
    /// ตำบล
    /// </summary>
    public int? nSubDistrict { get; set; }

    /// <summary>
    /// อำเภอ
    /// </summary>
    public int? nDistrict { get; set; }

    /// <summary>
    /// จังหวัด
    /// </summary>
    public int? nProvince { get; set; }

    /// <summary>
    /// รหัสไปรษณีย์
    /// </summary>
    public int? nPostcode { get; set; }

    /// <summary>
    /// เบอร์มือถือ
    /// </summary>
    public string? sTelephone { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }
}
