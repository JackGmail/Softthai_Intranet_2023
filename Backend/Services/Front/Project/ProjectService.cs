using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Backend.Interfaces.Front.Project;
using Backend.Models;
using ST.INFRA.Common;
using ST.INFRA;
using System.Data;
using Extensions.Common.STExtension;
using Backend.Models.Front.Project;

namespace Backend.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IConfiguration _config;
        private readonly IAuthentication _authen;
        private readonly ST_IntranetEntity _db;
        private readonly IHostEnvironment _env;

        public ProjectService(IConfiguration config, IAuthentication authen, ST_IntranetEntity db, IHostEnvironment env)
        {
            _config = config;
            _authen = authen;
            _db = db;
            _env = env;
        }

        public cResultProject GetInitData()
        {
            cResultProject result = new cResultProject();
            try
            {
                result.lstCustomer = _db.TB_Customer.Where(w => w.IsActive == true).Select(s => new cSelectOption
                {
                    value = s.nCustomerID.ToString(),
                    label = (s.sCustomerName ?? ""),
                }).ToList();

                result.lstContractPoint = _db.TB_ContractPoint.Where(w => w.IsActive == true).Select(s => new cSelectOptionCustomer
                {
                    value = s.nContractPointID.ToString(),
                    label = (s.sNameTH + " " + s.sSurnameTH ?? s.sNameEN + " " + s.sSurnameEN),
                    nCustomerID = s.nCustomerID,
                }).ToList();

                result.lstRedio = _db.TM_Data.Where(w => w.nDatatypeID == 3 && w.IsDelete != true && w.IsActive == true).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng),
                }).ToList();

                result.lstPosition = _db.TB_Position.Where(w => w.IsActive == true).Select(s => new cSelectOption
                {
                    value = s.nPositionID.ToString(),
                    label = (s.sPositionName ?? "")
                }).ToList();

                result.lstProject = _db.TB_Project.Where(w => w.IsDelete != true).Select(s => new cSelectOption
                {
                    value = s.nProjectID.ToString(),
                    label = (s.sProjectName ?? "")
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

        public ResultAPI SaveData(cProject req)
        {

            ResultAPI objResult = new ResultAPI();
            var ua = _authen.GetUserAccount();
            try
            {
                int nProjectID = (req.sID ?? "").DecryptParameter().ToInt();
                string sDateNew = "P" + DateTime.Now.Date.ToStringFromDate("yyMMdd");
                int nNumber = _db.TB_Project.Where(w => (w.sProjectCode + "").Contains(sDateNew)).Any() ? _db.TB_Project.Where(w => (w.sProjectCode + "").Contains(sDateNew)).ToList().Count() + 1 : 1;
                string sNumber = String.Format("{0:D3}", nNumber);
                string sFormCode = sDateNew + sNumber;


                TB_Project? objProject = _db.TB_Project.FirstOrDefault(w => w.nProjectID == nProjectID);
                if (objProject == null)  //Add
                {
                    objProject = new TB_Project();
                    objProject.nProjectID = (_db.TB_Project.Any() ? _db.TB_Project.Max(m => m.nProjectID) : 0) + 1; ;
                    nProjectID = objProject.nProjectID;
                    objProject.nParentID = req.nParentID;
                    objProject.sProjectCode = sFormCode;
                    objProject.dCreate = DateTime.Now;
                    objProject.nCreateBy = ua.nUserID;
                    objProject.IsDelete = false;
                    _db.TB_Project.Add(objProject);
                }
                objProject.sProjectName = req.sProjectName ?? "";
                objProject.sProjectAbbr = req.sProjectAbbr ?? "";
                objProject.sIntroduce = req.sIntroduce;
                objProject.nProjectTypeID = req.nProjectTypeID;
                objProject.dUpdate = DateTime.Now;
                objProject.nUpdateBy = ua.nUserID;

                #region lstContactData
                if (req.lstContactData != null)
                {
                    foreach (var item in req.lstContactData)
                    {

                        TB_Project_ContractPoint? objProjectContractPoint = _db.TB_Project_ContractPoint.FirstOrDefault(w => w.nContractPointID == item.nContactPointID && w.nProjectID == item.nProjectID && w.IsDelete != true);
                        if (objProjectContractPoint == null) //Add
                        {
                            objProjectContractPoint = new TB_Project_ContractPoint();
                            objProjectContractPoint.nProjectContractPointID = (_db.TB_Project_ContractPoint.Any() ? _db.TB_Project_ContractPoint.Max(m => m.nProjectContractPointID) : 0) + 1;
                            objProjectContractPoint.nProjectID = nProjectID;
                            objProjectContractPoint.dCreate = DateTime.Now;
                            objProjectContractPoint.nCreateBy = ua.nUserID;
                            _db.TB_Project_ContractPoint.Add(objProjectContractPoint);
                        }
                        objProjectContractPoint.nCustomerID = item.nCustomerID;
                        objProjectContractPoint.nContractPointID = item.nContactPointID;
                        objProjectContractPoint.dUpdate = DateTime.Now;
                        objProjectContractPoint.nUpdateBy = ua.nUserID;
                        objProjectContractPoint.IsDelete = item.IsDel;
                        if (item.IsDel == true)
                        {
                            objProjectContractPoint.dDelete = DateTime.Now;
                            objProjectContractPoint.nDeleteBy = ua.nUserID;
                        }
                        _db.SaveChanges();
                    }
                }
                #endregion
                #region lstMemberData
                if (req.lstMemberData != null)
                {
                    foreach (var item in req.lstMemberData)
                    {
                    
                        TB_Project_Person? objProjectPreson = _db.TB_Project_Person.FirstOrDefault(w => w.nEmployeeID == item.nEmployeeID && w.nPositionID == item.nPositionID && w.nProjectID == item.nProjectID && w.IsDelete != true);
                        if (objProjectPreson == null) //Add
                        {
                            objProjectPreson = new TB_Project_Person();
                            objProjectPreson.nProjectPersonID = (_db.TB_Project_Person.Any() ? _db.TB_Project_Person.Max(m => m.nProjectPersonID) : 0) + 1;
                            objProjectPreson.nProjectID = nProjectID;
                            objProjectPreson.dCreate = DateTime.Now;
                            objProjectPreson.nCreateBy = ua.nUserID;
                            _db.TB_Project_Person.Add(objProjectPreson);
                        }
                        objProjectPreson.nPositionID = item.nPositionID;
                        objProjectPreson.nEmployeeID = item.nEmployeeID;
                        objProjectPreson.IsStatus = item.IsStatus;
                        objProjectPreson.IsDelete = item.IsDel;
                        objProjectPreson.dUpdate = DateTime.Now;
                        objProjectPreson.nUpdateBy = ua.nUserID;
                        if (item.IsDel == true)
                        {
                            objProjectPreson.dDelete = DateTime.Now;
                            objProjectPreson.nDeleteBy = ua.nUserID;
                        }
                        _db.SaveChanges();
                    }
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

        public cProjectData GetDataInfo(int nContractPointID)
        {
            cProjectData result = new cProjectData();
            try
            {

                TB_ContractPoint? oContractPoint = _db.TB_ContractPoint.FirstOrDefault(w => w.nContractPointID == nContractPointID);
                if (oContractPoint != null)
                {
                    result.nContractPointID = oContractPoint.nContractPointID;
                    result.sName = oContractPoint.sNameTH + " " + oContractPoint.sSurnameTH;
                    result.sEmail = oContractPoint.sEmail ?? "";
                    result.sTelephone = oContractPoint.sTelephone ?? "";
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

        public List<EmployeeData>? GetSearchEmployee(string strSearch)
        {
            List<EmployeeData>? lstOption = new List<EmployeeData>();
            if (!string.IsNullOrEmpty(strSearch))
            {
                List<TB_Employee>? lstEmployee = _db.TB_Employee.Where(w => ((!string.IsNullOrEmpty(w.sNameTH) ? w.sNameTH.Contains(strSearch) : false) ||
                    (!string.IsNullOrEmpty(w.sNameEN) ? w.sNameEN.Contains(strSearch) : false)) ||
                    ((!string.IsNullOrEmpty(w.sSurnameTH) ? w.sSurnameTH.Contains(strSearch) : false) ||
                    (!string.IsNullOrEmpty(w.sSurnameEN) ? w.sSurnameEN.Contains(strSearch) : false)) || ((!string.IsNullOrEmpty(w.sNickname) ? w.sNickname.Contains(strSearch) : false)) && w.IsActive == true && w.IsDelete != true).ToList();
                foreach (var item in lstEmployee)
                {
                    var newData = new EmployeeData
                    {
                        nEmployeeID = item.nEmployeeID.ToString(),
                        sFirstname = item.sNameTH ?? item.sNameEN,
                        sLastName = item.sSurnameTH ?? item.sSurnameEN,
                        label = item.sNameTH + " " + item.sSurnameTH + " (" + item.sNickname + ")",
                        value = item.nEmployeeID + "",
                    };
                    lstOption.Add(newData);
                }
            }
            return lstOption;
        }

        public ResultAPI GetData(string sID)
        {
            cProject result = new cProject();
            try
            {

                int nID = (sID + "").DecryptParameter().ToInt();
                IQueryable<TB_ContractPoint>? lstContractPoint = _db.TB_ContractPoint.Where(w=> w.IsDelete != true && w.IsActive== true).AsQueryable();
                IQueryable<TB_Project_ContractPoint>? lstProjectContractPoint = _db.TB_Project_ContractPoint.Where(w => w.IsDelete != true && w.nProjectID == nID).AsQueryable();
                IQueryable<TB_Customer>? lstCustomer= _db.TB_Customer.Where(w => w.IsDelete == false && w.IsActive == true).AsQueryable();
                IQueryable<TB_Project_Person >? lstProjectPerson = _db.TB_Project_Person.Where(w=>w.nProjectID==nID).AsQueryable();
                IQueryable<TB_Position>? lstPorsition =_db.TB_Position.Where(w=> w.IsActive == true).AsQueryable();
                IQueryable<TB_Employee>? lstEmp =_db.TB_Employee.Where(w=> w.IsDelete != true).AsQueryable();
                var oData = _db.TB_Project.FirstOrDefault(f => f.nProjectID == nID && f.IsDelete == false);
                if (oData != null)
                {
                    result.nProjectID = oData.nParentID;
                    result.nProjectID = oData.nParentID;
                    result.nParentID = oData.nParentID;
                    result.sProjectName = oData.sProjectName ?? "";
                    result.sProjectAbbr = oData.sProjectAbbr ?? "";
                    result.sIntroduce = oData.sIntroduce ?? "";
                    result.nProjectTypeID = oData.nProjectTypeID;

                }
                result.lstAllDataContact = (from a in lstProjectContractPoint
                                            //from b in lstCustomer
                                            from c in lstContractPoint.Where(w => w.nContractPointID == a.nContractPointID).DefaultIfEmpty()
                                            select new cProjectContractPoint
                                            {
                                                sID = a.nProjectContractPointID.ToString(),
                                                nProjectContractPointID = a.nProjectContractPointID,
                                                sName = (c != null ? c.sNameTH : "") + " " + (c != null ? c.sSurnameTH : ""),
                                                sEmail = c != null ? c.sEmail : "",
                                                sTel = c != null ? c.sTelephone : "",
                                                //sCustomerName = b != null ? b.sCustomerName : "",
                                                nCustomerID = c.nCustomerID,
                                                nProjectID = a.nProjectID,
                                                nContactPointID = a.nContractPointID,
                                                IsDel = a.IsDelete,
                                            }).ToList();

                //result.lstAllDataContact = (from a in lstContractPoint
                //                            from b in lstCustomer.Where(w => w.nCustomerID == a.nCustomerID).DefaultIfEmpty()
                //                            from c in lstProjectContractPoint.Where(w => w.nContractPointID == a.nContractPointID).DefaultIfEmpty()
                //                            select new cProjectContractPoint
                //                            {
                //                                sID = c != null ? c.nProjectContractPointID.ToString() : null,
                //                                nProjectContractPointID = c!= null ? c.nProjectContractPointID : 0,
                //                                sName = (a.sNameTH) + " " + (a.sSurnameTH),
                //                                sEmail = a.sEmail,
                //                                sTel = a.sTelephone,
                //                                sCustomerName = b != null ? b.sCustomerName : "",
                //                                nProjectID = c != null ? c.nProjectID : 0,
                //                                nContactID = c != null ? c.nContractPointID : 0,
                //                                IsDel = c != null ? c.IsDelete : false,
                //                            }).ToList();

                result.lstAllMemberData =   (from a in lstProjectPerson
                                            from b in lstPorsition.Where(w=>w.nPositionID == a.nPositionID).DefaultIfEmpty()
                                            from c in lstEmp.Where(w => w.nEmployeeID == a.nEmployeeID ).DefaultIfEmpty()
                                            select new cProjectMember
                                            {
                                                sID = a.nProjectPersonID.ToString(),
                                                nProjectID = a.nProjectID,
                                                nPositionID = a.nPositionID,
                                                sPosition =  b!= null ?  b.sPositionName : "",
                                                sEmployeeName = (c != null ? c.sNameTH : "") +" "+ (c!= null ? c.sSurnameTH : ""),
                                                nEmployeeID = a.nEmployeeID,
                                                IsStatus = a.IsStatus,
                                                IsDel = a.IsDelete,
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

        //ProjectList
        public cResultProjectTable GetDataTableProject(cProjectTable req)
        {
            cResultProjectTable result = new cResultProjectTable();
            try
            {

                #region Query
                List<TB_Project> lstProject = _db.TB_Project.Where(w => w.IsDelete != true).ToList();

                var qry = (from a in lstProject
                           from b in _db.TB_Customer.Where(w => w.IsDelete != true ).DefaultIfEmpty()
                           select new ObjectResultProject
                           {
                               sID = a.nProjectID.EncryptParameter(),
                               nID = a.nProjectID,
                               sProjectName = a.sProjectName,
                               //nCustomerID = a.nCustomerID,
                               sCustomerName = b != null ? b.sCustomerName : "",
                               dStart = a.dCreate,
                               sStartDate = a.dCreate.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                               dEnd = a.dUpdate,
                               sEndDate = a.dUpdate.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                               //sProcess = 
                               //sPersen = 
                               //sMasterProcessName = s.sMasterProcessName,
                               //sNote = s.sNote,
                               //isStandProcess = s.isStandProcess,
                               dUpdate = a.dUpdate,
                               sUpdate = a.dUpdate.ToStringFromDate("dd/MM/yyyy" + " " + "HH:mm", "th-TH"),
                               //nParentID = s.nParentID,
                           }).ToList();

                if (!string.IsNullOrEmpty(req.sProjectName))
                {
                    qry = qry.Where(w => (w.sProjectName + "").Trim().ToLower().Contains(req.sProjectName.Trim().ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(req.sCustomer))
                {
                    qry = qry.Where(w => (w.sCustomerName + "").Trim().ToLower().Contains(req.sCustomer.Trim().ToLower())).ToList();
                }
                //if (req.dDatestart != null && req.dDateend == null)
                //{
                //    qry = qry.Where(w => w.dUpdateDate.Date >= req.dDatestart.Value.Date);
                //}
                //else if (req.dDateend != null && req.dDatestart == null)
                //{
                //    qry = qry.Where(w => w.dUpdateDate.Date <= req.dDateend.Value.Date);
                //}
                //else if (req.dDatestart != null && req.dDateend != null)
                //{
                //    qry = qry.Where(w => w.dUpdateDate.Date >= req.dDatestart.Value.Date && w.dUpdateDate.Date <= req.dDateend.Value.Date);
                //}
                #endregion
                #region//SORT
                string sSortColumn = req.sSortExpression;
                switch (req.sSortExpression)
                {
                    case "sCustomerName": sSortColumn = "sCustomerName"; break;
                    case "sProjectName": sSortColumn = "sProjectName"; break;
                    case "sStartDate": sSortColumn = "sStartDate"; break;
                    case "sEndDate": sSortColumn = "sEndDate"; break;
                    case "dUpdate": sSortColumn = "dUpdate"; break;
                }
                if (req.isASC)
                {
                    qry = qry.OrderBy<ObjectResultProject>(sSortColumn).ToList();
                }
                else if (req.isDESC)
                {
                    qry = qry.OrderByDescending<ObjectResultProject>(sSortColumn).ToList();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(req.nPageSize, req.nPageIndex, qry.Count());
                var lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();

                var nIndex = req.nPageSize * (req.nPageIndex - 1) + 1;
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
        public ResultAPI RemoveDataTable(cProjectTable req)
        {
            ResultAPI objResult = new ResultAPI();
            try
            {
                List<int> lstId = req.lstRemove.ConvertAll(item => item.DecryptParameter().ToInt()).ToList();
                var lstProject = _db.TB_Project.Where(w => lstId.Contains(w.nProjectID)).ToList();

                foreach (var item in lstProject)
                {
                    item.IsDelete = true;
                    item.nDeleteBy = 1;
                    item.dDelete = DateTime.Now;
                }
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

        //ProjectInputForm2
        public cResultProject GetInitDataProcess()
        {
            cResultProject result = new cResultProject();
            try
            {

                result.lstConfirmProcessSub = _db.TB_MasterProcess.Where(w => w.IsActive == true && w.nMasterProcessTypeID == 2 && w.nParentID == w.nMasterProcessID).Select(s => new cMasterDataProcess
                {
                    value = s.nMasterProcessID.ToString(),
                    label = (s.sMasterProcessName ?? ""),
                    sNote = s.sNote,
                    nMasterProcessTypeID = s.nMasterProcessTypeID,
                    nParentID = s.nParentID,
                    nOrder = s.nOrder,
                }).ToList();

                //result.lstConfirmProcessMain = _db.TB_MasterProcess.Where(w => w.isActive == true && w.nMasterProcessTypeID == 1 && w.nParentID == null).Select(s => new cMasterDataProcessMain
                //{
                //    value = s.nMasterProcessID.ToString(),
                //    label = (s.sMasterProcessName ?? ""),
                //    //lstDataProcessSub = _db.TB_MasterProcess.Where(w=>w.isActive == true && w.nMasterProcessTypeID == 2 &&  w.nParentID == w.nMasterProcessID).ToList(),
                //}).ToList();


                var lstHead = _db.TB_MasterProcess.Where(w => w.IsActive == true && w.nMasterProcessTypeID == 1 && w.nParentID == null).Select(s => new cMasterDataProcessMain
                {
                    value = s.nMasterProcessID.ToString(),
                    label = (s.sMasterProcessName ?? ""),
                    sID = s.nMasterProcessID.EncryptParameter(),
                    lstDataProcessSub = new cResultProcessTable(),
                }).ToList();

                foreach (var t in lstHead)
                {
                    var objHead = new cProcessTable
                    {
                        nParentID = t.value.ToInt(),
                        nPageIndex = 1,
                        nPageSize = 10,
                        nDataLength = 0,
                        sSortExpression = "dUpdate",
                        sSortDirection = "desc",
                    };
                    //var lstdataSub = GetDataTable(objHead);

                    //cResultProcessTable aSub = new cResultProcessTable();
                    //aSub.arrRows = lstdataSub.lstData ?? new List<ObjectResultProcess>();
                    //aSub.nDataLength = lstdataSub.nDataLength;
                    //aSub.nPageIndex = lstdataSub.nPageIndex;
                    //aSub.nDataLength = lstdataSub.nDataLength;
                    //aSub.nPageIndex = lstdataSub.nPageIndex;
                    //aSub.nPageSize = lstdataSub.nPageSize;
                    //aSub.nSkip = lstdataSub.nSkip;
                    //aSub.nTake = lstdataSub.nTake;
                    //aSub.nStartIndex = lstdataSub.nStartIndex;
                    //aSub.sID = lstdataSub.sID;
                    //aSub.sSortExpression = lstdataSub.sSortExpression;
                    //aSub.sSortDirection = lstdataSub.sSortDirection;

                    //t.lstDataProcessSub = lstdataSub;
                }

                result.lstConfirmProcessMain = lstHead;
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }

        public ResultAPI SaveDataProcess(cProcess req)
        {
            ResultAPI objResult = new ResultAPI();
            try
            {
                int nProcessID = (req.sID ?? "").DecryptParameter().ToInt();
                TB_Project_Process? objProjectProcess = _db.TB_Project_Process.FirstOrDefault(w => w.nProcessID == nProcessID);
                if (objProjectProcess == null)  //Add
                {
                    objProjectProcess = new TB_Project_Process();
                    objProjectProcess.nProcessID = (_db.TB_Project_Process.Any() ? _db.TB_Project_Process.Max(m => m.nProcessID) : 0) + 1; ;
                    //nProcessID = objProjectProcess.nProcessID;
                    objProjectProcess.nMasterProcessID = req.nMasterProcessID;
                    objProjectProcess.nProjectID = req.nProjectID;
                    objProjectProcess.dCreate = DateTime.Now;
                    objProjectProcess.nCreateBy = 1;
                    objProjectProcess.IsDelete = false;
                    _db.TB_Project_Process.Add(objProjectProcess);
                }

                objProjectProcess.nManhour = req.nManhour;
                objProjectProcess.IsManhour = req.isManhour;
                objProjectProcess.dUpdate = DateTime.Now;
                objProjectProcess.nUpdateBy = 1;

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

        public ResultAPI ChangeOrder(cProcess oData)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                int nMasterProcessID = (oData.sID + "").DecryptParameter().ToInt();
                List<TB_MasterProcess> lstMasterProcess = _db.TB_MasterProcess.Where(w => w.IsDelete != true).ToList();
                TB_MasterProcess? objProjectProcess = lstMasterProcess.FirstOrDefault(f => f.nMasterProcessID == nMasterProcessID);
                if (objProjectProcess != null)
                {
                    int oldOrder = objProjectProcess.nOrder;
                    bool isOrderNewLower = objProjectProcess.nOrder < oData.nOrder;
                    objProjectProcess.nOrder = oData.nOrder;
                    objProjectProcess.dUpdate = DateTime.Now;
                    objProjectProcess.nUpdateBy = 1;

                    List<TB_MasterProcess> lstMasterOrderNew = lstMasterProcess.Where(w =>
                    (isOrderNewLower ?
                    w.nOrder <= oData.nOrder && w.nOrder >= oldOrder :
                    w.nOrder >= oData.nOrder && w.nOrder <= oldOrder)
                    && w.nMasterProcessID == objProjectProcess.nMasterProcessID && w.nMasterProcessID != nMasterProcessID).ToList();

                    foreach (var objItem in lstMasterOrderNew)
                    {
                        if (isOrderNewLower)
                        {
                            objItem.nOrder -= 1;
                        }
                        else
                        {
                            objItem.nOrder += 1;
                        }
                        objProjectProcess.dUpdate = DateTime.Now;
                        objProjectProcess.nUpdateBy = 1;
                    }
                    _db.SaveChanges();
                    result.nStatusCode = StatusCodes.Status200OK;
                }
            }
            catch (System.Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }

        public cProcess GetDataProcess(int nProcessID)
        {
            cProcess result = new cProcess();
            try
            {


                int nid = nProcessID;
                var oData = nid != 0 ? _db.TB_Project_Process.FirstOrDefault(f => f.nProcessID == nid) : null;
                if (oData != null)
                {
                    result.nProcessID = nid;
                    result.nMasterProcessID = oData.nMasterProcessID;
                    result.nProjectID = oData.nProjectID;
                    result.nManhour = oData.nManhour;
                    result.isManhour = oData.IsManhour;
                    //result.isActive = oData.isActive;
                    result.nOrder = oData.nOrder;
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

        //public cResultProcessTable GetDataTable(cProcessTable param)
        //{
        //    cResultProcessTable result = new cResultProcessTable();
        //    try
        //    {
        //        List<TB_MasterProcess> lstMasterProcess = _db.TB_MasterProcess.Where(w => w.IsActive == true).OrderBy(o => o.nOrder).ToList();

        //        #region Query
        //        var qry = (from s in lstMasterProcess.Where(w => w.IsDelete != true && w.nMasterProcessTypeID != 1 && w.nParentID == param.nParentID)
        //                   from b in lstMasterProcess.Where(w => w.IsDelete != true && w.nMasterProcessID == s.nParentID).DefaultIfEmpty()
        //                   select new ObjectResultProcess
        //                   {

        //                       sID = s.nMasterProcessID.EncryptParameter(),
        //                       nID = s.nMasterProcessID,
        //                       nOrder = s.nOrder,
        //                       sMasterProcessName = s.sMasterProcessName,
        //                       sNote = s.sNote,
        //                       IsStandProcess = s.IsStandProcess,
        //                       dUpdate = s.dUpdate,
        //                       nParentID = s.nParentID,
        //                   }).OrderBy(w => w.nMasterProcessID).ThenBy(w => w.nParentID).ThenBy(w => w.nOrder).ToList(); ;

        //        var lstOrder = qry.Select(s => s.nOrder).OrderBy(o => o).Distinct().ToList();
        //        List<cOrderOptions> lstOrderOptions = new List<cOrderOptions>();
        //        foreach (var item in lstOrder)
        //        {
        //            var objOptions = new cOrderOptions();
        //            objOptions.value = item;
        //            objOptions.label = item.ToString();
        //            lstOrderOptions.Add(objOptions);
        //        }

        //        result.lstOrderOptions = lstOrderOptions;


        //        #endregion

        //        #region//SORT
        //        string sSortColumn = param.sSortExpression;
        //        switch (param.sSortExpression)
        //        {


        //            case "sMasterProcessName": sSortColumn = "sMasterProcessName"; break;
        //            case "sNote": sSortColumn = "sNote"; break;
        //            case "nOrder": sSortColumn = "nOrder"; break;
        //            case "dUpdate": sSortColumn = "dUpdate"; break;
        //        }
        //        if (param.isASC)
        //        {
        //            qry = qry.OrderBy<ObjectResultProcess>(sSortColumn).ToList();
        //        }
        //        else if (param.isDESC)
        //        {
        //            qry = qry.OrderByDescending<ObjectResultProcess>(sSortColumn).ToList();
        //        }
        //        #endregion

        //        #region//Final Action >> Skip , Take And Set Page
        //        var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qry.Count());
        //        var lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();


        //        result.lstData = lstData;
        //        result.nDataLength = dataPage.nDataLength;
        //        result.nPageIndex = dataPage.nPageIndex;
        //        result.nSkip = dataPage.nSkip;
        //        result.nTake = dataPage.nTake;
        //        result.nStartIndex = dataPage.nStartIndex;
        //        #endregion

        //        result.nStatusCode = StatusCodes.Status200OK;
        //    }
        //    catch (Exception e)
        //    {
        //        result.nStatusCode = StatusCodes.Status500InternalServerError;
        //        result.sMessage = e.Message;
        //    }

        //    return result;
        //}
        #region sun
        /// <summary>
        /// บันทึก ProjectProposal
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResultAPI SaveProjectTOR(cProjectTOR req)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                if (req.lstDataTOR != null)
                {
                    foreach (var item in req.lstDataTOR)
                    {
                        int nProjectID = (item.sProjectID + "").DecryptParameter().ToInt();
                        TB_Project_Proposal? ObjectTOR = _db.TB_Project_Proposal.FirstOrDefault(w => w.nProjectID == nProjectID && w.nProposalID == item.sID.ToInt() && w.IsDelete != true);
                        if (ObjectTOR == null)
                        {
                            ObjectTOR = new TB_Project_Proposal();
                            ObjectTOR.nProposalID = _db.TB_Project_Proposal.Any() ? _db.TB_Project_Proposal.Max(m => m.nProposalID) + 1 : 1;
                            ObjectTOR.nProjectID = nProjectID;
                            ObjectTOR.sCode = item.sCode;
                            ObjectTOR.dCreate = DateTime.Now;
                            ObjectTOR.nCreateBy = 1;
                            ObjectTOR.IsDelete = false;

                            _db.TB_Project_Proposal.Add(ObjectTOR);
                        }
                        ObjectTOR.sDetail = item.sDetail;
                        ObjectTOR.dUpdate = DateTime.Now;
                        ObjectTOR.nUpdateBy = 1;
                        if (item.IsDel == true)
                        {
                            ObjectTOR.IsDelete = true;
                            ObjectTOR.nDeleteBy = 1;
                            ObjectTOR.dDelete = DateTime.Now;
                        }
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
        /// <summary>
        /// แสดงข้อมูล ProjectProposal 
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <returns></returns>
        public ResultAPI GetDataProjectTOR(string? sProjectID)
        {
            cProjectTOR result = new cProjectTOR();
            try
            {
                int nProjectID = (sProjectID + "").DecryptParameter().toInt();
                var lstDataTOR = _db.TB_Project_Proposal.Where(w => w.nProjectID == nProjectID && w.IsDelete != true).Select(s => new ObjectProjectTOR
                {
                    sID = s.nProposalID.ToString(),
                    sProjectID = (s.nProjectID + "").EncryptParameter().ToString(),
                    sCode = s.sCode != null ? s.sCode : null,
                    sDetail = s.sDetail != null ? s.sDetail : null,
                    IsDel = false,
                }).OrderBy(o => o.sCode);
                result.lstDataTOR = lstDataTOR.ToList();
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }
        /// <summary>
        /// ExportExcel ProjectProposal
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //public cReturnExportExcelTOR ExportExcelTOR(cExportExcelTOR param)
        //{
        //    cReturnExportExcelTOR result = new cReturnExportExcelTOR();
        //    DateTime dDate = DateTime.Now;
        //    string sDate = dDate.ToString("DDMMYYYYHHmmss");
        //    string sFileName = "ExportExcelTOR_" + sDate;
        //    try
        //    {
        //        int nProjectID = (param.sProjectID + "").DecryptParameter().toInt();
        //        var lstDataTOR = _db.TB_Project_Proposal.Where(w => w.nProjectID == nProjectID && w.IsDelete != true).Select(s => new ObjectProjectTOR
        //        {
        //            sID = s.nProposalID.ToString(),
        //            sProjectID = (s.nProjectID + "").EncryptParameter().ToString(),
        //            sCode = s.sCode != null ? s.sCode : null,
        //            sDetail = s.sDetail != null ? s.sDetail : null,
        //            IsDel = false,
        //        }).OrderBy(o => o.sCode).ToList();
        //        using (var db = new ST_IntranetEntity())
        //        {
        //            var wb = new XLWorkbook();
        //            IXLWorksheet ws = wb.Worksheets.Add("data");
        //            int nFontSize = SystemFunction.FontSize_Export;
        //            int nFontSizeHead = SystemFunction.FontSize_Export_Head;
        //            var Color_Head = SystemFunction.Color_Head;
        //            var Color_Foot = SystemFunction.Color_Foot;

        //            ws.PageSetup.Margins.Top = 0.2;
        //            ws.PageSetup.Margins.Bottom = 0.2;
        //            ws.PageSetup.Margins.Left = 0.1;
        //            ws.PageSetup.Margins.Right = 0;
        //            ws.PageSetup.Margins.Footer = 0;
        //            ws.PageSetup.Margins.Header = 0;
        //            //ws.Columns("A:Z").Style.NumberFormat.Format = "@"; //ป้องกันแปลงวันที่
        //            //ws.Range("A1:B1").Merge(); //การรวม Cell

        //            #region  Header
        //            int nRow = 1;
        //            List<int> lstwidthHeader = new List<int>() { 15, 50 };
        //            List<string> lstHeader = new List<string>() { "Code", "Detail" };
        //            var RowsHead = ws;
        //            var itemCell = RowsHead.Cell(nRow, 1);

        //            int nColWidth = 1;
        //            foreach (var itmWidth in lstwidthHeader)
        //            {
        //                ws.Column(nColWidth).Width = itmWidth;
        //                nColWidth++;
        //            }
        //            int indexHead = 0;

        //            //nRow++; //ขึ้น Row ใหม่
        //            foreach (var item in lstHeader)
        //            {
        //                indexHead += 1;
        //                ws.Cell(nRow, indexHead).Value = item;
        //                ws.Cell(nRow, indexHead).Style.Font.FontSize = nFontSize;
        //                ws.Cell(nRow, indexHead).Style.Font.Bold = true;
        //                ws.Cell(nRow, indexHead).Style.Font.FontColor = XLColor.White;
        //                ws.Cell(nRow, indexHead).Style.Alignment.WrapText = true; //ปัดบรรทัด
        //                ws.Cell(nRow, indexHead).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                ws.Cell(nRow, indexHead).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        //                ws.Cell(nRow, indexHead).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("Green"));
        //            }
        //            #endregion
        //            int nStartBorder = nRow + 1;
        //            if (lstDataTOR != null && lstDataTOR.Any())
        //            {
        //                foreach (var item in lstDataTOR)
        //                {
        //                    nRow++; //ขึ้นบรรทัดใหม่

        //                    ws.Cell(nRow, 1).Value = item.sCode;
        //                    ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                    ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        //                    ws.Cell(nRow, 1).Style.Font.FontSize = nFontSize;
        //                    ws.Cell(nRow, 1).Style.Alignment.WrapText = true;

        //                    ws.Cell(nRow, 2).Value = item.sDetail;
        //                    ws.Cell(nRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                    ws.Cell(nRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        //                    ws.Cell(nRow, 2).Style.Font.FontSize = nFontSize;
        //                    ws.Cell(nRow, 2).Style.Alignment.WrapText = true;

        //                    ws.Cell(nRow, 20).Value = item.sID;
        //                }
        //            }
        //            else
        //            {
        //                // No data
        //                nRow++;
        //                ws.Range("A" + nRow + ":B" + nRow).Merge();
        //                ws.Cell(nRow, 1).Value = "ไม่พบข้อมูล";
        //                ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        //            }
        //            //ระยะเส้นขอบที่ต้องการให้แสดง
        //            ws.Range(nStartBorder, 1, nRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        //            ws.Range(nStartBorder, 1, nRow, 2).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        //            _db.Dispose();
        //            using (MemoryStream fs = new MemoryStream())
        //            {
        //                wb.SaveAs(fs);
        //                wb.Dispose();
        //                fs.Position = 0;
        //                result.objFile = fs.ToArray();
        //                result.sFileType = "application/vnd.ms-excel";
        //                result.sFileName = sFileName;
        //                return result;
        //            }
        //        }
        //    }
        //    catch (System.Exception)
        //    {
        //        //Error ส่งออกกระดาษเปล่าไป

        //        using (MemoryStream fs = new MemoryStream())
        //        {
        //            var wb = new XLWorkbook();
        //            IXLWorksheet ws = wb.Worksheets.Add("Error");
        //            wb.SaveAs(fs);
        //            wb.Dispose();
        //            fs.Position = 0;
        //            result.objFile = fs.ToArray();
        //            result.sFileType = "application/vnd.ms-excel";
        //            result.sFileName = sFileName;
        //            return result;
        //        }
        //    }
        //}

        //public cReturnImportExcelTOR ImportExcelTOR(reqImportTOR req)
        //{
        //    cReturnImportExcelTOR result = new cReturnImportExcelTOR();
        //    try
        //    {
        //        result.nStatusCode = StatusCodes.Status200OK; //check frontend message
        //        int nProjectID = (req.nMasterID + "").DecryptParameter().toInt();
        //        List<ObjectProjectTOR>? lstData = new List<ObjectProjectTOR>();
        //        List<string> lstMsg = new List<string>();
        //        if (req != null && req.fFile != null && req.fFile.Any())
        //        {
        //            ItemFileData? oFile = req.fFile.FirstOrDefault();
        //            if (oFile != null)
        //            {
        //                string sMessage = "";
        //                using (var db = new ST_IntranetEntity())
        //                {
        //                    //#region Get Data from Database
        //                    //var ProjectProposal = db.TB_Project_Proposal.Where(w => w.nProjectID == null);
        //                    //#endregion
        //                    string path = "Import\\IExcelTOR";
        //                    if (!Directory.Exists(STFunction.MapPath(path, _env)))
        //                    {
        //                        Directory.CreateDirectory(STFunction.MapPath(path, _env));
        //                    }
        //                    if (File.Exists(STFunction.MapPath(oFile.sPath + "/" + oFile.sSysFileName, _env)) && !string.IsNullOrEmpty(oFile.sSysFileName))
        //                    {
        //                        //SystemFunction.MoveFile(oFile.sPath + "/", path + "/", oFile.sSysFileName, _env);
        //                    }
        //                    string sMapPathFileCurrent = STFunction.MapPath(path + "\\" + oFile.sSysFileName, _env);
        //                    if (File.Exists(sMapPathFileCurrent))
        //                    {
        //                        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //                        using (var stream = new FileStream(sMapPathFileCurrent, FileMode.Open, FileAccess.Read))
        //                        {
        //                            IExcelDataReader xlsRd = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //                            DataSet ds = xlsRd.AsDataSet();
        //                            var sheet = "data";
        //                            DataTable? dt = ds.Tables[sheet];
        //                            string[] arrColumnHeader = new string[] { "Code", "Detail" };
        //                            List<string> lstInvalidColumn = new List<string>();
        //                            if (dt != null)
        //                            {
        //                                int index = 1;
        //                                for (int i = 0; i < dt.Rows.Count; i++)
        //                                {
        //                                    DataRow dr = dt.Rows[i];
        //                                    if (i == 0) //Check Header Column
        //                                    {
        //                                        for (int j = 0; j < arrColumnHeader.Length; j++)
        //                                        {
        //                                            var a = dr[j] + "";
        //                                            if ((dr[j] + "").Trim() != arrColumnHeader[j].Trim())
        //                                            {
        //                                                lstInvalidColumn.Add(dr[j] + "");
        //                                            }
        //                                        }

        //                                        if (lstInvalidColumn.Any())
        //                                        {
        //                                            string sInvalid = string.Join(", ", lstInvalidColumn);
        //                                            result.nStatusCode = StatusCodes.Status200OK;
        //                                            result.sMessage = "ชื่อคอลัมธ์ไม่ตรงกับเทมเพลต";
        //                                            // result.Message = "ชื่อคอลัมธ์ไม่ตรงกับเทมเพลต " + sInvalid;
        //                                            return result;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        int RowId = i + 1;
        //                                        if (!string.IsNullOrEmpty(dr[0] + ""))  //Check data Column
        //                                        {
        //                                            // string sMessageNotPass = "";
        //                                            bool IsPassValidateData = true;

        //                                            // check Data input 
        //                                            string? sCode = (dr[0] + "").Trim();
        //                                            string? sDetail = (dr[1] + "").Trim();

        //                                            string? sID = (dr[19] + "").Trim();

        //                                            bool IsEmpty = string.IsNullOrEmpty(sCode);
        //                                            if (!IsEmpty)
        //                                            {
        //                                                ObjectProjectTOR objDataTOR = new ObjectProjectTOR();
        //                                                objDataTOR.sID = sID;
        //                                                objDataTOR.sCode = sCode != null ? sCode : "";
        //                                                objDataTOR.sDetail = sDetail != null ? sDetail : null;
        //                                                objDataTOR.IsDel = false;
        //                                                objDataTOR.sProjectID = nProjectID.EncryptParameter();
        //                                                lstData.Add(objDataTOR);
        //                                                index++;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    db.Dispose();

        //                }
        //                result.sMessage = sMessage;
        //            }
        //            else
        //            {
        //                result.sMessage = "Invalid template.";
        //            }
        //        }
        //        else
        //        {
        //            result.sMessage = "Error: Template does not format. ";
        //        }

        //        result.lstProjectTOR = lstData;
        //        result.nStatusCode = StatusCodes.Status200OK;
        //    }
        //    catch (Exception e)
        //    {
        //        result.nStatusCode = StatusCodes.Status500InternalServerError;
        //        result.sMessage = e.Message;
        //    }
        //    return result;
        //}
        #endregion



        #region Project Planning Test
        /// <summary>
        /// </summary>
        //public objOptionPlanTest OptionPlanTest()
        //{
        //    TokenJWTSecret UserAccount = _authen.GetUserAccount();
        //    int nUserID = UserAccount.nUserID ?? 0;
        //    objOptionPlanTest result = new objOptionPlanTest();
        //    try
        //    {

        //        IQueryable<TB_Project> TBProject = _db.TB_Project.Where(w => !w.IsDelete);
        //        IQueryable<TB_Employee> TBEmployee = _db.TB_Employee.Where(w => !w.IsDelete);
        //        IQueryable<TB_Employee_Position> TBEmployeePosition = _db.TB_Employee_Position.Where(w => !w.IsDelete);
        //        IQueryable<TB_Project_Action> TBProjectAction = _db.TB_Project_Action.Where(w => !w.IsDelete);
        //        IQueryable<TB_Project_Plan> TBProjectPlan = _db.TB_Project_Plan.Where(w => !w.IsDelete);
        //        List<string> listProjectTest = new List<string>();
        //        List<string> listsTest = new List<string>();

        //        var qry = (from a in TBProjectAction.Where(w => w.nEmployeeID == nUserID)
        //                   from b in TBProjectPlan.Where(f => f.nPlanID == a.nPlanID)
        //                   from c in TBProject.Where(n => n.nProjectID == b.nProjectID)
        //                   select new { c });
        //        foreach (var item in qry)
        //        {
        //            listProjectTest.Add(item.c.nProjectID + "");
        //        }

        //        result.listProject = TBProject.Select(s => new objOption
        //        {
        //            value = s.nProjectID + "",
        //            label = s.sProjectName
        //        }).ToList();

        //        var qryTest = (from a in TBEmployee.Where(w => w.nEmployeeID == nUserID)
        //                       from b in TBEmployeePosition.Where(f => f.nPositionID == (int)TypePosition.Tester && f.nEmployeeID == a.nEmployeeID)
        //                       select new { a, b });

        //        var qryEmpAllTest = (from a in TBEmployee
        //                             from b in TBEmployeePosition.Where(f => f.nPositionID == (int)TypePosition.Tester && f.nEmployeeID == a.nEmployeeID)
        //                             select new { a, b });

        //        foreach (var item in qryTest)
        //        {
        //            listsTest.Add(item.a.nEmployeeID + "");
        //        }

        //        result.listTester = qryEmpAllTest.Select(s => new objOption
        //        {
        //            value = s.a.nEmployeeID + "",
        //            label = s.a.sNameTH + " " + s.a.sSurnameTH
        //        }).ToList();

        //        result.listProjectTest = listProjectTest;
        //        result.listsTester = listsTest;
        //        result.nStatusCode = StatusCodes.Status200OK;

        //    }
        //    catch (Exception e)
        //    {
        //        result.nStatusCode = StatusCodes.Status500InternalServerError;
        //        result.sMessage = e.Message;
        //    }
        //    return result;
        //}
        ///// <summary>
        ///// </summary>
        //public resultDynamicTable GetDataTablePlanTest(clsFilterProject obj)
        //{
        //    TokenJWTSecret UserAccount = _authen.GetUserAccount();
        //    int nUserID = UserAccount.nUserID ?? 0;
        //    resultDynamicTable result = new resultDynamicTable();
        //    try
        //    {
        //        DateTime? dDateStart = Convert.ToDateTime(obj.sStartDate, CultureInfo.InvariantCulture);
        //        DateTime? dDateEnd = Convert.ToDateTime(obj.sEndDate, CultureInfo.InvariantCulture);
        //        List<int?>? listProject = new List<int?>();
        //        List<int?>? listTester = new List<int?>();
        //        if (obj != null)
        //        {
        //            listProject = obj.sProject != null ? obj.sProject.Split(',').Select(s => s.ToIntOrNull()).ToList() : new List<int?>();
        //            listTester = obj.sTester != null ? obj.sTester.Split(',').Select(s => s.ToIntOrNull()).ToList() : new List<int?>();
        //        }

        //        IQueryable<TB_Project> TBProject = _db.TB_Project.Where(w => !w.IsDelete);
        //        IQueryable<TB_Employee> TBEmployee = _db.TB_Employee.Where(w => !w.IsDelete);
        //        IQueryable<TB_Employee_Position> TBEmployeePosition = _db.TB_Employee_Position.Where(w => !w.IsDelete);
        //        IQueryable<TB_Position> TB_Position = _db.TB_Position.Where(w => !w.IsDelete);
        //        IQueryable<TB_Project_Action> TBProjectAction = _db.TB_Project_Action.Where(w => !w.IsDelete);
        //        IQueryable<TB_Project_Plan> TBProjectPlan = _db.TB_Project_Plan.Where(w => !w.IsDelete);
        //        List<ProjectPlaning.cActionBy> lstActionInProject = new List<ProjectPlaning.cActionBy>();
        //        List<dynamicTable> listAll = new List<dynamicTable>();
        //        List<int> listAllTester = new List<int>();
        //        List<int> listSomeTester = new List<int>();

        //        var qryEmpAllTest = (from a in TBEmployee
        //                             from b in TBEmployeePosition.Where(f => f.nPositionID == (int)TypePosition.Tester && f.nEmployeeID == a.nEmployeeID)
        //                             select new { a, b });

        //        foreach (var item in qryEmpAllTest)
        //        {
        //            if (item.b.nEmployeeID != nUserID)
        //            {
        //                listAllTester.Add(item.b.nEmployeeID);
        //            }
        //        }

        //        // var qry = (from a in TBProject.Where(w=> listProject != null ?  listProject.Contains(w.nProjectID) : true)
        //        //             from b in TBProjectAction.Where(f=> listTester.Contains(f.nEmployeeID))
        //        //             from c in TBProjectPlan.Where(p => p.nPlanID == b.nPlanID)
        //        //             select new {a,b,c});
        //        // var qryTesterAction = TBProjectAction 
        //        var qry = TBProject.Where(w => listProject.Count() > 0 ? listProject.Contains(w.nProjectID) : true);

        //        // var qry = ( from b in TBProjectAction.Where(f=> listTester != null ? listTester.Contains(f.nEmployeeID) : true)
        //        //             from c in TBProjectPlan.Where(p => (listProject != null ? listProject.Contains(p.nProjectID) : true) &&  (p.nPlanID == b.nPlanID))
        //        //             from a in TBProject.Where(t=> t.nProjectID == c.nProjectID)
        //        //             select new {b,c,a});


        //        lstActionInProject = (from d in TBEmployeePosition.Where(w => w.nPositionID == (int)TypePosition.Tester)
        //                              from b in TBEmployee.Where(f => f.nEmployeeID == d.nEmployeeID)
        //                              from f in TB_Position.Where(w => w.nPositionID == d.nPositionID)
        //                              select new Models.ProjectPlaning.cActionBy
        //                              {
        //                                  sID = b.nEmployeeID + "",
        //                                  value = b.nEmployeeID + "",
        //                                  nRole = d.nPositionID,
        //                                  sRole = f.sPositionAbbr,
        //                                  sName = b.sNameTH + " " + b.sSurnameTH + " (" + b.sNickname + ")",
        //                                  label = b.sNameTH + " " + b.sSurnameTH + " (" + b.sNickname + ")",
        //                                  sManHour = "",
        //                                  sType = "1",
        //                                  IsDelete = false,
        //                                  srcImage = "https://w7.pngwing.com/pngs/340/946/png-transparent-avatar-user-computer-icons-software-developer-avatar-child-face-heroes-thumbnail.png",
        //                              }).ToList();
        //        int nOrder = 1;
        //        foreach (var item in lstActionInProject)
        //        {
        //            item.nOrder = nOrder;
        //            nOrder++;
        //        }
        //        result.lstActionBy = lstActionInProject;

        //        foreach (var item in qry)
        //        {
        //            List<objDynamicTable> listMainSub = new List<objDynamicTable>();
        //            List<objDynamicTable> listSub = new List<objDynamicTable>();
        //            dynamicTable table = new dynamicTable();
        //            table.sName = item.sProjectName;

        //            var qrySub = (from b in TBProjectAction.Where(f => listTester.Count() > 0 ? listTester.Contains(f.nEmployeeID) : true)
        //                          from c in TBProjectPlan.Where(p => (p.nProjectID == item.nProjectID) && (p.nPlanID == b.nPlanID))
        //                          from a in TBProject.Where(t => t.nProjectID == c.nProjectID)
        //                          select new { b, c, a });

        //            // Take nPlanID
        //            IQueryable<TB_Project_Action>? qryCheckAction = TBProjectAction.Where(f => listAllTester != null ? listAllTester.Contains(f.nEmployeeID) : true);
        //            if (qryCheckAction == null)
        //            {
        //                var qryAllAction = (from b in TBProjectAction
        //                                    from c in TBProjectPlan.Where(p => (p.nProjectID == item.nProjectID) && (p.nPlanID == b.nPlanID))
        //                                    from a in TBProject.Where(t => t.nProjectID == c.nProjectID)
        //                                    select new { b, c, a });

        //                listSub = qryAllAction.Select(s => new objDynamicTable
        //                {
        //                    sID = s.c.nPlanID,
        //                    nProjectID = s.a.nProjectID,
        //                    sDevelopTask = s.c.sDetail + "",
        //                    dStartDev = s.c.dPlanStart,
        //                    dEndDev = s.c.dPlanEnd,
        //                    nProgress = s.c.nPlanProgress,
        //                    sPlanManhour = s.c.nTesterManhour + "",
        //                    // sUpdateBy = 
        //                    lstActionBy = GetActionByPlanID(s.c.nPlanID, s.a.nProjectID)
        //                }).ToList();

        //                // listAll.Add(table);
        //            }
        //            else
        //            {
        //                foreach (var itemCheck in qryCheckAction)
        //                {
        //                    listSomeTester.Add(itemCheck.nPlanID);
        //                }
        //                listSomeTester = listSomeTester.Select(x => x).Distinct().ToList();

        //                var qrySomeAction = (from b in TBProjectAction.Where(w => !listSomeTester.Contains(w.nPlanID))
        //                                     from c in TBProjectPlan.Where(p => (p.nProjectID == item.nProjectID) && (p.nPlanID == b.nPlanID))
        //                                     from a in TBProject.Where(t => t.nProjectID == c.nProjectID)
        //                                     select new { b, c, a });

        //                listSub = qrySomeAction.Select(s => new objDynamicTable
        //                {
        //                    sID = s.c.nPlanID,
        //                    nProjectID = s.a.nProjectID,
        //                    sDevelopTask = s.c.sDetail + "",
        //                    dStartDev = s.c.dPlanStart,
        //                    dEndDev = s.c.dPlanEnd,
        //                    nProgress = s.c.nPlanProgress,
        //                    sPlanManhour = s.c.nTesterManhour + "",
        //                    lstActionBy = GetActionByPlanID(s.c.nPlanID, s.a.nProjectID)
        //                }).ToList();
        //            }

        //            listMainSub = qrySub.Select(s => new objDynamicTable
        //            {
        //                sID = s.c.nPlanID,
        //                nProjectID = s.a.nProjectID,
        //                sDevelopTask = s.c.sDetail + "",
        //                dStartDev = s.c.dPlanStart,
        //                dEndDev = s.c.dPlanEnd,
        //                nProgress = s.c.nPlanProgress,
        //                sPlanManhour = s.c.nTesterManhour + "",
        //                lstActionBy = GetActionByPlanID(s.c.nPlanID, s.a.nProjectID)
        //            }).ToList();

        //            table.listProjectSub = listMainSub.Concat(listSub).ToList();
        //            //  table.lstActionBy = GetActionByPlanID(s.nPlanID, s.nProjectID)

        //            listAll.Add(table);

        //        }

        //        result.listProject = listAll;
        //        result.Status = StatusCodes.Status200OK;

        //    }

        //    catch (Exception e)
        //    {
        //        result.Status = StatusCodes.Status500InternalServerError;
        //        result.Message = e.Message;
        //    }
        //    return result;
        //}

        //public static List<Models.ProjectPlaning.cActionBy> GetActionByPlanID(int nPlanID, int nProjectID)
        //{
        //    List<Models.ProjectPlaning.cActionBy> lstAction = new List<Models.ProjectPlaning.cActionBy>();
        //    using (ST_IntranetEntity _db = new ST_IntranetEntity())
        //    {
        //        lstAction = (from a in _db.TB_Project_Action.Where(w => !w.IsDelete && w.nPlanID == nPlanID)
        //                     from b in _db.TB_Employee_Position.Where(w => w.nEmployeeID == a.nEmployeeID && !w.IsDelete && w.nPositionID == (int)TypePosition.Tester)
        //                     from c in _db.TB_Employee.Where(w => w.nEmployeeID == a.nEmployeeID && !w.IsDelete && w.IsActive)
        //                     from d in _db.TB_Position.Where(w => b == null ? false : w.nPositionID == b.nPositionID && !w.IsDelete && w.IsActive)
        //                     select new ProjectPlaning.cActionBy
        //                     {
        //                         sID = a.nEmployeeID + "",
        //                         value = a.nEmployeeID + "",
        //                         nRole = d.nPositionID,
        //                         sRole = d.sPositionAbbr,
        //                         sName = c.sNameTH + " " + c.sSurnameTH + " (" + c.sNickname + ")",
        //                         label = c.sNameTH + " " + c.sSurnameTH + " (" + c.sNickname + ")",
        //                         sManHour = a.nManhour + "",
        //                         sType = a.nCalculateTypeID + "",
        //                         IsDelete = false,
        //                         srcImage = "https://w7.pngwing.com/pngs/340/946/png-transparent-avatar-user-computer-icons-software-developer-avatar-child-face-heroes-thumbnail.png",
        //                     }).ToList();
        //    }
        //    return lstAction;
        //}
        #endregion
    }
}
