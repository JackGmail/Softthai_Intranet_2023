using Backend.Models;
using Backend.Models.Back.Permission;

namespace Backend.Interfaces.Back.Permission.Menu
{
    /// <summary>
    /// </summary>
    public interface IMenuService
    {
        #region 
        List<TablePermission> GetMenuPermission();
        List<MenuAllModel> GetMenu();
        List<MenuAllModel> GetSubMenu(List<MenuModel> lstMenu, int nParentID);
        #endregion
    }

}