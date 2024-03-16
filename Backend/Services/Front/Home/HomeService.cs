using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Extensions.Common.STFunction;
using ST.INFRA;
using ST_API.Interfaces;
using static Extensions.Systems.AllClass;
using Extensions.Common.STExtension;
using ST_API.Models.IHomeService;
using Backend.Enum;
using ST.INFRA.Common;
using static Backend.Enum.EnumWFH;
using Backend.Models.Authentication;

namespace Backend.Service.HomeService
{
    public class HomeService : IHomeService
    {
        private readonly ST_IntranetEntity db;
        //private readonly IConfiguration _config;
        private readonly IAuthentication _authen;
        private readonly IHostEnvironment _env;


        public HomeService(ST_IntranetEntity _db, IAuthentication authen, IHostEnvironment env)
        {
            // _config = config;, IConfiguration config
            _authen = authen;
            _env = env;
            db = _db;
        }

        public cResultDataHome GetDataHomePage()
        {
            cResultDataHome result = new cResultDataHome();
            try
            {
                List<cMeetingRoomHome> lstDataMeeting = new List<cMeetingRoomHome>();
                List<cMeetingRoomHome> lstMeeting = new List<cMeetingRoomHome>();
                cMeetingRoomHome objData = new cMeetingRoomHome()
                {
                    sDayTH = DateTime.Now.ToStringFromDate("dddd, dd MMMM yyyy", "th-TH"),
                    sDayEN = DateTime.Now.ToStringFromDate("dddd, dd MMMM yyyy", "en-EN"),
                };

                objData.lstMeetingDetail = new List<cMeetingDetialHome>();

                var lstData = (from a in db.TB_Meeting.Where(w => !w.IsDelete && w.IsActive && w.dStart.Date == DateTime.Now.Date && w.nStatusID == 11)
                               from b in db.TB_Room.Where(w => w.nRoomID == a.nRoomID).DefaultIfEmpty()
                               from c in db.TB_Floor.Where(w => w.nFloorID == b.nFloorID)
                               select new cMeetingDetialHome
                               {
                                   nMeetingID = a.nMeetingID,
                                   stitle = a.sTitle,
                                   sDay = DateTime.Now.Day + "",
                                   sMonth = DateTime.Now.ToStringFromDate("MMM", "en-EN"),
                                   sRoomcode = b.sRoomCode != null ? b.sRoomCode : "",
                                   sRoom = c.sFloorName != null ? c.sFloorName : "",
                                   sTimeStart = a.dStart != null ? a.dStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.HHmm, ST.INFRA.Enum.CultureName.th_TH) : "",
                                   sTimeEnd = a.dEnd != null ? a.dEnd.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.HHmm, ST.INFRA.Enum.CultureName.th_TH) : "",
                               });
                objData.lstMeetingDetail.AddRange(lstData);
                lstMeeting.Add(objData);

                var date = DateTime.Now.Date.AddDays(1);
                List<cMeetingRoomHome> lstDataMeetingNew = new List<cMeetingRoomHome>();
                List<cMeetingRoomHome> lstMeetingNew = new List<cMeetingRoomHome>();
                cMeetingRoomHome objDataNew = new cMeetingRoomHome()
                {
                    sDayTH = date.ToStringFromDate("dddd, dd MMMM yyyy", "th-TH"),
                    sDayEN = date.ToStringFromDate("dddd, dd MMMM yyyy", "en-EN"),
                };

                objDataNew.lstMeetingDetailNew = new List<cMeetingDetialHome>();

