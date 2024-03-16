using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_Dental_Year
{
    public int nEmpDentalID { get; set; }

    public int nEmpID { get; set; }

    public int nYear { get; set; }

    public int nMoney { get; set; }

    public bool IsDelete { get; set; }

    public DateTime dCreateDate { get; set; }

    public int nCreateBy { get; set; }

    public DateTime dUpdateDate { get; set; }

    public int nUpdateBy { get; set; }
}
