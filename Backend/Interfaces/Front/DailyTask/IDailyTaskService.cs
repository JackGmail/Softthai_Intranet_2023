using Backend.Models;
using Backend.Services.ISystemService;
using Microsoft.AspNetCore.Mvc;
using ResultAPI = Backend.Models.ResultAPI;

namespace Backend.Interfaces
{
    public interface IDailyTaskService
    {
        ResultAPI PageLoad(bool IsFilterUserData);
        ResultAPI GetTask(ParamTask param);
        ResultAPI SaveTask(ParamSaveTask param);
        ResultAPI AddTask(ParamAddTask param);
        ResultAPI RemoveTask(ParamRemoveTask param);

        ResultAPI GetTaskOverAll(ParamTaskOverAll param);
        ResultAPI GetTeamEmployee(ParamTeamEmployee param);

        ResultAPI GetTaskRule();

        #region TaskFormList
        ResultAPI GetTaskFormList(ParamSearchTask param);
        ResultAPI SaveTaskFormList(ParamSaveTaskFormList param);
        #endregion

        #region TaskPlanMultiDate
        ResultAPI SaveTaskPlanMultiDate(ParamSaveTaskMultiDate param);
        #endregion
        ExportExcel onExportTaskMonitorReport(ExportTaskMonitorReport param);
    }
}