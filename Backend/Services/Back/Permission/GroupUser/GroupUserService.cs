
using Backend.EF.ST_Intranet;
using Backend.Enum;
using Backend.Interfaces.Authentication;
using ST.INFRA;
using ST.INFRA.Common;
using Backend.Interfaces;
using Backend.Interfaces.Permission.UserRole;
using Backend.Models;
using Backend.Interfaces.Back.Permission.Menu;
using Backend.Interfaces.Permission.GroupUser;

namespace Backend.Services.Back.Permission.GroupUser
{
    //service
    public class GroupUserService : IGroupUserService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _Auth;
        private readonly IHostEnvironment _env;
        private readonly IMenuService _menuservice;
        /// <summary>
        /// </summary>
        public GroupUserService(ST_IntranetEntity db, IAuthentication auth, IHostEnvironment env, IMenuService menu)
        {

            _db = db;
            _Auth = auth;
            _env = env;
            _menuservice = menu;
        }
        /// <summary>
        /// </summary>

        public cResultUserGroup GetInitData()
        {
            cResultUserGroup result = new cResultUserGroup();
            try
            {
                result.lstRoles = _db.TB_UserRole.Where(w => w.IsActive == true && w.IsDelete != true).Select(s => new cSelectOption
                {
                    value = s.nUserRoleID.EncryptParameter(),
                    label = s.sUserRoleName,
                }).ToList();

                result.lstRolesSH = _db.TB_UserRole.Where(w => w.IsActive == true && w.IsDelete != true).Select(s => new cSelectOption
                {
                    value = s.nUserRoleID.ToString(),
                    label = s.sUserRoleName,
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
        #region ออร์
        public cResultUserGroupTable GetDataTable(cUserGroupTable param)
        {
            cResultUserGroupTable result = new cResultUserGroupTable();
            try
            {
                #region Query
                List<TB_UserGroup> lstUserGroup = _db.TB_UserGroup.Where(w => !w.IsDelete).ToList();
                List<TB_UserGroupPermisson> lstGroupPermission = _db.TB_UserGroupPermisson.ToList();
                List<int>? lstUserGroupID = _db.TB_UserMappingGroup.Select(s => s.nUserGroupID).Distinct().ToList();

                var qry = (from a in lstUserGroup
                           from b in _db.TB_UserRole.Where(w => w.nUserRoleID == a.nUserRoleID)
                           select new ObjectResultUserGroup
                           {
                               sID = a.nUserGroupID.EncryptParameter(),
                               nID = a.nUserGroupID,
                               nRoleID = a.nUserRoleID,
                               sRole = b.sUserRoleName,
                               sGroup = a.sUserGroupName,
                               nUpdateBy = a.nUpdateBy,
                               dUpdate = a.dUpdate,
                               IsStatus = a.IsActive,
                               //IsCanDelete = a.IsCanDelete,
                               IsCanDelete = (a.IsCanDelete ? !lstUserGroupID.Any(f => f == a.nUserGroupID) : a.IsCanDelete),
                               nPermission = lstGroupPermission.Count(w => w.nUserGroupID == a.nUserGroupID && w.nPermission > 0),
                               lstPermission = GetPermissionList(lstGroupPermission.Where(w => w.nUserGroupID == a.nUserGroupID).Select(s => new TablePermission
                               {
                                   nMenuID = s.nMenuID,
                                   nPermission = s.nPermission
                               }).ToList()),
                           }).ToList();
                #endregion

                #region Search
                if (!string.IsNullOrEmpty(param.sGroup))
                {
                    qry = qry.Where(w => (w.sGroup + "").Trim().ToLower().Contains((param.sGroup + "").Trim().ToLower()) || (w.sGroup + "").Trim().ToLower().Contains((param.sGroup + "").Trim().ToLower())).ToList();
                }
                if (param.nStatus.HasValue)
                {
                    bool Active = param.nStatus == 1 ? true : false;
                    qry = qry.Where(w => w.IsStatus == Active).ToList();
                }
                if (param.nRoleID.HasValue)
                {
                    qry = qry.Where(w => w.nRoleID == param.nRoleID.Value).ToList();
                }
                #endregion

                #region//SORT
                string sSortColumn = param.sSortExpression;
                switch (param.sSortExpression)
                {
                    case "sUserRoleName": sSortColumn = "sUserRoleName"; break;
                    case "sStatus": sSortColumn = "sStatus"; break;
                    case "sUpdateBy": sSortColumn = "sUpdateBy"; break;
                    case "dUpdate": sSortColumn = "dUpdate"; break;
                }
                if (param.isASC)
                {
                    qry = qry.OrderBy(sSortColumn).ToList();
                }
                else if (param.isDESC)
                {
                    qry = qry.OrderByDescending(sSortColumn).ToList();
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
        /// <summary>
        /// </summary>
        public ResultAPI SaveUserGroup(UserGroupModel param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                var UserAccount = _Auth.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                IQueryable<TB_UserGroup>? lstUserGroup = _db.TB_UserGroup.Where(w => !w.IsDelete).AsQueryable();
                int nUserGroupID = param.sUserGroupID.DecryptParameter().ToInt();
                TB_UserGroup? oUserGroup = lstUserGroup.FirstOrDefault(w => w.nUserGroupID == nUserGroupID);
                if (oUserGroup == null)
                {
                    #region Check ซ้ำ
                    bool isDuplicate = lstUserGroup.Any(w => w.nUserGroupID != nUserGroupID && w.sUserGroupName.Trim().ToLower() == (param.sGroup + "").Trim().ToLower());
                    if (isDuplicate)
                    {
                        result.nStatusCode = StatusCodes.Status409Conflict;
                        result.sMessage = "มี Group นี้ในระบบแล้ว";
                        return result;
                    }
                    #endregion
                    oUserGroup = new TB_UserGroup();
                    nUserGroupID = (_db.TB_UserGroup.Any() ? _db.TB_UserGroup.Max(m => m.nUserGroupID) : 0) + 1;
                    oUserGroup.nUserGroupID = nUserGroupID;
                    oUserGroup.IsDelete = false;
                    oUserGroup.dCreate = DateTime.Now;
                    oUserGroup.nCreateBy = nUserID;
                    oUserGroup.IsCanDelete = true;
                    oUserGroup.IsActive = true;
                    _db.TB_UserGroup.Add(oUserGroup);
                }
                oUserGroup.nUserRoleID = param.sRoleID.DecryptParameter().ToInt();
                oUserGroup.sUserGroupName = param.sGroup != null ? param.sGroup.Trim() : "";
                oUserGroup.dUpdate = DateTime.Now;
                oUserGroup.nUpdateBy = nUserID;
                _db.SaveChanges();

                IQueryable<TB_UserGroupPermisson>? lstRemove = _db.TB_UserGroupPermisson.Where(w => w.nUserGroupID == nUserGroupID).AsQueryable();
                _db.TB_UserGroupPermisson.RemoveRange(lstRemove);
                _db.SaveChanges();

                if (param.lstPermission != null)
                {
                    foreach (var iPerm in param.lstPermission)
                    {
                        TB_UserGroupPermisson newPerm = new TB_UserGroupPermisson();
                        newPerm.nUserGroupID = nUserGroupID;
                        newPerm.nMenuID = iPerm.nMenuID;
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
                        _db.TB_UserGroupPermisson.Add(newPerm);
                    }
                    _db.SaveChanges();

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
        /// </summary>
        public ResultAPI RemoveDataGroupTable(cUserGroupTable param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                var UserAccount = _Auth.GetUserAccount();
                // TokenJWTSecret UserAccount = _auth.GetUserAccount();
                int nUserID = UserAccount.nUserID;

                List<int> lstId = param.lstRemove.ConvertAll(item => item.DecryptParameter().ToInt()).ToList();
                var oUserRole = _db.TB_UserGroup.Where(w => lstId.Contains(w.nUserGroupID)).ToList();
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
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }

        /// <summary>
        /// </summary>
        public UserGroupModel GetUserGroup(UserGroupModelRequest param)
        {
            UserGroupModel result = new();
            try
            {
                List<TablePermission> lstAllPermission = new();
                int nUserGroupID = param.sUserGroupID.DecryptParameter().ToInt();
                TB_UserGroup? oUserGroup = _db.TB_UserGroup.Where(w => w.nUserGroupID == nUserGroupID).FirstOrDefault();
                if (oUserGroup != null)
                {
                    result.sRoleID = oUserGroup.nUserRoleID.EncryptParameter();
                    result.sUserGroupID = oUserGroup.nUserGroupID.ToString();
                    result.sGroup = oUserGroup.sUserGroupName;
                    result.sDescription = oUserGroup.sUserGroupDescription;
                    result.IsActive = oUserGroup.IsActive;
                    foreach (var item in _db.TB_UserRolePermission.Where(w => w.nUserRoleID == oUserGroup.nUserRoleID))
                    {
                        int nPerm = _db.TB_UserGroupPermisson.Where(w => w.nMenuID == item.nMenuID).Select(s => s.nPermission).FirstOrDefault();
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

        /// <summary>
        /// </summary>
        public List<TablePermission> GetPermissionList(List<TablePermission>? lstAllPermission)
        {
            var lstPermission = _menuservice.GetMenuPermission();
            if (lstPermission != null && lstPermission.Count > 0)
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
        /// <summary>
        /// </summary>
        public cPermission GetPermissionUserRole(UserRoleModelRequest param)
        {
            cPermission result = new cPermission();
            try
            {
                var lstPermission = _menuservice.GetMenuPermission();
                if (!string.IsNullOrEmpty(param.sRoleID))
                {
                    IQueryable<TB_UserRolePermission>? lstUserRolePermission = _db.TB_UserRolePermission.Where(w => w.nUserRoleID == param.sRoleID.DecryptParameter().ToInt()).AsQueryable();
                    foreach (var iPerm in lstPermission)
                    {
                        int nPerm = lstUserRolePermission.Where(w => w.nMenuID == iPerm.nMenuID).Select(s => s.nPermission).FirstOrDefault();
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
                    GetPermissionList(lstAllPermission);
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

        public ResultAPI GroupToggleActive(ClassTableGroup param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                var objData = _db.TB_UserGroup.FirstOrDefault(w => w.nUserGroupID == param.nUserGroupID);
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

