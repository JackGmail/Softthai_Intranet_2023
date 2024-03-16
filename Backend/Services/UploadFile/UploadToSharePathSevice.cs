// using DocumentFormat.OpenXml.Wordprocessing;
using System.Net;
using Extensions.Common.STFunction;
using Extensions.Common.STResultAPI;
using Extensions.Systems;

namespace Backend.Service.UploadFileSharePath
{

    public interface IUploadToSharePathSevice
    {
        Task<AllClass.ItemFileData> UploadFileToTemp(string? sFolderTemp, bool? isResize, int? nWidthResize, int? nHeigthResize, int nIndex, bool? IsCheckRecommendSize);
        ResultFile DownloadFileInSharePath(FilterFile objsPathFile);
        /// <summary>
        /// Move File To SharePath
        /// </summary>
        /// <param name="sDirectorySourceFile">Path ต้นทาง</param>
        /// <param name="sSourceFileName">ชื่อไฟล ต้นทาง</param>
        /// <param name="sDestinationDirectory">Path ปลายทาง</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        ResultAPISharePath MoveFileToSharePath(string sDirectorySourceFile, string sSourceFileName, string sDestinationDirectory);
    }

    public class UploadToSharePathSevice : IUploadToSharePathSevice
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        public readonly string networkPath = STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathUpFile") + "";
        public readonly string IsLogonSharePath = STFunction.GetAppSettingJson("AppSetting:SharePath:IsLogonSharePath") + "";
        public readonly NetworkCredential credentials = new NetworkCredential(
            STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathUser") + "",
            STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathPassword") + "",
            STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathDomain") + ""
        );

        public UploadToSharePathSevice(IWebHostEnvironment IWebHostEnvironment_, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = IWebHostEnvironment_;
            _httpContext = httpContextAccessor;
        }

