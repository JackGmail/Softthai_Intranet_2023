using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_LogEmail
{
    /// <summary>
    /// Email ID
    /// </summary>
    public int nEmailID { get; set; }

    /// <summary>
    /// From
    /// </summary>
    public string sFrom { get; set; } = null!;

    /// <summary>
    /// To
    /// </summary>
    public string sTo { get; set; } = null!;

    /// <summary>
    /// Cc
    /// </summary>
    public string? sCc { get; set; }

    /// <summary>
    /// Bcc
    /// </summary>
    public string? sBcc { get; set; }

    /// <summary>
    /// Subject
    /// </summary>
    public string sSubject { get; set; } = null!;

    /// <summary>
    /// Message
    /// </summary>
    public string sMessage { get; set; } = null!;

    public bool IsMailTest { get; set; }

    /// <summary>
    /// 1 = Success, 0 = Error
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Message Error
    /// </summary>
    public string? sMessage_Error { get; set; }

    /// <summary>
    /// Send Date
    /// </summary>
    public DateTime? dSend { get; set; }
}
