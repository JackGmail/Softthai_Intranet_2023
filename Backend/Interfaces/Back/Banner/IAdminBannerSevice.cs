
using ST_API.Models;
using Extensions.Common.STFunction;
using Backend.Models;

namespace Backend.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IAdminBannerSevice
    {
        /// <summary>
        /// GetDataTable 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        ClsResultTableBanner GetDataTable(ClsFilterBanner obj);
        /// <summary>
        /// </summary>
        ResultAPI OnSave(ObjSave bannerSave);
        /// <summary>
        /// </summary>
        ObjSave GetDataBanner(string sID);
        /// <summary>
        /// </summary>
        ClsResult OnDelete(ClsFilterDelete bannerFilter);
        /// <summary>
        /// </summary>
        ResultAPI OnChangeOrder(STFunction.ChgFilter bannerFilter);
        ResultAPI BannerActive(ClassTableGroup param);
    }
}
