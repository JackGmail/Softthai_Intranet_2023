using Backend.Models;
using Backend.Models.Back.Permission;

namespace Backend.Interfaces.Back.Permission
{
    public interface IUserPermission
    {
        cResultOption GetOptionInitialData();
        List<cOptionAutoComplete> GetEmployee(string? strSearch);
        cResultUserTable GetDataTable(cReqUserTable param);
        cReturnDataUserPerForm GetUserPerForm(UserPerModelRequest param);
        ResultAPI SaveDataUser(cResultDataUserPer param);
        ResultAPI DeleteDataUser(cReqDeleteUser param);
        ResultAPI UserToggleActive(ClassTableUser param);
        cPermission GetPermissionUserGroup(UserModelRequest param);
    }
}
