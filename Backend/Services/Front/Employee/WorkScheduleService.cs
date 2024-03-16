using Backend.EF.ST_Intranet;
using Backend.Interfaces;
using Backend.Interfaces.Authentication;
using Backend.Models;
using ST.INFRA;
using ST.INFRA.Common;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;

namespace Backend.Service
{
    public class WorkScheduleService : IWorkScheduleService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _auth;
        private readonly IHostEnvironment _env;
        public WorkScheduleService(ST_IntranetEntity db, IAuthentication auth, IHostEnvironment env)
        {
            _db = db;
            _auth = auth;
            _env = env;
        }

        public cInitDataWorkSchedule GetInitData()
        {
            cInitDataWorkSchedule result = new cInitDataWorkSchedule();

            try
            {
                #region employee name
                result.lstEmployee = _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete).Select(s => new cSelectOption
                {
                    value = s.nEmployeeID + "",
                    label = $"{s.sNameTH} {s.sSurnameTH} ({s.sNickname})",
                }).ToList();
                #endregion

                #region list year
                // var lstYear = _db.TB_Employee_TimeStemp.GroupBy(g => new { g.dTimeDate }).Select(s => new cSelectOption
                // {
                //     value = (s.Key).ToString(),
                //     label = ((s.Key.dTimeDate.Year) + 543).ToString(),
                // }).ToList();

                result.lstYear = _db.TB_Employee_TimeStemp.GroupBy(g => new { g.dTimeDate.Year }).Select(s => new cSelectOption
                {
                    value = (s.Key.Year).ToString(),
                    label = ((s.Key.Year) + 543).ToString(),
                }).ToList();
                #endregion

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;

            }
            return result;
        }

        public cDataTable GetDataTable(cGetFilter objFilter)
        {
            cDataTable result = new cDataTable();
            var UserAccount = _auth.GetUserAccount();
            int nUserID = UserAccount.nUserID;
            try
            {
                var lstRequestLeave = _db.TB_Request_Leave.Where(w => w.nStatusID == 99 && !w.IsDelete).ToList();
                var lstRequestOT = _db.TB_Request_OT.Where(w => w.nStatusID == 99 && !w.IsDelete).ToList();
                var lstTimeStampLate = _db.TB_Employee_TimeStemp.ToList();
                var lstWFH = _db.TB_WFH.Where(w => !w.IsDelete).ToList();

                var lstEmp = _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete && !w.IsRetire).ToList();

                #region loop datetime
                List<cEmployeeList> lstFilter = new List<cEmployeeList>();

                var RoundStart = objFilter.dRequestStart == null ? DateTime.Now : objFilter.dRequestStart?.AddDays(-1);
                var RoundEnd = objFilter.dRequestEnd == null ? DateTime.Now : objFilter.dRequestEnd?.AddDays(1);

                for (DateTime? date = RoundStart; (date >= RoundStart && date <= RoundEnd); date = date?.AddDays(1))
                {
                    foreach (var item in lstEmp)
                    {
                        cEmployeeList objDate = new cEmployeeList()
                        {
                            dDateWork = date ?? default,
                            sID = item.nEmployeeID.EncryptParameter(),
                            nEmployeeID = item.nEmployeeID,
                            sEmpName = item.sNameTH + " " + item.sSurnameTH,
                            // objDate.lstFilter.Add(objDate)
                        };
                        lstFilter.Add(objDate);
                    }
                }
                #endregion

                var qry = _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete && !w.IsRetire).Select(s => new cEmployeeList
                {
                    sID = s.nEmployeeID.EncryptParameter(),
                    nEmployeeID = s.nEmployeeID,
                    sEmpName = s.sNameTH + " " + s.sSurnameTH,
                }).ToList();

                // var qry = lstFilter;
                qry.ForEach(f =>
                {
                    #region request leave
                    decimal sumLeaveUse = 0;
                    var userLeave = lstRequestLeave.Where(w => w.nEmployeeID == f.nEmployeeID).ToList();
                    if (userLeave.Count() != 0)
                    {
                        decimal[] arrLeaveUse = userLeave.Select(s => s.nLeaveUse).ToArray();
                        sumLeaveUse = arrLeaveUse.Sum();
                        // f.sLeaveMonth = userLeave.dStartDateTime.Month;
                    }
                    #endregion

                    #region request ot
                    decimal sumEstimateHour = 0;
                    var EstimateHour = lstRequestOT.Where(w => w.nCreateBy == f.nEmployeeID).ToList();
                    if (EstimateHour.Count() != 0)
                    {
                        decimal[] arrEstimateHour = EstimateHour.Select(s => s.nEstimateHour).ToArray();
                        sumEstimateHour = arrEstimateHour.Sum();
                        // f.sOTMonth = EstimateHour.dStartActionDateTime.Month;
                    }
                    #endregion

                    #region late
                    int sumLate = 0;
                    List<TB_Employee_TimeStemp> late = lstTimeStampLate.Where(w => w.nEmployeeID == f.nEmployeeID).ToList();
                    if (late.Count() != 0)
                    {
                        int[] arrLate = late.Select(s => s.nMinuteByHR).ToArray();
                        sumLate = arrLate.Sum();
                        // f.sLateMonth = late.dTimeDate.Month;
                    }
                    #endregion

                    #region wfh
                    int sumWFH = 0;
                    var wfh = lstWFH.Where(w => w.nCreateBy == f.nEmployeeID).ToList();
                    if (wfh.Count() != 0)
                    {
                        sumWFH = wfh.Count();
                        // f.sWFHMonth = wfh.dWFH.Month;
                    }
                    #endregion

                    int beforeDecimal = 0;
                    int afterDecimal = 0;
                    if (sumLeaveUse != 0)
                    {
                        string[] splitSumLeaveUse = (sumLeaveUse).ToString().Split(".");
                        beforeDecimal = int.Parse(splitSumLeaveUse[0]);
                        afterDecimal = int.Parse(splitSumLeaveUse[1]);
                    }

                    f.sLeaveRequest = sumLeaveUse == 0 ? "-" : beforeDecimal.ToString() + " วัน " + afterDecimal.ToString() + " ชม.";

                    // f.sLeaveRequest = sumLeaveUse == 0 ? "-" : (sumLeaveUse / 1).ToString() + " วัน " + ((sumLeaveUse % 1) * 10).ToString() + " ชม.";
                    f.sOTRequest = sumEstimateHour == 0 ? "-" : sumEstimateHour.ToString();
                    f.sLateTime = sumLate == 0 ? "-" : sumLate.ToString();
                    f.sWFHRequest = sumWFH == 0 ? "-" : sumWFH.ToString();
                });

                qry = qry.OrderBy(o => o.nEmployeeID).ToList();

                #region filter
                if (objFilter.lstEmployee != null && objFilter.lstEmployee.Any())
                {
                    qry = qry.Where(w => objFilter.lstEmployee.Contains(w.nEmployeeID.ToString())).ToList();
                }

                if (objFilter.objYear != null)
                {
                    int year = 0;
                    var date = DateTime.Now;
                    year = objFilter.objYear.ToInt() - DateTime.Now.Year;
                    if (year != 0)
                    {
                        date.AddYears(year);
                    }

                    // qry = qry.Where(w => w.dTimeStartWork != null ? w.dTimeStartWork.Year <= date.Year : false).ToList();
                    qry = qry.Where(w => w.dTimeStartWork.Year <= date.Year).ToList();
                }

                // if (objFilter.dRequestStart != null)
                // {
                //     qry = qry.Where(w => w.dTimeStartWork != null ? w.dTimeStartWork.Date >= objFilter.dRequestStart.Value.Date : false).ToList();
                // }

                // if (objFilter.dRequestEnd != null)
                // {
                //     qry = qry.Where(w => w.dTimeEndWork != null ? w.dTimeEndWork.Date <= objFilter.dRequestEnd.Value.Date : false).ToList();
                // }
                #endregion

                // var dNow = DateTime.Now;
                // var findMinMonth = dNow.AddMonths(-1);

                // int nYear = objFilter.objYear == null ? 2023 : objFilter.objYear.ToInt();
                // var minMonth = objFilter.sMonth == null ? findMinMonth.Month.ToString().ToInt() : objFilter.sMonth == "1" ? objFilter.sMonth.ToInt() : objFilter.sMonth.ToInt() - 1;
                // var maxMonth = objFilter.sMonth == null ? dNow.Month.ToString().ToInt() : objFilter.sMonth.ToInt();
                // DateTime minDate = new DateTime(nYear, minMonth, 26);
                // DateTime maxDate = new DateTime(nYear, maxMonth, 25);

                // result.minDate = minDate;
                // result.maxDate = maxDate;

                #region//SORT
                string sSortColumn = objFilter.sSortExpression;
                switch (objFilter.sSortExpression)
                {
                    case "nEmployeeID": sSortColumn = "nEmployeeID"; break;
                        // case "sNote": sSortColumn = "sNote"; break;
                        // case "nOrder": sSortColumn = "nOrder"; break;
                        // case "dUpdate": sSortColumn = "dUpdate"; break;
                }
                if (objFilter.isASC)
                {
                    qry = qry.OrderBy<cEmployeeList>(sSortColumn).ToList();
                }
                else if (objFilter.isDESC)
                {
                    qry = qry.OrderByDescending<cEmployeeList>(sSortColumn).ToList();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(objFilter.nPageSize, objFilter.nPageIndex, qry.Count());
                var lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();

                var nIndex = objFilter.nPageSize * (objFilter.nPageIndex - 1) + 1;
                if (nIndex < 1) nIndex = 1;
                foreach (var item in lstData)
                {
                    item.No = nIndex;
                    nIndex++;
                }

                result.lstData = lstData;
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                #endregion

                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }

        public cInitDataWorkSchedule GetEmployeeData(cEmployeeData param)
        {
            cInitDataWorkSchedule result = new cInitDataWorkSchedule();
            int nID = param.sID.DecryptParameter().ToInt();

            try
            {
                var empPosition = _db.TB_Employee_Position.Where(w => w.nEmployeeID == nID && !w.IsDelete);
                var findPosition = empPosition.Select(s => s.nPositionID).ToList();
                var position = _db.TB_Position.FirstOrDefault(w => findPosition.Contains(w.nPositionID) && !w.IsDelete);
                var masterData = _db.TM_Data.Where(w => w.nDatatypeID == 7 && !w.IsDelete);

                var empData = _db.TB_Employee.Where(w => w.nEmployeeID == nID && w.IsActive && !w.IsDelete && !w.IsRetire).Select(s =>
                new cEmpWorkSchedule
                {
                    sEmployeeCode = s.sEmplyeeCode,
                    sEmpName = s.sNameTH + " " + s.sSurnameTH + " (" + s.sNickname + ")",
                    sPhone = s.sTelephone,
                    sEmail = s.sEmail,
                    dWorkStart = s.dWorkStart,
                    sWorkYear = s.dWorkStart != null ? (DateTime.Now.Year - s.dWorkStart.Value.Year) + "ปี" + " " + (DateTime.Now.Month - s.dWorkStart.Value.Month > 0 ? DateTime.Now.Month - s.dWorkStart.Value.Month : 0) + "เดือน" : "",
                    nEmployeeTypeID = s.nEmployeeTypeID
                }).ToList();

                empData.ForEach(f =>
                {
                    f.sPosition = "-";
                    f.sEmployeeType = "-";

                    if (position != null)
                    {
                        f.sPosition = position.sPositionName;
                    }

                    var empType = masterData.FirstOrDefault(w => w.nData_ID == f.nEmployeeTypeID);
                    if (empType != null)
                    {
                        f.sEmployeeType = empType.sNameTH;
                    }

                });

                result.lstEmployeeData = empData.ToList();

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }

        public cDataTable GetDataTableEdit(cGetFilter objFilter)
        {
            cDataTable result = new cDataTable();
            int nID = objFilter.sID.DecryptParameter().ToInt();
            result.sEmpID = objFilter.sID.DecryptParameter();

            try
            {
                var lstEmp = _db.TB_Employee.Where(w => w.nEmployeeID == nID && w.IsActive && !w.IsDelete && !w.IsRetire).ToList();

                #region loop datetime   
                var dNow = DateTime.Now;
                var dstart = new DateTime((dNow.Month == 1 ? dNow.AddYears(-1).Year : dNow.Year), dNow.AddMonths(-1).Month, 26);
                var dend = new DateTime(dNow.Year, dNow.Month, 25);

                if (dNow.Date > dend.Date)
                {
                    dstart = new DateTime((dNow.Month == 1 ? dNow.AddYears(-1).Year : dNow.Year), dNow.Month, 26);
                    dend = new DateTime(dNow.Year, dNow.AddMonths(1).Month, 25);
                }

                var RoundStart = objFilter.dRequestStart == null ? dstart : objFilter.dRequestStart;
                var RoundEnd = objFilter.dRequestEnd == null ? dend : objFilter.dRequestEnd;

                List<cEmpWorkSchedule> lstDateFilter = new List<cEmpWorkSchedule>();
                for (DateTime? date = RoundStart; (date >= RoundStart && date <= RoundEnd); date = date?.AddDays(1))
                {
                    cEmpWorkSchedule objDate = new cEmpWorkSchedule();
                    var emp = lstEmp.FirstOrDefault();

                    objDate.nEmployeeID = emp.nEmployeeID;
                    objDate.sEmpName = emp.sNameTH + " " + emp.sSurnameTH;
                    objDate.dDateWork = date ?? default;
                    objDate.sDateWork = objDate.dDateWork.ToStringFromDateTime(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH);
                    lstDateFilter.Add(objDate);
                }
                #endregion

                var lstRequestOT = _db.TB_Request_OT.Where(w => w.nCreateBy == nID && w.nStatusID == 99 && !w.IsDelete).ToList();
                var lstWFH = _db.TB_WFH.Where(w => w.nCreateBy == nID && !w.IsDelete).ToList();
                var lstTimeStamp = _db.TB_Employee_TimeStemp.Where(w => w.nEmployeeID == nID).ToList();
                var lstRequestLeave = _db.TB_Request_Leave.Where(w => w.nEmployeeID == nID && w.nStatusID == 99 && !w.IsDelete).ToList();

                #region find date
                int No = 1;
                lstDateFilter.ForEach(f =>
                {
                    f.No = No;
                    f.sEmployeeID = f.nEmployeeID.ToString() + "_" + f.No;
                    f.sID = f.sEmployeeID;
                    No += 1;

                    #region time stamp
                    var findTimestampMatch = lstTimeStamp.Where(w => w.dTimeDate.Date == f.dDateWork.Date);
                    if (findTimestampMatch != null)
                    {
                        foreach (var item in findTimestampMatch)
                        {
                            f.dTimeStartWork = item.dTimeStartDate;
                            f.dTimeEndWork = item.dTimeEndDate ?? default;
                            f.sLateTime = item.nMinuteByHR == 0 ? "0" : item.nMinuteByHR.ToString();
                            f.sComment = item.sComment == null ? "" : item.sComment;
                        }
                    }
                    #endregion

                    #region time leave
                    var findLeaveMatch = lstRequestLeave.Where(w => w.dStartDateTime.Date == f.dDateWork.Date);

                    if (findLeaveMatch != null)
                    {
                        var lstRequestLeaveType = findLeaveMatch.Select(s => s.nLeaveTypeID);
                        var lstLeaveType = _db.TB_LeaveType.Where(w => lstRequestLeaveType.Contains(w.nLeaveTypeID) && !w.IsDelete).ToList();


                        var findLeave = (from t1 in findLeaveMatch
                                         from t2 in lstLeaveType.Where(w => w.nLeaveTypeID == t1.nLeaveTypeID)
                                         select new cLeaveRequest
                                         {
                                             nIDLeave = t1.nEmployeeID,
                                             sLeaveTypeName = t2.LeaveTypeName == null ? "-" : t2.LeaveTypeName,
                                             dStartDateTime = t1.dStartDateTime,
                                             dEndDateTime = t1.dEndDateTime,
                                             nLeaveTypeID = t1.nLeaveTypeID,
                                             nLeaveUse = t1.nLeaveUse
                                         }).ToList();

                        if (findLeave != null)
                        {
                            foreach (var item in findLeave)
                            {
                                f.sLeaveTime = item.sLeaveTypeName == null ? "-" : item.sLeaveTypeName + " ตั้งแต่ " + item.dStartDateTime?.ToShortTimeString() + " - " + item.dEndDateTime?.ToShortTimeString();
                            }
                        }
                    }
                    #endregion

                    #region time ot
                    decimal sumEstimateHour = 0;
                    var findOTMatch = lstRequestOT.Where(w => w.dStartActionDateTime?.Date == f.dDateWork.Date);
                    if (findOTMatch != null)
                    {
                        var otStart = findOTMatch.Select(s => s.dStartActionDateTime);
                        foreach (var item in otStart)
                        {
                            decimal[] arrEstimateHour = lstRequestOT.Where(w => w.nCreateBy == f.nEmployeeID).Select(s => s.nEstimateHour).ToArray();
                            sumEstimateHour = arrEstimateHour.Sum();
                        }
                    }
                    f.sOTRequestHours = sumEstimateHour == 0 ? "-" : sumEstimateHour.ToString();
                    result.nCalculateOT = sumEstimateHour;
                    #endregion

                    #region time wfh
                    int sumWFH = 0;
                    var findWFHMatch = lstWFH.Where(w => w.dWFH.Date == f.dDateWork.Date);
                    if (findWFHMatch != null)
                    {
                        //  var wfhDate = findWFHMatch.Select(s => s.dWFH);
                        // foreach (var item in findWFHMatch)
                        // {
                        TB_WFH[] aa = findWFHMatch.ToArray();
                        sumWFH = aa.Count();
                        // }
                    }
                    f.sWHFCount = sumWFH == 0 ? "-" : "WFH #" + sumWFH.ToString();

                    // result.nCalculateWFH = sumWFH;
                    #endregion

                });
                #endregion

                var lstDate = lstDateFilter.Select(s => s.dDateWork.Date).ToList();

                #region cal wfh
                int sumWFH = 0;
                var findWFHMatch = lstWFH.Where(w => lstDate.Contains(w.dWFH.Date));
                if (findWFHMatch != null)
                {
                    TB_WFH[] aa = findWFHMatch.ToArray();
                    sumWFH = aa.Count();
                }

                result.nCalculateWFH = sumWFH;
                #endregion

                #region cal late time
                var dupArray = lstDateFilter.ToArray();
                int nCalLate = 0;
                var arrCalculate = new List<int>();

                foreach (var date in lstDateFilter)
                {
                    foreach (var item in dupArray)
                    {
                        if (item.dDateWork == date.dDateWork)
                        {
                            int nLateTime = item.sLateTime == null ? 0 : item.sLateTime.ToInt();
                            arrCalculate.Add(nLateTime);
                        }
                    }
                }
                nCalLate = arrCalculate.Sum();
                result.nCalculateLateTime = nCalLate;
                #endregion

                #region cal leave time
                decimal nSumLeave;
                var lstleave = lstRequestLeave.Where(w => lstDate.Contains(w.dStartDateTime.Date)).ToList();
                nSumLeave = lstleave.Select(s => s.nLeaveUse).Sum();

                int beforeDecimal = 0;
                int afterDecimal = 0;
                if (nSumLeave != 0)
                {
                    string[] splitSumLeaveUse = (nSumLeave).ToString().Split(".");
                    beforeDecimal = int.Parse(splitSumLeaveUse[0]);
                    afterDecimal = int.Parse(splitSumLeaveUse[1]);
                }

                result.sCalculateLeave = nSumLeave == 0 ? "0 วัน" : beforeDecimal.ToString() + " วัน " + afterDecimal.ToString() + " ชม.";
                #endregion

                lstDateFilter = lstDateFilter.OrderBy(o => o.No).ToList();
                #region filter
                // if (objFilter.dRequestStart != null)
                // {
                //     lstDateFilter = lstDateFilter.Where(w => w.dTimeStartWork != null ? w.dTimeStartWork.Date >= objFilter.dRequestStart.Value.Date : false).ToList();
                // }

                // if (objFilter.dRequestEnd != null)
                // {
                //     lstDateFilter = lstDateFilter.Where(w => w.dTimeEndWork != null ? w.dTimeEndWork.Date <= objFilter.dRequestEnd.Value.Date : false).ToList();
                // }
                // if (objFilter.objYear != null)
                // {
                //     lstDateFilter = lstDateFilter.Where(w => w.dTimeStartWork != null ? w.dTimeStartWork.Year >= objFilter.dRequestStart.Value.Year : false).ToList();
                // }

                #endregion

                #region//SORT
                string sSortColumn = objFilter.sSortExpression;
                switch (objFilter.sSortExpression)
                {
                    case "No": sSortColumn = "No"; break;
                        // case "sNote": sSortColumn = "sNote"; break;
                        // case "nOrder": sSortColumn = "nOrder"; break;
                        // case "dUpdate": sSortColumn = "dUpdate"; break;
                }
                if (objFilter.isASC)
                {
                    lstDateFilter = lstDateFilter.OrderBy<cEmpWorkSchedule>(sSortColumn).ToList();
                }
                else if (objFilter.isDESC)
                {
                    lstDateFilter = lstDateFilter.OrderByDescending<cEmpWorkSchedule>(sSortColumn).ToList();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(objFilter.nPageSize, objFilter.nPageIndex, lstDateFilter.Count());
                var lstEmpData = lstDateFilter.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();

                var nIndex = objFilter.nPageSize * (objFilter.nPageIndex - 1) + 1;
                if (nIndex < 1) nIndex = 1;
                foreach (var item in lstEmpData)
                {
                    item.No = nIndex;
                    nIndex++;
                }

                result.lstEmpData = lstEmpData;
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                #endregion

                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }

        public cDataTable SaveEmployeeData(cSaveEmployeeData objSaveData)
        {
            cDataTable result = new cDataTable();
            int nID = objSaveData.sID.DecryptParameter().ToInt();

            try
            {
                DateTime dNow = DateTime.Now;

                objSaveData.lstEmpData?.ForEach(f =>
                {
                    if (f.sDateWork != "สรุป")
                    {
                        TimeSpan tStart = new TimeSpan(f.dStartTime?.Hour ?? dNow.Hour, f.dStartTime?.Minute ?? dNow.Minute, f.dStartTime?.Second ?? dNow.Second);
                        TimeSpan tEnd = new TimeSpan(f.dEndTime?.Hour ?? dNow.Hour, f.dEndTime?.Minute ?? dNow.Minute, f.dEndTime?.Second ?? dNow.Second);
                        var dDateStart = f.dDateWork.Add(tStart);
                        var dDateEnd = f.dDateWork.Add(tEnd);

                        bool isDefaultStartTime = (f.dStartTime?.Hour == 0 && f.dStartTime?.Minute == 0 && f.dStartTime?.Second == 0) ? true : false;
                        bool isDefaultEndTime = (f.dEndTime?.Hour == 0 && f.dEndTime?.Minute == 0 && f.dEndTime?.Second == 0) ? true : false;

                        if (!isDefaultStartTime)
                        {
                            TB_Employee_TimeStemp? objTimeStamp = _db.TB_Employee_TimeStemp.FirstOrDefault(w => w.nEmployeeID == nID && w.dTimeDate == f.dDateWork.Date);
                            if (objTimeStamp == null)
                            {
                                objTimeStamp = new TB_Employee_TimeStemp();
                                objTimeStamp.nTimeStampID = (_db.TB_Employee_TimeStemp.Any() ? _db.TB_Employee_TimeStemp.Max(m => m.nTimeStampID) : 0) + 1;
                                objTimeStamp.nEmployeeID = nID;
                                objTimeStamp.dTimeDate = f.dDateWork.Date;
                                _db.TB_Employee_TimeStemp.Add(objTimeStamp);
                            }
                            objTimeStamp.dTimeStartDate = dDateStart; //dStart;
                            objTimeStamp.dTimeEndDate = isDefaultEndTime ? dDateEnd : f.dEndTime;// dEnd;
                            objTimeStamp.nMinute = f.sLateTime == null ? 0 : f.sLateTime.ToInt();
                            objTimeStamp.nMinuteByHR = f.sLateTime == null ? 0 : f.sLateTime.ToInt();
                            objTimeStamp.sComment = f.sComment ?? null;
                            objTimeStamp.IsDelay = objTimeStamp.nMinuteByHR == 0 ? false : true;
                            _db.SaveChanges();
                        }
                    }
                });

                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = StatusCodes.Status500InternalServerError;
            }

            return result;
        }

        public cExport WorkShceduleExportExcel(cExportRequest objExport)
        {
            cExport result = new cExport();
            DateTime dNow = DateTime.Now;
            string sDate = dNow.ToString("DDMMYYYYHHmmss");
            string sFileName = "WorkSchedule_" + sDate;

            try
            {
                var lstEmp = objExport.lstWorkShcedule;

                var lstRequestOT = _db.TB_Request_OT.Where(w => w.nStatusID == 99 && !w.IsDelete).ToList();
                var lstWFH = _db.TB_WFH.Where(w => !w.IsDelete).ToList();
                var lstTimeStamp = _db.TB_Employee_TimeStemp.ToList();
                var lstRequestLeave = _db.TB_Request_Leave.Where(w => w.nStatusID == 99 && !w.IsDelete).ToList();

                #region loop datetime   
                var dstart = new DateTime((dNow.Month == 1 ? dNow.AddYears(-1).Year : dNow.Year), dNow.AddMonths(-1).Month, 26);
                var dend = new DateTime(dNow.Year, dNow.Month, 25);

                if (dNow.Date > dend.Date)
                {
                    dstart = new DateTime((dNow.Month == 1 ? dNow.AddYears(-1).Year : dNow.Year), dNow.Month, 26);
                    dend = new DateTime(dNow.Year, dNow.AddMonths(1).Month, 25);
                }

                //set round start and end
                var RoundStart = objExport.dRequestStart == null ? dstart : objExport.dRequestStart;
                var RoundEnd = objExport.dRequestEnd == null ? dend : objExport.dRequestEnd;

                List<cEmpWorkSchedule> lstDateFilter = new List<cEmpWorkSchedule>();
                for (DateTime? date = RoundStart; (date >= RoundStart && date <= RoundEnd); date = date?.AddDays(1))
                {//loop date and employee data
                    foreach (var item in lstEmp)
                    {

                        var employee = _db.TB_Employee.Where(w => w.nEmployeeID == item.nEmployeeID && w.IsActive && !w.IsDelete && !w.IsRetire).ToList();

                        var emp = employee.FirstOrDefault();

                        if (emp != null)
                        {
                            cEmpWorkSchedule objDate = new cEmpWorkSchedule();
                            objDate.nEmployeeID = emp.nEmployeeID;
                            objDate.sEmpName = emp.sNameTH + " " + emp.sSurnameTH;
                            objDate.dDateWork = date ?? default;
                            objDate.sDateWork = objDate.dDateWork.ToStringFromDateTime(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH);
                            lstDateFilter.Add(objDate);
                        }
                    }
                }
                #endregion

                int No = 1;
                lstDateFilter.ForEach(f =>
                {
                    f.No = No;
                    f.sEmployeeID = f.nEmployeeID.ToString() + "_" + f.No;
                    f.sID = f.sEmployeeID;
                    No += 1;

                    #region time stamp
                    var findTimestampMatch = lstTimeStamp.Where(w => w.dTimeDate.Date == f.dDateWork.Date && w.nEmployeeID == f.nEmployeeID);
                    if (findTimestampMatch != null)
                    {
                        int nCalLate = 0;
                        var arrCalculate = new List<int>();

                        foreach (var item in findTimestampMatch)
                        {
                            f.dTimeStartWork = item.dTimeStartDate;
                            f.dTimeEndWork = item.dTimeEndDate ?? default;
                            f.sTimeStartWork = f.dTimeStartWork.ToStringFromDateTime(ST.INFRA.Enum.DateTimeFormat.HHmm, ST.INFRA.Enum.CultureName.th_TH);
                            f.sTimeEndWork = f.dTimeEndWork.ToStringFromDateTime(ST.INFRA.Enum.DateTimeFormat.HHmm, ST.INFRA.Enum.CultureName.th_TH);
                            f.sLateTime = (item.nMinuteByHR == 0 && !item.IsDelay) ? "-" : item.nMinuteByHR.ToString();
                            f.sComment = item.sComment == null ? "" : item.sComment;

                            if (f.dDateWork.Date == item.dTimeDate.Date)
                            {
                                arrCalculate.Add(item.nMinuteByHR);
                            }


                        }
                        nCalLate = arrCalculate.Sum();
                        f.sCalculateLateTime = nCalLate == 0 ? "-" : nCalLate.ToString();
                    }
                    #endregion

                    #region time leave
                    var findLeaveMatch = lstRequestLeave.Where(w => w.dStartDateTime.Date == f.dDateWork.Date && w.nEmployeeID == f.nEmployeeID);

                    if (findLeaveMatch != null)
                    {
                        var lstRequestLeaveType = findLeaveMatch.Select(s => s.nLeaveTypeID);
                        var lstLeaveType = _db.TB_LeaveType.Where(w => lstRequestLeaveType.Contains(w.nLeaveTypeID) && !w.IsDelete).ToList();


                        var findLeave = (from t1 in findLeaveMatch
                                         from t2 in lstLeaveType.Where(w => w.nLeaveTypeID == t1.nLeaveTypeID)
                                         select new cLeaveRequest
                                         {
                                             nIDLeave = t1.nEmployeeID,
                                             sLeaveTypeName = t2.LeaveTypeName == null ? "-" : t2.LeaveTypeName,
                                             dStartDateTime = t1.dStartDateTime,
                                             dEndDateTime = t1.dEndDateTime,
                                             nLeaveTypeID = t1.nLeaveTypeID,
                                             nLeaveUse = t1.nLeaveUse
                                         }).ToList();

                        if (findLeave != null)
                        {
                            foreach (var item in findLeave)
                            {
                                f.sLeaveTime = item.sLeaveTypeName == null ? "-" : item.sLeaveTypeName + " ตั้งแต่ " + item.dStartDateTime?.ToShortTimeString() + " - " + item.dEndDateTime?.ToShortTimeString();
                            }

                            #region cal leave time
                            decimal nSumLeave;
                            var lstleave = lstRequestLeave.Where(w => f.dDateWork == w.dStartDateTime.Date).ToList();
                            nSumLeave = lstleave.Select(s => s.nLeaveUse).Sum();

                            int beforeDecimal = 0;
                            int afterDecimal = 0;
                            if (nSumLeave != 0)
                            {
                                string[] splitSumLeaveUse = (nSumLeave).ToString().Split(".");
                                beforeDecimal = int.Parse(splitSumLeaveUse[0]);
                                afterDecimal = int.Parse(splitSumLeaveUse[1]);
                            }

                            f.sCalculateLeave = nSumLeave == 0 ? "0 วัน" : beforeDecimal.ToString() + " วัน " + afterDecimal.ToString() + " ชม.";
                            #endregion
                        }
                    }
                    #endregion

                    #region time ot
                    decimal sumEstimateHour = 0;
                    var findOTMatch = lstRequestOT.Where(w => w.dStartActionDateTime?.Date == f.dDateWork.Date && w.nCreateBy == f.nEmployeeID);
                    if (findOTMatch != null)
                    {
                        var otStart = findOTMatch.Select(s => s.dStartActionDateTime);
                        foreach (var item in otStart)
                        {
                            decimal[] arrEstimateHour = lstRequestOT.Where(w => w.nCreateBy == f.nEmployeeID).Select(s => s.nEstimateHour).ToArray();
                            sumEstimateHour = arrEstimateHour.Sum();
                        }
                    }
                    f.sOTRequestHours = sumEstimateHour == 0 ? "-" : sumEstimateHour.ToString();
                    f.nCalculateOT = sumEstimateHour;
                    #endregion

                    #region time wfh
                    int nCountWFH = 0;

                    var findWFHMatch = lstWFH.Where(w => w.dWFH.Date == f.dDateWork.Date && w.nCreateBy == f.nEmployeeID);
                    if (findWFHMatch != null)
                    {
                        TB_WFH[] aa = findWFHMatch.ToArray();
                        nCountWFH = findWFHMatch.Count();
                    }

                    f.sWHFCount = nCountWFH == 0 ? "-" : "WFH #" + nCountWFH.ToString();
                    f.sCalculateWFH = nCountWFH == 0 ? "-" : nCountWFH.ToString(); ;
                    #endregion
                });

                var groupLstDate = lstEmp?.GroupBy(g => new { g.sEmpName }).Select(s => s.Key.sEmpName).ToList();
                var lstData = lstDateFilter.OrderBy(o => o.nEmployeeID).ThenBy(o => o.dDateWork.Date).ToList();

                //excel
                var wb = new XLWorkbook();
                foreach (var emp in groupLstDate)
                {
                    var lstDateCheckEmpName = lstData.Where(w => w.sEmpName == emp);

                    IXLWorksheet ws = wb.AddWorksheet(emp);
                    int nFontSize = 14; //ขนาดฟอนต์
                    int nFontSizeHead = 16;

                    ws.PageSetup.Margins.Top = 0.2;
                    ws.PageSetup.Margins.Bottom = 0.2;
                    ws.PageSetup.Margins.Left = 0.1;
                    ws.PageSetup.Margins.Right = 0;
                    ws.PageSetup.Margins.Footer = 0;
                    ws.PageSetup.Margins.Header = 0;

                    int nRow = 1;
                    // int nCol = 5;
                    #region  Header
                    List<string> lstEmpHead = new List<string>() { "วันที่", "ลงเวลาเข้า", "ลงเวลาออก", "สาย (นาที)", "ขออนุมัติลา", "ขออนุมัติ OT (ชั่วโมง)", "ขออนุมัติ WFH (ครั้ง)", "หมายเหตุ" };
                    List<int> lstwidthHeader = new List<int>() { 50, 15, 15, 20, 20, 35, 35, 50 };
                    var RowsHead = ws;
                    var itemCell = RowsHead.Cell(nRow, 1);

                    int nColWidth = 1;
                    foreach (var itmWidth in lstwidthHeader)
                    {
                        ws.Column(nColWidth).Width = itmWidth;
                        nColWidth++;
                    }
                    int indexHead = 0;

                    //nRow++; //ขึ้น Row ใหม่
                    foreach (var item in lstEmpHead)
                    {
                        indexHead += 1;
                        ws.Cell(nRow, indexHead).Value = item;
                        ws.Cell(nRow, indexHead).Style.Font.FontSize = nFontSizeHead;
                        ws.Cell(nRow, indexHead).Style.Font.Bold = true;
                        ws.Cell(nRow, indexHead).Style.Font.FontColor = XLColor.GrayAsparagus;
                        ws.Cell(nRow, indexHead).Style.Alignment.WrapText = true; //ปัดบรรทัด
                        ws.Cell(nRow, indexHead).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(nRow, indexHead).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(nRow, indexHead).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#68B984"));
                    }
                    #endregion

                    int nStartBorder = nRow;// + 1;
                    if (lstDateCheckEmpName != null)
                    {
                        foreach (var item in lstDateCheckEmpName)
                        {
                            nRow++; //ขึ้นบรรทัดใหม่

                            //work date
                            ws.Cell(nRow, 1).Value = item.sDateWork;
                            ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 1).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 1).Style.Alignment.WrapText = true;

                            //work start time
                            ws.Cell(nRow, 2).Value = item.sTimeStartWork == null ? "-" : item.sTimeStartWork;
                            ws.Cell(nRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 2).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 2).Style.Alignment.WrapText = true;

                            //work end time
                            ws.Cell(nRow, 3).Value = item.sTimeEndWork == null ? "-" : item.sTimeEndWork;
                            ws.Cell(nRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 3).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 3).Style.Alignment.WrapText = true;

                            //late (minute)
                            ws.Cell(nRow, 4).Value = item.sLateTime == null ? "-" : item.sLateTime;
                            ws.Cell(nRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 4).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 4).Style.Alignment.WrapText = true;

                            //leave
                            ws.Cell(nRow, 5).Value = item.sLeaveTime == null ? "-" : item.sLeaveTime;
                            ws.Cell(nRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 5).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 5).Style.Alignment.WrapText = true;

                            //ot
                            ws.Cell(nRow, 6).Value = item.sOTRequestHours;
                            ws.Cell(nRow, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 6).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 6).Style.Alignment.WrapText = true;

                            //wfh
                            ws.Cell(nRow, 7).Value = item.sWHFCount;
                            ws.Cell(nRow, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 7).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 7).Style.Alignment.WrapText = true;

                            //comment
                            ws.Cell(nRow, 8).Value = item.sComment;
                            ws.Cell(nRow, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(nRow, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(nRow, 8).Style.Font.FontSize = nFontSize;
                            ws.Cell(nRow, 8).Style.Alignment.WrapText = true;

                            if (nRow == 31)
                            {
                                nRow += 1;

                                ws.Cell(nRow, 1).Value = "สรุป";
                                ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 1).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 1).Style.Alignment.WrapText = true;
                                ws.Cell(nRow, 1).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#F0FF0A"));

                                ws.Cell(nRow, 2).Value = "-";
                                ws.Cell(nRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 2).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 2).Style.Alignment.WrapText = true;
                                ws.Cell(nRow, 2).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#F0FF0A"));


                                ws.Cell(nRow, 3).Value = "-";
                                ws.Cell(nRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 3).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 3).Style.Alignment.WrapText = true;
                                ws.Cell(nRow, 3).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#F0FF0A"));

                                ws.Cell(nRow, 4).Value = item.sCalculateLateTime;
                                ws.Cell(nRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 4).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 4).Style.Alignment.WrapText = true;
                                ws.Cell(nRow, 4).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#F0FF0A"));

                                ws.Cell(nRow, 5).Value = item.sCalculateLeave == "0 วัน" ? "-" : item.sCalculateLeave;
                                ws.Cell(nRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 5).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 5).Style.Alignment.WrapText = true;
                                ws.Cell(nRow, 5).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#F0FF0A"));

                                ws.Cell(nRow, 6).Value = item.nCalculateOT == 0 ? "-" : item.nCalculateOT;
                                ws.Cell(nRow, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 6).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 6).Style.Alignment.WrapText = true;
                                ws.Cell(nRow, 6).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#F0FF0A"));

                                ws.Cell(nRow, 7).Value = item.sCalculateWFH;
                                ws.Cell(nRow, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 7).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 7).Style.Alignment.WrapText = true;
                                ws.Cell(nRow, 7).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#F0FF0A"));

                                ws.Cell(nRow, 8).Value = "-";
                                ws.Cell(nRow, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(nRow, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(nRow, 8).Style.Font.FontSize = nFontSize;
                                ws.Cell(nRow, 8).Style.Alignment.WrapText = true;
                                ws.Cell(nRow, 8).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#F0FF0A"));
                            }
                        }
                    }
                    else
                    {
                        // No data
                        nRow++;
                        ws.Cell(nRow, 1).Value = "ไม่พบข้อมูล";
                        ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }
                    //ระยะเส้นขอบที่ต้องการให้แสดง
                    ws.Range(nStartBorder, 1, nRow, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(nStartBorder, 1, nRow, 8).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                }

                using (MemoryStream fs = new MemoryStream())
                {
                    wb.SaveAs(fs);
                    wb.Dispose();
                    fs.Position = 0;
                    result.objFile = fs.ToArray();
                    result.sFileType = "application/vnd.ms-excel";
                    result.sFileName = sFileName;
                    return result;
                }
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }

            return result;
        }

        public cDataTable GetDataExportExcel(cGetDataForExcel objFilter)
        {
            cDataTable result = new cDataTable();
            try
            {
                var allEmp = _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete && !w.IsRetire).Select(s => new cEmployeeList
                {
                    sID = s.nEmployeeID.EncryptParameter(),
                    nEmployeeID = s.nEmployeeID,
                    sEmpName = s.sNameTH + " " + s.sSurnameTH,
                    sNickName = s.sNickname
                }).ToList();

                #region loop datetime
                var dNow = DateTime.Now;
                var dstart = new DateTime(dNow.Year, dNow.AddMonths(-1).Month, 26);
                var dend = new DateTime(dNow.Year, dNow.Month, 25);

                if (dNow.Date > dend.Date)
                {
                    dstart = new DateTime(dNow.Year, dNow.Month, 26);
                    dend = new DateTime(dNow.Year, dNow.AddMonths(1).Month, 25);
                }

                var startDate = objFilter.dRequestStart == null ? dstart : objFilter.dRequestStart;
                var endDate = objFilter.dRequestEnd == null ? dend : objFilter.dRequestEnd;
                #endregion

                var lstRequestOT = _db.TB_Request_OT.Where(w => w.nStatusID == 99 && !w.IsDelete).ToList();
                var lstWFH = _db.TB_WFH.Where(w => !w.IsDelete).ToList();
                var lstTimeStamp = _db.TB_Employee_TimeStemp.ToList();
                var lstRequestLeave = _db.TB_Request_Leave.Where(w => w.nStatusID == 99 && !w.IsDelete).ToList();

                #region find dat
                // int No = 1;
                allEmp.ForEach(f =>
                {
                    #region request leave
                    decimal sumLeaveUse = 0;
                    var userLeave = lstRequestLeave.Where(w => w.nEmployeeID == f.nEmployeeID).ToList();
                    if (userLeave.Count() != 0)
                    {
                        decimal[] arrLeaveUse = userLeave.Select(s => s.nLeaveUse).ToArray();
                        sumLeaveUse = arrLeaveUse.Sum();
                        // f.sLeaveMonth = userLeave.dStartDateTime.Month;
                    }
                    #endregion

                    #region request ot
                    decimal sumEstimateHour = 0;
                    var EstimateHour = lstRequestOT.Where(w => w.nCreateBy == f.nEmployeeID).ToList();
                    if (EstimateHour.Count() != 0)
                    {
                        decimal[] arrEstimateHour = EstimateHour.Select(s => s.nEstimateHour).ToArray();
                        sumEstimateHour = arrEstimateHour.Sum();
                        // f.sOTMonth = EstimateHour.dStartActionDateTime.Month;
                    }
                    #endregion

                    #region late
                    int sumLate = 0;
                    List<TB_Employee_TimeStemp> late = lstTimeStamp.Where(w => w.nEmployeeID == f.nEmployeeID).ToList();
                    if (late.Count() != 0)
                    {
                        int[] arrLate = late.Select(s => s.nMinuteByHR).ToArray();
                        sumLate = arrLate.Sum();
                        // f.sLateMonth = late.dTimeDate.Month;
                    }
                    #endregion

                    #region wfh
                    int sumWFH = 0;
                    var wfh = lstWFH.Where(w => w.nCreateBy == f.nEmployeeID).ToList();
                    if (wfh.Count() != 0)
                    {
                        sumWFH = wfh.Count();
                        // f.sWFHMonth = wfh.dWFH.Month;
                    }
                    #endregion

                    int beforeDecimal = 0;
                    int afterDecimal = 0;
                    if (sumLeaveUse != 0)
                    {
                        string[] splitSumLeaveUse = (sumLeaveUse).ToString().Split(".");
                        beforeDecimal = int.Parse(splitSumLeaveUse[0]);
                        afterDecimal = int.Parse(splitSumLeaveUse[1]);
                    }

                    f.sLeaveRequest = sumLeaveUse == 0 ? "-" : beforeDecimal.ToString() + " วัน " + afterDecimal.ToString() + " ชม.";

                    // f.sLeaveRequest = sumLeaveUse == 0 ? "-" : (sumLeaveUse / 1).ToString() + " วัน " + ((sumLeaveUse % 1) * 10).ToString() + " ชม.";
                    f.sOTRequest = sumEstimateHour == 0 ? "-" : sumEstimateHour.ToString();
                    f.sLateTime = sumLate == 0 ? "-" : sumLate.ToString();
                    f.sWFHRequest = sumWFH == 0 ? "-" : sumWFH.ToString();
                });
                #endregion

                var lstDate = allEmp.Select(s => s.dDateWork.Date).ToList();

                #region cal wfh
                int sumWFH = 0;
                var findWFHMatch = lstWFH.Where(w => lstDate.Contains(w.dWFH.Date));
                if (findWFHMatch != null)
                {
                    TB_WFH[] aa = findWFHMatch.ToArray();
                    sumWFH = aa.Count();
                }

                result.nCalculateWFH = sumWFH;
                #endregion

                // #region cal late time
                // var dupArray = allEmp.ToArray();
                // int nCalLate = 0;
                // var arrCalculate = new List<int>();

                // foreach (var date in allEmp)
                // {
                //     foreach (var item in dupArray)
                //     {
                //         if (item.dTimeDate == date.dDateWork)
                //         {
                //             arrCalculate.Add(item.sLateTime.ToInt());
                //         }
                //     }
                // }
                // nCalLate = arrCalculate.Sum();
                // result.nCalculateLateTime = nCalLate;
                // #endregion

                // #region cal leave time
                // decimal nSumLeave;
                // var lstleave = lstRequestLeave.Where(w => lstDate.Contains(w.dStartDateTime.Date)).ToList();
                // nSumLeave = lstleave.Select(s => s.nLeaveUse).Sum();

                // int beforeDecimal = 0;
                // int afterDecimal = 0;
                // if (nSumLeave != 0)
                // {
                //     string[] splitSumLeaveUse = (nSumLeave).ToString().Split(".");
                //     beforeDecimal = int.Parse(splitSumLeaveUse[0]);
                //     afterDecimal = int.Parse(splitSumLeaveUse[1]);
                // }

                // result.sCalculateLeave = nSumLeave == 0 ? "0 วัน" : beforeDecimal.ToString() + " วัน " + afterDecimal.ToString() + " ชม.";
                // #endregion

                allEmp = allEmp.OrderBy(o => o.No).ToList();

                #region//SORT
                string sSortColumn = objFilter.sSortExpression;
                switch (objFilter.sSortExpression)
                {
                    case "No": sSortColumn = "No"; break;
                }
                if (objFilter.isASC)
                {
                    allEmp = allEmp.OrderBy<cEmployeeList>(sSortColumn).ToList();
                }
                else if (objFilter.isDESC)
                {
                    allEmp = allEmp.OrderByDescending<cEmployeeList>(sSortColumn).ToList();
                }
                #endregion

                result.lstData = allEmp;
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