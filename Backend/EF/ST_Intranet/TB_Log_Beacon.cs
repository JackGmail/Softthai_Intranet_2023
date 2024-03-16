using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Log_Beacon
{
    public int nLogID { get; set; }

    public DateTime dSend { get; set; }

    public DateTime dTimeStamp { get; set; }

    public string sReplyToken { get; set; } = null!;

    public string sUserId { get; set; } = null!;
}
