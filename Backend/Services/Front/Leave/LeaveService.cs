// using Alert.Api.Hubs;
// using Alert.Api.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;
using ST_API.Interfaces;
using ST_API.Models;
using Microsoft.EntityFrameworkCore;
using Extensions.Common.STResultAPI;
using ExcelDataReader;
using System.Data;
using Extensions.Common.STFunction;
using ClosedXML.Excel;
using Backend.Interfaces.Authentication;
using Extensions.Common.STExtension;
using Backend.EF.ST_Intranet;
using ST.INFRA;
using ST.INFRA.Common;
using static Extensions.Systems.AllClass;
using Backend.Enum;
using Backend.Interfaces;
using Backend.Models.Authentication;
using ResultAPI = Extensions.Common.STResultAPI.ResultAPI;
using Backend.Models;
using System.Globalization;
using System.Linq;
using System.Net;

namespace ST_API.Services.ISystemService
{
    public class LeaveService : ILeaveService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _authen;
        private readonly IHostEnvironment _env;
        private int[] arrStatusRequestorAction = { (int)Backend.Enum.EnumLeave.Status.RejectLead, (int)Backend.Enum.EnumLeave.Status.RejectHr,
                         (int)Backend.Enum.EnumLeave.Status.Recall,(int)Backend.Enum.EnumLeave.Status.Draft };
        public LeaveService(ST_IntranetEntity db, IAuthentication authen, IHostEnvironment env)
        {
            _db = db;
            _authen = authen;
            _env = env;
        }

