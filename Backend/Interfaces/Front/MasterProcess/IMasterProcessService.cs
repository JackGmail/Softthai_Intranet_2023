using ST_API.Models;
using ResultAPI = Extensions.Common.STResultAPI.ResultAPI;

namespace ST_API.Interfaces
{
    public interface IMasterProcessService
    {

        ResultAPI SaveProcessData(cReqMasterProcessData req);
        cFilterMainProcess LoadMainProcessOptions();
          cFilterMasterProcessTable LoadMasterProcessData(cMasterProcessData req);
        cFilterMasterProcessTable LoadSubProcessData(cMasterProcessData req);
        ResultAPI ChangeOrder(cMasterProcessOrder req);
        cMasterProcessValue GetDataToEdit(cReqMasterProcessDataValue req);
         Task<ResultAPI> RemoveProcessData(cRemoveTableMaster req);



    }


}