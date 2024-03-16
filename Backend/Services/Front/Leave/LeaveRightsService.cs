using ST_API.Interfaces;
using ST_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Extensions.Common.STFunction;
using ClosedXML.Excel;
using ST.INFRA.Common;
using ST.INFRA;
using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Backend.Models;
using ResultAPI = Extensions.Common.STResultAPI.ResultAPI;
using CsvHelper;
using Extensions.Systems;
using Backend.Models.Authentication;

namespace ST_API.Services.ISystemService
{
    public class LeaveRightsService : ILeaveRightsService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _authen;
        private readonly IHostEnvironment _env;
        private readonly ParamJWT _user;
        private int[] arrStatusRequestorAction = { (int)Backend.Enum.EnumLeave.Status.RejectLead, (int)Backend.Enum.EnumLeave.Status.RejectHr,
                         (int)Backend.Enum.EnumLeave.Status.Recall,(int)Backend.Enum.EnumLeave.Status.Draft };
        public LeaveRightsService(ST_IntranetEntity db, IAuthentication authen, IHostEnvironment env)
        {
            _db = db;
            _authen = authen;
            _env = env;
            _user = _authen.GetUserAccount();
        }

        public async Task<cLeaveInitData> SumLeaveCard()
        {
            cLeaveInitData result = new cLeaveInitData();
            try
            {
                DateTime dNow = DateTime.Now;

                #region Card leave summary
                TB_Request_Leave[] lstLeave = _db.TB_Request_Leave.Where(w => (w.nStatusID != 0 && w.nStatusID != 1) && (w.dStartDateTime.Day == dNow.Day && w.dStartDateTime.Month == dNow.Month && w.dStartDateTime.Year == dNow.Year) && (w.dEndDateTime.Day == dNow.Day && w.dEndDateTime.Month == dNow.Month && w.dEndDateTime.Year == dNow.Year) && !w.IsDelete).AsNoTracking().ToArray();
                TB_Employee_TimeStemp[] lstLate = _db.TB_Employee_TimeStemp.Where(w => w.dTimeDate == dNow && w.IsDelay).AsNoTracking().ToArray();
                TB_WFH[] lstWFH = _db.TB_WFH.Where(w => (w.dWFH.Day == dNow.Day && w.dWFH.Month == dNow.Month && w.dWFH.Year == dNow.Year) && !w.IsDelete).AsNoTracking().ToArray();
                TB_Request_Leave[] lstLeaveWithCert = _db.TB_Request_Leave.Where(w => w.nStatusID == 99 && (w.dStartDateTime == dNow && w.dEndDateTime >= dNow) && !w.IsDelete).AsNoTracking().ToArray();
                result.nLeave = lstLeave.Count();
                result.nLate = lstLate.Count();
                result.nWFH = lstWFH.Count();
                result.nLeaveWithCert = lstLeaveWithCert.Count();
                #endregion

                #region leave type card
                var arrLeaveTotal = await (from lt in _db.TB_LeaveType
                                           join lf in _db.TB_Image_Leave on lt.nLeaveTypeID equals lf.nLeaveTypeID
                                           where !lt.IsDelete && lt.IsActive && !lf.IsDelete
                                           orderby lt.nOrder
                                           select new LeaveRightsTotalRequest
                                           {
                                               sID = lt.nLeaveTypeID.EncryptParameter(),
                                               nID = lt.nLeaveTypeID,
                                               sLeaveTypeName = lt.LeaveTypeName,
                                               sFileLink = STFunction.GetPathUploadFile(lf.sImageParh ?? null, lf.sSystemName ?? null)
                                           }).ToArrayAsync();

                foreach (var item in arrLeaveTotal)
                {
                    item.nLeaveCount = lstLeave.Where(w => w.nLeaveTypeID == item.nID).Count();
                }

                result.arrLeaveTotal = arrLeaveTotal;
                #endregion

                #region Query
                var qryLeave = (from Tleave in _db.TB_Request_Leave
                                join TLeaveType in _db.TB_LeaveType on Tleave.nLeaveTypeID equals TLeaveType.nLeaveTypeID
                                where Tleave.nStatusID != 0 && Tleave.nStatusID != 1 && !Tleave.IsDelete && TLeaveType.IsActive && !TLeaveType.IsDelete
                                select new cCalendar
                                {
                                    sID = Tleave.nLeaveTypeID + "",
                                    groupId = "1",
                                    dEventStart = Tleave.dStartDateTime,
                                    dEventEnd = Tleave.dEndDateTime,
                                    title = "ลาหยุดงาน",//TLeaveType.LeaveTypeName,
                                    backgroundColor = "rgba(211, 47, 47, 0.3)",
                                    textColor = "#080808",
                                    IsMeetingRoom = false,
                                    allDay = false,
                                    nStatus = 0
                                }
                ).AsNoTracking().ToArray();

                var qryLate = (from Tlate in _db.TB_Employee_TimeStemp.Where(w => w.IsDelay)
                               join Temployee in _db.TB_Employee.Where(w => w.IsActive && !w.IsRetire) on Tlate.nEmployeeID equals Temployee.nEmployeeID
                               join Tempposition in _db.TB_Employee_Position.Where(w => !w.IsDelete) on Temployee.nEmployeeID equals Tempposition.nEmployeeID
                               join Tpostion in _db.TB_Position.Where(w => !w.IsDelete) on Tempposition.nPositionID equals Tpostion.nPositionID
                               //    join TLeaveType in _db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete) on Tleave.nLeaveTypeID equals TLeaveType.nLeaveTypeID
                               select new cCalendar
                               {
                                   sID = Tlate.nTimeStampID + "",
                                   groupId = "2",
                                   dEventStart = Tlate.dTimeStartDate,
                                   dEventEnd = Tlate.dTimeEndDate.HasValue ? Tlate.dTimeEndDate.Value : Tlate.dTimeStartDate,
                                   title = "มาสาย",//TLeaveType.LeaveTypeName,
                                   backgroundColor = "rgba(46, 125, 50, 0.3)",
                                   textColor = "#080808",
                                   allDay = false,
                                   IsMeetingRoom = false,
                                   nStatus = 0
                               }
                ).AsNoTracking().ToArray();

                var qryWFH = (from Twfh in _db.TB_WFH
                              join Temployee in _db.TB_Employee on Twfh.nCreateBy equals Temployee.nEmployeeID
                              join Tempposition in _db.TB_Employee_Position on Temployee.nEmployeeID equals Tempposition.nEmployeeID
                              join Tpostion in _db.TB_Position on Tempposition.nPositionID equals Tpostion.nPositionID
                              where !Twfh.IsDelete && Temployee.IsActive && !Temployee.IsRetire && !Temployee.IsDelete && !Tempposition.IsDelete && !Tpostion.IsDelete
                              select new cCalendar
                              {
                                  sID = Twfh.nWFHID + "",
                                  groupId = "3",
                                  dEventStart = Twfh.dWFH,// DateTime.Now,
                                  dEventEnd = Twfh.dWFH,
                                  title = "Work From Home",//TLeaveType.LeaveTypeName,
                                  backgroundColor = "rgba(237, 108, 2, 0.3)",
                                  textColor = "#080808",
                                  IsMeetingRoom = false,
                                  allDay = false,
                                  nStatus = 0
                              }
                ).AsNoTracking().ToArray();


                var qryLeaveWithCert = (from Tleave in _db.TB_Request_Leave.Where(w => !w.IsDelete)
                                        join TLeaveType in _db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete) on Tleave.nLeaveTypeID equals TLeaveType.nLeaveTypeID
                                        select new cCalendar
                                        {
                                            sID = Tleave.nLeaveTypeID + "",
                                            groupId = "4",
                                            dEventStart = Tleave.dStartDateTime,
                                            dEventEnd = Tleave.dEndDateTime,
                                            title = "รับรองเวลา",//TLeaveType.LeaveTypeName,
                                            backgroundColor = "rgba(32, 96, 220, 0.3)",
                                            textColor = "#080808",
                                            allDay = false,
                                            IsMeetingRoom = false,
                                            nStatus = 0
                                        }
               ).AsNoTracking().ToArray();

                var qry = new List<cCalendar>();
                qry.AddRange(qryLeave);
                qry.AddRange(qryLate);
                qry.AddRange(qryWFH);
                qry.AddRange(qryLeaveWithCert);
                #endregion

                result.lstData = qry;
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.sMessage = e.Message;
                result.nStatusCode = StatusCodes.Status500InternalServerError;
            }
            return result;
        }

        public cResultTableLeave GetDataTableLeave(cLeaveTable req)
        {
            cResultTableLeave result = new cResultTableLeave();
            try
            {
                DateTime dNow = DateTime.Now;
                // TB_Request_Leave[] lstLeave = _db.TB_Request_Leave.Where(w => (w.nStatusID != 0 && w.nStatusID != 1) 
                // && (w.dStartDateTime.Day == dNow.Day && w.dStartDateTime.Month == dNow.Month && w.dStartDateTime.Year == dNow.Year) 
                // && (w.dEndDateTime.Day == dNow.Day && w.dEndDateTime.Month == dNow.Month && w.dEndDateTime.Year == dNow.Year) && !w.IsDelete).AsNoTracking().ToArray();

                var qryAllLeave = (from Tleavetype in _db.TB_LeaveType
                                   join Tleave in _db.TB_Request_Leave on Tleavetype.nLeaveTypeID equals Tleave.nLeaveTypeID
                                   join Temployee in _db.TB_Employee on Tleave.nEmployeeID equals Temployee.nEmployeeID
                                   join Tempposition in _db.TB_Employee_Position on Temployee.nEmployeeID equals Tempposition.nEmployeeID
                                   join Tpostion in _db.TB_Position on Tempposition.nPositionID equals Tpostion.nPositionID
                                   where Tleavetype.IsActive && !Tleavetype.IsDelete
                                   && Tleave.nStatusID != 0 && Tleave.nStatusID != 1 && (Tleave.dStartDateTime.Day == dNow.Day && Tleave.dStartDateTime.Month == dNow.Month && Tleave.dStartDateTime.Year == dNow.Year)
                                   && (Tleave.dEndDateTime.Day == dNow.Day && Tleave.dEndDateTime.Month == dNow.Month && Tleave.dEndDateTime.Year == dNow.Year) && !Tleave.IsDelete
                                   && Temployee.IsActive && !Temployee.IsRetire && !Tempposition.IsDelete && !Tpostion.IsDelete
                                   select new cLeaveHomeTable
                                   {
                                       sID = Tleave.nRequestLeaveID.EncryptParameter(),
                                       nID = Tleavetype.nLeaveTypeID,
                                       sNameTH = Temployee.sNameTH + " " + Temployee.sSurnameTH,
                                       nLeaveType = Tleavetype.nLeaveTypeID,
                                       sPosition = Tpostion == null ? "-" : Tpostion.sPositionName,
                                       sLeaveDate = Tleave.dStartDateTime.ToString("dd/MM/yyy") + " - " + Tleave.dEndDateTime.ToString("dd/MM/yyy"),
                                       sLeaveTypeName = Tleavetype.LeaveTypeName
                                   }
                        ).ToList();

                int nCount = 0;
                int nLeaveID = 0;
                string sName = null;

                var lstCount = qryAllLeave;
                List<cLeaveHome> lstLeaveData = new List<cLeaveHome>();
                lstCount.ForEach(f =>
                {
                    nCount = _db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete && w.nLeaveTypeID == f.nLeaveType && w.LeaveTypeName == f.sLeaveTypeName).Count();
                    var aa = _db.TB_LeaveType.FirstOrDefault(w => w.IsActive && !w.IsDelete && w.nLeaveTypeID == f.nLeaveType);
                    if (aa != null)
                    {
                        nLeaveID = aa.nLeaveTypeID;
                        sName = aa.LeaveTypeName;
                    }

                    cLeaveHome objHome = new cLeaveHome()
                    {
                        nCount = nCount,
                        sName = sName,
                        nLeaveID = nLeaveID
                    };
                    lstLeaveData.Add(objHome);
                });

                result.arrHome = lstLeaveData.ToArray();
                qryAllLeave = qryAllLeave.OrderBy(o => o.nID).ToList();

                #region//SORT
                string sSortColumn = req.sSortExpression;
                switch (req.sSortExpression)
                {
                    case "sNameTH": sSortColumn = "sNameTH"; break;
                    case "nLeaveType": sSortColumn = "nLeaveType"; break;
                        // case "sDescription": sSortColumn = "sDescription"; break;
                        // case "sJobType": sSortColumn = "sJobType"; break;
                        // case "sHourPlan": sSortColumn = "sHourPlan"; break;
                        // case "sTargetPlan": sSortColumn = "sTargetPlan"; break;
                        // case "sHourActual": sSortColumn = "sHourActual"; break;
                        // case "sTargetActual": sSortColumn = "sTargetActual"; break;
                        // case "sStatus": sSortColumn = "sStatus"; break;
                        // case "sDescriptionDelay": sSortColumn = "sDescriptionDelay"; break;
                }
                if (req.isASC)
                {
                    qryAllLeave = qryAllLeave.OrderBy<cLeaveHomeTable>(sSortColumn).ToList();
                }
                else if (req.isDESC)
                {
                    qryAllLeave = qryAllLeave.OrderByDescending<cLeaveHomeTable>(sSortColumn).ToList();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(req.nPageSize, req.nPageIndex, qryAllLeave.Count());
                // result.lstDataHome = qryAllLeave.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                result.lstDataHome = qryAllLeave;
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
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

        public cResultTableLeave GetDataTableLate(cLeaveTable req)
        {
            cResultTableLeave result = new cResultTableLeave();
            try
            {
                DateTime dNow = DateTime.Now;

                #region Query
                TB_Employee_TimeStemp[] lstLate = _db.TB_Employee_TimeStemp.Where(w => w.dTimeDate == dNow && w.IsDelay).AsNoTracking().ToArray();
                var aa = _db.TB_Employee.Where(w => w.IsActive && !w.IsRetire);
                var bb = _db.TB_Employee_Position.Where(w => !w.IsDelete);
                var cc = _db.TB_Position.Where(w => !w.IsDelete);

                var qry = (from Tlate in _db.TB_Employee_TimeStemp.Where(w => w.dTimeDate == dNow && w.IsDelay)
                           join Temployee in _db.TB_Employee.Where(w => w.IsActive && !w.IsRetire) on Tlate.nEmployeeID equals Temployee.nEmployeeID
                           join Tempposition in _db.TB_Employee_Position.Where(w => !w.IsDelete) on Temployee.nEmployeeID equals Tempposition.nEmployeeID
                           join Tpostion in _db.TB_Position.Where(w => !w.IsDelete) on Tempposition.nPositionID equals Tpostion.nPositionID
                           select new cLeaveDetail
                           {
                               sID = Tlate.nTimeStampID.EncryptParameter(),
                               //    sNameTH = Temployee.sNameTH,
                               sNameTH = Temployee.sNameTH + " " + Temployee.sSurnameTH,
                               sNameEN = Temployee.sNameEN,
                               sSurNameTH = Temployee.sSurnameTH,
                               sSurNameEN = Temployee.sSurnameEN,
                               sPosition = Tpostion == null ? "-" : Tpostion.sPositionName,
                               //    dEventStart = Tlate.dStartDateTime,
                               //    dEventEnd = Tlate.dEndDateTime
                           }
                        ).OrderBy(o => o.sNameTH).ToList();
                #endregion

                #region//SORT
                string sSortColumn = req.sSortExpression;
                switch (req.sSortExpression)
                {
                    case "sNameTH": sSortColumn = "sNameTH"; break;
                        // case "nLeaveType": sSortColumn = "nLeaveType"; break;
                        // case "sDescription": sSortColumn = "sDescription"; break;
                        // case "sJobType": sSortColumn = "sJobType"; break;
                        // case "sHourPlan": sSortColumn = "sHourPlan"; break;
                        // case "sTargetPlan": sSortColumn = "sTargetPlan"; break;
                        // case "sHourActual": sSortColumn = "sHourActual"; break;
                        // case "sTargetActual": sSortColumn = "sTargetActual"; break;
                        // case "sStatus": sSortColumn = "sStatus"; break;
                        // case "sDescriptionDelay": sSortColumn = "sDescriptionDelay"; break;
                }
                if (req.isASC)
                {
                    qry = qry.OrderBy<cLeaveDetail>(sSortColumn).ToList();
                }
                else if (req.isDESC)
                {
                    qry = qry.OrderByDescending<cLeaveDetail>(sSortColumn).ToList();
                }
                #endregion

                // result.lstData = qry;

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(req.nPageSize, req.nPageIndex, qry.Count());
                // result.lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                result.lstData = qry;
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
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

        public cResultTableLeave GetDataTableWFH(cLeaveTable req)
        {
            cResultTableLeave result = new cResultTableLeave();
            try
            {
                DateTime dNow = DateTime.Now;

                #region Query
                var qry = (from TWFH in _db.TB_WFH
                           join Temployee in _db.TB_Employee on TWFH.nCreateBy equals Temployee.nEmployeeID
                           join Tempposition in _db.TB_Employee_Position on Temployee.nEmployeeID equals Tempposition.nEmployeeID
                           join Tpostion in _db.TB_Position on Tempposition.nPositionID equals Tpostion.nPositionID
                           where !TWFH.IsDelete && (TWFH.dWFH.Day == dNow.Day && TWFH.dWFH.Month == dNow.Month && TWFH.dWFH.Year == dNow.Year)
                           && Temployee.IsActive && !Temployee.IsRetire
                           && !Tempposition.IsDelete && !Tpostion.IsDelete
                           select new cLeaveDetail
                           {
                               sID = TWFH.nWFHID.EncryptParameter(),
                               //    sNameTH = Temployee.sNameTH,
                               sNameTH = Temployee.sNameTH + " " + Temployee.sSurnameTH,
                               sNameEN = Temployee.sNameEN,
                               sSurNameTH = Temployee.sSurnameTH,
                               sSurNameEN = Temployee.sSurnameEN,
                               sPosition = Tpostion == null ? "-" : Tpostion.sPositionName,
                               //    dEventStart = TWFH.dStartDateTime,
                               //    dEventEnd = TWFH.dEndDateTime
                           }
                ).ToList();
                #endregion
                qry = qry.OrderBy(o => o.sNameTH).ToList();
                // qry = qry.GroupBy(g => g.sNameTH).ToList();


                #region//SORT
                string sSortColumn = req.sSortExpression;
                switch (req.sSortExpression)
                {
                    case "sNameTH": sSortColumn = "sNameTH"; break;
                        // case "nLeaveType": sSortColumn = "nLeaveType"; break;
                        // case "sDescription": sSortColumn = "sDescription"; break;
                        // case "sJobType": sSortColumn = "sJobType"; break;
                        // case "sHourPlan": sSortColumn = "sHourPlan"; break;
                        // case "sTargetPlan": sSortColumn = "sTargetPlan"; break;
                        // case "sHourActual": sSortColumn = "sHourActual"; break;
                        // case "sTargetActual": sSortColumn = "sTargetActual"; break;
                        // case "sStatus": sSortColumn = "sStatus"; break;
                        // case "sDescriptionDelay": sSortColumn = "sDescriptionDelay"; break;
                }
                if (req.isASC)
                {
                    qry = qry.OrderBy<cLeaveDetail>(sSortColumn).ToList();
                }
                else if (req.isDESC)
                {
                    qry = qry.OrderByDescending<cLeaveDetail>(sSortColumn).ToList();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(req.nPageSize, req.nPageIndex, qry.Count());
                // result.lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                result.lstData = qry;
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                #endregion

                // result.lstData = qry;
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        public cResultTableLeave GetDataTableWorkCert(cLeaveTable req)
        {
            cResultTableLeave result = new cResultTableLeave();
            try
            {
                DateTime dNow = DateTime.Now;

                #region Query
                var qry = (from TCert in _db.TB_Request_Leave.Where(w => w.nStatusID == 99 && (w.dStartDateTime == dNow && w.dEndDateTime >= dNow) && !w.IsDelete).ToList()
                           join Temployee in _db.TB_Employee.Where(w => w.IsActive && !w.IsRetire) on TCert.nEmployeeID equals Temployee.nEmployeeID
                           join Tempposition in _db.TB_Employee_Position.Where(w => !w.IsDelete) on Temployee.nEmployeeID equals Tempposition.nEmployeeID
                           join Tpostion in _db.TB_Position.Where(w => !w.IsDelete) on Tempposition.nPositionID equals Tpostion.nPositionID
                           select new cLeaveDetail
                           {
                               sID = TCert.nRequestLeaveID.EncryptParameter(),
                               //    sNameTH = Temployee.sNameTH,
                               sNameTH = Temployee.sNameTH + " " + Temployee.sSurnameTH,
                               sNameEN = Temployee.sNameEN,
                               sSurNameTH = Temployee.sSurnameTH,
                               sSurNameEN = Temployee.sSurnameEN,
                               sPosition = Tpostion.sPositionName,
                               dEventStart = TCert.dStartDateTime,
                               dEventEnd = TCert.dEndDateTime,
                           }
                        ).OrderBy(o => o.sNameTH).ToList();
                #endregion

                #region//SORT
                string sSortColumn = req.sSortExpression;
                switch (req.sSortExpression)
                {
                    case "sNameTH": sSortColumn = "sNameTH"; break;
                        // case "nLeaveType": sSortColumn = "nLeaveType"; break;
                        // case "sDescription": sSortColumn = "sDescription"; break;
                        // case "sJobType": sSortColumn = "sJobType"; break;
                        // case "sHourPlan": sSortColumn = "sHourPlan"; break;
                        // case "sTargetPlan": sSortColumn = "sTargetPlan"; break;
                        // case "sHourActual": sSortColumn = "sHourActual"; break;
                        // case "sTargetActual": sSortColumn = "sTargetActual"; break;
                        // case "sStatus": sSortColumn = "sStatus"; break;
                        // case "sDescriptionDelay": sSortColumn = "sDescriptionDelay"; break;
                }
                if (req.isASC)
                {
                    qry = qry.OrderBy<cLeaveDetail>(sSortColumn).ToList();
                }
                else if (req.isDESC)
                {
                    qry = qry.OrderByDescending<cLeaveDetail>(sSortColumn).ToList();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(req.nPageSize, req.nPageIndex, qry.Count());
                // result.lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                result.lstData = qry;
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                #endregion

                // result.lstData = qry;
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        public async Task<cFormInit> GetFilterLeave()
        {
            cFormInit result = new cFormInit()
            {
                nStatusCode = StatusCodes.Status200OK,
            };

            try
            {
                _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                int nUserID = _authen.GetUserAccount().nUserID;
                int nEmployeeID = nUserID;
                TB_Request_Leave? objFormData = new TB_Request_Leave();

                if (_authen.GetUserAccount().lstUserPositionID.Contains(9))
                {
                    result.arrOptionEmployee = await _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive).Select(s => new cSelectOption
                    {
                        label = (s.sNameTH ?? "") + " " + (s.sSurnameTH ?? ""),
                        value = s.nEmployeeID.EncryptParameter()
                    }).ToArrayAsync();
                }
                // if (_authen.GetUserAccount().lstUserPositionID == 9)
                // {
                //     result.arrOptionEmployee = await _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive).Select(s => new cSelectOption
                //     {
                //         label = (s.sNameTH ?? "") + " " + (s.sSurnameTH ?? ""),
                //         value = s.nEmployeeID.EncryptParameter()
                //     }).ToArrayAsync();
                // }
                else
                {
                    result.arrOptionEmployee = await (from re in _db.TB_Employee_Report_To
                                                      join e in _db.TB_Employee on re.nEmployeeID equals e.nEmployeeID
                                                      where !re.IsDelete && re.nRepEmployeeID == nEmployeeID && !e.IsDelete && e.IsActive
                                                      select new cSelectOption
                                                      {
                                                          label = (e.sNameTH ?? "") + " " + (e.sSurnameTH ?? ""),
                                                          value = e.nEmployeeID.EncryptParameter()
                                                      }).ToArrayAsync();
                }
                result.arrOption = await
                (from lt in _db.TB_LeaveType
                 where lt.IsActive && !lt.IsDelete
                 orderby lt.nOrder
                 select new cSelectOption
                 {
                     label = lt.LeaveTypeName,
                     value = lt.nLeaveTypeID.EncryptParameter()
                 }
                 ).ToArrayAsync();

                result.arrOptionStatus = await _db.TM_Status.Where(w => w.nRequestTypeID == 1
                ).OrderBy(o => o.nOrder).Select(s => new cSelectOption
                {
                    label = s.sNextStatusName,
                    value = s.nStatusID.EncryptParameter()
                }).ToArrayAsync();
            }
            catch (System.Exception e)
            {
                result.sMessage = e.Message;
                result.nStatusCode = StatusCodes.Status500InternalServerError;
            }
            return result;
        }

        // 1 = บวก , 2 = ลบ
        private decimal _calDaysLeave(decimal dStart, decimal dEnd, int nMode)
        {
            decimal result = 0;
            int nStartAfterPoint = Convert.ToDouble(dStart).ToString("0.0").Split(".").Last().ToInt();
            int nEndAfterPoint = Convert.ToDouble(dEnd).ToString("0.0").Split(".").Last().ToInt();
            int nStartBeforePoint = Convert.ToInt32(dStart.ToString("0.0").Split(".").First());
            int nEndBeforePoint = Convert.ToInt32(dEnd.ToString("0.0").Split(".").First());

            int nStartToHour = (nStartBeforePoint * 8) + nStartAfterPoint;
            int nEndToHour = (nEndBeforePoint * 8) + nEndAfterPoint;
            int nResultHour = 0;
            if (nMode == 1)
            {
                nResultHour = nStartToHour + nEndToHour;
            }
            else
            {
                nResultHour = nStartToHour - nEndToHour;
            }
            int nModResult = nResultHour % 8;
            if (nModResult > 0)
            {
                double dHourToDay = nResultHour / 8;
                result = Convert.ToDecimal($"{Math.Floor(dHourToDay)}.{nModResult}");
            }
            else
            {
                result = nResultHour / 8;
            }
            return result;
        }

        public void LeaveFlow(TB_Request_Leave objLeaveRequest, int nModeAction)
        {
            switch (objLeaveRequest.nStatusID)
            {
                case (int)Backend.Enum.EnumLeave.Status.Submit:
                    {
                        #region  action
                        switch (nModeAction)
                        {
                            case (int)Backend.Enum.EnumLeave.SaveMode.Approve:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Approved;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Cancel:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Canceled;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Reject:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.RejectHr;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Recall:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Recall;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        #endregion
                        break;
                    }
                case (int)Backend.Enum.EnumLeave.Status.RecallHr:
                    {
                        #region  action
                        switch (nModeAction)
                        {
                            case (int)Backend.Enum.EnumLeave.SaveMode.Cancel:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Canceled;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Approve:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Approved;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Reject:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.RejectHr;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        #endregion
                        break;
                    }
                case (int)Backend.Enum.EnumLeave.Status.Approved:
                    {
                        #region  action
                        switch (nModeAction)
                        {
                            case (int)Backend.Enum.EnumLeave.SaveMode.Approve:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Complete;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Cancel:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Canceled;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Reject:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.RejectLead;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Recall:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.RecallHr;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        #endregion
                        break;
                    }
                case (int)Backend.Enum.EnumLeave.Status.RecallLead:
                    {
                        #region  action
                        switch (nModeAction)
                        {
                            case (int)Backend.Enum.EnumLeave.SaveMode.Approve:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Complete;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Cancel:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Canceled;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Reject:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.RejectLead;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        #endregion
                        break;
                    }
                case (int)Backend.Enum.EnumLeave.Status.RejectHr:
                    {
                        #region  action
                        switch (nModeAction)
                        {
                            case (int)Backend.Enum.EnumLeave.SaveMode.Cancel:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Canceled;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Recall:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.RecallHr;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Submit:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Submit;
                                    break;
                                }
                            default:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Draft;
                                    break;
                                }
                        }
                        #endregion
                        break;
                    }
                case (int)Backend.Enum.EnumLeave.Status.RejectLead:
                    {
                        #region  action
                        switch (nModeAction)
                        {
                            case (int)Backend.Enum.EnumLeave.SaveMode.Cancel:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Canceled;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Recall:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.RecallLead;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Submit:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Submit;
                                    break;
                                }
                            default:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Draft;
                                    break;
                                }
                        }
                        #endregion
                        break;
                    }
                case (int)Backend.Enum.EnumLeave.Status.Complete:
                    {
                        #region  action
                        switch (nModeAction)
                        {
                            case (int)Backend.Enum.EnumLeave.SaveMode.Cancel:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Canceled;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Recall:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.RecallLead;
                                    break;
                                }
                            case (int)Backend.Enum.EnumLeave.SaveMode.Submit:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Submit;
                                    break;
                                }
                            default:
                                {
                                    objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Draft;
                                    break;
                                }
                        }
                        #endregion
                        break;
                    }
                default:
                    {
                        if (arrStatusRequestorAction.Contains(objLeaveRequest.nStatusID))
                        {
                            #region  action
                            switch (nModeAction)
                            {
                                case (int)Backend.Enum.EnumLeave.SaveMode.Submit:
                                    {
                                        objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Submit;
                                        break;
                                    }
                                case (int)Backend.Enum.EnumLeave.SaveMode.Cancel:
                                    {
                                        objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Canceled;
                                        break;
                                    }
                                default:
                                    {
                                        objLeaveRequest.nStatusID = (int)Backend.Enum.EnumLeave.Status.Draft;
                                        break;
                                    }
                            }
                            #endregion
                        }
                        break;
                    }
            }
        }

        public async Task<ResultAPI> ApproveOnTable(cFormReq req)
        {
            ResultAPI result = new ResultAPI() { Status = StatusCodes.Status200OK };
            try
            {
                int nEmployeeID = _authen.GetUserAccount().nUserID;
                int? nLeaveReqID = req.sID.DecryptParameter().ToIntOrNull();
                TB_Request_Leave objReqLeave = await _db.TB_Request_Leave.FirstOrDefaultAsync(f => f.nRequestLeaveID == nLeaveReqID) ?? new TB_Request_Leave();
                if (objReqLeave == null)
                {
                    result.Message = "Data not found.";
                    return result;
                }
                objReqLeave.dUpdate = DateTime.Now;
                objReqLeave.nUpdateBy = nEmployeeID;
                LeaveFlow(objReqLeave, req.nMode);
                int nReqLogID = await _db.TB_Request_Leave_History.AnyAsync() ? await _db.TB_Request_Leave_History.MaxAsync(m => m.nRequestLeaveHisID) + 1 : 1;
                TB_Request_Leave_History objLog = new TB_Request_Leave_History()
                {
                    dCreate = DateTime.Now,
                    dUpdate = DateTime.Now,
                    dEndDateTime = objReqLeave.dEndDateTime,
                    dStartDateTime = objReqLeave.dStartDateTime,
                    IsEmergency = objReqLeave.IsEmergency,
                    nUpdateBy = nEmployeeID,
                    nCreateBy = nEmployeeID,
                    nEmployeeID = nEmployeeID,
                    nLeaveTypeID = objReqLeave.nLeaveTypeID,
                    nRequestLeaveID = objReqLeave.nRequestLeaveID,
                    nStatusID = objReqLeave.nStatusID,
                    sReason = objReqLeave.sReason,
                    nRequestLeaveHisID = nReqLogID,
                    sComment = req.sComment
                };
                await _db.TB_Request_Leave_History.AddAsync(objLog);
                await _db.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                result.Message = e.Message;
                result.Status = StatusCodes.Status500InternalServerError;
            }
            return result;
        }

        public cResultTableLeave GetDataTableHoliday(cLeaveTable req)
        {
            cResultTableLeave result = new cResultTableLeave();
            try
            {
                DateTime dNow = DateTime.Now;

                #region holiday 
                var lstActivityHoliday = _db.TB_HolidayDay.Where(w => w.nYear == dNow.Year && w.IsActivity && !w.IsDelete).Select(s => new cLeaveDetail
                {
                    sID = s.nHolidayDayID.EncryptParameter(),
                    sNameTH = s.sHolidayName,
                    dEventStart = s.dDate,
                    sActivity = "กิจกรรมบริษัท"
                }).ToList();

                var lstHoliday = _db.TB_HolidayDay.Where(w => w.nYear == dNow.Year && !w.IsActivity && !w.IsDelete).Select(s => new cLeaveDetail
                {
                    sID = s.nHolidayDayID.EncryptParameter(),
                    sNameTH = s.sHolidayName,
                    dEventStart = s.dDate,
                    sActivity = "วันหยุดประจำปี"
                }).ToList();
                #endregion

                #region//SORT
                string sSortColumn = req.sSortExpression;
                switch (req.sSortExpression)
                {
                    case "sNameTH": sSortColumn = "sNameTH"; break;
                }

                if (req.isASC)
                {
                    lstHoliday = lstHoliday.OrderBy<cLeaveDetail>(sSortColumn).ToList();
                    lstActivityHoliday = lstActivityHoliday.OrderBy<cLeaveDetail>(sSortColumn).ToList();
                }
                else if (req.isDESC)
                {
                    lstHoliday = lstHoliday.OrderByDescending<cLeaveDetail>(sSortColumn).ToList();
                    lstActivityHoliday = lstActivityHoliday.OrderByDescending<cLeaveDetail>(sSortColumn).ToList();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPageHoliday = STGrid.Paging(req.nPageSize, req.nPageIndex, lstHoliday.Count());
                // result.lstData = lstHoliday.Skip(dataPageHoliday.nSkip).Take(dataPageHoliday.nTake).ToList();
                result.lstHoliday = lstHoliday;
                result.nDataLength = dataPageHoliday.nDataLength;
                result.nPageIndex = dataPageHoliday.nPageIndex;
                result.nSkip = dataPageHoliday.nSkip;
                result.nTake = dataPageHoliday.nTake;
                result.nStartIndex = dataPageHoliday.nStartIndex;
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPageActivity = STGrid.Paging(req.nPageSize, req.nPageIndex, lstActivityHoliday.Count());
                // result.lstData = lstHoliday.Skip(dataPageActivity.nSkip).Take(dataPageActivity.nTake).ToList();
                result.lstActivity = lstActivityHoliday;
                result.nDataLength = dataPageActivity.nDataLength;
                result.nPageIndex = dataPageActivity.nPageIndex;
                result.nSkip = dataPageActivity.nSkip;
                result.nTake = dataPageActivity.nTake;
                result.nStartIndex = dataPageActivity.nStartIndex;
                #endregion

                // result.lstData = lstHoliday.ToList();
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        public cResultTableLeave GetLeaveSummary(cLeaveRights req)
        {
            cResultTableLeave result = new cResultTableLeave();
            try
            {
                DateTime dNow = DateTime.Now;
                List<string> lstYear = new List<string>();
                lstYear.Add(dNow.Year.ToString());

                List<string> checkYear = req.Year != null ? req.Year : lstYear;

                var lstLeaveGroup = _db.TB_LeaveSummary.Where(w => !w.IsDelete).GroupBy(g => new { g.nEmployeeID }).Select(s => new cLeaveDetail
                {
                    nEmployeeID = s.Key.nEmployeeID
                }).Select(s => s.nEmployeeID);

                var lstType = _db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete).ToList();
                var lstLeave = _db.TB_LeaveSummary.Where(w => !w.IsDelete).ToList();

                if (checkYear != null && checkYear.Any())
                {
                    lstLeave = lstLeave.Where(w => checkYear.Contains((w.nYear).ToString())).ToList();
                }

                List<TB_Employee> lstEmp = _db.TB_Employee.Where(w => lstLeaveGroup.Contains(w.nEmployeeID) && w.IsActive && !w.IsRetire).ToList();
                List<cLeaveSummary> lstLeaveSum = new List<cLeaveSummary>();
                List<cLeaveSummary> lstID = new List<cLeaveSummary>();

                lstEmp.ForEach(f =>
                {
                    int nYear = (dNow.Year - (f.dWorkStart ?? DateTime.Now).Year);
                    int nMonth = (((dNow.Year - (f.dWorkStart ?? DateTime.Now).Year) * 12) + dNow.Month - (f.dWorkStart ?? DateTime.Now).Month) % 12;

                    //loop employee
                    cLeaveSummary objData = new cLeaveSummary()
                    {
                        sID = f.nEmployeeID.ToString(),
                        nEmployeeID = f.nEmployeeID,
                        sFullNameTH = $"{f.sNameTH} {f.sSurnameTH ?? ""} ({f.sNickname})",//f.sNameTH + " " + f.sSurnameTH + "(" + f.sNickname + ")",
                        sNameTH = $"{f.sNameTH} {f.sSurnameTH ?? ""}",
                        sName = f.sNameTH,
                        sSurname = f.sSurnameTH,
                        dEventStart = f.dWorkStart ?? dNow,
                        nEmpType = f.nEmployeeTypeID,
                        sEmpType = f.nEmployeeTypeID == 21 ? "พนักงานประจำ" : f.nEmployeeTypeID == 22 ? "พนักงานทดลองาน" : f.nEmployeeTypeID == 23 ? "พนักงานสัญญาจ้าง" : "-",
                        sEmpYear = nYear + " ปี " + nMonth + " เดือน"
                    };

                    lstID.Add(objData);

                    objData.lstLeaveDetail = new List<cLeaveDetail>();
                    //loop leave type
                    foreach (var type in lstType)
                    {
                        cLeaveDetail objLeaveCount = new cLeaveDetail();
                        {
                            var lstLeaveDay = lstLeave.Where(w => w.nEmployeeID == f.nEmployeeID).ToList();
                            foreach (var item in lstLeaveDay)
                            {
                                if (type.nLeaveTypeID == item.nLeaveTypeID)
                                {
                                    objLeaveCount.nLeaveSummaryID = item.nLeaveSummaryID;
                                    objLeaveCount.nLeaveTypeID = type.nLeaveTypeID;
                                    objLeaveCount.nEmployeeID = item.nEmployeeID;
                                    objLeaveCount.sID = f.nEmployeeID.ToString();
                                    objLeaveCount.sPer = " / ";
                                    objLeaveCount.sLeaveUse = item.nLeaveUse.ToString();
                                    objLeaveCount.sUse = item.nQuantity.ToString();
                                }
                            }

                            objData.lstLeaveDetail.Add(objLeaveCount);
                        }
                    }

                    result.lstLeaveType = lstType.Select(s => new cSelectOption
                    {
                        label = s.LeaveTypeName,
                        value = s.nLeaveTypeID.ToString()
                    }).ToList();
                    lstLeaveSum.Add(objData);
                });
                var newQry = lstLeaveSum.OrderBy(o => o.nEmployeeID).ToList();

                #region filter
                if (req.lstEmpName != null && req.lstEmpName.Any())
                {
                    newQry = newQry.Where(w => req.lstEmpName.Contains(w.sID)).ToList();
                }

                if (req.sEmpTypeID != null && req.sEmpTypeID.Any())
                {
                    newQry = newQry.Where(w => req.sEmpTypeID.Contains(w.nEmpType.ToString())).ToList();
                }
                #endregion

                result.sID = lstID;

                #region//SORT
                string sSortColumn = req.sSortExpression;
                switch (req.sSortExpression)
                {
                    case "nEmployeeID": sSortColumn = "nEmployeeID"; break;
                        // case "sNote": sSortColumn = "sNote"; break;
                        // case "nOrder": sSortColumn = "nOrder"; break;
                        // case "dUpdate": sSortColumn = "dUpdate"; break;
                }
                if (req.isASC)
                {
                    newQry = newQry.OrderBy<cLeaveSummary>(sSortColumn).ToList();
                }
                else if (req.isDESC)
                {
                    newQry = newQry.OrderByDescending<cLeaveSummary>(sSortColumn).ToList();
                }
                #endregion

                result.query = newQry.ToList();

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(req.nPageSize, req.nPageIndex, newQry.Count());
                result.lstSummaryLeave = newQry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                #endregion

                #region check sync
                var countEmp = lstLeave.GroupBy(g => g.nEmployeeID).Count();
                var countLeaveType = lstLeave.GroupBy(g => g.nLeaveTypeID).Count();
                var allEmp = _db.TB_Employee.Where(w => w.dPromote != null || w.dWorkStart != null && w.IsActive && !w.IsRetire).Count();
                var allLeaveType = lstType.Count();
                #endregion

                if (countLeaveType != allLeaveType)
                {
                    result.Message = "กรุณาซิงค์ข้อมูล";
                    return result;
                }
                else if (countEmp != allEmp)
                {
                    result.Message = "กรุณาซิงค์ข้อมูล";
                    return result;
                }
                else
                {
                    result.Status = StatusCodes.Status200OK;
                }

            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        public async Task<List<ApproverData>> GetLineApproveLeave(int nEmployeeID)
        {
            List<ApproverData> result = new List<ApproverData>();

            ApproverData? objHr = await (from emp in _db.TB_Employee
                                         join emp_po in _db.TB_Employee_Position on emp.nEmployeeID equals emp_po.nEmployeeID
                                         join po in _db.TB_Position on emp_po.nPositionID equals po.nPositionID
                                         where emp_po.nPositionID == 9 //HR
                                         && !emp.IsDelete && !emp_po.IsDelete && emp.IsActive && !po.IsDelete && po.IsActive
                                         select new ApproverData
                                         {
                                             sName = (emp.sNameTH ?? "") + " " + (emp.sSurnameTH ?? ""),
                                             sPosition = po.sPositionName,
                                             sStatus = "",
                                             sEmployeeID = emp.nEmployeeID.EncryptParameter(),
                                             isHr = true,
                                         }).AsNoTracking().FirstOrDefaultAsync();
            if (objHr != null)
            {
                result.Add(objHr);
            }

            int nUpperPositionID = 0;
            var objEmpReportTo = _db.TB_Employee_Report_To.FirstOrDefault(f => !f.IsDelete && f.nEmployeeID == nEmployeeID);
            if (objEmpReportTo != null)
            {
                nUpperPositionID = objEmpReportTo.nRepEmployeeID ?? 0;
            }

            ApproverData? objApprover = await (from emp in _db.TB_Employee
                                               join emp_po in _db.TB_Employee_Position on emp.nEmployeeID equals emp_po.nEmployeeID
                                               join po in _db.TB_Position on emp_po.nPositionID equals po.nPositionID
                                               join po_s in _db.TB_Position_Secondary on emp_po.nLevelPosition equals po_s.nPositionSecondaryID
                                               where emp.nEmployeeID == nUpperPositionID
                                               && !emp.IsDelete && !emp_po.IsDelete && emp.IsActive && !po.IsDelete && po.IsActive
                                               select new ApproverData
                                               {
                                                   sName = (emp.sNameTH ?? "") + " " + (emp.sSurnameTH ?? ""),
                                                   sPosition = po_s.sPositionSecondaryName + " (" + po.sPositionName + ")",
                                                   sStatus = "",
                                                   sEmployeeID = emp.nEmployeeID.EncryptParameter()
                                               }).AsNoTracking().FirstOrDefaultAsync();

            if (objApprover != null)
            {
                result.Add(objApprover);
            }
            int nNo = 1;
            result.ForEach(f =>
            {
                f.nNo = nNo;
                f.sID = nNo.ToString();
                nNo += 1;
            });
            return result;
        }

        public async Task<cTabelWorkList> GetDataWorkList(cReqTableWorkList req)
        {
            cTabelWorkList result = new cTabelWorkList() { Status = StatusCodes.Status200OK };
            try
            {
                _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                int nEmployeeID = _authen.GetUserAccount().nUserID;

                List<int> arrEmployeeWaitApprove = new List<int>();

                if (req.isApproveListPage)
                {
                    if (_authen.GetUserAccount().lstUserPositionID.Contains(9))
                    {
                        arrEmployeeWaitApprove = await _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive).Select(s => s.nEmployeeID).ToListAsync();
                    }
                    else
                    {
                        arrEmployeeWaitApprove = await _db.TB_Employee_Report_To.Where(w => !w.IsDelete && w.nRepEmployeeID == nEmployeeID).Select(s => s.nEmployeeID).ToListAsync();
                    }
                }

                var qry = (from r in _db.TB_Request_Leave
                           join e in _db.TB_Employee on r.nEmployeeID equals e.nEmployeeID
                           join s in _db.TM_Status on r.nStatusID equals s.nStatusID
                           join t in _db.TB_LeaveType on r.nLeaveTypeID equals t.nLeaveTypeID
                           join re in _db.TB_Employee_Report_To on e.nEmployeeID equals re.nEmployeeID
                           into re
                           from g_re in re.Where(w => !w.IsDelete && w.nRepEmployeeID == nEmployeeID).DefaultIfEmpty()
                           where !r.IsDelete
                           && (req.isApproveListPage ? arrEmployeeWaitApprove.Contains(r.nEmployeeID) : r.nEmployeeID == nEmployeeID)
                           && s.nRequestTypeID == 1 // Leave Type 1
                           && !t.IsDelete && t.IsActive && e.IsActive && !e.IsDelete
                           select new { TB_Request_Leave = r, TM_Status = s, TB_LeaveType = t, TB_Employee = e, TB_Employee_Report_To = re }
                          );


                if (req.arrEmpID.Length > 0 && req.isApproveListPage)
                {
                    int[] arrEmpID = req.arrEmpID.Select(s => s.DecryptParameter().ToInt()).ToArray();
                    qry = qry.Where(w => arrEmpID.Contains(w.TB_Employee.nEmployeeID));
                }

                if (req.arrStatusID.Length > 0)
                {
                    int[] arrStatusID = req.arrStatusID.Select(s => s.DecryptParameter().ToInt()).ToArray();
                    qry = qry.Where(w => arrStatusID.Contains(w.TB_Request_Leave.nStatusID));
                }

                if (req.arrTypeID.Length > 0)
                {
                    int[] arrTypeID = req.arrTypeID.Select(s => s.DecryptParameter().ToInt()).ToArray();
                    qry = qry.Where(w => arrTypeID.Contains(w.TB_Request_Leave.nLeaveTypeID));
                }

                if (req.dStartFilterDate.HasValue)
                {
                    DateTime dStartDate = (req.dStartFilterDate ?? 0).ToDateTime();
                    if (req.nTypeFilterDate == 1)
                    { //วันที่ทำรายการ
                        qry = qry.Where(w => w.TB_Request_Leave.dCreate >= dStartDate);
                    }
                    else
                    { //วันที่ลา
                        qry = qry.Where(w => w.TB_Request_Leave.dStartDateTime >= dStartDate);
                    }
                }

                if (req.dEndFilterDate.HasValue)
                {
                    DateTime dEndDate = (req.dEndFilterDate ?? 0).ToDateTime();
                    if (req.nTypeFilterDate == 1)
                    { //วันที่ทำรายการ
                        qry = qry.Where(w => w.TB_Request_Leave.dCreate <= dEndDate);
                    }
                    else
                    { //วันที่ลา
                        qry = qry.Where(w => w.TB_Request_Leave.dEndDateTime <= dEndDate);
                    }
                }


                var qrySelector = qry.Select(s => new TableDataWorkList
                {
                    sID = s.TB_Request_Leave.nRequestLeaveID.EncryptParameter(),
                    sCreateDate = s.TB_Request_Leave.dCreate.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH),
                    dCreateDate = s.TB_Request_Leave.dCreate,
                    sFullName = (s.TB_Employee.sNameTH ?? "") + " " + (s.TB_Employee.sSurnameTH ?? ""),
                    sType = s.TB_LeaveType.LeaveTypeName,
                    nStatus = s.TB_Request_Leave.nStatusID,
                    sStatus = s.TM_Status.sNextStatusName ?? "",
                    sStartLeave = s.TB_Request_Leave.dStartDateTime.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmm, ST.INFRA.Enum.CultureName.th_TH),
                    dStartLeave = s.TB_Request_Leave.dStartDateTime,
                    sEndLeave = s.TB_Request_Leave.dEndDateTime.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmm, ST.INFRA.Enum.CultureName.th_TH),
                    dEndLeave = s.TB_Request_Leave.dEndDateTime,
                    sLeaveUse = s.TB_Request_Leave.nLeaveUse.ToString() + " วัน",
                    isEnableEdit = !req.isApproveListPage && arrStatusRequestorAction.Contains(s.TB_Request_Leave.nStatusID),
                    isEnableApprove = req.isApproveListPage && (
                    (_authen.GetUserAccount().lstUserPositionID.Contains(9) && s.TB_Request_Leave.nStatusID == (int)Backend.Enum.EnumLeave.Status.Submit) // Hr
                    ||
                    (s.TB_Request_Leave.nStatusID == (int)Backend.Enum.EnumLeave.Status.Approved &&
                    s.TB_Employee_Report_To != null && s.TB_Employee_Report_To.First().nRepEmployeeID == nEmployeeID))

                });

                #region//SORT
                string sSortColumn = req.sSortExpression;
                switch (req.sSortExpression)
                {
                    case "sCreateDate": sSortColumn = "dCreateDate"; break;
                    case "sFullName": sSortColumn = "sFullName"; break;
                    case "sType": sSortColumn = "sType"; break;
                    case "nStatusID": sSortColumn = "nStatusID"; break;
                    case "sStartLeave": sSortColumn = "dStartLeave"; break;
                    case "sEndLeave": sSortColumn = "dEndLeave"; break;
                    case "sLeaveUse": sSortColumn = "sLeaveUse"; break;
                }
                if (req.isASC)
                {
                    qrySelector = qrySelector.OrderBy<TableDataWorkList>(sSortColumn);
                }
                else if (req.isDESC)
                {
                    qrySelector = qrySelector.OrderByDescending<TableDataWorkList>(sSortColumn);
                }
                #endregion


                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(req.nPageSize, req.nPageIndex, qrySelector.Count());
                result.arrData = await qrySelector.Skip(dataPage.nSkip).Take(dataPage.nTake).ToArrayAsync();
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                #endregion


            }
            catch (System.Exception e)
            {
                result.Message = e.Message;
            }
            return result;
        }

        public cResultTableLeave GetMasterData()
        {
            cResultTableLeave result = new cResultTableLeave();
            try
            {
                DateTime dNow = DateTime.Now;
                cSelect[] lstEmployeeType = _db.TM_Data.Where(w => w.nDatatypeID == 7 && !w.IsDelete).Select(s => new cSelect
                {
                    value = s.nData_ID.ToString(),
                    label = s.sNameTH
                }).AsNoTracking().ToArray();

                cSelect[] lstYear = _db.TB_LeaveSummary.Where(w => !w.IsDelete).GroupBy(g => new { g.nYear }).Select(s => new cSelect
                {
                    value = s.Key.nYear.ToString(),
                    label = (s.Key.nYear + 543).ToString()
                }).AsNoTracking().ToArray();

                cSelect[] lstEmployee = _db.TB_Employee.Where(w => w.IsActive && !w.IsRetire).Select(s => new cSelect
                {
                    value = s.nEmployeeID.ToString(),
                    label = $"{s.sNameTH} {s.sSurnameTH ?? ""} ({s.sNickname})"//s.sNameTH + " " + s.sSurnameTH + "(" + s.sNickname + ")"

                }).AsNoTracking().ToArray();

                result.lstSelectEmpType = lstEmployeeType;
                result.lstYear = lstYear;
                result.lstEmployeeName = lstEmployee;
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        // public cReturnImportExcel LeaveImportExcel(cLeaveImportExcel req)
        // {
        //     cReturnImportExcel result = new cReturnImportExcel();
        //     try
        //     {
        //         List<cObjectImportExcel>? lstData = new List<cObjectImportExcel>();

        //         // if (req.sFileName != null && req.sFileName.Substring(0, 4) != "TOR_")
        //         if (req.sFileName == null && req.sFileName.Substring(0, 4) != "TOR_")
        //         {
        //             result.nStatusCode = StatusCodes.Status200OK;
        //             result.sMessage = "ไฟล์แนบไม่ตรงกับข้อมูลในระบบ";
        //         }
        //         else
        //         {
        //             string path = "Imprt/Leave";
        //             if (Directory.Exists(STFunction.MapPath(path, _env)))
        //             {
        //                 Directory.CreateDirectory(STFunction.MapPath(path, _env));
        //             }

        //             if (File.Exists(STFunction.MapPath(req.sPath + "/" + req.sSysFileName, _env)) && !string.IsNullOrEmpty(req.sSysFileName))
        //             {
        //                 STFunction.MoveFile(req.sPath + "/" + "", path + "/" + "", req.sSysFileName + "", _env);
        //             }
        //             string sMapPathFileCurrent = STFunction.MapPath(path + "/" + req.sSysFileName, _env);
        //             if (File.Exists(sMapPathFileCurrent))
        //             {
        //                 using (var stream = new FileStream(sMapPathFileCurrent, FileMode.Open, FileAccess.Read))
        //                 {
        //                     IExcelDataReader xlsRd = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //                     DataSet ds = xlsRd.AsDataSet();
        //                     var sheet = "data";
        //                     DataTable? dt = ds.Tables[sheet];
        //                     string[] arrColumnHeader = new string[] { "Code", "Detail" };
        //                     List<string> lstInvalidColumn = new List<string>();
        //                     if (dt != null)
        //                     {
        //                         int index = 1;
        //                         for (int i = 0; i < dt.Rows.Count; i++)
        //                         {
        //                             DataRow dr = dt.Rows[i];
        //                             if (i == 0) //Check Header Column
        //                             {
        //                                 for (int j = 0; j < arrColumnHeader.Length; j++)
        //                                 {
        //                                     var a = dr[j] + "";
        //                                     if ((dr[j] + "").Trim() != arrColumnHeader[j].Trim())
        //                                     {
        //                                         lstInvalidColumn.Add(dr[j] + "");
        //                                     }
        //                                 }

        //                                 if (lstInvalidColumn.Any())
        //                                 {
        //                                     string sInvalid = string.Join(", ", lstInvalidColumn);
        //                                     result.nStatusCode = StatusCodes.Status200OK;
        //                                     result.sMessage = "ชื่อคอลัมธ์ไม่ตรงกับเทมเพลต";
        //                                     // result.Message = "ชื่อคอลัมธ์ไม่ตรงกับเทมเพลต " + sInvalid;
        //                                     return result;
        //                                 }
        //                             }
        //                             else
        //                             {
        //                                 int RowId = i + 1;

        //                                 if (!string.IsNullOrEmpty(dr[0] + ""))  //Check data Column
        //                                 {
        //                                     // string sMessageNotPass = "";
        //                                     bool IsPassValidateData = true;

        //                                     // check Data input 
        //                                     string? sCode = (dr[0] + "").Trim();
        //                                     string? sDetail = (dr[1] + "").Trim();

        //                                     string? sID = (dr[19] + "").Trim();


        //                                     bool IsEmpty = string.IsNullOrEmpty(sCode);
        //                                     if (!IsEmpty)
        //                                     {
        //                                         cObjectImportExcel objData = new cObjectImportExcel();
        //                                         objData.sID = sID;
        //                                         objData.sCode = sCode != null ? sCode : "";
        //                                         objData.sDetail = sDetail != null ? sDetail : null;

        //                                         lstData.Add(objData);
        //                                         index++;
        //                                     }
        //                                 }
        //                             }
        //                         }
        //                     }
        //                 }
        //             }
        //         }
        //         result.lstData = lstData;
        //         result.nStatusCode = StatusCodes.Status200OK;
        //     }
        //     catch (Exception e)
        //     {
        //         result.nStatusCode = StatusCodes.Status500InternalServerError;
        //         result.sMessage = e.Message;
        //     }

        //     return result;
        // }

        public cExportExcel LeaveExportExcel(cLeaveExport req)
        {
            cExportExcel result = new cExportExcel();
            DateTime dDate = DateTime.Now;
            string sDate = dDate.ToString("DDMMYYYYHHmmss");
            string sFileName = "LeaveSummary_" + sDate;
            try
            {
                var wb = new XLWorkbook();
                IXLWorksheet ws = wb.Worksheets.Add("data");
                int nFontSize = 14; //ขนาดฟอนต์
                int nFontSizeHead = 16;

                ws.PageSetup.Margins.Top = 0.2;
                ws.PageSetup.Margins.Bottom = 0.2;
                ws.PageSetup.Margins.Left = 0.1;
                ws.PageSetup.Margins.Right = 0;
                ws.PageSetup.Margins.Footer = 0;
                ws.PageSetup.Margins.Header = 0;
                //ws.Columns("A:Z").Style.NumberFormat.Format = "@"; //ป้องกันแปลงวันที่
                //ws.Range("A1:B1").Merge(); //การรวม Cell

                List<string> lstLeaveHead = new List<string>();
                List<string> lstLeaveType = new List<string>();
                foreach (var type in req.lstType)
                {
                    lstLeaveHead.Add(type.label);
                    lstLeaveType.Add(type.value);
                }

                #region  Header
                int nRow = 1;
                int nCol = 5;
                List<string> lstEmpHead = new List<string>() { "ชื่อพนักงาน", "วันที่เริ่ม", "ประเภทพนักงาน", "อายุงาน" };
                List<string> lstHeader = new List<string>(lstEmpHead.Concat(lstLeaveHead));
                List<int> lstwidthHeader = new List<int>() { 50, 15, 30, 15, 20, 20, 20, 20, 20, 20, 20, 20 };
                var RowsHead = ws;
                var itemCell = RowsHead.Cell(nRow, 1);

                int nColWidth = 1;
                foreach (var itmWidth in lstwidthHeader)
                {
                    ws.Column(nColWidth).Width = itmWidth;
                    nColWidth++;
                }
                int indexHead = 0;

                //nRow++; //ขึ้น Row ใหม่
                foreach (var item in lstHeader)
                {
                    indexHead += 1;
                    ws.Cell(nRow, indexHead).Value = item;
                    ws.Cell(nRow, indexHead).Style.Font.FontSize = nFontSizeHead;
                    ws.Cell(nRow, indexHead).Style.Font.Bold = true;
                    ws.Cell(nRow, indexHead).Style.Font.FontColor = XLColor.GrayAsparagus;
                    ws.Cell(nRow, indexHead).Style.Alignment.WrapText = true; //ปัดบรรทัด
                    ws.Cell(nRow, indexHead).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(nRow, indexHead).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, indexHead).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#68B984"));
                }
                #endregion

                int nStartBorder = nRow;// + 1;
                if (req.lstData != null && req.lstData.Any())
                {
                    foreach (var item in req.lstData)
                    {
                        nRow++; //ขึ้นบรรทัดใหม่                    

                        //ชื่อพนักงาน
                        ws.Cell(nRow, 1).Value = item.sNameTH;
                        ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(nRow, 1).Style.Font.FontSize = nFontSize;
                        ws.Cell(nRow, 1).Style.Alignment.WrapText = true;

                        //วันที่เริ่ม
                        ws.Cell(nRow, 2).Value = item.sStartLeave;
                        ws.Cell(nRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(nRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(nRow, 2).Style.Font.FontSize = nFontSize;
                        ws.Cell(nRow, 2).Style.Alignment.WrapText = true;

                        //ประเภทพนักงาน
                        ws.Cell(nRow, 3).Value = item.sEmpType;
                        ws.Cell(nRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(nRow, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(nRow, 3).Style.Font.FontSize = nFontSize;
                        ws.Cell(nRow, 3).Style.Alignment.WrapText = true;

                        //อายุงาน
                        ws.Cell(nRow, 4).Value = item.sEmpYear;
                        ws.Cell(nRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(nRow, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(nRow, 4).Style.Font.FontSize = nFontSize;
                        ws.Cell(nRow, 4).Style.Alignment.WrapText = true;

                        nCol = 5;
                        //ประเภทการลา
                        // foreach (var f in item.lstLeaveDetail)
                        item.lstLeaveDetail.ForEach(f =>
                        {
                            foreach (var leave in lstLeaveType)
                            {
                                if (f.nLeaveTypeID.ToString() == leave)
                                {
                                    ws.Cell(nRow, nCol).Value = f.sLeaveUse + "/" + f.sUse;
                                    ws.Cell(nRow, nCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ws.Cell(nRow, nCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                    ws.Cell(nRow, nCol).Style.Font.FontSize = nFontSize;
                                    ws.Cell(nRow, nCol).Style.Alignment.WrapText = true;
                                }
                            }
                            nCol++;// new column
                        });
                    }
                }
                else
                {
                    // No data
                    nRow++;
                    ws.Range("A" + nRow + ":B" + nRow).Merge();
                    ws.Cell(nRow, 1).Value = "ไม่พบข้อมูล";
                    ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                }
                //ระยะเส้นขอบที่ต้องการให้แสดง
                ws.Range(nStartBorder, 1, nRow, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(nStartBorder, 1, nRow, 12).Style.Border.InsideBorder = XLBorderStyleValues.Thin;


                using (MemoryStream fs = new MemoryStream())
                {
                    wb.SaveAs(fs);
                    wb.Dispose();
                    fs.Position = 0;
                    result.objFile = fs.ToArray();
                    result.sFileType = "application/vnd.ms-excel";
                    result.sFileName = sFileName;
                    return result;
                }
            }
            catch (System.Exception)
            {
                using (MemoryStream fs = new MemoryStream())
                {
                    var wb = new XLWorkbook();
                    IXLWorksheet ws = wb.Worksheets.Add("Error");
                    wb.SaveAs(fs);
                    wb.Dispose();
                    fs.Position = 0;
                    result.objFile = fs.ToArray();
                    result.sFileType = "application/vnd.ms-excel";
                    result.sFileName = sFileName;
                    // return result;
                }
            }

            return result;
        }

        public cResultTableLeave EditLeaveSummary(cEditLeave req)
        {
            cResultTableLeave result = new cResultTableLeave();
            try
            {
                DateTime dNow = DateTime.Now;
                // int nYear = dNow.Year;
                // int nYear = req.Year != null ? req.Year.ToInt() : dNow.Year;

                req.lstLeaveDate.ForEach(f =>
                {
                    f.lstLeaveDetail.ForEach(item =>
                    {
                        TB_LeaveSummary objLeaveSummary = _db.TB_LeaveSummary.FirstOrDefault(w => item.nLeaveSummaryID == w.nLeaveSummaryID && w.nEmployeeID == item.nEmployeeID && w.nLeaveTypeID == item.nLeaveTypeID && req.Year.Contains(w.nYear.ToString()));
                        if (objLeaveSummary != null)
                        {
                            objLeaveSummary.nLeaveUse = item.sLeaveData.ToDecimal() > 0 ? item.sLeaveData.ToDecimal() : objLeaveSummary.nLeaveUse;
                            objLeaveSummary.nQuantity = item.sUse.ToDecimal() > 0 ? item.sUse.ToDecimal() : objLeaveSummary.nQuantity;

                            if (objLeaveSummary.nQuantity != 0 && (objLeaveSummary.nLeaveRemain < (objLeaveSummary.nQuantity - objLeaveSummary.nLeaveUse)))
                            {
                                objLeaveSummary.nLeaveRemain = objLeaveSummary.nQuantity - objLeaveSummary.nLeaveUse;
                            }
                            else
                            {
                                objLeaveSummary.nLeaveRemain = objLeaveSummary.nLeaveRemain;
                            }

                            // if (objLeaveSummary.nLeaveRemain == 0 && objLeaveSummary.nQuantity != 0 && (objLeaveSummary.nLeaveUse + objLeaveSummary.nQuantity != objLeaveSummary.nLeaveRemain))
                            // {
                            //     objLeaveSummary.nLeaveRemain = objLeaveSummary.nQuantity;
                            // }
                            // else if (objLeaveSummary.nLeaveRemain < 1 && objLeaveSummary.nQuantity != 0 && (objLeaveSummary.nLeaveUse + objLeaveSummary.nQuantity != objLeaveSummary.nLeaveRemain))
                            // {
                            //     objLeaveSummary.nLeaveRemain = objLeaveSummary.nQuantity;
                            // }
                            _db.TB_LeaveSummary.Update(objLeaveSummary);
                        }
                    });
                });

                _db.SaveChanges();
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }

        public ResultAPI ImportExcel(cImport objSaveData)
        {
            ResultAPI result = new ResultAPI();
            // bool IsNew = false;
            try
            {
                var dNow = DateTime.Now;
                var objInfo = new cImport();
                // string sFileName = objSaveData.sFileName_Sys;
                string sFileName = objSaveData.sSysFileName;
                var cDT = STFunction.OpenExcel2DataTable(sFileName);
                if (cDT.nStatusCode != 200) //checkว่าเปิดไฟล์ได้ไหม
                {
                    // result.Status = StatusCodes.Status400BadRequest;
                    result.Message = "กรุณาอัปโหลดไฟล์";
                    return result;
                }
                var DT_Risk = cDT.ds.Tables[0];

                var leaveSumID = _db.TB_LeaveSummary.Any() ? _db.TB_LeaveSummary.Max(m => m.nLeaveSummaryID) + 1 : 1;

                // List<cApprove> lstApprove = new List<cApprove>();
                // List<EntrySearchUnit> lstUnitPIS = new List<EntrySearchUnit>();
                List<cLeaveExport> lstUnitPIS = new List<cLeaveExport>();

                // Fix Data 1=EMOC 2=PMP 3=INCR 4=ENVI 5=RBI 6=AIMS
                string[] arrTypeFix_Unit = new string[] { "5" };
                string[] arrTypeFix_Process = new string[] { "1", "2", "3", "4", "5", "6" };
                string[] arrTypeFix_ReviewPeriod = new string[] { "1", "2", "3", "4", "5", "6" };
                string[] arrTypeFix_RiskFactor = new string[] { "4", "6" };
                string[] arrTypeFix_TopQ = new string[] { "6" };
                string[] arrTypeFix_RiskArea = new string[] { "2", "4" };
                string[] arrTypeFix_RiskSource = new string[] { "1", "2", "4", "5", "6" };
                string[] arrTypeFix_Treatment = new string[] { "1", "4", "5", "6" };
                // string[] arrTypeFix_SubImp = new string[] { "4" };

                var pDate = DateTime.Now.ToString("yyy").ToInt();
                int nRow = 0;

                List<string> arrAllDebug = new List<string>();
                string sAgency = "";
                string sProcess = "";
                string sRiskSource = "";
                string sTreatment = "";
                string sReviewPeriod = "";
                string sRiskFactor = "";
                string sTopQuatile = "";
                string sRiskArea = "";

                // List<string> lstLeaveHead = new List<string>();
                // List<string> lstLeaveType = new List<string>();
                // List<cSelectOption> lstOpt = new List<cSelectOption>();
                // foreach (var type in lstOpt)
                // {
                //     lstLeaveHead.Add(type.label);
                //     lstLeaveType.Add(type.value);
                // }

                // #region  Header
                // // int nRow = 1;
                // // int nCol = 5;
                // List<string> lstEmpHead = new List<string>() { "ชื่อพนักงาน", "วันที่เริ่ม", "ประเภทพนักงาน", "อายุงาน" };
                // List<string> lstHeader = new List<string>(lstEmpHead.Concat(lstLeaveHead));

                string sNameTH = "";
                string sStartLeave = "";
                string sEmpType = "";
                string sEmpYear = "";
                // string sLeaveName = "";

                if (DT_Risk.Rows.Count > 0)
                {
                    Func<int, string> Col = new Func<int, string>(n => (DT_Risk.Rows[2][n] + "").Trim());
                    string sSystem = Col(2).Split("_")[0];

                    bool isPassSys = false;
                    switch (objSaveData.sSystem)
                    {
                        case "1":
                            isPassSys = sSystem == "MOC";
                            break;
                        case "2":
                            isPassSys = sSystem == "PMP";
                            break;
                        case "3":
                            isPassSys = sSystem == "INCR";
                            break;
                        case "4":
                            isPassSys = sSystem == "ENV";
                            break;
                        case "5":
                            isPassSys = sSystem == "RBI";
                            break;
                        case "6":
                            isPassSys = sSystem == "AIMS";
                            break;
                    }

                    if (!isPassSys)
                    {
                        result.Message = "Template Excel ไม่ตรงกับระบบที่ต้องการ Import";
                        result.Status = StatusCodes.Status400BadRequest;
                        return result;
                    }
                }

                // string sSubImpact = "";
                foreach (DataRow dr in DT_Risk.Rows)
                {
                    // List<AllClass.SelectOption> arrDebug = new List<AllClass.SelectOption>();
                    // List<AllClass.SelectOption> arrDontSaveDebug = new List<AllClass.SelectOption>();
                    Func<int, string> Col = new Func<int, string>(n => (dr[n] + "").Trim());
                    nRow++;
                    // if (nRow <= 4)
                    // {
                    //     if (nRow == 3) // Fix Data (สีม่วง)
                    //     {
                    //         sStartLeave = Col(1).Trim();
                    //         sEmpType = Col(2).Trim();
                    //         sEmpYear = Col(3).Trim();
                    //         // sLeaveName = Col(4).Trim();

                    //         // sRiskFactor = Col(6).Trim();
                    //         // sTopQuatile = Col(7).Trim();
                    //         // sRiskArea = Col(8).Trim();
                    //         // sRiskSource = Col(9).Trim();
                    //         // // sSubImpact = Col(12).Trim();
                    //         // sTreatment = Col(13).Trim();
                    //     }
                    //     continue;
                    // }

                    sNameTH = Col(0).Trim();//arrTypeFix_Unit.Contains(objSaveData.sSystem) ? sAgency : Col(0).Trim();
                    sStartLeave = Col(1).Trim();
                    sEmpType = Col(2).Trim();
                    sEmpYear = Col(3).Trim();

                    // //  Process
                    // sProcess = arrTypeFix_Process.Contains(objSaveData.sSystem) ? sProcess : Col(1).Trim();
                    // //  Resource ID
                    // string sResourceID = Col(2).Trim();
                    // //  Risk Name
                    // string sRiskName = Col(3).Trim();
                    // //  Risk Description
                    // string sRiskDescription = Col(4).Trim();
                    // //  Review Period
                    // sReviewPeriod = arrTypeFix_ReviewPeriod.Contains(objSaveData.sSystem) ? sReviewPeriod : Col(5).Trim();
                    // //  RiskFactor
                    // sRiskFactor = arrTypeFix_RiskFactor.Contains(objSaveData.sSystem) ? sRiskFactor : Col(6).Trim();
                    // //  Top Quatile Objectives
                    // sTopQuatile = arrTypeFix_TopQ.Contains(objSaveData.sSystem) ? sTopQuatile : Col(7).Trim();
                    // //  Risk Area
                    // sRiskArea = arrTypeFix_RiskArea.Contains(objSaveData.sSystem) ? sRiskArea : Col(8).Trim();
                    // //  Risk Source
                    // sRiskSource = arrTypeFix_RiskSource.Contains(objSaveData.sSystem) ? sRiskSource : Col(9).Trim();
                    // //  Risk Impact Category
                    // string sImpactCategory = Col(10).Trim();
                    // //  Risk Likelihood Category
                    // string sLikelihoodCategory = Col(11).Trim();
                    // //  Risk Sub-Impact
                    // string sSubImpact = Col(12).Trim();

                    // sTreatment = arrTypeFix_Treatment.Contains(objSaveData.sSystem) ? sTreatment : Col(13).Trim();
                    //  Inherent Risk
                    string sInherentRisk = Col(14).Trim();
                    //  Residual Risk
                    string sResidualRisk = Col(15).Trim();
                    //  Control(1)
                    string Control = Col(16).Trim();
                    //  Control Description(1)
                    string ControlDescription = Col(17).Trim();
                    //  Control Owner(1)
                    string ControlOwner = Col(18).Trim();
                    //  progress control
                    string progresscontrol = Col(19).Trim();
                    //code ID
                    string Systemcode = Col(20).Trim();

                    //เรียกหน่วยงาน
                    string sRisk_Unit_Abbr = "";
                    string sunitcode = "";
                    string sunitname = "";
                    string sOwner_Unit_Abbr = "";
                    string sunitcodeOwner = "";
                    string sunitnameOwner = "";
                    // if (objSaveData.sSystem == "5")
                    // {
                    //     sAgency = "ตร.วบก.";
                    // }

                    // if (string.IsNullOrEmpty(Systemcode) && !string.IsNullOrEmpty(objSaveData.sCheckSystemcode))
                    // {
                    //     result.Message = "คุณไม่ได้กรอก Systemcode";
                    //     result.Status = StatusCodes.Status404NotFound;
                    //     return result;
                    // }

                    TB_Employee objEmployee = _db.TB_Employee.FirstOrDefault(w => w.IsActive && !w.IsDelete);
                    TB_LeaveType objLeaveType = _db.TB_LeaveType.FirstOrDefault(w => w.IsActive && !w.IsDelete);
                    TB_LeaveSummary objLeaveSummary = _db.TB_LeaveSummary.FirstOrDefault(w => leaveSumID == w.nLeaveSummaryID && w.nEmployeeID == objEmployee.nEmployeeID && w.nLeaveTypeID == objLeaveType.nLeaveTypeID && w.nYear == dNow.Year);

                    //save into database
                    TB_LeaveSummary objLeave = null;
                    objLeave.nLeaveSummaryID = leaveSumID;
                    objLeave.nEmployeeID = (objEmployee.sNameTH + " " + objEmployee.sSurnameTH) == sNameTH ? objEmployee.nEmployeeID : 0;
                    // objLeave.nLeaveTypeID = objLeaveType.LeaveTypeName == sLeaveName ? objLeaveType.nLeaveTypeID : 0;
                    objLeave.nYear = dNow.Year;
                    objLeave.nQuantity = 30;//วันลาทั้งหมด
                    objLeave.nTransferred = 0;
                    objLeave.nIntoMoney = 0;
                    objLeave.nLeaveUse = 0;//วันลาที่ใช้
                    objLeave.nLeaveRemain = 0;
                    objLeave.dCreate = dNow;
                    objLeave.nCreateBy = _user.nUserID;
                    objLeave.dUpdate = dNow;
                    objLeave.nUpdateBy = _user.nUserID;
                    objLeave.IsDelete = false;
                    objLeave.dDelete = null;
                    objLeave.nDeleteBy = null;

                    // //save into database
                    // T_Risk objRisk = null;
                    // objRisk.nRisk_Area_ID = nRiskArea;
                    // objRisk.nRisk_Frequency_ID = nFrequency_ID;
                    // objRisk.nRisk_Process_ID = nProcess_ID;
                    // objRisk.nRisk_Source_ID = nSource_ID;
                    // objRisk.nRisk_Quality_ID = nTopQuatile;
                    // objRisk.nRisk_Factor_ID = nRiskFactor;
                    // objRisk.nRisk_Inherent_ID = nInherent;
                    // objRisk.nRisk_Residual_ID = nResidual;
                    // objRisk.nRisk_Owner_ID = nRisk_Owner_ID;
                    // objRisk.sRisk_Owner_Code = sRisk_Owner_Code;
                    // objRisk.sRisk_Owner_Name = sRisk_Owner_Name;
                    // objRisk.sRisk_Unit_Code = sRisk_Unit_Code;
                    // objRisk.sRisk_Unit_Abbr = sRisk_Unit_Abbr;
                    // objRisk.sRisk_Unit_Name = sRisk_Unit_Name;
                    // objRisk.sRisk_Section_Manager_Code = sRisk_Section_Manager_Code;
                    // objRisk.sRisk_Section_Manager_Name = sRisk_Section_Manager_Name;
                    // objRisk.sRisk_Section_Code = sRisk_Section_Code;
                    // objRisk.sRisk_Section_Name = sRisk_Section_Name;
                    // objRisk.sRisk_Division_Manager_Code = sRisk_Division_Manager_Code;
                    // objRisk.sRisk_Division_Manager_Name = sRisk_Division_Manager_Name;
                    // objRisk.sRisk_Division_Code = sRisk_Division_Code;
                    // objRisk.sRisk_Division_Name = sRisk_Division_Name;
                    // objRisk.sRisk_Department_Manager_Code = sRisk_Department_Manager_Code;
                    // objRisk.sRisk_Department_Manager_Name = sRisk_Department_Manager_Name;
                    // objRisk.sRisk_Department_Code = sRisk_Department_Code;
                    // objRisk.sRisk_Department_Name = sRisk_Department_Name;
                    // objRisk.nRisk_CorporateArea_ID = 0;
                    // objRisk.nRisk_SubProcess_ID = nRisk_SubProcess_ID;
                    // objRisk.sRisk_Name = sRiskName;
                    // objRisk.IsRiskConrinuity = false;
                    // objRisk.sRisk_Description = sRiskDescription;
                    // objRisk.nRisk_SM_ID = 0;
                    // objRisk.nRisk_IIF_ID = 0;
                    // objRisk.nRisk_Stakeholder_ID = 0;
                    // objRisk.nRisk_OpEx_Element = 0;
                    // objRisk.nRisk_Current_ID = 0;
                    // objRisk.nRisk_Status = 0; // Draft
                    // objRisk.sUpdate_By = UserSession.nEmployee_ID.ToString();
                    // objRisk.sUpdateName_By = UserSession.sEmployee_Short_Name;
                    // objRisk.IsDelete = false;
                    // objRisk.nRisk_Version = 1;
                    // objRisk.dUpdate_By = DateTime.Now;
                    // db.T_Risk.Add(objRisk);
                }
                _db.SaveChanges();

                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }

        public static string? GetPathUploadFile(string FilePath, string SysFileName)
        {
            string? sFullPath = null;
            if (!string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(SysFileName))
            {
                // string sPathWeb = STFunction.GetAppSettingJson("AppSetting:UrlSiteBackend");
                sFullPath = "UploadFile/" + FilePath + "/" + SysFileName;
            }
            return sFullPath;
        }

        public static string GetAppSettingJson(string GetParameter)
        {
            string Result = "";
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            IConfigurationSection section = configuration.GetSection(GetParameter);
            Result = section.Value;
            return Result;
        }

    }
}