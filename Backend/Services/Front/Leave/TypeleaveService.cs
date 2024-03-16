
using System.Globalization;
using System.Linq;
using Backend.EF.ST_Intranet;
using Backend.Enum;
using Backend.Interfaces.Authentication;
using Extensions.Common.STFunction;
using Extensions.Common.STResultAPI;
using ST.INFRA;
using ST.INFRA.Common;
using ST_API.Interfaces;
using ST_API.Models;
using ST_API.Models.ITypeleaveService;
using static Extensions.Systems.AllClass;
using ClosedXML.Excel;
using Extensions.Common.STExtension;
using System.Net;
using SelectOption = ST_API.Models.ITypeleaveService.SelectOption;

namespace Backend.Service.TypeleaveService
{
    public class TypeleaveService : ITypeleaveService
    {
        private readonly ST_IntranetEntity db;
        //private readonly IConfiguration _config;
        private readonly IAuthentication _authen;
        private readonly IHostEnvironment _env;


        public TypeleaveService(ST_IntranetEntity _db, IAuthentication authen, IHostEnvironment env)
        {
            // _config = config;, IConfiguration config
            _authen = authen;
            _env = env;
            db = new ST_IntranetEntity();
        }
        public async Task<ResultTypeleave> GetData_MasterDetail()
        {
            ResultTypeleave result = new ResultTypeleave();
            try
            {
                var UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                result.lstDataSex = db.TM_Data.Where(w => w.IsActive && !w.IsDelete && w.nDatatypeID == (int)Enum.EnumLeave.DataType.nSex).OrderByDescending(o => o.nData_ID).Select(s => new SelectOption
                {
                    label = s.sNameTH,
                    value = s.nData_ID + "",
                    isActive = s.IsActive,
                }).ToList();

                result.lstDataEmployeeType = db.TM_Data.Where(w => w.IsActive && !w.IsDelete && w.nDatatypeID == (int)Enum.EnumLeave.DataType.nEmployeeType).OrderBy(o => o.nData_ID).Select(s => new SelectOption
                {
                    label = s.sNameTH,
                    value = s.nData_ID + "",
                    isActive = s.IsActive,
                }).ToList();

                result.lstDataleaveType = db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete && w.IsChangeIntoMoney == true).OrderBy(o => o.nLeaveTypeID).Select(s => new SelectOption
                {
                    label = s.LeaveTypeName,
                    value = s.nLeaveTypeID + "",
                    isActive = s.IsActive,
                }).ToList();

                result.lstDataleaveTypeAll = db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete).OrderBy(o => o.nLeaveTypeID).Select(s => new SelectOption
                {
                    label = s.LeaveTypeName,
                    value = s.nLeaveTypeID + "",
                    isActive = s.IsActive,
                }).ToList();

