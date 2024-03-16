using ST.INFRA;
using Extensions.Common.STExtension;
using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Backend.Models;
using Backend.Enum;
using Backend.Interfaces;
using Backend.Models.Authentication;
using ResultAPI = Extensions.Common.STResultAPI.ResultAPI;
using ST.INFRA.Common;
using Extensions.Common.STFunction;
using ST_API.Models;

namespace Backend.Services.ISystemService
{
    public class AllowanceService : IAllowanceService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _authen;

        public AllowanceService(ST_IntranetEntity db, IAuthentication authen)
        {
            _db = db;
            _authen = authen;
        }
        public cInitAllowanceData LoadDataInit()
        {
            cInitAllowanceData result = new cInitAllowanceData();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                var objUser = _db.TB_Employee.Where(w => w.nEmployeeID == nUserID).FirstOrDefault();
                result.lstProject = _db.TB_Project.Where(w => !w.IsDelete).Select(s => new cSelectOption
                {
                    value = s.nProjectID + "",
                    label = !string.IsNullOrEmpty(s.sProjectAbbr) ? s.sProjectAbbr + " : " + s.sProjectName : s.sProjectName
                }).ToArray();

                result.lstAllowanceType = _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.AllowanceType && w.IsActive && !w.IsDelete).Select(s => new cSelectOption
                {
                    value = s.nData_ID + "",
                    label = s.sNameTH
                }).ToArray();