        public async Task<AllClass.ItemFileData> UploadFileToTemp(string? sFolderTemp, bool? isResize, int? nWidthResize, int? nHeigthResize, int nIndex, bool? IsCheckRecommendSize)
        {
            AllClass.ItemFileData result = new AllClass.ItemFileData();
            if (_httpContext != null && _httpContext.HttpContext != null)
            {
                var HttpContext = _httpContext.HttpContext;
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i < HttpContext.Request.Form.Files.Count; i++)
                    {
                        IFormFile? file = HttpContext.Request.Form.Files[i];
                        if (file != null)
                        {
                            string sFileName = file.FileName;
                            string sSysFileName = DateTime.Now.ToString("ddMMyyyyHHmmssff") + "_s" + file.FileName;
                            string sFileType = "";
                            string filepath = "";
                            string sFileLink = "";

                            #region Check
                            //Check Is Folder
                            var arrSplit = sSysFileName.Split("/");
                            if (arrSplit.Count() > 1) // Is Folder
                            {
                                sSysFileName = arrSplit[1];
                                sFileName = arrSplit[1];
                            }

                            //Cut File Name
                            var arrfilename = sFileName.Split('.');
                            if (sFileName.Length > 100)
                            {
                                sFileName = sFileName.Substring(0, 90) + "." + arrfilename[arrfilename.Length - 1];
                            }

                            //Cut System File Name
                            sSysFileName = sSysFileName.Replace(" ", "_").Replace("(", "").Replace(")", "");
                            var arrSysfilename = sSysFileName.Split('.');
                            if (sSysFileName.Length > 25)
                            {
                                sSysFileName = sSysFileName.Substring(0, 20) + "." + arrSysfilename[arrSysfilename.Length - 1];
                            }
                            sFileType = arrSysfilename[arrSysfilename.Length - 1];
                            #endregion

                            #region Upload SharePath
                            bool isToSharePath = false;
                            try
                            {
                                if (IsLogonSharePath == "Y" && NetworkConnection.RevertToSelf())
                                {
                                    using (new NetworkConnection(networkPath, credentials))
                                    {
                                        string sPathDestination = Path.Combine(networkPath + @"\Temp\");
                                        var dirDestination = new DirectoryInfo(sPathDestination);
                                        if (!dirDestination.Exists)
                                        {
                                            dirDestination.Create();
                                        }

                                        if (dirDestination.Exists)
                                        {
                                            using (FileStream stream = new(Path.Combine(Path.Combine(sPathDestination), sSysFileName), FileMode.Create))
                                            {
                                                file.CopyTo(stream);
                                                filepath = "Temp/";
                                                var dirFileLin = new DirectoryInfo(Path.Combine(sPathDestination));
                                                var urlFile = dirFileLin.GetFiles(sSysFileName);
                                                if (urlFile.Length > 0)
                                                {
                                                    sFileLink = filepath + sSysFileName;
                                                    result.IsCompleted = true;
                                                }
                                                isToSharePath = true;
                                                stream.Dispose();
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                // Systemfunction.onSaveLog_SystemError("1", "UploadFileToTemp", e.Message, e.StackTrace, "");
                            }
                                    result.IsCompleted = true;
                            #endregion

                            #region Upload Local
                            // if (!isToSharePath)
                            // {
                            //     try
                            //     {
                            //         string sPathDestination = Path.Combine(_hostingEnvironment.ContentRootPath + @"wwwroot\UploadFile\Temp\");
                            //         var dirDestination = new DirectoryInfo(sPathDestination);
                            //         if (!dirDestination.Exists)
                            //         {
                            //             dirDestination.Create();
                            //         }

                            //         if (dirDestination.Exists)
                            //         {
                            //             using (FileStream stream = new(Path.Combine(Path.Combine(_hostingEnvironment.ContentRootPath + @"wwwroot\UploadFile\Temp"), sSysFileName), FileMode.Create))
                            //             {
                            //                 file.CopyTo(stream);
                            //                 filepath = "UploadFile/Temp/";
                            //                 var dirFileLin = new DirectoryInfo(Path.Combine(sPathDestination));
                            //                 var urlFile = dirFileLin.GetFiles(sSysFileName);
                            //                 if (urlFile.Length > 0)
                            //                 {
                            //                     sFileLink = filepath + sSysFileName;
                            //                     result.IsCompleted = true;
                            //                 }
                            //                 isToSharePath = true;
                            //                 stream.Dispose();
                            //             }
                            //         }
                            //     }
                            //     catch (Exception e)
                            //     {
                            //         // Systemfunction.onSaveLog_SystemError("1", "MoveFileToSharePath", e.Message, e.StackTrace, "");
                            //         result.IsCompleted = false;
                            //     }
                            // }
                            #endregion

                            result.nID = 0;
                            result.sSaveToFileName = sSysFileName;
                            result.sFileName = sFileName;
                            result.sFileLink = sFileLink;
                            result.IsNewFile = true;
                            result.IsDelete = false;
                            result.sFileType = sFileType;
                            result.sPath = filepath;
                            result.sMsg = "";

                        }
                    }
                }
            }

            return result;
        }

        public ResultFile DownloadFileInSharePath(FilterFile objsPathFile)
        {
            ResultFile res = new ResultFile();
            byte[] buffer;
            if (objsPathFile != null && !string.IsNullOrEmpty(objsPathFile.sPathFile))
            {
                try
                {
                    if (IsLogonSharePath == "Y" && NetworkConnection.RevertToSelf())
                    {
                        using (new NetworkConnection(networkPath, credentials))
                        {
                            using (FileStream fileStream = new FileStream(Path.Combine(networkPath, Path.Combine(objsPathFile.sPathFile + "")), FileMode.Open, FileAccess.Read))
                            {
                                var arrFilr = objsPathFile.sPathFile.Split(".");
                                if (arrFilr.Length > 1)
                                {
                                    res.sFileType = arrFilr[arrFilr.Length - 1];
                                }
                                // res.sFileType = fileStream.
                                int length = (int)fileStream.Length;  // get file length
                                buffer = new byte[length];            // create buffer
                                int count;                            // actual number of bytes read
                                int sum = 0;                          // total number of bytes read)
                                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                                    sum += count;
                            }
                            res.objFile = buffer;
                        }
                    }
                    else
                    {
                        throw new DirectoryNotFoundException($"Is Logon SharePathd: {false}");
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        using (FileStream fileStream = new FileStream(Path.Combine(_hostingEnvironment.ContentRootPath + @"wwwroot", Path.Combine(objsPathFile.sPathFile + "")), FileMode.Open, FileAccess.Read))
                        {
                            var arrFilr = objsPathFile.sPathFile.Split(".");
                            if (arrFilr.Length > 1)
                            {
                                res.sFileType = arrFilr[arrFilr.Length - 1];
                            }
                            // res.sFileType = fileStream.
                            int length = (int)fileStream.Length;  // get file length
                            buffer = new byte[length];            // create buffer
                            int count;                            // actual number of bytes read
                            int sum = 0;                          // total number of bytes read)
                            while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                                sum += count;
                        }
                        res.objFile = buffer;
                    }
                    catch (Exception e)
                    {

                    }

                }
            }

            return res;
        }

        /// <summary>
        /// Move File To SharePath
        /// </summary>
        /// <param name="sDirectorySourceFile">Path ต้นทาง</param>
        /// <param name="sSourceFileName">ชื่อไฟล ต้นทาง</param>
        /// <param name="sDestinationDirectory">Path ปลายทาง</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public ResultAPISharePath MoveFileToSharePath(string sDirectorySourceFile, string sSourceFileName, string sDestinationDirectory)
        {
            ResultAPISharePath result = new ResultAPISharePath();
            bool isToSharePath = false;
            #region SharePath
            if (IsLogonSharePath == "Y" && NetworkConnection.RevertToSelf())
            {
                try
                {
                    using (new NetworkConnection(networkPath, credentials))
                    {
                        string sPathSource = Path.Combine(networkPath + @"\" + sDirectorySourceFile.Replace("//", @"\").Replace("/", @"\"));
                        string sPathDestination = Path.Combine(networkPath + @"\" + sDestinationDirectory.Replace("/", @"\"));

                        var dirSource = new DirectoryInfo(sPathSource);
                        if (!dirSource.Exists)
                        {
                            isToSharePath = true;
                            throw new DirectoryNotFoundException($"Source directory not found: {dirSource.FullName}");
                        }

                        var dirDestination = new DirectoryInfo(sPathDestination);
                        if (!dirDestination.Exists)
                        {
                            dirDestination.Create();
                        }
                        foreach (FileInfo file in dirSource.GetFiles(sSourceFileName))
                        {
                            string targetFilePath = Path.Combine(Path.Combine(sPathDestination), file.Name);
                            file.MoveTo(targetFilePath);
                            result.sPath = sDestinationDirectory;
                            result.sFileName = file.Name;
                            result.Status = 200;
                            isToSharePath = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    // Systemfunction.onSaveLog_SystemError("1", "MoveFileToSharePath", e.Message, e.StackTrace, "");
                    result.Status = 500;
                    result.Message = e.Message;
                }
            }
            #endregion

            #region Local
            if (!isToSharePath)
            {
                try
                {
                    string sPathSource = Path.Combine(_hostingEnvironment.ContentRootPath + @"wwwroot\" + sDirectorySourceFile.Replace("//", @"\").Replace("/", @"\"));
                    string sPathDestination = Path.Combine(_hostingEnvironment.ContentRootPath + @"wwwroot\UploadFile\" + sDestinationDirectory.Replace("//", @"\").Replace("/", @"\"));
                    var dirSource = new DirectoryInfo(sPathSource);

                    if (!dirSource.Exists)
                    {
                        throw new DirectoryNotFoundException($"Source directory not found: {dirSource.FullName}");
                    }
                    var dirDestination = new DirectoryInfo(sPathDestination);
                    if (!dirDestination.Exists)
                    {
                        dirDestination.Create();
                    }
                    foreach (FileInfo file in dirSource.GetFiles(sSourceFileName))
                    {
                        string targetFilePath = Path.Combine(Path.Combine(sPathDestination), file.Name);
                        file.MoveTo(targetFilePath);
                        result.sPath = "UploadFile/" + sDestinationDirectory.Replace(@"\\", @"/").Replace(@"\", @"/");
                        result.sFileName = file.Name;
                        result.Status = 200;
                    }
                }
                catch (Exception e)
                {
                    // Systemfunction.onSaveLog_SystemError("1", "MoveFileToSharePath/Local", e.Message, e.StackTrace, "");
                    result.Status = 500;
                    result.Message = e.Message;
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Copy File SharePath ToLocal
        /// </summary>
        /// <param name="sDirectorySourceFile">Path ต้นทาง</param>
        /// <param name="sSourceFileName">ชื่อไฟล ต้นทาง</param>
        /// <param name="sDestinationDirectory">Path ปลายทาง</param>
        /// <returns></returns>
        public ResultAPISharePath CopyFileSharePathToLocal(string sDirectorySourceFile, string sSourceFileName, string sDestinationDirectory)
        {
            ResultAPISharePath result = new ResultAPISharePath();
            bool isToSharePath = false;

            #region SharePath
            if (IsLogonSharePath == "Y" && NetworkConnection.RevertToSelf())
            {
                try
                {
                    using (new NetworkConnection(networkPath, credentials))
                    {
                        string sPathDestination = Path.Combine(_hostingEnvironment.ContentRootPath + @"wwwroot\" + sDestinationDirectory.Replace("//", @"\").Replace("/", @"\"));
                        string sPathSource = Path.Combine(networkPath + @"\" + sDirectorySourceFile.Replace("/", @"\"));
                        var dirSource = new DirectoryInfo(sPathSource);
                        if (!dirSource.Exists)
                        {
                            isToSharePath = true;
                            throw new DirectoryNotFoundException($"Source directory not found: {dirSource.FullName}");
                        }

                        var dirDestination = new DirectoryInfo(sPathDestination);
                        if (!dirDestination.Exists)
                        {
                            dirDestination.Create();
                        }
                        var e = dirSource.GetFiles(sSourceFileName);
                        foreach (FileInfo file in dirSource.GetFiles(sSourceFileName))
                        {
                            string targetFilePath = Path.Combine(Path.Combine(sPathDestination), file.Name);
                            file.CopyTo(targetFilePath);
                            result.sFullPath = targetFilePath;
                            result.sFileName = file.Name;
                            result.sPath = sDestinationDirectory;
                            result.Status = 200;
                            isToSharePath = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    // Systemfunction.onSaveLog_SystemError("1", "CopyFileSharePathToLocal", e.Message, e.StackTrace, "");
                    result.Status = 500;
                    result.Message = e.Message;
                }
            }
            #endregion

            #region Local
            if (!isToSharePath)
            {
                try
                {
                    string sPathDestination = Path.Combine(_hostingEnvironment.ContentRootPath + @"wwwroot\" + sDestinationDirectory.Replace("//", @"\").Replace("/", @"\"));
                    string sPathSource = Path.Combine(_hostingEnvironment.ContentRootPath + @"wwwroot\" + sDirectorySourceFile.Replace("/", @"\"));
                    var dirSource = new DirectoryInfo(sPathSource);
                    if (!dirSource.Exists)
                    {
                        throw new DirectoryNotFoundException($"Source directory not found: {dirSource.FullName}");
                    }

                    var dirDestination = new DirectoryInfo(sPathDestination);
                    if (!dirDestination.Exists)
                    {
                        dirDestination.Create();
                    }
                    var e = dirSource.GetFiles(sSourceFileName);
                    foreach (FileInfo file in dirSource.GetFiles(sSourceFileName))
                    {
                        string targetFilePath = Path.Combine(Path.Combine(sPathDestination), file.Name);
                        file.CopyTo(targetFilePath);
                        result.sFullPath = targetFilePath;
                        result.sFileName = file.Name;
                        result.sPath = sDestinationDirectory;
                        result.Status = 200;
                        isToSharePath = true;
                    }
                }
                catch (Exception e)
                {
                    // Systemfunction.onSaveLog_SystemError("1", "CopyFileSharePathToLocal/local", e.Message, e.StackTrace, "");
                    result.Status = 500;
                    result.Message = e.Message;
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Delet File
        /// </summary>
        /// <param name="sDirectoryFile"></param>
        /// <param name="sfileName"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public ResultAPISharePath DeletFileInLocal(string sDirectoryFile, string sfileName)
        {
            ResultAPISharePath result = new ResultAPISharePath();
            try
            {
                string sPath = Path.Combine(_hostingEnvironment.ContentRootPath + @"wwwroot\" + sDirectoryFile.Replace("//", @"\").Replace("/", @"\"));
                var dir = new DirectoryInfo(sPath);
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
                }

                foreach (FileInfo file in dir.GetFiles(sfileName))
                {
                    file.Delete();
                    result.Status = 200;
                }
            }
            catch (Exception e)
            {
                result.Status = 500;
                result.Message = e.Message;
            }
            return result;
        }

        public ResultAPISharePath DeletFileInSharePath(string sPath)
        {
            ResultAPISharePath result = new ResultAPISharePath();
            bool isToSharePath = false;

            #region SharePath
            if (IsLogonSharePath == "Y" && NetworkConnection.RevertToSelf())
            {
                try
                {
                    using (new NetworkConnection(networkPath, credentials))
                    {
                         sPath = STFunction.Scan_CWE22_FullPathFile(sPath);

                    
                        if(File.Exists(sPath))
                        {
                        File.Delete(sPath);
                        }
                       
                        isToSharePath = true;
                        result.Status = 200;

                    }
                }
                catch (Exception e)
                {
                    result.Status = 500;
                    result.Message = e.Message;
                }
            }
            #endregion

            return result;
        }

        public ResultAPISharePath DeletFileInSharePathList(string sDirectoryFile, List<string> lstfileName)
        {
            ResultAPISharePath result = new ResultAPISharePath();
            bool isToSharePath = false;

            #region SharePath
            if (IsLogonSharePath == "Y" && NetworkConnection.RevertToSelf())
            {
                try
                {
                    using (new NetworkConnection(networkPath, credentials))
                    {
                        string sPath = Path.Combine(networkPath + @"\" + sDirectoryFile.Replace("/", @"\"));

                        var dir = new DirectoryInfo(sPath);
                        if (!dir.Exists)
                        {
                            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
                        }

                        foreach (FileInfo file in dir.GetFiles())
                        {
                            if (lstfileName.Where(w => w == file.Name).Count() == 0)
                            {
                                file.Delete();
                            }
                        }
                        isToSharePath = true;
                        result.Status = 200;

                    }
                }
                catch (Exception e)
                {
                    result.Status = 500;
                    result.Message = e.Message;
                }
            }
            #endregion

            #region Local
            if (!isToSharePath)
            {
                try
                {
                    string sPath = Path.Combine(_hostingEnvironment.ContentRootPath + @"wwwroot\" + sDirectoryFile.Replace("/", @"\"));
                    var dir = new DirectoryInfo(sPath);
                    if (!dir.Exists)
                    {
                        throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
                    }

                    foreach (FileInfo file in dir.GetFiles())
                    {
                        if (!lstfileName.Any(w => w == file.Name))
                        {
                            file.Delete();
                        }
                    }
                    isToSharePath = true;
                    result.Status = 200;

                }
                catch (Exception e)
                {
                    result.Status = 500;
                    result.Message = e.Message;
                }
            }
            #endregion

            return result;
        }

    }
    public class ResultFile
    {
        public byte[]? objFile { get; set; }
        public string? sFileType { get; set; }
        public string? sFileName { get; set; }
    }
    public class FilterFile
    {
        public string? sPathFile { get; set; }
    }
    public class ResultAPISharePath : ResultAPI
    {
        public string? sPath { get; set; }
        public string? sFullPath { get; set; }
        public string? sFileName { get; set; }
        public object? result2 { get; set; }
    }
}
