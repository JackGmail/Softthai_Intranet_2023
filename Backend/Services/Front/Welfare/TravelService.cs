using ST.INFRA;
using Extensions.Common.STExtension;
using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Backend.Models;
using Backend.Enum;
using Backend.Interfaces;
using Backend.Models.Authentication;
using static Extensions.Systems.AllClass;
using ResultAPI = Extensions.Common.STResultAPI.ResultAPI;
using Extensions.Common.STFunction;
using ST_API.Models;
namespace Backend.Services.ISystemService
{
    public class TravelService : ITravelService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _authen;
        private readonly IHostEnvironment _env;

        public TravelService(ST_IntranetEntity db, IAuthentication authen, IHostEnvironment env)
        {
            _db = db;
            _authen = authen;
            _env = env;
        }
        public cTravelData GetDataTravel(cReqDataAllowance param)
        {
            cTravelData result = new cTravelData();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                int nID = param.sID.DecryptParameter().ToInt();
                var objData = _db.TB_Request_WF_TravelExpenses.FirstOrDefault(f => f.nRequestTravelExpensesID == nID);
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
                    result.dMonthRequest = objData.dMonthRequest;
                    result.sComment = objData.sComment;
                    result.nStatusID = objData.nStatusID;
                    result.IsRequester = objData.nCreateBy == nUserID;
                    result.IsApprover = objUserHR != null ? objUserHR.nEmpID == nUserID : false;

                    //Public List
                    var lstPublicAll = _db.TB_Request_WF_PublicTransport.Where(w => w.nRequestTravelExpensesID == nID && !w.IsDelete).ToList();

                    var lstPublic = new List<objTravelRow>();
                    int nOrder = 1;
                    foreach (var item in lstPublicAll)
                    {
                        var objAdd = new objTravelRow();
                        objAdd.nID = item.nRequestPublicTransportID;
                        objAdd.sID = item.nRequestPublicTransportID + "";
                        objAdd.nNo = nOrder;
                        objAdd.dTravel = item.dDate;
                        objAdd.sProject = item.nProjectID + "";
                        objAdd.sVehicle = item.nVehicleType + "";
                        objAdd.objGo_Start = new objTravelChoice { sStartTypeID = item.nOriginDepartture + "", sOther = item.sOther1 };
                        objAdd.objGo_End = new objTravelChoice { sStartTypeID = item.nDestinationDepartture + "", sOther = item.sOther2 };
                        objAdd.objBack_Start = new objTravelChoice { sStartTypeID = item.nOriginReturn + "", sOther = item.sOther3 };
                        objAdd.objBack_End = new objTravelChoice { sStartTypeID = item.nDestinationReturn + "", sOther = item.sOther4 };
                        objAdd.nAmount = item.nMoney;
                        lstPublic.Add(objAdd);
                        nOrder++;
                    }
                    result.lstPublic = lstPublic;

                    //Private List
                    var lstPrivateAll = _db.TB_Request_WF_Privatecar.Where(w => w.nRequestTravelExpensesID == nID && !w.IsDelete).ToList();

                    var lstPrivate = new List<objTravelRow>();
                    nOrder = 1;
                    foreach (var item in lstPrivateAll)
                    {
                        var objAdd = new objTravelRow();
                        objAdd.nID = item.nRequestPrivatecarID;
                        objAdd.sID = item.nRequestPrivatecarID + "";
                        objAdd.nNo = nOrder;
                        objAdd.nUploadFileNo = nOrder;
                        objAdd.dTravel = item.dDate;
                        objAdd.sProject = item.nProjectID + "";
                        objAdd.objGo_Start = new objTravelChoice { sStartTypeID = item.nOriginDepartture + "", sOther = item.sOther1 };
                        objAdd.objGo_End = new objTravelChoice { sStartTypeID = item.nDestinationDepartture + "", sOther = item.sOther2 };
                        objAdd.objBack_Start = new objTravelChoice { sStartTypeID = item.nOriginReturn + "", sOther = item.sOther3 };
                        objAdd.objBack_End = new objTravelChoice { sStartTypeID = item.nDestinationReturn + "", sOther = item.sOther4 };
                        objAdd.nDistance = item.nDistance;
                        objAdd.nAmount = item.nMoney;

                        //List File
                        var lstFile = new List<ItemFileData>();
                        var arrFile = _db.TB_Request_WF_Privatecar_File.Where(w => !w.IsDelete && w.nRequestPrivatecarID == item.nRequestPrivatecarID).ToList();
                        foreach (var iF in arrFile)
                        {
                            var sPath = STFunction.GetPathUploadFile(iF.sPath, iF.sSystemFilename ?? "");
                            var objFile = new ItemFileData();
                            objFile.sFileName = iF.sFilename;
                            objFile.sCropFileLink = sPath;
                            objFile.sFileLink = sPath;
                            objFile.sSysFileName = iF.sSystemFilename;
                            objFile.sFileType = iF.sSystemFilename != null ? iF.sSystemFilename.Split(".")[1] + "" : null;
                            objFile.sFolderName = iF.sPath;
                            objFile.IsNew = false;
                            objFile.IsNewTab = false;
                            objFile.IsCompleted = true;
                            objFile.IsDelete = false;
                            objFile.IsProgress = false;
                            objFile.sProgress = "100";
                            lstFile.Add(objFile);
                        }

                        objAdd.arrFile = lstFile;
                        lstPrivate.Add(objAdd);
                        nOrder++;
                    }
                    result.lstPrivate = lstPrivate;

                    //History Log
                    var lstLogAll = _db.TB_Request_WF_TravelExpenses_History.Where(w => w.nRequestTravelExpensesID == nID).ToList();
                    var lstLog = new List<cHistoryLog>();
                    var lstStatus = _db.TM_Status.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Travel).ToArray();
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
        public ResultAPI SaveData(cTravelData param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                TB_Request_WF_TravelExpenses? objTravel = _db.TB_Request_WF_TravelExpenses.Where(w => w.nRequestTravelExpensesID == param.sID.DecryptParameter().ToInt()).FirstOrDefault();

                bool isNew = false;
                if (objTravel == null)
                {
                    objTravel = new TB_Request_WF_TravelExpenses();
                    isNew = true;
                }

                objTravel.nEmployeeID = nUserID;
                objTravel.dMonthRequest = param.dMonthRequest;
                objTravel.nRequestTypeID = (int)EnumGlobal.RequestFormType.Travel;
                objTravel.nStatusID = param.nStatusID;
                objTravel.dUpdate = DateTime.Now;
                objTravel.nUpdateBy = nUserID;
                objTravel.nTotalAmount = param.nTotalAmount ?? 0;
                objTravel.sComment = null;

                //New
                if (isNew)
                {
                    int newID = _db.TB_Request_WF_TravelExpenses.Any() ? _db.TB_Request_WF_TravelExpenses.Select(s => s.nRequestTravelExpensesID).Max() + 1 : 1;

                    objTravel.nRequestTravelExpensesID = newID;
                    objTravel.dCreate = DateTime.Now;
                    objTravel.nCreateBy = nUserID;
                    objTravel.IsDelete = false;

                    _db.TB_Request_WF_TravelExpenses.Add(objTravel);
                }
                else //Edit
                {
                }

                //Check Duplicate Public
                var lstTravelAll = _db.TB_Request_WF_TravelExpenses.ToList();
                if (param.lstPublic != null)
                {
                    var lstPublicAll = _db.TB_Request_WF_PublicTransport.ToList();
                    foreach (var item in param.lstPublic)
                    {
                        //เช็คว่า Row เหมือนกันไหม
                        var objDup = lstPublicAll.Where(w => w.dDate.Date == item.dTravel.Date && w.nProjectID == (item.sProject ?? "0").toInt()
                                            && w.nRequestTravelExpensesID != objTravel.nRequestTravelExpensesID && !w.IsDelete).FirstOrDefault();

                        if (objDup != null)
                        {
                            //เช็ค Status ของใบนั้น
                            var nStatusID = lstTravelAll.Where(w => w.nRequestTravelExpensesID == objDup.nRequestTravelExpensesID).Select(s => s.nStatusID).FirstOrDefault();
                            if (nStatusID != 0 && nStatusID != 1)
                            {
                                result.Status = StatusCodes.Status208AlreadyReported;
                                result.Message = "มีการขอเบิกค่าเดินทางรถสาธารณะ <br/>ในวันที่ และ โปรเจค ที่ท่านเลือกแล้ว";
                                return result;
                            }
                        }
                    }
                }

                //Check Duplicate Private
                if (param.lstPrivate != null)
                {
                    var lstPrivateAll = _db.TB_Request_WF_Privatecar.ToList();
                    foreach (var item in param.lstPrivate)
                    {
                        //เช็คว่า Row เหมือนกันไหม
                        var objDup = lstPrivateAll.Where(w => w.dDate.Date == item.dTravel.Date && w.nProjectID == (item.sProject ?? "0").toInt()
                                            && w.nRequestTravelExpensesID != objTravel.nRequestTravelExpensesID && !w.IsDelete).FirstOrDefault();

                        if (objDup != null)
                        {
                            //เช็ค Status ของใบนั้น
                            var nStatusID = lstTravelAll.Where(w => w.nRequestTravelExpensesID == objDup.nRequestTravelExpensesID).Select(s => s.nStatusID).FirstOrDefault();
                            if (nStatusID != 0 && nStatusID != 1)
                            {
                                result.Status = StatusCodes.Status208AlreadyReported;
                                result.Message = "มีการขอเบิกค่าเดินทางรถส่วนตัว <br/>ในวันที่ และ โปรเจค ที่ท่านเลือกแล้ว";
                                return result;
                            }
                        }
                    }
                }

                //set IsDelete old list
                var lstPublicOld = _db.TB_Request_WF_PublicTransport.Where(w => w.nRequestTravelExpensesID == objTravel.nRequestTravelExpensesID).ToArray();
                foreach (var item in lstPublicOld)
                {
                    item.IsDelete = true;
                    item.nDeleteBy = nUserID;
                    item.dDelete = DateTime.Now;
                }
                var lstPrivateOld = _db.TB_Request_WF_Privatecar.Where(w => w.nRequestTravelExpensesID == objTravel.nRequestTravelExpensesID).ToArray();
                foreach (var item in lstPrivateOld)
                {
                    item.IsDelete = true;
                    item.nDeleteBy = nUserID;
                    item.dDelete = DateTime.Now;
                }
                var lstPrivateFileOld = _db.TB_Request_WF_Privatecar_File.Where(w => lstPrivateOld.Select(s => s.nRequestPrivatecarID).ToList().Contains(w.nRequestPrivatecarID)).ToArray();
                foreach (var item in lstPrivateFileOld)
                {
                    item.IsDelete = true;
                    item.nDeleteBy = nUserID;
                    item.dDelete = DateTime.Now;
                }
                _db.SaveChanges();

                //Add new List Public
                int newPublicID = _db.TB_Request_WF_PublicTransport.Any() ? _db.TB_Request_WF_PublicTransport.Select(s => s.nRequestPublicTransportID).Max() + 1 : 1;
                int nOrder = 1;
                if (param.lstPublic != null)
                {
                    foreach (var item in param.lstPublic)
                    {
                        var objNew = new TB_Request_WF_PublicTransport();
                        objNew.nRequestPublicTransportID = newPublicID;
                        objNew.nRequestTravelExpensesID = objTravel.nRequestTravelExpensesID;
                        objNew.nOrder = nOrder;
                        objNew.dDate = item.dTravel;
                        objNew.nProjectID = (item.sProject ?? "").toInt();
                        objNew.nVehicleType = (item.sVehicle ?? "").toInt();
                        objNew.nOriginDepartture = (item.objGo_Start?.sStartTypeID ?? "").toInt();
                        objNew.sOther1 = item.objGo_Start?.sOther;
                        objNew.nDestinationDepartture = (item.objGo_End?.sStartTypeID ?? "").toInt();
                        objNew.sOther2 = item.objGo_End?.sOther;
                        objNew.nOriginReturn = (item.objBack_Start?.sStartTypeID ?? "").toInt();
                        objNew.sOther3 = item.objBack_Start?.sOther;
                        objNew.nDestinationReturn = (item.objBack_End?.sStartTypeID ?? "").toInt();
                        objNew.sOther4 = item.objBack_End?.sOther;
                        objNew.nMoney = item.nAmount ?? 0;
                        objNew.dCreate = DateTime.Now;
                        objNew.nCreateBy = nUserID;
                        objNew.dUpdate = DateTime.Now;
                        objNew.nUpdateBy = nUserID;
                        objNew.IsDelete = false;
                        _db.TB_Request_WF_PublicTransport.Add(objNew);

                        nOrder++;
                        newPublicID++;
                    }
                }


                //Add new List Private
                int newPrivateID = _db.TB_Request_WF_Privatecar.Any() ? _db.TB_Request_WF_Privatecar.Select(s => s.nRequestPrivatecarID).Max() + 1 : 1;
                nOrder = 1;
                int nFileID = _db.TB_Request_WF_Privatecar_File.Any() ? _db.TB_Request_WF_Privatecar_File.Select(s => s.nRequestFileID).Max() + 1 : 1;
                if (param.lstPrivate != null)
                {
                    foreach (var item in param.lstPrivate)
                    {
                        var objNew = new TB_Request_WF_Privatecar();
                        objNew.nRequestPrivatecarID = newPrivateID;
                        objNew.nRequestTravelExpensesID = objTravel.nRequestTravelExpensesID;
                        objNew.nOrder = nOrder;
                        objNew.dDate = item.dTravel;
                        objNew.nProjectID = (item.sProject ?? "").toInt();
                        objNew.nOriginDepartture = (item.objGo_Start?.sStartTypeID ?? "").toInt();
                        objNew.sOther1 = item.objGo_Start?.sOther;
                        objNew.nDestinationDepartture = (item.objGo_End?.sStartTypeID ?? "").toInt();
                        objNew.sOther2 = item.objGo_End?.sOther;
                        objNew.nOriginReturn = (item.objBack_Start?.sStartTypeID ?? "").toInt();
                        objNew.sOther3 = item.objBack_Start?.sOther;
                        objNew.nDestinationReturn = (item.objBack_End?.sStartTypeID ?? "").toInt();
                        objNew.sOther4 = item.objBack_End?.sOther;
                        objNew.nDistance = item.nDistance ?? 0;
                        objNew.nMoney = item.nAmount ?? 0;
                        objNew.dCreate = DateTime.Now;
                        objNew.nCreateBy = nUserID;
                        objNew.dUpdate = DateTime.Now;
                        objNew.nUpdateBy = nUserID;
                        objNew.IsDelete = false;
                        _db.TB_Request_WF_Privatecar.Add(objNew);

                        //Add List File
                        if (item.arrFile != null)
                        {
                            foreach (var iF in item.arrFile)
                            {
                                string pathTempContent = !string.IsNullOrEmpty(iF.sFolderName) ? iF.sFolderName + "/" : "Temp/";
                                string truePathFile = "Travel/";

                                STFunction.MoveFile(pathTempContent, truePathFile, iF.sSysFileName != null ? iF.sSysFileName : "", _env);

                                TB_Request_WF_Privatecar_File objNewFile = new TB_Request_WF_Privatecar_File()
                                {
                                    nUpdateBy = nUserID,
                                    nCreateBy = nUserID,
                                    dCreate = DateTime.Now,
                                    dUpdate = DateTime.Now,
                                    nRequestPrivatecarID = objNew.nRequestPrivatecarID,
                                    sPath = "Travel",
                                    sFilename = iF.sFileName + "",
                                    sSystemFilename = iF.sSysFileName + "",
                                    nRequestFileID = nFileID
                                };
                                nFileID++;
                                _db.TB_Request_WF_Privatecar_File.Add(objNewFile);
                            }
                        }

                        nOrder++;
                        newPrivateID++;
                    }
                }
                _db.SaveChanges();

                SaveLog(objTravel);

                if (objTravel.nStatusID != (int)EnumGlobal.StatusRequestWelfare.Draft)
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
                    objParam.sDate = objTravel.dUpdate.ToStringFromDate("dd/MM/yyyy");
                    objParam.sTime = objTravel.dUpdate.ToStringFromDate("HH:mm") + " น.";
                    objParam.sMonth = objTravel.dMonthRequest != null ? GetMonthTH(objTravel.dMonthRequest.Value.Month) : "";
                    objParam.sMoney = $"{objTravel.nTotalAmount:n}" ?? "0";

                    var lstHR = string.Join(",", lstUserHR.Select(s => s.nEmpID)) + "";

                    objParam.sStatus = arrStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Travel && w.nStatusID == objTravel.nStatusID).Select(s => s.sStatusName).FirstOrDefault() ?? "";
                    objParam.sPathApprove = "ApproveTravelLine&sID=" + objTravel.nRequestTravelExpensesID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestWelfare.Completed) + "&sGUID=" + sGUID;
                    objParam.sPathReject = "RejectTravelLine&sID=" + objTravel.nRequestTravelExpensesID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestOT.Reject) + "&sGUID=" + sGUID;
                    objParam.sPathCancel = "RejectTravelLine&sID=" + objTravel.nRequestTravelExpensesID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestOT.Canceled) + "&sGUID=" + sGUID;
                    objParam.sPathDetail = "DetailTravelLine&sID=" + objTravel.nRequestTravelExpensesID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter();

                    switch (objTravel.nStatusID)
                    {
                        case 2: //Wait for Approve
                            objParam.nTemplateID = 25;
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
        public ResultAPI SaveApprove(cReqApprove param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                TB_Request_WF_TravelExpenses? objData = _db.TB_Request_WF_TravelExpenses.Where(w => w.nRequestTravelExpensesID == param.sID.DecryptParameter().ToInt()).FirstOrDefault();

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
                    objParam.sDate = objData.dUpdate.ToStringFromDate("dd/MM/yyyy");
                    objParam.sTime = objData.dUpdate.ToStringFromDate("HH:mm") + " น.";
                    objParam.sMonth = objData.dMonthRequest != null ? GetMonthTH(objData.dMonthRequest.Value.Month) : "";
                    objParam.sMoney = $"{objData.nTotalAmount:n}" ?? "0";
                    objParam.sRemark = objData.sComment ?? "";

                    objParam.sStatus = arrStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Travel && w.nStatusID == objData.nStatusID).Select(s => s.sStatusName).FirstOrDefault() ?? "";
                    objParam.sPathDetail = "DetailTravelLine&sID=" + objData.nRequestTravelExpensesID.EncryptParameter() + "&sUserID=" + objData.nCreateBy.EncryptParameter();

                    switch (objData.nStatusID)
                    {
                        case 4: //Approved
                            objParam.nTemplateID = 26;
                            objParam.lstEmpTo = new List<int> { objData.nCreateBy };
                            break;
                        case 3: //Reject
                            objParam.nTemplateID = 28;
                            objParam.lstEmpTo = new List<int> { objData.nCreateBy };
                            break;
                        case 0: //Cancel
                            objParam.nTemplateID = 27;
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
        public bool SaveLog(TB_Request_WF_TravelExpenses data)
        {
            bool IsComplete = false;
            try
            {
                var objLog = _db.TB_Request_WF_TravelExpenses_History.Where(w => w.nRequestTravelExpensesID == data.nRequestTravelExpensesID && data.nStatusID == 1 && w.nStatusID == 1).FirstOrDefault();

                bool isNew = false;
                if (objLog == null)
                {
                    objLog = new TB_Request_WF_TravelExpenses_History();
                    isNew = true;
                }

                objLog.nRequestTravelExpensesID = data.nRequestTravelExpensesID;
                objLog.nRequestTypeID = data.nRequestTypeID;
                objLog.dMonthRequest = data.dMonthRequest;
                objLog.nEmployeeID = data.nEmployeeID;
                objLog.nStatusID = data.nStatusID;
                objLog.sComment = data.sComment;
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
                    int newID = _db.TB_Request_WF_TravelExpenses_History.Any() ? _db.TB_Request_WF_TravelExpenses_History.Select(s => s.nRequestTravelExpensesID_His).Max() + 1 : 1;
                    objLog.nRequestTravelExpensesID_His = newID;

                    _db.TB_Request_WF_TravelExpenses_History.Add(objLog);
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
        public string GetMonthTH(int? nMonth)
        {
            string sMonthTH;
            switch (nMonth)
            {
                case 1: sMonthTH = "มกราคม"; break;
                case 2: sMonthTH = "กุมภาพันธ์"; break;
                case 3: sMonthTH = "มีนาคม"; break;
                case 4: sMonthTH = "เมษายน"; break;
                case 5: sMonthTH = "พฤษภาคม"; break;
                case 6: sMonthTH = "มิถุนายน"; break;
                case 7: sMonthTH = "กรกฎาคม"; break;
                case 8: sMonthTH = "สิงหาคม"; break;
                case 9: sMonthTH = "กันยายน"; break;
                case 10: sMonthTH = "ตุลาคม"; break;
                case 11: sMonthTH = "พฤศจิกายน"; break;
                case 12: sMonthTH = "ธันวาคม"; break;
                default: sMonthTH = ""; break;
            }
            return sMonthTH;
        }
    }
}

