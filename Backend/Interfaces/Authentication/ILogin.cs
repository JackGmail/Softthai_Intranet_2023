using Backend.Models.Authentication;

namespace Backend.Interfaces.Authentication
{
    /// <summary>
    /// Service/LoginService.cs
    /// </summary>
    public interface ILogin
    {
        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <response code="200">ผ่าน</response>
        ResultLogin Login(ParamLogin obj);

        ///// <summary>
        ///// <summary>
        //ResultAPI GetUserPermission(string sRoute);
        ///// <summary>
        ///// <summary>
        //ResultAPI GetPermission(int? nMenuID, string sRoute);
        ///// <summary>description</summary>
        //cInitPage getGroupuser();

       
    }
}
