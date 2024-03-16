using Microsoft.AspNetCore.Mvc;
using Extensions.Systems;
using Backend.Service.UploadFile;
using Backend.Service.UploadFileSharePath;


namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UploadFileController : ControllerBase
    {
        private readonly IHostEnvironment _env;
        private readonly IUploadToSharePathSevice _IUploadToSharePathSevice;
        private readonly IUploadFileService _uploadfileservice;
        public UploadFileController(IHostEnvironment env, IUploadFileService uploadfileservice, IUploadToSharePathSevice iUploadToSharePathSevice)
        {
            _env = env;
            _uploadfileservice = uploadfileservice;
            _IUploadToSharePathSevice = iUploadToSharePathSevice;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFileToTemp(string? modeUpload, string? sFolderTemp, bool? isResize, int? nWidthResize, int? nHeigthResize, int nIndex, bool? IsCheckRecommendSize)
        {
            try
            {
                AllClass.ItemFileData? data = await _uploadfileservice.UploadFileToTemp(modeUpload, sFolderTemp, isResize, nWidthResize, nHeigthResize, nIndex, IsCheckRecommendSize);
                return Ok(data);
            }
            catch (Exception error)
            {
                return StatusCode(500, new
                {
                    result = "",
                    message = error
                });
            }
        }

        /// <summary>
        /// Crop Image after open is Crop 
        /// </summary>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> CropImageUploadFile(AllClass.cParamCrop Param)
        {
            try
            {
                AllClass.ItemFileData? data = await _uploadfileservice.CropImageUploadFile(Param);
                return Ok(data);
            }
            catch (Exception error)
            {
                return StatusCode(500, new
                {
                    result = "",
                    message = error
                });
            }
        }


        /// <summary>
        /// Delete File
        /// </summary>
        [HttpPost]
        public IActionResult DeleteInTemp(string? sPath, string? sMode)
        {
            if (!string.IsNullOrEmpty(sPath))
            {
                _uploadfileservice.deleteFile(sPath, sMode, _env);
            }
            return Ok();
        }

        /// <summary>
        /// The DownloadFileInSharePath function for SharePath
        /// </summary>
        [HttpPost]
        public IActionResult DownloadFileInSharePath(FilterFile objsPathFile)
        {
            var model = _IUploadToSharePathSevice.DownloadFileInSharePath(objsPathFile);
            if (model.objFile != null)
            {
                return File(model.objFile, "octet/stream", "Download." + model.sFileType);
            }
            else
            {
                return StatusCode(404, null);
            }
        }
    }
}