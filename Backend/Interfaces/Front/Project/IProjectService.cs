using Backend.Models;
using Backend.Models.Front.Project;
using System.Diagnostics;

namespace Backend.Interfaces.Front.Project
{
    public interface IProjectService
    {
        cResultProject GetInitData();
        ResultAPI SaveData(cProject req);
        cProjectData GetDataInfo(int nContractPointID);
        ResultAPI GetData(string sID);
        List<EmployeeData> GetSearchEmployee(string strSearch);

        ResultAPI SaveDataProcess(cProcess req);
        cResultProject GetInitDataProcess();
        cProcess GetDataProcess(int nProcessID);
        //cResultProcessTable GetDataTable(cProcessTable param);
        ResultAPI ChangeOrder(cProcess param);
        cResultProjectTable GetDataTableProject(cProjectTable req);
        ResultAPI RemoveDataTable(cProjectTable req);
        #region sun
        ResultAPI GetDataProjectTOR(string? sProjectID);
        ResultAPI SaveProjectTOR(cProjectTOR req);
        //cReturnExportExcelTOR ExportExcelTOR(cExportExcelTOR param);
        //cReturnImportExcelTOR ImportExcelTOR(reqImportTOR req);
        #endregion
    }
}
