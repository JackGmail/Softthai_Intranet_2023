using ST.INFRA;
using ST.INFRA.Common;
using Extensions.Common.STExtension;
using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Backend.Models;
using Backend.Enum;
using Backend.Interfaces;
using Backend.Models.Authentication;
using ResultAPI = Extensions.Common.STResultAPI.ResultAPI;
using ST_API.Models;
using Extensions.Common.STFunction;
using System.Data.Common;
using ClosedXML.Excel;

namespace Backend.Services.ISystemService
{
    public class OTService : IOTService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _authen;

        public OTService(ST_IntranetEntity db, IAuthentication authen)
        {
            _db = db;
            _authen = authen;
        }
        public cInitData LoadDataInit()
        {
            ParamJWT UserAccount = _authen.GetUserAccount();
            int nUserID = UserAccount.nUserID;

            cInitData result = new cInitData();

            result.lstProject = _db.TB_Project.Where(w => !w.IsDelete).Select(s => new cSelectOption
            {
                value = s.nProjectID + "",
                label = !string.IsNullOrEmpty(s.sProjectAbbr) ? s.sProjectAbbr + " : " + s.sProjectName : s.sProjectName
            }).ToArray();

            result.lstApproveByCreate = _db.TB_Employee.Where(w => w.IsActive && w.nEmployeeID != nUserID && !w.IsDelete).Select(s => new cSelectOption
            {
                value = s.nEmployeeID + "",
                label = $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})"
            }).ToArray();

            result.lstApproveBy = _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete).Select(s => new cSelectOption
            {
                value = s.nEmployeeID + "",
                label = $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})"
            }).ToArray();

            result.lstCondition = _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.ReasonType).OrderBy(o => o.nOrder).Select(s => new cSelectOption
            {
                value = s.nData_ID + "",
                label = s.sNameTH + ""
            }).ToArray();

            result.sRequester = _db.TB_Employee.Where(w => w.nEmployeeID == nUserID).Select(s => $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})").FirstOrDefault();

            result.Status = StatusCodes.Status200OK;
            result.Message = "";

            return result;
        }
        public cInitData GetProjectTask(cReqDataOT param)
        {
            cInitData result = new cInitData();

            if (!string.IsNullOrEmpty(param.sProjectID))
            {
                int nProjectID = param.sProjectID.toInt();

                //List Project
                int[]? arrProcessID = _db.TB_Project_Process.Where(w => w.nProjectID == nProjectID && w.IsActive && !w.IsDelete).Select(s => s.nMasterProcessID).ToArray();
                // result.lstProcess = _db.TB_MasterProcess.Where(w => w.IsActive && !w.IsDelete && arrProcessID.Contains(w.nMasterProcessID) && w.nMasterProcessTypeID == (int)EnumGlobal.MasterDataType.ProcessType).OrderBy(o => o.nOrder).Select(s => new cSelectOption
                // {
                //     value = s.nMasterProcessID + "",
                //     label = s.sMasterProcessName
                // }).ToArray();
                var lstProcess = new List<cSelectOption>();
                lstProcess.Add(new cSelectOption
                {
                    value = 1 + "",
                    label = "อยู่ระหว่างพัฒนา"
                });

                //List Task
                var lstTask = new List<cSelectOption>();
                lstTask.Add(new cSelectOption
                {
                    value = 1 + "",
                    label = "อยู่ระหว่างพัฒนา"
                });

                // result.lstProjectTask = _db.TB_Task.Where(w => w.nProjectID == nProjectID && !w.IsDelete).Select(s => new cSelectOption
                // {
                //     value = s.nTaskID + "",
                //     label = s.sDescription
                // }).ToArray();

                result.lstProcess = lstProcess.ToArray();
                result.lstProjectTask = lstTask.ToArray();
            }

            result.Status = StatusCodes.Status200OK;
            result.Message = "";

            return result;
        }
        public cInitData GetReason(cReqDataOT param)
        {
            cInitData result = new cInitData();

            if (!string.IsNullOrEmpty(param.sID))
            {
                int nRequestID = param.sID.DecryptParameter().toInt();

                //List Condition
                var lstCondition = _db.TB_Request_OT_Reason.Where(w => w.nRequestOTID == nRequestID && !w.IsDelete).ToList();
                var lstReasonAll = _db.TM_Data.Where(w => w.nDatatypeID == 2).ToList();
                var lstConditionResult = new List<string>();
                foreach (var iC in lstCondition)
                {
                    if (iC.nReasonID == 7)
                    {
                        lstConditionResult.Add(" - " + iC.sOther);
                    }
                    else
                    {
                        lstConditionResult.Add(" - " + lstReasonAll.Where(w => w.nData_ID == iC.nReasonID).Select(s => s.sNameTH).FirstOrDefault() + "");
                    }
                }
                result.lstConditionList = lstConditionResult;
            }

            result.Status = StatusCodes.Status200OK;
            result.Message = "";

            return result;
        }
        /// <summary>
        /// Get ข้อมูลของ Form
        /// </summary>
        public cOTData GetDataOT(cReqDataOT param)
        {
            cOTData result = new cOTData();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                int nID = param.sID.DecryptParameter().ToInt();
                var objData = _db.TB_Request_OT.FirstOrDefault(f => f.nRequestOTID == nID);
                if (objData != null)
                {
                    result.sProjectID = objData.nProjectID + "";
                    result.sProcessID = objData.nProcessID + "";
                    result.sApproveBy = objData.nApproveBy + "";
                    result.sRequester = _db.TB_Employee.Where(w => w.nEmployeeID == objData.nCreateBy).Select(s => $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})").FirstOrDefault();
                    result.sTopic = objData.sTopic + "";
                    result.dPlanDateTime = objData.dPlanDateTime;
                    result.nEstimateHour = objData.nEstimateHour;

                    //List Condition
                    result.lstCondition = _db.TB_Request_OT_Reason.Where(w => w.nRequestOTID == nID && !w.IsDelete).Select(s => s.nReasonID + "").ToList();
                    result.sOtherRemark = _db.TB_Request_OT_Reason.Where(w => w.nRequestOTID == nID && !w.IsDelete && w.nReasonID == 7).Select(s => s.sOther).FirstOrDefault();

                    result.lstProjectTask = _db.TB_Project_Plan.Where(w => w.nProjectID == objData.nProjectID).Select(s => new cSelectOption
                    {
                        value = s.nPlanID + "",
                        label = s.sDetail
                    }).ToArray();

                    //List Task
                    var lstTask = _db.TB_Request_OT_Task.Where(w => w.nRequestOTID == nID && !w.IsDelete).Select(s => new cTaskDetail
                    {
                        nID = s.nRequestTaskID,
                        sTaskID = s.nProjectTaskID + "",
                        sTaskDesc = s.sDescription + "",
                        IsTaskCompleted = s.IsTaskCompleted,
                        nOTHourResult = s.nOTHour,
                        sReasonResult = s.sReason,
                    }).ToList();

                    result.lstTask = lstTask;
                    result.nStatusID = objData.nStatusID;
                    result.IsApprover = objData.nApproveBy == nUserID;
                    result.IsRequester = objData.nCreateBy == nUserID;
                    result.sComment = objData.sNoteApprover;

                    result.sApprovedBy = _db.TB_Employee.Where(f => f.nEmployeeID == objData.nApproveBy).Select(s => $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})").FirstOrDefault();
                    result.sApprovedDate = objData.dUpdate.ToStringFromDate("dd/MM/yyyy HH:mm", "th-TH");

                    //Result
                    result.dActionStartResult = objData.dStartActionDateTime;
                    result.dActionEndResult = objData.dEndActionDateTime;

                    //History Log
                    var lstEmployee = _db.TB_Employee.ToArray();

                    var lstLogAll = _db.TB_Request_OT_History.Where(w => w.nRequestOTID == nID).ToList();
                    var lstLog = new List<cHistoryLog>();
                    var lstStatus = _db.TM_Status.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.OT).ToArray();
                    var lstEmpPosition = _db.TB_Employee_Position.ToArray();
                    var lstPosition = _db.TB_Position.ToArray();
                    var lstImage = _db.TB_Employee_Image.ToArray();

                    foreach (var item in lstLogAll)
                    {
                        var objEmp = lstEmployee.Where(w => w.nEmployeeID == item.nUpdateBy).FirstOrDefault();
                        string sPathImg = "";
                        if (objEmp != null)
                        {
                            var objImg = lstImage.Where(w => w.nEmployeeID == item.nUpdateBy).FirstOrDefault();
                            var sPosition = (from a in lstEmpPosition
                                             join b in lstPosition on a.nPositionID equals b.nPositionID
                                             where a.nEmployeeID == objEmp.nEmployeeID
                                             select b.sPositionName).FirstOrDefault();
                            if (objImg != null)
                            {
                                sPathImg = STFunction.GetPathUploadFile(objImg.sPath ?? "", objImg.sSystemFileName ?? "") ?? "";
                            }

                            lstLog.Add(new cHistoryLog
                            {
                                sName = $"{objEmp.sNameTH} {objEmp.sSurnameTH} ({objEmp.sNickname})",
                                sPosition = sPosition ?? "-",
                                sComment = item.sNoteApprover ?? "-",
                                sPathImg = sPathImg ?? "",
                                sStatus = lstStatus.Where(w => w.nStatusID == item.nStatusID).Select(s => s.sStatusName).FirstOrDefault() + "",
                                sActionMode = item.nStatusID == 0 ? "Cancel" : item.nStatusID == 2 ? "Submit" : item.nStatusID == 4 ? "Approve" : item.nStatusID == 5 ? "Save Result" : lstStatus.Where(w => w.nStatusID == item.nStatusID).Select(s => s.sStatusName).FirstOrDefault() + "",
                                sActionDate = item.dUpdate.ToStringFromDate("dd/MM/yyyy , HH:mm:ss", "th-TH"),
                            });
                        }
                    }

                    result.lstHistoryLog = lstLog;
                }

                result.Status = StatusCodes.Status200OK;
            }
            catch (System.Exception ex)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// Save ข้อมูลของ Form
        /// </summary>
        public ResultAPI SaveData(cOTData param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                TB_Request_OT? objOT = _db.TB_Request_OT.Where(w => w.nRequestOTID == param.sID.DecryptParameter().ToInt()).FirstOrDefault();



                //Action From Line
                if (param.IsActionFromLine == true && objOT != null)
                {
                    TB_Log_WebhookLine? objLog = _db.TB_Log_WebhookLine.Where(w => w.sGUID == param.sGUID).FirstOrDefault();
                    if (objLog != null)
                    {
                        objOT.dUpdate = DateTime.Now;
                        objOT.nUpdateBy = nUserID;
                        objOT.nStatusID = param?.nStatusID ?? 1;
                        objLog.IsActionAlready = true;
                        _db.SaveChanges();
                    }
                }
                else //Action From System
                {
                    bool isNew = false;
                    if (objOT == null)
                    {
                        objOT = new TB_Request_OT();
                        isNew = true;
                    }

                    objOT.nProjectID = (param.sProjectID ?? "0").ToInt();
                    objOT.nProcessID = (param.sProcessID ?? "0").ToInt();
                    objOT.nApproveBy = (param.sApproveBy ?? "0").ToInt();
                    objOT.sTopic = param?.sTopic ?? "";
                    objOT.dPlanDateTime = param?.dPlanDateTime ?? DateTime.Now;
                    objOT.nEstimateHour = param?.nEstimateHour ?? 0;
                    objOT.dUpdate = DateTime.Now;
                    objOT.nUpdateBy = nUserID;
                    objOT.nStatusID = param?.nStatusID ?? 1;
                    objOT.sNoteApprover = "";

                    //New
                    if (isNew)
                    {
                        var isDuplicate = _db.TB_Request_OT.Where(w => w.dPlanDateTime.Date == objOT.dPlanDateTime.Date && w.nCreateBy == nUserID && !w.IsDelete
                                            && w.nStatusID != (int)EnumGlobal.StatusRequestOT.Canceled && w.nStatusID != (int)EnumGlobal.StatusRequestOT.Draft && w.nProjectID == objOT.nProjectID
                                            && w.nProcessID == objOT.nProcessID).Any();
                        if (isDuplicate)
                        {
                            result.Status = StatusCodes.Status208AlreadyReported;
                            result.Message = "มีการขอ OT ในวันที่เลือกแล้ว";
                            return result;
                        }
                        int newID = _db.TB_Request_OT.Any() ? _db.TB_Request_OT.Select(s => s.nRequestOTID).Max() + 1 : 1;

                        objOT.nRequestOTID = newID;
                        objOT.nRequestTypeID = (int)EnumGlobal.RequestFormType.OT;
                        objOT.dCreate = DateTime.Now;
                        objOT.nCreateBy = nUserID;
                        objOT.IsDelete = false;

                        _db.TB_Request_OT.Add(objOT);
                    }
                    //Edit
                    else
                    {
                        if (param?.nStatusID == (int)EnumGlobal.StatusRequestOT.WaitingApprove)
                        {
                            var isDuplicate = _db.TB_Request_OT.Where(w => w.dPlanDateTime.Date == objOT.dPlanDateTime.Date && w.nCreateBy == nUserID && !w.IsDelete
                                                                       && w.nStatusID != (int)EnumGlobal.StatusRequestOT.Canceled && w.nStatusID != (int)EnumGlobal.StatusRequestOT.Draft && w.nProjectID == objOT.nProjectID
                                                                       && w.nProcessID == objOT.nProcessID && w.nRequestOTID != objOT.nRequestOTID).Any();
                            if (isDuplicate)
                            {
                                result.Status = StatusCodes.Status208AlreadyReported;
                                result.Message = "มีการขอ OT ในวันที่เลือกแล้ว";
                                return result;
                            }
                        }
                    }


                    //Check วันหยุด
                    bool isHoliday = false;
                    bool isHolidayComp = _db.TB_HolidayDay.Where(w => w.dDate.Date == objOT.dPlanDateTime && !w.IsDelete && w.nYear == objOT.dPlanDateTime.Year).Any();
                    if (isHolidayComp || objOT.dPlanDateTime.DayOfWeek == DayOfWeek.Saturday || objOT.dPlanDateTime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        isHoliday = true;
                    }
                    objOT.IsHoliday = isHoliday;

                    //Add Condition
                    var lstCon = _db.TB_Request_OT_Reason.Where(w => w.nRequestOTID == objOT.nRequestOTID).ToList();
                    int newConID = _db.TB_Request_OT_Reason.Any() ? _db.TB_Request_OT_Reason.Select(s => s.nRequestOTReasonID).Max() + 1 : 1;

                    //Set Delete Record ที่ไม่ได้ส่ง Param มา
                    foreach (var iL in lstCon.Where(w => !w.IsDelete).ToList())
                    {
                        if (param != null && param.lstCondition != null)
                        {
                            if (!param.lstCondition.Contains(iL.nReasonID + ""))
                            {
                                iL.IsDelete = true;
                                iL.dDelete = DateTime.Now;
                                iL.nDeleteBy = nUserID;
                            }
                        }
                    }
                    if (param != null && param.lstCondition != null)
                    {
                        //Add and Update
                        foreach (var iC in param.lstCondition)
                        {
                            //ถ้ายังไม่มีใน Table ให้เพิ่มใหม่
                            if (!lstCon.Select(s => s.nReasonID).Contains(iC.ToInt()))
                            {
                                _db.TB_Request_OT_Reason.Add(new TB_Request_OT_Reason
                                {
                                    nRequestOTReasonID = newConID,
                                    nRequestOTID = objOT.nRequestOTID,
                                    nReasonID = iC.ToInt(),
                                    sOther = iC.ToInt() == 7 ? param.sOtherRemark + "" : null,
                                    dCreate = DateTime.Now,
                                    nCreateBy = nUserID,
                                    dUpdate = DateTime.Now,
                                    nUpdateBy = nUserID,
                                    IsDelete = false,
                                });

                                newConID++;
                            }
                            //ถ้ามีใน Table ให้ Update
                            else
                            {
                                var objCon = lstCon.Where(w => w.nReasonID == iC.ToInt()).FirstOrDefault();
                                if (objCon != null)
                                {
                                    objCon.sOther = iC.ToInt() == 7 ? param.sOtherRemark + "" : null;
                                    objCon.dUpdate = DateTime.Now;
                                    objCon.nUpdateBy = nUserID;
                                    objCon.dDelete = null;
                                    objCon.nDeleteBy = null;
                                    objCon.IsDelete = false;
                                }
                            }
                        }

                    }

                    //Add Task
                    var lstTask = _db.TB_Request_OT_Task.Where(w => w.nRequestOTID == objOT.nRequestOTID).ToList();
                    int newTaskID = _db.TB_Request_OT_Task.Any() ? _db.TB_Request_OT_Task.Select(s => s.nRequestTaskID).Max() + 1 : 1;

                    //Set Delete Record ที่ไม่ได้ส่ง Param มา
                    foreach (var iL in lstTask.Where(w => !w.IsDelete).ToList())
                    {
                        if (param != null && param.lstTask != null)
                        {
                            if (!param.lstTask.Select(s => s.nID).Contains(iL.nRequestTaskID))
                            {
                                iL.IsDelete = true;
                                iL.dDelete = DateTime.Now;
                                iL.nDeleteBy = nUserID;
                            }
                        }
                    }
                    if (param != null && param.lstTask != null)
                    {
                        //Add and Update
                        foreach (var iT in param.lstTask)
                        {
                            //ถ้ายังไม่มีใน Table ให้เพิ่มใหม่
                            if (!lstTask.Select(s => s.nRequestTaskID).ToList().Contains(iT.nID ?? 0))
                            {
                                _db.TB_Request_OT_Task.Add(new TB_Request_OT_Task
                                {
                                    nRequestTaskID = newTaskID,
                                    nRequestOTID = objOT.nRequestOTID,
                                    nProjectTaskID = (iT.sTaskID ?? "").ToInt(),
                                    sDescription = iT.sTaskDesc + "",
                                    dCreate = DateTime.Now,
                                    nCreateBy = nUserID,
                                    dUpdate = DateTime.Now,
                                    nUpdateBy = nUserID,
                                    IsDelete = false,
                                });
                                newTaskID++;
                            }
                            //ถ้ามีใน Table ให้ Update
                            else
                            {
                                var objTask = lstTask.Where(w => w.nRequestTaskID == iT.nID).FirstOrDefault();
                                if (objTask != null)
                                {
                                    objTask.nProjectTaskID = (iT.sTaskID ?? "").ToInt();
                                    objTask.sDescription = iT.sTaskDesc + "";
                                    objTask.dUpdate = DateTime.Now;
                                    objTask.nUpdateBy = nUserID;
                                    objTask.IsDelete = false;
                                    objTask.dDelete = null;
                                    objTask.nDeleteBy = null;
                                }
                            }
                        }

                    }
                    _db.SaveChanges();
                }

                SaveLog(objOT);

                if (objOT.nStatusID != (int)EnumGlobal.StatusRequestOT.Draft)
                {
                    #region Send Line Noti
                    string sGUID = Guid.NewGuid().ToString();

                    var arrProject = _db.TB_Project.ToArray();
                    var arrStatus = _db.TM_Status.ToArray();

                    cParamSendLine objParam = new cParamSendLine();
                    objParam.sGUID = sGUID;
                    objParam.nRequesterID = nUserID;
                    objParam.sDate = objOT.dUpdate.ToStringFromDate("dd/MM/yyyy");
                    objParam.sTime = objOT.dUpdate.ToStringFromDate("HH:mm") + " น.";

                    objParam.sProject = arrProject.Where(w => w.nProjectID == objOT.nProjectID).Select(s => s.sProjectName).FirstOrDefault() ?? "";
                    objParam.sDetailRequest = objOT.sTopic ?? "";
                    objParam.sStartDate = objOT.dPlanDateTime.ToStringFromDate("dd/MM/yyyy");
                    objParam.sHours = ((objOT.nEstimateHour + "") ?? "-") + " ชม.";
                    objParam.sStatus = arrStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.OT && w.nStatusID == objOT.nStatusID).Select(s => s.sStatusName).FirstOrDefault() ?? "";
                    objParam.sPathApprove = "ApproveOTLine&sID=" + objOT.nRequestOTID.EncryptParameter() + "&sUserID=" + objOT.nApproveBy.EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestOT.WaitingResult) + "&sGUID=" + sGUID;
                    objParam.sPathReject = "RejectOTLine&sID=" + objOT.nRequestOTID.EncryptParameter() + "&sUserID=" + objOT.nApproveBy.EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestOT.Reject) + "&sGUID=" + sGUID;
                    objParam.sPathCancel = "RejectOTLine&sID=" + objOT.nRequestOTID.EncryptParameter() + "&sUserID=" + objOT.nApproveBy.EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestOT.Canceled) + "&sGUID=" + sGUID;
                    objParam.sPathDetail = "DetailOTLine&sID=" + objOT.nRequestOTID.EncryptParameter() + "&sUserID=" + objOT.nApproveBy.EncryptParameter();
                    switch (objOT.nStatusID)
                    {
                        case 2: //Waiting Approve
                            objParam.nTemplateID = 15;
                            objParam.lstEmpTo = new List<int> { objOT.nApproveBy };
                            break;
                        case 4: //Waiting Result
                            objParam.nTemplateID = 16;
                            objParam.lstEmpTo = new List<int> { objOT.nCreateBy };
                            break;
                    }
                    STFunction.SendToLine(objParam);
                    #endregion Send Line Noti 

                }

                result.Status = StatusCodes.Status200OK;
            }
            catch (System.Exception ex)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// Save ข้อมูลผลการทำ OT
        /// </summary>
        public ResultAPI SaveResult(cOTData param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                int nID = param.sID.DecryptParameter().ToInt();
                TB_Request_OT? objOT = _db.TB_Request_OT.Where(w => w.nRequestOTID == nID).FirstOrDefault();

                if (objOT != null)
                {
                    objOT.nUpdateBy = nUserID;
                    objOT.dUpdate = DateTime.Now;

                    //Action From Line
                    if (param.IsActionFromLine == true)
                    {
                        TB_Log_WebhookLine? objLog = _db.TB_Log_WebhookLine.Where(w => w.sGUID == param.sGUID).FirstOrDefault();
                        if (objLog != null)
                        {
                            objOT.nStatusID = (int)EnumGlobal.StatusRequestOT.Closed;
                            objLog.IsActionAlready = true;
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        //Add Result
                        objOT.dStartActionDateTime = param.dActionStartResult;
                        objOT.dEndActionDateTime = param.dActionEndResult < param.dActionStartResult ? param.dActionEndResult.Value.AddDays(1) : param.dActionEndResult;
                        objOT.nStatusID = param.IsClosedOT == true ? (int)EnumGlobal.StatusRequestOT.Closed : (int)EnumGlobal.StatusRequestOT.WaitingClosed;

                        //Add Task
                        var lstTask = _db.TB_Request_OT_Task.Where(w => w.nRequestOTID == nID && !w.IsDelete).ToList();
                        foreach (var iT in lstTask)
                        {
                            if (param.lstTask != null)
                            {
                                iT.IsTaskCompleted = param.lstTask.FirstOrDefault(w => w.nID == iT.nRequestTaskID)?.IsTaskCompleted;
                                iT.nOTHour = param.lstTask.FirstOrDefault(w => w.nID == iT.nRequestTaskID)?.nOTHourResult ?? 0;
                                iT.sReason = param.lstTask.FirstOrDefault(w => w.nID == iT.nRequestTaskID)?.sReasonResult;
                                iT.nUpdateBy = nUserID;
                                iT.dUpdate = DateTime.Now;
                            }
                        }
                        _db.SaveChanges();
                    }
                    SaveLog(objOT);
                    #region Send Line Noti
                    string sGUID = Guid.NewGuid().ToString();

                    var arrProject = _db.TB_Project.ToArray();
                    var arrStatus = _db.TM_Status.ToArray();
                    decimal? nSumHour = _db.TB_Request_OT_Task.Where(w => w.nRequestOTID == objOT.nRequestOTID && !w.IsDelete).Select(s => s.nOTHour).Sum();

                    cParamSendLine objParam = new cParamSendLine();
                    objParam.sGUID = sGUID;
                    objParam.nRequesterID = nUserID;
                    objParam.sDate = objOT.dUpdate.ToStringFromDate("dd/MM/yyyy");
                    objParam.sTime = objOT.dUpdate.ToStringFromDate("HH:mm") + " น.";

                    objParam.sProject = arrProject.Where(w => w.nProjectID == objOT.nProjectID).Select(s => s.sProjectName).FirstOrDefault() ?? "";
                    objParam.sDetailRequest = objOT.sTopic;
                    objParam.sStartDate = objOT.dPlanDateTime.ToStringFromDate("dd/MM/yyyy");
                    objParam.sHours = ((objOT.nEstimateHour + "") ?? "-") + " ชม.";
                    objParam.sHoursActual = nSumHour + " ชม.";
                    objParam.sStatus = arrStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.OT && w.nStatusID == objOT.nStatusID).Select(s => s.sStatusName).FirstOrDefault() ?? "";
                    objParam.sPathApprove = "ApproveOTLine&sID=" + objOT.nRequestOTID.EncryptParameter() + "&sUserID=" + objOT.nApproveBy.EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestOT.Closed) + "&sGUID=" + sGUID;
                    objParam.sPathDetail = "DetailOTLine&sID=" + objOT.nRequestOTID.EncryptParameter() + "&sUserID=" + objOT.nApproveBy.EncryptParameter();

                    switch (objOT.nStatusID)
                    {
                        case 5: //Waiting Closed
                            objParam.nTemplateID = 19;
                            objParam.lstEmpTo = new List<int> { objOT.nApproveBy };
                            break;
                        case 6: //Closed
                            objParam.nTemplateID = 20;
                            objParam.lstEmpTo = new List<int> { objOT.nCreateBy };
                            break;
                    }
                    STFunction.SendToLine(objParam);
                    #endregion Send Line Noti 
                }

                result.Status = StatusCodes.Status200OK;
            }
            catch (System.Exception ex)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// Get ข้อมูลรายการ OT
        /// </summary>
        public cResultTableOT GetDataTable(cReqTableOT param)
        {
            cResultTableOT result = new cResultTableOT();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                #region Query
                var qry = (from a in _db.TB_Request_OT.Where(w => w.IsDelete != true)
                           join b in _db.TB_Project on a.nProjectID equals b.nProjectID
                           join c in _db.TB_Employee on a.nApproveBy equals c.nEmployeeID
                           join d in _db.TM_Status.Where(w => w.nRequestTypeID == 2) on a.nStatusID equals d.nStatusID
                           join e in _db.TB_Employee on a.nCreateBy equals e.nEmployeeID
                           //    where (a.nCreateBy == nUserID || a.nApproveBy == nUserID)
                           select new cDataTableOT
                           {
                               sID = a.nRequestOTID.EncryptParameter(),
                               nRequestOTID = a.nRequestOTID,
                               sCreate = a.dCreate.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                               nRequester = e.nEmployeeID,
                               sRequester = $"{e.sNameTH} {e.sSurnameTH} ({e.sNickname})",
                               dCreate = a.dCreate,
                               dPlanDateTime = a.dPlanDateTime,
                               sPlanDate = a.dPlanDateTime.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                               nProjectID = a.nProjectID,
                               sProject = !string.IsNullOrEmpty(b.sProjectAbbr) ? b.sProjectAbbr + " : " + b.sProjectName : b.sProjectName,
                               sTopic = a.sTopic,
                               sType = a.IsHoliday ? "Holiday" : "Normal",
                               nHour = null,
                               sHour = null,
                               sApproveBy = $"{c.sNameTH} {c.sSurnameTH} ({c.sNickname})",
                               nStatusID = a.nStatusID,
                               sStatus = d.sStatusName,
                               IsRequester = a.nCreateBy == nUserID,
                               IsApprover = a.nApproveBy == nUserID,
                               dUpdate = a.dUpdate,
                           }).ToArray();

                if (param.lstProjectID != null && param.lstProjectID.Any())
                {
                    qry = qry.Where(w => param.lstProjectID.Contains(w.nProjectID + "")).ToArray();
                }

                if (param.dRequestStart != null)
                {
                    qry = qry.Where(w => w.dPlanDateTime != null ? w.dPlanDateTime.Value.Date >= param.dRequestStart.Value.Date : false).ToArray();
                }
                if (param.dRequestEnd != null)
                {
                    qry = qry.Where(w => w.dPlanDateTime != null ? w.dPlanDateTime.Value.Date <= param.dRequestEnd.Value.Date : false).ToArray();
                }
                if (param.lstRequesterID != null && param.lstRequesterID.Any())
                {
                    qry = qry.Where(w => param.lstRequesterID.Contains(w.nRequester + "")).ToArray();
                }
                if (param.lstStatusID != null && param.lstStatusID.Any())
                {
                    qry = qry.Where(w => param.lstStatusID.Contains(w.nStatusID + "")).ToArray();
                }

                //หาชั่วโมงที่ทำจริง
                var lstAllTaskResult = _db.TB_Request_OT_Task.Where(w => !w.IsDelete).ToList();
                foreach (var item in qry.ToList())
                {
                    var lstHour = lstAllTaskResult.Where(w => w.nRequestOTID == item.nRequestOTID && w.nOTHour != null).ToList();
                    item.nHour = lstHour != null && lstHour.Any() ? lstHour.Select(s => s.nOTHour).Sum() : 0;
                    item.sHour = lstHour != null && lstHour.Any() ? lstHour.Select(s => s.nOTHour).Sum().ToString() : "-";
                }

                #endregion

                #region//SORT
                string sSortColumn = param.sSortExpression;
                switch (param.sSortExpression)
                {
                    case "sCreate": sSortColumn = "dCreate"; break;
                    case "sPlanDate": sSortColumn = "dPlanDateTime"; break;
                    case "sProject": sSortColumn = "sProject"; break;
                    case "sTopic": sSortColumn = "sTopic"; break;
                    case "sType": sSortColumn = "sType"; break;
                    case "nHour": sSortColumn = "nHour"; break;
                    case "sApproveBy": sSortColumn = "sApproveBy"; break;
                    case "sStatus": sSortColumn = "sStatus"; break;
                    case "dUpdate": sSortColumn = "dUpdate"; break;
                }
                if (param.isASC)
                {
                    qry = qry.OrderBy<cDataTableOT>(sSortColumn).ToArray();
                }
                else if (param.isDESC)
                {
                    qry = qry.OrderByDescending<cDataTableOT>(sSortColumn).ToArray();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qry.Count());
                var lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();



                var nIndex = param.nPageSize * (param.nPageIndex - 1) + 1;
                if (nIndex < 1) nIndex = 1;
                foreach (var item in lstData)
                {
                    item.No = nIndex;
                    nIndex++;
                }

                result.lstData = lstData;
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
        public cResultTableOT GetInitDataTable()
        {
            cResultTableOT result = new cResultTableOT();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                result.lstStatus = _db.TM_Status.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.OT).OrderBy(o => o.nOrder).Select(s => new cSelectOption
                {
                    value = s.nStatusID + "",
                    label = s.sStatusName
                }).ToList();

                result.lstProject = _db.TB_Project.Where(w => !w.IsDelete).Select(s => new cSelectOption
                {
                    value = s.nProjectID + "",
                    label = !string.IsNullOrEmpty(s.sProjectAbbr) ? s.sProjectAbbr + " : " + s.sProjectName : s.sProjectName
                }).ToList();

                result.lstRequester = _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete).Select(s => new cSelectOption
                {
                    value = s.nEmployeeID + "",
                    label = $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})",
                }).ToList();

                List<string> lstUser = new List<string>();
                IQueryable<TB_Employee_Report_To> TBEmployeeReportTo = _db.TB_Employee_Report_To.Where(w => !w.IsDelete).OrderBy(o => o.nEmployeeID).AsQueryable();
                bool IsHead = TBEmployeeReportTo.Any(f => f.nRepEmployeeID == nUserID);

                //Add User
                lstUser.Add(nUserID + "");

                //Add ลูกทีม
                if (IsHead)
                {
                    List<TB_Employee_Report_To>? lstEmployeeReportTo = TBEmployeeReportTo.Where(w => w.nRepEmployeeID == nUserID).ToList();
                    int idx = 0;
                    foreach (var item in lstEmployeeReportTo)
                    {
                        if (idx == 0)
                        {
                            lstUser.Add(nUserID + "");
                        }
                        lstUser.Add(item.nEmployeeID + "");
                        idx++;
                    }
                }

                result.lstSelectUser = lstUser;

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
        /// ลบ Row ใน Table List
        /// </summary>
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
                    var lstData = _db.TB_Request_OT.Where(w => lstID.Contains(w.nRequestOTID)).ToList();

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
        /// <summary>
        /// Reject OT
        /// </summary>
        public ResultAPI Reject(cReqDataOT param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                TB_Request_OT? objOT = _db.TB_Request_OT.Where(w => w.nRequestOTID == param.sID.DecryptParameter().ToInt()).FirstOrDefault();

                if (objOT != null)
                {
                    objOT.nStatusID = param.nStatusID ?? 3;
                    objOT.sNoteApprover = param.sComment;
                    objOT.nUpdateBy = nUserID;
                    objOT.dUpdate = DateTime.Now;

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
                    SaveLog(objOT);

                    #region Send Line Noti
                    string sGUID = Guid.NewGuid().ToString();

                    var arrProject = _db.TB_Project.ToArray();
                    var arrStatus = _db.TM_Status.ToArray();

                    cParamSendLine objParam = new cParamSendLine();
                    objParam.sGUID = sGUID;
                    objParam.nRequesterID = nUserID;
                    objParam.sDate = objOT.dPlanDateTime.ToStringFromDate("dd/MM/yyyy");
                    objParam.sTime = objOT.dUpdate.ToStringFromDate("HH:mm") + " น.";

                    objParam.sProject = arrProject.Where(w => w.nProjectID == objOT.nProjectID).Select(s => s.sProjectName).FirstOrDefault() ?? "";
                    objParam.sDetailRequest = objOT.sTopic ?? "";
                    objParam.sStartDate = objOT.dPlanDateTime.ToStringFromDate("dd/MM/yyyy");
                    objParam.sHours = ((objOT.nEstimateHour + "") ?? "-") + " ชม.";
                    objParam.sRemark = objOT.sNoteApprover ?? "";
                    objParam.sStatus = arrStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.OT && w.nStatusID == objOT.nStatusID).Select(s => s.sStatusName).FirstOrDefault() ?? "";
                    objParam.sPathDetail = "DetailOTLine&sID=" + objOT.nRequestOTID.EncryptParameter() + "&sUserID=" + objOT.nApproveBy.EncryptParameter();

                    switch (objOT.nStatusID)
                    {
                        case 3: //Reject
                            objParam.nTemplateID = 17;
                            objParam.lstEmpTo = new List<int> { objOT.nCreateBy };

                            break;
                        case 0: //Canceled
                            objParam.nTemplateID = 18;
                            objParam.lstEmpTo = new List<int> { objOT.nCreateBy };

                            break;

                    }
                    STFunction.SendToLine(objParam);
                    #endregion Send Line Noti 

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
        public bool SaveLog(TB_Request_OT data)
        {
            bool IsComplete = false;
            try
            {
                var objLog = _db.TB_Request_OT_History.Where(w => w.nRequestOTID == data.nRequestOTID && data.nStatusID == 1 && w.nStatusID == 1).FirstOrDefault();

                bool isNew = false;
                if (objLog == null)
                {
                    objLog = new TB_Request_OT_History();
                    isNew = true;
                }

                objLog.nRequestOTID = data.nRequestOTID;
                objLog.nRequestTypeID = data.nRequestTypeID;
                objLog.IsHoliday = data.IsHoliday;
                objLog.nProjectID = data.nProjectID;
                objLog.nProcessID = data.nProcessID;
                objLog.nApproveBy = data.nApproveBy;
                objLog.sTopic = data.sTopic;
                objLog.dPlanDateTime = data.dPlanDateTime;
                objLog.nEstimateHour = data.nEstimateHour;
                objLog.dStartActionDateTime = data.dStartActionDateTime;
                objLog.dEndActionDateTime = data.dEndActionDateTime;
                objLog.nActionHour = data.nActionHour;
                objLog.nStepID = data.nStepID;
                objLog.nStatusID = data.nStatusID;
                objLog.sNoteApprover = data.sNoteApprover;
                objLog.dCreate = data.dCreate;
                objLog.nCreateBy = data.nCreateBy;
                objLog.dUpdate = data.dUpdate;
                objLog.nUpdateBy = data.nUpdateBy;
                objLog.dDelete = data.dDelete;
                objLog.nDeleteBy = data.nDeleteBy;
                objLog.IsDelete = data.IsDelete;

                //Add
                if (isNew)
                {
                    int newID = _db.TB_Request_OT_History.Any() ? _db.TB_Request_OT_History.Select(s => s.nRequestOTHisID).Max() + 1 : 1;
                    objLog.nRequestOTHisID = newID;
                    _db.TB_Request_OT_History.Add(objLog);
                }

                _db.SaveChanges();
                IsComplete = true;
            }
            catch (System.Exception)
            {
                IsComplete = false;
            }
            return IsComplete;
        }

        /// <summary>
        /// Report Excel
        /// </summary>
        /// 

        public async Task<cOTData> GetDataExport(cReqTableOT param)
        {

            cOTData result = new cOTData();
            var TB_Request_OT = _db.TB_Request_OT.Where(w => w.IsDelete != true && w.nStatusID != 1).ToList();
            var lstTime = _db.TB_Employee_TimeStemp.ToList();
            var lstTask = _db.TB_Request_OT_Task.Where(w => !w.IsDelete).ToList();
            var qry = (from a in TB_Request_OT
                       from b in _db.TB_Project.Where(w => w.nProjectID == a.nProjectID)
                       from c in _db.TB_Employee.Where(w => w.nEmployeeID == a.nCreateBy)
                       from d in _db.TM_Status.Where(w => w.nRequestTypeID == 2 && w.nStatusID == a.nStatusID)
                           // from e in _db.TB_Employee_TimeStemp.Where(w => w.nEmployeeID == a.nCreateBy && w.dTimeDate.Date == a.dPlanDateTime.Date)
                       select new cDataTableOT
                       {
                           sID = a.nRequestOTID + "",
                           sCreate = a.dPlanDateTime.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                           nRequester = a.nCreateBy,
                           sRequester = $"{c.sNameTH} {c.sSurnameTH} ({c.sNickname})",
                           dStartActionDateTime = lstTime.Any(w => w.nEmployeeID == a.nCreateBy && w.dTimeDate == a.dPlanDateTime) ? lstTime.First(w => w.nEmployeeID == a.nCreateBy && w.dTimeDate == a.dPlanDateTime).dTimeStartDate : a.dStartActionDateTime,
                           dEndActionDateTime = lstTime.Any(w => w.nEmployeeID == a.nCreateBy && w.dTimeDate == a.dPlanDateTime) ? lstTime.First(w => w.nEmployeeID == a.nCreateBy && w.dTimeDate == a.dPlanDateTime).dTimeEndDate : a.dEndActionDateTime,
                           nProjectID = a.nProjectID,
                           sProject = !string.IsNullOrEmpty(b.sProjectAbbr) ? b.sProjectAbbr + " : " + b.sProjectName : b.sProjectName,
                           nStatusID = a.nStatusID,
                           sStatus = d.sStatusName,
                           sActionStartTime = lstTime.Any(w => w.nEmployeeID == a.nCreateBy && w.dTimeDate == a.dPlanDateTime) ? lstTime.First(w => w.nEmployeeID == a.nCreateBy && w.dTimeDate == a.dPlanDateTime).dTimeStartDate.ToStringFromDate("HH:mm:ss", "th-TH") : a.dStartActionDateTime.ToStringFromDate("HH:mm:ss", "th-TH"),
                           sActionEndTime = lstTime.Any(w => w.nEmployeeID == a.nCreateBy && w.dTimeDate == a.dPlanDateTime) ? lstTime.First(w => w.nEmployeeID == a.nCreateBy && w.dTimeDate == a.dPlanDateTime).dTimeEndDate.ToStringFromDate("HH:mm:ss", "th-TH") : a.dEndActionDateTime.ToStringFromDate("HH:mm:ss", "th-TH"),
                           dPlanDateTime = a.dPlanDateTime,
                           IsHoliday = a.IsHoliday


                       }).ToArray();

            var Reason = (
                               from g in _db.TB_Request_OT_Reason.Where(w => !w.IsDelete)
                               from tm in _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.ReasonType && w.nData_ID == g.nReasonID)
                               select new cDataTableOT
                               {
                                   sReason = g.nReasonID == 7 ? g.sOther : tm.sNameTH,
                                   sID = g.nRequestOTID + "",
                               }).ToArray();

            var lstTaskDetail = lstTask.Select(s => new cDataTableOT
            {
                sID = s.nRequestOTID + "",
                sDescription = s.sDescription != null ? s.sDescription : "",
            }).ToList();
            var group = lstTask.GroupBy(g => g.nRequestOTID).ToList();

            //หาชั่วโมงที่ทำจริง
            var lstAllTaskResult = _db.TB_Request_OT_Task.Where(w => !w.IsDelete).ToList();
            foreach (var item in qry.ToList())
            {
                var lstHour = lstAllTaskResult.Where(w => w.nRequestOTID == item.sID.toInt() && w.nOTHour != null).ToList();
                item.nHourHoliday = item.IsHoliday == true ? lstHour != null && lstHour.Any() ? lstHour.Select(s => s.nOTHour).Sum() : 0 : 0;
                item.nHourNormal = item.IsHoliday == false ? lstHour != null && lstHour.Any() ? lstHour.Select(s => s.nOTHour).Sum() : 0 : 0;
            }

            foreach (var item in qry)
            {
                item.lstReason = Reason.Where(w => w.sID == item.sID).ToList();
                item.lstTask = lstTaskDetail.Where(w => w.sID == item.sID).ToList();
            }

            //Filter

            if (param.lstProjectID != null && param.lstProjectID.Any())
            {
                qry = qry.Where(w => param.lstProjectID.Contains(w.nProjectID + "")).ToArray();
                TB_Request_OT = TB_Request_OT.Where(w => param.lstProjectID.Contains(w.nProjectID + "")).ToList();
            }
            if (param.dRequestStart != null)
            {
                qry = qry.Where(w => w.dPlanDateTime.Value.Date >= param.dRequestStart.Value.Date).ToArray();
                TB_Request_OT = TB_Request_OT.Where(w => w.dPlanDateTime.Date >= param.dRequestStart.Value.Date).ToList();
            }
            if (param.dRequestEnd != null)
            {
                qry = qry.Where(w => w.dPlanDateTime.Value.Date <= param.dRequestEnd.Value.Date).ToArray();
                TB_Request_OT = TB_Request_OT.Where(w => w.dPlanDateTime.Date <= param.dRequestEnd.Value.Date).ToList();
            }
            if (param.lstRequesterID != null && param.lstRequesterID.Any())
            {
                qry = qry.Where(w => param.lstRequesterID.Contains(w.nRequester + "")).ToArray();
                TB_Request_OT = TB_Request_OT.Where(w => param.lstRequesterID.Contains(w.nCreateBy + "")).ToList();

            }
            if (param.lstStatusID != null && param.lstStatusID.Any())
            {
                qry = qry.Where(w => param.lstStatusID.Contains(w.nStatusID + "")).ToArray();
                TB_Request_OT = TB_Request_OT.Where(w => param.lstStatusID.Contains(w.nStatusID + "")).ToList();
            }

            var lstRequest = TB_Request_OT.GroupBy(g => new { g.nCreateBy }).ToList();
            List<cDataExportOT> lstEmployeeID = new List<cDataExportOT>();
            List<cDataExportOT> lstRequest_OT = new List<cDataExportOT>();

            foreach (var f in lstRequest)
            {
                var lstHoliday = TB_Request_OT.Where(w => w.nCreateBy == f.Key.nCreateBy && w.IsHoliday).Select(s => s.nRequestOTID).ToList();
                var lstNormal = TB_Request_OT.Where(w => w.nCreateBy == f.Key.nCreateBy && !w.IsHoliday).Select(s => s.nRequestOTID).ToList();
                var nSumHourHoliday = _db.TB_Request_OT_Task.Where(w => lstHoliday.Contains(w.nRequestOTID)).Select(s => s.nOTHour).Sum();
                var nSumHourNormal = _db.TB_Request_OT_Task.Where(w => lstNormal.Contains(w.nRequestOTID)).Select(s => s.nOTHour).Sum();
                var Employee = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == f.Key.nCreateBy);
                if (Employee != null)
                {
                    cDataExportOT objData = new cDataExportOT()
                    {
                        sRequester = $"{Employee.sNameTH} {Employee.sSurnameTH} ({Employee.sNickname})",
                        nSumHourHoliday = nSumHourHoliday != null ? nSumHourHoliday : 0,
                        nSumHourNormal = nSumHourNormal != null ? nSumHourNormal : 0,
                        sEmployeeID = Employee.nEmployeeID + "",
                    };
                    lstEmployeeID.Add(objData);
                    objData.lstDataDetail = new List<cDataTableOT>();
                    foreach (var item in lstEmployeeID)
                    {
                        item.lstDataDetail = qry.Where(w => w.nRequester == item.sEmployeeID.toInt()).ToList();
                    };
                    lstRequest_OT.Add(objData);
                }
            };

            result.lstData = lstRequest_OT.ToList();
            result.Status = StatusCodes.Status200OK;

            return result;

        }
        public async Task<cExportExcelOT> ExportExcelOT(cReqTableOT param)
        {
            cExportExcelOT result = new cExportExcelOT();

            string sFileName = "OT_" + DateTime.Now.ToString("ddMMyyyyHHmm");
            try
            {

                cOTData lstdata = await GetDataExport(param);

                string sMonth = param.dRequestEnd.ToStringFromDate("MMMM", "th-TH");
                string sDate = param.dRequestStart.ToStringFromDate("dd/MM/yyyy", "th-TH") + "-" + param.dRequestEnd.ToStringFromDate("dd/MM/yyyy", "th-TH");

                using (var db = new ST_IntranetEntity())
                {
                    var wb = new XLWorkbook();
                    IXLWorksheet ws = wb.Worksheets.Add(sMonth);
                    int nFontSize = STFunction.FontSize_Export;
                    int nFontSizeHead = STFunction.FontSize_Export_Head;
                    var Color_Head = STFunction.Color_Head;
                    var Color_Foot = STFunction.Color_Foot;

                    ws.PageSetup.Margins.Top = 0.2;
                    ws.PageSetup.Margins.Bottom = 0.2;
                    ws.PageSetup.Margins.Left = 0.1;
                    ws.PageSetup.Margins.Right = 0;
                    ws.PageSetup.Margins.Footer = 0;
                    ws.PageSetup.Margins.Header = 0;

                    #region  Header
                    int nRow = 1;
                    int nStartBorder = nRow + 1;
                    if (lstdata.lstData.Any() || lstdata.lstData != null)
                    {
                        List<string> lstHeader = new List<string>() { "Requester", "วันที่ทำ OT", "โครงการ", "รายละเอียดงาน", "เหตุผล", "ชั่วโมงที่ทำ OT (วันธรรมดา)", "ชั่วโมงที่ทำ OT (วันหยุด)", "เวลาเข้างาน", "เวลาออกงาน", };
                        List<int> lstwidthHeader = new List<int>() { 40, 20, 50, 40, 50, 20, 20, 15, 15 };
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
                        ws.Cell(nRow, 1).Value = "สรุป OT พนักงานประจำเดือน  " + sMonth + "  " + sDate;
                        ws.Cell(nRow, 1).Style.Font.FontSize = nFontSize;
                        ws.Cell(nRow, 1).Style.Font.Bold = true;
                        ws.Cell(nRow, 1).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(nRow, 1).Style.Alignment.WrapText = true; //ปัดบรรทัด
                        ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(nRow, 1).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#c9daf8"));

                        ws.Range(nRow, 1, nRow, 1 + 8).Merge();
                        foreach (var item in lstHeader)
                        {
                            nRow = 2;
                            indexHead += 1;
                            ws.Cell(nRow, indexHead).Value = item;
                            ws.Cell(nRow, indexHead).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, indexHead).Style.Font.Bold = true;
                            ws.Cell(nRow, indexHead).Style.Font.FontColor = XLColor.White;
                            ws.Cell(nRow, indexHead).Style.Alignment.WrapText = true; //ปัดบรรทัด
                            ws.Cell(nRow, indexHead).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, indexHead).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, indexHead).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#4a86e8"));
                        }
                        #endregion
                        foreach (var f in lstdata.lstData)
                        {
                            nRow++; //ขึ้นบรรทัดใหม่
                            ws.Cell(nRow, 1).Value = f.sRequester;
                            ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 1).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 1).Style.Alignment.WrapText = true;
                            foreach (var item in f.lstDataDetail)
                            {

                                ws.Cell(nRow, 2).Value = item.sCreate;
                                ws.Cell(nRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 2).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 2).Style.Alignment.WrapText = true;

                                ws.Cell(nRow, 3).Value = item.sProject;
                                ws.Cell(nRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                ws.Cell(nRow, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 3).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 3).Style.Alignment.WrapText = true;

                                string sDescription = String.Join(", ", item.lstTask.Select(s => s.sDescription));
                                ws.Cell(nRow, 4).Value = "Detail: " + sDescription;
                                ws.Cell(nRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                ws.Cell(nRow, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                                ws.Cell(nRow, 4).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 4).Style.Alignment.WrapText = true;

                                string sReason = String.Join(", ", item.lstReason.Select(s => s.sReason));
                                ws.Cell(nRow, 5).Value = "Reason: " + sReason;
                                ws.Cell(nRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                ws.Cell(nRow, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                                ws.Cell(nRow, 5).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 5).Style.Alignment.WrapText = true;

                                ws.Cell(nRow, 6).Value = item.nHourNormal;
                                ws.Cell(nRow, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 6).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 6).Style.Alignment.WrapText = true;
                                if (item.nHourNormal > 0)
                                {
                                    ws.Cell(nRow, 6).Style.Font.FontColor = XLColor.Black;
                                    ws.Cell(nRow, 6).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#fff2cc"));
                                }

                                ws.Cell(nRow, 7).Value = item.nHourHoliday;
                                ws.Cell(nRow, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 7).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 7).Style.Alignment.WrapText = true;
                                if (item.nHourHoliday > 0)
                                {
                                    ws.Cell(nRow, 7).Style.Font.FontColor = XLColor.Black;
                                    ws.Cell(nRow, 7).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#fff2cc"));
                                }


                                ws.Cell(nRow, 8).Value = item.sActionStartTime;
                                ws.Cell(nRow, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 8).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 8).Style.Alignment.WrapText = true;


                                ws.Cell(nRow, 9).Value = item.sActionEndTime;
                                ws.Cell(nRow, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 9).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 9).Style.Alignment.WrapText = true;
                                nRow++; //ขึ้นบรรทัดใหม่
                            }

                            ws.Cell(nRow, 1).Value = "รวม";
                            ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 1).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 1).Style.Alignment.WrapText = true;
                            ws.Range(nRow, 1, nRow, 1 + 4).Merge();
                            ws.Cell(nRow, 1).Style.Font.FontColor = XLColor.Black;
                            ws.Cell(nRow, 1).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#c9daf8"));


                            ws.Cell(nRow, 6).Value = f.nSumHourNormal;
                            ws.Cell(nRow, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 6).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 6).Style.Alignment.WrapText = true;
                            ws.Cell(nRow, 6).Style.Font.FontColor = XLColor.Black;
                            ws.Cell(nRow, 6).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#c9daf8"));

                            ws.Cell(nRow, 7).Value = f.nSumHourHoliday;
                            ws.Cell(nRow, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 7).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 7).Style.Alignment.WrapText = true;
                            ws.Cell(nRow, 7).Style.Font.FontColor = XLColor.Black;
                            ws.Cell(nRow, 7).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#c9daf8"));

                            ws.Cell(nRow, 8).Style.Font.FontColor = XLColor.Black;
                            ws.Cell(nRow, 8).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#c9daf8"));
                            ws.Cell(nRow, 9).Style.Font.FontColor = XLColor.Black;
                            ws.Cell(nRow, 9).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#c9daf8"));
                        }

                    }
                    else
                    {
                        // No data
                        nRow++;
                        ws.Range("A" + nRow + ":J" + nRow).Merge();
                        ws.Cell(nRow, 1).Value = "ไม่พบข้อมูล";
                        ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }
                    //ระยะเส้นขอบที่ต้องการให้แสดง
                    ws.Range(nStartBorder, 1, nRow, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(nStartBorder, 1, nRow, 9).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    using (MemoryStream fs = new MemoryStream())
                    {
                        wb.SaveAs(fs);
                        fs.Position = 0;
                        result.objFile = fs.ToArray();
                        result.sFileType = "application/vnd.ms-excel";
                        result.sFileName = sFileName;
                    }
                }
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

