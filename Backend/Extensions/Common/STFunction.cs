using System.Net;
using System.Text.Json;
using Backend.EF.ST_Intranet;
using Backend.Models;
using ST_API.Models;
using Backend.Enum;
using Extensions.Systems;
using System.Data;
using ExcelDataReader;
using CsvHelper.Configuration;
using System.Text;
using CsvHelper;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Extensions.Common.STFunction
{
    /// <summary>
    /// ST Function
    /// </summary>
    public class STFunction
    {
        public static string sPathAvatarDefault = "https://w7.pngwing.com/pngs/340/946/png-transparent-avatar-user-computer-icons-software-developer-avatar-child-face-heroes-thumbnail.png";
        /// <summary>
        /// connect Entity DB
        /// </summary>
        public static void CallEntityOffice(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("STConnectionStrings"));
            optionsBuilder.EnableSensitiveDataLogging();
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetAppSettingJson(string GetParameter)
        {
            string Result = "";
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false);
            IConfigurationRoot configuration = builder.Build();
            IConfigurationSection section = configuration.GetSection(GetParameter);
            Result = section != null ? (section.Value + "") : "";
            return Result;
        }

        public static void CallEntity(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration =
                new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
            optionsBuilder
                .UseSqlServer(configuration
                    .GetConnectionString("STConnectionStrings"));
        }

        /// <summary>
        /// 
        /// </summary>
        public static string MapPath(string sPath, IHostEnvironment _env)
        {
            string sPathName = "";
            if (!FilePathHasInvalidChars(sPath))
            {
                bool isWindow = GetAppSettingJson("AppSetting:MapPathToWindow") == "Y";
                string sRootPath = (_env.IsDevelopment() ? "wwwroot" : "wwwroot") + (isWindow ? "\\UploadFile\\" : "/UploadFile/");
                string sFolderName = Path.Combine(sRootPath, sPath);
                string baseFolder = Path.Combine(_env.ContentRootPath, sFolderName);
                string sFullPath = Path.GetFullPath(baseFolder);

                if (FilePathHasInvalidChars(baseFolder))
                {
                    return "Trying to read path outside of root";
                }

                if (!sFullPath.StartsWith(_env.ContentRootPath))
                {
                    return "Trying to read path outside of root";
                }

                sPathName = RemoveInvalidPathChars(baseFolder);
            }

            return sPathName;
        }
        /// <summary>
        /// 
        /// </summary>
        public static string Scan_CWE22_File(string pathName, string fileName)
        {
            string sPathFile = "";
            string sFileName = Path.GetFileName(fileName);
            if (!string.IsNullOrEmpty(pathName) && !string.IsNullOrEmpty(sFileName))
            {
                sPathFile = Path.Combine(pathName, fileName);
                string sPathSecurity = (sPathFile)
                .Replace("../", "")
                .Replace("..\\", "")
                .Replace("..", "")
                .Replace("/", "\\").Trim();

                Uri uriAddress2 = new Uri(sPathSecurity);
                if (!uriAddress2.IsFile)
                {
                    sPathFile = sPathSecurity;
                }
            }
            return sPathFile;
        }
        /// <summary>
        /// 
        /// </summary>
        public static string Scan_CWE22_FullPathFile(string pathFile)
        {
            string sPathSecurity = "";
            if (!string.IsNullOrEmpty(pathFile))
            {
                sPathSecurity = pathFile
                .Replace("../", "")
                .Replace("..\\", "")
                .Replace("..", "")
                .Replace("/", "\\").Trim();

                Uri uriAddress2 = new Uri(sPathSecurity);
                if (!uriAddress2.IsFile)
                {
                    sPathSecurity = "";
                }
            }
            return sPathSecurity;
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool FilePathHasInvalidChars(string path)
        {
            return (!string.IsNullOrEmpty(path) && path.IndexOfAny(Path.GetInvalidPathChars()) >= 0);
        }
        /// <summary>
        /// 
        /// </summary>
        public static string RemoveInvalidPathChars(string filepath)
        {
            return !string.IsNullOrEmpty(filepath) ? string.Concat(filepath.Split(Path.GetInvalidPathChars())) : "";
        }
        /// <summary>
        /// 
        /// </summary>
        public static string RemoveUnsupportChar(string fileName)
        {
            if (fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0)
            {
                throw new Exception("Error");
            }

            fileName = fileName.Replace("'", "").Replace(";", "");

            return fileName;
        }
        /// <summary>
        /// 
        /// </summary>
        public static string RemoveUnsupportFolderChar(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string sPathFolder = "";
                string[] arrData = fileName.Split('/');
                foreach (var item in arrData)
                {
                    if (item.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0)
                    {
                        throw new Exception("Error");
                    }
                    else
                    {
                        sPathFolder += "/" + item;
                    }
                }
                string sNewFoler = sPathFolder.Length > 0 ? sPathFolder.Remove(0, 1) : "";
                fileName = sNewFoler.Replace("'", "").Replace(";", "");
            }
            return fileName;
        }

        public static cReportFont GetReportFont(string sType)
        {
            cReportFont objFont = new cReportFont();
            if (sType == "docx" || sType == "pdf")
            {
                objFont.sFont = "Angsana New";
                objFont.nFontSize = 16;
            }
            else
            {
                objFont.sFont = "Tahoma";
                objFont.nFontSize = 12;
            }
            return objFont;
        }

        public class cReportFont
        {
            public string sFont { get; set; }
            public int nFontSize { get; set; }
        }
        /// <summary>
        /// Get path File for display
        /// </summary>
        /// <param name="FilePath">Path file</param>
        /// <param name="SysFileName">ชื่อไฟล์ในระบบ</param>
        public static string? GetPathUploadFile(string FilePath, string SysFileName)
        {
            string? sFullPath = null;
            if (!string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(SysFileName))
            {
                string sPathWeb = GetAppSettingJson("AppSetting:UrlSiteBackend");
                sFullPath = sPathWeb + "UploadFile/" + FilePath + "/" + SysFileName;
            }
            return sFullPath;
        }
        /// <summary>
        /// ใช้สำหรับการ Create Folder
        /// </summary>
        /// <param name="sPath">Path ที่ต้องการ  Create</param>
        /// <param name="_env">IHostEnvironment</param>
        public static void FolderCreate(string sPath, IHostEnvironment _env)
        {
            var path = STFunction.MapPath(sPath, _env);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        /// <summary>
        /// ใช้สำหรับการ ย้ายไฟล์
        /// </summary>
        /// <param name="TempPath">Path เก่าที่ต้องการย้าย</param>
        /// <param name="TruePath">Path ใหม่ที่ต้องการไปวาง</param>
        /// <param name="SysFileName">ชื่อไฟล์ในระบบ</param>
        /// <param name="_env">IHostEnvironment</param>
        public static void MoveFile(string TempPath, string TruePath, string SysFileName, IHostEnvironment _env)
        {
            FolderCreate(TruePath, _env);
            var ServerTempPath = MapPath(TempPath, _env);
            var ServerTruePath = MapPath(TruePath, _env);

            string FileTempPath = Scan_CWE22_File(ServerTempPath, SysFileName);
            string FileTruePath = Scan_CWE22_File(ServerTruePath, SysFileName);
            if (File.Exists(FileTempPath))
            {
                if (FileTempPath != FileTruePath)
                {
                    File.Move(FileTempPath, FileTruePath);
                }
            }
        }

        public static string sBaseAdrees = "https://api.line.me/v2/bot/";
        public static string sGetProfile = "profile/";
        public static string sSendSingle = "message/push";
        public static string sSendMulti = "message/multicast";
        public static string sBearer = STFunction.GetAppSettingJson("AppSetting:sBearer");
        /// <summary>
        /// FontName_Export สไตล์ฟ้อน
        /// </summary>
        public static string FontName_Export = "Angsana New";
        /// <summary>
        /// FontSize_Export
        /// </summary>
        public static int FontSize_Export = 14;
        /// <summary>
        /// FontSize_Export_Head
        /// </summary>
        public static int FontSize_Export_Head = 16;
        /// <summary>
        /// Color_Head
        /// </summary>
        public static string Color_Head = "#004290";
        /// <summary>
        /// Color_Foot
        /// </summary>
        public static string Color_Foot = "#FFC000";
        public static ResultAPI SendToLine(cParamSendLine objParam)
        {
            ResultAPI objReturn = new ResultAPI();

            try
            {
                ST_IntranetEntity _db = new ST_IntranetEntity();
                var lstTMConfig = _db.TM_Config.ToArray();
                bool IsSendLine = false;
                string sPathDefault = "";
                var objConfig = lstTMConfig.FirstOrDefault(w => w.nConfigID == (int)EnumGlobal.Config.SenddingNoti);
                if (objConfig != null)
                {
                    IsSendLine = (objConfig.nValue.HasValue ? objConfig.nValue.Value == 1 : false);
                }
                objConfig = lstTMConfig.FirstOrDefault(w => w.nConfigID == (int)EnumGlobal.Config.DefaultPath);
                if (objConfig != null)
                {
                    sPathDefault = objConfig.sValue ?? "";
                }
                using (var client = new HttpClient())
                {
                    ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls;


                    client.BaseAddress = new Uri(sBaseAdrees);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + sBearer);
                    string sPathAPI = sSendMulti;
                    string sMessage = "";
                    string sEmailTo = "";
                    if (objParam.lstEmpTo != null)
                    {
                        var lstLineAccount = _db.TB_Employee_LineAccount.Where(w => objParam.lstEmpTo.Contains(w.nEmployeeID)).ToList();
                        var lstLineToken = lstLineAccount.Select(s => s.sTokenID).ToList();
                        sEmailTo = string.Join("\\n", lstLineAccount.Select(s => "-" + s.sEmail).ToList());

                        if (string.IsNullOrEmpty(sEmailTo))
                        {
                            var lstEmailTemp = _db.TB_Employee.Where(w => objParam.lstEmpTo.Contains(w.nEmployeeID)).Select(s => "-" + s.sEmail).ToList();
                            sEmailTo = string.Join("\\n", lstEmailTemp);

                        }
                        objParam.sTo = JsonSerializer.Serialize(lstLineToken);
                    }

                    objConfig = lstTMConfig.FirstOrDefault(w => w.nConfigID == (int)EnumGlobal.Config.MailMode);

                    //Test Mode
                    string sTest = "";
                    if (objConfig != null)
                    {
                        if (objConfig.nValue == 2)
                        {
                            objConfig = _db.TM_Config.FirstOrDefault(w => w.nConfigID == (int)EnumGlobal.Config.EmailSendding);
                            if (objConfig != null)
                            {
                                List<string> lstTo = !string.IsNullOrEmpty(objConfig.sValue) ? objConfig.sValue.Split(',').ToList() : new List<string>();
                                var lstLineAccount = _db.TB_Employee_LineAccount.Where(w => lstTo.Contains(w.sEmail)).ToList();
                                var lstLineToken = lstLineAccount.Select(s => s.sTokenID).ToList();
                                objParam.sTo = JsonSerializer.Serialize(lstLineToken);
                            }
                            var objConfigTemplate = _db.TM_LineTemplate.FirstOrDefault(w => w.nID == 98);
                            if (objConfigTemplate != null)
                            {
                                sTest = objConfigTemplate.sDetail.Replace("{97}", sEmailTo);
                            }
                        }




                    }

                    var objLineAccount = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == objParam.nRequesterID);
                    if (objLineAccount != null)
                    {
                        objParam.sNameAccount = $"{objLineAccount.sNameEN} ({objLineAccount.sNickname})";

                        var objPathProfile = _db.TB_Employee_Image.FirstOrDefault(w => w.nImageType == (int)EnumGlobal.MasterData.ImageProfile && !w.IsDelete);
                        if (objPathProfile != null)
                        {
                            objParam.sPathImage = STFunction.GetPathUploadFile(objPathProfile.sPath ?? "", objPathProfile.sSystemFileName ?? "");
                        }
                    }
                    string sLIFFURL = @"https://liff.line.me/1657075099-7qVpevX9?sPage=";
                    var objFormat = _db.TM_LineTemplate.FirstOrDefault(w => w.nID == objParam.nTemplateID);
                    if (objFormat != null)
                    {
                        sMessage = objFormat.sDetail.Replace("{0}", objParam.sTo).Replace("\"[", "[").Replace("]\"", "]");
                        sMessage = sMessage.Replace("{96}", objParam.sDetailTemplate);
                        sMessage = sMessage.Replace("{1}", objParam.sNameAccount);
                        sMessage = sMessage.Replace("{2}", objParam.sTime);
                        sMessage = sMessage.Replace("{3}", objParam.sDate);
                        sMessage = sMessage.Replace("{4}", objParam.sLocation);
                        sMessage = sMessage.Replace("{5}", objParam.sDelay);
                        sMessage = sMessage.Replace("{6}", objParam.sProject);
                        sMessage = sMessage.Replace("{7}", objParam.sType);
                        sMessage = sMessage.Replace("{8}", objParam.sRemark);
                        sMessage = sMessage.Replace("{9}", objParam.sStatus);
                        sMessage = sMessage.Replace("{10}", objParam.sStartTime);
                        sMessage = sMessage.Replace("{11}", objParam.sEndTime);
                        sMessage = sMessage.Replace("{12}", objParam.sStartDate);
                        sMessage = sMessage.Replace("{13}", objParam.sEndDate);
                        sMessage = sMessage.Replace("{14}", objParam.sDetailRequest);
                        sMessage = sMessage.Replace("{15}", objParam.sHours);
                        sMessage = sMessage.Replace("{16}", objParam.sHoursActual);
                        sMessage = sMessage.Replace("{17}", objParam.sMoney);
                        sMessage = sMessage.Replace("{18}", objParam.sDay);
                        sMessage = sMessage.Replace("{19}", objParam.sMonth);
                        sMessage = sMessage.Replace("{20}", objParam.sHospital);
                        sMessage = sMessage.Replace("{21}", objParam.sRoom);
                        sMessage = sMessage.Replace("{22}", objParam.sTitle);
                        sMessage = sMessage.Replace("{23}", objParam.sDateRequest);
                        sMessage = sMessage.Replace("{24}", objParam.sTimeRequest);
                        sMessage = sMessage.Replace("https://Approve.com", sLIFFURL + objParam.sPathApprove);
                        sMessage = sMessage.Replace("https://Reject.com", sLIFFURL + objParam.sPathReject);
                        sMessage = sMessage.Replace("https://Detail.com", sLIFFURL + objParam.sPathDetail);
                        sMessage = sMessage.Replace("https://Cancel.com", sLIFFURL + objParam.sPathCancel);
                        sMessage = sMessage.Replace("{98}", sTest);
                        string sSubject = objFormat.sSubject.Replace("{1}", objParam.sNameAccount);
                        sMessage = sMessage.Replace("{99}", sSubject);


                        sMessage = sMessage.Replace(sPathDefault, objParam.sPathImage);
                    }
                    var objParamSend = new cParamSendLine();
                    var sStringContent = JsonSerializer.Serialize(objParamSend);
                    var requestContent = new StringContent(sMessage, System.Text.Encoding.UTF8, "application/json");

                    var response = client.PostAsync(sPathAPI, requestContent).GetAwaiter().GetResult();

                    var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    cResult? objResult = JsonSerializer.Deserialize<cResult>(content) as cResult;
                    if (objResult != null)
                    {
                        if (!string.IsNullOrEmpty(objResult.sMssage))
                        {
                            objReturn.sMessage = objResult.sMssage;
                            objReturn.nStatusCode = StatusCodes.Status500InternalServerError;
                        }
                    }

                    TB_Log_WebhookLine objWebhook = new TB_Log_WebhookLine();
                    objWebhook.sGUID = objParam.sGUID;
                    objWebhook.dSend = DateTime.Now;
                    objWebhook.sMessage = sMessage;
                    _db.TB_Log_WebhookLine.Add(objWebhook);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                objReturn.sMessage = ex.Message.ToString();
                objReturn.nStatusCode = StatusCodes.Status500InternalServerError;
            }
            return objReturn;
        }

        public static ResultAPI SendToEMail(cParamSendLine objParam)
        {
            ResultAPI objReturn = new ResultAPI();

            return objReturn;
        }

        public static AllClass.CResultReadFile OpenExcel2DataTable(string sFileName)
        {
            AllClass.CResultReadFile result = new AllClass.CResultReadFile();
            result.nStatusCode = StatusCodes.Status200OK;
            result.sMsg = "";
            result.lstError = new List<string>();
            string sMapPath = MapPath(sFileName);
            if (CheckFileExist(sFileName, ""))
            {
                try
                {
                    DataTable dtExcel = new DataTable();
                    IExcelDataReader excelReader = null;
                    DataTable dtCsv = new DataTable();

                    string[] arrFilename = (sMapPath + "").Split('.');
                    string sExtension = arrFilename[arrFilename.Length - 1];
                    if (sExtension.ToLower().Trim() == "csv")
                    {
                        using (var fs = new StreamReader(sMapPath))
                        {
                            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                            {
                                PrepareHeaderForMatch = args => args.Header.ToLower(),
                                Delimiter = ";",
                                Encoding = UTF8Encoding.UTF8,
                                MissingFieldFound = null
                            };

                            using (var csv = new CsvReader(fs, config))
                            {
                                using (var dr = new CsvDataReader(csv))
                                {
                                    dtExcel.Load(dr);

                                }
                            }

                        }
                    }
                    else if (sExtension.ToLower().Trim() == "xlsx")
                    {
                        FileStream stream = File.Open(sMapPath, FileMode.Open, FileAccess.Read);
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        DataSet ds = excelReader.AsDataSet();
                        result.ds = ds;
                        dtExcel = result.ds.Tables[0];
                        stream.Dispose();
                    }
                    else
                    {
                        FileStream stream = File.Open(sMapPath, FileMode.Open, FileAccess.Read);
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        DataSet ds = excelReader.AsDataSet();
                        result.ds = ds;
                        dtExcel = result.ds.Tables[0];
                    }
                    result.dt = dtExcel;

                }
                catch (Exception ex)
                {
                    result.nStatusCode = StatusCodes.Status500InternalServerError;
                    result.sMsg = ex.Message.ToString() + ""; //"Please save file before import !";
                    System.Diagnostics.Process.GetProcesses()
                        .Where(x => x.ProcessName.ToLower()
                                     .StartsWith("cheate"))
                        .ToList()
                        .ForEach(x => x.Kill());
                }

            }
            else
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMsg = "File not found"; //"Please save file before import !";
            }
            return result;
        }

        public static bool CheckFileExist(string sPath, string SysFileName)
        {
            bool Result = false;
            sPath = sPath + "";
            SysFileName = SysFileName + "";
            var _env = Directory.GetCurrentDirectory();
            var ServerTruePath = MapPath(sPath);
            string FileTruePath = ServerTruePath + SysFileName;
            if (File.Exists(FileTruePath))
            {
                Result = true;
            }

            return Result;
        }

        public static string MapPath(string path)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory() + "\\ClientApp\\build\\UploadFile\\Temp\\" + path).Replace("/", "\\");
            bool isWindow = GetAppSettingJson("AppSetting:MapPathToWindow") == "Y";
            if (!isWindow)
                filePath = filePath.Replace("\\", "/");
            return filePath;
        }


         /// <summary> Change Order aDynamic Table and column</summary>
        public static ResultAPI OnChangeOrder(ChgFilter oParam)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                string _Connect = GetConnectionString();
                using (SqlConnection _conneect = new SqlConnection(_Connect))
                {
                    using (SqlCommand cmd = new SqlCommand("spChangeOrder", _conneect))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        _conneect.Open();

                        cmd.Parameters.AddWithValue("@Table", oParam.Table);
                        cmd.Parameters.AddWithValue("@Column", oParam.Column);
                        cmd.Parameters.AddWithValue("@nID", oParam.nID);
                        cmd.Parameters.AddWithValue("@NewOrder", oParam.NewOrder);
                        cmd.ExecuteNonQuery();
                        _conneect.Close();
                    }
                }
                result.sMessage = "Success";
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception ex)
            {
                result.sMessage = ex.Message;
                result.nStatusCode = StatusCodes.Status500InternalServerError;
            }
            return result;
        }

          /// <summary>
        /// </summary>
        public static string GetConnectionString()
        {
            return GetAppSettingJson("ConnectionStrings:STConnectionStrings");
        }

         /// <summary>
        /// </summary>
        public class ChgFilter
        {
            /// <summary>name table for change nOrder</summary>
            public string Table { get; set; } = string.Empty;
            /// <summary>Column for change</summary>
            public string TypeColumn { get; set; } = string.Empty;
            /// <summary>Column for change</summary>
            public int? TypeID { get; set; }
            /// <summary>Column for change</summary>
            public string Column { get; set; } = string.Empty;
            /// <summary>id primary key table </summary>
            public int nID { get; set; }
            /// <summary>order target</summary>
            public int NewOrder { get; set; }
        }

    }
}
