using Backend.Models;
using ResultAPI = Backend.Models.ResultAPI;

namespace Backend.Interfaces
{
    public interface IWorkScheduleService
    {
        cInitDataWorkSchedule GetInitData();
        cDataTable GetDataTable(cGetFilter objFilter);
        cInitDataWorkSchedule GetEmployeeData(cEmployeeData param);
        cDataTable GetDataTableEdit(cGetFilter objFilter);
        cDataTable SaveEmployeeData(cSaveEmployeeData objSaveData);
        cExport WorkShceduleExportExcel(cExportRequest objExport);
        cDataTable GetDataExportExcel(cGetDataForExcel objData);
    }
}