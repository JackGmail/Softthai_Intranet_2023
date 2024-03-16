using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_Report_To
{
    public int nReportTOID { get; set; }

    public int nEmployeeID { get; set; }

    public int nPositionID { get; set; }

    public int? nRepEmployeeID { get; set; }

    public int? nRepPositionID { get; set; }

    public int nTeamID { get; set; }

    public bool IsDelete { get; set; }
}
