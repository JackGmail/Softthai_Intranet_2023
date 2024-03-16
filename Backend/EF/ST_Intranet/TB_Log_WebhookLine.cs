using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Log_WebhookLine
{
    public int nLogID { get; set; }

    public string? sGUID { get; set; }

    public DateTime dSend { get; set; }

    public string sMessage { get; set; } = null!;

    public bool? IsActionAlready { get; set; }
}
