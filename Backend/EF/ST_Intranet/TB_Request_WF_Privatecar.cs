using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Request_WF_Privatecar
{
    public int nRequestPrivatecarID { get; set; }

    /// <summary>
    /// รหัสใบคำขอการเบิกค่าเดินทาง (TB_Request_TravelExpenses)
    /// </summary>
    public int nRequestTravelExpensesID { get; set; }

    public int nOrder { get; set; }

    public DateTime dDate { get; set; }

    public int nProjectID { get; set; }

    /// <summary>
    /// ต้นทาง ไป (TM_Data.nData_ID = 35)
    /// </summary>
    public int? nOriginDepartture { get; set; }

    public string? sOther1 { get; set; }

    /// <summary>
    /// ปลายทาง ไป (TM_Data.nData_ID = 35)
    /// </summary>
    public int? nDestinationDepartture { get; set; }

    public string? sOther2 { get; set; }

    /// <summary>
    /// ต้นทาง กลับ (TM_Data.nData_ID = 35)
    /// </summary>
    public int? nOriginReturn { get; set; }

    public string? sOther3 { get; set; }

    /// <summary>
    /// ปลายทาง กลับ (TM_Data.nData_ID = 35)
    /// </summary>
    public int? nDestinationReturn { get; set; }

    public string? sOther4 { get; set; }

    /// <summary>
    /// ระยะทาง
    /// </summary>
    public decimal nDistance { get; set; }

    public decimal nMoney { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsDelete { get; set; }
}
