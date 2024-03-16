using Backend.Models;
using ST_API.Models;
using ResultAPI = Extensions.Common.STResultAPI.ResultAPI;

namespace ST_API.Interfaces
{
    public interface ILeaveService
    {
        Task<cFormInitRequest> GetFormRequest(string? sID);
        Task<cFormInitRequest> GetFilterLeave();
        Task<cLeaveLogsRequest> GetLog(string sID);
        Task<ResultAPI> SaveData(cFormReqRequest req);
        Task<ResultAPI> ApproveOnTable(cFormReqRequest req);
        cResultTableLeaveRequest GetMasterData();
        cReturnImportExcelRequest LeaveImportExcel(cLeaveImportExcelRequest req);
        cExportExcelRequest LeaveExportExcel(cLeaveExportRequest req);
        cResultTableLeaveRequest EditLeaveSummary(cEditLeaveRequest req);
        Task<cTabelWorkListRequest> GetDataWorkList(cReqTableWorkListRequest req);
        ResultAPI RemoveDataTable(cRemoveTable param);

    }
}