using Backend.Models;
using Extensions.Common.STResultAPI;
using ST_API.Models;
using ST_API.Models.IHomeService;

namespace ST_API.Interfaces
{
    public interface IHomeService
    {
        cResultDataHome GetDataHomePage();
         /// <summary>
        /// </summary>
        ResultBanner GetBanner();
        Backend.Models.ResultAPI PageLoad();
        Backend.Models.ResultAPI GetTableTaskViewManager(ParamViewManager param);
        Backend.Models.ResultAPI GetTableOTViewManager(ParamViewManager param);
        Backend.Models.ResultAPI GetTableWFHViewManager(ParamViewManager param);
        Backend.Models.ResultAPI GetTableLeaveViewManager(ParamViewManager param);
        Backend.Models.ResultAPI GetInitData();
    }
}