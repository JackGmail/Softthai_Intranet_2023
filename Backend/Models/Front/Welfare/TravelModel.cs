using Extensions.Common.STResultAPI;
using ST.INFRA.Common;
using static Extensions.Systems.AllClass;

namespace Backend.Models
{
    public class cTravelData : Extensions.Common.STResultAPI.ResultAPI
    {
        public string? sID { get; set; }
        public DateTime? dMonthRequest { get; set; }
        public List<objTravelRow>? lstPublic { get; set; }
        public List<objTravelRow>? lstPrivate { get; set; }
        public decimal? nTotalAmount { get; set; }
        public string? sComment { get; set; }
        public int nStatusID { get; set; }
        public string? sUserName { get; set; }
        public string? sPosition { get; set; }
        public bool? IsRequester { get; set; }
        public bool? IsApprover { get; set; }
        public List<cHistoryLog>? lstHistoryLog { get; set; }
    }

    public class objTravelRow
    {
        public int nID { get; set; }
        public string? sID { get; set; }
        public int? nNo { get; set; }
        public int? nUploadFileNo { get; set; }
        public DateTime dTravel { get; set; }
        public string? sProject { get; set; }
        public string? sVehicle { get; set; }
        public objTravelChoice? objGo_Start { get; set; }
        public objTravelChoice? objGo_End { get; set; }
        public objTravelChoice? objBack_Start { get; set; }
        public objTravelChoice? objBack_End { get; set; }
        public decimal? nAmount { get; set; }
        public decimal? nDistance { get; set; }
        public List<ItemFileData>? arrFile { get; set; }
    }
    public class objTravelChoice
    {
        public string? sStartTypeID { get; set; }
        public string? sOther { get; set; }
    }
}
