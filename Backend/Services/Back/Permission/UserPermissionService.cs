using Backend.EF.ST_Intranet;
using Backend.Enum;
using Backend.Interfaces.Authentication;
using Backend.Interfaces.Back.Permission;
using Backend.Interfaces.Back.Permission.Menu;
using Backend.Models;
using Backend.Models.Authentication;
using Backend.Models.Back.Permission;
using ST.INFRA;
using ST.INFRA.Common;

namespace Backend.Services.Permission
{
    public class UserPermissionService : IUserPermission
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _Auth;
        private readonly IHostEnvironment _env;
        private readonly IMenuService _menuservice;
        /// <summary>
        /// </summary>
        public UserPermissionService(ST_IntranetEntity db, IAuthentication auth, IHostEnvironment env, IMenuService menuService)
        {
            _db = db;
            _Auth = auth;
            _env = env;
            _menuservice = menuService;
        }

        public cResultOption GetOptionInitialData()
        {
            cResultOption result = new cResultOption();
            try
            {
                result.lstUserType = _db.TM_Data.Where(w => !w.IsDelete && w.IsActive && w.nDatatypeID == 7).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng)
                }).ToList();
                result.lstUserGroup = _db.TB_UserGroup.Where(w => !w.IsDelete && w.IsActive != false).Select(s => new cSelectOption
                {
                    value = s.nUserGroupID.ToString(),
                    label = s.sUserGroupName
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
        public List<cOptionAutoComplete> GetEmployee(string? strSearch)
        {
            List<cOptionAutoComplete> lstOption = new List<cOptionAutoComplete>();
            IQueryable<TB_Employee> lstEmployee = _db.TB_Employee.Where(w => !w.IsDelete).AsQueryable();
            IQueryable<TB_Employee_Position> lstEmpPosition = _db.TB_Employee_Position.Where(w => !w.IsDelete).AsQueryable();
            IQueryable<TB_Position> lstPosition = _db.TB_Position.Where(w => !w.IsDelete).AsQueryable();
            List<cOptionAutoComplete> lstData = (from a in lstEmployee.Where(w => !string.IsNullOrEmpty(strSearch) ? (w.nEmployeeID.ToString().Contains(strSearch) || (w.sNameTH != null ? w.sNameTH.Contains(strSearch) : true) || (w.sSurnameTH != null ? w.sSurnameTH.Contains(strSearch) : true) || (w.sNameEN != null ? w.sNameEN.Contains(strSearch) : true) || (w.sSurnameEN != null ? w.sSurnameEN.Contains(strSearch) : true)) : true)
                                                 from b in lstEmpPosition.Where(w => w.nEmployeeID == a.nEmployeeID).DefaultIfEmpty()
                                                 from c in lstPosition.Where(w => w.nPositionID == b.nPositionID).DefaultIfEmpty()
                                                 select new cOptionAutoComplete
                                                 {
                                                     sNameTH = a.sNameTH,
                                                     sSureNameTH = a.sSurnameTH,
                                                     sEmail = a.sEmail,
                                                     sPhoneNumber = a.sTelephone,
                                                     sPosition = c.sPositionName,
                                                     label = a.nEmployeeID.ToString() + " - " + a.sNameTH + " " + a.sSurnameTH + " " + "(" + a.sNickname + ")",
                                                     value = a.nEmployeeID.ToString(),
                                                 }).ToList();
            lstOption = lstData;
            return lstOption;
        }
        public cResultUserTable GetDataTable(cReqUserTable param)
        {
            cResultUserTable result = new cResultUserTable();
            try
            {
                #region Query
                List<TB_Employee> lstEmployee = _db.TB_Employee.Where(w => !w.IsDelete).ToList();
                List<TB_UserPermission> lstUserPermission = _db.TB_UserPermission.ToList();

                List<ObjectUser>? qry = (from a in lstEmployee
                                         select new ObjectUser
                                         {
                                             sID = a.nEmployeeID.EncryptParameter(),
                                             nEmployeeID = a.nEmployeeID.ToString(),
                                             sName = a.sNameTH + " " + a.sSurnameTH + " " + "(" + a.sNickname + ")",
                                             sNameTH = a.sNameTH,
                                             sSurnameTH = a.sSurnameTH,
                                             nUpdateBy = a.nUpdateBy,
                                             //sUpdateBy = c.sName + " " + c.sSureName,
                                             dUpdate = a.dUpdate,
                                             IsActive = a.IsActive,
                                             nPermission = lstUserPermission.Count(w => w.nEmployeeID == a.nEmployeeID && w.nPermission > 0),
                                             lstPermission = GetUserPermissionList(lstUserPermission.Where(w => w.nEmployeeID == a.nEmployeeID).Select(s => new TablePermission
                                             {
                                                 nMenuID = s.nMenuID,
                                                 nPermission = s.nPermission
                                             }
                                             ).ToList()),
                                         }).ToList();

                #region Search
                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    qry = qry.Where(w => ((w.sName + "").Trim().ToLower().Contains((param.sSearch + "").Trim().ToLower()))
                    || ((w.sNameEN + "").Trim().ToLower().Contains((param.sSearch + "").Trim().ToLower()))
                    || ((w.sSurnameEN + "").Trim().ToLower().Contains((param.sSearch + "").Trim().ToLower()))).ToList();
                }
                string sStatus = (param.nStatus + "").Trim().ToLower();
                if (param.nStatus.HasValue)
                {
                    bool Active = param.nStatus == 1 ? true : false;
                    qry = qry.Where(w => w.IsActive == Active).ToList();
                }

                #endregion
                #endregion
                #region//SORT
                string sSortColumn = param.sSortExpression;
                switch (param.sSortExpression)
                {
                    case "sName": sSortColumn = "sName"; break;
                    case "sStatus": sSortColumn = "sStatus"; break;
                    case "sUpdateBy": sSortColumn = "sUpdateBy"; break;
                    case "dUpdate": sSortColumn = "dUpdate"; break;
                }
                if (param.isASC)
                {
                    qry = qry.OrderBy<ObjectUser>(sSortColumn).ToList();
                }
                else if (param.isDESC)
                {
                    qry = qry.OrderByDescending<ObjectUser>(sSortColumn).ToList();
                }
                #endregion
                var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qry.Count());
                var lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();

                var nIndex = param.nPageSize * (param.nPageIndex - 1) + 1;
                if (nIndex < 1) nIndex = 1;
                foreach (var item in lstData)
                {
                    item.nNo = nIndex;
                    nIndex++;
                }
                result.lstData = lstData;
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }
        public List<TablePermission> GetUserPermissionList(List<TablePermission>? lstAllPermission)
        {

            var lstPermission = _menuservice.GetMenuPermission();
            if (lstAllPermission != null && lstAllPermission.Count > 0)
            {
                foreach (var iPerm in lstPermission)
                {
                    var nPerm = lstAllPermission?.Where(w => w.nMenuID == iPerm.nMenuID).Select(s => s.nPermission).FirstOrDefault();
                    iPerm.nHighPerm = lstAllPermission?.Where(w => w.nMenuID == iPerm.nMenuID).Select(s => s.nHighPerm).FirstOrDefault();
                    if (nPerm == (int)EnumPermission.Permission.ReadOnly) //Read Only
                    {
                        iPerm.isDisable = false;
                        iPerm.isReadOnly = true;
                        iPerm.isEnable = false;
                    }
                    else if (nPerm == (int)EnumPermission.Permission.Full) //Enable
                    {
                        iPerm.isDisable = false;
                        iPerm.isReadOnly = false;
                        iPerm.isEnable = true;
                    }
                    else
                    {
                        iPerm.isDisable = true;
                        iPerm.isReadOnly = false;
                        iPerm.isEnable = false;
                    }
                }
            }
            return lstPermission;
        }
        public cReturnDataUserPerForm GetUserPerForm(UserPerModelRequest param)
        {
            cReturnDataUserPerForm result = new();
            try
            {
                List<TablePermission> lstAllPermission = new();
                int nUserID = param.sUserID.DecryptParameter().ToInt();
                IQueryable<TB_Employee>? lstEmployee = _db.TB_Employee.Where(w => !w.IsDelete).AsQueryable();
                IQueryable<TB_Employee_Position>? lstEmpPosition = _db.TB_Employee_Position.Where(w => !w.IsDelete).AsQueryable();
                IQueryable<TB_Position>? lstPosition = _db.TB_Position.Where(w => !w.IsDelete).AsQueryable();
                TB_Employee? objUser = lstEmployee.FirstOrDefault(w => w.nEmployeeID == nUserID);
                if (objUser != null)
                {
                    result.sUserType = objUser.nEmployeeTypeID.ToString();
                    result.objEmployeeID = new objEmployee { value = objUser.nEmployeeID.ToString(), label = objUser.nEmployeeID.ToString() + " - " + objUser.sNameTH + " " + objUser.sSurnameTH + " " + "(" + objUser.sNickname + ")" };
                    result.sName = objUser.sNameTH;
                    result.sSurename = objUser.sSurnameTH;
                    result.sEmail = objUser.sEmail;
                    result.sTelephone = objUser.sTelephone;
                    result.IsActive = objUser.IsActive;
                    var Position = (from ep in lstEmpPosition.Where(w => w.nEmployeeID == objUser.nEmployeeID)
                                    from p in lstPosition.Where(w => w.nPositionID == ep.nPositionID)
                                    select new { p, ep }).FirstOrDefault();
                    if (Position != null)
                    {
                        result.sPosition = Position.p.sPositionName;
                    }
                    int? GroupID = _db.TB_UserMappingGroup.FirstOrDefault(f => f.nEmployeeID == nUserID)?.nUserGroupID;
                    result.sUserGroup = GroupID.ToString();
                    List<TB_UserPermission>? lstUserPermisson = _db.TB_UserPermission.Where(w => w.nEmployeeID == nUserID).ToList();
                    if (lstUserPermisson.Any())
                    {
                        foreach (var item in _db.TB_UserGroupPermisson.Where(w => w.nUserGroupID == GroupID))
                        {
                            int nPerm = lstUserPermisson.Where(w => w.nMenuID == item.nMenuID).Select(s => s.nPermission).FirstOrDefault();
                            TablePermission tablePermission = new();
                            tablePermission.nMenuID = item.nMenuID;
                            tablePermission.nPermission = nPerm;
                            tablePermission.nHighPerm = item.nPermission;
                            lstAllPermission.Add(tablePermission);
                        }
                    }
                    else
                    {
                        TablePermission tablePermission = new();
                        tablePermission.isDisable = true;
                        lstAllPermission.Add(tablePermission);
                    }

                }
                result.lstPermission = GetUserPermissionList(lstAllPermission);

                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }
        public ResultAPI SaveDataUser(cResultDataUserPer param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                int nEmployeeID = param.sEmpID.DecryptParameter().ToInt();

                List<TB_UserMappingGroup>? objEmp = _db.TB_UserMappingGroup.Where(w => w.nEmployeeID == nEmployeeID).ToList();
                _db.TB_UserMappingGroup.RemoveRange(objEmp);
                _db.SaveChanges();

                TB_UserMappingGroup? objData = _db.TB_UserMappingGroup.FirstOrDefault(w => w.nEmployeeID == nEmployeeID);
                if (objData == null)
                {
                    objData = new TB_UserMappingGroup();
                    objData.nEmployeeID = nEmployeeID;
                    objData.nUserGroupID = param.nUserGroup != null ? param.nUserGroup.ToInt() : 0;
                    _db.TB_UserMappingGroup.Add(objData);
                }
                _db.SaveChanges();

                var lstRemove = _db.TB_UserPermission.Where(w => w.nEmployeeID == nEmployeeID).ToList();
                _db.TB_UserPermission.RemoveRange(lstRemove);
                _db.SaveChanges();
                if (param.lstPermission != null)
                {
                    foreach (var iPerm in param.lstPermission)
                    {
                        TB_UserPermission newPerm = new TB_UserPermission();
                        newPerm.nEmployeeID = nEmployeeID;
                        newPerm.nMenuID = iPerm.sID.DecryptParameter().ToInt();
                        var isEnable = param.lstPermission.Any(a => a.nMenuHeadID == newPerm.nMenuID && (a.isReadOnly || a.isEnable));
                        if (isEnable)
                        {
                            iPerm.isReadOnly = true;
                        }
                        if (iPerm.isDisable)
                        {
                            newPerm.nPermission = (int)EnumPermission.Permission.Disable;
                        }
                        if (iPerm.isReadOnly)
                        {
                            newPerm.nPermission = (int)EnumPermission.Permission.ReadOnly;
                        }
                        if (iPerm.isEnable)
                        {
                            newPerm.nPermission = (int)EnumPermission.Permission.Full;
                        }
                        _db.TB_UserPermission.Add(newPerm);
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
        public ResultAPI DeleteDataUser(cReqDeleteUser param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                UserAccount UserAccount = _Auth.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                List<int> lstId = param.lstDelete.ConvertAll(item => item.DecryptParameter().ToInt()).ToList();
                List<TB_Employee>? oUser = _db.TB_Employee.Where(w => lstId.Contains(w.nEmployeeID)).ToList();
                if (oUser != null)
                {
                    foreach (var item in oUser)
                    {
                        item.IsDelete = true;
                        item.nDeleteBy = nUserID;
                        item.dDelete = DateTime.Now;
                    }
                }
                _db.SaveChanges();
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }
        public ResultAPI UserToggleActive(ClassTableUser param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                int nEmployeeID = param.nEmployeeID.ToInt();
                var objData = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == nEmployeeID);
                if (objData != null)
                {
                    objData.IsActive = param.IsActive;
                }
                _db.SaveChanges();
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;

            }
            return result;
        }
        public cPermission GetPermissionUserGroup(UserModelRequest req)
        {
            cPermission result = new cPermission();
            try
            {
                var lstPermission = _menuservice.GetMenuPermission();
                if (!string.IsNullOrEmpty(req.sGroupID))
                {
                    IQueryable<TB_UserGroupPermisson>? lstUserGroupPermission = _db.TB_UserGroupPermisson.Where(w => w.nUserGroupID == req.sGroupID.ToInt()).AsQueryable();
                    foreach (var iPerm in lstPermission)
                    {
                        int nPerm = lstUserGroupPermission.Where(w => w.nMenuID == iPerm.nMenuID).Select(s => s.nPermission).FirstOrDefault();
                        iPerm.nHighPerm = nPerm;
                        if (nPerm == (int)EnumPermission.Permission.ReadOnly) //Read Only
                        {
                            iPerm.isDisable = false;
                            iPerm.isReadOnly = true;
                            iPerm.isEnable = false;
                        }
                        else if (nPerm == (int)EnumPermission.Permission.Full) //Enable
                        {
                            iPerm.isDisable = false;
                            iPerm.isReadOnly = false;
                            iPerm.isEnable = true;
                        }
                        else
                        {
                            iPerm.isDisable = true;
                            iPerm.isReadOnly = false;
                            iPerm.isEnable = false;
                        }
                    }
                }
                else
                {
                    List<TablePermission> lstAllPermission = new List<TablePermission>();
                    GetUserPermissionList(lstAllPermission);
                }
                result.lstPermission = lstPermission;
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
