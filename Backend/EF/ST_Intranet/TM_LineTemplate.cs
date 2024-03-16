using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TM_LineTemplate
{
    public int nID { get; set; }

    public string sSubject { get; set; } = null!;

    public string sDescription { get; set; } = null!;

    public string sDetail { get; set; } = null!;
}
