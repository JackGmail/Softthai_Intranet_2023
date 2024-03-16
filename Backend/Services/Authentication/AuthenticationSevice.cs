using Backend.EF.ST_Intranet;
using Backend.Enum;
using Backend.Interfaces.Authentication;
using Backend.Models;
using Backend.Models.Authentication;
using Extensions.Common.STFunction;
using Microsoft.IdentityModel.Tokens;
using ST.INFRA;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static Backend.Enum.EnumGlobal;

namespace Backend.Service
{
    public class AuthenticationSevice : IAuthentication
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ST_IntranetEntity _db;
        public AuthenticationSevice(IHttpContextAccessor httpContextAccessor, IConfiguration config, ST_IntranetEntity db)
        {
            _httpContext = httpContextAccessor;
            _config = config;
            _db = db;
        }

        #region JWT
        private string BuildToken(ParamJWT jwtSecret)
        {
            List<Claim> lstClaims = new List<Claim>();
            PropertyInfo[] properties = typeof(ParamJWT).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var propType = property.PropertyType;
                if (propType.IsGenericType)
                {
                    PropertyInfo? propJwtSecret = jwtSecret.GetType().GetProperty(property.Name);
                    if (propJwtSecret != null)
                    {
                        var lst = propJwtSecret.GetValue(jwtSecret, null);
                        lstClaims.Add(new Claim(property.Name, JsonSerializer.Serialize(lst)));
                    }
                }
                else
                {
                    string sValue = "";
                    PropertyInfo? propJwtSecret = jwtSecret.GetType().GetProperty(property.Name);
                    if (propJwtSecret != null)
                    {
                        sValue = propJwtSecret.GetValue(jwtSecret, null) + "";
                    }
                    lstClaims.Add(new Claim(property.Name, sValue));
                }
            }

            string sSecretKey = _config["jwt:SecretKey"] ?? "";
            string sIssuer = _config["jwt:Issuer"] ?? "";
            string sAudience = _config["jwt:Audience"] ?? "";
            DateTime dExpires = DateTime.Now.AddMinutes(Convert.ToDouble(_config["jwt:Expire"]));