                result.lstDataEmployee = db.TB_Employee.Where(w => w.IsActive && !w.IsDelete).OrderBy(o => o.nEmployeeID).Select(s => new SelectOption
                {
                    label = $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})",
                    value = s.nEmployeeID + "",
                    isActive = s.IsActive,
                }).ToList();

                var startYear = STFunction.GetAppSettingJson("AppSetting:StartYear").ToInt();
                int yearMax = DateTime.Now.Year + 1;

                var lstIntYear = new List<int>();
                for (int i = yearMax; i >= startYear; i--)
                {
                    lstIntYear.Add(i);
                }

                result.lstOptionYear = lstIntYear.OrderByDescending(o => o).Select(i => new SelectOption
                {
                    value = i.ToString(),
                    label = i.ToString(),
                }).ToList();

                List<string> lstUser = new List<string>();
                IQueryable<TB_Employee_Report_To> TBEmployeeReportTo = db.TB_Employee_Report_To.Where(w => !w.IsDelete).OrderBy(o => o.nEmployeeID).AsQueryable();
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

                List<string> lstleaveTypeAll = new List<string>();
                var lstDataleaveTypeAll = db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete).Select(s => s.nLeaveTypeID + "").ToList();
                lstleaveTypeAll.AddRange(lstDataleaveTypeAll);
                result.lstleaveTypeAll = lstleaveTypeAll;

                result.Status = StatusCodes.Status200OK;
            }

            catch (Exception error)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = error.Message;
            }
            return result;
        }
        public async Task<ResultAPI> SaveLeaveType(TypeleaveData objSave)
        {
            ResultAPI result = new ResultAPI();
            try
            {

                var IsDuplicate = db.TB_LeaveType.FirstOrDefault(w => w.IsDelete == false && w.sLeaveTypeCode == objSave.sLeaveTypeCode && w.LeaveTypeName == objSave.sLeaveTypeName && w.nLeaveTypeID != objSave.nLeaveID);
                if (IsDuplicate != null)
                {

                    result.Message = "ข้อมูลรหัสอ้างอิง  " + IsDuplicate.sLeaveTypeCode + "  และข้อมูลชื่อการลา  " + IsDuplicate.LeaveTypeName + " มีในระบบแล้ว";
                    return result;
                }

                int nLeaveTypeID = objSave.lstDataType.Select(s => s.nEmployeeTypeID).Count();
                if (nLeaveTypeID <= 0)
                {

                    result.Message = "กรุณาเลือกประเภทพนักงานและกรอกข้อมูลอย่างน้อย 1 ประเภท";
                    return result;
                }
                int nUserID = 1;
                TB_LeaveType? objInfo = db.TB_LeaveType.FirstOrDefault(w => w.nLeaveTypeID == objSave.nLeaveID);
                if (objInfo == null)
                {
                    objInfo = new TB_LeaveType();
                    objInfo.nLeaveTypeID = (db.TB_LeaveType.Any() ? db.TB_LeaveType.Max(m => m.nLeaveTypeID) : 0) + 1;
                    objInfo.dCreate = DateTime.Now;
                    objInfo.nCreateBy = nUserID;
                    db.TB_LeaveType.Add(objInfo);
                }
                objInfo.sLeaveTypeCode = objSave.sLeaveTypeCode;
                objInfo.LeaveTypeName = objSave.sLeaveTypeName;
                objInfo.sSex = objSave.nSex;
                objInfo.nAssociate = objSave.nAssociate;
                objInfo.nAdvanceLeave = objSave.nAdvanceLeave;
                objInfo.nMaximum = objSave.nMaximum;
                objInfo.IsChangeIntoMoney = objSave.nChangeIntoMoney == 1 ? true : false;
                objInfo.sCondition = objSave.sCondition;
                objInfo.nOrder = (db.TB_LeaveType.Any() ? db.TB_LeaveType.Max(m => m.nOrder) : 0) + 1;
                objInfo.IsActive = true;
                objInfo.dUpdate = DateTime.Now;
                objInfo.nUpdateBy = nUserID;
                objInfo.IsDelete = false;

                //  db.SaveChanges();

                db.TB_LeaveSetting.Where(w => !w.IsDelete && w.nLeaveTypeID == objInfo.nLeaveTypeID).ToList().ForEach(f =>
                             {
                                 f.IsDelete = true;
                                 f.dDelete = DateTime.Now;
                                 f.dUpdate = DateTime.Now;
                                 f.nUpdateBy = nUserID;
                                 f.nDeleteBy = nUserID;
                             });

                db.SaveChanges();

                int nLeaveSettingID = (db.TB_LeaveSetting.Any() ? db.TB_LeaveSetting.Max(m => m.LeaveSettingID) : 0) + 1;
                int nLeaveTypeettingID = (db.TB_LeaveSetting_History.Any() ? db.TB_LeaveSetting_History.Max(m => m.nLeaveSettingHisID) : 0) + 1;
                foreach (var f in objSave.lstDataType)
                {
                    TB_LeaveSetting objLeave = db.TB_LeaveSetting.FirstOrDefault(w => w.nEmployeeTypeID == f.nEmployeeTypeID && w.LeaveSettingID == f.nLeaveSettingID);
                    if (objLeave == null)
                    {
                        objLeave = new TB_LeaveSetting();
                        objLeave.LeaveSettingID = nLeaveSettingID;
                        objLeave.dCreate = DateTime.Now;
                        objLeave.nCreateBy = nUserID;
                        db.TB_LeaveSetting.Add(objLeave);
                    };
                    objLeave.nLeaveTypeID = objInfo.nLeaveTypeID;
                    objLeave.nEmployeeTypeID = f.nEmployeeTypeID;
                    objLeave.nOrder = f.nOrder;
                    objLeave.nWorkingAgeStart = f.nWorkingAgeStart;
                    objLeave.nWorkingAgeEnd = f.nWorkingAgeEnd;
                    objLeave.nLeaveRights = f.nLeaveRights;
                    objLeave.IsProportion = f.isProportion;
                    objLeave.dUpdate = DateTime.Now;
                    objLeave.nUpdateBy = nUserID;
                    objLeave.IsDelete = f.IsDelete;


                    TB_LeaveSetting_History objLeaveLog = new TB_LeaveSetting_History()
                    {
                        nLeaveSettingHisID = nLeaveTypeettingID,
                        LeaveSettingID = objLeave.LeaveSettingID,
                        nLeaveTypeID = objLeave.nLeaveTypeID,
                        nEmployeeTypeID = objLeave.nEmployeeTypeID,
                        dCreate = DateTime.Now,
                        nCreateBy = nUserID,
                        nOrder = objLeave.nOrder,
                        nWorkingAgeStart = objLeave.nWorkingAgeStart,
                        nWorkingAgeEnd = objLeave.nWorkingAgeEnd,
                        nLeaveRights = objLeave.nLeaveRights,
                        IsProportion = objLeave.IsProportion,
                        dUpdate = DateTime.Now,
                        nUpdateBy = nUserID,
                        IsDelete = false,

                    };
                    db.TB_LeaveSetting_History.Add(objLeaveLog);

                    nLeaveSettingID += 1;
                    nLeaveTypeettingID += 1;
                }
                // สัดส่วนการลา


                db.TB_LeaveProportion.Where(w => !w.IsDelete && w.nLeaveTypeID == objInfo.nLeaveTypeID).ToList().ForEach(f =>
                                           {
                                               f.IsDelete = true;
                                               f.dDelete = DateTime.Now;
                                               f.dUpdate = DateTime.Now;
                                               f.nUpdateBy = nUserID;
                                               f.nDeleteBy = nUserID;
                                           });

                db.SaveChanges();
                decimal nCheck = (objSave.nJan ?? 0 + objSave.nFeb ?? 0 + objSave.nMar ?? 0 + objSave.nApr ?? 0 + objSave.nMay ?? 0 + objSave.nJun ?? 0 + objSave.nJul ?? 0 + objSave.nAug ?? 0 + objSave.nSep ?? 0 + objSave.nOct ?? 0 + objSave.nNov ?? 0 + objSave.nDec ?? 0);
                if (nCheck > 0)
                {
                    TB_LeaveProportion? objProportion = db.TB_LeaveProportion.FirstOrDefault(w => w.nLeaveTypeID == objInfo.nLeaveTypeID);
                    if (objProportion == null)
                    {
                        objProportion = new TB_LeaveProportion();
                        objProportion.LeaveProportionID = (db.TB_LeaveProportion.Any() ? db.TB_LeaveProportion.Max(m => m.LeaveProportionID) : 0) + 1;
                        objProportion.nLeaveTypeID = objInfo.nLeaveTypeID;
                        objProportion.nEmployeeTypeID = (int)Enum.EnumLeave.EmployeeType.FullTime;
                        objProportion.dCreate = DateTime.Now;
                        objProportion.nCreateBy = nUserID;
                        db.TB_LeaveProportion.Add(objProportion);
                    }

                    objProportion.nJan = objSave.nJan ?? 0;
                    objProportion.nFeb = objSave.nFeb ?? 0;
                    objProportion.nMar = objSave.nMar ?? 0;
                    objProportion.nApr = objSave.nApr ?? 0;
                    objProportion.nMay = objSave.nMay ?? 0;
                    objProportion.nJun = objSave.nJun ?? 0;
                    objProportion.nJul = objSave.nJul ?? 0;
                    objProportion.nAug = objSave.nAug ?? 0;
                    objProportion.nSep = objSave.nSep ?? 0;
                    objProportion.nOct = objSave.nOct ?? 0;
                    objProportion.nNov = objSave.nNov ?? 0;
                    objProportion.nDec = objSave.nDec ?? 0;
                    objProportion.dUpdate = DateTime.Now;
                    objProportion.nUpdateBy = nUserID;
                    objProportion.IsDelete = false;
                }
                db.SaveChanges();

                // File
                // List<TB_Image_Leave> lstInfoFile = db.TB_Image_Leave.Where(w => w.nLeaveTypeID == objInfo.nLeaveTypeID).ToList();
                // if (lstInfoFile.Count > 0)
                // {
                //     db.TB_Image_Leave.RemoveRange(lstInfoFile);
                //     db.SaveChanges();
                // }

                db.TB_Image_Leave.Where(w => !w.IsDelete && w.nLeaveTypeID == objInfo.nLeaveTypeID).ToList().ForEach(f =>
                        {
                            f.IsDelete = true;
                            f.dDelete = DateTime.Now;
                            f.dUpdate = DateTime.Now;
                            f.nUpdateBy = nUserID;
                            f.nDeleteBy = nUserID;
                        });

                db.SaveChanges();

                #region File
                var objAll = objSave.fFile?.FirstOrDefault();
                if (objAll != null)
                {
                    string pathTempContent = !string.IsNullOrEmpty(objAll.sFolderName) ? objAll.sFolderName + "/" : "Temp/Leave/";

                    string truePathFile = "Leave\\" + objInfo.nLeaveTypeID + "\\";
                    string fPathContent = "Leave/" + objInfo.nLeaveTypeID;

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

                    int nImageID = db.TB_Image_Leave.Any() ? db.TB_Image_Leave.Max(m => m.nImageID) + 1 : 1;
                    TB_Image_Leave objInfofile = new TB_Image_Leave()
                    {
                        nLeaveTypeID = objInfo.nLeaveTypeID,
                        nImageID = nImageID,
                        sImageParh = fPathContent,
                        sSystemName = objAll.sSysFileName != null ? objAll.sSysFileName : "",
                        sExpireName = objAll.sSysFileName != null ? objAll.sSysFileName : "",
                        nOrder = (db.TB_Image_Leave.Any() ? db.TB_Image_Leave.Max(m => m.nOrder) : 0) + 1,
                        dCreate = DateTime.Now,
                        nCreateBy = 1,
                        dUpdate = DateTime.Now,
                        nUpdateBy = 1,
                        IsDelete = false,
                    };
                    db.TB_Image_Leave.Add(objInfofile);


                }
                #endregion

                TB_LeaveType_History objSaveLog = new TB_LeaveType_History()
                {
                    nLeaveTypeHisID = (db.TB_LeaveType_History.Any() ? db.TB_LeaveType_History.Max(m => m.nLeaveTypeHisID) : 0) + 1,
                    nLeaveTypeID = objInfo.nLeaveTypeID,
                    sLeaveTypeCode = objInfo.sLeaveTypeCode,
                    LeaveTypeName = objInfo.LeaveTypeName,
                    nOrder = objInfo.nOrder,
                    sSex = objInfo.sSex,
                    nAssociate = objInfo.nAssociate,
                    nAdvanceLeave = objInfo.nAdvanceLeave,
                    nMaximum = objInfo.nMaximum,
                    isChangeIntoMoney = objInfo.IsChangeIntoMoney,
                    sCondition = objInfo.sCondition,
                    IsActive = objInfo.IsActive,
                    dCreate = DateTime.Now,
                    nCreateBy = nUserID,
                    dUpdate = DateTime.Now,
                    nUpdateBy = nUserID,
                    IsDelete = objInfo.IsDelete
                };
                db.TB_LeaveType_History.Add(objSaveLog);

                db.SaveChanges();
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception error)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = error.Message;
            }
            return result;

        }

        public async Task<cResultTableTypeLeave> GetTableLeaveType(cReqTableTypeleave param)
        {
            cResultTableTypeLeave result = new cResultTableTypeLeave();
            try
            {
                var lstTypeleave = db.TB_LeaveType.Where(w => !w.IsDelete).OrderBy(d => d.sLeaveTypeCode).ToList();
                var query = lstTypeleave.Select((s, Index) => new cTableTypeleave
                {
                    sID = s.nLeaveTypeID.EncryptParameter(),
                    nLeaveTypeID = s.nLeaveTypeID,
                    sStatus = s.IsActive ? EnumLeave.GlobalText.Active : EnumLeave.GlobalText.InActive,
                    sLeaveTypeCode = s.sLeaveTypeCode,
                    sLeaveTypeName = s.LeaveTypeName,
                    dUpdate = s.dUpdate,
                    nStatus = s.IsActive == true ? 1 : 0,
                    isActive = s.IsActive
                });


                #region Filter

                //   สถานะ
                if (!string.IsNullOrEmpty(param.sStatus))
                {
                    query = query.Where(w => w.nStatus == (param.sStatus.ToInt()));
                }
                if (!string.IsNullOrEmpty(param.sLeaveTypeCode))
                {
                    query = query.Where(w => (w.sLeaveTypeCode).Trim().ToLower().Contains((param.sLeaveTypeCode).Trim().ToLower()));
                }
                if (!string.IsNullOrEmpty(param.sLeaveTypeName))
                {
                    query = query.Where(w => (w.sLeaveTypeName).Trim().ToLower().Contains((param.sLeaveTypeName).Trim().ToLower()));
                }

                #endregion

                #region//SORT
                string sSortColumn = (param != null && !string.IsNullOrEmpty(param.sSortExpression) ? param.sSortExpression : "");
                switch (param.sSortExpression)
                {

                    case "sLeaveTypeCode": sSortColumn = "sLeaveTypeCode"; break;
                    case "sLeaveTypeName": sSortColumn = "sLeaveTypeName"; break;
                    case "sStatus": sSortColumn = "sStatus"; break;
                    case "dUpdate": sSortColumn = "dUpdate"; break;
                }

                if (param.isASC)
                {
                    query = query.OrderBy<cTableTypeleave>(sSortColumn);
                }
                else if (param.isDESC)
                {
                    query = query.OrderByDescending<cTableTypeleave>(sSortColumn);
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, query.Count());
                result.lstData = query.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                #endregion
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception error)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = error.Message;
            }
            return result;
        }

        public async Task<ResultTypeleave> GetDataEdit(cParam param)
        {
            ResultTypeleave result = new ResultTypeleave();
            try
            {

                var objEdit = db.TB_LeaveType.FirstOrDefault(f => f.nLeaveTypeID == param.nLeaveID);
                var lstLeaveSetting = db.TB_LeaveSetting.Where(w => !w.IsDelete);
                var objDataType = new TypeleaveData();
                if (objEdit != null)
                {
                    objDataType.nLeaveTypeID = objEdit.nLeaveTypeID;
                    objDataType.sLeaveTypeCode = objEdit.sLeaveTypeCode;
                    objDataType.sLeaveTypeName = objEdit.LeaveTypeName;
                    objDataType.nSex = objEdit.sSex;
                    objDataType.nAssociate = objEdit.nAssociate;
                    objDataType.nAdvanceLeave = objEdit.nAdvanceLeave;
                    objDataType.nMaximum = objEdit.nMaximum;
                    objDataType.sCondition = objEdit.sCondition;
                    objDataType.nChangeIntoMoney = objEdit.IsChangeIntoMoney == true ? 1 : 2;


                    var objProportion = db.TB_LeaveProportion.FirstOrDefault(f => f.nLeaveTypeID == objEdit.nLeaveTypeID && !f.IsDelete);
                    if (objProportion != null)
                    {
                        objDataType.nEmployeeTypeID = objProportion.nEmployeeTypeID;
                        objDataType.nJan = objProportion.nJan;
                        objDataType.nFeb = objProportion.nFeb;
                        objDataType.nMar = objProportion.nMar;
                        objDataType.nApr = objProportion.nApr;
                        objDataType.nMay = objProportion.nMay;
                        objDataType.nJun = objProportion.nJun;
                        objDataType.nJul = objProportion.nJul;
                        objDataType.nAug = objProportion.nAug;
                        objDataType.nSep = objProportion.nSep;
                        objDataType.nOct = objProportion.nOct;
                        objDataType.nNov = objProportion.nNov;
                        objDataType.nDec = objProportion.nDec;

                    }

                    var Image = db.TB_Image_Leave.FirstOrDefault(w => w.nLeaveTypeID == objEdit.nLeaveTypeID && !w.IsDelete);
                    List<ItemFileData> lstfile = new List<ItemFileData>();
                    if (Image != null)
                    {


                        ItemFileData fFile = new ItemFileData();
                        var sPath = GetPathUploadFile(Image.sImageParh ?? "", Image.sSystemName ?? "");
                        fFile.sFileName = Image.sExpireName;
                        fFile.sCropFileLink = sPath;
                        fFile.sFileLink = sPath;
                        fFile.sSysFileName = Image.sSystemName;
                        fFile.sFileType = Image.sSystemName != null ? Image.sSystemName.Split(".")[1] + "" : null;
                        fFile.sFolderName = Image.sImageParh;
                        fFile.IsNew = false;
                        fFile.IsNewTab = false;
                        fFile.IsCompleted = true;
                        fFile.IsDelete = false;
                        fFile.IsProgress = false;
                        fFile.sProgress = "100";
                        lstfile.Add(fFile);

                        result.fFileImage = lstfile;
                    }
                    var lstOrder = (from a in db.TB_LeaveType.Where(w => w.nLeaveTypeID == param.nLeaveID && !w.IsDelete)
                                    from b in lstLeaveSetting.Where(w => w.nLeaveTypeID == a.nLeaveTypeID).DefaultIfEmpty()
                                    select new cTypeleaveData
                                    {
                                        nLeaveSettingID = b.LeaveSettingID,
                                        nEmployeeTypeID = b.nEmployeeTypeID,
                                        nWorkingAgeStart = b.nWorkingAgeStart,
                                        nWorkingAgeEnd = b.nWorkingAgeEnd ?? 0,
                                        nLeaveRights = b.nLeaveRights,
                                        nOrder = b.nOrder,
                                        isProportion = b.IsProportion,
                                        isDataType = b.nEmployeeTypeID == (int)Enum.EnumLeave.EmployeeType.FullTime ? true : false,
                                        isChk = b.nEmployeeTypeID == (int)Enum.EnumLeave.EmployeeType.FullTime || b.nEmployeeTypeID == (int)Enum.EnumLeave.EmployeeType.Probationary || b.nEmployeeTypeID == (int)Enum.EnumLeave.EmployeeType.OutSouse ? true : false,
                                        IsDelete = b.IsDelete,

                                    }).ToList();


                    result.lstOrder = lstOrder.Where(w => w.nEmployeeTypeID == (int)Enum.EnumLeave.EmployeeType.FullTime).ToList();
                    result.lstOrder1 = lstOrder.Where(w => w.nEmployeeTypeID == (int)Enum.EnumLeave.EmployeeType.Probationary).ToList();
                    result.lstOrder2 = lstOrder.Where(w => w.nEmployeeTypeID == (int)Enum.EnumLeave.EmployeeType.OutSouse).ToList();
                    result.objEdit = objDataType;

                }
                result.Status = StatusCodes.Status200OK;
            }

            catch (Exception error)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = error.Message;
            }
            return result;
        }

        public async Task<ResultAPI> RemoveDataTable(cRemoveTableLeave param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                int nUserID = 1;
                List<int> lstID = param.lstID.ConvertAll(item => item.DecryptParameter().ToInt()).ToList();
                var lstData = db.TB_LeaveType.Where(w => lstID.Contains(w.nLeaveTypeID)).ToList();
                var lstLeaveSetting = db.TB_LeaveSetting.Where(w => lstID.Contains(w.nLeaveTypeID)).ToList();
                var lstLeaveProportion = db.TB_LeaveProportion.Where(w => lstID.Contains(w.nLeaveTypeID)).ToList();
                foreach (var item in lstData)
                {
                    item.IsDelete = true;
                    item.nDeleteBy = nUserID;
                    item.dDelete = DateTime.Now;
                }
                foreach (var item in lstLeaveSetting)
                {
                    item.IsDelete = true;
                    item.nDeleteBy = nUserID;
                    item.dDelete = DateTime.Now;
                }
                foreach (var item in lstLeaveProportion)
                {
                    item.IsDelete = true;
                    item.nDeleteBy = nUserID;
                    item.dDelete = DateTime.Now;
                }
                db.SaveChanges();
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }


        public async Task<cResultTableTypeLeave> GetTableLeaveSummary(cReqTableLeaveSummary param)
        {
            cResultTableTypeLeave result = new cResultTableTypeLeave();
            try
            {
                var lstLeaveChange = db.TB_LeaveChange.Where(w => !w.IsDelete).OrderByDescending(or => or.dUpdate).ToList();


                var lstLeaveType = db.TB_LeaveType.Where(w => w.IsChangeIntoMoney == true && w.IsActive && !w.IsDelete).ToList();
                var query = (from Leave in lstLeaveType
                             from a in lstLeaveChange.Where(w => w.nLeaveTypeID == Leave.nLeaveTypeID)
                             from b in db.TB_Employee.Where(w => w.nEmployeeID == a.nEmployeeID)

                             select new cTableLeaveSummary
                             {
                                 sID = a.nID.EncryptParameter(),
                                 nLeaveSummaryID = a.nID,
                                 nEmployeeID = a.nEmployeeID,
                                 sEmployeeName = b.sNameTH + "  " + b.sSurnameTH,
                                 sYearWork = (DateTime.Now.Year - b.dPromote.Value.Year) + "ปี" + " " + (DateTime.Now.Month - b.dPromote.Value.Month > 0 ? DateTime.Now.Month - b.dPromote.Value.Month : 0) + "เดือน",
                                 nQuantity = a.nQuantity,
                                 nIntoMoney = a.nIntoMoney,
                                 nLeaveRemain = a.nLeaveRemain ?? 0,
                                 nLeaveTypeID = a.nLeaveTypeID,
                                 sLeaveTypeName = lstLeaveType.Any(w => w.nLeaveTypeID == a.nLeaveTypeID) ? lstLeaveType.First(w => w.nLeaveTypeID == a.nLeaveTypeID).LeaveTypeName : "",

                             }).ToArray(); ;
                #region Filter

                //   สถานะ
                if (param.lstEmployeeID != null && param.lstEmployeeID.Any())
                {
                    query = query.Where(w => param.lstEmployeeID.Contains(w.nEmployeeID + "")).ToArray();
                }
                if (param.lstLeaveType != null && param.lstLeaveType.Any())
                {
                    query = query.Where(w => param.lstLeaveType.Contains(w.nLeaveTypeID + "")).ToArray();
                }
                if (param.nIntoMoney > 0)
                {
                    query = query.Where(w => w.nIntoMoney == param.nIntoMoney).ToArray();
                }
                if (param.nLeaveRemain > 0)
                {
                    query = query.Where(w => w.nLeaveRemain == param.nLeaveRemain).ToArray();
                }

                if (!string.IsNullOrEmpty(param.sYearWork))
                {
                    query = query.Where(w => (w.sYear).Trim().ToLower().Contains((param.sYearWork).Trim().ToLower())).ToArray();
                }

                #endregion

                #region//SORT
                string sSortColumn = (param != null && !string.IsNullOrEmpty(param.sSortExpression) ? param.sSortExpression : "");
                switch (param.sSortExpression)
                {
                    default:
                    case "nLeaveSummaryID": sSortColumn = "nLeaveSummaryID"; break;
                    case "sEmployeeName": sSortColumn = "sEmployeeName"; break;
                    case "sYear": sSortColumn = "sYear"; break;
                    case "nQuantity": sSortColumn = "nQuantity"; break;
                    case "nLeaveRemain": sSortColumn = "nLeaveRemain"; break;
                    case "nIntoMoney": sSortColumn = "nIntoMoney"; break;
                    case "dUpdate": sSortColumn = "dUpdate"; break;

                }

                if (param.isASC)
                {
                    query = query.OrderBy<cTableLeaveSummary>(sSortColumn).ToArray();
                }
                else if (param.isDESC)
                {
                    query = query.OrderByDescending<cTableLeaveSummary>(sSortColumn).ToArray();
                }

                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, query.Count());
                result.lstDataSummary = query.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                #endregion
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception error)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = error.Message;
            }
            return result;
        }

        public async Task<ResultAPI> RemoveDataTableSummary(cRemoveTableLeave param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                int nUserID = 1;
                List<int> lstID = param.lstID.ConvertAll(item => item.DecryptParameter().ToInt()).ToList();
                var lstData = db.TB_LeaveChange.Where(w => lstID.Contains(w.nID)).ToList();

                foreach (var item in lstData)
                {
                    item.IsDelete = true;
                    item.nDeleteBy = nUserID;
                    item.dDelete = DateTime.Now;
                }
                db.SaveChanges();
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }

        public async Task<cResultTableTypeLeave> GetDataTypeSummary(cParamSummary param)
        {
            cResultTableTypeLeave result = new cResultTableTypeLeave();
            try
            {
                var lstleave = db.TB_LeaveChange.ToList();
                var lstTypeleave = db.TB_LeaveChange.FirstOrDefault(w => w.nID == param.nLeaveSummaryID);
                var Employee = db.TB_Employee.Where(w => w.IsActive && !w.IsDelete).ToList();

                if (lstTypeleave != null)
                {
                    var query = (from a in lstleave.Where(w => w.nID == param.nLeaveSummaryID)
                                 from b in Employee.Where(w => w.nEmployeeID == a.nEmployeeID)
                                 select new cTableLeaveSummary
                                 {
                                     sID = a.nID.EncryptParameter(),
                                     nLeaveSummaryID = a.nID,
                                     nEmployeeID = a.nEmployeeID,
                                     sEmployeeName = b.sNameTH + "  " + b.sSurnameTH,
                                     sYearWork = b.dPromote != null ? (DateTime.Now.Year - b.dPromote.Value.Year) + "ปี" + " " + (DateTime.Now.Month - b.dPromote.Value.Month > 0 ? DateTime.Now.Month - b.dPromote.Value.Month : 0) + "เดือน" : "",
                                     nQuantity = a.nQuantity,
                                     nIntoMoney = a.nIntoMoney,
                                     nLeaveRemain = a.nLeaveRemain ?? 0,
                                     nLeaveTypeID = a.nLeaveTypeID,
                                     sLeaveTypeID = a.nLeaveTypeID + "",
                                     sEmployeeID = a.nEmployeeID + "",

                                 });

                    #region//Final Action >> Skip , Take And Set Page
                    var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, query.Count());
                    result.lstDataSummaryEdit = query.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                    result.nDataLength = dataPage.nDataLength;
                    result.nPageIndex = dataPage.nPageIndex;
                    result.nSkip = dataPage.nSkip;
                    result.nTake = dataPage.nTake;
                    result.nStartIndex = dataPage.nStartIndex;
                    #endregion
                }
                else
                {
                    int nID = 0;
                    var query = (from b in Employee.Where(w => w.nEmployeeID == param.nEmployeeID)
                                 from c in db.TB_LeaveSummary.Where(w => w.nEmployeeID == b.nEmployeeID && w.nLeaveTypeID == param.nLeaveTypeID && w.nYear == DateTime.Now.Year).DefaultIfEmpty()
                                 select new cTableLeaveSummary
                                 {
                                     sID = nID.EncryptParameter(),
                                     nLeaveSummaryID = nID,
                                     nEmployeeID = b.nEmployeeID,
                                     sEmployeeName = b.sNameTH + "  " + b.sSurnameTH,
                                     sYearWork = b.dPromote != null ? (DateTime.Now.Year - b.dPromote.Value.Year) + "ปี" + " " + (DateTime.Now.Month - b.dPromote.Value.Month > 0 ? DateTime.Now.Month - b.dWorkStart.Value.Month : 0) + "เดือน" : "",
                                     nQuantity = c != null ? c.nQuantity : 0,
                                     nIntoMoney = null,
                                     nLeaveRemain = c != null ? c.nLeaveRemain : 0,
                                     nLeaveTypeID = c != null ? c.nLeaveTypeID : 0,
                                     sLeaveTypeID = c != null ? c.nLeaveTypeID + "" : "",
                                     sEmployeeID = b.nEmployeeID + "",

                                 });


                    #region//Final Action >> Skip , Take And Set Page
                    var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, query.Count());
                    result.lstDataSummaryEdit = query.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                    result.nDataLength = dataPage.nDataLength;
                    result.nPageIndex = dataPage.nPageIndex;
                    result.nSkip = dataPage.nSkip;
                    result.nTake = dataPage.nTake;
                    result.nStartIndex = dataPage.nStartIndex;
                    #endregion
                }

                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception error)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = error.Message;
            }
            return result;

        }

        public async Task<ResultAPI> SaveLeaveSummary(cSaveSummary objSave)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                int nUserID = 1;
                int nYear = DateTime.Now.Year;
                TB_LeaveChange? objInfo = db.TB_LeaveChange.FirstOrDefault(w => w.nID == objSave.nSummaryID);
                int nLeaveChangeID = (db.TB_LeaveChange.Any() ? db.TB_LeaveChange.Max(m => m.nID) : 0) + 1;
                var lstEmployee = db.TB_Employee.FirstOrDefault(f => f.nEmployeeID == objSave.nEmployeeID);
                DateTime dWorkStart = DateTime.Now;
                DateTime dWorkNow = DateTime.Now;
                if (lstEmployee != null)
                {
                    dWorkStart = lstEmployee.dWorkStart.Value != null ? lstEmployee.dWorkStart.Value : DateTime.Now;
                    dWorkNow = lstEmployee.dPromote.Value != null ? lstEmployee.dPromote.Value : DateTime.Now;

                }
                var IsDuplicate = db.TB_LeaveChange.FirstOrDefault(w => w.IsDelete == false && w.nEmployeeID == objSave.nEmployeeID && w.nLeaveTypeID == objSave.nLeaveTypeID && w.nID != objSave.nSummaryID);
                if (IsDuplicate != null)
                {
                    result.Message = "มีข้อมูลประเภทการลานี้ในระบบแล้ว";
                    return result;
                }

                if (objInfo == null)
                {
                    objInfo = new TB_LeaveChange();
                    objInfo.nID = nLeaveChangeID;
                    objInfo.nEmployeeID = objSave.nEmployeeID;
                    objInfo.nLeaveTypeID = objSave.nLeaveTypeID;
                    objInfo.dCreate = DateTime.Now;
                    objInfo.nCreateBy = nUserID;
                    objInfo.dWorkStart = dWorkStart;
                    objInfo.dWorkNow = dWorkNow;
                    objInfo.nYear = nYear;
                    objInfo.nQuantity = objSave.nQuantity;
                    db.TB_LeaveChange.Add(objInfo);
                }
                objInfo.nIntoMoney = objSave.nIntoMoney;
                objInfo.nLeaveRemain = objSave.nLeaveRemain;
                objInfo.dUpdate = DateTime.Now;
                objInfo.nUpdateBy = nUserID;
                objInfo.IsDelete = false;
                db.SaveChanges();

                result.Status = StatusCodes.Status200OK;

            }
            catch (Exception error)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = error.Message;
            }
            return result;
        }


        public async Task<ResultTypeleave> GetDataSummary(cParamExport param)
        {
            ResultTypeleave result = new ResultTypeleave();

            var Employee = db.TB_Employee.Where(w => w.IsActive && !w.IsDelete).ToList();
            var TypeLeave = db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete && w.IsChangeIntoMoney == true).ToList();

            var lstTypeleave = db.TB_LeaveChange.Where(w => !w.IsDelete).ToList();
            var lstData = (from Type in TypeLeave
                           from a in lstTypeleave.Where(w => w.nLeaveTypeID == Type.nLeaveTypeID).DefaultIfEmpty()
                           from b in Employee.Where(w => w.nEmployeeID == a.nEmployeeID).DefaultIfEmpty()

                           select new cTableLeaveSummary
                           {
                               sID = a.nID.EncryptParameter(),
                               nLeaveSummaryID = a.nID,
                               nEmployeeID = a.nEmployeeID,
                               sEmployeeName = b != null ? b.sNameTH + "  " + b.sSurnameTH : "",
                               sYear = b.dPromote != null ? (DateTime.Now.Year - b.dPromote.Value.Year) + "ปี" + " " + (DateTime.Now.Month - b.dPromote.Value.Month > 0 ? DateTime.Now.Month - b.dPromote.Value.Month : 0) + "เดือน" : "",
                               nQuantity = a.nQuantity,
                               nIntoMoney = a.nIntoMoney,
                               nLeaveRemain = a.nLeaveRemain ?? 0,
                               nLeaveTypeID = a.nLeaveTypeID,
                               sLeaveTypeName = TypeLeave.Any(w => w.nLeaveTypeID == a.nLeaveTypeID) ? TypeLeave.First(w => w.nLeaveTypeID == a.nLeaveTypeID).LeaveTypeName : "",
                               dUpdate = a.dUpdate,
                               nYear = a.nYear
                           });

            #region Filter

            if (param.nEmployeeID > 0)
            {
                lstData = lstData.Where(w => w.nEmployeeID == param.nEmployeeID);
            }
            if (param.nLeaveTypeID > 0)
            {
                lstData = lstData.Where(w => w.nLeaveTypeID == param.nLeaveTypeID);
            }
            if (param.nIntoMoney > 0)
            {
                lstData = lstData.Where(w => w.nIntoMoney == param.nIntoMoney);
            }
            if (param.nLeaveRemain > 0)
            {
                lstData = lstData.Where(w => w.nLeaveRemain == param.nLeaveRemain);
            }
            if (param.nYear > 0)
            {
                lstData = lstData.Where(w => w.nYear == param.nYear);
            }
            if (!string.IsNullOrEmpty(param.sYearWork))
            {
                lstData = lstData.Where(w => (w.sYear).Trim().ToLower().Contains((param.sYearWork).Trim().ToLower()));
            }

            #endregion
            result.lstData = lstData.OrderByDescending(o => o.dUpdate).ToList();
            result.Status = StatusCodes.Status200OK;
            return result;
        }

        public async Task<cExportExcelSummary> ExportExcelSummary(cParamExport param)
        {
            cExportExcelSummary result = new cExportExcelSummary();

            string sFileName = "LeaveSummary_" + DateTime.Now.ToString("ddMMyyyyHHmm");
            try
            {

                ResultTypeleave lstdata = await GetDataSummary(param);

                using (var db = new ST_IntranetEntity())
                {
                    var wb = new XLWorkbook();
                    IXLWorksheet ws = wb.Worksheets.Add("data");
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

                    //  var lstData = db.TB_LeaveSummary.Where(w => w.nYear == param.nYear && !w.IsDelete).ToList();
                    #region  Header
                    int nRow = 1;
                    int nStartBorder = nRow + 1;
                    if (lstdata.lstData != null)
                    {

                        List<string> lstHeader = new List<string>() { "ชื่อพนักงาน", "อายุงาน", "ประเภทการลา", "ขอคืนเป็นเงิน (วัน)", "สิทธิ์การลาคงเหลือ (วัน)" };
                        List<int> lstwidthHeader = new List<int>() { 50, 30, 30, 30, 30 };
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
                            ws.Cell(nRow, indexHead).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, indexHead).Style.Font.Bold = true;
                            ws.Cell(nRow, indexHead).Style.Font.FontColor = XLColor.White;
                            ws.Cell(nRow, indexHead).Style.Alignment.WrapText = true; //ปัดบรรทัด
                            ws.Cell(nRow, indexHead).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, indexHead).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, indexHead).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#00075c"));
                        }
                        #endregion


                        foreach (var item in lstdata.lstData)
                        {
                            nRow++; //ขึ้นบรรทัดใหม่

                            ws.Cell(nRow, 1).Value = item.sEmployeeName;
                            ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 1).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 1).Style.Alignment.WrapText = true;

                            ws.Cell(nRow, 2).Value = item.sYear;
                            ws.Cell(nRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 2).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 2).Style.Alignment.WrapText = true;

                            ws.Cell(nRow, 3).Value = item.sLeaveTypeName;
                            ws.Cell(nRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 3).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 3).Style.Alignment.WrapText = true;

                            ws.Cell(nRow, 4).Value = item.nIntoMoney;
                            ws.Cell(nRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 4).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 4).Style.Alignment.WrapText = true;

                            ws.Cell(nRow, 5).Value = item.nLeaveRemain;
                            ws.Cell(nRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 5).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 5).Style.Alignment.WrapText = true;

                            ws.Cell(nRow, 20).Value = item.sID;
                        }
                    }
                    else
                    {
                        // No data
                        nRow++;
                        ws.Range("A" + nRow + ":E" + nRow).Merge();
                        ws.Cell(nRow, 1).Value = "ไม่พบข้อมูล";
                        ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }
                    //ระยะเส้นขอบที่ต้องการให้แสดง
                    ws.Range(nStartBorder, 1, nRow, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(nStartBorder, 1, nRow, 5).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
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
            catch (Exception ex)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultTypeleave> CheckAlreadyStart(ParamCheck param)
        {
            ResultTypeleave result = new ResultTypeleave();
            try
            {
                AlreadyDTO already = new AlreadyDTO();
                var LeaveType = db.TB_LeaveType.FirstOrDefault(w => w.sLeaveTypeCode == param.sLeaveTypeCode);
                if (LeaveType != null)
                {
                    var check = db.TB_LeaveSetting.FirstOrDefault(w => !w.IsDelete && w.nLeaveTypeID == LeaveType.nLeaveTypeID && w.nEmployeeTypeID == param.nEmployeeTypeID && (w.nWorkingAgeEnd >= param.nWorkingAgeStart || w.nWorkingAgeStart >= param.nWorkingAgeStart));

                    if (check != null)
                    {
                        already.IsAlready = check != null;
                        already.nWorkingAgeStart = param.nWorkingAgeEnd;
                    }
                }
                result.Data = already;
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception ex)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultTypeleave> CheckAlreadyEnd(ParamCheck param)
        {
            ResultTypeleave result = new ResultTypeleave();
            try
            {
                AlreadyDTO already = new AlreadyDTO();
                var LeaveType = db.TB_LeaveType.FirstOrDefault(w => w.sLeaveTypeCode == param.sLeaveTypeCode);
                if (LeaveType != null)
                {
                    var check = db.TB_LeaveSetting.FirstOrDefault(w => !w.IsDelete && w.nLeaveTypeID == LeaveType.nLeaveTypeID && w.nEmployeeTypeID == param.nEmployeeTypeID && (w.nWorkingAgeEnd >= param.nWorkingAgeEnd || w.nWorkingAgeStart >= param.nWorkingAgeEnd));

                    if (check != null)
                    {
                        already.IsAlready = check != null;
                        already.nWorkingAgeStart = param.nWorkingAgeEnd;
                    }
                }

                result.Data = already;
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception ex)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultTypeleave> ToggleActive(cTableTypeleave param)
        {
            ResultTypeleave result = new ResultTypeleave();
            try
            {
                var objData = db.TB_LeaveType.FirstOrDefault(w => w.nLeaveTypeID == param.nLeaveTypeID);
                if (objData != null)
                {
                    objData.IsActive = param.isActive;
                }
                db.SaveChanges();
                result.Status = StatusCodes.Status200OK;
            }
            catch
            {
                result.Status = StatusCodes.Status500InternalServerError;

            }
            return result;
        }


        public async Task<ResultTypeleave> GetData_Dashboard(cParamSearch Param)
        {
            ResultTypeleave result = new ResultTypeleave();
            try
            {
                var UserAccount = _authen.GetUserAccount();
                int nEmployeeUser = UserAccount.nUserID;
                List<int> lstLeaveType = Param.lstLeaveType != null ? Param.lstLeaveType.ConvertAll(item => item.toInt()).ToList() : new List<int>(new int[] { });
                var lstTypeleave = db.TB_LeaveSummary.Where(w => !w.IsDelete && w.nLeaveUse > 0).ToList();
                int nNumber = 5;
                if (Param.nNumber > 0)
                {
                    nNumber = Param.nNumber;
                }
                #region Getdata Graph1
                if (Param.lstLeaveType != null && Param.nYear > 0)
                {
                    lstTypeleave = db.TB_LeaveSummary.Where(w => !w.IsDelete && w.nLeaveUse > 0 && lstLeaveType.Contains(w.nLeaveTypeID) && w.nYear == Param.nYear).ToList();

                }
                if (Param.lstLeaveType != null)
                {
                    lstTypeleave = db.TB_LeaveSummary.Where(w => !w.IsDelete && w.nLeaveUse > 0 && lstLeaveType.Contains(w.nLeaveTypeID)).ToList();

                }
                if (Param.nYear > 0)
                {
                    lstTypeleave = db.TB_LeaveSummary.Where(w => !w.IsDelete && w.nLeaveUse > 0 && w.nYear == Param.nYear).ToList();

                }

                var lstLeaveGroup = lstTypeleave.Where(w => !w.IsDelete).GroupBy(g => new { g.nEmployeeID }).Select(s => new cTableLeaveSummary
                {
                    nEmployeeID = s.Key.nEmployeeID
                }).Select(s => s.nEmployeeID);

                var lstType = db.TB_LeaveType.Where(w => w.IsActive && !w.IsDelete).ToList();
                List<TB_Employee> lstEmp = db.TB_Employee.Where(w => lstLeaveGroup.Contains(w.nEmployeeID) && w.IsActive && !w.IsDelete && !w.IsRetire).ToList();
                List<cTableLeaveSummary> lstID = new List<cTableLeaveSummary>();
                List<cTableLeaveSummary> lstLeaveSum = new List<cTableLeaveSummary>();

                lstEmp.ForEach(f =>
                {
                    if (f.nEmployeeID != 0)
                    {
                        cTableLeaveSummary objData = new cTableLeaveSummary()
                        {
                            sID = f.nEmployeeID.ToString(),
                            nEmployeeID = f.nEmployeeID,
                            sEmployeeName = f.sNameTH + "  " + f.sSurnameTH,
                            nLeaveUseTotal = lstTypeleave.Where(w => w.nEmployeeID == f.nEmployeeID).Select(s => s.nLeaveUse).Sum(),
                        };
                        lstID.Add(objData);


                        objData.lstLeaveDetail = new List<LeaveSummeryData>();
                        var lstLeaveDay = lstTypeleave.Where(w => w.nEmployeeID == f.nEmployeeID).ToList();

                        foreach (var item in lstLeaveDay)
                        {
                            LeaveSummeryData objLeaveCount = new LeaveSummeryData();
                            var objType = lstType;
                            if (objType != null)
                            {
                                objLeaveCount.nLeaveTypeID = item.nLeaveTypeID;
                                objLeaveCount.nEmployeeID = item.nEmployeeID;
                                objLeaveCount.nLeaveUse = item.nLeaveUse;
                                objLeaveCount.nLeaveRemain = item.nLeaveRemain;
                                objLeaveCount.sLeaveTypeName = objType.Any(w => w.nLeaveTypeID == item.nLeaveTypeID) ? objType.First(w => w.nLeaveTypeID == item.nLeaveTypeID).LeaveTypeName : "";
                            }
                            objData.lstLeaveDetail.Add(objLeaveCount);
                        }
                        lstLeaveSum.Add(objData);
                    }
                });
                result.lstData = lstLeaveSum.OrderByDescending(o => o.nLeaveUseTotal).Take(nNumber).ToList();
                #endregion

                #region Getdata Graph2
                List<cTableLeaveSummary> lstMonth = new List<cTableLeaveSummary>();
                List<cTableLeaveSummary> lstLeaveRequestSum = new List<cTableLeaveSummary>();
                var objType = lstType;
                var lstRequest_Leave = db.TB_Request_Leave.Where(w => !w.IsDelete && w.nStatusID == (int)Enum.EnumLeave.Status.Complete).ToList();
                DateTime? dStartreq = Convert.ToDateTime(Param.sStartreq, CultureInfo.InvariantCulture);
                DateTime? dEndreq = Convert.ToDateTime(Param.sEndreq, CultureInfo.InvariantCulture);
                List<int> lstLeaveTypereq = Param.lstLeaveTypereq != null ? Param.lstLeaveTypereq.ConvertAll(item => item.toInt()).ToList() : new List<int>(new int[] { });
                //Search
                if (Param.lstLeaveTypereq != null && Param.sStartreq != null && Param.sEndreq != null)
                {
                    lstRequest_Leave = lstRequest_Leave.Where(w => lstLeaveTypereq.Contains(w.nLeaveTypeID) &&
                    w.dStartDateTime.Date >= dStartreq &&
                    w.dStartDateTime.Date <= dEndreq).ToList();
                }
                else if (Param.lstLeaveTypereq != null && Param.sStartreq != null)
                {
                    lstRequest_Leave = lstRequest_Leave.Where(w => lstLeaveTypereq.Contains(w.nLeaveTypeID) &&
                    w.dStartDateTime.Date >= dStartreq
                   ).ToList();
                }
                else if (Param.lstLeaveTypereq != null && Param.sEndreq != null)
                {
                    lstRequest_Leave = lstRequest_Leave.Where(w => lstLeaveTypereq.Contains(w.nLeaveTypeID) &&
                    w.dStartDateTime.Date <= dEndreq).ToList();
                }

                else if (Param.lstLeaveTypereq != null)
                {
                    lstRequest_Leave = lstRequest_Leave.Where(w => lstLeaveTypereq.Contains(w.nLeaveTypeID)).ToList();

                }
                else if (Param.sStartreq != null && Param.sEndreq != null)
                {
                    lstRequest_Leave = lstRequest_Leave.Where(w => w.dStartDateTime.Date >= dStartreq && w.dStartDateTime.Date <= dEndreq).ToList();
                }
                else if (Param.sStartreq != null)
                {
                    lstRequest_Leave = lstRequest_Leave.Where(w => w.dStartDateTime.Date == dStartreq).ToList();
                }
                else if (Param.sEndreq != null)
                {
                    lstRequest_Leave = lstRequest_Leave.Where(w => w.dStartDateTime.Date == dEndreq).ToList();
                }

                var lstRequest = lstRequest_Leave.Where(w => !w.IsDelete && w.nStatusID == (int)Enum.EnumLeave.Status.Complete).GroupBy(g => new { g.dStartDateTime.Month, g.nLeaveTypeID }).ToList();


                lstRequest.ForEach(f =>
           {
               if (f.Key.nLeaveTypeID != 0)
               {
                   cTableLeaveSummary objData = new cTableLeaveSummary()
                   {
                       nLeaveTypeID = f.Key.nLeaveTypeID,
                       nMonth = f.Key.Month,
                       sLeaveTypeName = objType.Any(w => w.nLeaveTypeID == f.Key.nLeaveTypeID) ? objType.First(w => w.nLeaveTypeID == f.Key.nLeaveTypeID).LeaveTypeName : "",
                       sMonth = lstRequest_Leave.Any(w => w.dStartDateTime.Month == f.Key.Month) ? lstRequest_Leave.First(w => w.dStartDateTime.Month == f.Key.Month).dStartDateTime.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyyMMMM, ST.INFRA.Enum.CultureName.th_TH) : "",

                   };
                   lstMonth.Add(objData);

                   objData.lstLeaveDetail = new List<LeaveSummeryData>();
                   var lstLeave = lstRequest_Leave.Where(w => w.dStartDateTime.Month == f.Key.Month && w.nLeaveTypeID == f.Key.nLeaveTypeID).ToList();

                   var nLeaveUse = lstLeave.Where(w => w.nLeaveTypeID == f.Key.nLeaveTypeID).Select(s => s.nLeaveUse).Sum();
                   var lstLeaveType = lstLeave.GroupBy(g => new { g.nLeaveTypeID, g.dStartDateTime.Month });

                   foreach (var item in lstLeaveType)
                   {

                       LeaveSummeryData objLeaveCount = new LeaveSummeryData();
                       objLeaveCount.nLeaveTypeID = item.Key.nLeaveTypeID;
                       objLeaveCount.sLeaveTypeName = objType.Any(w => w.nLeaveTypeID == item.Key.nLeaveTypeID) ? objType.First(w => w.nLeaveTypeID == item.Key.nLeaveTypeID).LeaveTypeName : "";
                       objLeaveCount.nLeaveUse = nLeaveUse;
                       objData.lstLeaveDetail.Add(objLeaveCount);
                   }
                   lstLeaveRequestSum.Add(objData);
               }
           });
                result.lstDataRequest = lstLeaveRequestSum.ToList();
                #endregion

                #region Getdata Graph3
                List<int> lstLeaveTypeemp = Param.lstLeaveTypeemp != null ? Param.lstLeaveTypeemp.ConvertAll(item => item.toInt()).ToList() : new List<int>(new int[] { });
                List<int> lstEmployeeEmp = Param.lstEmployeeID != null ? Param.lstEmployeeID.ConvertAll(item => item.toInt()).ToList() : new List<int>(new int[] { });
                if (Param.lstLeaveTypeemp != null && Param.lstEmployeeID != null && Param.nYearEmp > 0)
                {
                    lstTypeleave = db.TB_LeaveSummary.Where(w => !w.IsDelete && w.nLeaveUse > 0 &&
                     lstLeaveTypeemp.Contains(w.nLeaveTypeID) &&
                      lstEmployeeEmp.Contains(w.nEmployeeID) &&
                      w.nYear == Param.nYearEmp
                      ).ToList();
                }
                else if (Param.lstLeaveTypeemp != null && Param.nYearEmp > 0)
                {
                    lstTypeleave = db.TB_LeaveSummary.Where(w => !w.IsDelete && w.nLeaveUse > 0 &&
                     lstLeaveTypeemp.Contains(w.nLeaveTypeID) &&
                       w.nYear == Param.nYearEmp
                     ).ToList();
                }
                else if (Param.lstEmployeeID != null && Param.nYearEmp > 0)
                {
                    lstTypeleave = db.TB_LeaveSummary.Where(w => !w.IsDelete && w.nLeaveUse > 0 &&
                     lstEmployeeEmp.Contains(w.nEmployeeID) &&
                       w.nYear == Param.nYearEmp
                     ).ToList();

                }
                else if (Param.lstLeaveTypeemp != null)
                {
                    lstTypeleave = db.TB_LeaveSummary.Where(w => !w.IsDelete && w.nLeaveUse > 0 && lstLeaveTypeemp.Contains(w.nLeaveTypeID)).ToList();
                }
                else if (Param.lstEmployeeID != null)
                {
                    lstTypeleave = db.TB_LeaveSummary.Where(w => !w.IsDelete && w.nLeaveUse > 0 && lstEmployeeEmp.Contains(w.nEmployeeID)).ToList();

                }
                else if (Param.nYearEmp > 0)
                {
                    lstTypeleave = db.TB_LeaveSummary.Where(w => !w.IsDelete && w.nLeaveUse > 0 && w.nYear == Param.nYearEmp).ToList();
                }

                List<TB_Employee> lstEmployee = db.TB_Employee.Where(w => w.IsActive && !w.IsDelete && !w.IsRetire).ToList();
                var Report_To = db.TB_Employee_Report_To.FirstOrDefault(w => lstEmployeeEmp.Contains(w.nRepEmployeeID ?? 0));
                if (Param.nEmployeeID > 0)
                {
                    lstEmployee = db.TB_Employee.Where(w => lstEmployeeEmp.Contains(w.nEmployeeID) && w.IsActive && !w.IsDelete && !w.IsRetire).ToList();
                }
                else if (Report_To != null)
                {
                    var Report = db.TB_Employee_Report_To.Where(w => lstEmployeeEmp.Contains(w.nRepEmployeeID ?? 0)).Select(s => s.nEmployeeID);
                    lstEmployee = db.TB_Employee.Where(w => Report.Contains(w.nEmployeeID) && w.IsActive && !w.IsDelete && !w.IsRetire).ToList();
                }
                else
                {
                    lstEmployee = db.TB_Employee.Where(w => lstEmployeeEmp.Contains(w.nEmployeeID) && w.IsActive && !w.IsDelete && !w.IsRetire).ToList();
                }

                List<cTableLeaveSummary> lstEmployeeID = new List<cTableLeaveSummary>();
                List<cTableLeaveSummary> lstLeaveTotal = new List<cTableLeaveSummary>();

                lstEmployee.ForEach(f =>
                {
                    if (f.nEmployeeID != 0)
                    {
                        cTableLeaveSummary objData = new cTableLeaveSummary()
                        {
                            sID = f.nEmployeeID.ToString(),
                            nEmployeeID = f.nEmployeeID,
                            sEmployeeName = f.sNameTH + "  " + f.sSurnameTH,
                            nLeaveUseTotal = lstTypeleave.Where(w => w.nEmployeeID == f.nEmployeeID).Select(s => s.nLeaveUse).Sum(),
                        };
                        lstEmployeeID.Add(objData);

                        objData.lstLeaveDetail = new List<LeaveSummeryData>();
                        var lstLeaveDay = lstTypeleave.Where(w => w.nEmployeeID == f.nEmployeeID).ToList();
                        foreach (var item in lstLeaveDay)
                        {
                            LeaveSummeryData objLeaveCount = new LeaveSummeryData();
                            var objType = lstType;
                            if (objType != null)
                            {
                                objLeaveCount.nLeaveTypeID = item.nLeaveTypeID;
                                objLeaveCount.nEmployeeID = item.nEmployeeID;
                                objLeaveCount.nLeaveUse = item.nLeaveUse;
                                objLeaveCount.nLeaveRemain = item.nLeaveRemain;
                                objLeaveCount.sLeaveTypeName = objType.Any(w => w.nLeaveTypeID == item.nLeaveTypeID) ? objType.First(w => w.nLeaveTypeID == item.nLeaveTypeID).LeaveTypeName : "";
                            }
                            objData.lstLeaveDetail.Add(objLeaveCount);
                        }
                        lstLeaveTotal.Add(objData);
                    }
                });

                result.lstDataSummary = lstLeaveTotal.OrderByDescending(o => o.nLeaveUseTotal).ToList();

                #endregion
                result.Status = StatusCodes.Status200OK;
            }
            catch
            {
                result.Status = StatusCodes.Status500InternalServerError;

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

        public async Task<ResultAPI> SyncDataTypeleave(cSysTypeleave Param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                var UserAccount = _authen.GetUserAccount();
                var lstType = db.TB_LeaveType.Where(w => !w.IsDelete && w.IsActive).ToList();
                int nUserID = UserAccount.nUserID != null ? UserAccount.nUserID : 999;
                int nSummarryID = (db.TB_LeaveSummary.Any() ? db.TB_LeaveSummary.Max(m => m.nLeaveSummaryID) : 0) + 1;
                List<int> lstEmployee = Param.lstEmployee != null ? Param.lstEmployee.ConvertAll(item => item.toInt()).ToList() : new List<int>(new int[] { });
                var lstemployee = db.TB_Employee.Where(w => !w.IsDelete && w.IsActive && lstEmployee.Contains(w.nEmployeeID) && w.dPromote != null || w.dWorkStart != null && !w.IsRetire);

                foreach (var f in lstType)
                {
                    decimal nLeaveRights = 0;
                    int nMonthSetting = 0;
                    var objInfo = db.TB_LeaveSummary.Where(w => w.nQuantity >= 0 && w.nLeaveRemain >= 0 && w.nYear == Param.nYear && w.nLeaveTypeID == f.nLeaveTypeID && lstemployee.Select(s => s.nEmployeeID).Contains(w.nEmployeeID) && !w.IsDelete).ToList();
                    var lstLeaveProportion = db.TB_LeaveProportion.Where(w => w.nLeaveTypeID == f.nLeaveTypeID).ToList();
                    var LeaveSetting = db.TB_LeaveSetting.Where(w => w.nLeaveTypeID == f.nLeaveTypeID && !w.IsDelete).ToList();

                    foreach (var item in lstemployee.Where(w => w.sSex == f.sSex || 87 == f.sSex))
                    {
                        int nWorksYear = item.dPromote != null ? item.dPromote.Value.Year : item.dWorkStart.Value.Year;
                        int nYear = (DateTime.Now.Year - nWorksYear) * 12;
                        int nWorksMonth = item.dPromote != null ? item.dPromote.Value.Month : item.dWorkStart.Value.Month;
                        nMonthSetting = (DateTime.Now.Month - nWorksMonth) + nYear;

                        var TB_LeaveProportion = lstLeaveProportion.FirstOrDefault(w => w.nLeaveTypeID == f.nLeaveTypeID && w.nEmployeeTypeID == item.nEmployeeTypeID);
                        if (TB_LeaveProportion != null && nMonthSetting > 12 && nMonthSetting < 24)
                        {
                            switch (nWorksMonth)
                            {
                                case 12:
                                    nLeaveRights = TB_LeaveProportion.nDec;
                                    break;
                                case 11:
                                    nLeaveRights = TB_LeaveProportion.nNov;
                                    break;
                                case 10:
                                    nLeaveRights = TB_LeaveProportion.nOct;
                                    break;
                                case 9:
                                    nLeaveRights = TB_LeaveProportion.nSep;
                                    break;
                                case 8:
                                    nLeaveRights = TB_LeaveProportion.nAug;
                                    break;
                                case 7:
                                    nLeaveRights = TB_LeaveProportion.nJul;
                                    break;
                                case 6:
                                    nLeaveRights = TB_LeaveProportion.nJun;
                                    break;
                                case 5:
                                    nLeaveRights = TB_LeaveProportion.nMay;
                                    break;
                                case 4:
                                    nLeaveRights = TB_LeaveProportion.nApr;
                                    break;
                                case 3:
                                    nLeaveRights = TB_LeaveProportion.nMar;
                                    break;
                                case 2:
                                    nLeaveRights = TB_LeaveProportion.nFeb;
                                    break;
                                case 1:
                                    nLeaveRights = TB_LeaveProportion.nJan;
                                    break;
                            }
                        }
                        else
                        {
                            var objLeaveSetting = LeaveSetting.FirstOrDefault(w => w.nLeaveTypeID == f.nLeaveTypeID && nMonthSetting >= w.nWorkingAgeStart && nMonthSetting <= w.nWorkingAgeEnd && w.nEmployeeTypeID == item.nEmployeeTypeID);
                            if (objLeaveSetting != null)
                            {
                                nLeaveRights = objLeaveSetting.nLeaveRights;
                            }
                            else
                            {
                                nLeaveRights = 0;
                            }
                        }


                        var objInfonew = objInfo.FirstOrDefault(f => f.nEmployeeID == item.nEmployeeID);
                        if (objInfonew == null)
                        {
                            objInfonew = new TB_LeaveSummary();
                            objInfonew.nLeaveSummaryID = nSummarryID;
                            objInfonew.nEmployeeID = item.nEmployeeID;
                            objInfonew.nLeaveTypeID = f.nLeaveTypeID;
                            objInfonew.dCreate = DateTime.Now;
                            objInfonew.nCreateBy = nUserID;
                            objInfonew.nYear = Param.nYear;
                            objInfonew.nQuantity = nLeaveRights;
                            objInfonew.nTransferred = 0;
                            objInfonew.nLeaveUse = 0;
                            objInfonew.nIntoMoney = 0;
                            objInfonew.nLeaveRemain = nLeaveRights;
                            objInfonew.dUpdate = DateTime.Now;
                            objInfonew.nUpdateBy = nUserID;
                            objInfonew.IsDelete = false;
                            db.TB_LeaveSummary.Add(objInfonew);
                        }
                        else if (objInfonew != null && objInfonew.nQuantity == 0 && objInfonew.nLeaveRemain == 0)
                        {
                            objInfonew.nQuantity = nLeaveRights;
                            objInfonew.nTransferred = 0;
                            objInfonew.nLeaveUse = 0;
                            objInfonew.nIntoMoney = 0;
                            objInfonew.nLeaveRemain = nLeaveRights;
                            objInfonew.dUpdate = DateTime.Now;
                            objInfonew.nUpdateBy = nUserID;
                            objInfonew.IsDelete = false;
                        }

                        nSummarryID += 1;
                    }
                    db.SaveChanges();

                }
                db.SaveChanges();

                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception ex)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;

            }
            return result;
        }


    }

}

