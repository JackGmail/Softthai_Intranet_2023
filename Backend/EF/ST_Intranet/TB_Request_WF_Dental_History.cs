using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Request_WF_Dental_History
{
    public int nRequestDentalID_His { get; set; }

    /// <summary>
    /// รหัสใบคำขอเบิกค่าทันตกรรม
    /// </summary>
    public int nRequestDentalID { get; set; }

    /// <summary>
    /// รหัสประเภทใบคำขอ (TM_RequestType)
    /// </summary>
    public int nRequestTypeID { get; set; }

    /// <summary>
    /// รหัสพนักงาน
    /// </summary>
    public int nEmployeeID { get; set; }

    /// <summary>
    /// วันที่รับบริการทันตกรรม
    /// </summary>
    public DateTime dDate { get; set; }

    /// <summary>
    /// ชื่อสถานพยาาล
    /// </summary>
    public string sMedicalFacility { get; set; } = null!;

    /// <summary>
    /// รหัสประเภททันตกรรม (TM_Data.nDataTypeID = 36)
    /// </summary>
    public int nDentalTypeID { get; set; }

    /// <summary>
    /// ทันตกรรมอื่น ๆ 
    /// </summary>
    public string? sOther { get; set; }

    /// <summary>
    /// จำนวนเงินทั้งสิ้น
    /// </summary>
    public decimal nMoney { get; set; }

    /// <summary>
    /// จำนวนเงินที่เบิกได้ตามเงื่อนไขบริษัท
    /// </summary>
    public decimal nConditionAmount { get; set; }

    /// <summary>
    /// จำนวนเงินที่ขอเบิก
    /// </summary>
    public decimal nAmountWithdrawn { get; set; }

    /// <summary>
    /// ยอดคงเหลือ
    /// </summary>
    public decimal nRemain { get; set; }

    public int nStatus { get; set; }

    public string? sComent { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsDelete { get; set; }
}