            SymmetricSecurityKey smtSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sSecretKey));
            SigningCredentials credentials = new SigningCredentials(smtSecurityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: sIssuer,
                audience: sAudience,
                claims: lstClaims,
                expires: dExpires,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public UserAccount GetUserAccount()
        {
            UserAccount result = new UserAccount();
            if (_httpContext.HttpContext != null)
            {
                PropertyInfo[] properties = typeof(ParamJWT).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo property in properties)
                {
                    var resultProp = result.GetType().GetProperty(property.Name);
                    if (resultProp != null)
                    {
                        var obj = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == property.Name);
                        if (obj != null)
                        {
                            var propType = property.PropertyType;
                            if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var propTypeOrNull = Nullable.GetUnderlyingType(property.PropertyType);
                                if (!string.IsNullOrEmpty(obj.Value) && propTypeOrNull != null)
                                {
                                    resultProp.SetValue(result, Convert.ChangeType(obj.Value, propTypeOrNull), null);
                                }
                            }
                            else if (propType.IsGenericType)
                            {
                                var baseType = typeof(List<>);
                                Type genericType = baseType.MakeGenericType(propType.GetGenericArguments().First());
                                var lst = JsonSerializer.Deserialize(obj.Value, genericType);
                                resultProp.SetValue(result, lst);
                            }
                            else
                            {
                                resultProp.SetValue(result, Convert.ChangeType(obj.Value, propType), null);
                            }
                        }
                    }
                }
            }
            return result;
        }
        bool IAuthentication.HasExpired()
        {
            if (_httpContext.HttpContext != null)
            {
                return _httpContext.HttpContext.User.Claims.HasItems();
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Login
        public ResultAPI Login(ParamLogin param)
        {
            ResultAPI result = new ResultAPI();
            string sUsername = (param.sUsername + "").Trim().ToLower();
            var objUser = _db.TB_Employee.FirstOrDefault(w => (w.sUsername.ToLower() == sUsername || (w.sEmail + "").ToLower() == sUsername) && !w.IsDelete);
            if (objUser != null)
            {
                if (objUser.IsActive)
                {
                    TM_Config? objConfigBypass = _db.TM_Config.FirstOrDefault(w => w.nConfigID == (int)Config.ByPass);
                    TM_Config? objConfigIsLoginBypass = _db.TM_Config.FirstOrDefault(w => w.nConfigID == (int)Config.IsLoginBypass);
                    TM_Config? objConfigImagePath = _db.TM_Config.FirstOrDefault(w => w.nConfigID == (int)EnumGlobal.Config.DefaultPath);
                    string sByPWD = objConfigBypass != null ? objConfigBypass.sValue + "" : "";
                    string sImagePath = objConfigImagePath != null ? objConfigImagePath.sValue + "" : "";
                    bool IsLoginBypass = objConfigIsLoginBypass != null ? (objConfigIsLoginBypass.sValue == "1" && sByPWD == param.sPWD) : false;
                    string sPWD = param.sPWD.DecryptParameter();
                    TB_Employee_Image? objEmpImg = _db.TB_Employee_Image.FirstOrDefault(f => f.nEmployeeID == objUser.nEmployeeID);
                    if ((objUser.sPassword == sPWD || objUser.sPassword == param.sPWD) || IsLoginBypass)
                    {
                        List<int> lstUserRole = _db.TB_UserMappingRole.Where(w => w.nEmployeeID == objUser.nEmployeeID).Select(s => s.nUserRoleID).ToList();
                        List<int> lstUserGroup = _db.TB_UserMappingGroup.Where(w => w.nEmployeeID == objUser.nEmployeeID).Select(s => s.nUserGroupID).ToList();
                        List<int> lstPosition = _db.TB_Employee_Position.Where(w => w.nEmployeeID == objUser.nEmployeeID).Select(s => s.nPositionID).ToList();
                        List<string> lstPositionName = _db.TB_Position.Where(w => lstPosition.Contains(w.nPositionID) && !string.IsNullOrEmpty(w.sPositionName)).Select(s => s.sPositionName).ToList();

                        ParamUserPermisson paramUserPrms = new ParamUserPermisson();
                        paramUserPrms.nUserID = objUser.nEmployeeID;
                        paramUserPrms.lstUserGroupID = lstUserGroup;
                        paramUserPrms.lstUserRoleID = lstUserRole;
                        ResultUserPermisson objUserPermission = GetUserPermission(paramUserPrms);

                        ParamJWT tokenJWT = new ParamJWT();
                        tokenJWT.nUserID = objUser.nEmployeeID;
                        tokenJWT.lstUserRoleID = lstUserRole;
                        tokenJWT.lstUserGroupID = lstUserGroup;
                        tokenJWT.lstUserPositionID = lstPosition;
                        tokenJWT.sFullName = objUser.sNameEN + " " + objUser.sSurnameEN;
                        if(objEmpImg != null)
                        {
                            tokenJWT.sImageUser = STFunction.GetPathUploadFile(objEmpImg.sPath ?? "", objEmpImg.sSystemFileName ?? "");
                        }
                        tokenJWT.sPosition = string.Join(",", lstPositionName);
                        tokenJWT.sEmail = objUser.sEmail ?? "";
                        tokenJWT.lstMenuPrms = objUserPermission.lstMenuPermission;

                        string sToken = BuildToken(tokenJWT);
                        result.objResult = sToken;
                    }
                    else
                    {
                        result.nStatusCode = (int)APIStatusCode.Warning;
                        result.sMessage = "Password Not Found";
                    }
                }
                else
                {
                    result.nStatusCode = (int)APIStatusCode.Warning;
                    result.sMessage = "User Not Unauthorized";
                }
            }
            else
            {
                result.nStatusCode = (int)APIStatusCode.Warning;
                result.sMessage = "User Not Found";
            }
            return result;
        }

        public ResultAPI LoginAzure(ParamLogin param)
        {
            ResultAPI result = new ResultAPI();
            ParamJWT tokenJWT = new ParamJWT();
            tokenJWT.nUserID = 99;
            string sToken = BuildToken(tokenJWT);
            result.nStatusCode = StatusCodes.Status200OK;
            result.objResult = sToken;
            return result;
        }
        #endregion

        #region User Permission And Menu Permission
        public ResultUserPermisson GetUserPermission(ParamUserPermisson param)
        {
            ResultUserPermisson resultPermission = new ResultUserPermisson();

            #region Get Master Data
            List<int> lstMenu = _db.TM_Menu.Where(w => w.IsActive && w.IsSetPermission).Select(s => s.nMenuID).ToList();
            List<int> lstRole = _db.TB_UserRole.Where(w => w.IsActive).Select(s => s.nUserRoleID).ToList();
            List<int> lstGroup = _db.TB_UserGroup.Where(w => w.IsActive).Select(s => s.nUserGroupID).ToList();
            #endregion

            #region Get User Permission
            List<MenuPermisson> lstUserPermission = _db.TB_UserPermission.Where(w => w.nEmployeeID == param.nUserID).Select(s => new MenuPermisson
            {
                nMenuID = s.nMenuID,
                nPermission = s.nPermission
            }).ToList();

            List<MenuPermisson> lstUserGroupPermission = _db.TB_UserGroupPermisson
            .Where(w => lstGroup.Contains(w.nUserGroupID) && param.lstUserGroupID.Contains(w.nUserGroupID) && lstMenu.Contains(w.nMenuID))
            .GroupBy(g => new { g.nMenuID })
            .Select(s => new MenuPermisson
            {
                nMenuID = s.Key.nMenuID,
                nPermission = s.Max(m => m.nPermission)
            }).ToList();

            List<MenuPermisson> lstUserRolePermission = _db.TB_UserRolePermission
            .Where(w => lstRole.Contains(w.nUserRoleID) && param.lstUserRoleID.Contains(w.nUserRoleID) && lstMenu.Contains(w.nMenuID))
            .GroupBy(g => new { g.nMenuID })
            .Select(s => new MenuPermisson
            {
                nMenuID = s.Key.nMenuID,
                nPermission = s.Max(m => m.nPermission)
            }).ToList();
            #endregion

            #region Union Permission And Get Max Status
            List<MenuPermisson> lstPermission = lstUserPermission;
            //List<MenuPermisson> lstPermission = lstUserRolePermission.Union(lstUserGroupPermission)
            //.GroupBy(g => new { g.nMenuID })
            //.Select(s => new MenuPermisson
            //{
            //    nMenuID = s.Key.nMenuID,
            //    nPermission = s.Max(m => m.nPermission)
            //}).ToList();

            //#region Default User Permission First Piority
            //foreach (var item in lstUserPermission)
            //{
            //    MenuPermisson? objPermission = lstPermission.FirstOrDefault(f => f.nMenuID == item.nMenuID);
            //    if (objPermission == null)
            //    {
            //        lstPermission.Add(item);
            //    }
            //    else
            //    {
            //        objPermission.nPermission = item.nPermission;
            //    }
            //}
            //#endregion

            #endregion
            List<MenuPermisson> lstPermissionSub = new List<MenuPermisson>();
            foreach (var Item in lstPermission)
            {
                GetParentPermission(Item, lstPermissionSub);
            }

            resultPermission.lstMenuPermission = lstPermission.Union(lstPermissionSub).ToList();
            return resultPermission;
        }
        private void GetParentPermission(MenuPermisson objItem, List<MenuPermisson> lstMenuPermission)
        {
            var lstMenu = _db.TM_Menu.Where(w => w.nParentID == objItem.nMenuID && w.IsActive && !w.IsSetPermission);
            foreach (var item in lstMenu)
            {
                var objMenuItem = new MenuPermisson
                {
                    nMenuID = item.nMenuID,
                    nPermission = objItem.nPermission
                };
                lstMenuPermission.Add(objMenuItem);
                GetParentPermission(objMenuItem, lstMenuPermission);
            }
        }

        public ResultAPI GetMenuPermission(string sRoute)
        {
            ResultAPI result = new ResultAPI();
            ResultMenuPermisson ResultMenuPermisson = new ResultMenuPermisson();
            UserAccount ua = GetUserAccount();
            string[] arrRoute = sRoute.ToStringToLower().Split("/");
            string sMenuRoute = "/" + arrRoute.Last();
            TM_Menu? objMenu = _db.TM_Menu.FirstOrDefault(w => w.sRoute.ToLower() == sMenuRoute);
            if (objMenu != null)
            {
                MenuPermisson? objMenuItem = ua.lstMenuPrms.FirstOrDefault(w => w.nMenuID == objMenu.nMenuID);
                if (objMenuItem != null)
                {
                    ResultMenuPermisson.nPermission = objMenuItem.nPermission;
                }
            }
            result.objResult = ResultMenuPermisson;
            return result;
        }

        public ResultAPI LoginAccountLine(AccountLine Param)
        {
            ResultAPI result = new ResultAPI();

            int nEmployeeID = 0;
            var Account = _db.TB_Employee_LineAccount.FirstOrDefault(w => w.sTokenID == Param.sUserID);
            if (Account != null)
            {
                nEmployeeID = Account.nEmployeeID;
            }
            var Employee = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == nEmployeeID);
            var sPassword = _db.TM_Config.FirstOrDefault(w => w.nConfigID == (int)EnumGlobal.Config.ByPass);

            string sUsername = "";
            string sPasswordByPass = "";
            if (Employee != null)
            {
                sUsername = Employee.sUsername;
                if (sPassword != null)
                {
                    sPasswordByPass = sPassword.sValue ?? "";
                }
            }
            var param = new ParamLogin();
            {
                param.sUsername = sUsername;
                param.sPWD = sPasswordByPass;
            }
            ResultAPI objData = Login(param);

            result.objResult = objData.objResult;



            return result;
        }
        public ResultAPI AutoLoginFromLine(AccountLine Param)
        {
            ResultAPI result = new ResultAPI();


            int nEmployeeID = (Param.sUserID ?? "0").DecryptParameter().ToInt();
            if (string.IsNullOrEmpty(Param.sUserID))
            {
                var objAccount = _db.TB_Employee_LineAccount.FirstOrDefault(w => w.sTokenID == Param.sUsername);
                if (objAccount != null)
                {
                    nEmployeeID = objAccount.nEmployeeID;
                }
            }
            else
            {
                if (nEmployeeID == 0)
                {
                    List<int> arrEmp = Param.sUserID.DecryptParameter().Split(",").Select(s => s.ToInt()).ToList();
                    if (!string.IsNullOrEmpty(Param.sUserID))
                    {
                        var objAccount = _db.TB_Employee_LineAccount.FirstOrDefault(w => w.sTokenID == Param.sUsername && arrEmp.Contains(w.nEmployeeID));
                        if (objAccount != null)
                        {
                            nEmployeeID = objAccount.nEmployeeID;
                        }
                        else
                        {
                            nEmployeeID = arrEmp.FirstOrDefault();
                        }
                    }

                }
            }
            var objEmployee = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == nEmployeeID);
            var objPassword = _db.TM_Config.FirstOrDefault(w => w.nConfigID == (int)EnumGlobal.Config.ByPass);

            string sUsername = "";
            string sPasswordByPass = "";
            if (objEmployee != null)
            {
                sUsername = objEmployee.sUsername;
                if (objPassword != null)
                {
                    sPasswordByPass = objPassword.sValue ?? "";
                }
            }
            var param = new ParamLogin();
            {
                param.sUsername = sUsername;
                param.sPWD = sPasswordByPass;
            }
            ResultAPI objData = Login(param);
            result.objResult = objData.objResult;

            return result;
        }
        public ResultAPI CheckActionAlreadyLine(AccountLine Param)
        {
            ResultAPI result = new ResultAPI();

            TB_Log_WebhookLine? objLog = _db.TB_Log_WebhookLine.Where(w => w.sGUID == Param.sGUID).FirstOrDefault();
            if (objLog != null)
            {
                if (objLog.IsActionAlready == true)
                {
                    result.nStatusCode = StatusCodes.Status208AlreadyReported;
                    return result;
                }
            }
            result.nStatusCode = StatusCodes.Status200OK;
            return result;
        }
        public ResultAPI STEncrypt(STEnCrypt Param)
        {
            ResultAPI result = new ResultAPI();
            result.objResult = Param.sValue?.EncryptParameter();
            result.nStatusCode = StatusCodes.Status200OK;
            return result;
        }
        public ResultAPI STDecrypt(STEnCrypt Param)
        {
            ResultAPI result = new ResultAPI();
            result.objResult = Param.sValue?.DecryptParameter();
            result.nStatusCode = StatusCodes.Status200OK;
            return result;
        }
        #endregion
    }
}
