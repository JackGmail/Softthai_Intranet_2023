
using Extensions.Common.STResultAPI;
using Microsoft.EntityFrameworkCore;
using Extensions.Common.STExtension;
using ST.INFRA;
using Extensions.Common.STFunction;
using System.Globalization;
using ST.INFRA.Utility;
using Ionic.Zip;
using ST.INFRA.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using Backend.Interfaces;
using Backend.Interfaces.Authentication;
using Backend.EF.ST_Intranet;
using Backend.Models;
using static Backend.Enum.EnumMeeting;
using ResultAPI = Backend.Models.ResultAPI;
using static Extensions.Systems.AllClass;
using Backend.Enum;
using Backend.Models.Back.Permission;
using System.Text;
using ST_API.Models;
using Backend.Models.Authentication;

namespace Backend.Service
{

    /// <summary>
    /// MeetingRoomService
    /// </summary>
    public class MeetingRoomService : IMeetingRoomService
    {
        private readonly IAuthentication _authen;
        private readonly ST_IntranetEntity _db;
        private readonly IHostEnvironment _env;

        /// <summary>
        /// MeetingRoomService
        /// </summary>
        public MeetingRoomService(IAuthentication authen, ST_IntranetEntity db, IHostEnvironment env)
        {
            _authen = authen;
            _db = db;
            _env = env;
        }
        /// <summary>
        /// GetListCalendar
        /// </summary>
        public clsResultMeeting GetListCalendar(clsFilterMeeting param)
        {
            clsResultMeeting result = new clsResultMeeting();
            var ua = _authen.GetUserAccount();
            try
            {
                DateTime? dStart = param.dStart != null ? Convert.ToDateTime(param.dStart, CultureInfo.InvariantCulture) : null;
                DateTime? dEnd = param.dEnd != null ? Convert.ToDateTime(param.dEnd, CultureInfo.InvariantCulture) : null;
                string[]? sStart = param.dStart != null ? param.dStart.Split(" ") : null;
                string[]? sEnd = param.dEnd != null ? param.dEnd.Split(" ") : null;
                string? sDatestart = null;
                string? sDateend = null;
                if (sStart != null)
                {
                    if (sStart[1] == "00:01")
                    {
                        sDatestart = "09:00";
                    }
                    else
                    {
                        sDatestart = sStart[1];
                    }
                }
                if (sEnd != null)
                {
                    if (sEnd[1] == "23:59")
                    {
                        sDateend = "18:00";
                    }
                    else
                    {
                        sDateend = sEnd[1];
                    }
                }

                int nCancel = (int)ActionId.Cancel;
                IQueryable<TB_Meeting> TB_Meeting = _db.TB_Meeting.Where(w => !w.IsDelete && w.IsActive).AsQueryable();
                IQueryable<TB_Room> TB_Room = _db.TB_Room.Where(w => !w.IsDelete && w.IsActive).AsQueryable();
                IQueryable<TB_Project> TB_Project = _db.TB_Project.Where(w => !w.IsDelete).AsQueryable();
                IQueryable<TB_Floor> TB_Floor = _db.TB_Floor.Where(w => !w.IsDelete && w.IsActive).AsQueryable();
                IQueryable<TB_Employee> TB_Employee = _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive).AsQueryable();
                List<int?> listRoom = param.listRoom != null ? param.listRoom : new List<int?>();
                var listProject = _db.TB_Project.Where(w => !w.IsDelete).Select(s => new objRoom
                {
                    value = s.nProjectID + "",
                    label = !string.IsNullOrEmpty(s.sProjectAbbr) ? s.sProjectAbbr : s.sProjectName
                }).ToList();
                listProject.Add(new objRoom { value = "0", label = "อื่นๆ" });
                if (param.mode == 1)
                {
                    var lstEventCalendars = from a in _db.TB_Meeting_Event.Where(f => !f.IsDelete).GroupBy(g => g.nMeetingID).ToArray()
                                            from b in _db.TB_Meeting.Where(w => w.nMeetingID == a.Key
                                            && !w.IsDelete
                                             && w.IsActive
                                            && w.nStatusID != nCancel
                                            && (listRoom.Count() > 0
                                            ? listRoom.Contains(w.nRoomID)
                                            : true))
                                            select new
                                            {
                                                a,
                                                b
                                            };
                    List<objEventCalendar> letEventCalendars = new List<objEventCalendar>();
                    foreach (var item in lstEventCalendars)
                    {
                        objEventCalendar a = new objEventCalendar
                        {
                            sID = item.b.nMeetingID.EncryptParameter(),
                            groupId = item.b.nMeetingID + "",
                            dEventStart = item.b.dStart,
                            //dEventEnd = item.b.dEnd.HasValue && item.b.dStart == item.b.dEnd ? item.b.dEnd.Value.AddSeconds(30) : item.b.dEnd,
                            dEventEnd = item.b.IsAllDay ? item.b.dEnd.AddHours(6).AddMinutes(59) : item.b.dEnd,
                            title = item.b.nProjectID != null ? listProject.Where(w => item.b.nProjectID != null ? item.b.nProjectID == w.value.ToIntOrNull() : false).Select(s => s.label).FirstOrDefault() : "",
                            backgroundColor = "",
                            textColor = "#FF0000",
                            allDay = item.b.IsAllDay,
                            nStatus = item.b.nStatusID == 11 ? 1 : 0,
                            IsMeetingRoom = true,
                            sRoomName = TB_Room.FirstOrDefault(f => f.nRoomID == item.b.nRoomID)?.sRoomName,
                        };
                        letEventCalendars.Add(a);
                    }

                    if (dStart.HasValue && dEnd.HasValue)
                    {
                        letEventCalendars = letEventCalendars.Where(w => (w.sDisplayStart.Date <= dEnd.Value.Date && dStart.Value.Date <= w.sDisplayEnd.Date) &&
                                                    (w.sDisplayStart.TimeOfDay <= dEnd.Value.TimeOfDay && dStart.Value.TimeOfDay <= w.sDisplayEnd.TimeOfDay)).ToList();
                    }
                    STGrid.Pagination? dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, letEventCalendars.Count);

                    result.lstEvent = letEventCalendars.ToList();
                    result.nDataLength = dataPage.nDataLength;
                    result.nPageIndex = dataPage.nPageIndex;
                    result.nSkip = dataPage.nSkip;
                    result.nTake = dataPage.nTake;
                    result.nStartIndex = dataPage.nStartIndex;
                    result.sDateStart = param.dStart != null ? Convert.ToDateTime(param.dStart, CultureInfo.InvariantCulture) : null;
                    if (dStart.HasValue)
                    {
                        DateTime dCurrent = (DateTime)dStart;
                        List<objResultSearch> listResult = new List<objResultSearch>();
                        var listEventCalendar = (
                            from a in _db.TB_Meeting.Where(w => !w.IsDelete && w.IsActive && w.nStatusID == (int)ActionId.Booking
                                                   && (listRoom.Count() > 0
                                          ? listRoom.Contains(w.nRoomID)
                                          : true))
                            select new
                            {
                                a
                            });

                        for (DateTime i = dCurrent; i <= dEnd; i = i.AddDays(1))
                        {
                            string d2 = i.ToString("MM/dd/yyyy") + " 23:59";
                            DateTime? dPass = Convert.ToDateTime(d2, CultureInfo.InvariantCulture);
                            var doi = i.Date;
                            var iu = i.TimeOfDay;
                            List<objResultSearch> listRoomResult = new List<objResultSearch>();
                            foreach (var item in listEventCalendar)
                            {
                                objResultSearch a = new objResultSearch();
                                a.nRoomID = item.a.nRoomID;
                                a.sID = item.a.nMeetingID.EncryptParameter();
                                a.allDay = item.a.IsAllDay;
                                a.dEventStart = item.a.dStart;
                                a.dEventEnd = item.a.dEnd;
                                listRoomResult.Add(a);
                            }
                            listRoomResult = listRoomResult.Where(w =>
                                                (w.sDisplayStart.Value.Date <= dPass.Value.Date
                                                && i.Date <= w.sDisplayEnd.Value.Date)
                                                &&
                                                (w.sDisplayStart.Value.TimeOfDay <= dPass.Value.TimeOfDay
                                                && i.TimeOfDay <= w.sDisplayEnd.Value.TimeOfDay)).ToList();

                            List<int?> listRoomCheck = listRoomResult.GroupBy(g => g.nRoomID).Select(s => s.Key).ToList();
                            List<string> listsRoomNonBooking = listRoom.Count > 0 ?
                            listRoom.Where(w => !listRoomCheck.Contains(w)).Select(s => s + "").ToList() :
                            TB_Room.Where(w => !listRoomCheck.Contains(w.nRoomID)).Select(s => s.nRoomID + "").ToList();

                            List<int>? listnRoomNonBooking = listsRoomNonBooking.Select(int.Parse).ToList();

                            var listSearchResultNonBooking =
                                                 from c in TB_Room.Where(w => listnRoomNonBooking.Contains(w.nRoomID) && w.IsActive && !w.IsDelete)
                                                 from d in TB_Floor.Where(w => w.nFloorID == c.nFloorID && w.IsActive && !w.IsDelete).DefaultIfEmpty()
                                                 select new objResultSearch
                                                 {
                                                     nID = c.nRoomID,
                                                     nStatus = 0,
                                                     usetimes = sDatestart + " - " + sDateend + " น.",
                                                     usedates = i.ToStringFromDate("dd/MM/yyyy", "en-US"),
                                                     usetitle = c.sRoomName + " ชั้น " + d.sFloorName + " ห้อง " + c.sRoomCode
                                                 };
                            if (listRoomResult.Count() > 0)
                            {
                                var listSearchResultBooking = from j in listRoomResult.Where(w => listRoomCheck.Contains(w.nRoomID))
                                                              from c in TB_Room.Where(w => w.nRoomID == j.nRoomID && w.IsActive && !w.IsDelete)
                                                              from d in TB_Floor.Where(w => w.nFloorID == c.nFloorID && w.IsActive && !w.IsDelete).DefaultIfEmpty()
                                                              select new objResultSearch
                                                              {
                                                                  sID = j.sID,
                                                                  allDay = j.allDay,
                                                                  nStatus = 1,
                                                                  usetimes = j.allDay ? "09:00 - 18:00 น." : j.dEventStart.ToStringFromDate("HH:mm", "en-US") + " - " + j.dEventEnd.ToStringFromDate("HH:mm", "en-US") + " น.",
                                                                  usedates = i.ToStringFromDate("dd/MM/yyyy", "en-US"),
                                                                  usetitle = c.sRoomName + " ชั้น " + d.sFloorName + " ห้อง " + c.sRoomCode
                                                              };

                                listResult.AddRange(listSearchResultBooking.Union(listSearchResultNonBooking));
                            }

                            listResult.AddRange(listSearchResultNonBooking);

                        }

                        result.listSearchResult = listResult;
                    }
                    if (param.listRoom != null)
                    {
                        result.listRoom = param.listRoom.Count > 0 ? param.listRoom.Select(i => i.ToString()).ToList() : null;
                    }


                }
                else
                {
                    IQueryable<int>? nPositionID = _db.TB_Employee_Report_To.Where(w => ua.nUserID == w.nEmployeeID && !w.IsDelete).Select(s => s.nPositionID);
                    bool isCo = nPositionID.Any(w => w == (int)ActionType.Co);
                    int[]? intMTroom = null;
                    List<int>? intProject = new List<int>();
                    int[]? intStatus = null;
                    bool isOtherProject = false;
                    if (param.selectStatus?.Count() > 0)
                    {
                        intStatus = param.selectStatus.Select(int.Parse).ToArray();
                    }
                    if (param.selectMTroom?.Count() > 0)
                    {
                        intMTroom = param.selectMTroom.Select(int.Parse).ToArray();
                    }
                    if (param.selectProject?.Count() > 0)
                    {
                        foreach (var item in param.selectProject)
                        {
                            if (item == "0")
                            {
                                isOtherProject = true;
                            }
                            else
                            {
                                int nIPJ = item.ToInt();
                                intProject.Add(nIPJ);
                            }

                        }

                    }

                    var listCo = from empPosi in _db.TB_Employee_Report_To.Where(w => !w.IsDelete && w.nPositionID == (int)ActionType.Co)
                                 from emp in _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete && empPosi.nEmployeeID == w.nEmployeeID).DefaultIfEmpty()
                                 select new objEmployeeCo
                                 {
                                     sFullName = emp.sNameTH + " " + emp.sSurnameTH,
                                     nEmployeeID = emp.nEmployeeID
                                 };
                    TB_Meeting_Files[]? TBMeetingFiles = _db.TB_Meeting_Files.ToArray();
                    IEnumerable<IGrouping<int, int>>? listFile = TBMeetingFiles != null ? TBMeetingFiles.Select(s => s.nMeetingID).GroupBy(g => g) : null;
                    if (isOtherProject || intProject.Count() > 0)
                    {
                        TB_Meeting = TB_Meeting.Where(w => w.IsOther || intProject.Contains(w.nProjectID ?? 0));
                    }

