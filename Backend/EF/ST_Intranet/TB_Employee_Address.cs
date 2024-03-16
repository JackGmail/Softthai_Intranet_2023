using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_Address
{
    /// <summary>
    /// รหัสตาราง
    /// </summary>
    public int nAddressID { get; set; }

    /// <summary>
    /// รหัสตารางพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// TM_Data.nDatatype_ID = 12
    /// </summary>
    public int? nAdressType { get; set; }

    /// <summary>
    /// TM_Data.nDatatype_ID = 23
    /// </summary>
    public int nResidenceType { get; set; }

    /// <summary>
    /// ลำดับ
    /// </summary>
    public int nOrder { get; set; }

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
    /// ตำบล Refer.TM_SubDistrict
    /// </summary>
    public string? sSubDistrictID { get; set; }

    /// <summary>
    /// อำเภอ Refer.TM_District
    /// </summary>
    public int? nDistrictID { get; set; }

    /// <summary>
    /// จังหวัด Refer.TM_Province
    /// </summary>
    public int? nProvinceID { get; set; }

    /// <summary>
    /// รหัสไปรษณีย์ Refer.TM_SubDistrictID
    /// </summary>
    public int? nPostcode { get; set; }

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
