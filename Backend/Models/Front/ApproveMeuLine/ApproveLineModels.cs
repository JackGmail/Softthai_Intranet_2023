using Extensions.Common.STResultAPI;
using ST.INFRA.Common;

namespace Backend.Models
{
    public class ResponseMenu  : Extensions.Common.STResultAPI.ResultAPI
    {
        public List<cMenuLine>? lstMenuLine { get; set; }

    }
    public class cMenuLine
    {
        public int nMenuID { get; set; }
        public string sMenuName { get; set; }
        public string sRoute { get; set; }
    }


}
