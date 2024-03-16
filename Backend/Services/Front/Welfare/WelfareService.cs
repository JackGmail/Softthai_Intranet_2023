using ST.INFRA;
using ST.INFRA.Common;
using Extensions.Common.STExtension;
using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Backend.Models;
using Backend.Enum;
using Backend.Interfaces;
using Backend.Models.Authentication;
using Extensions.Common.STFunction;
using static Extensions.Systems.AllClass;
using ST.INFRA.Utility;
using ST_API.Models;
using Microsoft.EntityFrameworkCore;
using static Backend.Enum.EnumGlobal;
namespace Backend.Services.ISystemService
{
    public class WelfareService : IWelfareService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _authen;
        private readonly IHostEnvironment _env;
        private readonly ParamJWT _user;

        public WelfareService(ST_IntranetEntity db, IAuthentication authen, IHostEnvironment env)
        {
            _db = db;
            _authen = authen;
            _env = env;
            _user = _authen.GetUserAccount();
        }

        public cDentalResult SelectDentalType()
        {
            cDentalResult result = new cDentalResult();
            try
            {
                DateTime dNow = DateTime.Now;

                #region default amount/job year
                DateTime dWorkExperience = dNow;
                var checkEmp = _db.TB_Employee.FirstOrDefault(w => w.IsActive && !w.IsDelete && !w.IsRetire);
                var dEmployeeStartWork = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == _user.nUserID && w.IsActive && !w.IsDelete);

                if (dEmployeeStartWork != null)
                {
                    dWorkExperience = dEmployeeStartWork.dWorkStart ?? default;
                    result.sRequestName = dEmployeeStartWork.sNameTH + " " + dEmployeeStartWork.sSurnameTH + " (" + dEmployeeStartWork.sNickname + ")";
                    result.sEmpName = dEmployeeStartWork.sNameTH + " " + dEmployeeStartWork.sSurnameTH;
                }
                else
                {
                    result.Status = StatusCodes.Status409Conflict;
                    result.Message = "ไม่พบผู้ใช้";
                }

                TimeSpan dWorkYear = dNow.Subtract(dWorkExperience);

                int nWorkYear = new DateTime(dWorkYear.Ticks).Year;
                #endregion

                #region selcet dental type
                var optDentalType = _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.DentalType && w.IsActive && !w.IsDelete).Select(s => new cOptionSelect
                {
                    value = s.nData_ID.ToString(),
                    label = s.sNameTH ?? ""
                }).ToList();

                if (optDentalType != null)
                {
                    result.lstData = optDentalType;
                }
                #endregion

                // TB_Employee_Dental_Year
                int? nRemainsMoney = 0; //nRemain or moneyBalance = ยอดคงเหลือ
                int? nCondition = 0; //จำนวนเงินที่เบิกได้/ปี

                #region จำนวนเงินที่เบิกได้/ปี
                TM_Config? objConfig_MoreThreeYear = _db.TM_Config.FirstOrDefault(w => w.nConfigID == ((int)Config.WorkYear_ThreeYear));
                TM_Config? objConfig_UnderThreeYear = _db.TM_Config.FirstOrDefault(w => w.nConfigID == ((int)Config.WorkYear_UnderThreeYear));

                if (nWorkYear < 3)
                {
                    if (objConfig_UnderThreeYear != null)
                    {

                        nCondition = Decimal.ToInt32(objConfig_UnderThreeYear.nValue ?? 0);
                    }
                }
                else
                {
                    if (objConfig_MoreThreeYear != null)
                    {
                        nCondition = Decimal.ToInt32(objConfig_MoreThreeYear.nValue ?? 0);
                    }
                }
                #endregion

                #region remain money 
                List<TB_Request_WF_Dental> lstRequestDental = _db.TB_Request_WF_Dental.Where(w => w.nEmployeeID == _user.nUserID && w.nStatus != 3 && !w.IsDelete).ToList();
                List<TB_Employee_Dental_Year> lstDentalYear = _db.TB_Employee_Dental_Year.Where(w => w.nEmpID == _user.nUserID && !w.IsDelete).ToList();