        public async Task<cFormInitRequest> GetFilterLeave()
        {
            cFormInitRequest result = new cFormInitRequest()
            {
                Status = StatusCodes.Status200OK,
            };

            try
            {
                _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                string sPosition = UserAccount.sPosition;
                int nPositionID = 0;
                if (!string.IsNullOrEmpty(sPosition))
                {
                    var Position = _db.TB_Position.FirstOrDefault(f => f.sPositionName == sPosition);
                    if (Position != null)
                    {
                        nPositionID = Position.nPositionID;
                    }
                }
                result.isHr = nPositionID == 9;
                int nEmployeeID = nUserID;
                TB_Request_Leave? objFormData = new TB_Request_Leave();

                result.arrOptionEmployee = await _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive).Select(s => new cSelectOptionleave
                {
                    label = $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})",
                    value = s.nEmployeeID.EncryptParameter()
                }).ToArrayAsync();

                result.arrOption = await
                (from lt in _db.TB_LeaveType
                 where lt.IsActive && !lt.IsDelete
                 orderby lt.nOrder
                 select new cSelectOptionleave
                 {
                     label = lt.LeaveTypeName,
                     value = lt.nLeaveTypeID.EncryptParameter()
                 }
                 ).ToArrayAsync();

                result.arrOptionStatus = await _db.TM_Status.Where(w => w.nRequestTypeID == 1
                ).OrderBy(o => o.nOrder).Select(s => new cSelectOptionleave
                {
                    label = s.sNextStatusName,
                    value = s.nStatusID.EncryptParameter()
                }).ToArrayAsync();

                result.lstDataEmployee = _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete).OrderBy(o => o.nEmployeeID).Select(s => new cSelectOptionleave
                {
                    label = $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})",
                    value = s.nEmployeeID + "",

                }).ToList();



                result.sEmployeeID = nUserID + "";

                List<string> lstUser = new List<string>();
                IQueryable<TB_Employee_Report_To> TBEmployeeReportTo = _db.TB_Employee_Report_To.Where(w => !w.IsDelete).OrderBy(o => o.nEmployeeID).AsQueryable();
                bool IsHead = TBEmployeeReportTo.Any(f => f.nRepEmployeeID == nUserID);
                result.isLead = IsHead;

                //Add User
                lstUser.Add(nUserID.EncryptParameter());

                //Add ลูกทีม
                if (IsHead)
                {
                    List<TB_Employee_Report_To>? lstEmployeeReportTo = TBEmployeeReportTo.Where(w => w.nRepEmployeeID == nUserID).ToList();
                    int idx = 0;
                    foreach (var item in lstEmployeeReportTo)
                    {
                        if (idx == 0)
                        {
                            lstUser.Add(nUserID.EncryptParameter());
                        }
                        lstUser.Add(item.nEmployeeID.EncryptParameter());
                        idx++;
                    }
                }
                result.lstSelectUser = lstUser;
                List<string> lstStatus = new List<string>();
                var lstFillStatus = _db.TM_Status.Where(w => w.nRequestTypeID == 1 && w.nStatusID != 0).OrderBy(o => o.nOrder).Select(s => s.nStatusID.EncryptParameter());
                lstStatus.AddRange(lstFillStatus);
                result.lstStatus = lstStatus;
            }
            catch (System.Exception e)
            {
                result.Message = e.Message;
                result.Status = StatusCodes.Status500InternalServerError;
            }
            return result;
        }

        public async Task<cFormInitRequest> GetFormRequest(string? sID)
        {
            cFormInitRequest result = new cFormInitRequest()
            {
                Status = StatusCodes.Status200OK,
                nPermission = (int)Backend.Enum.EnumEmployee.MenuPermission.Control,
                isRequestor = true,
                nStatus = (int)Backend.Enum.EnumLeave.Status.Draft
            };

            try
            {
                _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                string sPosition = UserAccount.sPosition;
                int nPositionID = 0;
                if (!string.IsNullOrEmpty(sPosition))
                {
                    var Position = _db.TB_Position.FirstOrDefault(f => f.sPositionName == sPosition);
                    if (Position != null)
                    {
                        nPositionID = Position.nPositionID;
                    }
                }

                int nEmployeeID = string.IsNullOrEmpty(sID) ? nUserID : sID.DecryptParameter().ToInt();
                TB_Request_Leave? objFormData = new TB_Request_Leave();
                if (!string.IsNullOrEmpty(sID))
                {
                    objFormData = await _db.TB_Request_Leave.FirstOrDefaultAsync(f => f.nRequestLeaveID == sID.DecryptParameter().ToInt() && !f.IsDelete);
                    if (objFormData == null)
                    {
                        result.Message = "Data not found.";
                        return result;
                    }
                    nEmployeeID = objFormData.nEmployeeID;
                    result.nUse = objFormData.nLeaveUse;
                    result.sTypeID = objFormData.nLeaveTypeID.EncryptParameter();
                    result.isEmergency = objFormData.IsEmergency;
                    result.dStart = objFormData.dStartDateTime;
                    result.dEnd = objFormData.dEndDateTime;
                    result.sReason = objFormData.sReason;
                    result.sID = objFormData.nRequestLeaveID.EncryptParameter();
                    result.sEmployeeID = objFormData.nEmployeeID + "";


                    var File = _db.TB_Leave_File.FirstOrDefault(w => w.nRequestLeaveID == objFormData.nRequestLeaveID && !w.IsDelete);
                    List<ItemFileData> lstfile = new List<ItemFileData>();
                    if (File != null)
                    {


                        ItemFileData fFile = new ItemFileData();
                        var sPath = GetPathUploadFile(File.sPath ?? "", File.sSystemFilename ?? "");
                        fFile.sFileName = File.sFilename;
                        fFile.sCropFileLink = sPath;
                        fFile.sFileLink = sPath;
                        fFile.sSysFileName = File.sSystemFilename;
                        fFile.sFileType = File.sSystemFilename != null ? File.sSystemFilename.Split(".")[1] + "" : null;
                        fFile.sFolderName = File.sPath;
                        fFile.IsNew = false;
                        fFile.IsNewTab = false;
                        fFile.IsCompleted = true;
                        fFile.IsDelete = false;
                        fFile.IsProgress = false;
                        fFile.sProgress = "100";
                        lstfile.Add(fFile);

                        result.arrFile = lstfile;
                    }
                    result.nPermission = arrStatusRequestorAction.Contains(objFormData.nStatusID) &&
                    nUserID == nEmployeeID ? (int)Backend.Enum.EnumEmployee.MenuPermission.Control : (int)Backend.Enum.EnumEmployee.MenuPermission.ReadOnly;

                    result.nStatus = objFormData.nStatusID;
                    int? nStepActive = 0;
                    if (objFormData != null)
                    {
                        switch (objFormData.nStatusID)
                        {
                            case 1:
                                nStepActive = null;
                                break;
                            case 2:     //Wait for Approve (HR)
                                nStepActive = objFormData.nStatusID != null ? objFormData.nStatusID - 1 : 0;
                                break;
                            case 3:     //Wait for Approve
                                nStepActive = objFormData.nStatusID != null ? objFormData.nStatusID - 1 : 0;
                                break;
                            case 99:     //Completed
                                nStepActive = 3;
                                break;
                            case 5:
                                nStepActive = 0;
                                break;
                            case 6:     //Reject
                                nStepActive = 0;
                                break;
                            case 7:     //Cancel
                                nStepActive = objFormData.nStatusID != null ? objFormData.nStatusID - 1 : 0;
                                break;
                        }
                    }
                    else
                    {
                        nStepActive = null;
                    }

                    result.nFocusStep = nStepActive;
                }

                result.arrApprover = await GetLineApproveLeave(nEmployeeID);
                result.isHr = nPositionID == 9;
                if (nPositionID == 9 && objFormData.nStatusID != 1)
                {
                    result.isRequestor = false;
                }
                else
                {
                    result.isRequestor = nUserID == nEmployeeID;
                }

                result.isLead = result.arrApprover.Any(a => a.sEmployeeID.DecryptParameter().ToInt() == nUserID && !a.isHr);
                int nYear = DateTime.Now.Year;
                TB_Employee? objEmp = await _db.TB_Employee.FirstOrDefaultAsync(f => f.nEmployeeID == nEmployeeID && f.IsActive && !f.IsDelete);
                if (objEmp == null)
                {
                    result.Message = "Data not found.";
                    return result;
                }

                result.arrHoliday = await _db.TB_HolidayDay.Where(w => w.IsActivity && !w.IsDelete && w.nYear == nYear).Select(s => s.dDate).ToArrayAsync();
                result.arrOption = await
                (from lt in _db.TB_LeaveType
                 join ls in _db.TB_LeaveSummary on lt.nLeaveTypeID equals ls.nLeaveTypeID
                 where lt.IsActive && !lt.IsDelete && !ls.IsDelete && ls.nEmployeeID == nEmployeeID && ls.nLeaveRemain > 0 && ls.nYear == nYear
                 orderby lt.nOrder
                 select new cSelectOptionleave
                 {
                     label = lt.LeaveTypeName,
                     value = lt.nLeaveTypeID.EncryptParameter()
                 }
                 ).ToArrayAsync();

                result.arrOptionStatus = await _db.TM_Status.Where(w => w.nRequestTypeID == 1
                ).OrderBy(o => o.nOrder).Select(s => new cSelectOptionleave
                {
                    label = s.sNextStatusName,
                    value = s.nStatusID.EncryptParameter()
                }).ToArrayAsync();

                result.arrLeaveTotal = await (from lt in _db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete)
                                              from lf in _db.TB_Image_Leave.Where(w => lt.nLeaveTypeID == w.nLeaveTypeID && !w.IsDelete).DefaultIfEmpty()
                                              from ls in _db.TB_LeaveSummary.Where(w => lt.nLeaveTypeID == w.nLeaveTypeID && !w.IsDelete && w.nYear == nYear && w.nEmployeeID == objEmp.nEmployeeID)
                                              orderby lt.nOrder
                                              select new LeaveTotalRequest
                                              {
                                                  dLeaveMax = ls != null ? ls.nQuantity : 0,
                                                  dLeaveTotal = ls != null ? ls.nLeaveUse : 0,
                                                  dLeaveRemain = ls != null ? ls.nLeaveRemain : 0,
                                                  sID = ls != null ? ls.nLeaveTypeID.EncryptParameter() : "",
                                                  sLeaveTypeName = lt.LeaveTypeName,
                                                  sFileLink = lf != null ? GetPathUploadFile(lf.sImageParh ?? "", lf.sSystemName ?? "") : ""
                                              }).ToArrayAsync();


            }
            catch (System.Exception e)
            {
                result.Message = e.Message;
                result.Status = StatusCodes.Status500InternalServerError;
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

        public async Task<cLeaveLogsRequest> GetLog(string sID)
        {
            cLeaveLogsRequest result = new cLeaveLogsRequest()
            {
                Status = StatusCodes.Status200OK
            };
            try
            {
                int nFormID = sID.DecryptParameter().ToInt();
                result.arrData = await (from log in _db.TB_Request_Leave_History
                                        join emp in _db.TB_Employee on log.nCreateBy equals emp.nEmployeeID
                                        join log_status in _db.TM_Status on log.nStatusID equals log_status.nStatusID
                                        join emp_po in _db.TB_Employee_Position on emp.nEmployeeID equals emp_po.nEmployeeID
                                        into emp_po
                                        from j_emp_po in emp_po.Where(w => !w.IsDelete).Take(1).DefaultIfEmpty()
                                        join po in _db.TB_Position on j_emp_po.nPositionID equals po.nPositionID
                                        join img_emp in _db.TB_Employee_Image on emp.nEmployeeID equals img_emp.nEmployeeID
                                        into img_emp
                                        from l_img_emp in img_emp.Where(w => !w.IsDelete && w.nImageType == 88).DefaultIfEmpty()
                                        where !log.IsDelete && !po.IsDelete && po.IsActive
                                        && log.nRequestLeaveID == nFormID && log_status.nRequestTypeID == 1
                                        orderby log.dUpdate descending
                                        select new LeaveLogsDataRequest
                                        {
                                            sActionDate = log.dCreate.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH),
                                            sActionTime = log.dCreate.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.HHmmss, ST.INFRA.Enum.CultureName.th_TH),
                                            sComment = log.sComment ?? "",
                                            sImgPath = l_img_emp != null ? l_img_emp.sSystemFileName ?? "" : "",
                                            sLogName = (emp.sNameTH ?? "") + " " + (emp.sSurnameTH ?? ""),
                                            sPosition = po.sPositionName,
                                            sStatus = log_status.sStatusName ?? "",
                                            nStatus = log.nStatusID
                                        }
                                        ).AsNoTracking().ToArrayAsync();
                var status = _db.TB_Request_Leave.FirstOrDefault(w => w.nRequestLeaveID == nFormID);

                if (status != null)
                {
                    result.nStatus = status.nStatusID;
                }





            }
            catch (System.Exception e)
            {
                result.Message = e.Message;
                result.Status = StatusCodes.Status500InternalServerError;
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

        public async Task<ResultAPI> ApproveOnTable(cFormReqRequest req)
        {
            ResultAPI result = new ResultAPI() { Status = StatusCodes.Status200OK };
            try
            {
                var UserAccount = _authen.GetUserAccount();
                int nEmployeeID = UserAccount.nUserID;
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
                    nCreateBy = objReqLeave.nCreateBy,
                    nEmployeeID = objReqLeave.nEmployeeID,
                    nLeaveTypeID = objReqLeave.nLeaveTypeID,
                    nRequestLeaveID = objReqLeave.nRequestLeaveID,
                    nStatusID = objReqLeave.nStatusID,
                    sReason = objReqLeave.sReason,
                    nRequestLeaveHisID = nReqLogID,
                    sComment = req.sComment
                };
                await _db.TB_Request_Leave_History.AddAsync(objLog);

                #region Send Line Noti

                string sGUID = Guid.NewGuid().ToString();

                var lstUserHR = (from a in _db.TB_Employee_Position
                                 join b in _db.TB_Employee on a.nEmployeeID equals b.nEmployeeID
                                 where a.nPositionID == 9 && b.IsActive && !b.IsDelete
                                 select new
                                 {
                                     nEmpID = b.nEmployeeID,
                                 }
                 ).ToList();
                var lstHR = string.Join(",", lstUserHR.Select(s => s.nEmpID)) + "";

                var arrLeaveType = _db.TB_LeaveType.ToArray();
                var arrStatus = _db.TM_Status.ToArray();

                cParamSendLine objParam = new cParamSendLine();
                objParam.sGUID = sGUID;
                objParam.nRequesterID = objReqLeave.nEmployeeID;
                objParam.sDate = objReqLeave.dUpdate.ToStringFromDate("dd/MM/yyyy");
                objParam.sTime = objReqLeave.dUpdate.ToStringFromDate("HH:mm") + " น.";

                objParam.sType = arrLeaveType.Where(w => w.nLeaveTypeID == objReqLeave.nLeaveTypeID).Select(s => s.LeaveTypeName).FirstOrDefault() ?? "";
                //Gen Time Text
                string sDate = "";
                if (objReqLeave.dStartDateTime.Date == objReqLeave.dEndDateTime.Date)
                {
                    sDate = "วันที่ : " + objReqLeave.dStartDateTime.ToStringFromDate("dd/MM/yyyy");
                }
                else
                {
                    sDate = "ตั้งแต่วันที่ : " + objReqLeave.dStartDateTime.ToStringFromDate("dd/MM/yyyy") + " ถึง " + objReqLeave.dEndDateTime.ToStringFromDate("dd/MM/yyyy");
                }
                string sTime = objReqLeave.dStartDateTime.ToStringFromDate("HH:mm") + " ถึง " + objReqLeave.dEndDateTime.ToStringFromDate("HH:mm") + " น.";

                objParam.sDateRequest = sDate;
                objParam.sTimeRequest = sTime;
                objParam.sDetailRequest = objReqLeave.sReason ?? "";
                objParam.sRemark = req.sComment ?? "";
                objParam.sStatus = arrStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Leave && w.nStatusID == objReqLeave.nStatusID).Select(s => s.sNextStatusName).FirstOrDefault() ?? "";


                objParam.sPathApprove = "HRApproveLeaveLine&sID=" + objReqLeave.nRequestLeaveID.EncryptParameter() + "&sUserID=" + objReqLeave.nCreateBy.EncryptParameter() + "&nStatusID=" + 2 + "&sGUID=" + sGUID;
                objParam.sPathReject = "HRRejectLeaveLine&sID=" + objReqLeave.nRequestLeaveID.EncryptParameter() + "&sUserID=" + objReqLeave.nCreateBy.EncryptParameter() + "&nStatusID=" + 5 + "&sGUID=" + sGUID;
                objParam.sPathCancel = "HRRejectLeaveLine&sID=" + objReqLeave.nRequestLeaveID.EncryptParameter() + "&sUserID=" + objReqLeave.nCreateBy.EncryptParameter() + "&nStatusID=" + 4 + "&sGUID=" + sGUID;
                objParam.sPathDetail = "DetailLeaveLine&sID=" + objReqLeave.nRequestLeaveID.EncryptParameter() + "&sUserID=" + objReqLeave.nCreateBy.EncryptParameter();

                if (req.nMode == 4 || req.nMode == 5) //Cancel,Reject ส่งหา Creator
                {
                    switch (req.nMode)
                    {
                        case 4: //Cancel
                            objParam.nTemplateID = 13;
                            objParam.lstEmpTo = new List<int> { objReqLeave.nCreateBy };
                            break;
                        case 5: //Reject
                            objParam.nTemplateID = 14;
                            objParam.lstEmpTo = new List<int> { objReqLeave.nCreateBy };
                            break;
                    }
                    STFunction.SendToLine(objParam);
                }
                else if (req.nMode == 2) //HR,Lead Approve
                {
                    int nUpperPositionID = _db.TB_Employee_Report_To.FirstOrDefault(f => !f.IsDelete && f.nEmployeeID == objReqLeave.nCreateBy)?.nRepEmployeeID ?? 0;

                    switch (objReqLeave.nStatusID)
                    {
                        case 3: //Waiting HR Approve
                            objParam.nTemplateID = 11;
                            objParam.lstEmpTo = new List<int> { nUpperPositionID };
                            break;
                        case 99:
                            objParam.nTemplateID = 12;
                            objParam.lstEmpTo = new List<int> { objReqLeave.nCreateBy };
                            break;
                    }

                    STFunction.SendToLine(objParam);
                }



                #endregion Send Line Noti 

                //Action From Line
                if (req.IsActionFromLine == true)
                {
                    TB_Log_WebhookLine? objLogLine = _db.TB_Log_WebhookLine.Where(w => w.sGUID == req.sGUID).FirstOrDefault();
                    if (objLogLine != null)
                    {
                        objLogLine.IsActionAlready = true;
                    }
                }

                if (req.nMode == 4)
                {
                    TB_LeaveSummary? objSummary = _db.TB_LeaveSummary.FirstOrDefault(f => f.nEmployeeID == objReqLeave.nEmployeeID && f.nLeaveTypeID == objReqLeave.nLeaveTypeID && !f.IsDelete);
                    if (objSummary == null)
                    {
                        result.Message = "Data Summary not found.";
                        return result;
                    }
                    else
                    {
                        objSummary.nLeaveUse = objSummary.nLeaveUse - objReqLeave.nLeaveUse;
                        objSummary.nLeaveRemain = objSummary.nLeaveRemain + objReqLeave.nLeaveUse;
                    }
                }


                await _db.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                result.Message = e.Message;
                result.Status = StatusCodes.Status500InternalServerError;
            }
            return result;
        }

        public async Task<ResultAPI> SaveData(cFormReqRequest req)
        {
            ResultAPI result = new ResultAPI() { Status = StatusCodes.Status200OK };
            try
            {
                bool isNew = false;
                var UserAccount = _authen.GetUserAccount();
                int nEmployeeID = 0;
                if (!string.IsNullOrEmpty(req.sEmployeeID))
                {
                    nEmployeeID = req.sEmployeeID.toInt();
                }
                else
                {
                    nEmployeeID = UserAccount.nUserID;
                }


                int? nLeaveReqID = req.sID.DecryptParameter().ToIntOrNull();
                TB_Request_Leave objReqLeave = _db.TB_Request_Leave.FirstOrDefault(f => f.nRequestLeaveID == nLeaveReqID) ?? new TB_Request_Leave();

                bool checkStartDate = _db.TB_Request_Leave.Any(a => a.nEmployeeID == nEmployeeID
                                    && !a.IsDelete && a.nStatusID != (int)Backend.Enum.EnumLeave.Status.Canceled &&
                                     a.dStartDateTime <= req.dStart.ToDateTime() && a.dEndDateTime >= req.dStart.ToDateTime()
                                     && a.nRequestLeaveID != nLeaveReqID);

                bool checkEndDate = _db.TB_Request_Leave.Any(a => a.nEmployeeID == nEmployeeID
                 && !a.IsDelete && a.nStatusID != (int)Backend.Enum.EnumLeave.Status.Canceled &&
                  a.dStartDateTime <= req.dEnd.ToDateTime() && a.dEndDateTime >= req.dEnd.ToDateTime()
                    && a.nRequestLeaveID != nLeaveReqID);

                int[] arrStatusSaveData = { (int)Backend.Enum.EnumLeave.SaveMode.SaveDraft, (int)Backend.Enum.EnumLeave.SaveMode.Submit };

                if ((checkStartDate || checkEndDate) && arrStatusSaveData.Contains(req.nMode))
                {
                    result.Message = "มีการลาในช่วงเวลานี้แล้ว";
                    return result;
                }

                if (objReqLeave.nRequestLeaveID == 0)
                {
                    if (nLeaveReqID != null)
                    {
                        result.Message = "Data not found.";
                        return result;
                    }
                    int nReqID = _db.TB_Request_Leave.Any() ? _db.TB_Request_Leave.Max(m => m.nRequestLeaveID) + 1 : 1;
                    objReqLeave.nRequestLeaveID = nReqID;
                    objReqLeave.dCreate = DateTime.Now;
                    objReqLeave.nCreateBy = nEmployeeID;
                    objReqLeave.nEmployeeID = nEmployeeID;
                    objReqLeave.nStatusID = (int)Backend.Enum.EnumLeave.Status.Draft;
                    isNew = true;
                    _db.TB_Request_Leave.Add(objReqLeave);
                }
                objReqLeave.dUpdate = DateTime.Now;
                objReqLeave.nUpdateBy = nEmployeeID;
                objReqLeave.nRequestTypeID = 1; // Leave Type = 1
                objReqLeave.nLeaveTypeID = req.sTypeID.DecryptParameter().ToInt();
                objReqLeave.sReason = req.sReason;
                objReqLeave.dStartDateTime = req.dStart.ToDateTime();
                objReqLeave.dEndDateTime = req.dEnd.ToDateTime();
                objReqLeave.IsEmergency = req.isEmergency;

                decimal nOldUse = !isNew ? objReqLeave.nLeaveUse : 0;
                int nOldStatus = objReqLeave.nStatusID;

                objReqLeave.nLeaveUse = req.nUse;

                TB_LeaveSummary? objSummary = _db.TB_LeaveSummary.FirstOrDefault(f => f.nEmployeeID == objReqLeave.nEmployeeID && f.nLeaveTypeID == objReqLeave.nLeaveTypeID && !f.IsDelete);
                if (objSummary == null)
                {
                    result.Message = "Data Summary not found.";
                    return result;
                }

                LeaveFlow(objReqLeave, req.nMode);



                if (isNew)
                {
                    objSummary.nLeaveUse = _calDaysLeave(objSummary.nLeaveUse, req.nUse, 1);
                    objSummary.dUpdate = DateTime.Now;
                    objSummary.nUpdateBy = nEmployeeID;
                    objSummary.nLeaveRemain = _calDaysLeave(objSummary.nLeaveRemain, req.nUse, 2);
                }
                else if (!isNew && arrStatusRequestorAction.Contains(nOldStatus) && req.nMode != (int)Backend.Enum.EnumLeave.SaveMode.Cancel)
                {
                    objSummary.nLeaveUse = _calDaysLeave(objSummary.nLeaveUse - nOldUse, req.nUse, 1);
                    objSummary.dUpdate = DateTime.Now;
                    objSummary.nUpdateBy = nEmployeeID;
                    objSummary.nLeaveRemain = _calDaysLeave(objSummary.nLeaveRemain + nOldUse, req.nUse, 2);
                }
                else if (objReqLeave.nStatusID == (int)Backend.Enum.EnumLeave.Status.Canceled)
                {
                    objSummary.nLeaveUse -= nOldUse;
                    objSummary.dUpdate = DateTime.Now;
                    objSummary.nUpdateBy = nEmployeeID;
                    objSummary.nLeaveRemain += nOldUse;
                }


                int nReqLogID = _db.TB_Request_Leave_History.Any() ? _db.TB_Request_Leave_History.Max(m => m.nRequestLeaveHisID) + 1 : 1;
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
                _db.TB_Request_Leave_History.Add(objLog);
                TB_Leave_File[] arrLeaveFile = { };
                if (nLeaveReqID != null)
                {
                    arrLeaveFile = _db.TB_Leave_File.Where(w => w.nRequestLeaveID == objReqLeave.nRequestLeaveID
                   && !w.IsDelete).ToArray();
                    foreach (var item in arrLeaveFile)
                    {
                        item.IsDelete = true;
                        item.nDeleteBy = nEmployeeID;
                        item.dDelete = DateTime.Now;
                    }
                }

                #region File
                var objAll = req.arrFile?.FirstOrDefault();
                if (objAll != null)
                {
                    string pathTempContent = !string.IsNullOrEmpty(objAll.sFolderName) ? objAll.sFolderName + "/" : "Temp/LeaveRequest/";

                    string truePathFile = "LeaveRequest\\" + objReqLeave.nRequestLeaveID + "\\";
                    string fPathContent = "LeaveRequest/" + objReqLeave.nRequestLeaveID;

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

                    int nFile = _db.TB_Leave_File.Any() ? _db.TB_Leave_File.Max(m => m.nLeaveFileID) + 1 : 1;
                    int nOrderFile = 1;
                    TB_Leave_File objInfofile = new TB_Leave_File()
                    {
                        nRequestLeaveID = objReqLeave.nRequestLeaveID,
                        nLeaveFileID = nFile,
                        sPath = fPathContent,
                        sSystemFilename = objAll.sSysFileName != null ? objAll.sSysFileName : "",
                        sFilename = objAll.sSysFileName != null ? objAll.sSysFileName : "",
                        nOrder = nOrderFile,
                        dCreate = DateTime.Now,
                        nCreateBy = 1,
                        dUpdate = DateTime.Now,
                        nUpdateBy = 1,
                        IsDelete = false,
                    };
                    nFile += 1;
                    _db.TB_Leave_File.Add(objInfofile);
                    nOrderFile += 1;
                }
                #endregion

                // int nOrderFile = 1;
                // int nFile = _db.TB_Leave_File.Any() ? _db.TB_Leave_File.Max(m => m.nLeaveFileID) + 1 : 1;
                // foreach (var item in req.arrFile)
                // {

                //     TB_Leave_File? objFile = arrLeaveFile.FirstOrDefault(f => f.nLeaveFileID == item.nID);
                //     if (objFile != null)
                //     {
                //         objFile.IsDelete = false;
                //         objFile.nOrder = nOrderFile;
                //     }
                //     else
                //     {

                //         string pathTempContent = !string.IsNullOrEmpty(item.sFolderName) ? item.sFolderName + "/" : "Temp/";
                //         string truePathFile = "LeaveRequest\\";
                //         STFunction.MoveFile(pathTempContent, truePathFile, item.sSysFileName != null ? item.sSysFileName : "", _env);

                //         TB_Leave_File objNewFile = new TB_Leave_File()
                //         {
                //             nUpdateBy = nEmployeeID,
                //             nCreateBy = nEmployeeID,
                //             dCreate = DateTime.Now,
                //             dUpdate = DateTime.Now,
                //             nOrder = nOrderFile,
                //             nRequestLeaveID = objReqLeave.nRequestLeaveID,
                //             sPath = item.sPath,
                //             sFilename = item.sFileName,
                //             sSystemFilename = item.sSysFileName,
                //             nLeaveFileID = nFile
                //         };
                //         nFile += 1;
                //         _db.TB_Leave_File.Add(objNewFile);
                //     }
                //     nOrderFile += 1;
                // }

                _db.SaveChanges();

                if (objReqLeave.nStatusID != 1) //ไม่ - Draft
                {
                    #region Send Line Noti
                    string sGUID = Guid.NewGuid().ToString();

                    var lstUserHR = (from a in _db.TB_Employee_Position
                                     join b in _db.TB_Employee on a.nEmployeeID equals b.nEmployeeID
                                     where a.nPositionID == 9 && b.IsActive && !b.IsDelete
                                     select new
                                     {
                                         nEmpID = b.nEmployeeID,
                                     }
                     ).ToList();
                    var arrLeaveType = _db.TB_LeaveType.ToArray();
                    var arrStatus = _db.TM_Status.ToArray();

                    cParamSendLine objParam = new cParamSendLine();
                    objParam.sGUID = sGUID;
                    objParam.nRequesterID = nEmployeeID;
                    objParam.sDate = objReqLeave.dUpdate.ToStringFromDate("dd/MM/yyyy");
                    objParam.sTime = objReqLeave.dUpdate.ToStringFromDate("HH:mm") + " น.";

                    objParam.sType = arrLeaveType.Where(w => w.nLeaveTypeID == objReqLeave.nLeaveTypeID).Select(s => s.LeaveTypeName).FirstOrDefault() ?? "";
                    //Gen Time Text
                    string sDate = "";
                    if (objReqLeave.dStartDateTime.Date == objReqLeave.dEndDateTime.Date)
                    {
                        sDate = "วันที่ : " + objReqLeave.dStartDateTime.ToStringFromDate("dd/MM/yyyy");
                    }
                    else
                    {
                        sDate = "ตั้งแต่วันที่ : " + objReqLeave.dStartDateTime.ToStringFromDate("dd/MM/yyyy") + " ถึง " + objReqLeave.dEndDateTime.ToStringFromDate("dd/MM/yyyy");
                    }
                    string sTime = objReqLeave.dStartDateTime.ToStringFromDate("HH:mm") + " ถึง " + objReqLeave.dEndDateTime.ToStringFromDate("HH:mm") + " น.";

                    objParam.sDateRequest = sDate;
                    objParam.sTimeRequest = sTime;
                    objParam.sDetailRequest = objReqLeave.sReason;
                    objParam.sStatus = arrStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Leave && w.nStatusID == objReqLeave.nStatusID).Select(s => s.sStatusName).FirstOrDefault() ?? "";

                    var lstHR = string.Join(",", lstUserHR.Select(s => s.nEmpID)) + "";

                    objParam.sPathApprove = "HRApproveLeaveLine&sID=" + objReqLeave.nRequestLeaveID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + 2 + "&sGUID=" + sGUID;
                    objParam.sPathReject = "HRRejectLeaveLine&sID=" + objReqLeave.nRequestLeaveID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + 5 + "&sGUID=" + sGUID;
                    objParam.sPathCancel = "HRRejectLeaveLine&sID=" + objReqLeave.nRequestLeaveID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + 4 + "&sGUID=" + sGUID;
                    objParam.sPathDetail = "DetailLeaveLine&sID=" + objReqLeave.nRequestLeaveID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter();
                    switch (objReqLeave.nStatusID)
                    {
                        case 2: //Waiting HR Approve
                            objParam.nTemplateID = 11;
                            objParam.lstEmpTo = lstUserHR.Select(s => s.nEmpID).ToList();
                            break;
                    }
                    STFunction.SendToLine(objParam);
                    #endregion Send Line Noti 

                }
            }
            catch (System.Exception e)
            {
                result.Message = e.Message;
                result.Status = StatusCodes.Status500InternalServerError;
            }
            return result;
        }

        public async Task<List<ApproverDataRequest>> GetLineApproveLeave(int nEmployeeID)
        {
            List<ApproverDataRequest> result = new List<ApproverDataRequest>();

            ApproverDataRequest? objHr = await (from emp in _db.TB_Employee
                                                join emp_po in _db.TB_Employee_Position on emp.nEmployeeID equals emp_po.nEmployeeID
                                                join po in _db.TB_Position on emp_po.nPositionID equals po.nPositionID
                                                where emp_po.nPositionID == 9 //HR
                                                && !emp.IsDelete && !emp_po.IsDelete && emp.IsActive && !po.IsDelete && po.IsActive
                                                select new ApproverDataRequest
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

            int nUpperPositionID = _db.TB_Employee_Report_To.FirstOrDefault(f => !f.IsDelete && f.nEmployeeID == nEmployeeID)?.nRepEmployeeID ?? 0;

            ApproverDataRequest? objApprover = await (from emp in _db.TB_Employee
                                                      join emp_po in _db.TB_Employee_Position on emp.nEmployeeID equals emp_po.nEmployeeID
                                                      join po in _db.TB_Position on emp_po.nPositionID equals po.nPositionID
                                                      join po_s in _db.TB_Position_Secondary on emp_po.nLevelPosition equals po_s.nPositionSecondaryID
                                                      where emp.nEmployeeID == nUpperPositionID
                                                      && !emp.IsDelete && !emp_po.IsDelete && emp.IsActive && !po.IsDelete && po.IsActive
                                                      select new ApproverDataRequest
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

        public async Task<cTabelWorkListRequest> GetDataWorkList(cReqTableWorkListRequest req)
        {
            cTabelWorkListRequest result = new cTabelWorkListRequest() { Status = StatusCodes.Status200OK };
            try
            {
                _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var UserAccount = _authen.GetUserAccount();

                int nEmployeeID = UserAccount.nUserID;
                string sPosition = UserAccount.sPosition;
                int nPositionID = 0;
                if (!string.IsNullOrEmpty(sPosition))
                {
                    var Position = _db.TB_Position.FirstOrDefault(f => f.sPositionName == sPosition);
                    if (Position != null)
                    {
                        nPositionID = Position.nPositionID;
                    }
                }

                var TB_Request_Leave = _db.TB_Request_Leave.Where(w => !w.IsDelete);
                List<int> arrEmployeeWaitApprove = new List<int>();
                if (req.isApproveListPage)
                {
                    if (nPositionID == 9)
                    {
                        arrEmployeeWaitApprove = _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive).Select(s => s.nEmployeeID).ToList();
                    }
                    else
                    {
                        arrEmployeeWaitApprove = _db.TB_Employee_Report_To.Where(w => !w.IsDelete && w.nRepEmployeeID == nEmployeeID).Select(s => s.nEmployeeID).ToList();
                        // arrEmployeeWaitApprove = _db.TB_Employee_Report_To.Where(w => !w.IsDelete && arrEmpID.Contains(w.nRepEmployeeID ?? 0)).Select(s => s.nEmployeeID).ToList();
                    }

                    TB_Request_Leave = TB_Request_Leave.Where(w => arrEmployeeWaitApprove.Contains(w.nEmployeeID) && w.nStatusID != 1);
                }
                else
                {
                    TB_Request_Leave = TB_Request_Leave.Where(w => w.nEmployeeID == nEmployeeID);
                    // TB_Request_Leave = TB_Request_Leave.Where(w => arrEmpID.Contains(w.nEmployeeID) ? arrEmpID.Contains(w.nEmployeeID) : w.nEmployeeID == nEmployeeID);
                }

                var TM_Status = _db.TM_Status.Where(w => w.nRequestTypeID == 1);
                var TB_LeaveType = _db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete);
                var TB_Employee = _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete);
                var TB_Employee_Report_To = _db.TB_Employee_Report_To.Where(w => !w.IsDelete && w.nRepEmployeeID == nEmployeeID);

                var qry = (from r in TB_Request_Leave
                           join e in TB_Employee on r.nEmployeeID equals e.nEmployeeID
                           join s in TM_Status on r.nStatusID equals s.nStatusID
                           join t in TB_LeaveType on r.nLeaveTypeID equals t.nLeaveTypeID
                           join re in TB_Employee_Report_To on e.nEmployeeID equals re.nEmployeeID into gj
                           from re in gj.DefaultIfEmpty()
                           select new { TB_Request_Leave = r, TM_Status = s, TB_LeaveType = t, TB_Employee = e, TB_Employee_Report_To = re }
                          );

                // if (req.arrEmpID.Length > 0 && req.isApproveListPage)
                // {
                //     int[] arrEmpID = req.arrEmpID.Select(s => s.DecryptParameter().ToInt()).ToArray();
                //     qry = qry.Where(w => arrEmpID.Contains(w.TB_Employee.nEmployeeID));
                // }
                if (req.arrEmpID.Length > 0)
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

                var ListQry = qry.ToList();
                var qrySelector = ListQry.Select(s => new TableDataWorkListRequest
                {
                    sID = s.TB_Request_Leave.nRequestLeaveID.EncryptParameter(),
                    sCreateDate = s.TB_Request_Leave.dCreate.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH),
                    dCreateDate = s.TB_Request_Leave.dCreate,
                    dUpdate = s.TB_Request_Leave.dUpdate,
                    sFullName = (s.TB_Employee.sNameTH ?? "") + " " + (s.TB_Employee.sSurnameTH ?? ""),
                    sType = s.TB_LeaveType.LeaveTypeName,
                    nStatus = s.TB_Request_Leave.nStatusID,
                    sStatus = s.TM_Status.sNextStatusName ?? "",
                    sStartLeave = s.TB_Request_Leave.dStartDateTime.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmm, ST.INFRA.Enum.CultureName.th_TH),
                    dStartLeave = s.TB_Request_Leave.dStartDateTime,
                    sEndLeave = s.TB_Request_Leave.dEndDateTime.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmm, ST.INFRA.Enum.CultureName.th_TH),
                    dEndLeave = s.TB_Request_Leave.dEndDateTime,
                    sLeaveUse = s.TB_Request_Leave.nLeaveUse >= 1 ? s.TB_Request_Leave.nLeaveUse.ToString() + " วัน" : (s.TB_Request_Leave.nLeaveUse * 10).ToString() + " ชั่วโมง",
                    isEnableEdit = !req.isApproveListPage && arrStatusRequestorAction.Contains(s.TB_Request_Leave.nStatusID),
                    isEnableApprove = req.isApproveListPage && (
                                        (nPositionID == 9 && s.TB_Request_Leave.nStatusID == (int)Backend.Enum.EnumLeave.Status.Submit) // Hr
                                        ||
                                        (s.TB_Request_Leave.nStatusID == (int)Backend.Enum.EnumLeave.Status.Approved &&
                                        s.TB_Employee_Report_To != null && s.TB_Employee_Report_To.nRepEmployeeID == nEmployeeID)),
                    isDelete = s.TB_Request_Leave.nStatusID == 1 ? true : false

                }).ToList();

                #region//SORT
                string sSortColumn = req.sSortExpression;
                switch (req.sSortExpression)
                {
                    case "sCreateDate": sSortColumn = "dCreateDate"; break;
                    case "dUpdate": sSortColumn = "dUpdate"; break;
                    case "sFullName": sSortColumn = "sFullName"; break;
                    case "sType": sSortColumn = "sType"; break;
                    case "nStatusID": sSortColumn = "nStatusID"; break;
                    case "sStartLeave": sSortColumn = "dStartLeave"; break;
                    case "sEndLeave": sSortColumn = "dEndLeave"; break;
                    case "sLeaveUse": sSortColumn = "sLeaveUse"; break;
                }
                if (req.isASC)
                {
                    qrySelector = qrySelector.OrderBy<TableDataWorkListRequest>(sSortColumn).ToList();
                }
                else if (req.isDESC)
                {
                    qrySelector = qrySelector.OrderByDescending<TableDataWorkListRequest>(sSortColumn).ToList();
                }
                #endregion


                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(req.nPageSize, req.nPageIndex, qrySelector.Count());
                result.arrData = qrySelector.Skip(dataPage.nSkip).Take(dataPage.nTake).ToArray();
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
        public cResultTableLeaveRequest GetMasterData()
        {
            cResultTableLeaveRequest result = new cResultTableLeaveRequest();
            try
            {
                DateTime dNow = DateTime.Now;
                cSelectRequest[] lstEmployee = _db.TM_Data.Where(w => w.nDatatypeID == 7 && !w.IsDelete).Select(s => new cSelectRequest
                {
                    value = s.nData_ID.ToString(),
                    label = s.sNameTH
                }).AsNoTracking().ToArray();

                result.lstSelectEmpType = lstEmployee;
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        public cReturnImportExcelRequest LeaveImportExcel(cLeaveImportExcelRequest req)
        {
            cReturnImportExcelRequest result = new cReturnImportExcelRequest();
            try
            {
                List<cObjectImportExcelRequest>? lstData = new List<cObjectImportExcelRequest>();

                if (req.sFileName != null && req.sFileName.Substring(0, 4) != "TOR_")
                {
                    result.Status = StatusCodes.Status200OK;
                    result.Message = "ไฟล์แนบไม่ตรงกับข้อมูลในระบบ";
                }
                else
                {
                    string path = "Imprt/Leave";
                    if (Directory.Exists(STFunction.MapPath(path, _env)))
                    {
                        Directory.CreateDirectory(STFunction.MapPath(path, _env));
                    }

                    if (File.Exists(STFunction.MapPath(req.sPath + "/" + req.sSysFileName, _env)) && !string.IsNullOrEmpty(req.sSysFileName))
                    {
                        STFunction.MoveFile(req.sPath + "/" + "", path + "/" + "", req.sSysFileName + "", _env);
                    }
                    string sMapPathFileCurrent = STFunction.MapPath(path + "/" + req.sSysFileName, _env);
                    if (File.Exists(sMapPathFileCurrent))
                    {
                        using (var stream = new FileStream(sMapPathFileCurrent, FileMode.Open, FileAccess.Read))
                        {
                            IExcelDataReader xlsRd = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            DataSet ds = xlsRd.AsDataSet();
                            var sheet = "data";
                            DataTable? dt = ds.Tables[sheet];
                            string[] arrColumnHeader = new string[] { "Code", "Detail" };
                            List<string> lstInvalidColumn = new List<string>();
                            if (dt != null)
                            {
                                int index = 1;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = dt.Rows[i];
                                    if (i == 0) //Check Header Column
                                    {
                                        for (int j = 0; j < arrColumnHeader.Length; j++)
                                        {
                                            var a = dr[j] + "";
                                            if ((dr[j] + "").Trim() != arrColumnHeader[j].Trim())
                                            {
                                                lstInvalidColumn.Add(dr[j] + "");
                                            }
                                        }

                                        if (lstInvalidColumn.Any())
                                        {
                                            string sInvalid = string.Join(", ", lstInvalidColumn);
                                            result.Status = StatusCodes.Status200OK;
                                            result.Message = "ชื่อคอลัมธ์ไม่ตรงกับเทมเพลต";
                                            // result.Message = "ชื่อคอลัมธ์ไม่ตรงกับเทมเพลต " + sInvalid;
                                            return result;
                                        }
                                    }
                                    else
                                    {
                                        int RowId = i + 1;

                                        if (!string.IsNullOrEmpty(dr[0] + ""))  //Check data Column
                                        {
                                            // string sMessageNotPass = "";
                                            bool IsPassValidateData = true;

                                            // check Data input
                                            string? sCode = (dr[0] + "").Trim();
                                            string? sDetail = (dr[1] + "").Trim();

                                            string? sID = (dr[19] + "").Trim();


                                            bool IsEmpty = string.IsNullOrEmpty(sCode);
                                            if (!IsEmpty)
                                            {
                                                cObjectImportExcelRequest objData = new cObjectImportExcelRequest();
                                                objData.sID = sID;
                                                objData.sCode = sCode != null ? sCode : "";
                                                objData.sDetail = sDetail != null ? sDetail : null;

                                                lstData.Add(objData);
                                                index++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                result.lstData = lstData;
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        public cExportExcelRequest LeaveExportExcel(cLeaveExportRequest req)
        {
            cExportExcelRequest result = new cExportExcelRequest();
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
                                     ws.Cell(nRow, nCol).Value = f.sLeaveUse;
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
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        public cResultTableLeaveRequest EditLeaveSummary(cEditLeaveRequest req)
        {
            cResultTableLeaveRequest result = new cResultTableLeaveRequest();
            try
            {
                DateTime dNow = DateTime.Now;
                int nYear = dNow.Year;

                req.lstLeaveDate.ForEach(f =>
                {
                    f.lstLeaveDetail.ForEach(item =>
                    {
                        TB_LeaveSummary objLeaveSummary = _db.TB_LeaveSummary.FirstOrDefault(w => w.nEmployeeID == item.nEmployeeID && w.nLeaveTypeID == item.nLeaveTypeID && w.nYear == nYear);
                        if (objLeaveSummary == null)
                        {
                            objLeaveSummary = new TB_LeaveSummary();
                            objLeaveSummary.nLeaveTypeID = (_db.TB_LeaveSummary.Any() ? _db.TB_LeaveSummary.Max(m => m.nLeaveTypeID) : 0) + 1;
                            objLeaveSummary.dCreate = DateTime.Now;
                            objLeaveSummary.nYear = nYear;
                            _db.TB_LeaveSummary.Add(objLeaveSummary);
                        }
                        objLeaveSummary.nLeaveUse = item.sLeaveData.ToDecimal() > 0 ? item.sLeaveData.ToDecimal() : objLeaveSummary.nLeaveUse;
                        _db.TB_LeaveSummary.Update(objLeaveSummary);
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

        public ResultAPI RemoveDataTable(cRemoveTable param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                if (param.lstID != null)
                {
                    List<int> lstID = param.lstID.ConvertAll(item => item.DecryptParameter().ToInt()).ToList();
                    var lstData = _db.TB_Request_Leave.Where(w => lstID.Contains(w.nRequestLeaveID)).ToList();

                    foreach (var item in lstData)
                    {
                        item.IsDelete = true;
                        item.nDeleteBy = nUserID;
                        item.dDelete = DateTime.Now;
                    }
                    _db.SaveChanges();
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

    }
}