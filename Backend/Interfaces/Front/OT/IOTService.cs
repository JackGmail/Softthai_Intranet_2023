using Backend.Models;
using ResultAPI = Extensions.Common.STResultAPI.ResultAPI;

namespace Backend.Interfaces
{
    public interface IOTService
    {
        cInitData LoadDataInit();
        cInitData GetProjectTask(cReqDataOT param);
        cInitData GetReason(cReqDataOT param);
        ResultAPI SaveData(cOTData param);
        ResultAPI SaveResult(cOTData param);
        cResultTableOT GetDataTable(cReqTableOT param);
        cResultTableOT GetInitDataTable();
        cOTData GetDataOT(cReqDataOT param);
        ResultAPI RemoveDataTable(cRemoveTable param);
        ResultAPI Reject(cReqDataOT param);
        Task<cExportExcelOT> ExportExcelOT(cReqTableOT param);
    }
}
