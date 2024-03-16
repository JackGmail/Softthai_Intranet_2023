using Extensions.Common.STResultAPI;
using ST.INFRA.Common;

namespace Backend.Models
{

    #region Async Auto Complete
    public class cDataOptions_AutoComplete
    {
        public string label { get; set; }
        public string value { get; set; }
    }

    public class cDataAutoComplete_MethodAxiosPost
    {
        public string? strSearch { get; set; }
        public string? sParam { get; set; }
        public string? sDate { get; set; }
        public int? nValue { get; set; }
        public string? sName { get; set; }

    }
    #endregion
}
