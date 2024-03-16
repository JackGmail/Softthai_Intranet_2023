using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_LineAccount
{
    public int nLineAccountID { get; set; }

    public int nEmployeeID { get; set; }

    public string sEmail { get; set; } = null!;

    public string sTokenID { get; set; } = null!;

    public string sNameAccount { get; set; } = null!;

    public bool IsDelete { get; set; }
}
