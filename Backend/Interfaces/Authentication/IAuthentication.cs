using Backend.Models;
using Backend.Models.Authentication;

namespace Backend.Interfaces.Authentication
{
    public interface IAuthentication
    {
        #region JWT
        UserAccount GetUserAccount();
        bool HasExpired();
        #endregion

        #region Login
        ResultAPI Login(ParamLogin obj);
        #endregion

        #region Permission
        ResultUserPermisson GetUserPermission(ParamUserPermisson obj);
        ResultAPI GetMenuPermission(string sRoute);
        ResultAPI LoginAccountLine(AccountLine Param);
        ResultAPI AutoLoginFromLine(AccountLine Param);
        ResultAPI CheckActionAlreadyLine(AccountLine Param);
        ResultAPI STEncrypt(STEnCrypt Param);
        ResultAPI STDecrypt(STEnCrypt Param);
        #endregion
    }
}