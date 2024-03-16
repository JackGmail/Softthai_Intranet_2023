using Backend.EF.ST_Intranet;
using Backend.Interfaces;
using Backend.Interfaces.Authentication;
using Backend.Models;
using Backend.Models.Authentication;
using Extensions.Common.STFunction;
using ST.INFRA;
using ST.INFRA.Common;
using System.Data;
using static Backend.Enum.EnumStandard;
using static Extensions.Systems.AllClass;

namespace Backend.Service
{
    /// <summary>
    /// </summary>
    public class AdminBannerService : IAdminBannerSevice
    {
         private readonly IAuthentication _authen;
        private readonly ST_IntranetEntity _db;
        private readonly IHostEnvironment _env;
        /// <summary>
        /// </summary>
        public AdminBannerService(IAuthentication authen, ST_IntranetEntity db, IHostEnvironment env)
        {
            _db = db;
            _authen = authen;
            _env = env;
        }
        /// <summary>
        /// </summary>
        public ClsResultTableBanner GetDataTable(ClsFilterBanner obj)
        {
            ClsResultTableBanner result = new ClsResultTableBanner();
            try
            {
                string? sTitle = null;
                List<int>? listActive = new List<int>();
                List<bool> listStatus = new List<bool>();

                listActive = !string.IsNullOrEmpty(obj.nStatus) && obj.nStatus != null  ? obj.nStatus.Split(',').Select(s => s.ToInt()).ToList() : new List<int>() { (int)Status.Active, (int)Status.InActive };
                sTitle = obj.sTitle;

                listActive.ForEach(f =>
                {
                    bool IsActive = f == 1;
                    listStatus.Add(IsActive);
                });

                List<ObjResultTableBanner> listBanner = (from a in _db.TB_Banner.Where(w => !w.IsDelete && listStatus.Contains(w.IsActive) && (sTitle == null || w.sBannerName.Trim().ToLower().Contains(sTitle.Trim().ToLower())))
                                                         from b in _db.TB_Employee.Where(w => w.nEmployeeID == a.nUpdateBy).DefaultIfEmpty()
                                                         select new ObjResultTableBanner
                                                         {
                                                             sID = a.nBannerID,
                                                             sPRBanner = a.sBannerName,
                                                             dStart = a.sStartDate,
                                                             dEnd = a.sEndDate,
                                                             sUpdateby = b != null ? b.sNameTH + " " + b.sSurnameTH : null,
                                                             dLastUpdate = a.dUpdate,
                                                             IsStatus = a.IsActive,
                                                             nOrder = a.nOrder
                                                         }).OrderBy(o => o.nOrder).ToList();

                #region//SORT
                string sSortColumn = !string.IsNullOrEmpty(obj.sSortExpression) ? obj.sSortExpression : "";
                switch (sSortColumn)
                {
                    case "sPRBanner": sSortColumn = "sPRBanner"; break;
                    case "No": sSortColumn = "nOrder"; break;
                    case "slastUpdate": sSortColumn = "dLastUpdate"; break;
                    case "sSEDate": sSortColumn = "dStart"; break;
                }
                if (obj.sSortDirection == "asc")
                {
                    listBanner = listBanner.OrderBy<ObjResultTableBanner>(sSortColumn).ToList();
                }
                else if (obj.sSortDirection == "desc")
                {
                    listBanner = listBanner.OrderByDescending<ObjResultTableBanner>(sSortColumn).ToList();
                }
                int i = 1;
                foreach (ObjResultTableBanner? item in listBanner)
                {
                    item.No = i;
                    item.nOrder = i;
                    i++;
                }
                #endregion
                result.nOrder = listBanner.Any() ? listBanner.OrderByDescending(m => m.No).First(f => f.No != null).No ?? 1 : 1;
                STGrid.Pagination? dataPage = STGrid.Paging(obj.nPageSize, obj.nPageIndex, listBanner.Count);
                List<ObjResultTableBanner>? lstDataTable = listBanner.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();

                result.lstData = lstDataTable.OrderBy(o => o.nOrder).ToList();
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception ex)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;
            }

            return result;

        }
        /// <summary>
        /// </summary>

