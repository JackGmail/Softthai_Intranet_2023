

using Backend.Models;
using ResultAPI = Extensions.Common.STResultAPI.ResultAPI;

namespace Backend.Interfaces
{
    public interface ITravelService
    {
        ResultAPI SaveData(cTravelData param);
        cTravelData GetDataTravel(cReqDataAllowance param);
        ResultAPI SaveApprove(cReqApprove param);
    }
}