                var lstDataNew = (from a in db.TB_Meeting.Where(w => !w.IsDelete && w.IsActive && w.dStart.Date == date && w.nStatusID == 11)
                                  from b in db.TB_Room.Where(w => w.nRoomID == a.nRoomID).DefaultIfEmpty()
                                  from c in db.TB_Floor.Where(w => w.nFloorID == b.nFloorID)
                                  select new cMeetingDetialHome
                                  {
                                      nMeetingID = a.nMeetingID,
                                      stitle = a.sTitle,
                                      sDay = a.dStart.Day + "",
                                      sMonth = a.dStart.ToStringFromDate("MMM", "en-EN"),
                                      sRoomcode = b.sRoomCode != null ? b.sRoomCode : "",
                                      sRoom = c.sFloorName != null ? c.sFloorName : "",
                                      sTimeStart = a.dStart != null ? a.dStart.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.HHmm, ST.INFRA.Enum.CultureName.th_TH) : "",
                                      sTimeEnd = a.dEnd != null ? a.dEnd.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.HHmm, ST.INFRA.Enum.CultureName.th_TH) : "",
                                  });
                objDataNew.lstMeetingDetailNew.AddRange(lstDataNew);
                lstDataMeeting.Add(objDataNew);


                List<cDataHolidayHome> lstDataHoliday = new List<cDataHolidayHome>();
                List<cDataHolidayHome> lstHoliday = new List<cDataHolidayHome>();
                cDataHolidayHome objDataHoliday = new cDataHolidayHome()
                {
                    sDayTH = DateTime.Now.ToStringFromDate("dddd, dd MMMM yyyy", "th-TH"),
                    sDayEN = DateTime.Now.ToStringFromDate("dddd, dd MMMM yyyy", "en-EN"),
                };

                objDataHoliday.lstDataHoliday = new List<cHolidayHome>();

                var DataHoliday = db.TB_HolidayDay.Where(w => !w.IsActivity && !w.IsDelete && w.dDate.Date == DateTime.Now.Date).Select(s => new cHolidayHome
                {
                    nHolidayID = s.nHolidayDayID,
                    stitle = s.sHolidayName,
                    sDay = DateTime.Now.Day + "",
                    sMonth = DateTime.Now.ToStringFromDate("MMM", "en-EN"),
                    nYear = s.nYear,
                });
                objDataHoliday.lstDataHoliday.AddRange(DataHoliday);
                lstHoliday.Add(objDataHoliday);
                result.lstDataHoliday = lstHoliday.ToList();

                result.lstDataHolidayTotal = db.TB_HolidayDay.Where(w => !w.IsActivity && !w.IsDelete && w.dDate.Date > DateTime.Now.Date && w.nYear == DateTime.Now.Year).Select(s => new cHolidayHome
                {
                    nHolidayID = s.nHolidayDayID,
                    stitle = s.sHolidayName,
                    sDay = s.dDate.Day + "",
                    sMonth = s.dDate.ToStringFromDate("MMM", "en-EN"),
                    nYear = s.nYear,
                }).ToList();


                List<cDataHolidayHome> lstDataActivity = new List<cDataHolidayHome>();
                List<cDataHolidayHome> lstActivity = new List<cDataHolidayHome>();
                cDataHolidayHome objDataActivity = new cDataHolidayHome()
                {
                    sDayTH = DateTime.Now.ToStringFromDate("dddd, dd MMMM yyyy", "th-TH"),
                    sDayEN = DateTime.Now.ToStringFromDate("dddd, dd MMMM yyyy", "en-EN"),
                };

                objDataActivity.lstDataActivity = new List<cHolidayHome>();

                var DataActivity = db.TB_HolidayDay.Where(w => w.IsActivity && !w.IsDelete && w.dDate.Date == DateTime.Now.Date).Select(s => new cHolidayHome
                {
                    nHolidayID = s.nHolidayDayID,
                    stitle = s.sHolidayName,
                    sDay = DateTime.Now.Day + "",
                    sMonth = DateTime.Now.ToStringFromDate("MMM", "en-EN"),
                    nYear = s.nYear,
                });
                objDataActivity.lstDataActivity.AddRange(DataActivity);
                lstActivity.Add(objDataActivity);
                result.lstDataActivity = lstActivity.ToList();

                result.lstDataActivityTotal = db.TB_HolidayDay.Where(w => w.IsActivity && !w.IsDelete && w.dDate.Date > DateTime.Now.Date && w.nYear == DateTime.Now.Year).Select(s => new cHolidayHome
                {
                    nHolidayID = s.nHolidayDayID,
                    stitle = s.sHolidayName,
                    sDay = s.dDate.Day + "",
                    sMonth = s.dDate.ToStringFromDate("MMM", "en-EN"),
                    nYear = s.nYear,
                }).ToList();
                result.sYear = DateTime.Now.Date.ToStringFromDate("yyyy", "th-TH");

                result.lstMeeting = lstMeeting.ToList();
                result.lstMeetingNew = lstDataMeeting.ToList();
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;

            }
            return result;
        }

        public Backend.Models.ResultAPI GetInitData()
        {
            ResultInit result = new();
            var UserAccount = _authen.GetUserAccount();
            string sPosition = UserAccount.sPosition;
            result.sPosition = sPosition;
            result.HeadDate = DateTime.Now.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmm, ST.INFRA.Enum.CultureName.th_TH);
            return result;
        }

        #region GetBanner
        /// <summary>
        /// </summary>
        public ResultBanner GetBanner()
        {
            ResultBanner result = new ResultBanner();
            try
            {
                List<BannerData> lstBanner = new List<BannerData>();
                DateTime dCurrent = DateTime.Now;
                IQueryable<TB_Banner> listTable = db.TB_Banner.Where(w => !w.IsDelete && w.IsActive).AsQueryable();
                listTable = listTable.Where(w => ((w.sStartDate.HasValue && w.sStartDate.Value.Date <= dCurrent.Date) && ((w.sEndDate != null && w.sEndDate.Value.Date >= dCurrent.Date) || w.IsSetDate)));

                foreach (var item in listTable)
                {
                    string? type = !string.IsNullOrEmpty(item.sSystemFileName) ? item.sSystemFileName.Substring(item.sSystemFileName.LastIndexOf('.') + 1) : null;
                    BannerData a = new BannerData()
                    {
                        fFileBaner = new ItemFileData()
                        {
                            sFileName = item.sFileName,
                            sFileLink = STFunction.GetPathUploadFile(item.sPath, item.sSystemFileName ?? ""),
                            sSysFileName = item.sSystemFileName,
                            sFileType = type,
                            sFolderName = item.sPath,
                            IsNew = false,
                            IsNewTab = false,
                            IsCompleted = true,
                            IsDelete = false,
                            IsProgress = false,
                            sProgress = "100",
                        },
                        nOrder = item.nOrder,
                        dLastUpdate = item.dUpdate,
                        sBannerName = item.sBannerName,
                        sID = item.nBannerID + "",

                    };
                    lstBanner.Add(a);

                }
                lstBanner = lstBanner.OrderBy(w => w.nOrder).ToList();

                result.lstBanner = lstBanner;
                result.Status = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        #endregion

        public Backend.Models.ResultAPI PageLoad()
        {
            ResultOpt result = new();
            var TB_Project = db.TB_Project.Where(w => !w.IsDelete).AsQueryable();
            var TB_Employee = db.TB_Employee.Where(w => !w.IsDelete && w.IsActive).AsQueryable();
            var lstProject = TB_Project.OrderBy(w => w.sProjectName).Select(s => new Backend.Models.Option
            {
                label = (!string.IsNullOrEmpty(s.sProjectAbbr) && (s.sProjectAbbr + "").Trim() != "-" ? s.sProjectAbbr + " : " : "") + s.sProjectName,
                value = s.nProjectID.ToString()
            }).ToList();
            var lstEmployee = TB_Employee.Where(w => !w.IsDelete && w.IsActive).Select(s => new Backend.Models.Option
            {
                label = s.sNameTH + " " + s.sSurnameTH + (!string.IsNullOrEmpty(s.sNickname) ? "(" + s.sNickname + ")" : ""),
                value = s.nEmployeeID.ToString()
            }).ToList();
            var lstLeaveType = db.TB_LeaveType.Where(w => !w.IsDelete && w.IsActive).Select(s => new Backend.Models.Option
            {
                label = s.LeaveTypeName,
                value = s.nLeaveTypeID.ToString()
            }).ToList();
            result.lstProject = lstProject;
            result.lstEmployee = lstEmployee;
            result.lstLeaveType = lstLeaveType;
            return result;
        }

        public Models.ResultAPI GetTableTaskViewManager(ParamViewManager param)
        {
            Models.ResultAPI result = new();
            ResultTaskManager objResult = new();
            try
            {
                IQueryable<TB_Task> lstTask = db.TB_Task.Where(w => !w.IsDelete).AsQueryable();
                if (string.IsNullOrEmpty(param.sDateStart) && string.IsNullOrEmpty(param.sDateEnd) && string.IsNullOrEmpty(param.sProject) && string.IsNullOrEmpty(param.sEmployee))
                {
                    //defoult filter
                    lstTask = lstTask.Where(w => w.dTask.Date == DateTime.Now.Date).AsQueryable();
                }
                else
                {
                    if (string.IsNullOrEmpty(param.sDateStart) && string.IsNullOrEmpty(param.sDateEnd))
                    {
                        if (!string.IsNullOrEmpty(param.sProject))
                        {
                            lstTask = lstTask.Where(w => param.sProject == w.nProjectID + "" && w.dTask.Year == DateTime.Now.Year).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sEmployee))
                        {
                            lstTask = lstTask.Where(w => param.sEmployee == w.nEmployeeID + "" && w.dTask.Year == DateTime.Now.Year).AsQueryable();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(param.sDateStart))
                        {
                            DateTime dStartCurrent = param.sDateStart.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                            lstTask = lstTask.Where(w => w.dTask.Date >= dStartCurrent.Date).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sDateEnd))
                        {
                            DateTime dEndCurrent = param.sDateEnd.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                            lstTask = lstTask.Where(w => w.dTask.Date <= dEndCurrent.Date).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sProject))
                        {
                            lstTask = lstTask.Where(w => param.sProject == w.nProjectID + "").AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sEmployee))
                        {
                            lstTask = lstTask.Where(w => param.sEmployee == w.nEmployeeID + "").AsQueryable();
                        }

                    }

                }

                #region Task Form Database
                var lstJobType = from i in db.TM_Task_Activity_Mapping
                                 join k in db.TM_Task_Activity on i.nActivityID equals k.nActivityID
                                 join l in db.TM_Task_Activity_Type on i.nActivityTypeID equals l.nActivityTypeID
                                 select new
                                 {
                                     label = l.sActivityTypeAbbr + " - " + k.sActivity,
                                     i.nMappingActivityID
                                 };
                var qry = (from task in lstTask.ToList()
                           join prj in db.TB_Project.Where(w => !w.IsDelete) on task.nProjectID equals prj.nProjectID
                           join jobtype in lstJobType on task.nTaskTypeID equals jobtype.nMappingActivityID into job
                           from jobtype in job.DefaultIfEmpty()
                           join employee in db.TB_Employee.Where(w => !w.IsDelete) on task.nEmployeeID equals employee.nEmployeeID
                           join status in db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.TaskStatus) on task.nTaskStatusID equals status.nData_ID
                           select new Backend.Models.TaskItem
                           {
                               sEncryptID = task.nTaskID.EncryptParameter(),
                               dTask = task.dTask,
                               sTaskDate = task.dTask.ToString("dd/MM/yyyy"),
                               nProjectID = prj.nProjectID,
                               sProjectName = prj.sProjectAbbr ?? prj.sProjectName,
                               sDescription = task.sDescription,
                               nTaskTypeID = task.nTaskTypeID,
                               sJobType = jobtype != null ? jobtype.label ?? "" : "",
                               nPlan = task.nPlan,
                               nActual = task.nActual,
                               nPlanProcess = task.nPlanProcess,
                               nActualProcess = task.nActualProcess,
                               nTaskStatusID = task.nTaskStatusID,
                               sStatus = status.sNameTH ?? "",
                               nProgressID = task.nProgressID,
                               sDescriptionDelay = task.sDescriptionDelay ?? "",
                               IsDelete = false,
                               IsModified = false,
                               IsRequireDelay = false,
                               IsLock = false,
                               sEmployeeName = employee.sNickname ?? employee.sNameTH + " " + employee.sSurnameTH
                           }).ToList();
                #endregion

                qry.ForEach(f =>
                {
                    var objProgress = db.TM_Task_Progress.FirstOrDefault(w => w.nProgressID == f.nProgressID && !w.IsDelete);
                    if (objProgress != null)
                    {
                        f.sProgress = objProgress.sProgressName;
                    }
                });

                objResult.arrFullData = qry;
                #region//SORT
                string sSortColumn = param.sSortExpression;
                switch (param.sSortExpression)
                {
                    case "sTaskDate": sSortColumn = "dTask"; break;
                    case "sProjectName": sSortColumn = "sProjectName"; break;
                    case "sDescription": sSortColumn = "sDescription"; break;
                    case "sJobType": sSortColumn = "sJobType"; break;
                    case "sStatus": sSortColumn = "sStatus"; break;
                    case "sDescriptionDelay": sSortColumn = "sDescriptionDelay"; break;
                }
                if (param.isASC)
                {
                    qry = qry.OrderBy<Backend.Models.TaskItem>(sSortColumn).ToList();
                }
                else if (param.isDESC)
                {
                    qry = qry.OrderByDescending<Backend.Models.TaskItem>(sSortColumn).ToList();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qry.Count());
                objResult.arrData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                int nKey = 1;
                objResult.arrData.ForEach(f =>
                {
                    f.sID = nKey + "";
                    nKey++;
                });
                objResult.nDataLength = dataPage.nDataLength;
                objResult.nPageIndex = dataPage.nPageIndex;
                objResult.nSkip = dataPage.nSkip;
                objResult.nTake = dataPage.nTake;
                objResult.nStartIndex = dataPage.nStartIndex;
                #endregion

                result.objResult = objResult;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;

            }
            return result;
        }

        public Models.ResultAPI GetTableLeaveViewManager(ParamViewManager param)
        {
            Models.ResultAPI result = new();
            ResultLeaveManager objResult = new();
            try
            {
                var UserAccount = _authen.GetUserAccount();
                int nEmployeeID = UserAccount.nUserID;
                string sPosition = UserAccount.sPosition;
                int nPositionID = 0;
                if (!string.IsNullOrEmpty(sPosition))
                {
                    var Position = db.TB_Position.FirstOrDefault(f => f.sPositionName == sPosition);
                    if (Position != null)
                    {
                        nPositionID = Position.nPositionID;
                    }
                }

                List<int> arrEmployeeWaitApprove = new List<int>();

                if (nPositionID == 9)
                {
                    arrEmployeeWaitApprove = db.TB_Employee.Where(w => !w.IsDelete && w.IsActive).Select(s => s.nEmployeeID).ToList();
                }
                else
                {
                    arrEmployeeWaitApprove = db.TB_Employee_Report_To.Where(w => !w.IsDelete && w.nRepEmployeeID == nEmployeeID).Select(s => s.nEmployeeID).ToList();
                }

                var lstLeave = db.TB_Request_Leave.Where(w => !w.IsDelete).AsQueryable();
                if (string.IsNullOrEmpty(param.sDateStart) && string.IsNullOrEmpty(param.sDateEnd) && string.IsNullOrEmpty(param.sProject) && string.IsNullOrEmpty(param.sEmployee))
                {
                    //defoult filter

                    //SELECT* FROM TB_Request_Leave
                    //WHERE(CONVERT(DATE, dStartDateTime) >= '2024-03-12' AND CONVERT(DATE, dEndDateTime) <= '2024-03-12')
                    //OR(CONVERT(DATE, dEndDateTime) >= '2024-03-12' AND CONVERT(DATE, dStartDateTime) <= '2024-03-12')
                    lstLeave = lstLeave.Where(w => (w.dStartDateTime >= DateTime.Now && w.dEndDateTime <= DateTime.Now
                                                    || w.dEndDateTime >= DateTime.Now && w.dStartDateTime <= DateTime.Now)
                                                    || arrEmployeeWaitApprove.Contains(w.nEmployeeID) && w.nStatusID != 1 && w.nStatusID == 3
                    ).AsQueryable();

                }
                else
                {
                    if (string.IsNullOrEmpty(param.sDateStart) && string.IsNullOrEmpty(param.sDateEnd))
                    {
                        if (!string.IsNullOrEmpty(param.sTypeLeave))
                        {
                            lstLeave = lstLeave.Where(w => param.sTypeLeave == w.nLeaveTypeID + ""
                                                        && w.dStartDateTime.Year == DateTime.Now.Year
                                                        && w.dEndDateTime.Year == DateTime.Now.Year
                            ).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sEmployee))
                        {
                            lstLeave = lstLeave.Where(w => param.sEmployee == w.nCreateBy + ""
                                                        && w.dStartDateTime.Year == DateTime.Now.Year
                                                        && w.dEndDateTime.Year == DateTime.Now.Year
                            ).AsQueryable();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(param.sDateStart))
                        {
                            DateTime dStartCurrent = param.sDateStart.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                            lstLeave = lstLeave.Where(w => w.dStartDateTime <= dStartCurrent).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sDateEnd))
                        {
                            DateTime dEndCurrent = param.sDateEnd.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                            lstLeave = lstLeave.Where(w => w.dEndDateTime >= dEndCurrent).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sTypeLeave))
                        {
                            lstLeave = lstLeave.Where(w => param.sTypeLeave == w.nLeaveTypeID + "").AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sEmployee))
                        {
                            lstLeave = lstLeave.Where(w => param.sEmployee == w.nCreateBy + "").AsQueryable();
                        }
                    }
                }


                var TM_Status = db.TM_Status.Where(w => w.nRequestTypeID == 1).AsQueryable();
                var TB_LeaveType = db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete).AsQueryable();
                var TB_Employee = db.TB_Employee.Where(w => w.IsActive && !w.IsDelete).AsQueryable();
                var TB_Employee_Report_To = db.TB_Employee_Report_To.Where(w => !w.IsDelete && w.nRepEmployeeID == nEmployeeID).AsQueryable();

                var qry = (from r in lstLeave
                           join e in TB_Employee on r.nEmployeeID equals e.nEmployeeID
                           join s in TM_Status on r.nStatusID equals s.nStatusID
                           join t in TB_LeaveType on r.nLeaveTypeID equals t.nLeaveTypeID
                           join re in TB_Employee_Report_To on e.nEmployeeID equals re.nEmployeeID into gj
                           from re in gj.DefaultIfEmpty()
                           select new LeaveItem
                           {
                               sEncryptID = r.nRequestLeaveID.EncryptParameter(),
                               sID = r.nRequestLeaveID.ToString(),
                               sCreateDate = r.dCreate.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmm, ST.INFRA.Enum.CultureName.th_TH),
                               sEmployeeName = e.sNickname ?? e.sNameTH + " " + e.sSurnameTH,
                               sTypeLeave = t.LeaveTypeName ?? "",
                               nStatus = r.nStatusID,
                               sStatus = s.sNextStatusName,
                               sDetail = r.sReason,
                               sStartLeave = r.dStartDateTime.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmm, ST.INFRA.Enum.CultureName.th_TH),
                               sEndLeave = r.dEndDateTime.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmm, ST.INFRA.Enum.CultureName.th_TH),
                               sLeaveUse = r.nLeaveUse >= 1 ? r.nLeaveUse.ToString() + " วัน" : (r.nLeaveUse * 10).ToString() + " ชั่วโมง",
                               isEnableApprove = (nPositionID == 9 && r.nStatusID == (int)EnumLeave.Status.Submit) // Hr
                                        || (r.nStatusID == (int)EnumLeave.Status.Approved && re != null && re.nRepEmployeeID == nEmployeeID),
                           }).ToList();

                objResult.arrFullData = qry;

                #region//SORT
                string sSortColumn = param.sSortExpression;
                switch (param.sSortExpression)
                {
                    case "action_approve": sSortColumn = "isEnableApprove"; break;
                    case "sCreateDate": sSortColumn = "sCreateDate"; break;
                    case "sEmployeeName": sSortColumn = "sEmployeeName"; break;
                    case "sStatus": sSortColumn = "sStatus"; break;
                    case "sDetail": sSortColumn = "sDetail"; break;
                }
                if (param.isASC)
                {
                    qry = qry.OrderBy<LeaveItem>(sSortColumn).ToList();
                }
                else if (param.isDESC)
                {
                    qry = qry.OrderByDescending<LeaveItem>(sSortColumn).ToList();
                }
                #endregion
                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qry.Count());
                objResult.arrData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                int nKey = 1;
                objResult.arrData.ForEach(f =>
                {
                    f.sID = nKey + "";
                    nKey++;
                });
                objResult.nDataLength = dataPage.nDataLength;
                objResult.nPageIndex = dataPage.nPageIndex;
                objResult.nSkip = dataPage.nSkip;
                objResult.nTake = dataPage.nTake;
                objResult.nStartIndex = dataPage.nStartIndex;
                #endregion

                result.objResult = objResult;

            }
            catch (System.Exception e)
            {
                result.sMessage = e.Message;
            }
            return result;
        }

        public Models.ResultAPI GetTableOTViewManager(ParamViewManager param)
        {
            Models.ResultAPI result = new();
            ResultOTManager objResult = new();
            try
            {
                var UserAccount = _authen.GetUserAccount();
                int nEmployeeID = UserAccount.nUserID;
                string sPosition = UserAccount.sPosition;
                IQueryable<TB_Request_OT> lstOT = db.TB_Request_OT.Where(w => !w.IsDelete).AsQueryable();
                if (string.IsNullOrEmpty(param.sDateStart) && string.IsNullOrEmpty(param.sDateEnd) && string.IsNullOrEmpty(param.sProject) && string.IsNullOrEmpty(param.sEmployee))
                {
                    //defoult filter
                    lstOT = lstOT.Where(w => w.dPlanDateTime == DateTime.Now ||
                                            (w.nApproveBy == nEmployeeID && w.nRequestTypeID == 2 && (w.nStatusID == 5 || w.nStatusID == 2))).AsQueryable();
                }
                else
                {
                    if (string.IsNullOrEmpty(param.sDateStart) && string.IsNullOrEmpty(param.sDateEnd))
                    {
                        if (!string.IsNullOrEmpty(param.sProject))
                        {
                            lstOT = lstOT.Where(w => param.sProject == w.nProjectID + "" && w.dPlanDateTime.Year == DateTime.Now.Year).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sEmployee))
                        {
                            lstOT = lstOT.Where(w => param.sEmployee == w.nCreateBy + "" && w.dPlanDateTime.Year == DateTime.Now.Year).AsQueryable();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(param.sDateStart))
                        {
                            DateTime dStartCurrent = param.sDateStart.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                            lstOT = lstOT.Where(w => w.dPlanDateTime.Date >= dStartCurrent.Date).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sDateEnd))
                        {
                            DateTime dEndCurrent = param.sDateEnd.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                            lstOT = lstOT.Where(w => w.dPlanDateTime.Date <= dEndCurrent.Date).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sProject))
                        {
                            lstOT = lstOT.Where(w => param.sProject == w.nProjectID + "").AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sEmployee))
                        {
                            lstOT = lstOT.Where(w => param.sEmployee == w.nCreateBy + "").AsQueryable();
                        }
                    }
                }

                var qry = (from a in lstOT
                           join b in db.TB_Project on a.nProjectID equals b.nProjectID
                           join c in db.TB_Employee on a.nApproveBy equals c.nEmployeeID
                           join d in db.TM_Status.Where(w => w.nRequestTypeID == 2) on a.nStatusID equals d.nStatusID
                           join e in db.TB_Employee on a.nCreateBy equals e.nEmployeeID
                           select new OTItem
                           {
                               sEncryptID = a.nRequestOTID.EncryptParameter(),
                               sID = a.nRequestOTID.ToString(),
                               sTypeOT = a.IsHoliday ? "Holiday" : "Normal",
                               sEmployeeName = e.sNickname ?? e.sNameTH + " " + e.sSurnameTH,
                               sOTDate = a.dPlanDateTime.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                               sDetail = a.sTopic,
                               sStatus = d.sStatusName,
                               sApproveBy = $"{c.sNameTH} {c.sSurnameTH}",
                               nPlan = a.nEstimateHour,
                           }).ToList();
                var lstAllTaskResult = db.TB_Request_OT_Task.Where(w => !w.IsDelete).ToList();
                foreach (var item in qry.ToList())
                {
                    var lstHour = lstAllTaskResult.Where(w => w.nRequestOTID + "" == item.sID && w.nOTHour != null).ToList();
                    item.nActual = lstHour != null && lstHour.Any() ? lstHour.Select(s => s.nOTHour).Sum() : 0;
                }

                objResult.arrFullData = qry;

                #region//SORT
                string sSortColumn = param.sSortExpression;
                switch (param.sSortExpression)
                {
                    case "sTypeOT": sSortColumn = "sTypeOT"; break;
                    case "sEmployeeName": sSortColumn = "sEmployeeName"; break;
                    case "sOTDate": sSortColumn = "sOTDate"; break;
                    case "sStatus": sSortColumn = "sStatus"; break;
                    case "sDetail": sSortColumn = "sDetail"; break;
                }
                if (param.isASC)
                {
                    qry = qry.OrderBy<OTItem>(sSortColumn).ToList();
                }
                else if (param.isDESC)
                {
                    qry = qry.OrderByDescending<OTItem>(sSortColumn).ToList();
                }
                #endregion
                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qry.Count());
                objResult.arrData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                int nKey = 1;
                objResult.arrData.ForEach(f =>
                {
                    f.sID = nKey + "";
                    nKey++;
                });
                objResult.nDataLength = dataPage.nDataLength;
                objResult.nPageIndex = dataPage.nPageIndex;
                objResult.nSkip = dataPage.nSkip;
                objResult.nTake = dataPage.nTake;
                objResult.nStartIndex = dataPage.nStartIndex;
                #endregion

                result.objResult = objResult;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;

            }
            return result;
        }

        public Models.ResultAPI GetTableWFHViewManager(ParamViewManager param)
        {
            Models.ResultAPI result = new();
            ResultWFHManager objResult = new();
            var UserAccount = _authen.GetUserAccount();
            int nEmployeeID = UserAccount.nUserID;
            string sPosition = UserAccount.sPosition;
            try
            {
                IQueryable<TB_WFH> lstWFH = db.TB_WFH.Where(w => !w.IsDelete).AsQueryable();
                if (string.IsNullOrEmpty(param.sDateStart) && string.IsNullOrEmpty(param.sDateEnd) && string.IsNullOrEmpty(param.sTypeWFH) && string.IsNullOrEmpty(param.sEmployee))
                {
                    //defoult filter
                    lstWFH = lstWFH.Where(w => w.dWFH.Date == DateTime.Now.Date
                                            || w.nApproveBy == nEmployeeID && w.nFlowProcessID == 3
                    ).AsQueryable();

                }
                else
                {
                    if (string.IsNullOrEmpty(param.sDateStart) && string.IsNullOrEmpty(param.sDateEnd))
                    {
                        if (!string.IsNullOrEmpty(param.sTypeWFH))
                        {
                            lstWFH = lstWFH.Where(w => (param.sTypeWFH == "1" ? w.IsEmergency == true : w.IsEmergency == false) && w.dWFH.Month == DateTime.Now.Month).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sEmployee))
                        {
                            lstWFH = lstWFH.Where(w => param.sEmployee == w.nCreateBy + "" && w.dWFH.Month == DateTime.Now.Month).AsQueryable();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(param.sDateStart))
                        {
                            DateTime dStartCurrent = param.sDateStart.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                            lstWFH = lstWFH.Where(w => w.dWFH.Date >= dStartCurrent.Date).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sDateEnd))
                        {
                            DateTime dEndCurrent = param.sDateEnd.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                            lstWFH = lstWFH.Where(w => w.dWFH.Date <= dEndCurrent.Date).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sTypeWFH))
                        {
                            lstWFH = lstWFH.Where(w => param.sTypeWFH == "1" ? w.IsEmergency == true : w.IsEmergency == false).AsQueryable();
                        }
                        if (!string.IsNullOrEmpty(param.sEmployee))
                        {
                            lstWFH = lstWFH.Where(w => param.sEmployee == w.nCreateBy + "").AsQueryable();
                        }
                    }
                }
                List<int>? listChecDraft = db.TB_WFH.Where(w => w.nCreateBy == UserAccount.nUserID && w.nFlowProcessID == (int)WFHStatus.Draft).Select(s => s.nWFHID).ToList();
                var listGroupHS = db.TB_WFHFlowHistory.Where(w => w.nFlowProcessID != 0 && !w.IsDelete).GroupBy(entry => entry.nWFHID)
                                    .Select(group => new
                                    {
                                        Group = group.Key,
                                        LastDate = group.Max(entry => entry.dApprove)
                                    });
                var TBWFHFlowHistory = db.TB_WFHFlowHistory.Where(w => listGroupHS.Select(s => s.Group).Contains(w.nWFHID) && listGroupHS.Select(s => s.LastDate).Contains(w.dApprove));

                var qry = (from wfh in lstWFH
                           join his in TBWFHFlowHistory on wfh.nWFHID equals his.nWFHID
                           join employee in db.TB_Employee.Where(w => !w.IsDelete) on wfh.nCreateBy equals employee.nEmployeeID
                           join approveby in db.TB_Employee.Where(w => !w.IsDelete) on wfh.nApproveBy equals approveby.nEmployeeID
                           select new WFHItem
                           {
                               sEncryptID = wfh.nWFHID.EncryptParameter(),
                               sID = wfh.nWFHID.ToString(),
                               sType = wfh.IsEmergency == true ? "ฉุกเฉิน" : "ปกติ",
                               sEmployeeName = employee.sNickname ?? employee.sNameTH + " " + employee.sSurnameTH,
                               sRequestDate = wfh.dCreate.ToString("dd/MM/yyyy"),
                               sWFHDate = wfh.dWFH.ToString("dd/MM/yyyy"),
                               sStatus = db.TM_WFHFlowProcess.Where(w => w.nFlowProcessID == wfh.nFlowProcessID).Select(w => w.sProcess).FirstOrDefault() ?? "",
                               sApproveBy = approveby.sNameEN + " " + approveby.sNameEN,
                               nFlowProcessID = wfh.nFlowProcessID,
                               sDescription = his.sDescription ?? ""
                           }).ToList();

                objResult.arrFullData = qry;

                #region//SORT
                string sSortColumn = param.sSortExpression;
                switch (param.sSortExpression)
                {
                    case "sWFHDate": sSortColumn = "sWFHDate"; break;
                    case "sRequestDate": sSortColumn = "sRequestDate"; break;
                    case "sApproveBy": sSortColumn = "sApproveBy"; break;
                    case "sStatus": sSortColumn = "sStatus"; break;
                    case "sDescription": sSortColumn = "sDescription"; break;
                }
                if (param.isASC)
                {
                    qry = qry.OrderBy<WFHItem>(sSortColumn).ToList();
                }
                else if (param.isDESC)
                {
                    qry = qry.OrderByDescending<WFHItem>(sSortColumn).ToList();
                }
                #endregion
                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qry.Count());
                objResult.arrData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                int nKey = 1;
                objResult.arrData.ForEach(f =>
                {
                    f.sID = nKey + "";
                    nKey++;
                });
                objResult.nDataLength = dataPage.nDataLength;
                objResult.nPageIndex = dataPage.nPageIndex;
                objResult.nSkip = dataPage.nSkip;
                objResult.nTake = dataPage.nTake;
                objResult.nStartIndex = dataPage.nStartIndex;
                #endregion

                result.objResult = objResult;
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