                result.lstVehicle = _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.VehicleType && w.IsActive && !w.IsDelete).Select(s => new cSelectOption
                {
                    value = s.nData_ID + "",
                    label = s.sNameTH
                }).ToArray();

                result.lstStartType = _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.TravelStartType && w.IsActive && !w.IsDelete).Select(s => new cSelectOption
                {
                    value = s.nData_ID + "",
                    label = s.sNameTH
                }).ToArray();

                if (objUser != null)
                {
                    result.sUserName = $"{objUser.sNameTH} {objUser.sSurnameTH} ({objUser.sNickname})";
                    var objPos = _db.TB_Employee_Position.Where(w => w.nEmployeeID == nUserID).FirstOrDefault();
                    if (objPos != null)
                    {
                        result.sPosition = _db.TB_Position.Where(w => w.nPositionID == objPos.nPositionID).Select(s => s.sPositionName).FirstOrDefault() + "";
                    }
                }

                var objUserHR = (from a in _db.TB_Employee_Position
                                 join b in _db.TB_Employee on a.nEmployeeID equals b.nEmployeeID
                                 join c in _db.TB_Position on a.nPositionID equals c.nPositionID
                                 where a.nPositionID == 9 && b.IsActive && !b.IsDelete
                                 select new cUserHR
                                 {
                                     sID = b.nEmployeeID + "",
                                     nNo = 1,
                                     sName = $"{b.sNameTH} {b.sSurnameTH} ({b.sNickname})",
                                     sPosition = c.sPositionName,
                                     sAction = "อนุมัติ",
                                 }
                                 ).FirstOrDefault();

                if (objUserHR != null)
                {
                    result.objHR = objUserHR;
                }

                result.Status = StatusCodes.Status200OK;
                result.Message = "";
            }
            catch (System.Exception ex)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;
            }

            return result;
        }
        public cAllowanceData GetDataAllowance(cReqDataAllowance param)
        {
            cAllowanceData result = new cAllowanceData();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                int nID = param.sID.DecryptParameter().ToInt();
                var objData = _db.TB_Request_WF_Allowance.FirstOrDefault(f => f.nRequestAllowanceID == nID);
                if (objData != null)
                {
                    var lstEmployee = _db.TB_Employee.ToArray();

                    var objUser = _db.TB_Employee.Where(w => w.nEmployeeID == objData.nCreateBy).FirstOrDefault();
                    if (objUser != null)
                    {
                        result.sUserName = $"{objUser.sNameTH} {objUser.sSurnameTH} ({objUser.sNickname})";
                        var objPos = _db.TB_Employee_Position.Where(w => w.nEmployeeID == objData.nCreateBy).FirstOrDefault();
                        if (objPos != null)
                        {
                            result.sPosition = _db.TB_Position.Where(w => w.nPositionID == objPos.nPositionID).Select(s => s.sPositionName).FirstOrDefault();
                        }
                    }

                    var objUserHR = (from a in _db.TB_Employee_Position
                                     join b in _db.TB_Employee on a.nEmployeeID equals b.nEmployeeID
                                     where a.nPositionID == 9 && b.IsActive && !b.IsDelete
                                     select new
                                     {
                                         nEmpID = b.nEmployeeID,
                                         sHRName = $"{b.sNameTH} {b.sSurnameTH} ({b.sNickname})",
                                     }
                                 ).FirstOrDefault();

                    result.sProjectID = objData.nProjectID + "";
                    result.sDesc = objData.sDescription;
                    result.nAllowanceTypeID = objData.nAllowanceTypeID;
                    result.dStartDate_StartTime = objData.dStartDate_StartTime;
                    result.dStartDate_EndTime = objData.dStartDate_EndTime;
                    result.dEndDate_StartTime = objData.dEndDate_StartTime;
                    result.dEndDate_EndTime = objData.dEndDate_EndTime;
                    result.nSumAllowanceDay = objData.nSumDay;
                    result.nSumAllowanceMoney = objData.nSumMoney;
                    result.sComment = objData.sComment;
                    result.nStatusID = objData.nStatusID;
                    result.IsRequester = objData.nCreateBy == nUserID;
                    result.IsApprover = objUserHR != null ? objUserHR.nEmpID == nUserID : false;

                    //History Log
                    var lstLogAll = _db.TB_Request_WF_Allowance_History.Where(w => w.nRequestAllowanceID == nID).ToList();
                    var lstLog = new List<cHistoryLog>();
                    var lstStatus = _db.TM_Status.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Allowance).ToArray();
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
                                sComment = item.sComment ?? "-",
                                sPathImg = sPathImg ?? "",
                                sStatus = lstStatus.Where(w => w.nStatusID == item.nStatusID).Select(s => s.sStatusName).FirstOrDefault() + "",
                                sActionMode = item.nStatusID == 0 ? "Cancel" : item.nStatusID == 2 ? "Submit" : item.nStatusID == 4 ? "Approve" : lstStatus.Where(w => w.nStatusID == item.nStatusID).Select(s => s.sStatusName).FirstOrDefault() + "",
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
        public ResultAPI SaveData(cAllowanceData param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                TB_Request_WF_Allowance? objAlw = _db.TB_Request_WF_Allowance.Where(w => w.nRequestAllowanceID == param.sID.DecryptParameter().ToInt()).FirstOrDefault();

                bool isNew = false;
                if (objAlw == null)
                {
                    objAlw = new TB_Request_WF_Allowance();
                    isNew = true;
                }

                objAlw.nEmployeeID = nUserID;
                objAlw.nRequestTypeID = (int)EnumGlobal.RequestFormType.Allowance;
                objAlw.nProjectID = (param.sProjectID ?? "0").toInt();
                objAlw.sDescription = param.sDesc;
                objAlw.nAllowanceTypeID = param.nAllowanceTypeID;
                objAlw.dStartDate_StartTime = param.dStartDate_StartTime;
                objAlw.dStartDate_EndTime = param.dStartDate_EndTime;
                objAlw.dEndDate_StartTime = param.dEndDate_StartTime;
                objAlw.dEndDate_EndTime = param.dEndDate_EndTime;
                objAlw.nSumDay = param.nSumAllowanceDay;
                objAlw.nSumMoney = param.nSumAllowanceMoney;
                objAlw.nStatusID = param.nStatusID;
                objAlw.dUpdate = DateTime.Now;
                objAlw.nUpdateBy = nUserID;
                objAlw.sComment = null;

                //New
                if (isNew)
                {
                    // var isDuplicate = _db.TB_Request_WF_Allowance.Where(w => (w.sBannerNameTH.Trim().ToLower() == data.sBannerNameTH.Trim().ToLower()) && w.isDel != true).Any();
                    // if (isDuplicate)
                    // {
                    //     result.Status = StatusCodes.Status200OK;
                    //     result.Message = "Data is Duplicated";
                    //     return result;
                    // }
                    int newID = _db.TB_Request_WF_Allowance.Any() ? _db.TB_Request_WF_Allowance.Select(s => s.nRequestAllowanceID).Max() + 1 : 1;

                    objAlw.nRequestAllowanceID = newID;
                    objAlw.dCreate = DateTime.Now;
                    objAlw.nCreateBy = nUserID;
                    objAlw.IsDelete = false;

                    _db.TB_Request_WF_Allowance.Add(objAlw);
                }
                else //Edit
                {
                    // var isDuplicate = _db.TB_Request_WF_Allowance.Where(w => (w.sBannerNameTH.Trim().ToLower() == data.sBannerNameTH.Trim().ToLower()) && w.isDel != true).Any();
                    // if (isDuplicate)
                    // {
                    //     result.Status = StatusCodes.Status200OK;
                    //     result.Message = "Data is Duplicated";
                    //     return result;
                    // }
                }

                _db.SaveChanges();
                SaveLog(objAlw);

                if (objAlw.nStatusID != (int)EnumGlobal.StatusRequestWelfare.Draft)
                {
                    #region Send Line Noti
                    string sGUID = Guid.NewGuid().ToString();

                    var arrProject = _db.TB_Project.ToArray();
                    var arrStatus = _db.TM_Status.ToArray();
                    var lstUserHR = (from a in _db.TB_Employee_Position
                                     join b in _db.TB_Employee on a.nEmployeeID equals b.nEmployeeID
                                     where a.nPositionID == 9 && b.IsActive && !b.IsDelete
                                     select new
                                     {
                                         nEmpID = b.nEmployeeID,
                                     }
                     ).ToList();

                    cParamSendLine objParam = new cParamSendLine();
                    objParam.sGUID = sGUID;
                    objParam.nRequesterID = nUserID;
                    objParam.sDate = objAlw.dUpdate.ToStringFromDate("dd/MM/yyyy");
                    objParam.sTime = objAlw.dUpdate.ToStringFromDate("HH:mm") + " น.";
                    objParam.sProject = arrProject.Where(w => w.nProjectID == objAlw.nProjectID).Select(s => s.sProjectName).FirstOrDefault() ?? "";
                    objParam.sDetailRequest = objAlw.sDescription ?? "";

                    if (objAlw.nAllowanceTypeID == 123) //ไป-กลับ
                    {
                        objParam.sStartDate = objAlw.dStartDate_StartTime.ToStringFromDate("dd/MM/yyyy");
                        objParam.sStartTime = objAlw.dStartDate_StartTime.ToStringFromDate("HH:mm") + " น.";
                        objParam.sEndDate = objAlw.dStartDate_EndTime.ToStringFromDate("dd/MM/yyyy");
                        objParam.sEndTime = objAlw.dStartDate_EndTime.ToStringFromDate("HH:mm") + " น.";
                    }
                    else //ค้างคืน
                    {
                        objParam.sStartDate = objAlw.dStartDate_StartTime.ToStringFromDate("dd/MM/yyyy");
                        objParam.sStartTime = objAlw.dStartDate_StartTime.ToStringFromDate("HH:mm") + " น.";
                        objParam.sEndDate = objAlw.dEndDate_EndTime.ToStringFromDate("dd/MM/yyyy");
                        objParam.sEndTime = objAlw.dEndDate_EndTime.ToStringFromDate("HH:mm") + " น.";
                    }

                    objParam.sMoney = $"{objAlw.nSumMoney:n}" ?? "0";
                    objParam.sDay = objAlw.nSumDay.ToString() ?? "0";

                    var lstHR = string.Join(",", lstUserHR.Select(s => s.nEmpID)) + "";

                    objParam.sStatus = arrStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Allowance && w.nStatusID == objAlw.nStatusID).Select(s => s.sStatusName).FirstOrDefault() ?? "";
                    objParam.sPathApprove = "ApproveAllowanceLine&sID=" + objAlw.nRequestAllowanceID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestWelfare.Completed) + "&sGUID=" + sGUID;
                    objParam.sPathReject = "RejectAllowanceLine&sID=" + objAlw.nRequestAllowanceID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestOT.Reject) + "&sGUID=" + sGUID;
                    objParam.sPathCancel = "RejectAllowanceLine&sID=" + objAlw.nRequestAllowanceID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestOT.Canceled) + "&sGUID=" + sGUID;
                    objParam.sPathDetail = "DetailAllowanceLine&sID=" + objAlw.nRequestAllowanceID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter();

                    switch (objAlw.nStatusID)
                    {
                        case 2: //Wait for Approve
                            objParam.nTemplateID = 21;
                            objParam.lstEmpTo = lstUserHR.Select(s => s.nEmpID).ToList();
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
        public cResultTableWelfare GetInitDataTable()
        {
            cResultTableWelfare result = new cResultTableWelfare();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                var objUserHR = (from a in _db.TB_Employee_Position
                                 join b in _db.TB_Employee on a.nEmployeeID equals b.nEmployeeID
                                 where a.nPositionID == 9 && b.IsActive && !b.IsDelete
                                 select new
                                 {
                                     nEmpID = b.nEmployeeID,
                                     sName = $"{b.sNameTH} {b.sSurnameTH} ({b.sNickname})",
                                 }
                                 ).FirstOrDefault();

                result.IsApprover = objUserHR != null && objUserHR.nEmpID == nUserID;
                result.nUserID = nUserID;

                //Filter
                result.lstForm = _db.TM_RequestType.Where(w => w.IsActive
                && new List<int> { (int)EnumGlobal.RequestFormType.Allowance, (int)EnumGlobal.RequestFormType.Travel, (int)EnumGlobal.RequestFormType.Dental }.Contains(w.nRequestTypeID)
                ).Select(s => new cSelectOption
                {
                    value = s.nRequestTypeID + "",
                    label = s.nRequestTypeName
                }).ToList();
                result.lstRequester = _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete).Select(s => new cSelectOption
                {
                    value = s.nEmployeeID + "",
                    label = $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})",
                }).ToList();
                result.lstStatus = _db.TM_Status.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Allowance).Select(s => new cSelectOption
                {
                    value = s.nStatusID + "",
                    label = s.sStatusName
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
        public cResultTableWelfare GetDataTable(cReqTableWelfare param)
        {
            cResultTableWelfare result = new cResultTableWelfare();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                var objUserHR = (from a in _db.TB_Employee_Position
                                 join b in _db.TB_Employee on a.nEmployeeID equals b.nEmployeeID
                                 where a.nPositionID == 9 && b.IsActive && !b.IsDelete
                                 select new
                                 {
                                     nEmpID = b.nEmployeeID,
                                     sName = $"{b.sNameTH} {b.sSurnameTH} ({b.sNickname})",
                                 }
                                 ).FirstOrDefault();

                var lstAllowance = _db.TB_Request_WF_Allowance.Where(w => (w.nCreateBy == nUserID || (objUserHR != null ? (objUserHR.nEmpID == nUserID && w.nStatusID != 1) : false)) && w.IsDelete != true).ToArray();
                var lstTravel = _db.TB_Request_WF_TravelExpenses.Where(w => (w.nCreateBy == nUserID || (objUserHR != null ? (objUserHR.nEmpID == nUserID && w.nStatusID != 1) : false)) && w.IsDelete != true).ToArray();
                var lstDental = _db.TB_Request_WF_Dental.Where(w => (w.nCreateBy == nUserID || (objUserHR != null ? (objUserHR.nEmpID == nUserID && w.nStatus != 1) : false)) && w.IsDelete != true).ToArray();
                var lstRequestForm = _db.TM_RequestType.ToArray();
                var lstEmployee = _db.TB_Employee.ToArray();
                var lstStatus = _db.TM_Status.ToArray();

                #region Query
                var qryAllowance = (from a in lstAllowance
                                    join b in lstEmployee on a.nCreateBy equals b.nEmployeeID
                                    join c in lstStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Allowance) on a.nStatusID equals c.nStatusID
                                    select new cDataTableWelfare
                                    {
                                        sID = "sAllowance_" + a.nRequestAllowanceID.EncryptParameter(),
                                        dRequestDate = a.dCreate,
                                        sRequestDate = a.dCreate.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                                        sForm = lstRequestForm.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Allowance).Select(s => s.nRequestTypeName).FirstOrDefault(),
                                        sAmount = a.nSumMoney?.ToString("#,##0.##"),
                                        nRequester = b.nEmployeeID,
                                        sRequester = $"{b.sNameTH} {b.sSurnameTH} ({b.sNickname})",
                                        sWaitingBy = a.nStatusID == 2 ? objUserHR?.sName : "-",
                                        nStatusID = a.nStatusID,
                                        sStatus = lstStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Allowance && w.nStatusID == a.nStatusID).Select(s => s.sStatusName).FirstOrDefault(),
                                        sComment = a.sComment,
                                        IsRequester = a.nCreateBy == nUserID,
                                        IsApprover = objUserHR?.nEmpID == nUserID,
                                        dUpdate = a.dUpdate,
                                        nRequestTypeID = (int)EnumGlobal.RequestFormType.Allowance,
                                    }
                                    ).ToArray();

                var qryTravel = (from a in lstTravel
                                 join b in lstEmployee on a.nCreateBy equals b.nEmployeeID
                                 join c in lstStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Travel) on a.nStatusID equals c.nStatusID
                                 select new cDataTableWelfare
                                 {
                                     sID = "sTravel_" + a.nRequestTravelExpensesID.EncryptParameter(),
                                     dRequestDate = a.dCreate,
                                     sRequestDate = a.dCreate.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                                     sForm = lstRequestForm.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Travel).Select(s => s.nRequestTypeName).FirstOrDefault(),
                                     sAmount = a.nTotalAmount.ToString("#,##0.##"),
                                     nRequester = b.nEmployeeID,
                                     sRequester = $"{b.sNameTH} {b.sSurnameTH} ({b.sNickname})",
                                     sWaitingBy = a.nStatusID == 2 ? objUserHR?.sName : "-",
                                     nStatusID = a.nStatusID,
                                     sStatus = lstStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Travel && w.nStatusID == a.nStatusID).Select(s => s.sStatusName).FirstOrDefault(),
                                     sComment = a.sComment,
                                     IsRequester = a.nCreateBy == nUserID,
                                     IsApprover = objUserHR?.nEmpID == nUserID,
                                     dUpdate = a.dUpdate,
                                     nRequestTypeID = (int)EnumGlobal.RequestFormType.Travel,
                                 }
                ).ToArray();

                var qryDental = (from a in lstDental
                                 join b in lstEmployee on a.nCreateBy equals b.nEmployeeID
                                 join c in lstStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Dental) on a.nStatus equals c.nStatusID
                                 select new cDataTableWelfare
                                 {
                                     sID = "sDental_" + a.nRequestDentalID.EncryptParameter(),
                                     dRequestDate = a.dCreate,
                                     sRequestDate = a.dCreate.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                                     sForm = lstRequestForm.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Dental).Select(s => s.nRequestTypeName).FirstOrDefault(),
                                     sAmount = a.nAmountWithdrawn.ToString("#,##0.##"),
                                     nRequester = b.nEmployeeID,
                                     sRequester = $"{b.sNameTH} {b.sSurnameTH} ({b.sNickname})",
                                     sWaitingBy = a.nStatus == 2 ? objUserHR?.sName : "-",
                                     nStatusID = a.nStatus,
                                     sStatus = lstStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Dental && w.nStatusID == a.nStatus).Select(s => s.sStatusName).FirstOrDefault(),
                                     sComment = a.sComent,
                                     IsRequester = a.nCreateBy == nUserID,
                                     IsApprover = objUserHR?.nEmpID == nUserID,
                                     dUpdate = a.dUpdate,
                                     nRequestTypeID = (int)EnumGlobal.RequestFormType.Dental,
                                 }
                ).ToArray();

                var qry = qryAllowance.Union(qryTravel).Union(qryDental);
                if (param.lstFormID != null && param.lstFormID.Any())
                {
                    qry = qry.Where(w => param.lstFormID.Contains(w.nRequestTypeID + "")).ToArray();
                }

                if (param.dRequestStart != null)
                {
                    qry = qry.Where(w => w.dRequestDate != null ? w.dRequestDate.Value.Date >= param.dRequestStart.Value.Date : false).ToArray();
                }
                if (param.dRequestEnd != null)
                {
                    qry = qry.Where(w => w.dRequestDate != null ? w.dRequestDate.Value.Date <= param.dRequestEnd.Value.Date : false).ToArray();
                }

                if (param.lstRequesterID != null && param.lstRequesterID.Any())
                {
                    qry = qry.Where(w => param.lstRequesterID.Contains(w.nRequester + "")).ToArray();
                }
                if (param.lstStatusID != null && param.lstStatusID.Any())
                {
                    qry = qry.Where(w => param.lstStatusID.Contains(w.nStatusID + "")).ToArray();
                }
                #endregion

                #region//SORT
                string sSortColumn = param.sSortExpression;
                switch (param.sSortExpression)
                {
                    case "sRequestDate": sSortColumn = "dRequestDate"; break;
                    case "sForm": sSortColumn = "sForm"; break;
                    case "nAmount": sSortColumn = "nAmount"; break;
                    case "sRequester": sSortColumn = "sRequester"; break;
                    case "sWaitingBy": sSortColumn = "sWaitingBy"; break;
                    case "sStatus": sSortColumn = "sStatus"; break;
                    case "sRemark": sSortColumn = "sRemark"; break;
                    case "dUpdate": sSortColumn = "dUpdate"; break;
                }
                if (param.isASC)
                {
                    qry = qry.OrderBy<cDataTableWelfare>(sSortColumn).ToArray();
                }
                else if (param.isDESC)
                {
                    qry = qry.OrderByDescending<cDataTableWelfare>(sSortColumn).ToArray();
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
        public ResultAPI RemoveDataTable(cRemoveTable param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                if (param.lstID != null)
                {
                    foreach (var item in param.lstID)
                    {
                        var sMode = param.lstID.First().Split("_")[0];

                        var nID = item.Split("_")[1].DecryptParameter().ToInt();

                        if (sMode == "sAllowance")
                        {
                            var objAllowanceData = _db.TB_Request_WF_Allowance.Where(w => w.nRequestAllowanceID == nID).FirstOrDefault();
                            if (objAllowanceData != null)
                            {
                                objAllowanceData.IsDelete = true;
                                objAllowanceData.nDeleteBy = nUserID;
                                objAllowanceData.dDelete = DateTime.Now;
                            }
                        }
                        else if (sMode == "sTravel")
                        {
                            var objTravelData = _db.TB_Request_WF_TravelExpenses.Where(w => w.nRequestTravelExpensesID == nID).FirstOrDefault();
                            if (objTravelData != null)
                            {
                                objTravelData.IsDelete = true;
                                objTravelData.nDeleteBy = nUserID;
                                objTravelData.dDelete = DateTime.Now;
                            }
                        }
                        else if (sMode == "sDental")
                        {
                            var objDentalData = _db.TB_Request_WF_Dental.Where(w => w.nRequestDentalID == nID).FirstOrDefault();
                            if (objDentalData != null)
                            {
                                objDentalData.IsDelete = true;
                                objDentalData.nDeleteBy = nUserID;
                                objDentalData.dDelete = DateTime.Now;
                            }
                        }
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
        public ResultAPI SaveApprove(cReqApprove param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                TB_Request_WF_Allowance? objData = _db.TB_Request_WF_Allowance.Where(w => w.nRequestAllowanceID == param.sID.DecryptParameter().ToInt()).FirstOrDefault();

                if (objData != null)
                {
                    objData.nStatusID = param.nStatusID ?? 0;
                    objData.sComment = param.sComment;
                    objData.nUpdateBy = nUserID;
                    objData.dUpdate = DateTime.Now;

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
                    SaveLog(objData);

                    #region Send Line Noti
                    string sGUID = Guid.NewGuid().ToString();

                    var arrProject = _db.TB_Project.ToArray();
                    var arrStatus = _db.TM_Status.ToArray();

                    cParamSendLine objParam = new cParamSendLine();
                    objParam.sGUID = sGUID;
                    objParam.nRequesterID = nUserID;
                    objParam.sDate = objData.dUpdate.ToStringFromDate("dd/MM/yyyy");
                    objParam.sTime = objData.dUpdate.ToStringFromDate("HH:mm") + " น.";
                    objParam.sProject = arrProject.Where(w => w.nProjectID == objData.nProjectID).Select(s => s.sProjectName).FirstOrDefault() ?? "";
                    objParam.sDetailRequest = objData.sDescription ?? "";

                    if (objData.nAllowanceTypeID == 123) //ไป-กลับ
                    {
                        objParam.sStartDate = objData.dStartDate_StartTime.ToStringFromDate("dd/MM/yyyy");
                        objParam.sStartTime = objData.dStartDate_StartTime.ToStringFromDate("HH:mm") + " น.";
                        objParam.sEndDate = objData.dStartDate_EndTime.ToStringFromDate("dd/MM/yyyy");
                        objParam.sEndTime = objData.dStartDate_EndTime.ToStringFromDate("HH:mm") + " น.";
                    }
                    else //ค้างคืน
                    {
                        objParam.sStartDate = objData.dStartDate_StartTime.ToStringFromDate("dd/MM/yyyy");
                        objParam.sStartTime = objData.dStartDate_StartTime.ToStringFromDate("HH:mm") + " น.";
                        objParam.sEndDate = objData.dEndDate_EndTime.ToStringFromDate("dd/MM/yyyy");
                        objParam.sEndTime = objData.dEndDate_EndTime.ToStringFromDate("HH:mm") + " น.";
                    }

                    objParam.sMoney = $"{objData.nSumMoney:n}" ?? "0";
                    objParam.sDay = objData.nSumDay.ToString() ?? "0";

                    objParam.sRemark = objData.sComment ?? "";
                    objParam.sStatus = arrStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Allowance && w.nStatusID == objData.nStatusID).Select(s => s.sStatusName).FirstOrDefault() ?? "";
                    objParam.sPathDetail = "DetailAllowanceLine&sID=" + objData.nRequestAllowanceID.EncryptParameter() + "&sUserID=" + objData.nCreateBy.EncryptParameter();

                    switch (objData.nStatusID)
                    {
                        case 4: //Approved
                            objParam.nTemplateID = 22;
                            objParam.lstEmpTo = new List<int> { objData.nCreateBy };
                            break;
                        case 3: //Reject
                            objParam.nTemplateID = 24;
                            objParam.lstEmpTo = new List<int> { objData.nCreateBy };
                            break;
                        case 0: //Cancel
                            objParam.nTemplateID = 23;
                            objParam.lstEmpTo = new List<int> { objData.nCreateBy };
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
        public bool SaveLog(TB_Request_WF_Allowance data)
        {
            bool IsComplete = false;
            try
            {
                var objLog = _db.TB_Request_WF_Allowance_History.Where(w => w.nRequestAllowanceID == data.nRequestAllowanceID && data.nStatusID == 1 && w.nStatusID == 1).FirstOrDefault();

                bool isNew = false;
                if (objLog == null)
                {
                    objLog = new TB_Request_WF_Allowance_History();
                    isNew = true;
                }

                objLog.nRequestAllowanceID = data.nRequestAllowanceID;
                objLog.nRequestTypeID = data.nRequestTypeID;
                objLog.nEmployeeID = data.nEmployeeID;
                objLog.nProjectID = data.nProjectID;
                objLog.sDescription = data.sDescription;
                objLog.nAllowanceTypeID = data.nAllowanceTypeID;
                objLog.dStartDate_StartTime = data.dStartDate_StartTime;
                objLog.dStartDate_EndTime = data.dStartDate_EndTime;
                objLog.dEndDate_StartTime = data.dEndDate_StartTime;
                objLog.dEndDate_EndTime = data.dEndDate_EndTime;
                objLog.nSumDay = data.nSumDay;
                objLog.nSumMoney = data.nSumMoney;
                objLog.sComment = data.sComment;
                objLog.nStatusID = data.nStatusID;
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
                    int newID = _db.TB_Request_WF_Allowance_History.Any() ? _db.TB_Request_WF_Allowance_History.Select(s => s.nRequestAllowanceID_His).Max() + 1 : 1;
                    objLog.nRequestAllowanceID_His = newID;

                    _db.TB_Request_WF_Allowance_History.Add(objLog);
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
    }
}

