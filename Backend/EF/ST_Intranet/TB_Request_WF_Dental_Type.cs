using System;
using System.Collections.Generic;

namespace Backend.EF.ST_Intranet;

public partial class TB_Request_WF_Dental_Type
{
    public int nRequestDentalTypeID { get; set; }

    public int nRequestDentalID { get; set; }

    public int nDentalTypeID { get; set; }

    /// <summary>
    /// จำนวนซี่ฟัน (กรณีเป็นอุดฟัน กับภอนฟัน จะมีให้กรอกจำนวนของซี่ฟันด้วย)
    /// </summary>
    public int? nQuantity { get; set; }

    public string? sOther { get; set; }
}
