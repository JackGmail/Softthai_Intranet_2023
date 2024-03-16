﻿using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_WorkExperience
{
    public int nWorkExperienceID { get; set; }

    /// <summary>
    ///  รหัสตารางพนักงาน Refer.TB_Employee
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// ลำดับ
    /// </summary>
    public int nOrder { get; set; }

    /// <summary>
    /// สถานที่ทำงาน
    /// </summary>
    public string? sWorkCompany { get; set; }

    /// <summary>
    /// วันที่เรี่มงาน
    /// </summary>
    public DateTime? dWorkStart { get; set; }

    /// <summary>
    /// วันที่สิ้นสุดการทำงาน
    /// </summary>
    public DateTime? dWorkEnd { get; set; }

    /// <summary>
    /// ตำแหน่งงาน
    /// </summary>
    public string? sPosition { get; set; }

    /// <summary>
    /// ลักษณะงาน
    /// </summary>
    public string? sJobDescription { get; set; }

    /// <summary>
    /// เงินเดือน
    /// </summary>
    public decimal? nSalary { get; set; }

    /// <summary>
    /// เหตุผลที่ออก
    /// </summary>
    public string? sReasonsOfResignation { get; set; }

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
