using Backend.Models;
using ResultAPI = Extensions.Common.STResultAPI.ResultAPI;

namespace Backend.Interfaces
{
    public interface IAllowanceService
    {
        cInitAllowanceData LoadDataInit();
        ResultAPI SaveData(cAllowanceData param);
        cResultTableWelfare GetInitDataTable();
        cResultTableWelfare GetDataTable(cReqTableWelfare param);
        cAllowanceData GetDataAllowance(cReqDataAllowance param);
        ResultAPI RemoveDataTable(cRemoveTable param);
        ResultAPI SaveApprove(cReqApprove param);
    }
}