                if (lstRequestDental.Count() != 0)
                {
                    var objDentalRemainPerYear = lstDentalYear.FirstOrDefault(w => w.nYear == dNow.Year);
                    if (objDentalRemainPerYear != null)
                    {
                        nRemainsMoney = objDentalRemainPerYear.nMoney;
                    }
                }
                else
                {
                    nRemainsMoney = nCondition;
                }
                #endregion

                result.nRemainsMoney = nRemainsMoney;
                result.nConditionAmount = nCondition;

                #region position
                var empPosition = _db.TB_Employee_Position.FirstOrDefault(w => w.nEmployeeID == _user.nUserID);
                if (empPosition != null)
                {
                    var positionName = _db.TB_Position.FirstOrDefault(w => w.nPositionID == empPosition.nPositionID);
                    if (positionName != null)
                    {
                        result.sEmpPosition = positionName.sPositionName;
                    }
                }
                #endregion

                #region HR
                var objUserHR = (from a in _db.TB_Employee_Position.Where(w => w.nPositionID == (int)EnumEmployee.PositionID.HR)
                                 join b in _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete) on a.nEmployeeID equals b.nEmployeeID
                                 join c in _db.TB_Position on a.nPositionID equals c.nPositionID
                                 select new cobjHR
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

        public cDentalTableResult GetData_Dental(cDentalTable obj)
        {
            cDentalTableResult result = new cDentalTableResult();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                DateTime dNow = DateTime.Now;
                int nYear = dNow.Year;

                var lstDentalRequest = (from requestTable in _db.TB_Request_WF_Dental.Where(w => w.nEmployeeID == _user.nUserID && !w.IsDelete)
                                        join statusTable in _db.TM_Status.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Dental) on requestTable.nStatus equals statusTable.nStatusID
                                        join empTable in _db.TB_Employee on requestTable.nEmployeeID equals empTable.nEmployeeID
                                        join empPosition in _db.TB_Employee_Position on empTable.nEmployeeID equals empPosition.nEmployeeID
                                        join position in _db.TB_Position on empPosition.nPositionID equals position.nPositionID

                                        select new cDentalDetail
                                        {
                                            sID = requestTable.nRequestDentalID.ToString(),
                                            nEmpID = empTable.nEmployeeID,
                                            sFullNameTH = empTable.sNameTH + " " + empTable.sSurnameTH,
                                            sPositionTH = position.sPositionName,
                                            sStatusTH = statusTable.sStatusName
                                        }
                      ).ToList();

                #region requested name
                int? nStatus = 0;
                string? sRequestName = null;
                int? nPositionID = 0;
                string? sPositionName = "-";
                string? sEmployee = null;

