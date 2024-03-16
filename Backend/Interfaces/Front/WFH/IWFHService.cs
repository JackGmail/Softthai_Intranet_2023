using Backend.Models;
using Backend.Models.Back.Permission;

namespace Backend.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IWFHService
    {
        /// <summary>
        /// </summary>
        Models.ResultAPI GetActionFlowWorkFromHome(string? Key);
        Models.ResultAPI OptionApproverWFH(ParamProjectID Key);
        Models.ResultAPI UpdateFlowWorkFromHome(cUpdateFlow param);
        cResWFHFlow WFHWorkflow(cWFHFlow oWFH);
        string SendMailWFH(int nWFHID, int nFlowProcessID);
        #region ตี้
        clcResultTableWFH GetDataTable(clcFilterWFH oWFH);
        clcResultTableWFH GetOption();
        ResultAPI DeleteData(cReqDeleteUser param);
        #endregion
        cInitialDataWFH GetInitData();
        cReturnHistoryWFH GetHistoryWFH(string? sWFHID);
        cResultTaskPlanTable GetDataTableTaskPlan(cTaskTable param);
        cReturnWFHApprove GetFormWFH(string sWFHID);
        ResultAPI SaveData(cWFHSave param);


    }
}