                    if (intStatus != null)
                    {
                        TB_Meeting = TB_Meeting.Where(w => intStatus.Contains(w.nStatusID));
                    }
                    if (intMTroom != null)
                    {
                        TB_Meeting = TB_Meeting.Where(w => intMTroom.Contains(w.nRoomID ?? 0));
                    }
                    List<TB_Meeting_Flow?>? TBMeetingFlow = _db.TB_Meeting_Flow.GroupBy(l => l.nMeetingID)
                                        .Select(g => g.OrderByDescending(c => c.dAction).FirstOrDefault()).ToList();

                    IQueryable<objImage>? listEmpImage = from a in _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete)
                                                         from b in _db.TB_Employee_Image.Where(w => !w.IsDelete && a.nEmployeeID == w.nEmployeeID)
                                                         select new objImage
                                                         {
                                                             sPath = b.sPath,
                                                             sSystemFileName = b.sSystemFileName,
                                                             nEmployeeID = b.nEmployeeID,
                                                             nEmployeeImageID = b.nEmployeeImageID
                                                         };
                    IEnumerable<objEventCalendar>? listData = from a in TB_Meeting.Where(w => !isCo ? ua.nUserID == w.nCreateBy : true).ToArray()
                                                              from c in TB_Room.Where(tb => tb.nRoomID == a.nRoomID)
                                                              from d in TB_Floor.Where(tf => tf.nFloorID == c.nFloorID)
                                                              from f in TBMeetingFlow.Where(tmf => tmf?.nMeetingID == a.nMeetingID && a.nStatusID == tmf.nStatusID)
                                                              orderby a.dUpdate descending
                                                              select new objEventCalendar
                                                              {
                                                                  sID = a.nMeetingID.EncryptParameter(),
                                                                  dEventStart = a.dStart,
                                                                  dEventEnd = a.dEnd,
                                                                  nStatus = a.nStatusID,
                                                                  usedates = a.dStart.ToStringFromDate("dd/MM/yyyy", "en-US"),
                                                                  usedateE = a.dEnd.ToStringFromDate("dd/MM/yyyy", "en-US"),
                                                                  usetitle = c.sRoomName + " ชั้น " + d.sFloorName + " ห้อง " + c.sRoomCode,
                                                                  Topic = TB_Project.Any(w => (a.nProjectID != null ? a.nProjectID == w.nProjectID : false) && w.sProjectAbbr != null) ? " [ " + (a.nProjectID != null ? TB_Project.Where(w => a.nProjectID != null ? a.nProjectID == w.nProjectID : false).Select(s => s.sProjectAbbr + "").FirstOrDefault() : a.sOther) + " ] : " + a.sTitle : a.sTitle,
                                                                  sReq = TB_Employee.Any(f => f.nEmployeeID == a.nCreateBy) ? TB_Employee.Where(w => w.nEmployeeID == a.nCreateBy).Select(s => s.sNameTH + " " + s.sSurnameTH).FirstOrDefault() : "Request",
                                                                  sAppove = listCo.Any(ko => ko.nEmployeeID == f.nActionBy) ? listCo.FirstOrDefault(w => w.nEmployeeID == f.nActionBy)?.sFullName : "Role Co Project",
                                                                  dlastUpate = a.dUpdate,
                                                                  sRemark = f.sRemark,
                                                                  allDay = a.IsAllDay,
                                                                  usetimes = a.dStart.ToStringFromDate("HH:mm", "en-US") + " - " + a.dEnd.ToStringFromDate("HH:mm", "en-US") + " น.",
                                                                  isFile = listFile == null ? null : listFile.Select(s => s.Key).Contains(a.nMeetingID),
                                                                  nCountFile = TBMeetingFiles?.Where(w => w.nMeetingID == a.nMeetingID).Count(),
                                                                  sImgURLReq = listEmpImage.Any(f => f.nEmployeeID == a.nCreateBy) ? STFunction.GetPathUploadFile(listEmpImage.FirstOrDefault(f => f.nEmployeeID == a.nCreateBy)?.sPath ?? "", listEmpImage.FirstOrDefault(f => f.nEmployeeID == a.nCreateBy)?.sSystemFileName ?? "") : null,
                                                                  sImgURLCo = listCo.Any(f => f.nEmployeeID == a.nUpdateBy) ? STFunction.GetPathUploadFile(listEmpImage.FirstOrDefault(f => f.nEmployeeID == a.nUpdateBy)?.sPath ?? "", listEmpImage.FirstOrDefault(f => f.nEmployeeID == a.nUpdateBy)?.sSystemFileName ?? "") : null,
                                                              };

