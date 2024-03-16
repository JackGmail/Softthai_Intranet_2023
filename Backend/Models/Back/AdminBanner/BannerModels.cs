
using Extensions.Common.STExtension;
using Extensions.Common.STResultAPI;
using ST.INFRA;
using ST.INFRA.Common;
using static Backend.Enum.EnumStandard;
using static Extensions.Systems.AllClass;
namespace Backend.Models
{
    /// <summary>
    /// </summary>
    public class ClsFilterBanner: STGrid.PaginationData
    {
        /// <summary>
        /// </summary>
        public string? sTitle { get; set; } = "";
        /// <summary>
        /// </summary>
        public string? nStatus { get; set; }= "";

    }

    /// <summary>
    /// </summary>
    public class ClsResultTableBanner : Pagination
    {
        /// <summary>
        /// </summary>
        public List<ObjResultTableBanner>? lstData { get; set; }
        /// <summary>
        /// </summary>
        public int nOrder { get; set; }
    }
    /// <summary>
    /// </summary>
    public class ObjResultTableBanner
    {
        /// <summary>
        /// </summary>
        public int? No { get; set; }
        /// <summary>
        /// </summary>
        public int? sID { get; set; }
        /// <summary>
        /// </summary>
        public string? sPRBanner { get; set; }
        /// <summary>
        /// </summary>
        public DateTime? dStart { get; set; }
        /// <summary>
        /// </summary>
        public DateTime? dEnd { get; set; }
        /// <summary>
        /// </summary>
        public string sSEDate
        {
            get
            {
                return dStart.ToStringFromDateNullAble(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH) + " - " + (dEnd != null ? dEnd.ToStringFromDateNullAble(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH) : "ไม่มีกำหนดการ");
            }
        }
        /// <summary>
        /// </summary>
        public string? sUpdateby { get; set; }
        /// <summary>
        /// </summary>
        public DateTime dLastUpdate { get; set; }
        /// <summary>
        /// </summary>
        public string sLastUpdate
        {
            get
            {
                return dLastUpdate.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmmss, ST.INFRA.Enum.CultureName.th_TH);
            }
        }
        /// <summary>
        /// </summary>
        public bool IsStatus { get; set; }
        /// <summary>
        /// </summary>
        public string sStatus
        {
            get
            {
                return IsStatus ? Status.Active.GetEnumValue() : Status.InActive.GetEnumValue();
            }
        }
        /// <summary>
        /// </summary>
        public int? nOrder { get; set; }



    }

    /// <summary>
    /// </summary>
    public class ClsFilterDelete
    {
        /// <summary>
        /// </summary>
        public List<int>? lstBannerDelete { get; set; }

    }
    /// <summary>
    /// </summary>
    public class ObjSave : Backend.Models.ResultAPI
    {
        /// <summary>
        /// </summary>
        public string? sID { get; set; }
        /// <summary>
        /// </summary>
        public string sBannerName { get; set; } = string.Empty;
        /// <summary>
        /// </summary>
        public string? sNote { get; set; }
        /// <summary>
        /// </summary>
        public string? sStart { get; set; }
        /// <summary>
        /// </summary>
        public string? sEnd { get; set; }
        /// <summary>
        /// </summary>
        public DateTime? dStart { get; set; }
        /// <summary>
        /// </summary>
        public DateTime? dEnd { get; set; }
        /// <summary>
        /// </summary>
        public List<ItemFileData> fFile { get; set; } = new List<ItemFileData> { };
        /// <summary>
        /// </summary>
        public bool IsStatus { get; set; }
        /// <summary>
        /// </summary>
        public bool IsAllDay { get; set; }
    }

    /// <summary>
    /// </summary>
    public class ClsResultOption : Pagination
    {
        /// <summary>
        /// </summary>
        public List<RoomItem>? listStatus { get; set; } = new List<RoomItem>();

    }
    /// <summary>
    /// </summary>
    public class RoomItem
    {
        /// <summary>
        /// </summary>
        public string? label { get; set; }
        /// <summary>
        /// </summary>
        public string? value { get; set; }

    }
    /// <summary>
    /// </summary>
    public class ClsResult : Backend.Models.ResultAPI
    {
        /// <summary>
        /// </summary>
        public string sID { get; set; } = string.Empty;
    }

    public class FileItem
    {
        /// <summary>
        /// Path
        /// </summary>
        public string sPath { get; set; } = null!;

        /// <summary>
        /// System in FileName
        /// </summary>
        public string sSystemFileName { get; set; } = null!;

        /// <summary>
        /// FileName
        /// </summary>
        public string sFileName { get; set; } = null!;
        /// <summary>
        /// </summary>
        public string? sFolderName { get; set; }
    }

}