                #region selcet dental type
                var optDentalType = _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.DentalType && w.IsActive && !w.IsDelete).Select(s => new cOptionSelect
                {
                    value = s.nData_ID.ToString(),
                    label = s.sNameTH ?? ""
                }).ToList();
                #endregion

                List<TB_Request_WF_Dental_Type> objDentalType = _db.TB_Request_WF_Dental_Type.Where(w => w.nRequestDentalID == obj.sID.DecryptParameter().ToInt()).ToList();

                #region position
                var objPosition = _db.TB_Employee_Position.FirstOrDefault(w => w.nEmployeeID == _user.nUserID);
                if (objPosition != null)
                {
                    //show all position
                    // List<string> lstPosition = new List<string>();

                    nPositionID = objPosition.nPositionID;
                    //find position name
                    var positionName = _db.TB_Position.FirstOrDefault(w => w.nPositionID == nPositionID);
                    if (positionName != null)
                    {
                        sPositionName = positionName.sPositionName;
                    }
                }
                #endregion

                TB_Employee? objEmployee = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == _user.nUserID);
                if (objEmployee != null)
                {
                    sEmployee = objEmployee.sNameTH + " " + objEmployee.sSurnameTH + " (" + objEmployee.sNickname + ")";
                }
                string[] arrDentType = objDentalType.Select(s => s.nDentalTypeID.ToString()).ToArray();

                var objDentalRequest = _db.TB_Request_WF_Dental.FirstOrDefault(w => w.nRequestDentalID == obj.sID.DecryptParameter().ToInt() && !w.IsDelete);
                var objDentalDetail = new cDentalDetail();
                if (objDentalRequest != null)
                {
                    objDentalDetail.sID = objDentalRequest.nRequestDentalID.EncryptParameter();
                    objDentalDetail.nEmpID = objDentalRequest.nEmployeeID;
                    objDentalDetail.dDentalService = objDentalRequest.dCreate;
                    objDentalDetail.nMoneyBalance = objDentalRequest.nRemain;
                    objDentalDetail.nRequest = objDentalRequest.nAmountWithdrawn;
                    objDentalDetail.nTotal = objDentalRequest.nMoney;
                    objDentalDetail.nConditionAmount = objDentalRequest.nConditionAmount;
                    objDentalDetail.sHospitalName = objDentalRequest.sMedicalFacility;
                    objDentalDetail.sName = sEmployee;
                    objDentalDetail.sPosition = sPositionName;
                    objDentalDetail.nRequestType = objDentalRequest.nRequestTypeID;
                    objDentalDetail.nStatusID = objDentalRequest.nStatus;

                    objDentalDetail.sDentalType = arrDentType;
                }
                #endregion

                var Image = _db.TB_WF_Dental_File.FirstOrDefault(w => w.nRequestDentalID == obj.sID.DecryptParameter().ToInt() && w.IsDelete == "no");
                List<ItemFileData> lstfile = new List<ItemFileData>();
                if (Image != null)
                {
                    ItemFileData fFile = new ItemFileData();
                    var sPath = GetPathUploadFile(Image.sPath ?? "", Image.sSystemFilename ?? "");
                    fFile.sFileName = Image.sFilename;
                    fFile.sCropFileLink = sPath;
                    fFile.sFileLink = sPath;
                    fFile.sSysFileName = Image.sSystemFilename;
                    fFile.sFileType = Image.sSystemFilename != null ? Image.sSystemFilename.Split(".")[1] + "" : null;
                    fFile.sFolderName = Image.sPath;
                    fFile.IsDelete = false;
                    lstfile.Add(fFile);

                    result.fFileImage = lstfile;
                }

                #region history log
                var lstEmployee = _db.TB_Employee.AsNoTracking().ToArray();
                var lstMedRequest = _db.TB_Request_WF_Dental_History.Where(w => w.nRequestDentalID == obj.sID.DecryptParameter().ToInt() && w.nUpdateBy != 0 && !w.IsDelete).ToList();
                var lstStatus = _db.TM_Status.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Allowance).AsNoTracking().ToArray();
                var lstEmpPosition = _db.TB_Employee_Position.OrderBy(o => o.nLevelPosition).AsNoTracking().ToArray();
                var lstPosition = _db.TB_Position.AsNoTracking().ToArray();

                var lstLog = new List<cMedRequestHistoryLog>();
                foreach (var item in lstMedRequest)
                {
                    var objEmp = lstEmployee.FirstOrDefault(w => w.nEmployeeID == item.nUpdateBy);
                    if (objEmp != null)
                    {
                        var position = (from a in lstEmpPosition.Where(w => w.nEmployeeID == objEmp.nEmployeeID)
                                        join b in lstPosition on a.nPositionID equals b.nPositionID
                                        select b.sPositionName).FirstOrDefault();

                        lstLog.Add(new cMedRequestHistoryLog
                        {
                            sName = $"{objEmp.sNameTH} {objEmp.sSurnameTH} ({objEmp.sNickname})",
                            sPosition = position ?? "-",
                            sComment = item.sComent ?? "-",
                            sStatus = lstStatus.Where(w => w.nStatusID == item.nStatus).Select(s => s.sStatusName).FirstOrDefault() + "",
                            sActionMode = item.nStatus == (int)EnumGlobal.StatusRequestWelfare.Canceled ? "Cancel" :
                                item.nStatus == (int)EnumGlobal.StatusRequestWelfare.WaitingApprove ? "Submit" :
                                    item.nStatus == (int)EnumGlobal.StatusRequestWelfare.Reject ? "Reject" :
                                        item.nStatus == (int)EnumGlobal.StatusRequestWelfare.Completed ? "Approve" :
                                            lstStatus.Where(w => w.nStatusID == item.nStatus).Select(s => s.sStatusName).FirstOrDefault() + "",
                        });
                    }
                }

                result.lstHistoryLog = lstLog;
                #endregion

                if (objDentalRequest != null && objDentalRequest.nEmployeeID != _user.nUserID)
                {
                    result.IsHR = nPositionID == (int)EnumEmployee.PositionID.HR ? true : false;
                }

                result.sRequestName = sRequestName;
                result.nStatus = nStatus;

                result.objEdit = objDentalDetail;
                result.lstDentalRequest = lstDentalRequest;
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        public cDentalResult SaveData_DentalRequest(cDentalDetail objSave)
        {
            cDentalResult result = new cDentalResult();
            try
            {
                DateTime dNow = DateTime.Now;
                int nYear = dNow.Year;

                int nEmpID = 0;
                TB_Employee? objEmployee = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == _user.nUserID);
                if (objEmployee != null)
                {
                    nEmpID = objEmployee.nEmployeeID;
                }

                TB_Request_WF_Dental? objDentalRequest = _db.TB_Request_WF_Dental.FirstOrDefault(w => w.nRequestDentalID == objSave.sID.DecryptParameter().ToInt());

                //Action From Line
                if (objSave.IsActionFromLine == true && objDentalRequest != null)
                {
                    TB_Log_WebhookLine? objLogLine = _db.TB_Log_WebhookLine.Where(w => w.sGUID == objSave.sGUID).FirstOrDefault();
                    if (objLogLine != null)
                    {
                        objDentalRequest.dUpdate = DateTime.Now;
                        objDentalRequest.nUpdateBy = nEmpID;
                        objDentalRequest.nStatus = objSave?.nStatusID ?? 1;
                        objDentalRequest.sComent = objSave?.sComment ?? "";
                        objLogLine.IsActionAlready = true;
                        _db.SaveChanges();
                    }
                }
                else
                {
                    #region save
                    if (objDentalRequest == null)
                    {
                        objDentalRequest = new TB_Request_WF_Dental();
                        objDentalRequest.nRequestDentalID = (_db.TB_Request_WF_Dental.Any() ? _db.TB_Request_WF_Dental.Max(m => m.nRequestDentalID) : 0) + 1;
                        objDentalRequest.dCreate = dNow;
                        objDentalRequest.nCreateBy = nEmpID;
                        _db.TB_Request_WF_Dental.Add(objDentalRequest);
                    }
                    objDentalRequest.nRequestTypeID = objSave.nRequestType ?? 5;
                    objDentalRequest.nEmployeeID = objDentalRequest.nCreateBy;
                    objDentalRequest.dDate = objSave.dDentalService != null ? objSave.dDentalService.Value.AddDays(1) : DateTime.Now;
                    objDentalRequest.sMedicalFacility = objSave.sHospitalName ?? "";
                    objDentalRequest.nMoney = objSave.nTotal ?? 0;
                    objDentalRequest.nConditionAmount = objSave.nConditionAmount ?? 0;
                    objDentalRequest.nAmountWithdrawn = objSave.nRequest ?? 0;
                    objDentalRequest.nRemain = objSave.nMoneyBalance ?? 0;
                    objDentalRequest.nStatus = objSave.nStatusID ?? 1;
                    objDentalRequest.dUpdate = dNow;
                    objDentalRequest.nUpdateBy = nEmpID;
                    objDentalRequest.IsDelete = false;
                    objDentalRequest.sComent = objSave.sComment;

                    foreach (var sDentalType in objSave.sDentalType)
                    {
                        TB_Request_WF_Dental_Type? objDentalType = _db.TB_Request_WF_Dental_Type.FirstOrDefault(w => w.nRequestDentalID == objSave.sID.DecryptParameter().ToInt() && w.nDentalTypeID == sDentalType.ToInt());
                        if (objDentalType == null)
                        {
                            objDentalType = new TB_Request_WF_Dental_Type();
                            objDentalType.nRequestDentalTypeID = (_db.TB_Request_WF_Dental_Type.Any() ? _db.TB_Request_WF_Dental_Type.Max(m => m.nRequestDentalTypeID) : 0) + 1;
                            _db.TB_Request_WF_Dental_Type.Add(objDentalType);
                        }
                        objDentalType.nRequestDentalID = objDentalRequest.nRequestDentalID; //objSave.sID.DecryptParameter().ToInt();
                        objDentalType.nDentalTypeID = sDentalType.ToInt();
                        objDentalType.nQuantity = sDentalType.ToInt() == 136 ? objSave.nCheck1 : sDentalType.ToInt() == 137 ? objSave.nCheck2 : sDentalType.ToInt() == 138 ? objSave.nCheck3 : 0; //จำนวนซี่ฟัน
                        objDentalType.sOther = sDentalType.ToInt() == 139 ? objSave.sDentalTypeName : null;

                        _db.SaveChanges();
                    }
                    #endregion

                    #region TB_Employee_Dental_Year
                    TB_Employee_Dental_Year? objDentalPerYear = _db.TB_Employee_Dental_Year.FirstOrDefault(w => w.nEmpID == objDentalRequest.nEmployeeID);
                    if (objDentalPerYear == null)
                    {
                        objDentalPerYear = new TB_Employee_Dental_Year();
                        objDentalPerYear.nEmpDentalID = (_db.TB_Employee_Dental_Year.Any() ? _db.TB_Employee_Dental_Year.Max(m => m.nEmpDentalID) : 0) + 1;
                        objDentalPerYear.nEmpID = nEmpID;
                        objDentalPerYear.dCreateDate = dNow;
                        objDentalPerYear.nCreateBy = nEmpID;
                        _db.TB_Employee_Dental_Year.Add(objDentalPerYear);
                    }
                    objDentalPerYear.nMoney = Decimal.ToInt32(objSave.nMoneyBalance ?? 0);
                    objDentalPerYear.IsDelete = false;
                    objDentalPerYear.dUpdateDate = dNow;
                    objDentalPerYear.nUpdateBy = nEmpID;
                    if (objDentalPerYear.nEmpID == nEmpID && objDentalPerYear.nYear == 0)
                    {
                        objDentalPerYear.nYear = nYear;
                    }
                    _db.SaveChanges();
                    #endregion

                    #region File
                    var objAll = objSave.fFile?.FirstOrDefault();
                    if (objAll != null)
                    {
                        string pathTempContent = !string.IsNullOrEmpty(objAll.sFolderName) ? objAll.sFolderName + "/" : "Temp/Leave/";
                        string truePathFile = "Dental\\" + objDentalRequest.nRequestDentalID + "\\";
                        string fPathContent = "Dental/" + objDentalRequest.nRequestDentalID;

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

                        int fileID = _db.TB_WF_Dental_File.Any() ? _db.TB_WF_Dental_File.Max(m => m.nDentalFileID) + 1 : 1;
                        TB_WF_Dental_File objInfofile = new TB_WF_Dental_File()
                        {
                            nDentalFileID = fileID,
                            nRequestDentalID = objDentalRequest.nRequestDentalID,
                            sPath = fPathContent,
                            sSystemFilename = objAll.sSysFileName != null ? objAll.sSysFileName : "",
                            sFilename = objAll.sSysFileName != null ? objAll.sSysFileName : "",
                            nOrder = (_db.TB_WF_Dental_File.Any() ? _db.TB_WF_Dental_File.Max(m => m.nOrder) : 0) + 1,
                            dCreate = dNow.ToString("dd/MM/yyy"),
                            nCreateBy = objDentalRequest.nCreateBy.ToString(),
                            dUpdate = dNow.ToString("dd/MM/yyy"),
                            nUpdateBy = objDentalRequest.nCreateBy.ToString(),
                            IsDelete = "no",
                        };
                        _db.TB_WF_Dental_File.Add(objInfofile);
                    }
                    #endregion
                }

                #region log
                foreach (var type in objSave.sDentalType)
                {
                    TB_Request_WF_Dental_History objDentalLog = new TB_Request_WF_Dental_History()
                    {
                        nRequestDentalID_His = (_db.TB_Request_WF_Dental_History.Any() ? _db.TB_Request_WF_Dental_History.Max(m => m.nRequestDentalID_His) : 0) + 1,
                        nRequestDentalID = objDentalRequest.nRequestDentalID,
                        nRequestTypeID = objSave.nRequestType ?? 5,//objSave.sID.DecryptParameter().ToInt(),
                        nEmployeeID = nEmpID,
                        dDate = objSave.dDentalService ?? dNow,
                        sMedicalFacility = objSave.sHospitalName ?? "",
                        nDentalTypeID = type.ToInt(),
                        sOther = type.ToInt() == (int)EnumGlobal.MasterData.OtherDental ? objSave.sDentalTypeName : null,
                        nMoney = objSave.nTotal ?? 0,
                        nConditionAmount = objSave.nConditionAmount ?? 0,
                        nAmountWithdrawn = objSave.nRequest ?? 0,
                        nRemain = objSave.nMoneyBalance ?? 0,
                        nStatus = objSave.nStatusID ?? 0,
                        sComent = objSave.sComment,
                        dCreate = dNow,
                        nCreateBy = nEmpID,
                        dUpdate = dNow,
                        nUpdateBy = nEmpID,
                        IsDelete = false,
                    };
                    _db.TB_Request_WF_Dental_History.Add(objDentalLog);
                    _db.SaveChanges();
                }
                #endregion

                _db.SaveChanges();

                if (objSave.nStatusID != (int)EnumGlobal.StatusRequestWelfare.Draft)
                {
                    #region Send Line Noti
                    string sGUID = Guid.NewGuid().ToString();

                    var arrStatus = _db.TM_Status.ToArray();
                    var lstUserHR = (from a in _db.TB_Employee_Position.Where(w => w.nPositionID == (int)EnumEmployee.PositionID.HR)
                                     join b in _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete) on a.nEmployeeID equals b.nEmployeeID
                                     select new
                                     {
                                         nEmpID = b.nEmployeeID,
                                     }
                     ).ToList();

                    cParamSendLine objParam = new cParamSendLine();
                    objParam.sGUID = sGUID;
                    objParam.nRequesterID = nEmpID;
                    objParam.sDate = objDentalRequest.dUpdate.ToStringFromDate("dd/MM/yyyy");
                    objParam.sTime = objDentalRequest.dUpdate.ToStringFromDate("HH:mm") + " น.";
                    objParam.sStartDate = objDentalRequest.dDate.ToStringFromDate("dd/MM/yyyy");
                    objParam.sHospital = objDentalRequest.sMedicalFacility ?? "";

                    objParam.sMoney = $"{objDentalRequest.nAmountWithdrawn:n}" ?? "0";

                    var lstHR = string.Join(",", lstUserHR.Select(s => s.nEmpID)) + "";

                    objParam.sStatus = arrStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Allowance && w.nStatusID == objDentalRequest.nStatus).Select(s => s.sStatusName).FirstOrDefault() ?? "";
                    objParam.sPathApprove = "ApproveDentalLine&sID=" + objDentalRequest.nRequestDentalID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestWelfare.Completed) + "&sGUID=" + sGUID;
                    objParam.sPathReject = "RejectDentalLine&sID=" + objDentalRequest.nRequestDentalID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestOT.Reject) + "&sGUID=" + sGUID;
                    objParam.sPathCancel = "RejectDentalLine&sID=" + objDentalRequest.nRequestDentalID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter() + "&nStatusID=" + ((int)EnumGlobal.StatusRequestOT.Canceled) + "&sGUID=" + sGUID;
                    objParam.sPathDetail = "DetailDentalLine&sID=" + objDentalRequest.nRequestDentalID.EncryptParameter() + "&sUserID=" + string.Join(",", lstHR).EncryptParameter();

                    switch (objDentalRequest.nStatus)
                    {
                        case 2: //Wait for Approve
                            objParam.nTemplateID = 29;
                            objParam.lstEmpTo = lstUserHR.Select(s => s.nEmpID).ToList();
                            break;
                        case 3: //Reject
                            objParam.sRemark = objDentalRequest.sComent ?? "";
                            objParam.nTemplateID = 32;
                            objParam.lstEmpTo = new List<int> { objDentalRequest.nCreateBy };
                            break;
                        case 0: //Cancel
                            objParam.sRemark = objDentalRequest.sComent ?? "";
                            objParam.nTemplateID = 31;
                            objParam.lstEmpTo = new List<int> { objDentalRequest.nCreateBy };
                            break;
                        case 4: //Approve
                            objParam.nTemplateID = 30;
                            objParam.lstEmpTo = new List<int> { objDentalRequest.nCreateBy };
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

        public ResultAPI SaveApprove(cReqWelfareApprove param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                TB_Request_WF_Dental? objData = _db.TB_Request_WF_Dental.FirstOrDefault(w => w.nRequestDentalID == param.sID.DecryptParameter().ToInt());
                if (objData != null)
                {
                    objData.nStatus = param.nStatusID ?? 0;
                    objData.sComent = param.sComment;
                    objData.nUpdateBy = nUserID;
                    objData.dUpdate = DateTime.Now;

                    //Action From Line
                    if (param.IsActionFromLine == true)
                    {
                        TB_Log_WebhookLine? objLog = _db.TB_Log_WebhookLine.FirstOrDefault(w => w.sGUID == param.sGUID);
                        if (objLog != null)
                        {
                            objLog.IsActionAlready = true;
                        }
                    }

                    _db.SaveChanges();

                    #region history log
                    var lstEmployee = _db.TB_Employee.AsNoTracking().ToArray();
                    var lstMedRequest = _db.TB_Request_WF_Dental_History.Where(w => w.nRequestDentalID == param.sID.DecryptParameter().ToInt() && w.nUpdateBy != 0 && !w.IsDelete).ToList();
                    var lstStatus = _db.TM_Status.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Allowance).AsNoTracking().ToArray();
                    var lstEmpPosition = _db.TB_Employee_Position.AsNoTracking().ToArray();
                    var lstPosition = _db.TB_Position.AsNoTracking().ToArray();

                    var lstLog = new List<cMedRequestHistoryLog>();
                    foreach (var item in lstMedRequest)
                    {
                        int nEmpID = 0;
                        string? sEmpName = "";
                        var objEmp = lstEmployee.FirstOrDefault(w => w.nEmployeeID == item.nUpdateBy);

                        if (objEmp != null)
                        {
                            nEmpID = objEmp.nEmployeeID;
                            sEmpName = $"{objEmp.sNameTH} {objEmp.sSurnameTH} ({objEmp.sNickname})";
                        }

                        var position = (from a in lstEmpPosition.Where(w => w.nEmployeeID == nEmpID)
                                        join b in lstPosition on a.nPositionID equals b.nPositionID
                                        select b.sPositionName).FirstOrDefault();

                        lstLog.Add(new cMedRequestHistoryLog
                        {
                            sName = sEmpName,
                            sPosition = position ?? "-",
                            sComment = item.sComent ?? "-",
                            sStatus = lstStatus.Where(w => w.nStatusID == item.nStatus).Select(s => s.sStatusName).FirstOrDefault() + "",
                            sActionMode = item.nStatus == (int)EnumGlobal.StatusRequestWelfare.Canceled ? "Cancel" :
                                item.nStatus == (int)EnumGlobal.StatusRequestWelfare.WaitingApprove ? "Submit" :
                                    item.nStatus == (int)EnumGlobal.StatusRequestWelfare.Reject ? "Reject" :
                                        item.nStatus == (int)EnumGlobal.StatusRequestWelfare.Completed ? "Approve" :
                                            lstStatus.Where(w => w.nStatusID == item.nStatus).Select(s => s.sStatusName).FirstOrDefault() + "",
                            sActionDate = item.dUpdate.ToStringFromDate("dd/MM/yyyy , HH:mm:ss", "th-TH"),
                        });
                    }

                    _db.SaveChanges();
                    #endregion

                    #region Send Line Noti
                    string sGUID = Guid.NewGuid().ToString();

                    var arrProject = _db.TB_Project.ToArray();
                    var arrStatus = _db.TM_Status.ToArray();
                    var lstUserHR = (from a in _db.TB_Employee_Position
                                     join b in _db.TB_Employee on a.nEmployeeID equals b.nEmployeeID
                                     where a.nPositionID == (int)EnumEmployee.PositionID.HR && b.IsActive && !b.IsDelete
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
                    objParam.sMoney = $"{objData.nRemain:n}" ?? "0";
                    objParam.sRemark = objData.sComent ?? "";

                    objParam.sStatus = arrStatus.Where(w => w.nRequestTypeID == (int)EnumGlobal.RequestFormType.Dental && w.nStatusID == objData.nStatus).Select(s => s.sStatusName).FirstOrDefault() ?? "";
                    objParam.sPathDetail = "DetailDentalLine&sID=" + objData.nRequestDentalID.EncryptParameter() + "&sUserID=" + objData.nCreateBy.EncryptParameter();

                    switch (objData.nStatus)
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

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
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
    }
}