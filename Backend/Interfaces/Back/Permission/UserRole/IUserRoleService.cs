
using Backend.Models;

namespace Backend.Interfaces.Permission.UserRole
{
    public interface IUserRoleService
    {
        cResultUserRoleTable GetDataTable(cUserRoleTable param);
        UserRoleModel GetUserRole(UserRoleModelRequest param);
        ResultAPI SaveUserRole(UserRoleModel param);
        ResultAPI RemoveDataTable(cUserGroupTable param);
        ResultAPI UserRoleToggleActive(ClassTableUserRole param);
    }
}
