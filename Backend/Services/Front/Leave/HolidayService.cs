using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST.INFRA;
using ST.INFRA.Common;
using ST_API.Controllers;
using Spire.Doc.Documents;
using System.Linq;
using Backend.EF.ST_Intranet;
using Backend.Enum;
using Backend.Interfaces.Authentication;
using Extensions.Common.STFunction;
using ST_API.Interfaces;
using ST_API.Models.IHolidayService;
using Extensions.Common.STResultAPI;


namespace ST_API.Service
{
    public class HolidayService : IHolidayService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _Auth;
        private readonly IHostEnvironment _env;
        public HolidayService(ST_IntranetEntity db, IAuthentication auth, IHostEnvironment env)
        {
            _db = db;
            _Auth = auth;
            _env = env;
        }

        public ResultAPI SaveDataHoliday(cHolidayYear req)
        {
            cHolidayYear result = new cHolidayYear();
            try
            {
                #region SaveTB_HolidayYear
                int nUserID = 7;
                var objYear = _db.TB_HolidayYear.FirstOrDefault(w => w.nYear == req.nYear);

                if (objYear == null)
                {
                    int newHolidayYearID = (_db.TB_HolidayYear.Any() ? _db.TB_HolidayYear.Max(m => m.nHolidayYearID) : 0) + 1;
                    int newOrder = (_db.TB_HolidayYear.Any() ? _db.TB_HolidayYear.Max(m => m.nOrder) : 0) + 1;
                    // int nHolidayYearID = (req.sID ?? "").DecryptParameter().ToInt();
                    // nHolidayYearID = (_db.TB_HolidayYear.Any() ? _db.TB_HolidayYear.Max(m => m.nHolidayYearID) : 0) + 1;
                    // int newHolidayYearID = nHolidayYearID;
                    int nOrder = _db.TB_HolidayYear.Where(w => w.nHolidayYearID == newHolidayYearID && w.IsDelete != true).Any() ?
                                _db.TB_HolidayYear.Where(w => w.nHolidayYearID == newHolidayYearID && w.IsDelete != true).Select(s => s.nOrder).Max() + 1 : 1;

                    objYear = new TB_HolidayYear();

                    objYear.nHolidayYearID = newHolidayYearID;
                    objYear.nYear = req.nYear;
                    objYear.nOrder = nOrder;

                    objYear.dCreate = DateTime.Now;
                    objYear.nCreateBy = nUserID;

                    _db.TB_HolidayYear.Add(objYear);
                }

                objYear.nHoliday = req.nTotalHoliday;
                objYear.nActivity = req.nTotalActivity;
                objYear.dUpdate = DateTime.Now;
                objYear.nUpdateBy = nUserID;
                objYear.IsDelete = false;


                #endregion

                #region SaveTB_HolidayDay
                _db.TB_HolidayDay.Where(w => w.nYear == objYear.nYear).ToList().ForEach(f =>
                {
                    f.dDelete = DateTime.Now;
                    f.IsDelete = true;
                    f.nDeleteBy = nUserID;
                });
                _db.SaveChanges();

                int nHolidayDayID = (_db.TB_HolidayDay.Any() ? _db.TB_HolidayDay.Max(m => m.nHolidayDayID) : 0) + 1;

                foreach (var item in req.lstHolidayDetails ?? new List<cHolidayDetails>())
                {
                    DateTime dHolidayDate = item.dHolidayDate != null ? item.dHolidayDate : DateTime.Now; // null can't compare
                    var objDay = _db.TB_HolidayDay.FirstOrDefault(w => w.nYear == objYear.nYear && w.dDate.Date == dHolidayDate);
                    if (objDay == null)
                    {
                        objDay = new TB_HolidayDay();

                        objDay.nYear = req.nYear;
                        objDay.nHolidayDayID = nHolidayDayID;
                        objDay.dDate = item.dHolidayDate;

                        objDay.sHolidayName = item.sHolidayName;
                        objDay.nCreateBy = nUserID;
                        objDay.dCreate = DateTime.Now;

                        _db.TB_HolidayDay.Add(objDay);
                        nHolidayDayID++;
                    }

                    objDay.IsActivity = item.IsActivity ?? false;
                    objDay.sDrescription = item.sDescription;
                    objDay.dUpdate = DateTime.Now;
                    objDay.nUpdateBy = nUserID;
                    objDay.IsDelete = false;
                }

                #endregion

                _db.SaveChanges();
                result.Status = StatusCodes.Status200OK;


            }
            catch (System.Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }

        public cFilterHolidayDayTable GetHolidayList(cHolidayData param)
        {
            cFilterHolidayDayTable result = new cFilterHolidayDayTable();

            try
            {
                var objYear = _db.TB_HolidayYear.Where(w => w.nHolidayYearID == param.sID.DecryptParameter().ToInt()).FirstOrDefault();
                if (objYear != null)
                {
                    var lstTableHoliday = _db.TB_HolidayDay.Where(w => !w.IsDelete).ToList();
                    var qry = lstTableHoliday.Where(w => w.nYear == objYear.nYear).Select((s, Index) => new cHolidayData
                    {
                        nID = s.nHolidayDayID,
                        sID = s.nHolidayDayID.EncryptParameter(),
                        sHolidayDate = s.dDate.ToStringFromDate(),
                        dHolidayDate = s.dDate,
                        IsActivity = s.IsActivity,
                        sHolidayName = s.sHolidayName,
                        sActivity = s.IsActivity == true ? "กิจกรรมบริษัท" : "วันหยุดประจำปี",
                        sDescription = s.sDrescription,
                        IsDelete = s.IsDelete
                    }).ToArray();

                    //Dialog Holiday
                    if (param.IsActivity != null)
                    {
                        qry = qry.Where(w => w.IsActivity == param.IsActivity).ToArray();
                        result.nTotalHoliday = qry.Count();
                    }

                    var lstName = qry.FirstOrDefault(w => w.IsActivity == param.IsActivity);
                    if (lstName != null)
                    {
                        result.sNameActivity = lstName.IsActivity == true ? "กิจกรรมบริษัท" : "วันหยุดประจำปี";
                    }

                    #region//SORT
                    string sSortColumn = param.sSortExpression;
                    switch (param.sSortExpression)
                    {
                        case "dHolidayDate": sSortColumn = "dHolidayDate"; break;
                        case "sHolidayName": sSortColumn = "sHolidayName"; break;
                        case "sActivity": sSortColumn = "sActivity"; break;
                        case "sDescription": sSortColumn = "sDescription"; break;
                    }
                    if (param.isASC)
                    {
                        qry = qry.OrderBy<cHolidayData>(sSortColumn).ToArray();
                    }
                    else if (param.isDESC)
                    {
                        qry = qry.OrderByDescending<cHolidayData>(sSortColumn).ToArray();
                    }
                    #endregion

                    #region//Final Action >> Skip , Take And Set Page
                    var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qry.Count());
                    var lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                    result.nYear = objYear.nYear;
                    result.lstData = lstData;
                    result.nDataLength = dataPage.nDataLength;
                    result.nPageIndex = dataPage.nPageIndex;
                    result.nSkip = dataPage.nSkip;
                    result.nTake = dataPage.nTake;
                    result.nStartIndex = dataPage.nStartIndex;
                    #endregion 
                }

                result.Status = StatusCodes.Status200OK;

            }
            catch (System.Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }

        public cReturnHolidayList GetHolidayYearList(cReqHolidayYear param)
        {
            cReturnHolidayList result = new cReturnHolidayList();

            try
            {
                var lstTableYear = _db.TB_HolidayYear.Where(w => !w.IsDelete).ToList();



                var qry = lstTableYear.Select((s, Index) => new cFilterHolidayYear
                {
                    sID = s.nHolidayYearID.EncryptParameter(),
                    nYear = s.nYear,
                    nTotalHoliday = s.nHoliday,
                    nTotalActivity = s.nActivity,

                }).ToArray();

                #region//SORT 
                string sSortColumn = param.sSortExpression;
                switch (param.sSortExpression)
                {
                    case "nYear": sSortColumn = "nYear"; break;
                    case "nTotalHoliday": sSortColumn = "nTotalHoliday"; break;
                    case "nTotalActivity": sSortColumn = "nTotalActivity"; break;
                }
                if (param.isASC)
                {
                    qry = qry.OrderBy<cFilterHolidayYear>(sSortColumn).ToArray();
                }
                else if (param.isDESC)
                {
                    qry = qry.OrderByDescending<cFilterHolidayYear>(sSortColumn).ToArray();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page 
                var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qry.Count());
                var lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();

                result.lstHolidayYear = lstData;
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                #endregion

                result.Status = StatusCodes.Status200OK;

            }
            catch (System.Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        public cHolidayYear GetYearOptionList()
        {
            cHolidayYear result = new cHolidayYear();
            try
            {
                var lstOptionsYear = _db.TB_HolidayYear.ToList();

                result.lstnYear = lstOptionsYear.Select((s, Index) => new cRequestYear
                {
                    value = s.nYear,
                    label = s.nYear.ToString(),

                }).ToList();
                // result.lstYearOptions = result.lstYearOptions;

                result.Status = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }


        public ResultAPI DuplicateHolidayList(cYearList req)
        {
            cHolidayYear result = new cHolidayYear();

            try
            {
                int nUserID = 7;
                var objOldYear = _db.TB_HolidayYear.FirstOrDefault(w => w.nYear == req.nOldYear);
                var objNewYear = new TB_HolidayYear();

                if (objOldYear != null)
                {
                    int newHolidayYearID = (_db.TB_HolidayYear.Any() ? _db.TB_HolidayYear.Max(m => m.nHolidayYearID) : 0) + 1;
                    int nOrder = _db.TB_HolidayYear.Where(w => w.nHolidayYearID == newHolidayYearID && w.IsDelete != true).Any() ?
                                _db.TB_HolidayYear.Where(w => w.nHolidayYearID == newHolidayYearID && w.IsDelete != true).Select(s => s.nOrder).Max() + 1 : 1;

                    //วน Insert New Year
                    foreach (var iNewYear in req.lstNewYear)
                    {
                        var IsCheckDuplicateYear = (_db.TB_HolidayYear.Where(w => w.nYear == iNewYear).Any());
                        if (IsCheckDuplicateYear)
                        {
                            result.Status = StatusCodes.Status200OK;
                            result.Message = "ปีซ้ำ";

                            return result;
                        }

                        objNewYear = new TB_HolidayYear();
                        objNewYear.nHolidayYearID = newHolidayYearID;
                        objNewYear.nYear = iNewYear;
                        objNewYear.nHoliday = objOldYear.nHoliday;
                        objNewYear.nActivity = objOldYear.nActivity;
                        objNewYear.dUpdate = DateTime.Now;
                        objNewYear.nUpdateBy = nUserID;
                        objNewYear.IsDelete = false;
                        objNewYear.nOrder = nOrder;

                        objNewYear.dCreate = DateTime.Now;
                        objNewYear.nCreateBy = nUserID;

                        _db.TB_HolidayYear.Add(objNewYear);

                        newHolidayYearID++;
                        nOrder++;
                    }
                }

                var lstHolidayOldYear = _db.TB_HolidayDay.Where(w => w.nYear == req.nOldYear && !w.IsDelete).ToList();
                int nHolidayDayID = (_db.TB_HolidayDay.Any() ? _db.TB_HolidayDay.Max(m => m.nHolidayDayID) : 0) + 1;

                //วน Insert New Year Holiday
                foreach (var iNewYear in req.lstNewYear)
                {
                    foreach (var iOldYear in lstHolidayOldYear)
                    {
                        var objDay = new TB_HolidayDay();
                        objDay.nYear = iNewYear;
                        objDay.nHolidayDayID = nHolidayDayID;
                        objDay.dDate = iOldYear.dDate.AddYears(iNewYear - iOldYear.nYear);
                        objDay.sHolidayName = iOldYear.sHolidayName;
                        objDay.IsActivity = iOldYear.IsActivity;
                        objDay.nCreateBy = nUserID;
                        objDay.dCreate = DateTime.Now;
                        objDay.sDrescription = iOldYear.sDrescription;
                        objDay.dUpdate = DateTime.Now;
                        objDay.nUpdateBy = nUserID;

                        _db.TB_HolidayDay.Add(objDay);
                        nHolidayDayID++;
                    }
                }

                _db.SaveChanges();
                result.Status = StatusCodes.Status200OK;
            }


            catch (System.Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }

            return result;
        }

        #region ExportPDF 
        public async Task<cExportOutput> ExportPDFHoliday(cHolidayData param)
        {
            cExportOutput result = new cExportOutput();
            try
            {
                string sFileLicensePath = STFunction.MapPath("Spire_License/Spire_Doc/license.elic.xml", _env);
                // Spire.License.LicenseProvider.SetLicenseFileFullPath(sFileLicensePath);

                string sFileName = "Holiday_List";

                int nYear = DateTime.Now.Year;
                cHolidayData objActivity = new cHolidayData();

                objActivity.sID = param.sID;
                objActivity.nPageIndex = 1;
                objActivity.nPageSize = 10000;
                cFilterHolidayDayTable objData = GetHolidayList(objActivity);

                cHolidayData[] lstDataActivities = objData.lstData.Where(w => w.IsActivity == true).ToArray();

                cHolidayData[] lstDataHoliday = objData.lstData.Where(w => w.IsActivity == false).ToArray();

                //Local Function
                string ReplaceHtml(string? sWord)
                {
                    string sWordResult = "";
                    if (!string.IsNullOrEmpty(sWord))
                        sWordResult = sWord.Replace("\n", "<br/>");
                    return sWordResult;
                }

                Spire.Doc.Document document = new Spire.Doc.Document();
                var pSize = new System.Drawing.SizeF(595.3891766f, 841.9560595f);

                STFunction.cReportFont objFont = STFunction.GetReportFont("pdf");

                //Set Font Style and Size
                Spire.Doc.Documents.ParagraphStyle style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "FontStyle";
                style.CharacterFormat.FontName = objFont.sFont;
                style.CharacterFormat.FontSize = objFont.nFontSize;
                document.Styles.Add(style);

                string sCssStyle = @"td, th {
                                border: 2px solid #fff;
                                text-align: left;
                                word-break: break-word;
                                white-space: pre-wrap;
                                padding: 8px;
                            }";

                sCssStyle += @".head-table-sit {
                                    background-color: #29375e;
                                    color: white;
                                    text-align:center;
                                    font-weight:bold;
                                }";

                sCssStyle += @".label-2 {
                                vertical-align:top;
                                font-size: 16pt;
                                background-color:#eceff5;
                                word-break: break-word;
                            }";

                sCssStyle += @".label-3 {
                                vertical-align:top;
                                font-size: 16pt;
                                background-color:#eceff5;
                                text-align:center;
                                }";
                sCssStyle += @".label-nodata {
                                vertical-align:top;
                                font-size: 16pt;
                                background-color:#eceff5;
                                word-break: break-word;  
                                text-align: center;
                            }";

                string sHTMLActivity = "";

                //Head
                sHTMLActivity += @"<table style='width:100%;table-layout: fixed;'>
                                <tr>
                                    <th style='width:10%' class='head-table-sit'>วันที่</th>
                                    <th style='width:30%' class='head-table-sit'>กิจกรรมบริษัท</th>
                                    <th style='width:15%' class='head-table-sit'>ประเภทวันหยุด</th>
                                </tr>";
                //Body
                if (lstDataActivities != null && lstDataActivities.Any())
                {
                    foreach (var iR in lstDataActivities)
                    {
                        //Body

                        sHTMLActivity += "<tr>";
                        sHTMLActivity += @"<td class='label-2'>" + ReplaceHtml(iR.sHolidayDate) + "</td>";
                        sHTMLActivity += @"<td class='label-2'>" + ReplaceHtml(iR.sHolidayName) + "</td>";
                        sHTMLActivity += @"<td class='label-3'>" + ReplaceHtml(iR.sActivity) + "</td>";
                        sHTMLActivity += "</tr>";

                    }
                }
                else
                {
                    // No data
                    sHTMLActivity += "<tr>";
                    sHTMLActivity += @"<td class='label-nodata'>" + "</td>";
                    sHTMLActivity += @"<td class='label-nodata'>" + ReplaceHtml("ไม่มีข้อมูล") + "</td>";
                    sHTMLActivity += @"<td class='label-nodata'>" + "</td>";
                    sHTMLActivity += "</tr>";
                }
                sHTMLActivity += "</table>";
                sHTMLActivity += "<br/>";

                string sHTMLHoliday = "";

                //Head
                sHTMLHoliday += @"<table style='width:100%;table-layout: fixed;'>
                                <tr>
                                    <th style='width:10%' class='head-table-sit'>วันที่</th>
                                    <th style='width:30%' class='head-table-sit'>ชื่อวันหยุด</th>
                                    <th style='width:15%' class='head-table-sit'>ประเภทวันหยุด</th>
                                </tr>";
                //Body
                if (lstDataHoliday != null && lstDataHoliday.Any())
                {
                    foreach (var iR in lstDataHoliday)
                    {
                        //Body

                        sHTMLHoliday += "<tr>";
                        sHTMLHoliday += @"<td class='label-2'>" + ReplaceHtml(iR.sHolidayDate) + "</td>";
                        sHTMLHoliday += @"<td class='label-2'>" + ReplaceHtml(iR.sHolidayName) + "</td>";
                        sHTMLHoliday += @"<td class='label-3'>" + ReplaceHtml(iR.sActivity) + "</td>";
                        sHTMLHoliday += "</tr>";

                    }
                }
                else
                {
                    // No data
                    sHTMLActivity += "<tr>";
                    sHTMLActivity += @"<td class='label-nodata'>" + "</td>";
                    sHTMLActivity += @"<td class='label-nodata'>" + ReplaceHtml("ไม่มีข้อมูล") + "</td>";
                    sHTMLActivity += @"<td class='label-nodata'>" + "</td>";
                    sHTMLActivity += "</tr>";
                }
                sHTMLHoliday += "</table>";
                sHTMLHoliday += "<br/>";

                //Add Page
                string sFormatTable = "";
                sFormatTable += "<div>";
                sFormatTable += "<style>";
                sFormatTable += sCssStyle;
                sFormatTable += "</style>";
                sFormatTable += "<div class='head-exp'>";
                sFormatTable += "วันหยุดประจำปี " + objData.nYear;
                sFormatTable += "</div>";
                sFormatTable += "<div>";
                sFormatTable += "{0}";
                sFormatTable += "</div>";
                sFormatTable += "<div>";
                sFormatTable += "{1}";
                sFormatTable += "</div>";
                sFormatTable += "</div>";
                string sOutput = sFormatTable.Replace("{0}", sHTMLActivity).Replace("{1}", sHTMLHoliday);

                Spire.Doc.Section section = document.AddSection();
                section.PageSetup.PageSize = pSize;
                section.PageSetup.Orientation = Spire.Doc.Documents.PageOrientation.Landscape;
                section.PageSetup.Margins.Top = 30f;
                section.PageSetup.Margins.Bottom = 30f;
                section.PageSetup.Margins.Right = 30f;
                section.PageSetup.Margins.Left = 30f;
                Spire.Doc.Documents.Paragraph Para = section.AddParagraph();
                Para.ApplyStyle(style.Name);
                Para.AppendHTML(sOutput);


                //Set Head Row
                Spire.Doc.Table table = section.Tables[0] as Spire.Doc.Table;
                Spire.Doc.TableRow headRow = table.Rows[0];
                headRow.IsHeader = true;

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Position = 0;
                    document.SaveToStream(ms, Spire.Doc.FileFormat.PDF);
                    result.sFileType = "application/pdf";
                    result.objFile = ms.ToArray();
                    result.sFileName = sFileName;
                    ms.Dispose();
                }
                document.Dispose();
            }
            catch (System.Exception e)
            {
                var msg = e.Message;
                throw;
            }
            return result;
        }
        #endregion

        public async Task<ResultAPI> RemoveDataTable(cRemoveTableHoliday param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                int nUserID = 1;
                List<int> lstID = param.lstID.ConvertAll(item => item.DecryptParameter().ToInt()).ToList();
                var lstData = _db.TB_HolidayYear.Where(w => lstID.Contains(w.nHolidayYearID)).ToList();

                foreach (var item in lstData)
                {
                    item.IsDelete = true;
                    item.nDeleteBy = nUserID;
                    item.dDelete = DateTime.Now;
                }
                var lstDataYear = lstData.Where(w => lstID.Contains(w.nHolidayYearID)).Select(s => s.nYear).ToList();
                var lstDataDay = _db.TB_HolidayDay.Where(w => lstDataYear.Contains(w.nYear)).ToList();
                foreach (var item in lstDataDay)
                {
                    item.IsDelete = true;
                    item.nDeleteBy = nUserID;
                    item.dDelete = DateTime.Now;
                }



                _db.SaveChanges();




                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }
    }
}