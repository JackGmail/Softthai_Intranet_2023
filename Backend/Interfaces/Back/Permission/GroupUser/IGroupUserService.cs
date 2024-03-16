
using Backend.Models;

namespace Backend.Interfaces.Permission.GroupUser
{
    public interface IGroupUserService
    { 
        cResultUserGroupTable GetDataTable(cUserGroupTable param);
        /// <summary>
        /// </summary>
        cResultUserGroup GetInitData();
        /// <summary>
        /// </summary>
        UserGroupModel GetUserGroup(UserGroupModelRequest param);
        /// <summary>
        /// </summary>
        ResultAPI SaveUserGroup(UserGroupModel param);
        /// <summary>
        /// </summary>
        ResultAPI RemoveDataGroupTable(cUserGroupTable param);
        /// <summary>
        /// </summary>
        cPermission GetPermissionUserRole(UserRoleModelRequest param);
        ResultAPI GroupToggleActive(ClassTableGroup param);
    }
}