                    if ((int)ActionDate.BookingDate == param.DateType)
                    {
                        listData = listData.OrderByDescending(o => o.dEventStart).ThenByDescending(op => op.nST);
                        if (dStart.HasValue && !dEnd.HasValue)
                        {
                            listData = listData.Where(w => dStart.Value <= w.sDisplayEnd || w.sDisplayEnd == null);
                        }
                        else if (!dStart.HasValue && dEnd.HasValue)
                        {
                            listData = listData.Where(w => dEnd.Value >= w.sDisplayStart);
                        }
                        else if (dStart.HasValue && dEnd.HasValue)
                        {
                            listData = listData.Where(w =>
                                (w.sDisplayStart <= dStart.Value && w.sDisplayEnd >= dEnd.Value) ||
                                (w.sDisplayStart >= dStart.Value && w.sDisplayEnd <= dEnd.Value) ||
                                (w.sDisplayStart >= dStart.Value && w.sDisplayStart <= dEnd.Value) ||
                                (w.sDisplayEnd >= dStart.Value && w.sDisplayEnd <= dEnd.Value) ||
                                (w.sDisplayStart >= dStart.Value && w.sDisplayEnd == null && w.sDisplayStart <= dEnd.Value) ||
                                (w.sDisplayStart <= dStart.Value && w.sDisplayEnd == null)
                                                );
                        }
                    }
                    else if ((int)ActionDate.lastUpdate == param.DateType)
                    {

                        if (dStart.HasValue && !dEnd.HasValue)
                        {
                            listData = listData.Where(w => dStart.Value <= w.dlastUpate);
                        }
                        else if (!dStart.HasValue && dEnd.HasValue)
                        {
                            listData = listData.Where(w => dEnd.Value >= w.dlastUpate);
                        }
                        else if (dStart.HasValue && dEnd.HasValue)
                        {
                            listData = listData.Where(w =>
                                (w.dlastUpate <= dStart.Value && w.dlastUpate >= dEnd.Value) ||
                                (w.dlastUpate >= dStart.Value && w.dlastUpate <= dEnd.Value) ||
                                (w.dlastUpate >= dStart.Value && w.dlastUpate <= dEnd.Value) ||
                                (w.dlastUpate >= dStart.Value && w.dlastUpate <= dEnd.Value) ||
                                (w.dlastUpate >= dStart.Value && w.dlastUpate == null && w.dlastUpate <= dEnd.Value) ||
                                (w.dlastUpate <= dStart.Value && w.dlastUpate == null)
                                 );
                        }
                    }

                    if (!string.IsNullOrEmpty(param.sTopic))
                    {
                        listData = listData.Where(w => (w.usetitle != null ? w.usetitle.ToLower().Trim().Contains(param.sTopic.ToLower().Trim()) : false)
                                            || (w.sRemark != null ? w.sRemark.ToLower().Trim().Contains(param.sTopic.ToLower().Trim()) : false)
                                            || (w.Topic != null ? w.Topic.ToLower().Trim().Contains(param.sTopic.ToLower().Trim()) : false));
                    }


