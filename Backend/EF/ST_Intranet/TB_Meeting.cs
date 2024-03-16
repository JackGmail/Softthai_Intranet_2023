using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Meeting
{
    public int nMeetingID { get; set; }

    public int? nProjectID { get; set; }

    public int? nRoomID { get; set; }

    public int? nMasterProcessID { get; set; }

    public string? sTitle { get; set; }

    /// <summary>
    /// true = ลบ , false = ยังไม่ถูกลบ
    /// </summary>
    public bool IsOther { get; set; }

    public string? sOther { get; set; }

    /// <summary>
    /// true = ลบ , false = ยังไม่ถูกลบ
    /// </summary>
    public bool IsOtherProcess { get; set; }

    public string? sOtherProcess { get; set; }

    public int? nPerson { get; set; }

    public string? sRemark { get; set; }

    public DateTime dStart { get; set; }

    public DateTime dEnd { get; set; }

    /// <summary>
    /// true = ลบ , false = ยังไม่ถูกลบ
    /// </summary>
    public bool IsAllDay { get; set; }

    /// <summary>
    /// จำนวนคนที่รองรับ
    /// </summary>
    public bool IsActive { get; set; }

    public int nStatusID { get; set; }

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
