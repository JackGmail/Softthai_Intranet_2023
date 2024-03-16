using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Room
{
    public int nRoomID { get; set; }

    public int nFloorID { get; set; }

    public string? sRoomName { get; set; }

    public string? sRoomCode { get; set; }

    public string? sEquipment { get; set; }

    /// <summary>
    /// ลำดับรายการ
    /// </summary>
    public int nPerson { get; set; }

    /// <summary>
    /// Path เก็บไฟล์
    /// </summary>
    public string? sPath { get; set; }

    /// <summary>
    /// ชื่อไฟล์ที่เก็บในระบบ
    /// </summary>
    public string? sSystemFilename { get; set; }

    /// <summary>
    /// ชื่อไฟล์
    /// </summary>
    public string? sFilename { get; set; }

    /// <summary>
    /// จำนวนคนที่รองรับ
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// ลำดับรายการ
    /// </summary>
    public int nOrder { get; set; }

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
    /// วันที่ลบรายการ
    /// </summary>
    public DateTime? dDelete { get; set; }

    /// <summary>
    /// ผู้ที่ลบรายการ
    /// </summary>
    public int? nDeleteBy { get; set; }

    /// <summary>
    /// true = ลบ , false = ยังไม่ถูกลบ
    /// </summary>
    public bool IsDelete { get; set; }
}
