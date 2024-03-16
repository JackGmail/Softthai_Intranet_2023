
using Extensions.Common.STResultAPI;
using ST.INFRA.Common;
using static Extensions.Systems.AllClass;
using ST.INFRA;
using static Backend.Enum.EnumMeeting;
using Extensions.Common.STExtension;

namespace Backend.Models
{
    public class clsResultMeeting : Pagination
    {
        public List<objEventCalendar>? lstEvent { get; set; }
        public DateTime? sDateStart { get; set; }
        public List<objResultSearch>? listSearchResult { get; set; }
        public List<objRoom>? lstRoom { get; set; } = new List<objRoom>();
        public List<objRoom>? lstRoomAll { get; set; } = new List<objRoom>();
        public List<objRoom>? Process { get; set; } = new List<objRoom>();
        public List<objRoom>? Person { get; set; } = new List<objRoom>();
        public List<objRoom>? lstStatus { get; set; } = new List<objRoom>();
        public List<objRoom>? Project { get; set; } = new List<objRoom>();
        public List<objRoom>? ProjectAll { get; set; } = new List<objRoom>();
        public List<string>? CoProject { get; set; } = new List<string>();
        public List<objRoomDetail>? listContent { get; set; } = new List<objRoomDetail>();
        public List<string>? PersonProject { get; set; } = new List<string>();
        public List<string?>? listRoom { get; set; } = new List<string?>();
        public bool isPositionCo { get; set; }
        public string sName { get; set; } = "";
        public string Position { get; set; } = "";

    }


    public class objEventCalendar
    {
        public string? sID { get; set; }
        public string? groupId { get; set; }
        public DateTime dEventStart { get; set; }
        public DateTime dEventEnd { get; set; }
        public bool allDay { get; set; }
        public string? usetitle { get; set; }

        public DateTime sDisplayStart
        {
            get
            {
                return dEventStart;
            }
        }
        public string? usedateE { get; set; }
        public string? start
        {
            get
            {
                return sDisplayStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmmss);
            }
        }
        public string? end
        {
            get
            {
                return sDisplayEnd.ToStringFromDate("yyyy-MM-dd HH:mm:ss", "en-US");
            }
        }

