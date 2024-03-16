using Backend.Interfaces;
using Backend.Models;
using ST.INFRA;
using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Backend.Models.Authentication;

namespace Backend.Services
{
    public class NavigationService : INavigation
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _authen;
        public NavigationService(ST_IntranetEntity db, IAuthentication authen)
        {
            _db = db;
            _authen = authen;
        }

        /// <summary>
        /// Get Bredcrumb
        /// </summary>
        /// <param name="sRoute"></param>
        /// <param name="sLanguages"></param>
        /// <returns></returns>
        public ResultAPI GetBreadCrumb(string sRoute, string sLanguages)
        {
            ResultAPI result = new ResultAPI();
            Breadcrumbs objBreadcrumbs = new Breadcrumbs();
            List<ItemBreadcrumbs> lstBreadcrumbs = new List<ItemBreadcrumbs>();
            string[] arrRoute = sRoute.ToStringToLower().Split("/");
            string sMenuRoute = "/" + arrRoute.Last();
            string sLang = sLanguages.ToStringToLower();
            var objMenu = _db.TM_Menu.FirstOrDefault(w => w.sRoute.ToLower() == sMenuRoute && w.IsActive && w.IsShowBreadcrumb);
            if (objMenu != null)
            {
                lstBreadcrumbs.Add(new ItemBreadcrumbs
                {
                    nLevel = objMenu.nLevel,
                    nMenuID = objMenu.nMenuID,
                    sMenuName = objMenu.sMenuName,
                    sIcon = objMenu.sIcon,
                    sRoute = objMenu.sRoute
                });
                GetParentBreadCrumb(objMenu, lstBreadcrumbs, sLang);
            }
            objBreadcrumbs.lstBreadcrumbs = lstBreadcrumbs.OrderBy(w => w.nLevel).ToList();
            result.objResult = objBreadcrumbs;
            return result;
        }

        /// <summary>
        /// Get HeadMenu Recursive
        /// </summary>
        /// <param name="objItem"></param>
        /// <param name="lstBreadCrumb"></param>
        /// <param name="sLang"></param>
        /// <returns></returns>
        private void GetParentBreadCrumb(TM_Menu objItem, List<ItemBreadcrumbs> lstBreadCrumb, string sLang)
        {
            var objMenu = _db.TM_Menu.FirstOrDefault(w => w.nMenuID == objItem.nParentID && w.IsActive && w.IsShowBreadcrumb);
            if (objMenu != null)
            {
                lstBreadCrumb.Add(new ItemBreadcrumbs
                {
                    nLevel = objMenu.nLevel,
                    nMenuID = objMenu.nMenuID,
                    sMenuName = objMenu.sMenuName,
                    sIcon = objMenu.sIcon,
                    sRoute = objMenu.sRoute
                });
                GetParentBreadCrumb(objMenu, lstBreadCrumb, sLang);
            }
        }

        /// <summary>
        /// Get Menu
        /// </summary>
        /// <returns></returns>
        public ResultAPI GetMenu(int nMenuType)
        {
            ResultAPI result = new ResultAPI();
            Menu objMenu = new Menu();
            List<ItemMenu> lstMenu = new List<ItemMenu>();
            UserAccount ua = _authen.GetUserAccount();
            var lstPermission = ua.lstMenuPrms.Where(w => w.nPermission == 1 || w.nPermission == 2).Select(w => w.nMenuID).ToArray();
            var lstChildrenMenu = _db.TM_Menu.Where(w => w.nParentID.HasValue && w.IsActive && w.IsDisplay && lstPermission.Contains(w.nMenuID)).ToList();
            var lstMenuHead = _db.TM_Menu.Where(w => w.IsActive && w.IsDisplay && w.nLevel == 1 && w.nMenuType == nMenuType && lstPermission.Contains(w.nMenuID)).OrderBy(w=> w.nOrder);
            foreach (var item in lstMenuHead)
            {
                lstMenu.Add(new ItemMenu
                {
                    sKey = item.nMenuID + "",
                    nMenuID = item.nMenuID,
                    nParentID = item.nParentID,
                    sMenuName = item.sMenuName,
                    nLevel = item.nLevel,
                    sRoute = item.sRoute,
                    sIcon = item.sIcon,
                    lstChildrenMenu = GetParentMenu(item, nMenuType, lstChildrenMenu)
                });
            }
            objMenu.lstMenu = lstMenu.ToList();
            result.objResult = objMenu;
            return result;
        }
        private List<ItemMenu> GetParentMenu(TM_Menu objItem, int nMenuType, List<TM_Menu> lstMenuPermission)
        {
            List<ItemMenu> lstChildrenMenu = new List<ItemMenu>();
            var lstMenu = lstMenuPermission.Where(w => w.nParentID == objItem.nMenuID);
            foreach (var item in lstMenu)
            {
                lstChildrenMenu.Add(new ItemMenu
                {
                    sKey = item.nMenuID + "",
                    nMenuID = item.nMenuID,
                    nParentID = item.nParentID,
                    sMenuName = item.sMenuName,
                    nLevel = item.nLevel,
                    sRoute = item.sRoute,
                    sIcon = item.sIcon,
                    lstChildrenMenu = GetParentMenu(item, nMenuType, lstMenuPermission)
                });
            }
            return lstChildrenMenu;
        }


    }
}