        public ResultAPI OnSave(ObjSave bannerSave)
        {
            ResultAPI result = new();
            UserAccount?  ua = _authen.GetUserAccount();
            try
            {
                int nUserID = ua.nUserID;
                string sBannerName = bannerSave.sBannerName;
                string? sNote = !string.IsNullOrEmpty(bannerSave.sNote) ? bannerSave.sNote : null;
                DateTime? dDateEnd = !string.IsNullOrEmpty(bannerSave.sEnd) ? bannerSave.sEnd.ToDateTimeFromStringNullAble(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmmss, ST.INFRA.Enum.CultureName.en_US) : null;
                DateTime? dDateStart = !string.IsNullOrEmpty(bannerSave.sStart) ? bannerSave.sStart.ToDateTimeFromStringNullAble(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmmss, ST.INFRA.Enum.CultureName.en_US) : null;
                bool IsStatus = bannerSave.IsStatus;
                bool IsAllDay = bannerSave.IsAllDay;
                int? nID = bannerSave.sID.ToIntOrNull();
                List<ItemFileData> fFile = bannerSave.fFile ?? new();

                DateTime currentDate = DateTime.Now;
                IQueryable<TB_Banner>? TBBanner = _db.TB_Banner.Where(w => !w.IsDelete).AsQueryable();
                TB_Banner? oBanner = TBBanner.FirstOrDefault(f => f.nBannerID == nID);
                int newID = _db.TB_Banner.Any() ? _db.TB_Banner.Select(s => s.nBannerID).Max() + 1 : 1;
                int newOrder = TBBanner.Any() ? TBBanner.Where(w => !w.IsDelete).Select(s => s.nOrder).Max() + 1 : 1;
                if (!string.IsNullOrEmpty(sBannerName) && dDateStart != null && fFile.Any())
                {
                    if (oBanner == null)
                    {
                        oBanner = new TB_Banner()
                        {
                            dCreate = currentDate,
                            nCreateBy = nUserID,
                            IsDelete = false,
                            nBannerID = newID,
                            nOrder = newOrder,
                        };
                        _db.TB_Banner.Add(oBanner);
                    }
                    #region check ซ้ำ
                    TBBanner = TBBanner.Where(w => w.sBannerName == sBannerName);
                    bool isExist = TBBanner.Any(f => f.nBannerID != oBanner.nBannerID);
                    if (isExist)
                    {
                        result.nStatusCode = StatusCodes.Status409Conflict;
                        result.sMessage = "Duplicates data";
                        return result;
                    }
                    #endregion
                    oBanner.sBannerName = sBannerName;
                    oBanner.sNote = sNote;

                    oBanner.sStartDate = dDateStart;
                    oBanner.sEndDate = dDateEnd;
                    oBanner.IsActive = IsStatus;
                    oBanner.IsSetDate = IsAllDay;
                    oBanner.dUpdate = DateTime.Now;
                    oBanner.nUpdateBy = nUserID;
                    List<ItemFileData> lstAll = fFile.ToList();
                    lstAll.ForEach(item =>
                    {
                        FileItem? listFile = GetFileNew(item, oBanner);
                        oBanner.sPath = listFile.sPath;
                        oBanner.sSystemFileName = item.sSysFileName ?? "";
                        oBanner.sFileName = item.sFileName ?? "";
                    });

                    _db.SaveChanges();
                }
                else
                {
                    result.sMessage = "param is Empty";
                }
                result.nStatusCode = StatusCodes.Status200OK;


            }
            catch (Exception ex)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = ex.Message;
            }
            return result;

        }

        /// <summary>
        /// </summary>
        public FileItem GetFileNew(ItemFileData item, TB_Banner oBanner)
        {
            FileItem result = new();

            item.sFolderName = !string.IsNullOrEmpty(item.sFolderName) ? item.sFolderName + "/" : null;
            string pathTempContent = !string.IsNullOrEmpty(item.sFolderName) ? item.sFolderName : "Temp/AdminBannerTemp/";
            string sTmp = "\\";
            string truePathFile = "AdminBanner\\" + oBanner.nBannerID + sTmp;
            string fPathContent = "AdminBanner/" + oBanner.nBannerID;

            item.sFolderName = pathTempContent;
            if (item.IsNew)
            {
                STFunction.MoveFile(pathTempContent, truePathFile, item.sSysFileName ?? "", _env);
                //DO NOT DELETE
            }

            result.sPath = fPathContent;

            return result;

        }


        /// <summary>
        /// </summary>
        public ObjSave GetDataBanner(string sID)
        {
            ObjSave result = new ObjSave();
            try
            {
                IQueryable<TB_Banner>? TBBanner = _db.TB_Banner.AsQueryable();
                int nID = sID.ToInt();
                TB_Banner? oBanner = nID != 0 ? TBBanner.FirstOrDefault(f => f.nBannerID == nID) : null;
                if (oBanner != null)
                {
                    result.sBannerName = oBanner.sBannerName;
                    result.sNote = oBanner.sNote;
                    result.dStart = oBanner.sStartDate;
                    result.dEnd = oBanner.sEndDate;
                    result.IsStatus = oBanner.IsActive;
                    result.IsAllDay = oBanner.IsSetDate;

                    List<ItemFileData> lstfile = new List<ItemFileData>();
                    string? type = !string.IsNullOrEmpty(oBanner.sSystemFileName) ? oBanner.sSystemFileName.Substring(oBanner.sSystemFileName.LastIndexOf('.') + 1) : null;

                    if (oBanner.sPath != null)
                    {
                        //DO NOT DELETE
                        ItemFileData fFile = new ItemFileData();
                        var sPath = STFunction.GetPathUploadFile(oBanner.sPath, oBanner.sSystemFileName);
                        fFile.sFileName = oBanner.sFileName;
                        fFile.sCropFileLink = sPath;
                        fFile.sFileLink = sPath;
                        fFile.sSysFileName = oBanner.sSystemFileName;
                        fFile.sFileType = type;
                        fFile.sFolderName = oBanner.sPath;
                        fFile.IsNew = false;
                        fFile.IsNewTab = false;
                        fFile.IsCompleted = true;
                        fFile.IsDelete = false;
                        fFile.IsProgress = false;
                        fFile.sProgress = "100";
                        lstfile.Add(fFile);
                    }

                    result.fFile = lstfile;
                    result.sID = oBanner.nBannerID + "";
                    result.nStatusCode = StatusCodes.Status200OK;
                }

            }
            catch (Exception ex)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// </summary>
        public ClsResult OnDelete(ClsFilterDelete bannerFilter)
        {
            ClsResult result = new ClsResult();
            try
            {
                UserAccount?  ua = _authen.GetUserAccount();
                int nUserID = ua.nUserID;

                if (bannerFilter.lstBannerDelete != null)
                {
                    List<TB_Banner> tB_Banners = _db.TB_Banner.Where(w => bannerFilter.lstBannerDelete.Contains(w.nBannerID)).ToList();
                    if (tB_Banners.Any())
                    {
                        foreach (var item in tB_Banners)
                        {
                            item.IsDelete = true;
                            item.nDeleteBy = nUserID;
                            item.dDelete = DateTime.Now;
                        }

                        _db.SaveChanges();

                        tB_Banners = _db.TB_Banner.Where(w => !w.IsDelete).OrderBy(o => o.nOrder).ToList();
                        int nNewOrder = 0;
                        foreach (var item in tB_Banners)
                        {
                            nNewOrder++;
                            item.nOrder = nNewOrder;
                        }
                        _db.SaveChanges();
                    }
                    result.nStatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    result.sMessage = "list Delete empty";
                }

            }
            catch (Exception ex)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = ex.Message;

            }
            return result;
        }
        /// <summary>
        /// </summary>
        public ResultAPI OnChangeOrder(STFunction.ChgFilter bannerFilter)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                if (!string.IsNullOrEmpty(bannerFilter.Column) && !string.IsNullOrEmpty(bannerFilter.Table) && (bannerFilter.nID != 0) && (bannerFilter.NewOrder != 0))
                {
                    result = STFunction.OnChangeOrder(bannerFilter);
                    result.nStatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    result.sMessage = "Filter Change Order empty";
                }

            }
            catch (Exception ex)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = ex.Message;
            }
            return result;
        }

         public ResultAPI BannerActive(ClassTableGroup param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                var objData = _db.TB_Banner.FirstOrDefault(w => w.nBannerID == param.nUserGroupID);
                if (objData != null)
                {
                    objData.IsActive = param.IsActive;
                }
                _db.SaveChanges();
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;

            }
            return result;
        }


    }
}