        public DateTime sDisplayEnd
        {
            get
            {
                return dEventEnd;
            }
        }
        public string? title { get; set; }
        public string? backgroundColor { get; set; }
        public string? textColor { get; set; }
        public int? nStatus { get; set; }
        public string? usedates { get; set; }
        public int? nST
        {
            get
            {
                return nStatus == (int)ActionId.InProcess ? 2 : nStatus == (int)ActionId.Booking ? 3 : 4;
            }
        }
        public bool? IsMeetingRoom { get; set; }
        public string? Topic { get; set; }
        public string? sReq { get; set; }
        public string? sAppove { get; set; }
        public string? sRemark { get; set; }
        public DateTime dlastUpate { get; set; }
        public string slastUpate
        {
            get
            {
                return dlastUpate.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmm);
            }
        }
        public string? usetimes { get; set; }
        public bool? isFile { get; set; }
        public int? nCountFile { get; set; }
        public string? sImgURLReq { get; set; }
        public string? sImgURLCo { get; set; }

        public string? sRoomName { get; set; }
    }

    public class objRoom
    {
        public string? value { get; set; }
        public string? label { get; set; }
    }


    public class clsFilterMeeting : STGrid.PaginationData
    {
        public string? dStart { get; set; }
        public string? dEnd { get; set; }
        public string? sTopic { get; set; }
        public List<int?>? listRoom { get; set; }
        public int? mode { get; set; }
        public int? DateType { get; set; }
        public List<string>? selectProject { get; set; } = new List<string>();
        public List<string>? selectMTroom { get; set; } = new List<string>();
        public List<string>? selectStatus { get; set; } = new List<string>();
    }

    public class clsFilterCheckCo
    {
        public bool checkCoProject { get; set; }
        public bool isMode { get; set; }
    }

    public class clsFilter : ResultAPI
    {
        public List<objCheckTotal> listAllData { get; set; } = new List<objCheckTotal>();
        public string sDetail { get; set; } = "";
    }


    public class clsSaveMeeting : ResultAPI
    {
        public int? nMode { get; set; }
        public string sStart { get; set; } = "";
        public string sStartTest { get; set; } = "";
        public string sEnd { get; set; } = "";
        public DateTime? dStart { get; set; }
        public DateTime? dEnd { get; set; }
        public string? sID { get; set; } // check id
        public string? sName { get; set; }
        public string? sPosition { get; set; }
        public List<ItemFileData>? fFile { get; set; } = new List<ItemFileData> { };
        public string? sTitle { get; set; }
        public string? selectProject { get; set; }
        public bool? checkBoxOtherPJ { get; set; }
        public string? sOtherPJ { get; set; }
        public string? selectProcess { get; set; }
        public bool? checkBoxOtherPC { get; set; }
        public string? sOtherPC { get; set; }
        public string? selectMTroom { get; set; }
        public int? nNop { get; set; }
        public bool? IsAllDay { get; set; }
        public string? sRemark { get; set; }
        public string? sRemarkCancel { get; set; }
        public List<string>? selectAllPerson { get; set; }
        public List<TablePersonName>? selectAllPersonName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPass { get; set; }
        //<summary>
        //เช็คว่าเป็นการจองของตัวเองไหม
        //</summary>
        public bool? IsBookMine { get; set; }
        public bool? IsActionFromLine { get; set; } = false;
        public string? sGUID { get; set; }
        public int? nStatusID { get; set; }

    }
    public class TablePersonName
    {
        public string? sID { get; set; }
        public string? Name { get; set; }

    }

    public class objGetData
    {
        public int? mode { get; set; }
        public string? sStart { get; set; }
        public string? sEnd { get; set; }
        public string? nRoom { get; set; }
        public string? nID { get; set; } // check id
        public bool? isAllDay { get; set; }
    }

    public class objRoomDetail
    {
        public int? sID { get; set; }
        public string? Floor { get; set; }
        public string? Room { get; set; }
        public string? Seating { get; set; }
        public string? Name { get; set; }
        public string? sImgURL { get; set; }
        public string? Equipment { get; set; }
        public bool IsCanDel { get; set; }
    }

    public class objResultSearch
    {
        public string? sID { get; set; }
        public int? nID { get; set; }
        public bool allDay { get; set; }
        public int? nStatus { get; set; }
        public DateTime dEventStart { get; set; }
        public DateTime? dEventEnd { get; set; }
        public DateTime? sDisplayStart
        {
            get
            {
                return dEventStart;
            }
        }
        public DateTime? sDisplayEnd
        {
            get
            {
                return dEventEnd;
            }
        }
        public string? usetimes { get; set; }
        public string? usetitle { get; set; }
        public string? usedates { get; set; }
        public int? nRoomID { get; set; }

    }

    public class cFileZip
    {
        public Stream objFile { get; set; } //byte[]?
        public string? sFileType { get; set; }
        public string? sFileName { get; set; }
    }
    public class cFileDocument
    {
        public string? sFileName_Sys { get; set; }
        public string? sFilePath { get; set; }
        public string? sTypeFileAll { get; set; }
        public int nCenterID { get; set; }
    }

    public class cFileID
    {
        public List<int> sID { get; set; } = new List<int>();

    }

    public class objCheckTotal
    {
        public DateTime dStart { get; set; }
        public DateTime dEnd { get; set; }
        public double? dulationDay { get; set; }
        public int? nID { get; set; }
        public string? sProject { get; set; }
        public bool? isAllDay { get; set; }


    }

    public class cSaveRoom : ResultAPI
    {
        public string sID { get; set; }
        public int nRoomID { get; set; }
        public int nFloorID { get; set; }
        public string? sRoomName { get; set; }
        public string? sRoomCode { get; set; }
        public int nPerson { get; set; }
        public string? sEquipment { get; set; }
        public List<ItemFileData>? fFile { get; set; } //= new List<ItemFileData> { };
        public string? sFloorName { get; set; }
        public bool isActive { get; set; }
    }

    public class clsFileAllTable : Pagination
    {
        public List<objResultFileAllTable>? lstData { get; set; }
    }

    public class objResultFileAllTable
    {
        public int? sID { get; set; }
        public int? nMeetingID { get; set; }
        public string? sType { get; set; }
        public string? sNameDoc { get; set; }
        public string? sDate { get; set; }
    }

    public class cFilterTable : STGrid.PaginationData
    {
        //public string? sProjectName { get; set; }
        public string? sID { get; set; }
    }

    public class objImage
    {
        public string? sPath { get; set; }
        public string? sSystemFileName { get; set; }
        public int? nEmployeeID { get; set; }
        public int? nEmployeeImageID { get; set; }
    }

    public class objEmployeeCo
    {
        public string sFullName { get; set; } = "";
        public int nEmployeeID { get; set; }
    }

}