                    STGrid.Pagination? dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, listData.ToList().Count);
                    List<objEventCalendar>? letEventCalendar = listData.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                    result.lstEvent = letEventCalendar.ToList();
                    result.nDataLength = dataPage.nDataLength;
                    result.nPageIndex = dataPage.nPageIndex;
                    result.nSkip = dataPage.nSkip;
                    result.nTake = dataPage.nTake;
                    result.nStartIndex = dataPage.nStartIndex;
                }

                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        /// <summary>
        /// GetOption
        /// </summary>
        public clsResultMeeting GetOption(clsFilterCheckCo param)
        {
            clsResultMeeting result = new clsResultMeeting();
            var ua = _authen.GetUserAccount();
            try
            {

                var nPositionID = _db.TB_Employee_Report_To.Where(w => ua.nUserID == w.nEmployeeID && !w.IsDelete).Select(s => s.nPositionID);
                result.isPositionCo = nPositionID.Any(w => w == (int)ActionType.Co);
                bool isCo = nPositionID.Any(w => w == (int)ActionType.Co);
                int Co = (int)ActionType.Co;
                string? sName = _db.TB_Employee.Where(f => f.nEmployeeID == ua.nUserID && !f.IsDelete && f.IsActive).Select(s => s.sNameTH + " " + s.sSurnameTH).FirstOrDefault();
                var sPosition = (from a in _db.TB_Employee_Report_To.Where(w => w.nEmployeeID == ua.nUserID && !w.IsDelete)
                                 from b in _db.TB_Position.Where(s => s.nPositionID == a.nPositionID && !s.IsDelete && s.IsActive)
                                 select b.sPositionName).FirstOrDefault();
                result.sName = sName ?? "";
                result.Position = sPosition ?? "";

                var lstRoom = from a in _db.TB_Room.Where(w => !w.IsDelete && w.IsActive)
                              from b in _db.TB_Floor.Where(w => !w.IsDelete && w.nFloorID == a.nFloorID && w.IsActive).DefaultIfEmpty()
                              select new objRoom
                              {
                                  value = a.nRoomID + "",
                                  label = a.sRoomName + " ชั้น " + b.sFloorName + " ห้อง " + a.sRoomCode
                              };

                var listProject = _db.TB_Project.Where(w => !w.IsDelete).Select(s => new objRoom
                {
                    value = s.nProjectID + "",
                    label = !string.IsNullOrEmpty(s.sProjectAbbr) ? s.sProjectAbbr + " : " + s.sProjectName : s.sProjectName
                }).ToList();

                result.lstRoom = lstRoom.ToList();
                result.lstRoomAll = lstRoom.ToList();

                listProject.Add(new objRoom { value = "0", label = "อื่นๆ" });
                result.ProjectAll = listProject;
                if (isCo)
                {
                    result.Project = listProject;
                }


                if (isCo && param.isMode)
                {
                    var lstProjectCo = _db.TB_Project_Person.Where(w => w.nPositionID == Co && !w.IsDelete).Select(s => s.nProjectID + "").Distinct().ToList();
                    // lstProjectCo.Add("0");
                    result.CoProject = lstProjectCo;
                }

                if (!isCo)
                {
                    result.lstRoom = (from a in _db.TB_Meeting.Where(f => f.nCreateBy == ua.nUserID && !f.IsDelete && f.IsActive).GroupBy(g => g.nRoomID).ToList()
                                      from b in lstRoom.Where(w => w.value == a.Key.ToString())
                                      select new objRoom
                                      {
                                          value = b.value,
                                          label = b.label
                                      }).ToList();

                    listProject = listProject.Where(w => w.value != "0").ToList();
                    listProject = (from a in _db.TB_Meeting.Where(f => f.nCreateBy == ua.nUserID && !f.IsDelete && f.IsActive).GroupBy(g => g.nProjectID).ToList()
                                   from b in listProject.Where(w => w.value == a.Key.ToString())
                                   select new objRoom
                                   {
                                       value = b.value,
                                       label = b.label
                                   }).ToList();
                    bool isOther = _db.TB_Meeting.Any(f => f.nCreateBy == ua.nUserID && !f.IsDelete && f.nProjectID == 0 && f.IsActive);
                    if (isOther)
                    {
                        listProject.Add(new objRoom { value = "0", label = "อื่นๆ" });
                    }
                    result.Project = listProject;
                }


                var lstProcess = _db.TB_MasterProcess.Where(w => !w.IsDelete && w.IsActive).ToArray();
                result.Process = lstProcess.Select(s => new objRoom
                {
                    value = s.nMasterProcessID + "",
                    label = s.sMasterProcessName
                }).ToList();


                var lstStatus = _db.TM_Data.Where(w => !w.IsDelete && w.IsActive == true && w.nDatatypeID == 4 && w.IsActive)
                                .OrderBy(o => o.nOrder).Take(3).ToArray();
                result.lstStatus = lstStatus.Select(s => new objRoom
                {
                    value = s.nData_ID + "",
                    label = s.sNameTH
                }).ToList();

                var listPerson = _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive);
                result.Person = listPerson.Select(s => new objRoom
                {
                    value = s.nEmployeeID + "",
                    label = s.sNameTH + " " + s.sSurnameTH + "( " + s.sNickname + " )"
                }).ToList();

                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }
        /// <summary>
        /// GetPerson
        /// </summary>
        public clsResultMeeting GetPerson(int nProjectID)
        {
            clsResultMeeting result = new clsResultMeeting();
            try
            {
                var listPersonInProject = _db.TB_Project_Person.Where(w => w.nProjectID == nProjectID)
                                        .Select(s => s.nEmployeeID + "");
                result.PersonProject = listPersonInProject.ToList();
                //result.nCountPerson = listPersonInProject.Count();
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;

        }
        /// <summary>
        /// SaveForm
        /// </summary>
        public ResultAPI SaveForm(clsSaveMeeting obj)
        {
            var result = new ResultAPI();
            var ua = _authen.GetUserAccount();
            try
            {

                int nInprocess = 0;
                if (obj.nMode == 0)
                {
                    nInprocess = (int)ActionId.InProcess;
                }
                else if (obj.nMode == 1)
                {
                    nInprocess = (int)ActionId.Booking;
                }
                else if (obj.nMode == 2)
                {
                    nInprocess = (int)ActionId.Cancel;
                }
                int nid = string.IsNullOrEmpty(obj.sID) ? 0 : obj.sID.DecryptParameter().ToInt();
                // int nid = (int)(obj.nID != null ? obj.nID : 0);
                TB_Meeting? oData = nid != 0 ? _db.TB_Meeting.FirstOrDefault(f => f.nMeetingID == nid) : null;
                TB_Meeting_Event? oDataMeetingEvent = nid != 0 ? _db.TB_Meeting_Event.FirstOrDefault(f => f.nMeetingID == nid) : null;

                //Action From Line
                if (obj.IsActionFromLine == true && oData != null)
                {
                    TB_Log_WebhookLine? objLog = _db.TB_Log_WebhookLine.Where(w => w.sGUID == obj.sGUID).FirstOrDefault();
                    if (objLog != null)
                    {
                        oData.dUpdate = DateTime.Now;
                        oData.nUpdateBy = ua.nUserID;
                        oData.nStatusID = nInprocess;
                        objLog.IsActionAlready = true;
                        _db.SaveChanges();
                    }

                    // MeetingFlow
                    int newFlowId = (_db.TB_Meeting_Flow.Any() ? _db.TB_Meeting_Flow.Max(m => m.nMeetingFlowID) : 0) + 1;
                    TB_Meeting_Flow TBMeetingFlow = new()
                    {
                        nMeetingFlowID = newFlowId,
                        nMeetingID = oData.nMeetingID,
                        nStatusID = oData.nStatusID,
                        sRemark = obj.sRemarkCancel,
                        nActionBy = ua.nUserID,
                        dAction = DateTime.Now
                    };
                    _db.TB_Meeting_Flow.Add(TBMeetingFlow);
                    _db.SaveChanges();
                }
                else //Action From System
                {
                    DateTime? dDateStart = obj.sStart.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm).toUnixTime().toDateTime();
                    DateTime dDateEnd = obj.sEnd.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm).toUnixTime().toDateTime();
                    double dulationDay = (dDateEnd.Date - dDateStart.Value.Date).TotalDays;
                    double nDateLoop = dulationDay == 0 ? 1 : dulationDay + 1;
                    if (oData == null)
                    {
                        int newId = (_db.TB_Meeting.Any() ? _db.TB_Meeting.Max(m => m.nMeetingID) : 0) + 1;

                        oData = new TB_Meeting()
                        {
                            nMeetingID = newId,
                            dCreate = DateTime.Now,
                            nCreateBy = ua.nUserID,  // user.nEmpId
                            IsDelete = false,
                        };
                        _db.TB_Meeting.Add(oData);
                    }
                    int? nOther = null;
                    bool IscheckBoxOtherPJ = obj.checkBoxOtherPJ ?? false;
                    if (IscheckBoxOtherPJ)
                    {
                        nOther = 0;
                    }
                    oData.nProjectID = obj.selectProject != null ? obj.selectProject.ToInt() : nOther;
                    int? ParamRoom = obj.selectMTroom?.ToInt();
                    List<int> nPerson = _db.TB_Room.Where(w => ParamRoom == w.nRoomID).Select(s => s.nPerson).ToList();
                    oData.nRoomID = obj.selectMTroom != null ? obj.selectMTroom.ToInt() : null;
                    oData.nMasterProcessID = obj.selectProcess != null ? obj.selectProcess.ToInt() : null;
                    oData.sTitle = obj.sTitle;
                    oData.IsOther = IscheckBoxOtherPJ;
                    oData.sOther = obj.sOtherPJ;
                    oData.IsOtherProcess = obj.checkBoxOtherPC ?? false;
                    oData.sOtherProcess = obj.sOtherPC;
                    if (nPerson[0] < obj.nNop)
                    {
                        result.nStatusCode = StatusCodes.Status200OK;
                        result.sMessage = "จำนวนคนที่เข้าร่วมมากกว่าความจุห้อง";
                        return result;
                    }
                    oData.nPerson = obj.nNop;
                    oData.sRemark = obj.sRemark;
                    oData.IsAllDay = obj.IsAllDay ?? false;

                    oData.dStart = (DateTime)dDateStart;
                    oData.dEnd = dDateEnd;
                    oData.IsActive = true;
                    // user 
                    oData.nStatusID = nInprocess;
                    oData.dUpdate = DateTime.Now;
                    oData.nUpdateBy = ua.nUserID;

                    // Person
                    if (obj.selectAllPerson != null)
                    {
                        IQueryable<TB_Employee_Position>? TB_Employee_Position = _db.TB_Employee_Position.Where(w => !w.IsDelete);
                        IQueryable<TB_Meeting_Person>? oDataMeetingPerson = _db.TB_Meeting_Person.Where(w => w.nMeetingID == oData.nMeetingID);
                        if (oDataMeetingPerson.Count() > 0)
                        {
                            _db.TB_Meeting_Person.RemoveRange(oDataMeetingPerson);
                            _db.SaveChanges();
                        }
                        int nPersonMeetingID = _db.TB_Meeting_Person.Any() ? _db.TB_Meeting_Person.Max(m => m.nPersonMeetingID) + 1 : 1;
                        List<string> lstAllPerson = obj.selectAllPerson.ToList();
                        List<TB_Meeting_Person>? lstTBMeetingPerson = new List<TB_Meeting_Person>();
                        if (lstAllPerson.Count > 0)
                        {
                            foreach (string? item in lstAllPerson)
                            {
                                TB_Meeting_Person objInfoEmp = new TB_Meeting_Person()
                                {
                                    nPersonMeetingID = nPersonMeetingID,
                                    nMeetingID = oData.nMeetingID,
                                    nEmployeeID = item.ToInt(),
                                    dCreate = DateTime.Now,
                                    nCreateBy = ua.nUserID,
                                    dUpdate = DateTime.Now,
                                    nUpdateBy = ua.nUserID,
                                    IsDelete = false
                                };
                                lstTBMeetingPerson.Add(objInfoEmp);

                                nPersonMeetingID++;
                            }
                        }
                        _db.TB_Meeting_Person.AddRange(lstTBMeetingPerson);

                    }




                    // M_Event
                    var oDataME = _db.TB_Meeting_Event.Where(w => w.nMeetingID == oData.nMeetingID);
                    if (oDataME.Count() > 0)
                    {
                        _db.TB_Meeting_Event.RemoveRange(oDataME);
                        _db.SaveChanges();
                    }
                    long newIdME = (_db.TB_Meeting_Event.Any() ? _db.TB_Meeting_Event.Max(m => m.nEventID) : 0) + 1;
                    for (int i = 1; i <= nDateLoop; i++)
                    {
                        oDataMeetingEvent = new TB_Meeting_Event()
                        {
                            nMeetingID = oData.nMeetingID,
                            nEventID = newIdME,
                            dCreate = DateTime.Now,
                            nCreateBy = ua.nUserID,  // user.nEmpId
                            dUpdate = DateTime.Now,
                            nUpdateBy = ua.nUserID,
                            dEventStart = (DateTime)dDateStart,
                            dEventEnd = (DateTime)dDateEnd,
                            IsDelete = false,
                        };
                        _db.TB_Meeting_Event.Add(oDataMeetingEvent);
                        newIdME++;
                    }

                    // File
                    List<TB_Meeting_Files> lstInfoFile = _db.TB_Meeting_Files.Where(w => w.nMeetingID == oData.nMeetingID).ToList();
                    if (lstInfoFile.Count > 0)
                    {
                        _db.TB_Meeting_Files.RemoveRange(lstInfoFile);
                        _db.SaveChanges();
                    }
                    int nFileID = _db.TB_Meeting_Files.Any() ? _db.TB_Meeting_Files.Max(m => m.nFileID) + 1 : 1;
                    var lstAll = obj.fFile?.ToList();
                    var lstD = new List<TB_Meeting_Files>();
                    if (lstAll != null && lstAll.Count() > 0)
                    {
                        foreach (var item in lstAll)
                        {
                            string pathTempContent = !string.IsNullOrEmpty(item.sFolderName) ? item.sFolderName + "/" : "Temp/";

                            string truePathFile = "Meeting\\" + oData.nMeetingID + "\\";
                            string fPathContent = "Meeting/" + oData.nMeetingID;

                            item.sFolderName = pathTempContent;
                            if (item.IsNew)
                            {
                                // SystemFunction.MoveFile(pathTempContent, truePathFile, item.sSysFileName != null ? item.sSysFileName : "", _env);
                                var path = STFunction.MapPath(truePathFile, _env);
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }

                                var ServerTempPath = STFunction.MapPath(pathTempContent, _env);
                                var ServerTruePath = STFunction.MapPath(truePathFile, _env);

                                string FileTempPath = STFunction.Scan_CWE22_File(ServerTempPath, item.sSysFileName != null ? item.sSysFileName : "");
                                string FileTruePath = STFunction.Scan_CWE22_File(ServerTruePath, item.sSysFileName != null ? item.sSysFileName : "");
                                if (File.Exists(FileTempPath))
                                {
                                    if (FileTempPath != FileTruePath)
                                    {
                                        File.Move(FileTempPath, FileTruePath);
                                    }
                                }
                            }
                            TB_Meeting_Files objInfofile = new TB_Meeting_Files()
                            {
                                nMeetingID = oData.nMeetingID,
                                nFileID = nFileID,
                                sPath = fPathContent,
                                sSystemFilename = item.sSysFileName != null ? item.sSysFileName : "",
                                sFilename = item.sSysFileName != null ? item.sSysFileName : "",
                                dCreate = DateTime.Now,
                                nCreateBy = ua.nUserID,
                                dUpdate = DateTime.Now,
                                nUpdateBy = ua.nUserID,
                                IsDelete = false
                            };
                            lstD.Add(objInfofile);

                            nFileID++;
                        }
                    }


                    _db.TB_Meeting_Files.AddRange(lstD);

                    // MeetingFlow
                    int newFlowId = (_db.TB_Meeting_Flow.Any() ? _db.TB_Meeting_Flow.Max(m => m.nMeetingFlowID) : 0) + 1;
                    TB_Meeting_Flow TBMeetingFlow = new TB_Meeting_Flow()
                    {
                        nMeetingFlowID = newFlowId,
                        nMeetingID = oData.nMeetingID,
                        nStatusID = oData.nStatusID,
                        sRemark = obj.sRemarkCancel,
                        nActionBy = ua.nUserID,
                        dAction = DateTime.Now
                    };
                    _db.TB_Meeting_Flow.Add(TBMeetingFlow);

                    _db.SaveChanges();
                }

                if (nInprocess != (int)ActionId.Cancel)
                {
                    #region Send Line Noti
                    string sGUID = Guid.NewGuid().ToString();

                    IQueryable<objRoom>? arrRoom = from a in _db.TB_Room.Where(w => !w.IsDelete && oData.nRoomID == w.nRoomID)
                                                   from b in _db.TB_Floor.Where(w => !w.IsDelete && a.nFloorID == w.nFloorID).DefaultIfEmpty()
                                                   select new objRoom
                                                   {
                                                       value = a.nRoomID + "",
                                                       label = a.sRoomName + " ชั้น " + b.sFloorName + " ห้อง " + a.sRoomCode
                                                   };
                    string? sProject = _db.TB_Project.Where(w => w.nProjectID == oData.nProjectID).Select(s => s.sProjectName).FirstOrDefault();
                    sProject = oData.nProjectID == 0 ? "อื่นๆ" : sProject;

                    TM_Data[]? arrStatus = _db.TM_Data.ToArray();
                    int nMode = 0;

                    var listCo = _db.TB_Employee_Report_To.Where(w => w.nPositionID == (int)ActionType.Co).Select(s => s.nEmployeeID).ToList();
                    string sApproveBy = listCo.FirstOrDefault().EncryptParameter();

                    cParamSendLine objParam = new()
                    {
                        sGUID = sGUID,
                        nRequesterID = ua.nUserID,
                        sDate = oData.dUpdate.ToStringFromDate("dd/MM/yyyy"),
                        sTime = oData.dUpdate.ToStringFromDate("HH:mm") + " น.",
                        sTitle = oData.sTitle ?? ""
                    };
                    if (oData.IsAllDay)
                    {
                        if (oData.dEnd.Date == oData.dStart.Date)
                        {
                            objParam.sStartDate = oData.dStart.ToStringFromDate("dd/MM/yyyy");
                        }
                        else
                        {
                            objParam.sStartDate = oData.dStart.ToStringFromDate("dd/MM/yyyy");
                            objParam.sEndDate = " - " + oData.dEnd.ToStringFromDate("dd/MM/yyyy");
                        }
                    }
                    else
                    {
                        objParam.sStartDate = oData.dStart.ToStringFromDate("dd/MM/yyyy");
                        objParam.sEndDate = " - " + oData.dEnd.ToStringFromDate("dd/MM/yyyy");
                    }
                    objParam.sStartTime = oData.dStart.ToStringFromDate("HH:mm");
                    objParam.sEndTime = " - " + oData.dEnd.ToStringFromDate("HH:mm") + " น.";
                    objParam.sRoom = arrRoom.Select(s => s.label).FirstOrDefault() ?? "";
                    objParam.sProject = sProject ?? "";
                    objParam.sStatus = arrStatus.Where(w => w.nData_ID == nInprocess).Select(s => s.sNameTH).FirstOrDefault() ?? "";
                    objParam.sPathApprove = "ApproveMTLine&sID=" + oData.nMeetingID.EncryptParameter() + "&sUserID=" + sApproveBy + "&sGUID=" + sGUID +
                                            "&dStart=" + oData.dStart.toUnixTime() + "&dEnd=" + oData.dEnd.toUnixTime() + "&nRoom=" + oData.nRoomID.ToInt().EncryptParameter() + "&IsAllDay=" + (oData.IsAllDay ? 1 : 0).EncryptParameter();
                    objParam.sPathReject = "CancelAndRejectMTLine&sID=" + oData.nMeetingID.EncryptParameter() + "&sUserID=" + sApproveBy;
                    objParam.sPathCancel = "CancelAndRejectMTLine&sID=" + oData.nMeetingID.EncryptParameter() + "&sUserID=" + sApproveBy + "&sGUID=" + sGUID + "&nStatusID=" + (int)ActionId.Cancel;
                    switch (nInprocess)
                    {
                        case (int)ActionId.InProcess: //Inprogress
                            nMode = 2;
                            objParam.nTemplateID = 33;
                            objParam.lstEmpTo = listCo;
                            objParam.sPathDetail = "DetailMTLine&sID=" + oData.nMeetingID.EncryptParameter() + "&sUserID=" + sApproveBy + "&mode=" + nMode.EncryptParameter();
                            break;
                        case (int)ActionId.Booking: //Booking
                            nMode = 3;
                            objParam.nTemplateID = 34;
                            objParam.lstEmpTo = new List<int> { oData.nCreateBy };
                            objParam.sPathDetail = "DetailMTLine&sID=" + oData.nMeetingID.EncryptParameter() + "&sUserID=" + sApproveBy + "&mode=" + nMode.EncryptParameter();
                            break;
                    }
                    STFunction.SendToLine(objParam);
                    #endregion Send Line Noti 

                }

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;

        }
        /// <summary>
        /// GetData
        /// </summary>

        public clsSaveMeeting GetData(string nID, string? Mode)
        {
            clsSaveMeeting result = new clsSaveMeeting();
            UserAccount? ua = _authen.GetUserAccount();
            try
            {
                int nid = nID.DecryptParameter().ToInt();
                //int nid = string.IsNullOrEmpty(nID) ? 0 : nID.DecryptParameter().ToInt();
                // int nid = nID;
                IQueryable<TB_Meeting> TBMeeting = _db.TB_Meeting.AsQueryable();
                TB_Meeting? oData = nid != 0 ? _db.TB_Meeting.FirstOrDefault(f => f.nMeetingID == nid) : null;
                if (oData != null)
                {
                    var TBMeetingFiles = _db.TB_Meeting_Files.Where(w => w.nMeetingID == nid);
                    List<ItemFileData> lstfile = new List<ItemFileData>();
                    if (TBMeetingFiles.Count() > 0)
                    {
                        foreach (var item in TBMeetingFiles)
                        {
                            string? sFullPath = null;
                            if (!string.IsNullOrEmpty(item.sPath) && !string.IsNullOrEmpty(item.sSystemFilename))
                            {
                                string sPathWeb = STFunction.GetAppSettingJson("AppSetting:UrlSiteBackend");
                                StringBuilder sb = new StringBuilder();
                                sb.AppendFormat("{0} {1} {2} {3}", sPathWeb, "UploadFile/", item.sPath + "/", item.sSystemFilename);
                                sFullPath = sb.ToString(); //string.builder
                            }
                            ItemFileData fFile = new ItemFileData();
                            //fFile.sFileID = item.nFileID + "";
                            fFile.sFileName = item.sFilename;
                            fFile.sFileLink = sFullPath;
                            fFile.sSysFileName = item.sSystemFilename;
                            fFile.sFileType = item.sSystemFilename.Split(".")[1] + "";
                            fFile.sFolderName = item.sPath;
                            fFile.IsNew = false;
                            fFile.IsNewTab = false;
                            fFile.IsCompleted = true;
                            fFile.IsDelete = false;
                            fFile.IsProgress = false;
                            fFile.sProgress = "100";
                            lstfile.Add(fFile);
                        }
                    }
                    string? Copy = null;
                    if (!string.IsNullOrEmpty(Mode))
                    {
                        Copy = Mode;
                    }
                    IQueryable<TB_Employee> TBEmployee = _db.TB_Employee.AsQueryable();
                    IQueryable<TB_Position> TBPosition = _db.TB_Position.AsQueryable();
                    IQueryable<TB_Employee_Position> TBEmployeePosition = _db.TB_Employee_Position.AsQueryable();
                    string? sName = TBEmployee.Where(f => Copy != null ? f.nEmployeeID == ua.nUserID : f.nEmployeeID == oData.nCreateBy).Select(s => s.sNameTH + " " + s.sSurnameTH).FirstOrDefault();
                    var sPosition = (from a in TBEmployeePosition.Where(w => Copy != null ? w.nEmployeeID == ua.nUserID : w.nEmployeeID == oData.nCreateBy)
                                     from b in TBPosition.Where(s => s.nPositionID == a.nPositionID)
                                     select b.sPositionName).FirstOrDefault();

                    result.fFile = lstfile.ToList();
                    result.sName = sName;
                    result.sTitle = oData.sTitle;
                    result.sOtherPC = oData.sOtherProcess;
                    result.sOtherPJ = oData.sOther;
                    result.sPosition = sPosition;
                    result.selectProject = oData.nProjectID + "";
                    result.selectProcess = oData.nMasterProcessID + "";
                    result.selectMTroom = oData.nRoomID + "";
                    result.nNop = oData.nPerson;
                    result.checkBoxOtherPJ = oData.IsOther;
                    result.checkBoxOtherPC = oData.IsOtherProcess;
                    result.IsAllDay = oData.IsAllDay;
                    result.IsActive = oData.IsActive;
                    result.dEnd = oData.dEnd;
                    result.dStart = oData.dStart;
                    result.nMode = oData.nStatusID == (int)ActionId.Booking ? 3 : oData.nStatusID == (int)ActionId.InProcess ? 2 : 4;

                    DateTime? dDateEnd = DateTime.Now;
                    double dulationDay = (dDateEnd.Value.Date - (oData.dEnd != null ? oData.dEnd.Date : oData.dStart.Date)).TotalDays;
                    result.IsPass = false;
                    if (dulationDay > 0 && oData.nStatusID == (int)ActionId.Booking)
                    {
                        result.IsPass = true;
                    }

                    result.sRemark = oData.sRemark != null ? oData.sRemark : null;
                    var list = (from a in _db.TB_Meeting_Person.Where(w => !w.IsDelete && oData.nMeetingID == w.nMeetingID)
                                from b in _db.TB_Employee.Where(w => !w.IsDelete && a.nEmployeeID == w.nEmployeeID)
                                select new objRoom
                                {
                                    value = a.nEmployeeID + "",
                                    label = b.sNameTH + " " + b.sSurnameTH
                                }).ToList();
                    List<TablePersonName> lstPersonName = new();
                    int nIndx = 1;
                    foreach (var item in list)
                    {
                        TablePersonName objPersonName = new();
                        objPersonName.Name = item.label ?? "";
                        objPersonName.sID = nIndx.ToString();
                        lstPersonName.Add(objPersonName);
                        nIndx++;
                    }
                    result.selectAllPersonName = lstPersonName;
                    result.selectAllPerson = list.Select(s => s.value ?? "").ToList();
                    result.IsActive = oData.IsActive;
                    result.IsBookMine = oData.nCreateBy == ua.nUserID;

                }
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;

            }

            return result;


        }

        /// <summary>
        /// GetListRoom
        /// </summary>
        public clsResultMeeting GetListRoom()
        {
            clsResultMeeting result = new clsResultMeeting();
            try
            {
                var lstRoom = from a in _db.TB_Room.Where(w => !w.IsDelete && w.IsActive)
                              from b in _db.TB_Floor.Where(w => !w.IsDelete && w.nFloorID == a.nFloorID && w.IsActive).DefaultIfEmpty()
                              select new
                              {
                                  a,
                                  b,
                              };
                result.listContent = lstRoom.OrderBy(o => o.a.nOrder).Select(s => new objRoomDetail
                {
                    sID = s.a.nRoomID,
                    Name = s.a.sRoomName,
                    Floor = s.b.sFloorName,
                    Seating = s.a.nPerson + "",
                    Room = s.a.sRoomCode,
                    Equipment = s.a.sEquipment + "",
                    IsCanDel = _db.TB_Meeting.Any(w => !w.IsDelete && w.IsActive && w.nRoomID == s.a.nRoomID),
                    sImgURL = (s.a.sPath != null && s.a.sSystemFilename != null)
                    ? STFunction.GetPathUploadFile(s.a.sPath, s.a.sSystemFilename) : null
                }).ToList();
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }
        /// <summary>
        /// GetFileZip Download File Zip
        /// </summary>
        public cFileZip GetFileZip(cFileID param)
        {
            cFileZip result = new cFileZip();
            using (ST_IntranetEntity _db = new ST_IntranetEntity())
            {
                string sFileName = "ZipALLFile_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".zip";
                string ToPathZip = "AllZipFile\\";
                var pather = _env.ContentRootPath + "wwwroot\\UploadFile\\AllZipFile";
                // var pather = STFunction.MapPath("AllZipFile", _env);
                if (Directory.Exists(pather))
                {
                    Directory.Delete(pather, true);
                }

                #region GetData File Document
                List<cFileDocument> aFile = new List<cFileDocument>();
                List<TB_Meeting_Files> lstCenterAll = _db.TB_Meeting_Files.Where(w => param.sID.Contains(w.nFileID)).ToList();
                int index = 1;
                foreach (var i in lstCenterAll)
                {
                    string sFilePath = i.sPath.Replace("UploadFile/", "").Replace("/", "\\") + "";
                    string sTypeFileAll = i.sSystemFilename.Split('.')[1] + "";

                    //var path = STFunction.MapPath(sFilePath, _env);
                    var path = _env.ContentRootPath + "wwwroot\\UploadFile\\" + sFilePath;
                    if (Directory.Exists(path))
                    {
                        string FilePath = path + "\\" + i.sSystemFilename;
                        if (File.Exists(FilePath))
                        {

                            // SystemFunction.FolderCreate(ToPathZip, _env);
                            //var ServerToPathZip = STFunction.MapPath(ToPathZip, _env);
                            var ServerToPathZip = _env.ContentRootPath + "wwwroot\\UploadFile\\AllZipFile";
                            var pathCreate = ServerToPathZip;
                            if (!Directory.Exists(pathCreate))
                            {
                                Directory.CreateDirectory(pathCreate);
                            }
                            string FileToPathZip = ServerToPathZip + "\\" + index + " - " + i.sFilename;
                            if (FilePath != FileToPathZip)
                            {
                                System.IO.File.Copy(FilePath, FileToPathZip);
                            }
                            cFileDocument a = new cFileDocument();
                            a.sFileName_Sys = index + " - " + i.sFilename;
                            a.sFilePath = ServerToPathZip;
                            a.sTypeFileAll = sTypeFileAll;
                            a.nCenterID = i.nMeetingID;
                            aFile.Add(a);
                        }
                    }
                    index++;
                }
                #endregion

                #region Read File Document
                if (aFile.Count() > 0)
                {
                    var zipPath = _env.ContentRootPath + "wwwroot\\UploadFile\\" + ToPathZip + sFileName;
                    List<FileModel> sourceFileList = new List<FileModel>();
                    foreach (var item in aFile)
                    {
                        string fileName = item.sFileName_Sys != null ? item.sFileName_Sys + "" : "";

                        sourceFileList.Add(new FileModel()
                        {
                            fileStream = new FileStream(item.sFilePath + "\\" + item.sFileName_Sys, FileMode.Open),
                            fileName = fileName,
                        });
                    }

                    using (Ionic.Zip.ZipFile zipFile = new Ionic.Zip.ZipFile(System.Text.Encoding.UTF8))
                    {
                        zipFile.AlternateEncodingUsage = ZipOption.AsNecessary;
                        foreach (var item in sourceFileList)
                        {
                            zipFile.AddEntry(item.fileName, item.fileStream);
                        }
                        zipFile.Save(zipPath);
                    }

                    FileStream CfileStream = new FileStream(zipPath, System.IO.FileMode.Open);
                    result.objFile = CfileStream;
                    result.sFileType = "application/zip";
                    result.sFileName = sFileName;
                }
                #endregion
            }





            return result;


        }
        /// <summary>
        /// GetAllInprogress
        /// </summary>

        public clsFilter GetAllInprogress(objGetData obj)
        {
            clsFilter result = new clsFilter();
            try
            {
                int nid = obj.nID.DecryptParameter().ToInt();
                DateTime? dDateStart = Convert.ToDateTime(obj.sStart, CultureInfo.InvariantCulture);
                DateTime? dDateEnd = Convert.ToDateTime(obj.sEnd, CultureInfo.InvariantCulture);
                int nRoomID = !string.IsNullOrEmpty(obj.nRoom) ? obj.nRoom.ToInt() : 0;
                int nInprocess = 0;
                if (obj.mode == 0)
                {
                    nInprocess = (int)ActionId.InProcess;
                }
                else if (obj.mode == 1)
                {
                    nInprocess = (int)ActionId.Booking;
                }
                else if (obj.mode == 2)
                {
                    nInprocess = (int)ActionId.Cancel;
                }
                if (nInprocess == (int)ActionId.Booking)
                {
                    IEnumerable<objCheckTotal>? listCheck = (from a in _db.TB_Meeting.Where(w => w.nMeetingID != nid && w.nRoomID == nRoomID
                                                    && !w.IsDelete && w.nStatusID == (int)ActionId.Booking).ToArray()
                                                             from c in _db.TB_Project.Where(w => w.nProjectID == a.nProjectID)
                                                             select new objCheckTotal
                                                             {
                                                                 isAllDay = a.IsAllDay,
                                                                 dStart = a.dStart,
                                                                 dEnd = a.dEnd,
                                                                 dulationDay = (a.dEnd.Date - dDateStart.Value.Date).TotalDays,
                                                                 nID = a.nMeetingID,
                                                                 sProject = !string.IsNullOrEmpty(c.sProjectAbbr) ? " [ " + c.sProjectAbbr + " ] " + a.sTitle : a.sTitle
                                                             });
                    listCheck = listCheck.Where(w =>
                     (w.dStart.Date <= dDateEnd.Value.Date && dDateStart.Value.Date <= w.dEnd.Date)
                    &&
                    (w.dStart.TimeOfDay <= dDateEnd.Value.TimeOfDay && dDateStart.Value.TimeOfDay <= w.dEnd.TimeOfDay));
                    //var Test = listCheck.Select(s => new { s. });
                    // Booking Complete dulationDay = 0 อยู่ในช่วงเวลาไหน
                    StringBuilder txt = new();
                    string sDetail = "";
                    if (listCheck.Count() > 0)
                    {
                        foreach (var item in listCheck)
                        {
                            if (item.isAllDay == true && item.dStart.Date == item.dEnd.Date)
                            {
                                txt.AppendFormat("{0}<br>{1}{2}<br>", item.sProject + " ได้จองห้องประชุมนี้ในช่วงเวลา", "Booking Date : " + item.dStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy), "เวลา ทั้งวัน");

                            }
                            else if (item.isAllDay == true && item.dStart.Date != item.dEnd.Date)
                            {
                                txt.AppendFormat("{0}<br>{1}{2}<br>", item.sProject + " ได้จองห้องประชุมนี้ในช่วงเวลา", "Booking Date : " + item.dStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy) + item.dEnd.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy), "เวลา ทั้งวัน");
                            }
                            else
                            {
                                txt.AppendFormat("{0}<br>{1}<br>{2}{3}", item.sProject + " ได้จองห้องประชุมนี้ในช่วงเวลา", "Booking Date : " + item.dStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy) + item.dEnd.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy), "Booking Time : " + item.dStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.HHmm) + " - " + item.dEnd.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.HHmm), "น.");
                            }
                            sDetail += txt.ToString();
                        }
                        result.sDetail = sDetail;
                        result.listAllData = listCheck.ToList();
                        result.nStatusCode = StatusCodes.Status200OK;
                        result.sMessage = "ช่วงวันเวลา การจองห้องประชุมซ้ำ";
                        return result;
                    }
                    result.sDetail = "";
                    result.listAllData = listCheck.ToList();
                    result.nStatusCode = StatusCodes.Status200OK;
                    result.sMessage = "Not Complete";
                    return result;
                }
                else
                {
                    var listCheck = (from a in _db.TB_Meeting.Where(w => w.nMeetingID != nid && w.nRoomID == nRoomID
                                                    && !w.IsDelete && w.nStatusID != (int)ActionId.Cancel).ToArray()
                                     from c in _db.TB_Project.Where(w => w.nProjectID == a.nProjectID)
                                     select new objCheckTotal
                                     {
                                         isAllDay = a.IsAllDay,
                                         dStart = a.dStart,
                                         dEnd = a.dEnd,
                                         dulationDay = (a.dEnd.Date - dDateStart.Value.Date).TotalDays,
                                         nID = a.nMeetingID,
                                         sProject = string.IsNullOrEmpty(c.sProjectAbbr) ? " [ " + c.sProjectAbbr + " ] " + a.sTitle : a.sTitle
                                     });
                    listCheck = listCheck.Where(w => (w.dStart.Date <= dDateEnd.Value.Date && dDateStart.Value.Date <= w.dEnd.Date)
                   &&
                   (w.dStart.TimeOfDay <= dDateEnd.Value.TimeOfDay && dDateStart.Value.TimeOfDay <= w.dEnd.TimeOfDay));
                    StringBuilder txt = new();
                    string sDetail = "";
                    if (listCheck.Count() > 0)
                    {
                        foreach (var item in listCheck)
                        {
                            if (item.isAllDay == true && item.dStart.Date == item.dEnd.Date)
                            {
                                txt.AppendFormat("{0}<br>{1}{2}<br>", item.sProject + " ได้จองห้องประชุมนี้ในช่วงเวลา", "Booking Date : " + item.dStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy), "เวลา ทั้งวัน");

                            }
                            else if (item.isAllDay == true && item.dStart.Date != item.dEnd.Date)
                            {
                                txt.AppendFormat("{0}<br>{1}{2}", item.sProject + " ได้จองห้องประชุมนี้ในช่วงเวลา", "Booking Date : " + item.dStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy) + " - " + item.dEnd.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy), "เวลา ทั้งวัน");
                            }
                            else
                            {
                                txt.AppendFormat("{0}<br>{1}<br>{2}{3}", item.sProject + " ได้จองห้องประชุมนี้ในช่วงเวลา", "Booking Date : " + item.dStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy) + " - " + item.dEnd.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy), "Booking Time : " + item.dStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.HHmm) + " - " + item.dEnd.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.HHmm), "น.");
                            }
                            sDetail += txt.ToString();
                        }
                        result.sDetail = sDetail;
                        result.listAllData = listCheck.ToList();
                        result.nStatusCode = StatusCodes.Status200OK;
                        result.sMessage = "Inprogress";
                        return result;
                    }
                }


                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;

            }

            return result;
        }
        /// <summary>
        /// SaveDataRoom
        /// </summary>
        public ResultAPI SaveDataRoom(cSaveRoom req)
        {
            ResultAPI result = new ResultAPI();
            var ua = _authen.GetUserAccount();
            try
            {
                #region Room
                int nFloorID = 0;
                int nRoomID = (req.sID ?? "").ToInt();
                // var Image = req.fFile;
                TB_Room? objRoom = _db.TB_Room.FirstOrDefault(w => w.nRoomID == nRoomID && w.IsDelete != true && w.IsActive == true);
                int nOrder = _db.TB_Room.Where(w => !w.IsDelete).Any() ? _db.TB_Room.Where(w => !w.IsDelete).Select(s => s.nOrder).Max() + 1 : 1;

                TB_Floor? objFloor = _db.TB_Floor.FirstOrDefault(w => (w.sFloorName + "").Trim().ToLower() == (req.sFloorName + "").Trim().ToLower() && w.IsDelete != true);
                if (objFloor != null) //Edit
                {
                    nFloorID = objFloor.nFloorID;
                    objFloor.sFloorName = req.sFloorName;
                    objFloor.dUpdate = DateTime.Now;
                    objFloor.nUpdateBy = ua.nUserID;
                }
                else //Add
                {
                    int nOrderFloor = _db.TB_Floor.Where(w => !w.IsDelete).Any() ? _db.TB_Floor.Where(w => !w.IsDelete).Select(s => s.nOrder).Max() + 1 : 1;
                    objFloor = new TB_Floor();
                    nFloorID = (_db.TB_Floor.Any() ? _db.TB_Floor.Max(m => m.nFloorID) : 0) + 1;
                    objFloor.nFloorID = nFloorID;
                    objFloor.dCreate = DateTime.Now;
                    objFloor.nOrder = nOrderFloor;
                    objFloor.IsActive = true;
                    objFloor.nCreateBy = ua.nUserID;
                    objFloor.nUpdateBy = ua.nUserID;
                    objFloor.IsDelete = false;
                    objFloor.sFloorName = req.sFloorName;
                    objFloor.dUpdate = DateTime.Now;
                    _db.TB_Floor.Add(objFloor);

                }
                _db.SaveChanges();

                if (objRoom == null)  //Add
                {
                    var isDuplicateAdd = _db.TB_Room.Where(w => ((w.sRoomName + "").Trim().ToLower() == (req.sRoomName + "").Trim().ToLower()) ||
                    ((w.sRoomCode + "").Trim().ToLower() == (req.sRoomCode + "").Trim().ToLower()) && w.IsDelete != true).Any();
                    if (isDuplicateAdd)
                    {
                        result.nStatusCode = (int)APIStatusCode.Warning;
                        result.sMessage = "ชื่อห้องหรือเลขห้องมีอยู่แล้วในระบบ!!";
                        return result;
                    }
                    objRoom = new TB_Room();
                    objRoom.nRoomID = (_db.TB_Room.Any() ? _db.TB_Room.Max(m => m.nRoomID) : 0) + 1;
                    nRoomID = objRoom.nRoomID;
                    objRoom.nFloorID = nFloorID;
                    objRoom.dCreate = DateTime.Now;
                    objRoom.nOrder = nOrder;
                    objRoom.nCreateBy = ua.nUserID;
                    objRoom.IsDelete = false;
                    _db.TB_Room.Add(objRoom);
                }
                objRoom.nRoomID = nRoomID;
                objRoom.sRoomName = req.sRoomName;
                objRoom.sRoomCode = req.sRoomCode;
                objRoom.sEquipment = req.sEquipment;
                objRoom.IsActive = true;
                objRoom.nPerson = req.nPerson;
                objRoom.dUpdate = DateTime.Now;
                objRoom.nUpdateBy = ua.nUserID;

                #region File
                var objAll = req.fFile?.FirstOrDefault();
                if (objAll != null)
                {
                    string pathTempContent = !string.IsNullOrEmpty(objAll.sFolderName) ? objAll.sFolderName + "/" : "Temp/ProfileRetangle/";

                    string truePathFile = "MeetingRoom\\" + nRoomID + "\\";
                    string fPathContent = "MeetingRoom/" + nRoomID;

                    objAll.sFolderName = pathTempContent;
                    if (objAll.IsNew)
                    {

                        var path = STFunction.MapPath(truePathFile, _env);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var ServerTempPath = STFunction.MapPath(pathTempContent, _env);
                        var ServerTruePath = STFunction.MapPath(truePathFile, _env);

                        string FileTempPath = STFunction.Scan_CWE22_File(ServerTempPath, objAll.sSysFileName != null ? objAll.sSysFileName : "");
                        string FileTruePath = STFunction.Scan_CWE22_File(ServerTruePath, objAll.sSysFileName != null ? objAll.sSysFileName : "");
                        if (File.Exists(FileTempPath))
                        {
                            if (FileTempPath != FileTruePath)
                            {
                                File.Move(FileTempPath, FileTruePath);
                            }
                        }
                    }

                    objRoom.sPath = fPathContent;
                    objRoom.sSystemFilename = objAll.sSysFileName != null ? objAll.sSysFileName : "";
                    objRoom.sFilename = objAll.sSysFileName != null ? objAll.sSysFileName : "";
                }
                #endregion

                _db.SaveChanges();
                #endregion
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }
        /// <summary>
        /// GetDataTableFileALL
        /// </summary>
        public clsFileAllTable GetDataTableFileALL(cFilterTable param)
        {
            clsFileAllTable result = new clsFileAllTable();
            try
            {
                int nid = param.sID.DecryptParameter().ToInt();
                List<TB_Meeting_Files>? listData = _db.TB_Meeting_Files.Where(w => w.nMeetingID == nid).ToList();
                IEnumerable<objResultFileAllTable>? qry = listData.Select((s, index) => new objResultFileAllTable
                {
                    sID = s.nFileID,
                    nMeetingID = s.nMeetingID,
                    sNameDoc = s.sFilename,
                    sDate = s.dCreate.ToStringFromDate("dd/MM/yyyy HH:mm", "en-US")
                });
                #region//SORT
                string sSortColumn = param.sSortExpression;
                if (param.isASC)
                {
                    qry = qry.OrderBy<objResultFileAllTable>(sSortColumn);
                }
                else if (param.isDESC)
                {
                    qry = qry.OrderByDescending<objResultFileAllTable>(sSortColumn);
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page


                result.lstData = qry.ToList();

                #endregion

                result.Status = StatusCodes.Status200OK;

            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;

            }

            return result;

        }
        /// <summary>
        /// DeleteRoom
        /// </summary>
        public ResultAPI DeleteRoom(int nID)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                List<TB_Room> lsitTBRoom = _db.TB_Room.Where(w => w.nRoomID == nID).ToList();
                foreach (var item in lsitTBRoom)
                {
                    item.IsDelete = true;
                    item.IsActive = false;
                    item.nOrder = 0;
                    item.nDeleteBy = 1;
                    item.dDelete = DateTime.Now;
                }
                _db.SaveChanges();
                result.nStatusCode = StatusCodes.Status200OK;

            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;

            }
            return result;
        }
        /// <summary>
        /// GetDataRoom
        /// </summary>
        public cSaveRoom GetDataRoom(int nID)
        {
            cSaveRoom result = new cSaveRoom();
            try
            {
                //int nid = string.IsNullOrEmpty(sID) ? 0 : (sID ?? "").DecryptParameter().ToInt();
                int nid = nID;
                var oData = nid != 0 ? _db.TB_Room.FirstOrDefault(f => f.nRoomID == nid) : null;
                var oDataFloor = _db.TB_Floor.FirstOrDefault(f => f.nFloorID == oData.nFloorID);
                if (oData != null)
                {
                    List<ItemFileData> lstfile = new List<ItemFileData>();

                    if (oData.sFilename != null)
                    {
                        ItemFileData fFile = new ItemFileData();
                        //fFile.sFileID = item.nFileID + "";
                        var sPath = STFunction.GetPathUploadFile(oData.sPath ?? "", oData.sSystemFilename ?? "");

                        fFile.sFileName = oData.sFilename;
                        fFile.sCropFileLink = sPath;
                        fFile.sFileLink = sPath;
                        fFile.sSysFileName = oData.sSystemFilename;
                        fFile.sFileType = oData.sSystemFilename != null ? oData.sSystemFilename.Split(".")[1] + "" : null;
                        fFile.sFolderName = oData.sPath;
                        fFile.IsNew = false;
                        fFile.IsNewTab = false;
                        fFile.IsCompleted = true;
                        fFile.IsDelete = false;
                        fFile.IsProgress = false;
                        fFile.sProgress = "100";
                        lstfile.Add(fFile);
                    }


                    result.nRoomID = oData.nRoomID;
                    result.sRoomName = oData.sRoomName;
                    result.nPerson = oData.nPerson;
                    result.sEquipment = oData.sEquipment;
                    result.sRoomCode = oData.sRoomCode;
                    result.sFloorName = oDataFloor.sFloorName;
                    result.nFloorID = oDataFloor.nFloorID;
                    result.fFile = lstfile.Count() > 0 ? lstfile : null;
                }
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;

            }

            return result;


        }


        // / <summary>
        // / Reject MT
        // / </summary>
        public ResultAPI Cancel(cReqDataOT param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                UserAccount? UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                TB_Meeting? oData = _db.TB_Meeting.FirstOrDefault(f => f.nMeetingID == param.sID.DecryptParameter().ToInt());

                if (oData != null)
                {
                    oData.nStatusID = param.nStatusID ?? 12;
                    // oData.sRemark = param.sComment;
                    oData.nUpdateBy = nUserID;
                    oData.dUpdate = DateTime.Now;

                    // MeetingFlow
                    int newFlowId = (_db.TB_Meeting_Flow.Any() ? _db.TB_Meeting_Flow.Max(m => m.nMeetingFlowID) : 0) + 1;
                    TB_Meeting_Flow TBMeetingFlow = new()
                    {
                        nMeetingFlowID = newFlowId,
                        nMeetingID = oData.nMeetingID,
                        nStatusID = param.nStatusID ?? 12,
                        sRemark = param.sComment,
                        nActionBy = nUserID,
                        dAction = DateTime.Now
                    };
                    _db.TB_Meeting_Flow.Add(TBMeetingFlow);
                    _db.SaveChanges();

                    //Action From Line
                    if (param.IsActionFromLine == true)
                    {
                        TB_Log_WebhookLine? objLog = _db.TB_Log_WebhookLine.Where(w => w.sGUID == param.sGUID).FirstOrDefault();
                        if (objLog != null)
                        {
                            objLog.IsActionAlready = true;
                        }
                    }

                    _db.SaveChanges();

                    #region Send Line Noti
                    string sGUID = Guid.NewGuid().ToString();

                    var arrProject = _db.TB_Project.ToArray();
                    TM_Data[]? arrStatus = _db.TM_Data.ToArray();
                    TB_Meeting_Flow? oDataFlow = _db.TB_Meeting_Flow.FirstOrDefault(f => f.nMeetingID == param.sID.DecryptParameter().ToInt() && f.nStatusID == (int)ActionId.Cancel);

                    IQueryable<objRoom>? arrRoom = from a in _db.TB_Room.Where(w => !w.IsDelete && oData.nRoomID == w.nRoomID)
                                                   from b in _db.TB_Floor.Where(w => !w.IsDelete && a.nFloorID == w.nFloorID).DefaultIfEmpty()
                                                   select new objRoom
                                                   {
                                                       value = a.nRoomID + "",
                                                       label = a.sRoomName + " ชั้น " + b.sFloorName + " ห้อง " + a.sRoomCode
                                                   };
                    var listCo = _db.TB_Employee_Report_To.Where(w => w.nPositionID == (int)ActionType.Co).Select(s => s.nEmployeeID).ToList();
                    string sApproveBy = listCo.FirstOrDefault().EncryptParameter();
                    int nMode = 4;
                    string? sProject = _db.TB_Project.Where(w => w.nProjectID == oData.nProjectID).Select(s => s.sProjectName).FirstOrDefault();

                    cParamSendLine objParam = new cParamSendLine
                    {
                        sGUID = sGUID,
                        nRequesterID = nUserID,
                        sDate = oDataFlow != null ? oDataFlow.dAction.ToStringFromDate("dd/MM/yyyy") : "",
                        sTime = oDataFlow != null ? oDataFlow.dAction.ToStringFromDate("HH:mm") + " น." : "",
                        sTitle = oData.sTitle ?? ""
                    };

                    if (oData.IsAllDay)
                    {
                        if (oData.dEnd.Date == oData.dStart.Date)
                        {
                            objParam.sStartDate = oData.dStart.ToStringFromDate("dd/MM/yyyy");
                        }
                        else
                        {
                            objParam.sStartDate = oData.dStart.ToStringFromDate("dd/MM/yyyy");
                            objParam.sEndDate = " - " + oData.dEnd.ToStringFromDate("dd/MM/yyyy");
                        }
                    }
                    else
                    {
                        objParam.sStartDate = oData.dStart.ToStringFromDate("dd/MM/yyyy");
                        objParam.sEndDate = " - " + oData.dEnd.ToStringFromDate("dd/MM/yyyy");
                    }
                    objParam.sProject = sProject ?? "";
                    objParam.sStartTime = oData.dStart.ToStringFromDate("HH:mm");
                    objParam.sEndTime = " - " + oData.dEnd.ToStringFromDate("HH:mm") + " น.";
                    objParam.sRoom = arrRoom.Select(s => s.label).FirstOrDefault() ?? "";
                    objParam.sStatus = arrStatus.Where(w => w.nData_ID == oData.nStatusID).Select(s => s.sNameTH).FirstOrDefault() ?? "";
                    objParam.sRemark = oDataFlow?.sRemark ?? "";
                    objParam.sPathDetail = "DetailMTLine&sID=" + oData.nMeetingID.EncryptParameter() + "&sUserID=" + sApproveBy + "&mode=" + nMode.EncryptParameter();

                    switch (oData.nStatusID)
                    {
                        case (int)ActionId.Cancel: //Canceled
                            objParam.nTemplateID = 35;
                            objParam.lstEmpTo = new List<int> { oData.nCreateBy };

                            break;

                    }
                    STFunction.SendToLine(objParam);
                    #endregion Send Line Noti 

                }

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }

    }
}

