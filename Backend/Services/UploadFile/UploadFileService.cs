using Extensions.Common.STFunction;
using Extensions.Systems;
using Backend.Interfaces.Authentication;
using SkiaSharp;
using System.Net;


namespace Backend.Service.UploadFile
{

    /// <summary>interface</summary>
    public interface IUploadFileService
    {
        /// <summary>UploadFile</summary>
        Task<AllClass.ItemFileData> UploadFileToTemp(string? modeUpload, string? sFolderTemp, bool? isResize, int? nWidthResize, int? nHeigthResize, int nIndex, bool? IsCheckRecommendSize);

        /// <summary>Crop Image</summary>
        Task<AllClass.ItemFileData> CropImageUploadFile(AllClass.cParamCrop oParam);
        void deleteFile(string sPath,string? sMode, IHostEnvironment _env);
    }

    /// <summary>
    /// UploadFile Service
    /// </summary>
    public class UploadFileService : IUploadFileService
    {
        private readonly IHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContext;
        public readonly string networkPath = STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathUpFile") + "";
        public readonly string IsLogonSharePath = STFunction.GetAppSettingJson("AppSetting:SharePath:IsLogonSharePath") + "";
        public readonly NetworkCredential credentials = new NetworkCredential(
            STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathUser") + "",
            STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathPassword") + "",
            STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathDomain") + ""
        );
        /// <summary>
        /// UploadFile Service
        /// </summary>
        public UploadFileService(IHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContext = httpContextAccessor;

        }
        /// <summary>
        /// config i18n
        /// ความสูงของภาพไม่ถึงขนาดที่กำหนด
        /// ความกว้างของภาพไม่ถึงขนาดที่กำหนด
        /// ความกว้างและความสูงของภาพไม่ถึงขนาดที่กำหนด
        /// ความสูงของภาพไม่ถึงขนาดที่กำหนด
        /// ความกว้างของภาพไม่ถึงขนาดที่กำหนด
        /// Error: Cannot create directory !
        /// 
        /// </summary>

