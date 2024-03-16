// using DocumentFormat.OpenXml.Drawing.Diagrams;
// using DocumentFormat.OpenXml.Wordprocessing;
using Extensions.Common.STResultAPI;
using ST.INFRA;
using ST.INFRA.Common;
using Extensions.Common.STFunction;
using System.Drawing;
using System.Globalization;
using Backend.Interfaces;
using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Backend.Models;
using static Backend.Enum.EnumEmployee;
using ResultAPI = Backend.Models.ResultAPI;
using Extensions.Common.STExtension;
using static Extensions.Systems.AllClass;
using Spire.Doc;
using System.Drawing.Drawing2D;
using Spire.Doc.Documents;
using Backend.Models.Authentication;
using Backend.Enum;

namespace Backend.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _auth;
        private readonly IHostEnvironment _env;
        public EmployeeService(ST_IntranetEntity db, IAuthentication auth, IHostEnvironment env)
        {
            _db = db;
            _auth = auth;
            _env = env;
        }

        public cResultEmployee GetInitData()
        {
            cResultEmployee result = new cResultEmployee();
            var UserAccount = _auth.GetUserAccount();
            int nUserID = UserAccount.nUserID;
            try
            {
                result.lstPosition = _db.TB_Position.Where(w => w.IsActive == true).Select(s => new cSelectOption
                {
                    value = s.nPositionID.ToString(),
                    label = (s.sPositionName ?? ""),
                }).ToList();

                result.lstMilitaryStatus = _db.TM_Data.Where(w => w.IsDelete != true && w.nDatatypeID == (int)DataTypeID.nStatus).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.lstMilitaryConditions = _db.TM_Data.Where(w => w.IsDelete != true && w.nDatatypeID == (int)DataTypeID.nMilitaryStatus).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.lstSex = _db.TM_Data.Where(w => w.IsDelete != true && w.nDatatypeID == (int)DataTypeID.nSex && w.nData_ID != 87).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.lstReligion = _db.TM_Data.Where(w => w.IsDelete != true && w.nDatatypeID == (int)DataTypeID.nReligion).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.lstNationality = _db.TM_Data.Where(w => w.IsDelete != true && w.nDatatypeID == (int)DataTypeID.nNationality).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.lstEthnicity = _db.TM_Data.Where(w => w.IsDelete != true && w.nDatatypeID == (int)DataTypeID.nRace).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.lstHousingType = _db.TM_Data.Where(w => w.IsDelete != true && w.nDatatypeID == (int)DataTypeID.nHousingType).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.lstEducational_Level = _db.TM_Data.Where(w => w.IsDelete != true && w.nDatatypeID == (int)DataTypeID.nEducational_Level).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.lstRelationship = _db.TM_Data.Where(w => w.IsDelete != true && w.nDatatypeID == (int)DataTypeID.nRelationship).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.lstYesNo = _db.TM_Data.Where(w => w.IsDelete != true && w.nDatatypeID == (int)DataTypeID.nYesNo).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.lstProvince = _db.TM_Provinces.Select(s => new cSelectOption
                {
                    value = s.nProvinceID.ToString(),
                    label = (s.sProvinceNameTH ?? s.sProvinceNameEN),
                }).ToList();

                result.lstDistrict = _db.TM_District.Select(s => new cSelectDataOption
                {
                    value = s.nDistrictID.ToString(),
                    label = (s.sDistrictNameTH ?? s.sDistrictNameTH),
                    nHeadID = s.nProvinceID
                }).ToList();

                result.lstSubDistrict = _db.TM_Subdistrict.Select(s => new cSelectDataOption
                {
                    value = s.sSubDistrictID,
                    label = (s.sSubdistrictNameTH ?? s.sSubdistrictNameEN),
                    nHeadID = s.nDistrictID,
                    nZip_code = s.nZip_code,
                }).ToList();
                result.lstTypeEmployee = _db.TM_Data.Where(w => !w.IsDelete && w.nDatatypeID == (int)DataTypeID.nTypeEmployee).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();
                result.lstAddressType = _db.TM_Data.Where(w => !w.IsDelete && w.nDatatypeID == (int)DataTypeID.nTypeAddress).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }

        #region Employee List
        public ResultAPI GetInitEmployeeList()
        {
            ReqEmpName result = new ReqEmpName();
            var UserAccount = _auth.GetUserAccount();
            int nUserID = UserAccount.nUserID;
            try
            {
                result.lstEmployee = _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete).Select(s => new cSelectOption
                {
                    value = s.nEmployeeID + "",
                    label = $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})",
                }).ToList();

                bool IsHR = _db.TB_Employee_Position.Any(a => a.nEmployeeID == nUserID && a.nPositionID == (int)EnumWFH.LineApprover.HR);
                List<string> lstUser = new List<string>();
                if (!IsHR)
                {
                    lstUser.Add(nUserID + "");
                }
                result.lstSelectEmployee = lstUser;

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }
        public cReturnEmployee GetDataEmployee(cReqEmployee param)
        {
            cReturnEmployee result = new cReturnEmployee();
            try
            {
                int nImageType = (int)DataTypeID.nImageType;
                IQueryable<TB_Employee>? TB_Employee = _db.TB_Employee.Where(w => !w.IsDelete).AsQueryable();
                IQueryable<TM_Data>? TM_Data = _db.TM_Data.Where(w => !w.IsDelete).AsQueryable();
                IQueryable<TB_Employee_Image>? lstImage = _db.TB_Employee_Image.Where(w => w.nImageType == nImageType && !w.IsDelete).AsQueryable();
                IQueryable<TB_Employee_Position>? lstEmpPosition = _db.TB_Employee_Position.Where(w => !w.IsDelete).AsQueryable();
                IQueryable<TB_Position>? lstPosition = _db.TB_Position.Where(w => !w.IsDelete).AsQueryable();
                bool isActive = param.nStatus == 1 ? true : param.nStatus == 2 ? false : true;
                List<ObjectResultEmployee>? lstData = (from a in TB_Employee.Where(w => (param.nStatus > 0 ? w.IsActive == isActive : true))
                                                       from b in TM_Data.Where(w => (a != null ? w.nData_ID == a.nEmployeeTypeID : false)).DefaultIfEmpty()
                                                       from c in lstImage.Where(w => w.nEmployeeID == a.nEmployeeID).DefaultIfEmpty()
                                                       from d in lstEmpPosition.Where(w => w.nEmployeeID == a.nEmployeeID).DefaultIfEmpty()
                                                       from e in lstPosition.Where(w => w.nPositionID == d.nPositionID).DefaultIfEmpty()
                                                       select new ObjectResultEmployee
                                                       {
                                                           sID = a.nEmployeeID.EncryptParameter().ToString(),
                                                           nEmpID = a.nEmployeeID,
                                                           //sFullname = a.sNameEN + " " + a.sSurnameEN ?? "",
                                                           sNickName = a.sNickname ?? " ",
                                                           sFullname = a.sNameTH + " " + a.sSurnameTH ?? " ",
                                                           sBirth = a.dBirth != null ? ((DateTime)a.dBirth).ToString("dd/MM/yyyy") : "",
                                                           sBDYear = a.dBirth != null ? ((DateTime.Now.Year) - ((DateTime)a.dBirth).Year).ToString() : "",
                                                           dWorkStart = a.dPromote ?? null,
                                                           sWorkStart = a.dPromote != null ? ((DateTime)a.dPromote).ToString("dd/MM/yyyy") : "",
                                                           IsRetire = a.IsRetire,
                                                           sRetire = a.IsRetire ? a.dRetire != null ? ((DateTime)a.dRetire).ToString("dd/MM/yyyy") : "" : "",
                                                           sEmpType = b != null ? b.sNameTH : "",
                                                           sTelephone = a.sTelephone ?? "",
                                                           sEmail = a.sEmail ?? "",
                                                           isActive = a.IsActive,
                                                           sPosition = e != null ? e.sPositionName : "",
                                                           sFileLink = STFunction.GetPathUploadFile(c.sFileName + "", c.sSystemFileName + "") ?? null,
                                                       }).ToList();
                if (param.lstSearch != null && param.lstSearch.Any())
                {
                    lstData = lstData.Where(w => param.lstSearch.Contains(w.nEmpID + "")).ToList();
                }
                foreach (var item in lstData)
                {
                    #region Timespan
                    TimeSpan? timeSpan = item.dWorkStart.HasValue ? DateTime.Now - item.dWorkStart.Value : null;
                    DateTime? total = timeSpan.HasValue ? DateTime.MinValue + timeSpan.Value : null;
                    int years = total.HasValue ? total.Value.Year - 1 : 0;
                    int months = total.HasValue ? total.Value.Month - 1 : 0;
                    int days = total.HasValue ? total.Value.Day - 1 : 0;
                    item.sTotalDate = years.ToString() + " ปี " + months.ToString() + " เดือน (" + days.ToString() + " วัน)";
                    #endregion

                    #region Img
                    TB_Employee_Image? objEmpImg = _db.TB_Employee_Image.FirstOrDefault(f => f.nEmployeeID == item.sID.DecryptParameter().toInt());

                    if (objEmpImg?.sFileName != null)
                    {
                        var sPath = STFunction.GetPathUploadFile(objEmpImg.sPath ?? "", objEmpImg.sSystemFileName ?? "");
                        item.sFileLink = sPath;
                    }
                    #endregion
                }
                STGrid.Pagination? dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, lstData.Count);
                List<ObjectResultEmployee>? lstDataCard = lstData.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();

                result.lstDataEmp = lstDataCard;
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;

        }
        #endregion

        #region Employee Form
        public cEmployeeForm GetDataEmployeeForm(string? sEmployeeID)
        {
            cEmployeeForm result = new cEmployeeForm();
            try
            {
                int nEmpID = (sEmployeeID + "").DecryptParameter().ToInt();
                TB_Employee? objEmp = _db.TB_Employee.Where(w => w.nEmployeeID == nEmpID).FirstOrDefault();
                TB_Employee_Image? objImg = _db.TB_Employee_Image.FirstOrDefault(w => w.nEmployeeID == nEmpID);
                if (objEmp != null)
                {
                    result.sEmployeeID = objEmp.nEmployeeID.EncryptParameter().ToString();
                    result.sEmplyeeCode = objEmp.sEmplyeeCode;
                    result.sUsername = objEmp.sUsername;
                    result.sPassword = objEmp.sPassword;
                    result.sNameTH = objEmp.sNameTH != null ? objEmp.sNameTH : "";
                    result.sSurnameTH = objEmp.sSurnameTH != null ? objEmp.sSurnameTH : "";
                    result.sNameEN = objEmp.sNameEN != null ? objEmp.sNameEN : "";
                    result.sSurnameEN = objEmp.sSurnameEN != null ? objEmp.sSurnameEN : "";
                    result.sNickname = objEmp.sNickname;
                    result.sSex = objEmp.sSex.ToString();
                    //result.sPositionName = (from a in _db.TB_Employee_Position.Where(w => w.nEmployeeID == nEmpID)
                    //                        from b in _db.TB_Position.Where(w => w.nPositionID == a.nPositionID)
                    //                        select new { b.sPositionName }).FirstOrDefault().ToString();
                    result.dBirthday = objEmp.dBirth;
                    result.sEthnicity = objEmp.nRace.ToString();
                    result.sReligion = objEmp.nReligion.ToString();
                    result.sNationality = objEmp.nNationality.ToString();
                    result.sMaritalStatus = objEmp.nMaritalStatus.ToString();
                    result.sIDCard = objEmp.nIDCard != null ? objEmp.nIDCard : "";
                    result.nHeight = objEmp.nHeight;
                    result.nWeight = objEmp.nWeight;
                    result.sTelephone = objEmp.sTelephone;
                    result.sEmail = objEmp.sEmail;
                    result.sMilitaryConditions = objEmp.nMilitaryConditions.ToString();
                    result.sPresentAddress = _db.TB_Employee_Address.Where(w => w.nEmployeeID == nEmpID).Select(s => s.sPresentAddress).FirstOrDefault();
                    result.sMoo = _db.TB_Employee_Address.Where(w => w.nEmployeeID == nEmpID).Select(s => s.sMoo).FirstOrDefault();
                    result.sRoad = _db.TB_Employee_Address.Where(w => w.nEmployeeID == nEmpID && w.IsActive == true && w.IsDelete != true).Select(s => s.sRoad).FirstOrDefault();
                    result.sProvinceID = _db.TB_Employee_Address.Where(w => w.nEmployeeID == nEmpID && w.IsActive == true && w.IsDelete != true).Select(s => s.nProvinceID).FirstOrDefault().ToString();
                    result.sDistrictID = _db.TB_Employee_Address.Where(w => w.nEmployeeID == nEmpID && w.IsActive == true && w.IsDelete != true).Select(s => s.nDistrictID).FirstOrDefault().ToString();
                    result.sSubDistrictID = _db.TB_Employee_Address.Where(w => w.nEmployeeID == nEmpID && w.IsActive == true && w.IsDelete != true).Select(s => s.sSubDistrictID).FirstOrDefault();
                    result.sAdressType = _db.TB_Employee_Address.Where(w => w.nEmployeeID == nEmpID && w.IsActive == true && w.IsDelete != true).Select(s => s.nAdressType).FirstOrDefault().ToString();
                    result.nPostcode = _db.TB_Employee_Address.Where(w => w.nEmployeeID == nEmpID && w.IsActive == true && w.IsDelete != true).Select(s => s.nPostcode).FirstOrDefault();
                    result.IsActive = objEmp.IsActive;

                    TB_Employee_Image? objEmpImg = _db.TB_Employee_Image.FirstOrDefault(f => f.nEmployeeID == nEmpID);
                    List<ItemFileData> lstfile = new List<ItemFileData>();

                    if (objEmpImg?.sFileName != null)
                    {
                        ItemFileData fFile = new ItemFileData();
                        var sPath = STFunction.GetPathUploadFile(objEmpImg.sPath ?? "", objEmpImg.sSystemFileName ?? "");

                        fFile.sFileName = objEmpImg.sFileName;
                        fFile.sCropFileLink = sPath;
                        fFile.sFileLink = sPath;
                        fFile.sSysFileName = objEmpImg.sSystemFileName;
                        fFile.sFileType = objEmpImg.sSystemFileName != null ? objEmpImg.sSystemFileName.Split(".")[1] + "" : null;
                        fFile.sFolderName = objEmpImg.sPath;
                        fFile.IsNew = false;
                        fFile.IsNewTab = false;
                        fFile.IsCompleted = true;
                        fFile.IsDelete = false;
                        fFile.IsProgress = false;
                        fFile.sProgress = "100";
                        lstfile.Add(fFile);
                    }
                    result.lstFile = lstfile.Count() > 0 ? lstfile : null;

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

        public ResultAPI SaveDataEmployee(cEmployeeForm param)
        {

            ResultAPI objResult = new ResultAPI();
            try
            {
                var UserAccount = _auth.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                int nEmployeeID = (param.sEmployeeID ?? "").DecryptParameter().ToInt();
                #region SaveTB_Employee
                TB_Employee? objEmp = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == nEmployeeID);
                if (objEmp == null)  //Add
                {
                    bool isDuplicateAdd = _db.TB_Employee.Where(w => w.nEmployeeID != nEmployeeID && (w.nIDCard.Trim().ToLower() == (param.sIDCard + "").Trim().ToLower())
                    || w.sUsername.Trim().ToLower() == (param.sUsername + "").Trim().ToLower()
                    || w.sEmail.Trim().ToLower() == (param.sEmail + "").Trim().ToLower()).Any();
                    if (isDuplicateAdd)
                    {
                        objResult.nStatusCode = StatusCodes.Status409Conflict;
                        objResult.sMessage = "ชื่อหรือเลขบัตรประชาชนมีอยู่แล้วในระบบ!!";
                        return objResult;
                    }
                    int newID = (_db.TB_Employee.Any() ? _db.TB_Employee.Max(m => m.nEmployeeID) : 0) + 1;
                    int nNumber = _db.TB_Employee.Where(w => (w.sEmplyeeCode + "" == param.sEmplyeeCode)).Any() ? _db.TB_Employee.Where(w => (w.sEmplyeeCode + "" == param.sEmplyeeCode)).ToList().Count() + 1 : 1;
                    string sNumber = String.Format("{0:D3}", nNumber);
                    objEmp = new TB_Employee()
                    {
                        nEmployeeID = newID,
                        sEmplyeeCode = sNumber,
                        dCreate = DateTime.Now,
                        nIDCard = param.sIDCard,
                        nCreateBy = nUserID,
                        IsActive = true,
                        IsDelete = false,
                        nEmployeeTypeID = 22 //Employeetype
                    };
                    _db.TB_Employee.Add(objEmp);
                    nEmployeeID = newID;
                }

                objEmp.sUsername = param.sUsername;
                objEmp.sPassword = param.sPassword;
                objEmp.sNameTH = param.sNameTH;
                objEmp.sSurnameTH = param.sSurnameTH;
                objEmp.sNickname = param.sNickname;
                objEmp.sNameEN = param.sNameEN;
                objEmp.sSurnameEN = param.sSurnameEN;
                objEmp.sEmail = param.sEmail;
                DateTime? dBirthday = param.sBirthday != null ? Convert.ToDateTime(param.sBirthday, CultureInfo.InvariantCulture) : null;
                objEmp.dBirth = dBirthday ?? null;
                objEmp.nHeight = param.nHeight;
                objEmp.nWeight = param.nWeight;
                objEmp.sTelephone = param.sTelephone;
                objEmp.sSex = param.sSex?.toInt();
                objEmp.nRace = param.sEthnicity?.toInt();
                objEmp.nNationality = param.sNationality?.toInt();
                objEmp.nReligion = param.sReligion?.toInt();
                objEmp.nMilitaryConditions = param.sMilitaryConditions?.toInt();
                objEmp.nMaritalStatus = param.sMaritalStatus?.toInt();
                objEmp.dUpdate = DateTime.Now;
                objEmp.nUpdateBy = nUserID;
                #endregion

                #region SaveTB_Employee_Address
                TB_Employee_Address? objEmpAdderss = _db.TB_Employee_Address.FirstOrDefault(w => w.nEmployeeID == nEmployeeID && w.IsDelete == false);
                if (objEmpAdderss == null)  //Add
                {
                    int nOrderAddess = _db.TB_Employee_Address.Where(w => w.nEmployeeID == nEmployeeID && w.IsDelete != true).Any() ?
                        _db.TB_Employee_Address.Where(w => w.nEmployeeID == nEmployeeID && w.IsDelete != true).Select(s => s.nOrder).Max() + 1 : 1;
                    objEmpAdderss = new TB_Employee_Address();
                    objEmpAdderss.nAddressID = (_db.TB_Employee_Address.Any() ? _db.TB_Employee_Address.Max(m => m.nAddressID) : 0) + 1; ;
                    objEmpAdderss.nEmployeeID = nEmployeeID;
                    objEmpAdderss.nOrder = nOrderAddess;
                    objEmpAdderss.dCreate = DateTime.Now;
                    objEmpAdderss.nCreateBy = nUserID;
                    objEmpAdderss.IsActive = true;
                    objEmpAdderss.IsDelete = false;
                    _db.TB_Employee_Address.Add(objEmpAdderss);
                }
                objEmpAdderss.nAdressType = param.sAdressType?.toInt();
                objEmpAdderss.nResidenceType = 1;
                objEmpAdderss.sPresentAddress = param.sPresentAddress;
                objEmpAdderss.sMoo = param.sMoo;
                objEmpAdderss.sRoad = param.sRoad;
                objEmpAdderss.sSubDistrictID = param.sSubDistrictID;
                objEmpAdderss.nDistrictID = param.sDistrictID?.toInt();
                objEmpAdderss.nProvinceID = param.sProvinceID?.toInt();
                objEmpAdderss.nPostcode = param.nPostcode;
                objEmpAdderss.dUpdate = DateTime.Now;
                objEmpAdderss.nUpdateBy = nUserID;

                #endregion
                #region File
                TB_Employee_Image? objEmpImage = _db.TB_Employee_Image.FirstOrDefault(w => w.nEmployeeID == nEmployeeID && w.IsDelete == false);
                if (objEmpImage == null)  //Add
                {
                    objEmpImage = new TB_Employee_Image();
                    objEmpImage.nEmployeeImageID = (_db.TB_Employee_Image.Any() ? _db.TB_Employee_Image.Max(m => m.nEmployeeImageID) : 0) + 1; ;
                    objEmpImage.nEmployeeID = nEmployeeID;
                    objEmpImage.dCreate = DateTime.Now;
                    objEmpImage.nCreateBy = nUserID;
                    objEmpImage.IsDelete = false;
                    _db.TB_Employee_Image.Add(objEmpImage);
                }
                objEmpImage.nImageType = 88;
                objEmpImage.IsDelete = false;
                objEmpImage.dUpdate = DateTime.Now;
                objEmpImage.nUpdateBy = nUserID;
                var objFile = param.lstFile?.FirstOrDefault();
                if (objFile != null)
                {
                    string pathTempContent = !string.IsNullOrEmpty(objFile.sFolderName) ? objFile.sFolderName + "/" : "Temp/ProfileCircle/";
                    string truePathFile = "Employee\\" + nEmployeeID + "\\";
                    string fPathContent = "Employee/" + nEmployeeID;

                    objFile.sFolderName = pathTempContent;
                    if (objFile.IsNew)
                    {

                        var path = STFunction.MapPath(truePathFile, _env);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var ServerTempPath = STFunction.MapPath(pathTempContent, _env);
                        var ServerTruePath = STFunction.MapPath(truePathFile, _env);

                        string FileTempPath = STFunction.Scan_CWE22_File(ServerTempPath, objFile.sSysFileName != null ? objFile.sSysFileName : "");
                        string FileTruePath = STFunction.Scan_CWE22_File(ServerTruePath, objFile.sSysFileName != null ? objFile.sSysFileName : "");
                        if (File.Exists(FileTempPath))
                        {
                            if (FileTempPath != FileTruePath)
                            {
                                File.Move(FileTempPath, FileTruePath);
                            }
                        }
                    }

                    objEmpImage.sPath = fPathContent;
                    objEmpImage.sSystemFileName = objFile.sSysFileName != null ? objFile.sSysFileName : "";
                    objEmpImage.sFileName = objFile.sSysFileName != null ? objFile.sSysFileName : "";
                }
                #endregion
                _db.SaveChanges();

                objResult.nStatusCode = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                objResult.nStatusCode = StatusCodes.Status500InternalServerError;
                objResult.sMessage = e.Message;
            }
            return objResult;
        }

        #endregion

        #region Employee Family
        public ResultAPI GetDataFamilyForm(string? sEmployeeID)
        {
            cFamily result = new cFamily();
            int nEmpID = (sEmployeeID + "").DecryptParameter().ToInt();
            try
            {
                var lstData = (from a in _db.TB_Employee_FamilyPerson.Where(w => w.nEmployeeID == nEmpID && w.IsDelete == false)
                               select new ObjectFamilyData
                               {
                                   nFamilyPersonID = a.nFamilyPersonID,
                                   sFName = a.sName ?? "",
                                   sSName = a.sSureName ?? "",
                                   nAge = a.nAge,
                                   nRelationship = a.nRelationship,
                                   sOccupation = a.sOccupation ?? "",
                                   sPosition = a.sPosition ?? "",
                                   sWorkplace = a.sWorkplace ?? "",
                                   sID = a.nFamilyPersonID.ToString(),
                                   IsDel = a.IsDelete,
                               });
                result.lstAllData = lstData.ToList();
                var TB_Employee_Family = _db.TB_Employee_Family.FirstOrDefault(w => w.IsDelete == false && w.nEmployeeID == nEmpID);
                if (TB_Employee_Family != null)
                {
                    result.sEmployeeID = TB_Employee_Family.nEmployeeID.EncryptParameter().ToString();
                    result.nTotalChild = TB_Employee_Family.nChilden.HasValue ? TB_Employee_Family.nChilden.Value : 0;
                    result.nTotalSibling = TB_Employee_Family.nSiblings.HasValue ? TB_Employee_Family.nSiblings.Value : 0;
                    result.nChildPosition = TB_Employee_Family.nChildPosition.HasValue ? TB_Employee_Family.nChildPosition.Value : 0;
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
        public ResultAPI SaveDataFamily(cFamily param)
        {
            ResultAPI objResult = new ResultAPI();
            var UserAccount = _auth.GetUserAccount();
            int nUserID = UserAccount.nUserID;
            int nEmpID = (param.sEmployeeID + "").DecryptParameter().ToInt();
            try
            {
                TB_Employee_Family? ObjectFamily = _db.TB_Employee_Family.FirstOrDefault(w => w.nEmployeeID == nEmpID && w.IsDelete != true);
                if (ObjectFamily == null)
                {
                    ObjectFamily = new TB_Employee_Family();
                    ObjectFamily.nFamilyID = (_db.TB_Employee_Family.Any() ? _db.TB_Employee_Family.Max(m => m.nFamilyID) : 0) + 1;
                    ObjectFamily.nEmployeeID = nEmpID;
                    ObjectFamily.dCreate = DateTime.Now;
                    ObjectFamily.nCreateBy = nUserID;
                    ObjectFamily.IsDelete = false;
                    _db.TB_Employee_Family.Add(ObjectFamily);
                }
                ObjectFamily.nChilden = param.nTotalChild.HasValue ? param.nTotalChild.Value : 0;
                ObjectFamily.nSiblings = param.nTotalSibling ?? null;
                ObjectFamily.nBrother = param.lstAllData != null ? param.lstAllData.Where(w => w.nRelationship == 45).Count() : 0;
                ObjectFamily.nSister = param.lstAllData != null ? param.lstAllData.Where(w => w.nRelationship == 46).Count() : 0;
                ObjectFamily.nChildPosition = param.nChildPosition ?? null;
                ObjectFamily.dUpdate = DateTime.Now;
                ObjectFamily.nUpdateBy = nUserID;
                _db.SaveChanges();

                if (param.lstAllData != null)
                {
                    foreach (var item in param.lstAllData)
                    {
                        TB_Employee_FamilyPerson? ObjectFamilyPerson = _db.TB_Employee_FamilyPerson.FirstOrDefault(w => w.nEmployeeID == nEmpID && w.nFamilyPersonID == item.nFamilyPersonID && w.IsDelete != true);
                        if (ObjectFamilyPerson == null) //Add new Data
                        {
                            ObjectFamilyPerson = new TB_Employee_FamilyPerson();
                            ObjectFamilyPerson.nFamilyPersonID = _db.TB_Employee_FamilyPerson.Any() ? _db.TB_Employee_FamilyPerson.Max(m => m.nFamilyPersonID) + 1 : 1;
                            ObjectFamilyPerson.nEmployeeID = nEmpID;
                            ObjectFamilyPerson.dCreate = DateTime.Now;
                            ObjectFamilyPerson.nCreateBy = nUserID;
                            _db.TB_Employee_FamilyPerson.Add(ObjectFamilyPerson);
                        }

                        ObjectFamilyPerson.sName = item.sFName;
                        ObjectFamilyPerson.sSureName = item.sSName;
                        ObjectFamilyPerson.nRelationship = item.nRelationship.HasValue ? item.nRelationship.Value : 0;
                        ObjectFamilyPerson.nAge = item.nAge;
                        ObjectFamilyPerson.sOccupation = item.sOccupation;
                        ObjectFamilyPerson.sWorkplace = item.sWorkplace;
                        ObjectFamilyPerson.sPosition = item.sPosition;
                        ObjectFamilyPerson.dUpdate = DateTime.Now;
                        ObjectFamilyPerson.nUpdateBy = nUserID;
                        ObjectFamilyPerson.IsDelete = item.IsDel;
                        if (item.IsDel == true)
                        {
                            ObjectFamilyPerson.dDelete = DateTime.Now;
                            ObjectFamilyPerson.nDeleteBy = nUserID;
                        }
                        _db.SaveChanges();
                    }
                }
                objResult.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                objResult.nStatusCode = StatusCodes.Status500InternalServerError;
                objResult.sMessage = e.Message;
            }
            return objResult;
        }
        #endregion

        #region Employee Language
        public ResultAPI GetDataLanguageForm(string? sEmployeeID)
        {
            int nEmpID = (sEmployeeID + "").DecryptParameter().ToInt();
            cLanguage result = new cLanguage();
            try
            {
                int idLang = (int)DataTypeID.nTypeLang;
                int idSkill_Level = (int)DataTypeID.nTypeTask;
                int idTH = (int)DataLang.Thai;
                int idEN = (int)DataLang.Eng;


                var lstDataSkill_Level = _db.TM_Data.Where(w => w.nDatatypeID == idSkill_Level).Select(a => new ObjectSkill_Level
                {
                    value = a.nData_ID.ToString(),
                    label = a.sNameTH,
                }).ToList();

                var lstDataLanquaue = _db.TB_Employee_Language.Where(w => w.IsDelete != true && w.nEmployeeID == nEmpID).Select(dt => new ObjectDataLanguage
                {
                    nLanguage = dt.nLanguage,
                    nReading = dt.nReading,
                    nSpeaking = dt.nSpeaking,
                    nWriting = dt.nWriting,
                }).ToList();

                var lstLanguage = _db.TM_Data.Where(w => w.nDatatypeID == idLang && (w.nData_ID == idTH || w.nData_ID == idEN)).Select(b => new ObjectLanguage
                {
                    sID = b.nData_ID.ToString(),
                    sName = b.sNameTH,
                    nReading = null,
                    nSpeaking = null,
                    nWriting = null,
                    lstSpeakingSkill_Level = lstDataSkill_Level,
                    lstWritingSkill_Level = lstDataSkill_Level,
                    lstReadingSkill_Level = lstDataSkill_Level,
                }).ToList();

                if (lstDataLanquaue.Count() > 0)
                {
                    lstLanguage.ForEach((f) =>
                    {
                        int nLanguageID = f.sID.ToInt();
                        var IsLanquaue = lstDataLanquaue.FirstOrDefault(g => g.nLanguage == nLanguageID);
                        if (IsLanquaue != null)
                        {
                            f.nReading = IsLanquaue.nReading;
                            f.nSpeaking = IsLanquaue.nSpeaking;
                            f.nWriting = IsLanquaue.nWriting;
                        }
                    });
                }

                result.lstLanguage = lstLanguage;
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }
        public ResultAPI SaveDataLanguage(cLanguage param)
        {
            ResultAPI objResult = new ResultAPI();
            var UserAccount = _auth.GetUserAccount();
            int nUserID = UserAccount.nUserID;
            int nEmpID = (param.sEmployeeID + "").DecryptParameter().ToInt();
            try
            {
                if (param.lstLanguage != null)
                {
                    foreach (var item in param.lstLanguage)
                    {
                        TB_Employee_Language? ObjectLanguage = _db.TB_Employee_Language.FirstOrDefault(w => w.nEmployeeID == nEmpID && w.nLanguage == item.sID.ToInt() && w.IsDelete != true);
                        if (ObjectLanguage == null)
                        {
                            ObjectLanguage = new TB_Employee_Language();
                            ObjectLanguage.nLanguageID = _db.TB_Employee_Language.Any() ? _db.TB_Employee_Language.Max(m => m.nLanguageID) + 1 : 1;
                            ObjectLanguage.nEmployeeID = nEmpID;
                            ObjectLanguage.nLanguage = item.sID.ToInt();
                            ObjectLanguage.dCreate = DateTime.Now;
                            ObjectLanguage.nCreateBy = nUserID;
                            ObjectLanguage.IsDelete = false;
                            _db.TB_Employee_Language.Add(ObjectLanguage);
                        }
                        ObjectLanguage.nReading = item.nReading;
                        ObjectLanguage.nSpeaking = item.nSpeaking;
                        ObjectLanguage.nWriting = item.nWriting;
                        ObjectLanguage.dUpdate = DateTime.Now;
                        ObjectLanguage.nUpdateBy = nUserID;
                        _db.SaveChanges();
                    }
                }
                objResult.nStatusCode = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                objResult.nStatusCode = StatusCodes.Status500InternalServerError;
                objResult.sMessage = e.Message;
            }
            return objResult;
        }
        #endregion

        #region Employee Position
        public ResultAPI GetDataPositionForm(string? sEmployeeID)
        {
            cPosition result = new cPosition();
            int nEmpID = (sEmployeeID + "").DecryptParameter().ToInt();
            try
            {
                IQueryable<TB_Employee> TB_Employee = _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive).AsQueryable();
                TB_Employee? data = TB_Employee.FirstOrDefault(w => w.nEmployeeID == nEmpID);
                if (data != null)
                {
                    result.sName = data.sEmplyeeCode + " : " + data.sNameTH + " " + data.sSurnameTH;
                    result.dWorkStart = data.dWorkStart;
                    result.dPromote = data.dPromote;
                    result.sUserType = data.nEmployeeTypeID.ToString();
                    result.dRetire = data.dRetire;
                    #region Timespan
                    TimeSpan? timeSpan = data.dPromote.HasValue ? DateTime.Now - data.dPromote.Value : null;
                    DateTime? total = timeSpan.HasValue ? DateTime.MinValue + timeSpan.Value : null;
                    int years = total.HasValue ? total.Value.Year - 1 : 0;
                    int months = total.HasValue ? total.Value.Month - 1 : 0;
                    int days = total.HasValue ? total.Value.Day - 1 : 0;
                    #endregion
                    result.sLongevity = years.ToString() + " ปี " + months.ToString() + " เดือน (" + days.ToString() + " วัน)";
                }
                TB_Employee_Position? dataPosition = _db.TB_Employee_Position.FirstOrDefault(w => w.nEmployeeID == nEmpID);
                if (dataPosition != null)
                {
                    result.sPosition = dataPosition.nPositionID != 0 ? dataPosition.nPositionID.ToString() : null;
                }
                var lstdataHistory = _db.TB_Employee_Position_History.Where(w => w.nEmployeeID == nEmpID && !w.IsDelete).Select(item => new ObjectPosition
                {
                    sID = item.PositionHistoryID.ToString(),
                    sNewPosition = item.nPositionID.ToString(),
                    sOriginalPosition = item.nPositionID.ToString(),
                    dStartDate = item.dStartDate ?? DateTime.MinValue,
                    sRemark = item.sRemark,
                    nOrder = item.nOrder,
                }).OrderBy(o => o.nOrder).ToList();

                result.lstData = lstdataHistory;

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }
        public ResultAPI SaveDataPosition(cPosition param)
        {
            ResultAPI result = new ResultAPI();
            var UserAccount = _auth.GetUserAccount();
            int nUserID = UserAccount.nUserID;
            int nEmpID = (param.sEmployeeID + "").DecryptParameter().ToInt();
            try
            {
                DateTime? dWorkStart = param.sWorkStart != null ? Convert.ToDateTime(param.sWorkStart, CultureInfo.InvariantCulture) : null;
                DateTime? dPromote = param.sPromote != null ? Convert.ToDateTime(param.sPromote, CultureInfo.InvariantCulture) : null;
                DateTime? dRetire = param.sRetire != null ? Convert.ToDateTime(param.sRetire, CultureInfo.InvariantCulture) : null;
                TB_Employee? TBEmployee = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == nEmpID && !w.IsDelete && w.IsActive);
                if (TBEmployee != null)
                {
                    TBEmployee.dWorkStart = dWorkStart != null ? dWorkStart : null;
                    TBEmployee.dPromote = dPromote != null ? dPromote : null;
                    TBEmployee.dRetire = dRetire != null ? dRetire : null;
                    TBEmployee.IsRetire = dRetire != null ? true : false;
                    TBEmployee.nEmployeeTypeID = param.sUserType != null ? param.sUserType.ToInt() : 0;
                    _db.SaveChanges();
                }
                if (param.lstData != null)
                {
                    foreach (var item in param.lstData)
                    {

                        TB_Employee_Position? TBEmployeePosition = _db.TB_Employee_Position.FirstOrDefault(w => w.nEmployeeID == nEmpID && !w.IsDelete);
                        if (TBEmployeePosition == null) //Add new Data
                        {
                            TBEmployeePosition = new TB_Employee_Position();
                            TBEmployeePosition.nEmpPositionID = _db.TB_Employee_Position.Any() ? _db.TB_Employee_Position.Max(m => m.nEmpPositionID) + 1 : 1;
                            TBEmployeePosition.nEmployeeID = nEmpID;
                            TBEmployeePosition.nLevelPosition = 0;
                            TBEmployeePosition.dCreate = DateTime.Now;
                            TBEmployeePosition.nCreateBy = nUserID;
                            TBEmployeePosition.IsDelete = false;
                            _db.TB_Employee_Position.Add(TBEmployeePosition);
                        }
                        DateTime dStartDate = Convert.ToDateTime(item.dStartDate, CultureInfo.InvariantCulture);
                        TBEmployeePosition.dStartDate = dStartDate;
                        TBEmployeePosition.nPositionID = item.sNewPosition.ToInt();
                        TBEmployeePosition.sRemark = item.sRemark;
                        TBEmployeePosition.dUpdate = DateTime.Now;
                        TBEmployeePosition.nUpdateBy = nUserID;
                        _db.SaveChanges();
                        TB_Employee_Position_History? TBEmpPositionHistory = _db.TB_Employee_Position_History.FirstOrDefault(w => w.nEmployeeID == nEmpID && w.nPositionID == item.sNewPosition.ToInt() && w.nEmpPositionID == TBEmployeePosition.nEmpPositionID && !w.IsDelete);
                        if (TBEmpPositionHistory == null) //Add new Data
                        {
                            TBEmpPositionHistory = new TB_Employee_Position_History();
                            TBEmpPositionHistory.PositionHistoryID = _db.TB_Employee_Position_History.Any() ? _db.TB_Employee_Position_History.Max(m => m.PositionHistoryID) + 1 : 1;
                            TBEmpPositionHistory.nEmpPositionID = TBEmployeePosition.nEmpPositionID;
                            TBEmpPositionHistory.nEmployeeID = nEmpID;
                            TBEmpPositionHistory.nOrder = _db.TB_Employee_Position_History.Where(w => w.nEmployeeID == nEmpID).Any() ? _db.TB_Employee_Position_History.Where(w => w.nEmployeeID == nEmpID).Max(m => m.nOrder) + 1 : 1;
                            TBEmpPositionHistory.nLevelPosition = 1;
                            TBEmpPositionHistory.dCreate = DateTime.Now;
                            TBEmpPositionHistory.nCreateBy = nUserID;
                            TBEmpPositionHistory.IsDelete = false;
                            _db.TB_Employee_Position_History.Add(TBEmpPositionHistory);
                        }
                        TBEmpPositionHistory.nPromotePositionID = 0;
                        TBEmpPositionHistory.nPositionID = item.sNewPosition.ToInt();
                        TBEmpPositionHistory.dStartDate = item.dStartDate;
                        TBEmpPositionHistory.sRemark = item.sRemark;
                        TBEmpPositionHistory.dUpdate = DateTime.Now;
                        TBEmpPositionHistory.nUpdateBy = nUserID;
                        _db.SaveChanges();
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
        #endregion

        #region Profile and export word
        /// <summary>
        /// GetProfile
        /// </summary>
        public clsProfileEmp GetProfile(string? sID)
        {
            clsProfileEmp res = new clsProfileEmp();
            UserAccount? ua = _auth.GetUserAccount();
            try
            {
                int? nID = sID.DecryptParameter().ToIntOrNull();
                int nDatatypeEduID = (int)DataTypeID.nTypeEducation;
                int nDatatypeLangID = (int)DataTypeID.nTypeLang;
                int nDatatypeTask = (int)DataTypeID.nTypeTask;
                int nImageType = (int)DataTypeID.nImageType;
                int nTypeSpecial = (int)DataTypeID.nTypeSpecial;
                int nProgramskills = (int)DataTypeID.nProgramskills;
                int nProgramlanguageskills = (int)DataTypeID.nProgramlanguageskills;
                int nNation = (int)DataTypeID.nNationality;

                IQueryable<TM_Data> TMData = _db.TM_Data.Where(w => !w.IsDelete).AsQueryable();
                IQueryable<TB_Position> TBPosition = _db.TB_Position.Where(w => !w.IsDelete).AsQueryable();
                IQueryable<TB_Employee_Position> TBEmployeePosition = _db.TB_Employee_Position.Where(w => !w.IsDelete).AsQueryable();
                IQueryable<TB_Employee_SpecialAbility> TBEmployeeSpecialAbility = _db.TB_Employee_SpecialAbility.Where(w => !w.IsDelete).AsQueryable();
                IQueryable<TB_Employee_Language> TBEmployeeLanguage = _db.TB_Employee_Language.Where(w => !w.IsDelete).AsQueryable();
                List<TM_Data>? TMDataNation = TMData.Where(w => w.nDatatypeID == nNation).ToList();
                List<TM_Data>? TMDataEdu = TMData.Where(w => w.nDatatypeID == nDatatypeEduID).ToList();
                List<TM_Data>? TMDataLang = TMData.Where(w => w.nDatatypeID == nDatatypeLangID).ToList();
                List<TM_Data>? TMDataTask = TMData.Where(w => w.nDatatypeID == nDatatypeTask).ToList();
                var sPosition = (from a in TBEmployeePosition.Where(w => w.nEmployeeID == ua.nUserID)
                                 from b in TBPosition.Where(w => w.nPositionID == a.nPositionID)
                                 select new
                                 {
                                     b
                                 });
                List<TM_Data>? TMDataSpecia = TMData.Where(w => w.nDatatypeID == nTypeSpecial && (w.nData_ID == nProgramskills || w.nData_ID == nProgramlanguageskills)).OrderBy(o => o.nData_ID).ToList();
                string dCurrent = DateTime.Now.Year.ToString();
                int nCurrent = dCurrent.ToInt();
                List<objOption> TMSubdistrict = _db.TM_Subdistrict.Select(s => new objOption
                {
                    value = s.sSubDistrictID,
                    label = s.sSubdistrictNameTH
                }).ToList();
                List<objOption> TMDistrict = _db.TM_District.Select(s => new objOption
                {
                    value = s.nDistrictID + "",
                    label = s.sDistrictNameTH
                }).ToList();
                List<objOption> TMProvinces = _db.TM_Provinces.Select(s => new objOption
                {
                    value = s.nProvinceID + "",
                    label = s.sProvinceNameTH + ""
                }).ToList();
                var sImage = _db.TB_Employee_Image.FirstOrDefault(w => w.nEmployeeID == nID && w.nImageType == nImageType && !w.IsDelete);
                var listEmp = (from a in _db.TB_Employee.Where(w => w.nEmployeeID == nID && !w.IsDelete).ToList()
                               from b in _db.TB_Employee_Address.Where(w => w.nEmployeeID == a.nEmployeeID && !w.IsDelete).DefaultIfEmpty()
                               select new
                               {
                                   a,
                                   b,
                               }).ToList();

                var listData = new List<objEmployeeInfo>();
                var listEducation = (from a in _db.TB_Employee_Education.Where(w => w.nEmployeeID == nID && !w.IsDelete).ToList()
                                     from b in TMDataEdu.Where(w => w.nData_ID == a.nEducational_Level).DefaultIfEmpty()
                                     select new
                                     {
                                         a,
                                         b
                                     });
                //  TBEmployeePosition.First(w=> w.nEmployeeID == f.a.nEmployeeID).nEmpPositionID
                foreach (var f in listEmp)
                {
                    //(TBPosition.First(d => (TBEmployeePosition.First(w => w.nEmployeeID == f.a.nEmployeeID).nEmpPositionID) == d.nPositionID).sPositionName)
                    var objRowdata = new objEmployeeInfo();
                    objRowdata.sPosition = sPosition.FirstOrDefault()?.b.sPositionName;
                    objRowdata.sNameTH = f.a?.sNameTH;
                    objRowdata.sSurnameTH = f.a?.sSurnameTH;
                    objRowdata.nEmployeeID = f.a.nEmployeeID;
                    objRowdata.sPresentAddress = f.b?.sPresentAddress;
                    objRowdata.sMoo = f.b?.sMoo;
                    objRowdata.sRoad = f.b?.sRoad;
                    objRowdata.sSubDistrict = TMSubdistrict.Any(w => w.value == f.b?.sSubDistrictID) ? TMSubdistrict.Where(w => w.value == f.b?.sSubDistrictID).Select(o => o.label).FirstOrDefault() : null;
                    objRowdata.nDistrict = TMDistrict.Any(w => w.value == f.b?.nDistrictID + "") ? TMDistrict.Where(w => w.value == f.b?.nDistrictID + "").Select(o => o.label).FirstOrDefault() : null;
                    objRowdata.nProvince = TMProvinces.Any(w => w.value == f.b?.nProvinceID + "") ? TMProvinces.Where(w => w.value == f.b?.nProvinceID + "").Select(o => o.label).FirstOrDefault() : null;
                    objRowdata.nPostcode = f.b?.nPostcode;
                    objRowdata.sEmail = f.a?.sEmail;
                    objRowdata.sTelephone = f.a?.sTelephone;
                    objRowdata.sNationality = TMDataNation.Any(w => w.nData_ID == f.a?.nNationality) ? TMDataNation.Where(w => w.nData_ID == f.a?.nNationality).Select(s => s.sNameTH).FirstOrDefault() : null;
                    objRowdata.dBirth = f.a?.dBirth ?? new DateTime();
                    objRowdata.sFileLink = sImage != null ? STFunction.GetPathUploadFile(sImage.sPath + "", sImage.sSystemFileName + "") : null;
                    objRowdata.listEducation = (List<objEmployeeInfoEducation>)(listEducation.Select(s => new objEmployeeInfoEducation
                    {
                        sEducational_Level = s.b.sNameTH + "",
                        nEducational_Level = s.a.nEducational_Level,
                        sEducationStart = s.a.nEducationStart + "",
                        sEducationEnd = nCurrent == s.a.nEducationEnd ? "PRESENT" : s.a.nEducationEnd + "",
                        sAcademy = s.a.sAcademy,
                        sMajor = s.a.sMajor,
                        nEducationEnd = s.a.nEducationEnd
                    })).OrderByDescending(o => o.nEducational_Level).ThenByDescending(t => t.nEducationEnd).ToList();
                    objRowdata.listWorkEx = (List<objEmployeeInfoWork>)_db.TB_Employee_WorkExperience.Where(w => w.nEmployeeID == nID)
                                   .Select(s => new objEmployeeInfoWork
                                   {
                                       sWorkCompany = s.sWorkCompany,
                                       sWorkStart = s.dWorkStart != null ? ((DateTime)s.dWorkStart).Year.ToString() : null,
                                       sWorkEnd = ((s.dWorkStart != null ? ((DateTime)s.dWorkStart).Year.ToString() : null) == dCurrent) ? "PRESENT" : (s.dWorkStart != null ? ((DateTime)s.dWorkStart).Year.ToString() : null),
                                       sPosition = s.sPosition,
                                       sJobDescription = s.sJobDescription,
                                       dWorkEnd = s.dWorkEnd
                                   }).OrderByDescending(o => o.dWorkEnd).Take(3).ToList();
                    List<objEmployeeInfoLanguage>? listLanguage = new List<objEmployeeInfoLanguage>();
                    List<TB_Employee_Language>? listLang = TBEmployeeLanguage.Where(w => w.nEmployeeID == nID && w.nLanguage != (int)DataLang.Thai).ToList();
                    foreach (var item in listLang)
                    {
                        objEmployeeInfoLanguage? objLang = new objEmployeeInfoLanguage();
                        objLang.sLanguage = TMDataLang.First(w => w.nData_ID == item.nLanguage).sNameTH;
                        objLang.sSpeaking = TMDataTask.First(w => w.nData_ID == item.nSpeaking).sNameTH;
                        objLang.sReading = TMDataTask.First(w => w.nData_ID == item.nReading).sNameTH;
                        objLang.sWriting = TMDataTask.First(w => w.nData_ID == item.nWriting).sNameTH;
                        objLang.nLanguage = item.nLanguage;
                        listLanguage.Add(objLang);
                    }
                    objRowdata.listLanguage = listLanguage.OrderByDescending(o => o.nLanguage).ToList();
                    List<objSpecial>? listSpecial = new List<objSpecial>();
                    foreach (var item in TMDataSpecia)
                    {
                        objSpecial? objSpecial = new objSpecial();
                        objSpecial.sSpecialAbilityTypeID = item.sNameTH + "";
                        List<objChildSpecial>? listChild = new List<objChildSpecial>();
                        if (item.nData_ID == nProgramskills)
                        {
                            listChild = (from c in _db.TB_Employee_SpecialAbility.Where(w => w.nSpecialAbilityTypeID == item.nData_ID && w.nEmployeeID == nID)
                                         select new objChildSpecial
                                         {
                                             nOrder = c.nOrder,
                                             sDrescription = "ทักษะที่" + c.nOrder + " " + c.sDrescription
                                         }).OrderBy(o => o.nOrder).Take(3).ToList();
                        }
                        else
                        {
                            listChild = (from c in _db.TB_Employee_SpecialAbility.Where(w => w.nSpecialAbilityTypeID == item.nData_ID && w.nEmployeeID == nID)
                                         select new objChildSpecial
                                         {
                                             nOrder = c.nOrder,
                                             sDrescription = "ทักษะที่" + c.nOrder + " " + c.sDrescription
                                         }).OrderBy(o => o.nOrder).Take(5).ToList();
                        }
                        objSpecial.listDrescription = listChild;
                        listSpecial.Add(objSpecial);
                    }
                    objRowdata.listSpecial = listSpecial;
                    listData.Add(objRowdata);
                }
                res.listData = listData;
                res.nStatusCode = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                res.nStatusCode = StatusCodes.Status500InternalServerError;
                res.sMessage = e.Message;
            }
            return res;
        }

        public cReturnExport SpireDoc_ExportWord(objParam obj)
        {
            cReturnExport result = new cReturnExport();
            try
            {
                List<objEmployeeInfo>? listData = GetProfile(obj.sID).listData;
                string sReportName = "Resume_" + DateTime.Now.ToString("ddMMyyyyHHmm");

                // start Spire Doc
                Spire.Doc.Document document = new Spire.Doc.Document();

                // page size A4
                var pSizeA4 = new System.Drawing.SizeF(595.3891766f, 841.9560595f);

                //Set Font Style and Size
                Spire.Doc.Documents.ParagraphStyle style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "FontStyle";
                style.CharacterFormat.FontName = "Angsana New";
                style.CharacterFormat.FontSize = 16;
                document.Styles.Add(style);

                document.AddStyle(BuiltinStyle.Normal);
                Style styleLocale = document.Styles.FindByName("Normal");
                styleLocale.CharacterFormat.LocaleIdASCII = 1054;

                Spire.Doc.Section section = document.AddSection();
                section.PageSetup.PageSize = pSizeA4;
                section.PageSetup.Orientation = Spire.Doc.Documents.PageOrientation.Portrait;
                section.PageSetup.Margins.Top = 0f;
                section.PageSetup.Margins.Bottom = 0f;
                section.PageSetup.Margins.Right = 0f;
                section.PageSetup.Margins.Left = 0f;

                section.PageSetup.HeaderDistance = 0f;
                section.PageSetup.FooterDistance = 0f;
                section.AddColumn(70f, 20f);
                section.AddColumn(100f, 20f);


                //This is necessary
                section.PageSetup.DifferentFirstPageHeaderFooter = true;
                section.HeadersFooters.FirstPageHeader.ChildObjects.Clear();

                Spire.Doc.Documents.Paragraph? Para = null;
                Spire.Doc.Documents.Paragraph? ParaStyle = null;
                //string sPathTemplete = Path.GetFullPath("UploadFile/WordTemplate.docx");
                var sPathTemplete = _env.ContentRootPath + "wwwroot\\UploadFile\\WordTemplate.docx";
                if (File.Exists(sPathTemplete))
                {
                    Spire.Doc.Document documentCloneStyle = new Spire.Doc.Document();
                    documentCloneStyle.LoadFromFile(sPathTemplete);
                    ParaStyle = (Spire.Doc.Documents.Paragraph)documentCloneStyle.Sections[0].Paragraphs[0].Clone();
                }

                Para = section.AddParagraph();
                Para.ApplyStyle(style.Name);

                //add text to the paragraph
                string img = listData.Select(s => s.sFileLink).FirstOrDefault() ?? "";
                //string srcProfile = Path.GetFullPath("UploadFile/Picture_word/7309681.jpg");
                string srcProfile = _env.ContentRootPath + "wwwroot\\" + img;
                if (!File.Exists(srcProfile))
                {
                    srcProfile = "D:\\Project\\2022\\Softthai\\Softthai_Intranet_2023\\Frontend\\src\\assets\\images\\NoImage\\default-avatar.png";
                }

                Image ImgProfile = Image.FromFile(srcProfile);
                ImgProfile = CropToCircle(ImgProfile);
                Spire.Doc.Fields.DocPicture picProfile = Para.AppendPicture(ImgProfile);
                picProfile.Width = 100;
                picProfile.Height = 100;
                picProfile.HorizontalAlignment = Spire.Doc.ShapeHorizontalAlignment.Center;
                picProfile.VerticalAlignment = Spire.Doc.ShapeVerticalAlignment.Top;
                picProfile.HorizontalPosition = 120;
                picProfile.TextWrappingStyle = TextWrappingStyle.TopAndBottom;

                // Table 
                string sPathImg = "wwwroot\\UploadFile\\Picture_word\\Contact.png";
                GetHeadName(section, sPathImg, "ข้อมูลส่วนตัว");
                ////Set alignment for cells 
                TextEmpty(section, "", false, false, 0);


                // Table 
                Table tableContact = section.AddTable(true);
                tableContact.TableFormat.Borders.Left.BorderType = Spire.Doc.Documents.BorderStyle.None;
                tableContact.TableFormat.Borders.Right.BorderType = Spire.Doc.Documents.BorderStyle.None;
                tableContact.TableFormat.Borders.Top.BorderType = Spire.Doc.Documents.BorderStyle.None;
                tableContact.TableFormat.Borders.Bottom.BorderType = Spire.Doc.Documents.BorderStyle.None;
                tableContact.TableFormat.Borders.Vertical.BorderType = Spire.Doc.Documents.BorderStyle.None;
                tableContact.TableFormat.Borders.Horizontal.BorderType = Spire.Doc.Documents.BorderStyle.None;
                tableContact.ResetCells(5, 2);


                foreach (var item in listData)
                {
                    #region ข้อมูลส่วนตัว
                    for (int i = 0; i < 5; i++)
                    {
                        //Set the first row as table header
                        Spire.Doc.TableRow FRowContact = tableContact.Rows[i];

                        //Set the height and color of the first row
                        FRowContact.Height = 25f;
                        string sPath;
                        string sText;
                        for (int j = 0; j < 2; j++)
                        {

                            switch (i)
                            {
                                case 0:
                                    sPath = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\email.png";
                                    sText = item.sEmail + "";
                                    Contact(item, FRowContact, j, sPath, sText);
                                    break;
                                case 1:
                                    sPath = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\Home.png";
                                    sText = item.sHome + "";
                                    Contact(item, FRowContact, j, sPath, sText);
                                    break;
                                case 2:
                                    sPath = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\Mobile.png";
                                    sText = item.sTelephone + "";
                                    Contact(item, FRowContact, j, sPath, sText);
                                    break;
                                case 3:
                                    sPath = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\Calendar.png";
                                    sText = item.sBirth + "";
                                    Contact(item, FRowContact, j, sPath, sText);
                                    break;
                                case 4:
                                    sPath = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\World.png";
                                    sText = item.sNationality + "";
                                    Contact(item, FRowContact, j, sPath, sText);
                                    break;

                            }

                        }
                    }
                    #endregion

                    //Set alignment for cells 
                    #region ความสามารถด้านต่างๆ
                    TextEmpty(section, "", false, false, 1);
                    sPathImg = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\Setting.png";
                    GetHeadName(section, sPathImg, "ความสามารถด้านต่างๆ");
                    TextEmpty(section, "", false, false, 2);
                    // TextEmpty(section, "ทักษะการพัฒนา หรือใช้งานโปรแกรม", true, false, 3, 10f);
                    var countSecial = 0;

                    if (item.listSpecial != null && item.listSpecial.Any())
                    {
                        foreach (var itemSpecial in item.listSpecial)
                        {
                            TextEmpty(section, itemSpecial.sSpecialAbilityTypeID, true, false, 3, 30f, true);
                            if (itemSpecial.listDrescription != null && itemSpecial.listDrescription.Any())
                            {
                                foreach (var itemSub in itemSpecial.listDrescription)
                                {
                                    TextEmpty(section, itemSub.sDrescription, false, false, 4, 30f);
                                    countSecial++;
                                }
                            }

                        }
                    }
                    #endregion

                    #region ทักษะภาษา
                    bool isBreckLanguage = false;
                    if (item.listLanguage.Count() > 3 && countSecial > 4)
                    {
                        isBreckLanguage = true;
                    }

                    TextEmpty(section, "", false, isBreckLanguage, 5);
                    if (isBreckLanguage)
                    {
                        TextEmpty(section, item.sPosition, true, false, 9);
                        TextEmpty(section, item.sNameTH + " " + item.sSurnameTH, true, false, 9);
                    }

                    sPathImg = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\Language.png";
                    GetHeadName(section, sPathImg, "ทักษะภาษา");
                    TextEmpty(section, "", false, false, 6);
                    if (item.listLanguage.Any())
                    {
                        foreach (var itemLanguage in item.listLanguage)
                        {
                            // itemLanguage.sLanguage
                            TextEmpty(section, itemLanguage.sLanguage, true, false, 7, 20f);
                            TextEmpty(section, itemLanguage.sRendar, false, false, 7, 20f);

                        }
                    }
                    else
                    {
                        TextEmpty(section, "", true, false, 7);
                    }
                    #endregion


                    #region ประสบการณ์ทำงาน

                    TextEmpty(section, "", false, !isBreckLanguage ? true : false, 8);
                    if (!isBreckLanguage)
                    {
                        // 80,152,255
                        TextEmpty(section, item.sPosition, true, false, 9, 0, false, 80, 152, 255, 22);
                        TextEmpty(section, item.sNameTH + " " + item.sSurnameTH, true, false, 9, 20);
                    }
                    TextEmpty(section, "", false, false, 10);

                    sPathImg = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\Work.png";
                    GetHeadName(section, sPathImg, "ประสบการณ์ทำงาน");
                    TextEmpty(section, "", false, false, 11);
                    // Content
                    if (item.listWorkEx.Any())
                    {
                        Table tableContent = section.AddTable(true);
                        tableContent.TableFormat.Borders.Left.BorderType = Spire.Doc.Documents.BorderStyle.None;
                        tableContent.TableFormat.Borders.Right.BorderType = Spire.Doc.Documents.BorderStyle.None;
                        tableContent.TableFormat.Borders.Top.BorderType = Spire.Doc.Documents.BorderStyle.None;
                        tableContent.TableFormat.Borders.Bottom.BorderType = Spire.Doc.Documents.BorderStyle.None;
                        tableContent.TableFormat.Borders.Vertical.BorderType = Spire.Doc.Documents.BorderStyle.None;
                        tableContent.TableFormat.Borders.Horizontal.BorderType = Spire.Doc.Documents.BorderStyle.None;
                        int nCountWorkEx = item.listWorkEx.Count() * 3;
                        tableContent.ResetCells(nCountWorkEx, 2);

                        TableCotent(item, tableContent, nCountWorkEx);
                    }
                    #endregion

                    #region ประวัติการศึกษา

                    bool isBreckEducation = false;
                    if ((item.listEducation.Count() > 2 && isBreckLanguage) || (item.listEducation.Count() > 3 && item.listWorkEx.Count() > 2))
                    {
                        isBreckEducation = true;
                    }
                    // TextEmpty(section, "", false, isBreckEducation, 12);
                    // Create the second section
                    if (item.listEducation.Any())
                    {
                        if (isBreckEducation)
                        {
                            Spire.Doc.Section sectionTwo = document.AddSection();
                            sectionTwo.PageSetup.PageSize = pSizeA4;
                            sectionTwo.PageSetup.Orientation = Spire.Doc.Documents.PageOrientation.Portrait;
                            sectionTwo.PageSetup.Margins.Top = 0f;
                            sectionTwo.PageSetup.Margins.Bottom = 0f;
                            sectionTwo.PageSetup.Margins.Right = 0f;
                            sectionTwo.PageSetup.Margins.Left = 0f;
                            sectionTwo.PageSetup.HeaderDistance = 0f;
                            sectionTwo.PageSetup.FooterDistance = 0f;

                            Spire.Doc.Documents.Paragraph? Para2 = null;
                            Para2 = section.AddParagraph();
                            Para2.ApplyStyle(style.Name);

                            // Add content to section two
                            // Spire.Doc.Documents.Paragraph paraTwo = sectionTwo.AddParagraph();
                            // paraTwo.AppendText("Content of section two goes here.");
                            sPathImg = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\Education.png";
                            GetHeadName(sectionTwo, sPathImg, "ประวัติการศึกษา", true);

                            Table tableEducation = sectionTwo.AddTable(true);
                            tableEducation.TableFormat.Borders.Left.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            tableEducation.TableFormat.Borders.Right.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            tableEducation.TableFormat.Borders.Top.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            tableEducation.TableFormat.Borders.Bottom.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            tableEducation.TableFormat.Borders.Vertical.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            tableEducation.TableFormat.Borders.Horizontal.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            int nCountEducation = item.listEducation.Count() * 3;
                            tableEducation.ResetCells(nCountEducation, 2);

                            TableCotent(item, tableEducation, nCountEducation, false);

                        }
                        else
                        {
                            TextEmpty(section, "", false, false, 12);
                            sPathImg = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\Education.png";
                            GetHeadName(section, sPathImg, "ประวัติการศึกษา");
                            TextEmpty(section, "", false, false, 0);

                            Table tableEducation = section.AddTable(true);
                            tableEducation.TableFormat.Borders.Left.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            tableEducation.TableFormat.Borders.Right.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            tableEducation.TableFormat.Borders.Top.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            tableEducation.TableFormat.Borders.Bottom.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            tableEducation.TableFormat.Borders.Vertical.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            tableEducation.TableFormat.Borders.Horizontal.BorderType = Spire.Doc.Documents.BorderStyle.None;
                            int nCountEducation = item.listEducation.Count() * 3;
                            tableEducation.ResetCells(nCountEducation, 2);

                            TableCotent(item, tableEducation, nCountEducation, false);

                        }
                    }
                    else
                    {

                        sPathImg = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\Education.png";
                        GetHeadName(section, sPathImg, "ประวัติการศึกษา");
                    }

                    // }

                    #endregion

                }

                //Set background picture
                string srcGraph = _env.ContentRootPath + "wwwroot\\UploadFile\\Picture_word\\Template2.jpg";
                Image Img = Image.FromFile(srcGraph);
                // document.Background.Picture = Img;
                //   

                PictureWatermark picture = new PictureWatermark();
                picture.Picture = Img;
                picture.IsWashout = false;

                document.Watermark = picture;


                #region save
                string ToLocationWord = "";
                ToLocationWord = STFunction.MapPath(sReportName + ".docx", _env);
                document.SaveToFile(ToLocationWord, Spire.Doc.FileFormat.Docx2013);
                #endregion


                // #region File Docx
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Position = 0;
                    document.SaveToStream(ms, Spire.Doc.FileFormat.Docx2013);
                    result.objFile = ms.ToArray();
                    result.sFileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    result.sFileName = sReportName;
                }
                // #endregion
            }
            catch (Exception ex)
            {

            }
            return result;

            static void GetHeadName(Spire.Doc.Section section, string sPathImg, string sTextHead, bool isAutoFit = false)
            {
                Table table = section.AddTable(true);
                if (isAutoFit)
                {
                    table.TableFormat.LayoutType = LayoutType.Fixed;
                    table.PreferredWidth = new PreferredWidth(WidthType.Percentage, 100);
                }
                table.TableFormat.Borders.Left.BorderType = Spire.Doc.Documents.BorderStyle.None;
                table.TableFormat.Borders.Right.BorderType = Spire.Doc.Documents.BorderStyle.None;
                table.TableFormat.Borders.Top.BorderType = Spire.Doc.Documents.BorderStyle.None;
                table.TableFormat.Borders.Bottom.BorderType = Spire.Doc.Documents.BorderStyle.None;
                table.TableFormat.Borders.Vertical.BorderType = Spire.Doc.Documents.BorderStyle.None;
                table.TableFormat.Borders.Horizontal.BorderType = Spire.Doc.Documents.BorderStyle.None;

                table.ResetCells(1, 2);

                //Set the first row as table header
                Spire.Doc.TableRow FRow = table.Rows[0];
                FRow.IsHeader = true;

                //Set the height and color of the first row
                FRow.Height = 35f;
                FRow.RowFormat.BackColor = Color.FromArgb(162, 182, 254);

                //Set alignment for cells 
                Paragraph p = FRow.Cells[0].AddParagraph();
                FRow.Cells[0].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                FRow.Cells[0].SetCellWidth(20, CellWidthType.Percentage);
                p.Format.HorizontalAlignment = HorizontalAlignment.Right;

                //string srcContact = Path.GetFullPath(sPathImg);
                Image ImgContact = Image.FromFile(sPathImg);
                Spire.Doc.Fields.DocPicture pic = p.AppendPicture(ImgContact);
                pic.Width = 15;
                pic.Height = 15;
                pic.Color = PictureColor.Automatic;
                pic.HorizontalAlignment = Spire.Doc.ShapeHorizontalAlignment.Center;
                pic.VerticalAlignment = Spire.Doc.ShapeVerticalAlignment.Top;
                pic.HorizontalPosition = 120;
                pic.TextWrappingStyle = TextWrappingStyle.Inline;

                //Set alignment for cells 
                Paragraph p2 = FRow.Cells[1].AddParagraph();
                FRow.Cells[1].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                if (isAutoFit)
                {
                    FRow.Cells[1].SetCellWidth(80, CellWidthType.Percentage);
                }
                // FRow.Cells[1].CellFormat.BackColor = Color.CornflowerBlue;
                p2.Format.HorizontalAlignment = HorizontalAlignment.Left;

                //Set data format
                Spire.Doc.Fields.TextRange TR = p2.AppendText(sTextHead);
                TR.CharacterFormat.FontName = "Angsana New";
                TR.CharacterFormat.FontSize = 20;
                TR.CharacterFormat.TextColor = Color.FromArgb(55, 76, 154);
                TR.CharacterFormat.Bold = true;
            }

            static void TextEmpty(Spire.Doc.Section section, string? sText, bool isBold = false, bool isBreak = false, int i = 0, float LeftIndent = 0f, bool isList = false, int green = 0, int red = 0, int blue = 0, int FSize = 16)
            {
                Spire.Doc.Documents.Paragraph pEmpty = section.Paragraphs[i];
                pEmpty = section.AddParagraph();
                //Set data format
                Spire.Doc.Fields.TextRange textEmpty = pEmpty.AppendText(sText);
                textEmpty.CharacterFormat.FontName = "Angsana New";
                textEmpty.CharacterFormat.FontSize = FSize;
                textEmpty.CharacterFormat.Bold = isBold;
                // textEmpty.CharacterFormat.TextColor = Color.FromArgb(55,76,154);
                textEmpty.CharacterFormat.TextColor = Color.FromArgb(green, red, blue);
                pEmpty.Format.LeftIndent = LeftIndent;
                // if (isList)
                // {
                //     pEmpty.ListFormat.ApplyStyle("bulletedList");
                //     pEmpty.ListFormat.ListLevelNumber = 0;
                // }


                // pEmpty.ApplyStyle(BuiltinStyle.Heading1);
                if (isBreak)
                {
                    pEmpty.AppendBreak(BreakType.ColumnBreak);
                }



            }

            static void TableCotent(objEmployeeInfo item, Table tableContent, int nCountWorkEx, bool isCatagory = true)
            {
                int nRow = 2;
                List<int> listNotMerge = new List<int>();
                for (int i = 0; i <= (nCountWorkEx - 3); i++)
                {
                    for (int j = i; j < nRow; j++)
                    {
                        tableContent.ApplyHorizontalMerge((j + 1), 0, 1);
                    }
                    listNotMerge.Add(i);
                    i = i + 2;
                    nRow = nRow + 3;
                }


                for (int i = 0; i < nCountWorkEx; i++)
                {
                    int nCount = i / 3;
                    bool isNotMerge = listNotMerge.Any(f => f == i);
                    Spire.Doc.TableRow FRowContent = tableContent.Rows[i];
                    //Set the height and color of the first row
                    FRowContent.Height = 20f;
                    string? sText = !isCatagory ? item.listEducation[nCount].sAcademy : item.listWorkEx[nCount].sWorkCompany;
                    if (i % 2 == 0) { sText = !isCatagory ? item.listEducation[nCount].sMajor : item.listWorkEx[nCount].sJobDescription; }
                    if (isNotMerge)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            if (j == 0)
                            {
                                //Set alignment for cells 
                                Paragraph sTextRow = FRowContent.Cells[j].AddParagraph();
                                FRowContent.Cells[j].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                                // FRow.Cells[1].CellFormat.BackColor = Color.CornflowerBlue;
                                sTextRow.Format.HorizontalAlignment = HorizontalAlignment.Left;
                                sTextRow.Format.LeftIndent = 10f;

                                //Set data format
                                string? sHead = !isCatagory ? item.listEducation[nCount].sEducational_Level : item.listWorkEx[nCount].sPosition;
                                Spire.Doc.Fields.TextRange textEmail = sTextRow.AppendText(sHead);
                                textEmail.CharacterFormat.FontName = "Angsana New";
                                textEmail.CharacterFormat.FontSize = 16;
                                // textEmail.CharacterFormat.TextColor = Color.DarkSlateBlue;
                                textEmail.CharacterFormat.Bold = true;



                            }
                            else
                            {
                                //Set alignment for cells 
                                Paragraph sTextRow = FRowContent.Cells[j].AddParagraph();
                                FRowContent.Cells[j].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                                // FRow.Cells[1].CellFormat.BackColor = Color.CornflowerBlue;
                                sTextRow.Format.HorizontalAlignment = HorizontalAlignment.Right;
                                sTextRow.Format.RightIndent = 20f;


                                //Set data format
                                string? sHeadRight = !isCatagory ? item.listEducation[nCount].sEducationStart + " - " + item.listEducation[nCount].sEducationEnd : item.listWorkEx[nCount].sWorkStart + " - " + item.listWorkEx[nCount].sWorkEnd;
                                Spire.Doc.Fields.TextRange textEmail = sTextRow.AppendText(sHeadRight);
                                textEmail.CharacterFormat.FontName = "Angsana New";
                                textEmail.CharacterFormat.FontSize = 16;
                                // textEmail.CharacterFormat.TextColor = Color.DarkSlateBlue;
                                textEmail.CharacterFormat.Bold = true;
                            }

                        }
                    }
                    else
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            if (j == 0)
                            {
                                //Set alignment for cells 
                                Paragraph sTextRow = FRowContent.Cells[j].AddParagraph();
                                FRowContent.Cells[j].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                                // FRow.Cells[1].CellFormat.BackColor = Color.CornflowerBlue;
                                sTextRow.Format.HorizontalAlignment = HorizontalAlignment.Left;
                                sTextRow.Format.LeftIndent = 10f;

                                //Set data format
                                Spire.Doc.Fields.TextRange textEmail = sTextRow.AppendText(sText);
                                textEmail.CharacterFormat.FontName = "Angsana New";
                                textEmail.CharacterFormat.FontSize = 16;
                                // textEmail.CharacterFormat.TextColor = Color.DarkSlateBlue;
                                // textEmail.CharacterFormat.Bold = true;
                            }
                        }
                    }


                    // TextEmpty(section, item.listWorkEx[i].sWorkCompany, false, false, 12);
                    // TextEmpty(section, item.listWorkEx[i].sJobDescription, false, false, 12);



                }
            }

        }

        private static void Contact(objEmployeeInfo item, Spire.Doc.TableRow FRowContact, int j, string sPath, string sText)
        {
            if (j == 0)
            {
                //Set alignment for cells 
                Paragraph paragraph = FRowContact.Cells[j].AddParagraph();
                FRowContact.Cells[j].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                FRowContact.Cells[j].SetCellWidth(20, CellWidthType.Percentage);
                paragraph.Format.HorizontalAlignment = HorizontalAlignment.Center;

                //string src = Path.GetFullPath(sPath);
                Image Img = Image.FromFile(sPath);
                Spire.Doc.Fields.DocPicture picEmail = paragraph.AppendPicture(Img);
                picEmail.Width = 12;
                picEmail.Height = 12;
                picEmail.Color = PictureColor.Automatic;
                picEmail.HorizontalAlignment = Spire.Doc.ShapeHorizontalAlignment.Center;
                picEmail.VerticalAlignment = Spire.Doc.ShapeVerticalAlignment.Top;
                // picEmail.HorizontalPosition = 120;
                // picEmail.TextWrappingStyle = TextWrappingStyle.Inline;


            }
            else
            {
                //Set alignment for cells 
                Paragraph sTextRow = FRowContact.Cells[j].AddParagraph();
                FRowContact.Cells[j].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                // FRow.Cells[1].CellFormat.BackColor = Color.CornflowerBlue;
                sTextRow.Format.HorizontalAlignment = HorizontalAlignment.Left;
                sTextRow.Format.LeftIndent = 0f;


                //Set data format
                Spire.Doc.Fields.TextRange textEmail = sTextRow.AppendText(sText);
                textEmail.CharacterFormat.FontName = "Angsana New";
                textEmail.CharacterFormat.FontSize = 16;
                // textEmail.CharacterFormat.TextColor = Color.DarkSlateBlue;
                // textEmail.CharacterFormat.Bold = true;
            }
        }

        public static Image CropToCircle(Image sourceImage)
        {
            int width = sourceImage.Width > 200 ? 200 : sourceImage.Width;
            int height = sourceImage.Height > 200 ? 200 : sourceImage.Height;
            Bitmap result = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, width, height);
                    g.Clip = new Region(path);
                    g.DrawImage(sourceImage, 0, 0, width, height);
                }
            }

            return result;
        }

        #endregion

        #region ออร์
        public ResultAPI SaveDataEducation(cEducation req)
        {

            ResultAPI objResult = new ResultAPI();
            var ua = _auth.GetUserAccount();
            try
            {
                int nEmployeeID = (req.sID ?? "").DecryptParameter().ToInt();

                #region lstDataSchool
                if (req.lstDataSchool != null)
                {
                    foreach (var item in req.lstDataSchool)
                    {
                        TB_Employee_Education? objEnducationSchool = _db.TB_Employee_Education.FirstOrDefault(w => w.nEducationID == item.nEducationID && w.nEmployeeID == nEmployeeID && w.IsDelete == false);
                        if (objEnducationSchool == null) //Add
                        {
                            objEnducationSchool = new TB_Employee_Education();
                            objEnducationSchool.nEducationID = (_db.TB_Employee_Education.Any() ? _db.TB_Employee_Education.Max(m => m.nEducationID) : 0) + 1;
                            objEnducationSchool.nEmployeeID = nEmployeeID;
                            objEnducationSchool.dCreate = DateTime.Now;
                            objEnducationSchool.nCreateBy = ua.nUserID;
                            _db.TB_Employee_Education.Add(objEnducationSchool);
                        }
                        objEnducationSchool.nEducational_Level = item.nEducational_Level;
                        objEnducationSchool.sAcademy = item.sAcademy;
                        objEnducationSchool.sMajor = item.sMajor;
                        objEnducationSchool.nEducationStart = item.nEducationStart;
                        objEnducationSchool.nEducationEnd = item.nEducationEnd;
                        objEnducationSchool.IsDelete = item.IsDel;
                        objEnducationSchool.dUpdate = DateTime.Now;
                        objEnducationSchool.nUpdateBy = ua.nUserID;
                        if (item.IsDel == true)
                        {
                            objEnducationSchool.dDelete = DateTime.Now;
                            objEnducationSchool.nDeleteBy = ua.nUserID;
                        }
                        _db.SaveChanges();
                    }

                }

                #endregion


                objResult.nStatusCode = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                objResult.nStatusCode = StatusCodes.Status500InternalServerError;
                objResult.sMessage = e.Message;
            }
            return objResult;
        }
        public ResultAPI SaveDataWorkExperien(cWorkExperien req)
        {

            ResultAPI objResult = new ResultAPI();
            var ua = _auth.GetUserAccount();
            try
            {
                int nEmployeeID = (req.sID ?? "").DecryptParameter().ToInt();
                #region lstDataWork
                if (req.lstDataWork != null)
                {
                    foreach (var item in req.lstDataWork)
                    {
                        int nOrder = (_db.TB_Employee_WorkExperience.Where(w => w.nEmployeeID == nEmployeeID).Any() ? _db.TB_Employee_WorkExperience.Where(w => w.nEmployeeID == nEmployeeID).Max(m => m.nOrder) : 0) + 1;
                        TB_Employee_WorkExperience? objWorkExperien = _db.TB_Employee_WorkExperience.FirstOrDefault(w => w.nWorkExperienceID == item.nWorkExperienceID && w.nEmployeeID == nEmployeeID && w.IsDelete == false);
                        if (objWorkExperien == null) //Add
                        {
                            objWorkExperien = new TB_Employee_WorkExperience();
                            objWorkExperien.nWorkExperienceID = (_db.TB_Employee_WorkExperience.Any() ? _db.TB_Employee_WorkExperience.Max(m => m.nWorkExperienceID) : 0) + 1;
                            objWorkExperien.nEmployeeID = nEmployeeID;
                            objWorkExperien.dCreate = DateTime.Now;
                            objWorkExperien.nCreateBy = ua.nUserID;
                            _db.TB_Employee_WorkExperience.Add(objWorkExperien);
                        }
                        objWorkExperien.nOrder = nOrder;
                        objWorkExperien.sWorkCompany = item.sWorkCompany;
                        objWorkExperien.dWorkStart = item.dWorkStart;
                        objWorkExperien.dWorkEnd = item.dWorkEnd;
                        objWorkExperien.sPosition = item.sPosition;
                        objWorkExperien.sJobDescription = item.sJobDescription;
                        objWorkExperien.dUpdate = DateTime.Now;
                        objWorkExperien.nUpdateBy = ua.nUserID;
                        nOrder++;
                        if (item.IsDel == true)
                        {
                            objWorkExperien.dDelete = DateTime.Now;
                            objWorkExperien.nDeleteBy = ua.nUserID;
                        }
                        _db.SaveChanges();
                    }
                }
                #endregion

                objResult.nStatusCode = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                objResult.nStatusCode = StatusCodes.Status500InternalServerError;
                objResult.sMessage = e.Message;
            }
            return objResult;
        }
        public cEducation GetDataEducation(cEducation req)
        {
            cEducation result = new cEducation();
            try
            {

                int nID = (req.sID + "").DecryptParameter().ToInt();
                var lstData = _db.TB_Employee_Education.Where(w => w.nEmployeeID == nID && w.IsDelete == false).Select(a => new cEducationSchool
                {
                    sID = a.nEducationID.ToString(),
                    nEducationID = a.nEducationID,
                    nEducational_Level = a.nEducational_Level,
                    sAcademy = a.sAcademy ?? "",
                    sMajor = a.sMajor ?? "",
                    nEducationStart = a.nEducationStart,
                    nEducationEnd = a.nEducationEnd,
                    IsDel = a.IsDelete,
                }).ToList();
                result.lstAllDataEducation = lstData;

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;

            }

            return result;


        }
        public cWorkExperien GetDataWorkExperien(cWorkExperien req)
        {
            cWorkExperien result = new cWorkExperien();
            try
            {

                int nID = (req.sID + "").DecryptParameter().ToInt();
                var lstData = _db.TB_Employee_WorkExperience.Where(w => w.nEmployeeID == nID && w.IsDelete == false).Select(s => new objWorkExperien
                {
                    sID = s.nWorkExperienceID.ToString(),
                    nWorkExperienceID = s.nWorkExperienceID,
                    sWorkCompany = s.sWorkCompany ?? "",
                    dWorkStart = s.dWorkStart,
                    dWorkEnd = s.dWorkEnd,
                    sPosition = s.sPosition,
                    sJobDescription = s.sJobDescription ?? "",
                    IsDel = s.IsDelete,
                }).ToList();
                result.lstAllDataWorkExperien = lstData;

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }
        public ResultAPI SaveDataOtherParts(cOtherParts req)
        {

            ResultAPI objResult = new ResultAPI();
            var ua = _auth.GetUserAccount();
            try
            {
                int nEmployeeID = (req.sID ?? "").DecryptParameter().ToInt();

                TB_Employee_SpecialAbility? ObjectWorkoutSide = _db.TB_Employee_SpecialAbility.FirstOrDefault(w => w.nEmployeeID == nEmployeeID && w.IsDelete != true);
                int nOrderWork = (_db.TB_Employee_SpecialAbility.Where(w => w.nEmployeeID == nEmployeeID && w.nSpecialAbilityTypeID == 73).Any() ?
                       _db.TB_Employee_SpecialAbility.Where(w => w.nEmployeeID == nEmployeeID && w.nSpecialAbilityTypeID == 73).Max(m => m.nOrder) ?? 0 : 0) + 1;
                if (ObjectWorkoutSide != null)
                {
                    ObjectWorkoutSide.nSpecialAbilityTypeID = 73;
                    ObjectWorkoutSide.nOrder = nOrderWork;
                    ObjectWorkoutSide.nCan = req.nCan;
                    ObjectWorkoutSide.dUpdate = DateTime.Now;
                    ObjectWorkoutSide.nUpdateBy = ua.nUserID;
                    _db.SaveChanges();
                }
                #region lstDataPersonContact
                if (req.lstDataPersonContact != null)
                {
                    foreach (var item in req.lstDataPersonContact)
                    {

                        int nOrderPer = (_db.TB_Employee_PersonContact.Where(w => w.nEmployeeID == nEmployeeID && w.nContactType == 74).Any() ? _db.TB_Employee_PersonContact.Where(w => w.nEmployeeID == nEmployeeID && w.nContactType == 74).Max(m => m.nOrder) ?? 0 : 0) + 1;
                        TB_Employee_PersonContact? objPersonContact = _db.TB_Employee_PersonContact.FirstOrDefault(w => w.nPersonContactID == item.nPersonContactID && w.nEmployeeID == nEmployeeID && w.IsDelete == false && w.nContactType == item.nContactType);
                        if (objPersonContact == null) //Add
                        {
                            objPersonContact = new TB_Employee_PersonContact();
                            objPersonContact.nPersonContactID = (_db.TB_Employee_PersonContact.Any() ? _db.TB_Employee_PersonContact.Max(m => m.nPersonContactID) : 0) + 1;
                            objPersonContact.nContactType = item.nContactType;
                            objPersonContact.nOrder = nOrderPer;
                            objPersonContact.nEmployeeID = nEmployeeID;
                            objPersonContact.dCreate = DateTime.Now;
                            objPersonContact.nCreateBy = ua.nUserID;
                            _db.TB_Employee_PersonContact.Add(objPersonContact);

                        }
                        objPersonContact.nOrder = nOrderPer;
                        objPersonContact.sName = item.sName;
                        objPersonContact.sSurename = item.sSurename;
                        objPersonContact.nRelationship = item.nRelationship;
                        objPersonContact.sAddress = item.sAddress;
                        objPersonContact.sTelephone = item.sTelephone;
                        objPersonContact.dUpdate = DateTime.Now;
                        objPersonContact.nUpdateBy = ua.nUserID;
                        if (item.IsDel == true)
                        {
                            objPersonContact.dDelete = DateTime.Now;
                            objPersonContact.nDeleteBy = ua.nUserID;
                        }
                    }
                    _db.SaveChanges();
                }
                #endregion
                int nAllergyID = (_db.TB_Employee_Allergy.Any() ? _db.TB_Employee_Allergy.Max(m => m.nAllergyID) : 0) + 1;
                int nOrder = (_db.TB_Employee_Allergy.Where(w => w.nEmployeeID == nEmployeeID && w.nAllergyType == req.nAllergyType).Any() ?
                       _db.TB_Employee_Allergy.Where(w => w.nEmployeeID == nEmployeeID && w.nAllergyType == req.nAllergyType).Max(m => m.nOrder) : 0) + 1;
                #region lstAllDataAllergy
                if (req.lstAllDataAllergy != null)
                {
                    foreach (var item in req.lstAllDataAllergy)
                    {
                        TB_Employee_Allergy? objAllergy = _db.TB_Employee_Allergy.FirstOrDefault(w => w.nAllergyID == item.nAllergyID && w.nEmployeeID == nEmployeeID && w.nAllergyType == item.nAllergyType && w.IsDelete != true);
                        if (objAllergy == null) //Add
                        {
                            objAllergy = new TB_Employee_Allergy();
                            objAllergy.nAllergyID = nAllergyID;
                            objAllergy.nOrder = nOrder;
                            objAllergy.nAllergyType = item.nAllergyType;
                            objAllergy.nEmployeeID = nEmployeeID;
                            objAllergy.dCreate = DateTime.Now;
                            objAllergy.nCreateBy = ua.nUserID;
                            _db.TB_Employee_Allergy.Add(objAllergy);
                            nAllergyID++;
                        }
                        objAllergy.nOrder = nOrder;
                        objAllergy.nAllergyName = item.nAllergyName;
                        objAllergy.nDrescription = item.nDrescription;
                        objAllergy.dUpdate = DateTime.Now;
                        objAllergy.nUpdateBy = ua.nUserID;
                        objAllergy.IsDelete = item.IsDel;
                        nOrder++;
                        if (item.IsDel == true)
                        {
                            objAllergy.dDelete = DateTime.Now;
                            objAllergy.nDeleteBy = ua.nUserID;
                        }
                    }
                    _db.SaveChanges();
                }
                #endregion
                #region lstDataAllergy95
                //if (req.lstDataAllergy95 != null)
                //{
                //    foreach (var item in req.lstDataAllergy95)
                //    {
                //        //int nOrder = (_db.TB_Employee_Allergy.Where(w => w.nEmployeeID == nEmployeeID && w.nAllergyType == item.nAllergyType).Any() ?
                //        //_db.TB_Employee_Allergy.Where(w => w.nEmployeeID == nEmployeeID && w.nAllergyType == item.nAllergyType).Max(m => m.nOrder) : 0) + 1;
                //        TB_Employee_Allergy? objAllergyMedicine = _db.TB_Employee_Allergy.FirstOrDefault(w => w.nAllergyID == item.nAllergyID && w.nEmployeeID == nEmployeeID && w.nAllergyType == item.nAllergyType && w.IsDelete != true);
                //        if (objAllergyMedicine == null) //Add
                //        {
                //            objAllergyMedicine = new TB_Employee_Allergy();
                //            objAllergyMedicine.nAllergyID = nAllergyID;
                //            objAllergyMedicine.nOrder = nOrder;
                //            objAllergyMedicine.nAllergyType = item.nAllergyType;
                //            objAllergyMedicine.nEmployeeID = nEmployeeID;
                //            objAllergyMedicine.dCreate = DateTime.Now;
                //            objAllergyMedicine.nCreateBy = ua.nUserID;
                //            _db.TB_Employee_Allergy.Add(objAllergyMedicine);
                //            nAllergyID++;

                //        }
                //        objAllergyMedicine.nOrder = nOrder;
                //        objAllergyMedicine.nAllergyName = item.nAllergyName;
                //        objAllergyMedicine.nDrescription = item.nDrescription;
                //        objAllergyMedicine.dUpdate = DateTime.Now;
                //        objAllergyMedicine.nUpdateBy = ua.nUserID;
                //        nOrder++;
                //        if (item.IsDel == true)
                //        {
                //            objAllergyMedicine.dDelete = DateTime.Now;
                //            objAllergyMedicine.nDeleteBy = ua.nUserID;
                //        }
                //    }
                //    _db.SaveChanges();
                //}
                //#endregion
                //#region lstDataAllergy96
                //if (req.lstDataAllergy96 != null)

                //    foreach (var item in req.lstDataAllergy96)
                //    {
                //        {
                //            //int nOrder = (_db.TB_Employee_Allergy.Where(w => w.nEmployeeID == nEmployeeID && w.nAllergyType == item.nAllergyType).Any() ?
                //            //_db.TB_Employee_Allergy.Where(w => w.nEmployeeID == nEmployeeID && w.nAllergyType == item.nAllergyType).Max(m => m.nOrder) : 0) + 1;
                //            TB_Employee_Allergy? objAllergyFood = _db.TB_Employee_Allergy.FirstOrDefault(w => w.nAllergyID == item.nAllergyID && w.nEmployeeID == nEmployeeID && w.nAllergyType == item.nAllergyType && w.IsDelete != true);
                //            if (objAllergyFood == null) //Add
                //            {
                //                objAllergyFood = new TB_Employee_Allergy();
                //                objAllergyFood.nAllergyID = nAllergyID;
                //                objAllergyFood.nOrder = nOrder;
                //                objAllergyFood.nAllergyType = item.nAllergyType;
                //                objAllergyFood.nEmployeeID = nEmployeeID;
                //                objAllergyFood.dCreate = DateTime.Now;
                //                objAllergyFood.nCreateBy = ua.nUserID;
                //                _db.TB_Employee_Allergy.Add(objAllergyFood);
                //                nAllergyID++;

                //            }
                //            objAllergyFood.nOrder = nOrder;
                //            objAllergyFood.nAllergyName = item.nAllergyName;
                //            objAllergyFood.nDrescription = item.nDrescription;
                //            objAllergyFood.dUpdate = DateTime.Now;
                //            objAllergyFood.nUpdateBy = ua.nUserID;
                //            nOrder++;
                //            if (item.IsDel == true)
                //            {
                //                objAllergyFood.dDelete = DateTime.Now;
                //                objAllergyFood.nDeleteBy = ua.nUserID;
                //            }
                //        }
                //        _db.SaveChanges();

                //    }
                #endregion
                objResult.nStatusCode = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                objResult.nStatusCode = StatusCodes.Status500InternalServerError;
                objResult.sMessage = e.Message + e.StackTrace;
            }
            return objResult;
        }
        public cOtherParts GetDataOtherParts(cOtherParts req)
        {
            cOtherParts result = new cOtherParts();
            try
            {
                int nID = (req.sID + "").DecryptParameter().ToInt();
                TB_Employee_SpecialAbility? objEmpSpecialAbility = _db.TB_Employee_SpecialAbility.Where(w => w.nEmployeeID == nID && w.nSpecialAbilityTypeID == 73).FirstOrDefault();
                if (objEmpSpecialAbility != null)
                {
                    result.nEmployeeID = objEmpSpecialAbility.nEmployeeID;
                    result.nSpecialAbilityTypeID = objEmpSpecialAbility.nSpecialAbilityTypeID;
                    result.nCan = objEmpSpecialAbility.nCan;


                }
                var lstDataAllAllergy = _db.TB_Employee_Allergy.Where(w => w.nEmployeeID == nID && w.IsDelete == false).Select(s => new objAllergy
                {
                    sID = s.nAllergyID.ToString(),
                    nAllergyID = s.nAllergyID,
                    nAllergyName = s.nAllergyName,
                    nAllergyType = s.nAllergyType,
                    nDrescription = s.nDrescription,
                    nOrder = s.nOrder,
                    IsDel = s.IsDelete,
                }).ToList();

                var lstDataAllContact = _db.TB_Employee_PersonContact.Where(w => w.nEmployeeID == nID && w.IsDelete == false).Select(s => new objPersonContact
                {
                    sID = s.nPersonContactID.ToString(),
                    nPersonContactID = s.nPersonContactID,
                    nContactType = s.nContactType,
                    sName = s.sName,
                    sSurename = s.sSurename,
                    nRelationship = s.nRelationship,
                    sAddress = s.sAddress,
                    sTelephone = s.sTelephone,
                    nOrder = s.nOrder,
                    IsDel = s.IsDelete,
                }).ToList();

                result.lstAllDataContact = lstDataAllContact;
                result.lstAllDataAllergy = lstDataAllAllergy;

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }
        public ResultAPI SaveDataSpecialAbility(cSpecialAbility req)
        {
            ResultAPI objResult = new ResultAPI();
            try
            {
                var ua = _auth.GetUserAccount();
                int nEmployeeID = (req.sID ?? "").DecryptParameter().ToInt();

                if (req.lstAllData != null)
                {
                    foreach (var item in req.lstAllData)
                    {
                        int nOrderSpecialAbility = (_db.TB_Employee_SpecialAbility.Where(w => w.nEmployeeID == nEmployeeID && w.nSpecialAbilityTypeID == req.nSpecialAbilityTypeID && w.IsDelete == false).Any() ?
                            _db.TB_Employee_SpecialAbility.Where(w => w.nEmployeeID == nEmployeeID && w.nSpecialAbilityTypeID == req.nSpecialAbilityTypeID && w.IsDelete == false).Select(s => s.nOrder).Max() ?? 1 : 1) + 1;
                        TB_Employee_SpecialAbility? ObjSpecialAbility = _db.TB_Employee_SpecialAbility.FirstOrDefault(w => w.nEmployeeID == nEmployeeID && w.nSpecialAbilityID == item.nSpecialAbilityID && w.IsDelete != true);
                        if (ObjSpecialAbility == null) //Add new Data
                        {
                            ObjSpecialAbility = new TB_Employee_SpecialAbility();
                            ObjSpecialAbility.nSpecialAbilityID = _db.TB_Employee_SpecialAbility.Any() ? _db.TB_Employee_SpecialAbility.Max(m => m.nSpecialAbilityID) + 1 : 1;
                            ObjSpecialAbility.nEmployeeID = nEmployeeID;
                            ObjSpecialAbility.nSpecialAbilityTypeID = item.nSpecialAbilityTypeID;
                            ObjSpecialAbility.dCreate = DateTime.Now;
                            ObjSpecialAbility.nCreateBy = ua.nUserID;

                            _db.TB_Employee_SpecialAbility.Add(ObjSpecialAbility);
                        }
                        ObjSpecialAbility.nOrder = nOrderSpecialAbility;
                        ObjSpecialAbility.nCan = item.nCan;
                        ObjSpecialAbility.sDrescription = item.sDrescription;
                        ObjSpecialAbility.nUpdateBy = ua.nUserID;
                        ObjSpecialAbility.dUpdate = DateTime.Now;
                        ObjSpecialAbility.IsDelete = item.IsDel;
                        if (item.IsDel == true)
                        {
                            ObjSpecialAbility.dDelete = DateTime.Now;
                            ObjSpecialAbility.nDeleteBy = ua.nUserID;
                        }
                        _db.SaveChanges();
                    }
                }
                objResult.nStatusCode = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                objResult.nStatusCode = StatusCodes.Status500InternalServerError;
                objResult.sMessage = e.Message;
            }
            return objResult;
        }
        public cSpecialAbility GetDataSpecialAbility(cSpecialAbility req)
        {
            cSpecialAbility result = new cSpecialAbility();
            try
            {
                int nID = (req.sID + "").DecryptParameter().ToInt();
                var lstAllDataSpecialAbility = _db.TB_Employee_SpecialAbility.Where(w => w.nEmployeeID == nID && w.IsDelete == false).Select(a => new ObjectSpecialAbility
                {
                    sID = a.nSpecialAbilityID.ToString(),
                    nSpecialAbilityID = a.nSpecialAbilityID,
                    nSpecialAbilityTypeID = a.nSpecialAbilityTypeID,
                    sDrescription = a.sDrescription ?? "",
                    nCan = a.nCan,
                    nOrder = a.nOrder,
                    IsDel = a.IsDelete,
                }).ToList();
                result.lstAllDataSpecialAbility = lstAllDataSpecialAbility;
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
            #endregion
        }

        public CEmployeeID GetEmployeeID()
        {
            CEmployeeID result = new CEmployeeID();
            var UserAccount = _auth.GetUserAccount();
            int nUserID = UserAccount.nUserID;
            try
            {
                result.sEmployeeID = nUserID.EncryptParameter().ToString();
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }
        public CMenueEmployee GetMenueEmployee()
        {
            CMenueEmployee result = new CMenueEmployee();
            UserAccount? UserAccount = _auth.GetUserAccount();
            int nUserID = UserAccount.nUserID;
            try
            {
                List<lstMenuEmployee>? lstdata = _db.TM_Menu.Where(w => w.nParentID == 9).Select(s => new lstMenuEmployee
                {
                    nMenuID = s.nMenuID,
                    nParentID = s.nParentID,
                    nLevel = s.nLevel,
                    nMenuType = s.nMenuType,
                    nOrder = s.nOrder,
                    sIcon = s.sIcon,
                    sMenuName = s.sMenuName,
                    sRoute = s.sRoute,
                }).ToList();
                result.lstMenuEmployees = lstdata;
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