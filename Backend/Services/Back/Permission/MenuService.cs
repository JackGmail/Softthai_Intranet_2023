
using Backend.EF.ST_Intranet;
using Backend.Enum;
using Backend.Interfaces.Authentication;
using Backend.Interfaces.Back.Permission.Menu;
using Backend.Models;
using Backend.Models.Back.Permission;
using ST.INFRA;


namespace Backend.Services
{

    //service
    public class MenuService : IMenuService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _Auth;
        private readonly IHostEnvironment _env;
        /// <summary>
        /// </summary>
        public MenuService(ST_IntranetEntity db, IAuthentication auth, IHostEnvironment env)
        {
            _db = db;
            _Auth = auth;
            _env = env;
        }
        /// <summary>
        /// </summary>
        #region ออร์

        public List<MenuAllModel> GetMenu()
        {
            {
                var lstMenu = (from m in _db.TM_Menu.Where(w => w.IsActive && w.IsSetPermission && w.nLevel != (int)EnumPermission.MenuDisplay.nLevel3)
                               select new MenuModel
                               {
                                   sID = m.nMenuID.EncryptParameter(),
                                   nMenuID = m.nMenuID,
                                   nParentID = m.nParentID,
                                   nMenuType = m.nMenuType,
                                   sMenuName = m.sMenuName,
                                   nLevel = m.nLevel,
                                   sRoute = m.sRoute,
                                   sIcon = m.sIcon,
                                   IsDisplay = m.IsDisplay,
                                   IsSetPermission = m.IsSetPermission,
                                   IsDisable = m.IsDisable,
                                   IsView = m.IsView,
                                   IsManage = m.IsManage,
                                   IsShowBreadcrumb = m.IsShowBreadcrumb,
                                   IsActive = m.IsActive,
                                   nOrder = m.nOrder,
                               }
                ).ToList();

                var lstData = lstMenu.Where(w => w.nParentID == null).Select(s => new MenuAllModel
                {
                    sID = s.sID,
                    nMenuID = s.nMenuID,
                    nParentID = s.nParentID,
                    nMenuType = s.nMenuType,
                    sMenuName = s.sMenuName,
                    nLevel = s.nLevel,
                    sRoute = s.sRoute,
                    sIcon = s.sIcon,
                    IsDisplay = s.IsDisplay,
                    IsSetPermission = s.IsSetPermission,
                    IsDisable = s.IsDisable,
                    IsView = s.IsView,
                    IsManage = s.IsManage,
                    IsShowBreadcrumb = s.IsShowBreadcrumb,
                    IsActive = s.IsActive,
                    nOrder = s.nOrder,
                    lstSubMenu = GetSubMenu(lstMenu, s.nMenuID)
                }).ToList();

                return lstData;
            }
        }
        public List<MenuAllModel> GetSubMenu(List<MenuModel> lstMenu, int nParentID)
        {
            var lstData = lstMenu.Where(w => w.nParentID == nParentID).Select(s => new MenuAllModel
            {
                sID = s.sID,
                nMenuID = s.nMenuID,
                nParentID = s.nParentID,
                nMenuType = s.nMenuType,
                sMenuName = s.sMenuName,
                nLevel = s.nLevel,
                sRoute = s.sRoute,
                sIcon = s.sIcon,
                IsDisplay = s.IsDisplay,
                IsSetPermission = s.IsSetPermission,
                IsDisable = s.IsDisable,
                IsView = s.IsView,
                IsManage = s.IsManage,
                IsShowBreadcrumb = s.IsShowBreadcrumb,
                IsActive = s.IsActive,
                nOrder = s.nOrder,
                lstSubMenu = GetSubMenu(lstMenu, s.nMenuID)
            }).ToList();
            return lstData;
        }
        public List<TablePermission> GetMenuPermission()
        {
            List<TablePermission> lstPermission = new List<TablePermission>();
            var lstMenu = GetMenu();
            var nOrder = 0;
            var nCount = 0;
            foreach (var iMenu in lstMenu)
            {
                TablePermission iPerm = new TablePermission();
                if (iMenu.nParentID == null)
                {
                    iPerm.nOrder = nOrder + 1;
                    nOrder++;
                    ++nCount;
                }
                iPerm.nNo = nCount;
                iPerm.sID = iMenu.nMenuID.EncryptParameter();
                iPerm.nMenuID = iMenu.nMenuID;
                iPerm.sMenuName = iMenu.sMenuName;
                iPerm.isFontEnd = iMenu.isFontEnd;
                iPerm.isManagePRM = iMenu.IsManage;
                iPerm.isDisable = false;
                iPerm.isReadOnly = false;
                iPerm.isEnable = false;
                iPerm.nHighPerm = (int)EnumPermission.Permission.Full;
                iPerm.nMenuHeadID = iMenu.nParentID;
                iPerm.isHasSub = iMenu.lstSubMenu != null && iMenu.lstSubMenu.Any(w => w.nParentID == iMenu.nMenuID);
                iPerm.isSubMenu = iMenu.nParentID != null;
                iPerm.nDisabledMode = null;
                if (lstMenu.Any(w => w.nParentID == iMenu.nMenuID))
                {
                    nOrder = nOrder + lstMenu.Count(w => w.nParentID == iMenu.nMenuID);
                }
                lstPermission.Add(iPerm);

                var nNewOrder = 1;
                if (iMenu.lstSubMenu != null)
                {
                    foreach (var menu in iMenu.lstSubMenu)
                    {
                        TablePermission iPermSub = new TablePermission();
                        iPermSub.sID = menu.nMenuID.EncryptParameter();
                        iPermSub.nMenuID = menu.nMenuID;
                        iPermSub.sMenuName = menu.sMenuName;
                        iPermSub.isFontEnd = menu.isFontEnd;
                        iPermSub.isManagePRM = menu.IsSetPRMS;
                        iPermSub.isDisable = false;
                        iPermSub.isReadOnly = false;
                        iPermSub.isEnable = false;
                        iPermSub.nMenuHeadID = menu.nParentID;
                        iPermSub.nHighPerm = (int)EnumPermission.Permission.Full;
                        iPermSub.isHasSub = menu.lstSubMenu != null && menu.lstSubMenu.Any(w => w.nParentID == iMenu.nMenuID);
                        iPermSub.isSubMenu = menu.nParentID != null;
                        iPermSub.nOrder = lstPermission.Where(w => w.nMenuID == menu.nParentID).Select(s => s.nOrder).FirstOrDefault() + nNewOrder;
                        iPerm.nDisabledMode = null;
                        nNewOrder++;
                        lstPermission.Add(iPermSub);
                    }
                }
            }
            return lstPermission;
        }
        #endregion
    }
}