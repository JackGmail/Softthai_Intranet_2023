using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee
{
    /// <summary>
    /// รหัสตาราง
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// รหัสพนักงาน
    /// </summary>
    public string? sEmplyeeCode { get; set; }

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
    /// วันที่เริ่มงาน
    /// </summary>
    public DateTime? dWorkStart { get; set; }

    /// <summary>
    /// วันที่ผ่านโปร
    /// </summary>
    public DateTime? dPromote { get; set; }

    /// <summary>
    /// วันที่ลาออก
    /// </summary>
    public DateTime? dWorkEnd { get; set; }

    /// <summary>
    /// วันเกิด
    /// </summary>
    public DateTime? dBirth { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? sEmail { get; set; }

    /// <summary>
    /// เบอร์ติดต่อ
    /// </summary>
    public string? sTelephone { get; set; }

    public int? nStatusID { get; set; }

    /// <summary>
    /// true = ใช้งาน , false = ไม่ใช้งาน
    /// </summary>
    public bool IsActive { get; set; }

    public bool IsRetire { get; set; }

    public DateTime? dRetire { get; set; }

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

    /// <summary>
    /// รหัสเงินเดือน Refer.TB_Salary_History
    /// </summary>
    public int? nSalaryID { get; set; }

    /// <summary>
    /// หมายเลขบัตรประชาชน
    /// </summary>
    public string? nIDCard { get; set; }

    /// <summary>
    /// วันที่บัตรประชาชนหมดอายุ
    /// </summary>
    public DateTime? dExpirationDate { get; set; }

    /// <summary>
    /// เชื้อชาติ  Refer.TM_Data
    /// </summary>
    public int? nRace { get; set; }

    /// <summary>
    /// สัญชาติ  Refer.TM_Data
    /// </summary>
    public int? nNationality { get; set; }

    /// <summary>
    /// ศาสนา  Refer.TM_Data
    /// </summary>
    public int? nReligion { get; set; }

    /// <summary>
    /// ชื่อผู้ใช้
    /// </summary>
    public string sUsername { get; set; } = null!;

    /// <summary>
    /// รหัสผ่าน
    /// </summary>
    public string sPassword { get; set; } = null!;

    /// <summary>
    /// ประเภทพนักงาน Refer.TM_Data : Type 7
    /// </summary>
    public int nEmployeeTypeID { get; set; }

    /// <summary>
    /// การเปลี่ยนรหัสผ่าน
    /// </summary>
    public bool IsLoginChangePassword { get; set; }

    /// <summary>
    /// วันที่เปลี่ยนรหัสผ่าน
    /// </summary>
    public DateTime? dChangePasswordDate { get; set; }

    public int? nHeight { get; set; }

    /// <summary>
    /// น้ำหนัก
    /// </summary>
    public decimal? nWeight { get; set; }

    /// <summary>
    /// ภาวะทางการทหาร Refer.TM_Data
    /// </summary>
    public int? nMilitaryConditions { get; set; }

    /// <summary>
    /// สถานภาพรส Refer.TM_Data
    /// </summary>
    public int? nMaritalStatus { get; set; }

    /// <summary>
    /// เพศ Refer.TM_Data
    /// </summary>
    public int? sSex { get; set; }
}
