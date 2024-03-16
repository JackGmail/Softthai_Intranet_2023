using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_LogLogin
{
    /// <summary>
    /// Log ID
    /// </summary>
    public int nLogID { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    public string sUsername { get; set; } = null!;

    /// <summary>
    /// Device Name
    /// </summary>
    public string? sDeviceName { get; set; }

    /// <summary>
    /// Login Date
    /// </summary>
    public DateTime dLoginDate { get; set; }

    /// <summary>
    /// Logout Date
    /// </summary>
    public DateTime? dLogoutDate { get; set; }

    /// <summary>
    /// IP Address
    /// </summary>
    public string? sIPAddress { get; set; }

    /// <summary>
    /// Status: 1=success, 0=err
    /// </summary>
    public int? nStatus { get; set; }

    /// <summary>
    /// Erroe Message
    /// </summary>
    public string? sErrorMsg { get; set; }
}
