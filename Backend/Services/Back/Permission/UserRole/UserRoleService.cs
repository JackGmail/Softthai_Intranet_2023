
using Backend.EF.ST_Intranet;
using Backend.Enum;
using Backend.Interfaces.Authentication;
using ST.INFRA;
using ST.INFRA.Common;
using Backend.Models;
using Backend.Interfaces.Back.Permission.Menu;
using Backend.Interfaces.Permission.UserRole;
using ST_API.Models.ITypeleaveService;

namespace Backend.Service
{
    //service
    public class UserRoleService : IUserRoleService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _Auth;
        private readonly IHostEnvironment _env;
        private readonly IMenuService _menuservice;
        /// <summary>
        /// </summary>
        public UserRoleService(ST_IntranetEntity db, IAuthentication auth, IHostEnvironment env, IMenuService menu)
        {

            _db = db;
            _Auth = auth;
            _env = env;
            _menuservice = menu;
        }
        /// <summary>
        /// </summary>
        #region ออร์

        public cResultUserRoleTable GetDataTable(cUserRoleTable param)
        {
            cResultUserRoleTable result = new cResultUserRoleTable();
            try
            {
                #region Query
                List<TB_UserRole> lstUserRole = _db.TB_UserRole.Where(w => !w.IsDelete).ToList();
                List<TB_UserRolePermission> lstRolePermissio = _db.TB_UserRolePermission.ToList();
                var lstUserGroup = _db.TB_UserGroup.Where(w => w.IsDelete != true).Select(s => s.nUserRoleID).Distinct().ToList();
                var qry = (from a in lstUserRole
                           select new ObjectResultUserRole
                           {
                               sID = a.nUserRoleID.EncryptParameter(),
                               nID = a.nUserRoleID,
                               sUserRoleName = a.sUserRoleName,
                               nUpdateBy = a.nUpdateBy,
                               dUpdate = a.dUpdate,
                               IsActive = a.IsActive,
                               //IsCanDelete = a.IsCanDelete,
                               IsCanDelete = (a.IsCanDelete ? !lstUserGroup.Any(f => f == a.nUserRoleID) : a.IsCanDelete),
                               nPermission = lstRolePermissio.Count(w => w.nUserRoleID == a.nUserRoleID && w.nPermission > 0),
                               lstPermission = GetPermissionList(lstRolePermissio.Where(w => w.nUserRoleID == a.nUserRoleID).Select(s => new TB_UserRolePermission
                               {
                                   nMenuID = s.nMenuID,
                                   nPermission = s.nPermission
                               }).ToList()),
                           }).ToList();

                #endregion

                #region Search
                if (!string.IsNullOrEmpty(param.sUserRoleName))
                {
                    qry = qry.Where(w => ((w.sUserRoleName + "").Trim().ToLower().Contains((param.sUserRoleName + "").Trim().ToLower())) || ((w.sUserRoleName + "").Trim().ToLower().Contains((param.sUserRoleName + "").Trim().ToLower()))).ToList();
                }
                string sStatus = (param.nStatus + "").Trim().ToLower();
                if (param.nStatus.HasValue)
                {
                    bool Active = param.nStatus == 1 ? true : false;
                    qry = qry.Where(w => w.IsActive == Active).ToList();
                }

                #endregion

                #region//SORT
                string sSortColumn = param.sSortExpression;
                switch (param.sSortExpression)
                {
                    case "sUserRoleName": sSortColumn = "sUserRoleName"; break;
                    case "sActive": sSortColumn = "sActive"; break;
                    case "sUpdateBy": sSortColumn = "sUpdateBy"; break;
                    case "dUpdate": sSortColumn = "dUpdate"; break;
                }
                if (param.isASC)
                {
                    qry = qry.OrderBy<ObjectResultUserRole>(sSortColumn).ToList();
                }
                else if (param.isDESC)
                {
                    qry = qry.OrderByDescending<ObjectResultUserRole>(sSortColumn).ToList();
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

        public ResultAPI SaveUserRole(UserRoleModel param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                var UserAccount = _Auth.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                var lstUserRole = _db.TB_UserRole.Where(w => w.IsDelete != true).ToList();
                int nUserRoleID = param.sRoleID.DecryptParameter().ToInt();
                var oUserRole = lstUserRole.FirstOrDefault(f => f.nUserRoleID == nUserRoleID);

                if (oUserRole == null)
                {
                    #region Check ซ้ำ
                    bool isDuplicate = lstUserRole.Any(w => w.nUserRoleID != nUserID && w.sUserRoleName.Trim().ToLower() == (param.sUserRoleName + "").Trim().ToLower());
                    if (isDuplicate)
                    {
                        result.nStatusCode = StatusCodes.Status409Conflict;
                        result.sMessage = "มี Role นี้อยู่ในระบบแล้ว";
                        return result;
                    }
                    #endregion

                    oUserRole = new TB_UserRole();
                    nUserRoleID = (_db.TB_UserRole.Any() ? _db.TB_UserRole.Max(m => m.nUserRoleID) : 0) + 1;
                    oUserRole.nUserRoleID = nUserRoleID;
                    oUserRole.IsDelete = false;
                    oUserRole.dCreate = DateTime.Now;
                    oUserRole.nCreateBy = nUserID;
                    oUserRole.IsCanDelete = true;
                    oUserRole.IsActive = true;
                    _db.TB_UserRole.Add(oUserRole);
                }
                oUserRole.sUserRoleName = param.sUserRoleName ?? "";
                oUserRole.sUserRoleDescription = param.sDrescription;
                oUserRole.dUpdate = DateTime.Now;
                oUserRole.nUpdateBy = nUserID;
                oUserRole.IsDelete = false;

                _db.SaveChanges();

                var lstRemove = _db.TB_UserRolePermission.Where(w => w.nUserRoleID == nUserRoleID).ToList();
                _db.TB_UserRolePermission.RemoveRange(lstRemove);
                _db.SaveChanges();
                if (param.lstPermission != null)
                {
                    foreach (var iPerm in param.lstPermission)
                    {
                        TB_UserRolePermission newPerm = new TB_UserRolePermission();
                        newPerm.nUserRoleID = nUserRoleID;
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
                        _db.TB_UserRolePermission.Add(newPerm);
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

        public UserRoleModel GetUserRole(UserRoleModelRequest param)
        {
            UserRoleModel result = new();
            try
            {
                List<TB_UserRolePermission> lstAllPermission = new List<TB_UserRolePermission>();
                int nUserRoleID = param.sRoleID.DecryptParameter().ToInt();
                TB_UserRole? oUserRole = _db.TB_UserRole.Where(w => w.nUserRoleID == nUserRoleID).FirstOrDefault();
                if (oUserRole != null)
                {
                    result.sRoleID = oUserRole.nUserRoleID.ToString();
                    result.sUserRoleName = oUserRole.sUserRoleName;
                    result.sDrescription = oUserRole.sUserRoleDescription;
                    result.IsActive = oUserRole.IsActive;
                    lstAllPermission = _db.TB_UserRolePermission.Where(w => w.nUserRoleID == nUserRoleID).ToList();
                }
                else
                {
                    TB_UserRolePermission tablePermission = new();
                    lstAllPermission.Add(tablePermission);
                }
                result.lstPermission = GetPermissionList(lstAllPermission);

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }

        public List<TablePermission> GetPermissionList(List<TB_UserRolePermission>? lstAllPermission)
        {

            var lstPermission = _menuservice.GetMenuPermission();
            if (lstAllPermission != null && lstAllPermission.Count > 0)
            {
                foreach (var iPerm in lstPermission)
                {
                    var nPerm = lstAllPermission.Where(w => w.nMenuID == iPerm.nMenuID).Select(s => s.nPermission).FirstOrDefault();

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
                    if (iPerm.isSubMenu)
                    {
                        int nCheckPermission = lstAllPermission.FirstOrDefault(w => w.nMenuID == iPerm.nMenuHeadID)?.nPermission ?? 0;
                        iPerm.nDisabledMode = (nCheckPermission == (int)EnumPermission.Permission.Disable ? (int)EnumPermission.Permission.Disable : (nCheckPermission == (int)EnumPermission.Permission.ReadOnly ? (int)EnumPermission.Permission.ReadOnly : (int)EnumPermission.Permission.Full)); //Set Sub Mode Disabled Radio
                    }
                }
            }
            return lstPermission;
        }

        public ResultAPI RemoveDataTable(cUserGroupTable param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                var UserAccount = _Auth.GetUserAccount();
                // TokenJWTSecret UserAccount = _auth.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                List<int> lstId = param.lstRemove.ConvertAll(item => item.DecryptParameter().ToInt()).ToList();
                var oUserRole = _db.TB_UserRole.Where(w => lstId.Contains(w.nUserRoleID)).ToList();
                if (oUserRole != null)
                {
                    foreach (var item in oUserRole)
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

        public ResultAPI UserRoleToggleActive(ClassTableUserRole param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                var objData = _db.TB_UserRole.FirstOrDefault(w => w.nUserRoleID == param.nUserRoleID);
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
        #endregion
    }


}

