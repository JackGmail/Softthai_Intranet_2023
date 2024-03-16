using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Employee_LineConfirm
{
    public int nConfirmEmail_ID { get; set; }

    public string? sEmail { get; set; }

    public bool? IsConfirm { get; set; }

    public bool? IsTimeOut { get; set; }

    public TimeSpan tSendEmail { get; set; }
}
