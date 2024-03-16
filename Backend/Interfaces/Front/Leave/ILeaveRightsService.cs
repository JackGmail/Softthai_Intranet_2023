using Extensions.Common.STResultAPI;
using ST_API.Models;

namespace ST_API.Interfaces
{
    public interface ILeaveRightsService
    {
        Task<cLeaveInitData> SumLeaveCard();
        cResultTableLeave GetDataTableLeave(cLeaveTable req);
        cResultTableLeave GetDataTableLate(cLeaveTable req);
        cResultTableLeave GetDataTableWFH(cLeaveTable req);
        cResultTableLeave GetDataTableWorkCert(cLeaveTable req);
        cResultTableLeave GetDataTableHoliday(cLeaveTable req);
        Task<cFormInit> GetFilterLeave();
        Task<ResultAPI> ApproveOnTable(cFormReq req);
        cResultTableLeave GetLeaveSummary(cLeaveRights req);
        cResultTableLeave GetMasterData();
        cExportExcel LeaveExportExcel(cLeaveExport req);
        cResultTableLeave EditLeaveSummary(cEditLeave req);
        Task<cTabelWorkList> GetDataWorkList(cReqTableWorkList req);
        // cReturnImportExcelLeave ImportExcelTOR(reqImport req);
        ResultAPI ImportExcel(cImport objSaveData);
    }
}