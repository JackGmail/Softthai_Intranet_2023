using Backend.Models;
using ResultAPI = Backend.Models.ResultAPI;

namespace Backend.Interfaces
{
    public interface IEmployeeService
    {
        cResultEmployee GetInitData();

        #region ซัน
        ResultAPI GetInitEmployeeList();
        cReturnEmployee GetDataEmployee(cReqEmployee param);
        cEmployeeForm GetDataEmployeeForm(string? sEmployeeID);
        ResultAPI SaveDataEmployee(cEmployeeForm param);
        ResultAPI GetDataFamilyForm(string? sEmployeeID);
        ResultAPI SaveDataFamily(cFamily req);
        ResultAPI GetDataLanguageForm(string? sEmployeeID);
        ResultAPI SaveDataLanguage(cLanguage req);
        ResultAPI GetDataPositionForm(string? sEmployeeID);
        ResultAPI SaveDataPosition(cPosition param);
        #endregion

        #region ตี๋
        clsProfileEmp GetProfile(string? sID);
        cReturnExport SpireDoc_ExportWord(objParam obj);
        #endregion

        #region ออร์
        ResultAPI SaveDataEducation(cEducation req);
        ResultAPI SaveDataWorkExperien(cWorkExperien req);
      
  
        cEducation GetDataEducation(cEducation req);
        ResultAPI SaveDataOtherParts(cOtherParts req);
        cWorkExperien GetDataWorkExperien(cWorkExperien req);
        cOtherParts GetDataOtherParts(cOtherParts req);
        ResultAPI SaveDataSpecialAbility (cSpecialAbility req);
        cSpecialAbility GetDataSpecialAbility(cSpecialAbility req);
        #endregion
        CEmployeeID GetEmployeeID();
        CMenueEmployee GetMenueEmployee();
    }
}

