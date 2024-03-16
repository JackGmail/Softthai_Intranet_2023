using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Request_WF_TravelExpenses_History
{
    public int nRequestTravelExpensesID_His { get; set; }

    /// <summary>
    /// รหัสใบคำขอการเบิกค่าเดินทาง
    /// </summary>
    public int nRequestTravelExpensesID { get; set; }

    /// <summary>
    /// รหัสประเภทใบคำขอ (TM_RequestType)
    /// </summary>
    public int nRequestTypeID { get; set; }

    public DateTime? dMonthRequest { get; set; }

    /// <summary>
    /// รหัสพนักงาน
    /// </summary>
    public int nEmployeeID { get; set; }

    public int nStatusID { get; set; }

    public string? sComment { get; set; }

    public decimal nTotalAmount { get; set; }

    public DateTime dCreate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdate { get; set; }

    public int nUpdateBy { get; set; }

    public DateTime? dDelete { get; set; }

    public int? nDeleteBy { get; set; }

    public bool IsDelete { get; set; }
}
