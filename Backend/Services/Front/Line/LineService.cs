using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Extensions.Common.STFunction;
using ST_API.Interfaces;
using ST_API.Models;
using Extensions.Common.STResultAPI;
using Extensions.Common.STExtension;
using Extensions.Systems;
using Backend.Interfaces.Authentication;
using Backend.EF.ST_Intranet;
using Backend.Enum;

namespace ST_API.Services.ISystemService
{
    public class LineService : ILineService
    {
        private readonly IConfiguration _config;

        private readonly IAuthentication _authen;

        private readonly ST_IntranetEntity _db;

        private readonly IHostEnvironment _env;

        /// <summary>
        /// MeetingRoomService
        /// </summary>
        public LineService(
            IConfiguration config,
            IAuthentication authen,
            ST_IntranetEntity db,
            IHostEnvironment env
        )
        {
            _config = config;
            _authen = authen;
            _db = db;
            _env = env;
        }
        public string TestSend()
        {
            var objTest = new cUser();

            DateTime dDateNow = DateTime.Now.Date;
            DateTime dDateCheck = dDateNow.AddHours(9);
            var objTime = _db.TB_Employee_TimeStemp.FirstOrDefault(w => w.nTimeStampID == 7);
            double nCalTime = (dDateCheck - objTime.dTimeStartDate).TotalMinutes;
            int nMinute = Math.Abs(Convert.ToInt32(nCalTime));
            string xx = nMinute.ToString();

            var Test = JsonSerializer.Serialize(objTest);
            return Test;
        }
        public void Webhook(dynamic objReq)
        {
            string sWebhook = JsonSerializer.Serialize(objReq);
            if (!string.IsNullOrEmpty(sWebhook))
            {

                Events? events = JsonSerializer.Deserialize<Events>(sWebhook) as Events;

                if (events != null)
                {
                    if (events.lstEvent != null)
                    {

                        events.lstEvent.ForEach(f =>
                        {
                            var objSource = f.objSource;
                            var objMessage = f.objMessage;
                            string sUserName = "";
                            int nEmpID = 0;
                            string sUserID = "";

                            string sLocation = "Office";
                            cParamSendLine objParam = new cParamSendLine();
                            objParam.nTemplateID = 0;
                            objParam.sLocation = sLocation;
                            objParam.lstEmpTo = new List<int>();
                            bool IsFullTask = false;
                            int nTask = 0;
                            bool IsWaittingApprove = false;
                            int nWaittingApprove = 0;
                            if (objSource != null)
                            {
                                if (!string.IsNullOrEmpty(objSource.sUserID))
                                {
                                    sUserID = objSource.sUserID;
                                    sUserName = GetName(objSource.sUserID, 0);
                                    TB_Employee_LineAccount? objLine = _db.TB_Employee_LineAccount.FirstOrDefault(w => w.sTokenID == objSource.sUserID);
                                    if (objLine != null)
                                    {
                                        objLine.sNameAccount = sUserName;
                                        nEmpID = objLine.nEmployeeID;
                                    }
                                    _db.SaveChanges();
                                }
                            }

                            var lst = _db.TB_Employee_Image.Select(s => new
                            {

                                sPath = STFunction.GetPathUploadFile(s.sPath ?? "", s.sSystemFileName ?? "")
                            }).ToList();
                            DateTime dDateNow = DateTime.Now.Date;
                            bool IsTimestamp = false; //f.sType == "postback" || f.sType == "beacon";
                            bool IsCheckIn = false;
                            bool IsCheckOut = false;
                            var objWFH = _db.TB_WFH.FirstOrDefault(w => w.nCreateBy == nEmpID && w.dWFH == dDateNow && (w.nFlowProcessID != 7 && w.nFlowProcessID != 1));
                            DateTime dDateLine = f.nTimestamp.toDateTime();
                            if (objWFH != null)
                            {
                                objParam.sLocation = "Work Fome Home";
                            }
                            DateTime dDateEnd = dDateNow.AddHours(18);
                            DateTime dDateStart = dDateNow.AddHours(9);


                            if (f.sType == "postback")
                            {
                                if (f.Postback != null)
                                {
                                    IsCheckIn = f.Postback.sData == "IN"; // Add Table Beacon
                                    IsCheckOut = f.Postback.sData == "OUT";
                                      IsTimestamp = true;
                                    if (IsCheckIn)
                                    {
                                        if (objWFH != null)
                                        {
                                            objParam.sLocation = "Work Fome Home";
                                        }
                                        else
                                        {
                                            var objBeacon = _db.TB_Log_Beacon.FirstOrDefault(w => w.sUserId == sUserID && w.dTimeStamp.Date == dDateNow.Date);
                                            if (objBeacon == null)
                                            {
                                                    objParam.sLocation = "นอกสถานที่";
                                            }
                                        }
                                    }

                                }
                            }
                            else  if (f.sType == "message")
                            {
                               if(f.objMessage != null)
                                {
                                     IsTimestamp = true;
                                    IsCheckIn = f.objMessage.sText.ToUpper() == "IN"; // Add Table Beacon
                                    IsCheckOut = f.objMessage.sText.ToUpper() == "OUT";
                                    if (IsCheckIn)
                                    {
                                        if (objWFH != null)
                                        {
                                            objParam.sLocation = "Work Fome Home";
                                           
                                        }
                                    }
                                }
                            }
                            else if (f.sType == "beacon")
                            {
                                TB_Log_Beacon objBeacon = new TB_Log_Beacon();
                                objBeacon.dSend = DateTime.Now;
                                objBeacon.dTimeStamp = dDateNow;
                                objBeacon.sUserId = sUserID;
                                objBeacon.sReplyToken = f.sReplyToken;
                                _db.TB_Log_Beacon.Add(objBeacon);
                                // OT
                                if (!_db.TB_HolidayDay.Any(w => w.dDate == dDateNow) && dDateNow.DayOfWeek != DayOfWeek.Saturday && dDateNow.DayOfWeek != DayOfWeek.Sunday)
                                {

                                    TB_Employee_TimeStemp? objTime = _db.TB_Employee_TimeStemp.Where(w => w.nEmployeeID == nEmpID && !w.dTimeEndDate.HasValue).OrderByDescending(or => or.dTimeStartDate).FirstOrDefault();
                                    if (objTime != null)
                                    {
                                        IsTimestamp = true;
                                        DateTime dDateCheckOut = objTime.dTimeDate.AddHours(18);
                                        if (dDateLine > dDateCheckOut)
                                        {
                                            IsCheckOut = true;
                                        }
                                    }
                                }
                            }
                            var objRequestLeave = _db.TB_Request_Leave.FirstOrDefault(w => w.nEmployeeID == nEmpID && w.nStatusID > 1 && (w.dStartDateTime.Date <= dDateNow.Date && w.dEndDateTime.Date >= dDateNow.Date));
                            if (objRequestLeave != null)
                            {
                                if (objRequestLeave.nLeaveUse < 1)
                                {
                                    dDateStart = objRequestLeave.dEndDateTime;
                                    if (dDateStart.Hour == 12)
                                    {
                                        dDateStart = dDateNow.AddHours(13);
                                    }
                                }
                                

                            }

                            var lstTask = _db.TB_Task.Where(w => w.nEmployeeID == nEmpID && w.dTask.Date == dDateNow.Date).ToList();
                         if(IsCheckOut)
                         {
                                var dDateCheck = dDateNow.AddDays(-1);
                            var objTime = _db.TB_Employee_TimeStemp.Where(w => w.nEmployeeID == nEmpID && !w.dTimeEndDate.HasValue && w.dTimeDate.Date >= dDateCheck).OrderByDescending(or => or.dTimeDate).FirstOrDefault();
                            if(objTime != null)
                            {
                                lstTask = _db.TB_Task.Where(w => w.nEmployeeID == nEmpID && w.dTask.Date == objTime.dTimeDate.Date).ToList();
                                    //dDateNow = objTime.dTimeDate.Date;
                                    dDateNow = objTime.dTimeDate.Date;
                            }
                            else
                                {
                                    objTime = _db.TB_Employee_TimeStemp.Where(w => w.nEmployeeID == nEmpID).OrderByDescending(or => or.dTimeDate).FirstOrDefault();
                                    if (objTime != null)
                                    {
                                        lstTask = _db.TB_Task.Where(w => w.nEmployeeID == nEmpID && w.dTask.Date == objTime.dTimeDate.Date).ToList();
                                        //dDateNow = objTime.dTimeDate.Date;
                                        dDateNow = objTime.dTimeDate.Date;
                                    }
                                }
                         }
                            bool IsTask = lstTask.Any(w => w.nEmployeeID == nEmpID && w.dTask.Date == dDateNow.Date);


                            if (!_db.TB_HolidayDay.Any(w => w.dDate.Date == dDateNow.Date) && dDateNow.DayOfWeek != DayOfWeek.Saturday && dDateNow.DayOfWeek != DayOfWeek.Sunday)
                            {

                                if (IsTask)
                                {
                                    int nWorkHour = 8;
                                    if (dDateStart.Hour != 9)
                                    {
                                        nWorkHour = (dDateEnd - dDateStart).TotalHours.ToString().toInt();
                                        if (dDateStart.Hour < 12)
                                        {
                                            nWorkHour = nWorkHour - 1;
                                        }
                                    }
                                    IsFullTask = lstTask.Where(w => w.nActual.HasValue).Sum(w => w.nActual.Value) >= nWorkHour;
                                    var lstTaskCheck = lstTask.Where(w => !w.nActual.HasValue).ToList();
                                    nTask = lstTask.Where(w => !w.nActual.HasValue).Count();
                                }
                            }

                            if (_db.TB_Request_OT.Any(w => !w.IsDelete && (w.nStatusID != 1 && w.nStatusID != 0) && w.nCreateBy == nEmpID && w.dPlanDateTime.Date == dDateNow.Date))
                            {
                                IsFullTask = true;
                            }
                            if (IsTimestamp)
                            {
                                if (IsCheckIn)
                                {
                                    TB_Employee_TimeStemp? objTime = _db.TB_Employee_TimeStemp.FirstOrDefault(w => w.nEmployeeID == nEmpID && w.dTimeDate == dDateNow);
                                    if (objTime == null)
                                    {
                                        objTime = new TB_Employee_TimeStemp();
                                        objTime.nTimeStampID = (_db.TB_Employee_TimeStemp.Any() ? _db.TB_Employee_TimeStemp.Max(m => m.nTimeStampID) : 0) + 1;
                                        objTime.nEmployeeID = nEmpID;
                                        objTime.dTimeStartDate = f.nTimestamp.toDateTime();
                                        string sTestStart = DateTime.Now.toUnixTime().ToString();
                                        objTime.dTimeEndDate = null;
                                        objTime.dTimeDate = dDateNow;
                                        _db.TB_Employee_TimeStemp.Add(objTime);

                                        DateTime dDateCheck = dDateNow.AddHours(9);
                                        // ลาเช้าเข้าบ่าย

                                        var objReqLeave = _db.TB_Request_Leave.FirstOrDefault(w => w.nEmployeeID == nEmpID && w.dStartDateTime.Date == dDateNow.Date);
                                        if (objReqLeave != null)
                                        {
                                            dDateCheck = objReqLeave.dEndDateTime;

                                            if (dDateCheck.Hour == 12)
                                            {
                                                dDateCheck = dDateNow.AddHours(13);

                                            }
                                        }
                                        double nCalTime = Math.Round((dDateCheck - objTime.dTimeStartDate).TotalMinutes, MidpointRounding.ToZero);
                                        if (_db.TB_Request_OT.Any(w => !w.IsDelete && (w.nStatusID != 1 && w.nStatusID != 0) && w.dPlanDateTime.Date == dDateNow.Date))
                                        {
                                            nCalTime = 0;
                                        }
                                        objTime.IsDelay = (nCalTime < 0);
                                        objTime.nMinute = objTime.IsDelay ? Math.Abs(nCalTime.ToString().toInt()) : 0;
                                        objTime.nMinuteByHR = objTime.nMinute;


                                    }
                                    int nYear = objTime.dTimeDate.Year;
                                    int nMinuteCollap = _db.TB_Employee_TimeStemp.Where(w => w.nEmployeeID == nEmpID && w.dTimeDate.Year == nYear && w.dTimeDate.Date != dDateNow.Date).Sum(s => s.nMinuteByHR);
                                    objParam.sEndDate = nMinuteCollap.ToString();
                                    if (!objTime.IsDelay)
                                    {
                                        DateTime dDateCheck = dDateNow.AddHours(9);
                                        double nCalTime = Math.Round((dDateCheck - objTime.dTimeStartDate).TotalMinutes, MidpointRounding.ToZero);
                                        int nMinute = Math.Abs(nCalTime.ToString().toInt());
                                        objParam.sStartDate = nMinute == 0 ? "1" : nMinute.ToString();
                                        objParam.nTemplateID = nMinuteCollap > 0 ? 2 : 1;
                                    }
                                    else
                                    {
                                        objParam.nTemplateID = nMinuteCollap > 0 ? 4 : 3;
                                    }
                                    objParam.sTime = objTime.dTimeStartDate.ToStringFromDate("HH:mm");
                                    objParam.sDate = objTime.dTimeDate.ToStringFromDate("dd/MM/yyyy");
                                    objParam.sTime = objTime.dTimeStartDate.ToStringFromDate("HH:mm:ss");

                                    objParam.sDelay = objTime.nMinuteByHR > 0 ? objTime.nMinuteByHR + "" : "-";

                                    var objConfig = _db.TM_Config.FirstOrDefault(w => w.nConfigID == (int)EnumGlobal.Config.CheckIn);
                                    if (objConfig != null)
                                    {
                                        objParam.sStartTime = objConfig.sValue ?? "";
                                    }
                                    objParam.sEndTime = objTime.dTimeStartDate.ToStringFromDate("HH:mm");

                                }
                                else if (IsCheckOut)
                                {

                                    var objTime = _db.TB_Employee_TimeStemp.Where(w => w.nEmployeeID == nEmpID && !w.dTimeEndDate.HasValue).OrderByDescending(or => or.dTimeDate).FirstOrDefault();
                                    if (objTime != null)
                                    {
                                        if(IsFullTask || _db.TB_Request_OT.Any(w => !w.IsDelete && (w.nStatusID != 1 && w.nStatusID != 0) && w.nCreateBy == nEmpID && w.dPlanDateTime.Date == dDateNow.Date))
                                        {
                                            objTime.dTimeEndDate = f.nTimestamp.toDateTime();
                                        }
                                     
                                        objParam.sTime = objTime.dTimeEndDate.HasValue ? objTime.dTimeEndDate.Value.ToStringFromDate("HH:mm") : "-";
                                        objParam.sDate = objTime.dTimeDate.ToStringFromDate("dd/MM/yyyy");
                                        objParam.sTime = objTime.dTimeEndDate.HasValue ? objTime.dTimeEndDate.Value.ToStringFromDate("HH:mm") : "-";
                                        objParam.nTemplateID = 3;
                                        objParam.sEndTime = objTime.dTimeEndDate.HasValue ? objTime.dTimeEndDate.Value.ToStringFromDate("HH:mm") : "";

                                        if (objTime.IsDelay)
                                        {
                                            DateTime dDateCheckOut = objTime.dTimeDate.AddHours(19);
                                            if (dDateLine > dDateCheckOut)
                                            {
                                                objTime.nMinute = (objTime.nMinute - 30) <= 0 ? 0 : (objTime.nMinute - 30);
                                                objTime.nMinuteByHR = objTime.nMinuteByHR;
                                                if (objTime.nMinuteByHR <= 0)
                                                {
                                                    objTime.IsDelay = false;
                                                }
                                                _db.SaveChanges();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        objTime = _db.TB_Employee_TimeStemp.Where(w => w.nEmployeeID == nEmpID && w.dTimeEndDate.HasValue).OrderByDescending(or => or.dTimeEndDate).FirstOrDefault();
                                        if (objTime != null)
                                        {
                                            objTime.dTimeEndDate = f.nTimestamp.toDateTime();
                                            objParam.sTime = objTime.dTimeEndDate.Value.ToStringFromDate("HH:mm");
                                            objParam.sDate = objTime.dTimeDate.ToStringFromDate("dd/MM/yyyy");
                                            objParam.sTime = objTime.dTimeEndDate.Value.ToStringFromDate("HH:mm:ss");
                                            objParam.nTemplateID = 3;
                                            objParam.sEndTime = objTime.dTimeEndDate.HasValue ? objTime.dTimeEndDate.Value.ToStringFromDate("HH:mm") : "";

                                            if (objTime.IsDelay)
                                            {
                                                DateTime dDateCheckOut = objTime.dTimeDate.AddHours(19);
                                                if (dDateLine > dDateCheckOut)
                                                {
                                                    objTime.nMinute = (objTime.nMinute - 30) <= 0 ? 0 : (objTime.nMinute - 30);
                                                    objTime.nMinuteByHR = objTime.nMinuteByHR;
                                                    if (objTime.nMinuteByHR <= 0)
                                                    {
                                                        objTime.IsDelay = false;
                                                    }
                                                    _db.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                    int nYear = objTime.dTimeDate.Year;
                                    int nMinuteCollap = _db.TB_Employee_TimeStemp.Where(w => w.nEmployeeID == nEmpID && w.dTimeDate.Year == nYear).Sum(s => s.nMinute);
                                    objParam.sEndDate = nMinuteCollap.ToString();

                                  
                                    if (nMinuteCollap > 0)
                                    {
                                        objParam.nTemplateID = 6;
                                        if (!IsTask)
                                        {
                                            objParam.nTemplateID = 8;
                                        }
                                        if (!IsFullTask)
                                        {
                                            objParam.nTemplateID = 10;
                                        }

                                    }
                                    else
                                    {
                                        objParam.nTemplateID = 5;
                                        if (!IsTask)
                                        {
                                            objParam.nTemplateID = 7;
                                        }
                                        if (!IsFullTask)
                                        {
                                            objParam.nTemplateID = 9;
                                        }
                                    }
                                    var objConfig = _db.TM_Config.FirstOrDefault(w => w.nConfigID == (int)EnumGlobal.Config.CheckOUt);
                                    if (objConfig != null)
                                    {
                                        objParam.sStartTime = objConfig.sValue ?? "";
                                    }

                                }
                                if (objParam.nTemplateID != 0)
                                {
                                    objParam.nRequesterID = nEmpID;
                                    objParam.lstEmpTo.Add(nEmpID);
                                    if (IsFullTask || IsCheckIn)
                                    {
                                        STFunction.SendToLine(objParam);
                                    }
                                 
                                    string sDetail = "";
                                    string sFormatTitle = "";
                                    string sFormatDetail = "";
                                    TM_LineTemplate? objTemplate = _db.TM_LineTemplate.FirstOrDefault(w => w.nID == 96);
                                    if (objTemplate != null)
                                    {
                                        sFormatTitle = objTemplate.sDetail;
                                    }
                                    objTemplate = _db.TM_LineTemplate.FirstOrDefault(w => w.nID == 92);
                                    if (objTemplate != null)
                                    {
                                        sFormatDetail = objTemplate.sDetail;
                                    }
                                    if (IsCheckIn)
                                    {
                                        bool IsMeeting = false;
                                        bool IsMyTask = false;
                                        bool IsMember = false;


                                        #region My Task

                                        var lstMyTask = _db.TB_Task.Where(w => w.dTask.Date == dDateNow && !w.IsDelete && w.nEmployeeID == nEmpID).ToList();
                                        IsMyTask = lstMyTask.Count() == 0;
                                        int nMyTask = lstMyTask.Count();
                                        #endregion

                                        #region Meeting
                                        var lstMeeting = _db.TB_Meeting.Where(w => (w.dStart.Date <= dDateNow && w.dEnd.Date >= dDateNow)).ToList();
                                        var lstMeetingID = lstMeeting.Select(s => s.nMeetingID).ToList();
                                        var lstMeetingPersion = _db.TB_Meeting_Person.Where(w => lstMeetingID.Contains(w.nMeetingID) && w.nEmployeeID == nEmpID).Select(s => s.nMeetingID).ToList();
                                        var lstMeetingUser = lstMeeting.Where(w => lstMeetingPersion.Contains(w.nMeetingID)).ToList();
                                        IsMeeting = lstMeetingUser.Count() > 0;
                                        int nMeeting = lstMeetingUser.Count();
                                        #endregion

                                        if (!IsMyTask)
                                        {
                                            if (!string.IsNullOrEmpty(sDetail))
                                            {
                                                sDetail += ",";
                                            }
                                            sDetail = sFormatTitle.Replace("{95}", "My Task  ").Replace("{94}", nMyTask + "");
                                            string sSubDetail = "";
                                            lstMyTask.ForEach(f =>
                                            {
                                                var objProject = _db.TB_Project.FirstOrDefault(w => w.nProjectID == f.nProjectID);
                                                if(objProject != null)
                                                {
                                                    sSubDetail += ",";
                                                    string sAbbr = !string.IsNullOrEmpty(objProject.sProjectAbbr) ? objProject.sProjectAbbr + " : " : "";
                                                    string sH = f.nPlan.HasValue ? $"({f.nPlan.Value} hr.)" : "";
                                                    sSubDetail += sFormatDetail.Replace("{92}", $"{sAbbr}{objProject.sProjectName} {sH}");
                                                }

                                            });
                                            sDetail += sSubDetail;
                                        }

                                        if (!IsMeeting)
                                        {
                                            if (!string.IsNullOrEmpty(sDetail))
                                            {
                                                sDetail += ",";
                                            }
                                            sDetail += sFormatTitle.Replace("{95}", "Meeting  ").Replace("{94}", nMeeting + "");
                                            string sSubDetail = "";
                                            lstMeetingUser.ForEach(f =>
                                            {
                                                var objProject = _db.TB_Project.FirstOrDefault(w => w.nProjectID == f.nProjectID);
                                                if (objProject != null)
                                                {
                                                    sSubDetail += ",";
                                                    string sAbbr = !string.IsNullOrEmpty(objProject.sProjectAbbr) ? objProject.sProjectAbbr + " : " : "";
                                                    string sH = $" เวลา {f.dStart.ToStringFromDate("HH:mm")} - {f.dEnd.ToStringFromDate("HH:mm")} {(f.dEnd - f.dStart).TotalHours.ToString()}";
                                                    sSubDetail += sFormatDetail.Replace("{92}", $"{sAbbr}{objProject.sProjectName} {sH}");
                                                }

                                            });
                                            sDetail += sSubDetail;
                                        }

                                        if (IsWaittingApprove)
                                        {
                                            if (!string.IsNullOrEmpty(sDetail))
                                            {
                                                sDetail += ",";
                                            }
                                            sDetail += sFormatTitle.Replace("{95}", "มีรายการที่ยังไม่อนุมัติ  ").Replace("{94}", nWaittingApprove + "");
                                        }
                                    }
                                    else if (IsCheckOut)
                                    {
                                        if (!IsFullTask)
                                        {
                                            sDetail = sFormatTitle.Replace("{95}", "คุณยังลง Task งานไม่ครบ  ").Replace("{94}", nTask + "");
                                        }
                                        if (IsWaittingApprove)
                                        {
                                            if (!string.IsNullOrEmpty(sDetail))
                                            {
                                                sDetail += ",";
                                            }
                                            sDetail += sFormatTitle.Replace("{95}", "มีรายการที่ยังไม่อนุมัติ  ").Replace("{94}", nWaittingApprove + "");
                                        }
                                    }

                                    //sDetail += sDetail.Replace("{93}", "");
                                    objParam.sDetailTemplate = sDetail;
                                    if (!string.IsNullOrEmpty(sDetail))
                                    {
                                        objParam.nTemplateID = 91;
                                        STFunction.SendToLine(objParam);
                                    }
                                }
                                _db.SaveChanges();



                            }
                        });
                    }
                }
                TB_Log_WebhookLine objWebhook = new TB_Log_WebhookLine();
                objWebhook.dSend = DateTime.Now;
                objWebhook.sMessage = sWebhook;
                _db.TB_Log_WebhookLine.Add(objWebhook);
                _db.SaveChanges();
            }

        }

        ///<Summary>
        /// Get Name Line
        ///</Summary>
        public string GetName(string sID, int nType)
        {
            WebClient serviceRequest = new WebClient();
            serviceRequest.Encoding = UTF8Encoding.UTF8;
            string sResult = "";

            serviceRequest.Headers.Add("Accept", "application/json");
            serviceRequest.Headers.Add("Authorization", "Bearer " + STFunction.sBearer);
            string url = $"{STFunction.sBaseAdrees}{STFunction.sGetProfile}{sID}";
            string response = serviceRequest.DownloadString(new Uri(url));
            cUser? objResult =
                JsonSerializer.Deserialize<cUser>(response) as cUser;


            if (objResult != null)
            {
                sResult = objResult.sDisplayName ?? "";
            }
            return sResult;
        }


    }
}