        public async Task<AllClass.ItemFileData> UploadFileToTemp(string? modeUpload, string? sFolderTemp, bool? isResize, int? nWidthResize, int? nHeigthResize, int nIndex, bool? IsCheckRecommendSize)
        {
            AllClass.ItemFileData? data = new AllClass.ItemFileData();

            if (_httpContext != null && _httpContext.HttpContext != null)
            {
                var HttpContext = _httpContext.HttpContext;
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    string filepath = "Temp" + (!string.IsNullOrEmpty(sFolderTemp) ? "/"+ sFolderTemp : "");
                    for (int i = 0; i < HttpContext.Request.Form.Files.Count; i++)
                    {
                        IFormFile? file = HttpContext.Request.Form.Files[i];
                        if (file != null)
                        {
                            string sFileName, sSysFileName = "";
                            bool isPassResize = true;
                            sFileName = file.FileName;
                            //For Upload Folder
                            string[]? splitName = file.FileName.Contains("/") ? file.FileName.Split("/") : null;
                            string sFileNameFolder = splitName != null && splitName.Any() ? splitName[splitName.Length - 1] : file.FileName;
                            sSysFileName = DateTime.Now.ToString("ddMMyyyyHHmmffffff") + "_s" + sFileNameFolder;
                            //generate file name
                            string[]? arrSplit = sSysFileName.Split("/");
                            if (arrSplit.Count() > 1) // Is Folder
                            {
                                sSysFileName = arrSplit[1];
                                sFileName = arrSplit[1];
                            }

                            //Cut File NameR
                            string[]? arrfilename = sFileName.Split('.');
                            if (sFileName.Length > 100)
                            {
                                sFileName = sFileName.Substring(0, 90) + "." + arrfilename[arrfilename.Length - 1];
                            }

                            //Cut System File Name
                            sSysFileName = sSysFileName.Replace(" ", "_").Replace("(", "").Replace(")", "");
                            string[]? arrSysfilename = sSysFileName.Split('.');
                            string sFileType = arrSysfilename[arrSysfilename.Length - 1];
                            if (sSysFileName.Length > 29)
                            {
                                sSysFileName = sSysFileName.Substring(0, 24) + "." + sFileType;
                            }

                            if (modeUpload == "sharepath" && IsLogonSharePath == "Y" && NetworkConnection.RevertToSelf())
                            {
                                using (new NetworkConnection(networkPath, credentials))
                                {
                                    string pathTempShare = "\\Temp\\" + (!string.IsNullOrEmpty(sFolderTemp) ? sFolderTemp : "");
                                    string sPathDestination = Path.Combine(networkPath + pathTempShare);
                                    var dirDestination = new DirectoryInfo(sPathDestination);
                                    if (!dirDestination.Exists)
                                    {
                                        dirDestination.Create();
                                    }

                                    if (dirDestination.Exists)
                                    {
                                        var sMapPath = Path.Combine(Path.Combine(sPathDestination), sSysFileName);
                                        isPassResize = await OnCheckResizeImage(isResize, nWidthResize, nHeigthResize, IsCheckRecommendSize, data, filepath, sSysFileName, file, sMapPath);
                                        if (isPassResize)
                                        {
                                            using (FileStream stream = new(sMapPath, FileMode.Create))
                                            {
                                                file.CopyTo(stream);
                                                var dirFileLin = new DirectoryInfo(Path.Combine(sPathDestination));
                                                var urlFile = dirFileLin.GetFiles(sSysFileName);
                                                if (urlFile.Length > 0)
                                                {
                                                    data.IsCompleted = true;
                                                    filepath = isPassResize && isResize == true ? filepath + "/Editor" : filepath;
                                                    filepath = filepath.Replace("../", "");

                                                    data.nID = 0;
                                                    data.IsCompleted = true;
                                                    data.sSaveToFileName = sSysFileName;
                                                    data.sUrl = filepath + "/" + sSysFileName;
                                                    data.sFileName = sFileNameFolder;
                                                    data.sFileLink = GetSharePathUploadFile(filepath, sSysFileName);
                                                    data.sCropFileLink = GetSharePathUploadFile(filepath, sSysFileName);
                                                    data.IsNewFile = true;
                                                    data.IsDelete = false;
                                                    data.sFileType = sFileType;
                                                    data.sPath = filepath;
                                                    data.sCropPath = filepath;
                                                    data.sMsg = "";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //check folder and create
                                var path = STFunction.MapPath(filepath, _env);
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }

                                if (Directory.Exists(path))
                                {
                                    var sMapPath = STFunction.MapPath(filepath + "/" + sSysFileName, _env);
                                    if (!File.Exists(sMapPath))
                                    {
                                        //Check Image Only
                                        isPassResize = await OnCheckResizeImage(isResize, nWidthResize, nHeigthResize, IsCheckRecommendSize, data, filepath, sSysFileName, file, sMapPath);
                                        if (isPassResize)
                                        {
                                            using (FileStream? stream = new FileStream(sMapPath, FileMode.Create))
                                            {
                                                file.CopyTo(stream);
                                                filepath = isPassResize && isResize == true ? filepath + "/Editor" : filepath;
                                                filepath = filepath.Replace("../", "");
                                                data.nID = 0;
                                                data.IsCompleted = true;
                                                data.sSaveToFileName = sSysFileName;
                                                data.sUrl = filepath + "/" + sSysFileName;
                                                data.sFileName = sFileNameFolder;
                                                data.sFileLink = GetPathUploadFile(filepath, sSysFileName);
                                                data.sCropFileLink = GetPathUploadFile(filepath, sSysFileName);
                                                data.IsNewFile = true;
                                                data.IsDelete = false;
                                                data.sFileType = sFileType;
                                                data.sPath = filepath;
                                                data.sCropPath = filepath;
                                                data.sMsg = "";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        data.IsCompleted = false;
                                        data.sMsg = "Error: Cannot create directory !";
                                    }
                                }
                                else
                                {
                                    data.IsCompleted = false;
                                    data.sMsg = "Error: Cannot create directory !";
                                }
                            }
                        }
                    }
                }
            }
            return data;
        }

        private async Task<bool> OnCheckResizeImage(bool? isResize, int? nWidthResize, int? nHeigthResize, bool? IsCheckRecommendSize, AllClass.ItemFileData data, string filepath, string sSysFileName, IFormFile file, string sMapPath)
        {
            bool isPassResize = true;
            string sTypefile = Path.GetExtension(sMapPath).ToLower();
            List<string>? lstMimeTypeImage = new List<string>() { ".jpeg", ".jpg", ".png" };
            if (isResize == true && lstMimeTypeImage.Contains(sTypefile))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    using (SKBitmap skBitmap = SKBitmap.Decode(memoryStream))
                    {
                        int nWidth = skBitmap.Width;
                        int nHeigth = skBitmap.Height;

                        int nWidth_new = nWidth;
                        int nHeight_new = nHeigth;

                        // Cal width heigth
                        if (nWidthResize.HasValue && !nHeigthResize.HasValue)
                        {
                            nWidth_new = nWidthResize.Value;
                            nHeight_new = nWidth_new != 0 ? (nHeigth * nWidth_new) / nWidth : nHeigth;
                        }
                        else if (!nWidthResize.HasValue && nHeigthResize.HasValue)
                        {
                            nHeight_new = nHeigthResize.Value;
                            nWidth_new = nHeight_new != 0 ? (nWidth * nHeight_new) / nHeigth : nWidth;
                        }
                        else if (nWidthResize.HasValue && nHeigthResize.HasValue)
                        {
                            nWidth_new = nWidthResize.Value;
                            nHeight_new = nHeigthResize.Value;
                        }


                        if (nHeigthResize.HasValue)
                        {
                            //vertical
                            if (nHeigthResize > nHeigth)
                            {
                                isPassResize = false;
                                data.IsCompleted = false;
                                data.sMsg = "ความสูงของภาพไม่ถึงขนาดที่กำหนด";
                            }
                            else
                            {
                                SKBitmap? newImage = ResizeBitmap(skBitmap, nWidth_new, nHeight_new);
                                OnSaveToPath(sSysFileName, filepath, newImage);
                            }
                        }
                        else if (nWidthResize.HasValue && nWidthResize > nWidth)
                        {
                            //horizontal
                            isPassResize = false;
                            data.IsCompleted = false;
                            data.sMsg = "ความกว้างของภาพไม่ถึงขนาดที่กำหนด";
                            if (nWidthResize > nWidth)
                            {
                                isPassResize = false;
                                data.IsCompleted = false;
                                data.sMsg = "ความกว้างของภาพไม่ถึงขนาดที่กำหนด";
                            }
                            else
                            {
                                SKBitmap? newImage = ResizeBitmap(skBitmap, nWidth_new, nHeight_new);
                                OnSaveToPath(sSysFileName, filepath, newImage);
                            }
                        }
                        else
                        {
                            SKBitmap? newImage = ResizeBitmap(skBitmap, nWidth_new, nHeight_new);
                            OnSaveToPath(sSysFileName, filepath, newImage);
                        }
                    }

                }
            }
            else if (IsCheckRecommendSize.HasValue)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    using (SKBitmap skBitmap = SKBitmap.Decode(memoryStream))
                    {
                        int nWidth = skBitmap.Width;
                        int nHeigth = skBitmap.Height;


                        if (nWidthResize.HasValue && nHeigthResize.HasValue && nHeigthResize > nHeigth && nWidthResize > nWidth)
                        {
                            isPassResize = false;
                            data.IsCompleted = false;
                            data.sMsg = "ความกว้างและความสูงของภาพไม่ถึงขนาดที่กำหนด";
                        }
                        else if (nHeigthResize.HasValue && nHeigthResize > nHeigth)
                        {
                            //vertical
                            if (nHeigthResize > nHeigth)
                            {
                                isPassResize = false;
                                data.IsCompleted = false;
                                data.sMsg = "ความสูงของภาพไม่ถึงขนาดที่กำหนด";
                            }
                        }
                        else if (nWidthResize.HasValue && nWidthResize > nWidth)
                        {
                            //horizontal
                            isPassResize = false;
                            data.IsCompleted = false;
                            data.sMsg = "ความกว้างของภาพไม่ถึงขนาดที่กำหนด";
                        }
                    }
                }
            }

            return isPassResize;
        }


        public Task<AllClass.ItemFileData> CropImageUploadFile(AllClass.cParamCrop oParam)
        {
            AllClass.ItemFileData? result = new AllClass.ItemFileData();
            try
            {
                if (!string.IsNullOrEmpty(oParam.sOldPath) && !string.IsNullOrEmpty(oParam.sBase64))
                {
                    string[]? arrPath = oParam.sOldPath.Split("/");
                    string sSystemName = arrPath[arrPath.Length - 1];
                    string sPathFolder = oParam.sOldPath.Replace(sSystemName, "") + "Crop";

                    var path = STFunction.MapPath(sPathFolder, _env);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (Directory.Exists(path))
                    {
                        var sMapPath = STFunction.MapPath(sPathFolder + "/" + sSystemName, _env);
                        if (File.Exists(sMapPath))
                        {
                            File.Delete(sMapPath);
                        }
                        string? base64Image = oParam.sBase64.Split(",").Length > 0 ? oParam.sBase64.Split(",")[1] : null;
                        if (!string.IsNullOrEmpty(base64Image))
                        {
                            byte[] imageBytes = Convert.FromBase64String(base64Image);
                            // Create SKBitmap from the bytes
                            using SKBitmap skBitmap = SKBitmap.Decode(imageBytes);
                            OnSaveToPath(sSystemName, sPathFolder, skBitmap);
                            string filepath = sPathFolder.Replace("../", "");

                            result.IsCompleted = true;
                            result.sUrl = filepath + "/" + sSystemName;
                            result.sCropFileLink = GetPathUploadFile(filepath, sSystemName);
                            result.sCropPath = filepath;
                            result.sMsg = "";
                        }
                    }
                    else
                    {
                        result.IsCompleted = false;
                        result.sMsg = "Error: Cannot create directory !";
                    }
                }

            }
            catch (Exception ex)
            {
                result.sMsg = ex.Message + "";
            }
            return Task.FromResult(result);
        }

        private void OnSaveToPath(string sSysFileName, string sPathFolder, SKBitmap? skBitmap)
        {
            if (skBitmap != null)
            {
                string sPathFolderEditor = sPathFolder.Replace("/","\\");
                var path = STFunction.MapPath(sPathFolderEditor, _env);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string? sMapPathEditor = STFunction.MapPath(path + sSysFileName, _env);
                // Save the SKBitmap as an image
                using (var image = SKImage.FromBitmap(skBitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var fileStream = new FileStream(sMapPathEditor, FileMode.Create))
                {
                    data.SaveTo(fileStream);

                    string filepath = sPathFolder.Replace("../", "");
                }
            }
        }

        /// <summary>
        /// Get path File for display
        /// </summary>
        /// <param name="FilePath">Path file</param>
        /// <param name="SysFileName">ชื่อไฟล์ในระบบ</param>
        public static string? GetSharePathUploadFile(string FilePath, string SysFileName)
        {
            string? sFullPath = null;
            if (!string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(SysFileName))
            {
                sFullPath = FilePath + "/" + SysFileName;
            }
            return sFullPath;
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
                string sPathWeb = STFunction.GetAppSettingJson("AppSetting:UrlSiteBackend");
                sFullPath = sPathWeb + "UploadFile/" + FilePath + "/" + SysFileName;
            }
            return sFullPath;
        }

        /// <summary>
        /// ใช้สำหรับการ Resize Image
        /// </summary>
        /// <param name="bmp">ภาพ Bitmap</param>
        /// <param name="width">ขนาดกว้างของภาพที่ต้องการ Resize</param>
        /// <param name="height">ขนาดความสูงของภาพที่ต้องการ Resize</param>

        public static SKBitmap? ResizeBitmap(SKBitmap bmp, int? width, int? height)
        {
            int newWidth = 0;
            int newHeight = 0;
            int nWidth_bmp = bmp.Width;
            int nHeigth_bmp = bmp.Height;
            if (width.HasValue && height.HasValue)
            {
                newWidth = width.Value;
                newHeight = height.Value;
            }
            else if (width.HasValue && !height.HasValue)
            {
                newWidth = width.Value;
                newHeight = nWidth_bmp != 0 ? nHeigth_bmp * newWidth / nWidth_bmp : nHeigth_bmp;
            }
            else if (!width.HasValue && height.HasValue)
            {
                newWidth = nHeigth_bmp != 0 ? nWidth_bmp * newHeight / nHeigth_bmp : nWidth_bmp;
                newHeight = height.Value;
            }
            // Create the SKImageInfo for the target size and color type
            SKImageInfo targetInfo = new SKImageInfo(newWidth, newHeight, bmp.Info.ColorType, bmp.Info.AlphaType);

            // Choose the filter quality for resizing
            SKFilterQuality filterQuality = SKFilterQuality.High;

            // Perform the resize operation
            using SKBitmap resized = bmp.Resize(targetInfo, filterQuality);

            return resized;
        }

        private static int nCountDel = 0; //จำนวนครั้งที่ Delete ไม่ได้เนื่องจากไฟล์ถูก Read อยู่
        public void deleteFile(string sPath, string? sMode, IHostEnvironment _env)
        {
            try
            {
                if (sMode == "sharepath" && IsLogonSharePath == "Y" && NetworkConnection.RevertToSelf())
                {
                    using (new NetworkConnection(networkPath, credentials))
                    {
                        sPath = Path.Combine(networkPath + @"\" + sPath.Replace("/", @"\"));
                        if (!string.IsNullOrEmpty(sPath) && File.Exists(sPath))
                        {
                            File.Delete(sPath);
                        }
                    }
                }
                else
                {
                    var path = STFunction.MapPath(sPath, _env);
                    if (nCountDel <= 10)
                    {
                        sPath = STFunction.Scan_CWE22_FullPathFile(path);
                        if (!string.IsNullOrEmpty(sPath) && File.Exists(sPath))
                        {
                            File.Delete(sPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message.ToString();
                if (x.Contains("The process cannot access the file"))
                {
                    nCountDel++;
                    Thread.Sleep(1000);
                    deleteFile(sPath, sMode, _env);
                }
            }
        }

    }
}
