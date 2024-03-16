using Extensions.Common.STResultAPI;
using ST_API.Models.ITypeleaveService;

namespace ST_API.Interfaces
{
    public interface ITypeleaveService
    {
        Task<ResultTypeleave> GetData_MasterDetail();
        Task<ResultAPI> SaveLeaveType(TypeleaveData objSave);
        Task<cResultTableTypeLeave> GetTableLeaveType(cReqTableTypeleave param);
        Task<ResultTypeleave> GetDataEdit(cParam param);
        Task<ResultAPI> RemoveDataTable(cRemoveTableLeave param);
        Task<cResultTableTypeLeave> GetTableLeaveSummary(cReqTableLeaveSummary param);
        Task<ResultAPI> RemoveDataTableSummary(cRemoveTableLeave param);
        Task<cResultTableTypeLeave> GetDataTypeSummary(cParamSummary param);
        Task<ResultAPI> SaveLeaveSummary(cSaveSummary objSave);
        Task<cExportExcelSummary> ExportExcelSummary(cParamExport param);
        Task<ResultTypeleave> CheckAlreadyStart(ParamCheck param);
        Task<ResultTypeleave> CheckAlreadyEnd(ParamCheck param);
        Task<ResultTypeleave> ToggleActive(cTableTypeleave param);
        Task<ResultTypeleave> GetData_Dashboard(cParamSearch Param);
        Task<ResultAPI> SyncDataTypeleave(cSysTypeleave Param);
      
    }
}
