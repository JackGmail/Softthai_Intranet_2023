using System;
using System.Globalization;
using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Extensions.Common.STEmail;
using Extensions.Common.STExtension;
using Backend.Interfaces;
using Backend.Enum;
using static Backend.Enum.EnumWFH;
using ST.INFRA;
using Backend.Models.Authentication;
using Backend.Models;
using static Backend.Enum.EnumMeeting;
using Backend.Models.Back.Permission;
using ST.INFRA.Common;
using ST_API.Models;
using Extensions.Common.STFunction;

namespace Backend.Service
{
    /// <summary>
    /// </summary>
    public class WFHService : IWFHService
    {
        private readonly IConfiguration _config;
        private readonly IAuthentication _authen;
        private readonly IHostEnvironment _env;
        private readonly ST_IntranetEntity _db;

        /// <summary>
        /// </summary>
        public WFHService(IConfiguration config, IAuthentication authen, IHostEnvironment env, ST_IntranetEntity db)
        {
            _config = config;
            _authen = authen;
            _env = env;
            _db = db;
        }

        public ResultAPI SaveData(cWFHSave param)
        {
            ResultAPI result = new ResultAPI();
            var UserAccount = _authen.GetUserAccount();
            int nUserID = UserAccount.nUserID;
            int nID = !string.IsNullOrEmpty(param.sWFHID) ? param.sWFHID.DecryptParameter().toInt() : 0;
            try
            {
                DateTime dNow = DateTime.Now;
                TB_WFH? ObjectWFH = _db.TB_WFH.FirstOrDefault(w => w.nWFHID == nID && w.IsDelete != true);
                DateTime dnewWFH = Convert.ToDateTime(param.dWFHDate, CultureInfo.InvariantCulture);

                #region Date current
                if (dnewWFH.Date < dNow.Date)
                {
                    result.nStatusCode = StatusCodes.Status409Conflict;
                    result.sMessage = "วันที่ " + dnewWFH.ToStringFromDateTime(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH) + " เลยวันปัจจุบันมาแล้ว";
                    return result;
                }
                #endregion

                #region Check วันซ้ำ
                bool IsDup = _db.TB_WFH.Any(w => !w.IsDelete && w.nCreateBy == nUserID && w.nFlowProcessID != 1 && w.nFlowProcessID != 5 && w.nFlowProcessID != 6 && w.nFlowProcessID != 7 && w.dWFH == Convert.ToDateTime(param.dWFHDate, CultureInfo.InvariantCulture));
                if (param.nMode != 1 && IsDup)
                {
                    result.nStatusCode = StatusCodes.Status409Conflict;
                    result.sMessage = "วันที่ " + dnewWFH.ToStringFromDateTime(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH) + " ได้มีการขอ Work From Home ในระบบแล้ว";
                    return result;
                }
                #endregion

                if (!param.IsSos)
                {
                    #region condition
                    bool IsHaveDateLeaveWFH = _db.TB_Request_Leave.Where(w => !w.IsDelete && w.nCreateBy == nUserID && w.nStatusID != 0 && w.nStatusID != 1 && dnewWFH.Date >= w.dStartDateTime.Date && dnewWFH.Date <= w.dEndDateTime.Date).Any();
                    bool IsNotPassManHour = param.nManhour < 8;
                    if (IsNotPassManHour && !IsHaveDateLeaveWFH)
                    {
                        result.nStatusCode = StatusCodes.Status409Conflict;
                        result.sMessage = @"Task งานของคุณ มีจำนวน Manhour ไม่ถึง 8 ชั่วโมง (ยกเว้นถ้ามีลาเป็นชั่วโมง)";
                        return result;
                    }

                    List<TB_WFH>? lstCheck5d = _db.TB_WFH.Where(w => w.IsEmergency == false && !w.IsDelete && w.nCreateBy == nUserID && w.nFlowProcessID != 1 && w.nFlowProcessID != 7 && w.dCreate.Month == DateTime.Now.Month && w.dCreate.Year == DateTime.Now.Year).ToList();
                    bool IsWFH5D = lstCheck5d.Count >= 5;
                    if (IsWFH5D)
                    {
                        result.nStatusCode = StatusCodes.Status409Conflict;
                        result.sMessage = @"การขอ Work From Home ของคุณครบตามกำหนดแล้วในเดือนนีิ";
                        return result;
                    }

                    TB_Employee? dtEmp = _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive && w.nEmployeeID == nUserID).FirstOrDefault();
                    if (dtEmp != null)
                    {
                        if (!dtEmp.dPromote.HasValue)
                        {
                            result.nStatusCode = StatusCodes.Status409Conflict;
                            result.sMessage = @"คุณยังไม่เป็นพนักงานประจำ";
                            return result;
                        }

                        if (dtEmp.dWorkStart.HasValue)
                        {
                            int nMonth6 = GetMonthsBetween(dnewWFH, dtEmp.dWorkStart.Value);
                            if (nMonth6 < 6)
                            {
                                result.nStatusCode = StatusCodes.Status409Conflict;
                                result.sMessage = @"อายุงานของคุณไม่เข้าเงื่อนไขการขอ Work From Home (ต้องมีอายุงานไม่น้อยกว่า 6 เดือน)";
                                return result;
                            }
                        }
                    }
                    #endregion
                }

                if (ObjectWFH == null)
                {
                    ObjectWFH = new TB_WFH
                    {
                        nWFHID = (_db.TB_WFH.Any() ? _db.TB_WFH.Max(m => m.nWFHID) : 0) + 1,
                        dCreate = DateTime.Now,
                        nCreateBy = nUserID,
                        IsDelete = false
                    };
                    _db.TB_WFH.Add(ObjectWFH);
                }
                DateTime dWFH = Convert.ToDateTime(param.dWFHDate, CultureInfo.InvariantCulture);
                ObjectWFH.dWFH = dWFH;
                ObjectWFH.nManhour = param.nManhour;
                ObjectWFH.nFlowProcessID = param.nMode;
                ObjectWFH.IsOnsite = param.IsOnsite;
                ObjectWFH.nApproveBy = param.nApproveBy;
                ObjectWFH.dUpdate = DateTime.Now;
                ObjectWFH.nUpdateBy = nUserID;
                ObjectWFH.IsEmergency = param.IsSos;
                _db.SaveChanges();

                if (param.lstDataTask != null)
                {
                    List<int> lstUsingID = new();
                    foreach (var item in param.lstDataTask)
                    {
                        int newIDTask = 0;
                        if (item.nTypeRequest == 2)
                        {
                            var ObjectTask = _db.TB_Task.FirstOrDefault(w => w.nEmployeeID == nUserID && w.nTaskID == item.nTaskID.toInt() && w.IsDelete != true);
                            if (ObjectTask == null) //Add new Data
                            {
                                ObjectTask = new()
                                {
                                    nTaskID = _db.TB_Task.Any() ? _db.TB_Task.Max(m => m.nTaskID) + 1 : 1,
                                    nEmployeeID = nUserID,
                                    dCreate = DateTime.Now,
                                    nCreateBy = nUserID,
                                    //IsComplete = false,
                                    IsDelete = false
                                };
                                _db.TB_Task.Add(ObjectTask);
                            }
                            newIDTask = ObjectTask.nTaskID;
                            ObjectTask.nProjectID = item.nProjectID;
                            DateTime dTask = Convert.ToDateTime(dWFH, CultureInfo.InvariantCulture);
                            ObjectTask.dTask = dTask;
                            ObjectTask.sDescription = item.sDetail != null ? item.sDetail : "";
                            ObjectTask.nPlan = item.nPlan;
                            ObjectTask.nPlanProcess = item.nPlanProcess;
                            ObjectTask.nTaskTypeID = item.nTaskTypeID;
                            ObjectTask.nTaskStatusID = 1;
                            ObjectTask.nTypeRequest = item.nTypeRequest;            //1 = Task , 2 = WFH
                            //ObjectTask.nPlanTypeID = item.nTypeRequest == 1 ? 111 : 112;            //111 = Plan , 112 = Other Plan
                            ObjectTask.dUpdate = DateTime.Now;
                            ObjectTask.nUpdateBy = nUserID;
                            _db.SaveChanges();
                        }

                        TB_WFHTask? ObjectTaskWFH = _db.TB_WFHTask.FirstOrDefault(w => w.nTaskID == item.nPlanID && !w.IsDelete);
                        if (ObjectTaskWFH == null) //Add new Data
                        {
                            ObjectTaskWFH = new()
                            {
                                nTaskID = _db.TB_WFHTask.Any() ? _db.TB_WFHTask.Max(m => m.nTaskID) + 1 : 1,
                                nOrder = _db.TB_WFHTask.Any(a => a.nWFHID == nID) ? _db.TB_WFHTask.Where(w => w.nWFHID == nID).Max(m => m.nOrder) + 1 : 1,
                                nWFHID = ObjectWFH.nWFHID,
                                dCreate = DateTime.Now,
                                nCreateBy = nUserID,
                                IsDelete = false
                            };
                            _db.TB_WFHTask.Add(ObjectTaskWFH);
                        }
                        ObjectTaskWFH.nPlanType = item.nTypeRequest == 2 ? 112 : 111;
                        ObjectTaskWFH.nPlanID = item.nTypeRequest == 2 ? newIDTask : item.nTaskID.toInt();
                        ObjectTaskWFH.dUpdate = DateTime.Now;
                        ObjectTaskWFH.nUpdateBy = nUserID;
                        _db.SaveChanges();

                        lstUsingID.Add(ObjectTaskWFH.nTaskID);
                    }

                    var LstdtNotUsing = _db.TB_WFHTask.Where(w => !w.IsDelete && w.nWFHID == nID && !lstUsingID.Contains(w.nTaskID)).ToList();
                    if (LstdtNotUsing.Any())
                    {
                        foreach (var item in LstdtNotUsing)
                        {
                            item.IsDelete = true;
                            item.dDelete = DateTime.Now;
                            item.nDeleteBy = nUserID;
                        }
                        _db.SaveChanges();
                    }
                }

                int nPositionID = _db.TB_Employee_Position.Where(w => w.nEmployeeID == nUserID).Select(s => s.nPositionID).FirstOrDefault();

                cWFHFlow oWFH = new()
                {
                    nWFHID = ObjectWFH.nWFHID,
                    nRequesterID = nUserID,
                    nRequesterPositionID = nPositionID,
                    nMode = param.nMode,
                    sComment = param.sComment
                };

                List<cLineApprover>? lstApprover = new();
                cLineApprover? HrApprover = _db.TB_Position.Select(s => new cLineApprover
                {
                    nWFHID = ObjectWFH.nWFHID,
                    nPositionID = (int)LineApprover.HR,
                    IsLineApprover = true,
                }).FirstOrDefault();

                if (HrApprover != null) lstApprover.Add(HrApprover);

                cLineApprover? Approver = (from emp in _db.TB_Employee.Where(w => w.nEmployeeID == param.nApproveBy)
                                           from p in _db.TB_Employee_Position.Where(w => w.nEmployeeID == emp.nEmployeeID).DefaultIfEmpty()
                                           select new cLineApprover
                                           {
                                               nWFHID = ObjectWFH.nWFHID,
                                               nApproveID = emp.nEmployeeID,
                                               nPositionID = p != null ? p.nPositionID : 0,
                                               IsLineApprover = true,
                                           }).FirstOrDefault();
                if (Approver != null) lstApprover.Add(Approver);

                oWFH.lstApprover = lstApprover;

                cResWFHFlow Result = WFHWorkflow(oWFH);

                result.objResult = ObjectWFH.nWFHID.EncryptParameter();

                result.nStatusCode = StatusCodes.Status200OK;

            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }

        /// <summary>
        /// </summary>
        public ResultAPI OptionApproverWFH(ParamProjectID Param)
        {
            var nUserID = _authen.GetUserAccount().nUserID;
            ResultAPI result = new();
            try
            {
                if (Param.lstProjectID != null && Param.lstProjectID.Any())
                {
                    var IntProjectID = Param.lstProjectID.ToArray();
                    var lstApproveByProject = _db.TB_Project_Person.Where(w => !w.IsDelete && w.IsStatus && w.nPositionID == 3 && IntProjectID.Contains(w.nProjectID)).Select(s => s.nEmployeeID).ToArray();
                    var lstApproveByUser = _db.TB_Employee_Report_To.Where(w => !w.IsDelete && w.nEmployeeID == nUserID).Select(s => s.nRepEmployeeID ?? 0).ToArray();
                    var lstApprove = lstApproveByProject.Concat(lstApproveByUser).Distinct().ToArray();

                    var OptionApprove = (from a in _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive && lstApprove.Contains(w.nEmployeeID))
                                         from b in _db.TB_Employee_Position.Where(w => w.nEmployeeID == a.nEmployeeID).DefaultIfEmpty()
                                         from c in _db.TB_Position.Where(w => w.nPositionID == b.nPositionID).DefaultIfEmpty()
                                         select new cSelectOptions
                                         {
                                             value = a.nEmployeeID.ToString(),
                                             label = a.sNameTH + " " + a.sSurnameTH + " (" + a.sNickname + ")",
                                             Parent = b.nPositionID.ToString(),
                                             sPosition = c.sPositionName
                                         }).ToList();

                    result.objResult = OptionApprove;
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
        public void WFHHistory(cWFHRequestHistory oWFH)
        {
            // TokenJWTSecret oUser = _authen.GetUserAccount();
            var oUser = _authen.GetUserAccount();
            try
            {
                int nHistoryID = _db.TB_WFHFlowHistory.Any() ? _db.TB_WFHFlowHistory.Max(m => m.nHistoryID) + 1 : 1;
                TB_WFHFlowHistory oHistory = new TB_WFHFlowHistory()
                {
                    nHistoryID = nHistoryID,
                    nWFHID = oWFH.nWFHID,
                    nFlowProcessID = oWFH.nFlowProcessID,
                    nApproveBy = oWFH.nApproveBy,
                    dApprove = oWFH.dApprove,
                    sDescription = oWFH.sComment,
                    dCreate = DateTime.Now,
                    nCreateBy = oUser.nUserID,
                    dUpdate = DateTime.Now,
                    nUpdateBy = oUser.nUserID,
                    IsDelete = false
                };

                _db.TB_WFHFlowHistory.Add(oHistory);
                _db.SaveChanges();
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        #region ตี้
        /// <summary>
        /// </summary>
        public clcResultTableWFH GetDataTable(clcFilterWFH oWFH)
        {
            // TokenJWTSecret oUser = _authen.GetUserAccount();
            UserAccount? oUser = _authen.GetUserAccount();
            clcResultTableWFH result = new clcResultTableWFH();
            try
            {
                string[]? listRequest = oWFH.listRequest?.Split(",");
                List<int?>? nlistRequest = listRequest != null ? listRequest.Select(s => Int32.TryParse(s, out int n) ? n : (int?)null).ToList() : null;
                string[]? listStatus = oWFH.listStatus?.Split(",");
                List<int?>? nlistStatus = listStatus != null ? listStatus.Select(s => Int32.TryParse(s, out int n) ? n : (int?)null).ToList() : null;

                // bool isHasEmp = _db.TB_Employee_Position.Any(w => w.nEmployeeID == oUser.nUserID && !w.IsDelete);
                // if (nlistRequest != null && nlistRequest.Any() && isHasEmp)
                // {
                //     nlistRequest = _db.TB_Employee_Position.FirstOrDefault(w => w.nEmployeeID == oUser.nUserID && !w.IsDelete)?.nPositionID == (int)ActionType.Hr ? null : nlistRequest;
                // }
                DateTime? dStart = oWFH.sStart != null ? Convert.ToDateTime(oWFH.sStart, CultureInfo.InvariantCulture) : null;
                DateTime? dEnd = oWFH.sEnd != null ? Convert.ToDateTime(oWFH.sEnd, CultureInfo.InvariantCulture) : DateTime.Now;


                var listGroupHS = _db.TB_WFHFlowHistory.Where(w => w.nFlowProcessID != 0 && !w.IsDelete).GroupBy(entry => entry.nWFHID)
               .Select(group => new
               {
                   Group = group.Key,
                   LastDate = group.Max(entry => entry.dApprove)
               });

                var TBWFHFlowHistory = _db.TB_WFHFlowHistory.Where(w => listGroupHS.Select(s => s.Group).Contains(w.nWFHID) && listGroupHS.Select(s => s.LastDate).Contains(w.dApprove));

                List<int>? listChecDraft = _db.TB_WFH.Where(w => w.nCreateBy == oUser.nUserID && w.nFlowProcessID == (int)WFHStatus.Draft).Select(s => s.nWFHID).ToList();

                List<objResultTableWFH>? TableAll = (from a in _db.TB_WFH.Where(w => (nlistRequest != null ? nlistRequest.Contains(w.nCreateBy) : true)
                                           && (nlistStatus == null || nlistStatus.Contains(w.nFlowProcessID)) && !w.IsDelete)
                                                     from test in TBWFHFlowHistory.Where(w => w.nWFHID == a.nWFHID).DefaultIfEmpty()
                                                         //  from c in _db.TB_WFHFlowHistory.Where(w => w.nFlowProcessID != 0 && test.LastDate == w.dApprove).DefaultIfEmpty()
                                                     select new objResultTableWFH
                                                     {
                                                         nID = a.nWFHID,
                                                         sID = a.nWFHID.EncryptParameter(),
                                                         sWFHDate = a.dWFH.ToStringFromDate("dd/MM/yyyy ", "th-TH"),
                                                         sRequestDate = a.dUpdate.ToStringFromDate("dd/MM/yyyy ", "th-TH"),
                                                         sStatus = _db.TM_WFHFlowProcess.Where(w => w.nFlowProcessID == a.nFlowProcessID).Select(w => w.sProcess).FirstOrDefault() ?? "",
                                                         sComment = test != null ? test.sDescription ?? "" : "",
                                                         nFlowProcessID = a.nFlowProcessID,
                                                         dWFH = a.dWFH,
                                                         dUpdate = a.dUpdate,
                                                         nCreateBy = a.nCreateBy,
                                                     }).ToList();
                foreach (objResultTableWFH? item in TableAll)
                {
                    item.IsCanDel = item.nCreateBy == oUser.nUserID;
                    item.lsitTask = (from ptask in _db.TB_WFHTask.Where(w => item.nID == w.nWFHID && !w.IsDelete)
                                     from pt in _db.TB_Task.Where(w => w.nTaskID == ptask.nPlanID && !w.IsDelete).DefaultIfEmpty()
                                     from pj in _db.TB_Project.Where(w => w.nProjectID == pt.nProjectID && !w.IsDelete).DefaultIfEmpty()
                                     select new objTaskTable
                                     {
                                         Project = pj.sProjectName,
                                         Progress = pt.nPlanProcess,
                                         Name = pt.sDescription.Replace("\n", "<br/>"),
                                         Manhour = pt.nPlan ?? new decimal(0.00)
                                     }).ToList();
                }

                switch (oWFH.sType.toInt())
                {
                    case (int)WFHType.WFHDate:
                        TableAll = TableAll.Where(w => dEnd.Value.Date >= w.dWFH.Date).OrderBy(w => w.dWFH).ToList();
                        if (dStart != null)
                        {
                            TableAll = TableAll.Where(w => w.dWFH.Date >= dStart.Value.Date).ToList();
                        }
                        if (dEnd != null)
                        {
                            TableAll = TableAll.Where(w => w.dWFH.Date <= dEnd.Value.Date).ToList();
                        }
                        break;
                    case (int)WFHType.RequestDate:
                        TableAll = TableAll.Where(w => dEnd.Value.Date >= w.dUpdate.Date).OrderBy(w => w.dUpdate).ToList();
                        if (dStart != null)
                        {
                            TableAll = TableAll.Where(w => w.dUpdate.Date >= dStart.Value.Date).ToList();
                        }
                        if (dEnd != null)
                        {
                            TableAll = TableAll.Where(w => w.dUpdate.Date <= dEnd.Value.Date).ToList();
                        }
                        break;
                }



                #region//SORT
                string sSortColumn = oWFH.sSortExpression;
                switch (oWFH.sSortExpression)
                {
                    case "sWFHDate": sSortColumn = "dWFH"; break;
                    case "sRequestDate": sSortColumn = "dUpdate"; break;
                    case "sRequester": sSortColumn = "sRequester"; break;
                    case "sWaitingBy": sSortColumn = "sWaitingBy"; break;
                    case "sStatus": sSortColumn = "sStatus"; break;
                    case "sRemark": sSortColumn = "sRemark"; break;
                }
                if (oWFH.isASC)
                {
                    TableAll = TableAll.OrderBy<objResultTableWFH>(sSortColumn).ToList();
                }
                else if (oWFH.isDESC)
                {
                    TableAll = TableAll.OrderByDescending<objResultTableWFH>(sSortColumn).ToList();
                }
                #endregion

                int index = 0;
                IQueryable<TB_Employee> TBEmployee = _db.TB_Employee.AsQueryable();
                IQueryable<TB_Position> TBPosition = _db.TB_Position.AsQueryable();
                string resultFlow = " - ";
                List<objResultTableWFH> listfinal = new List<objResultTableWFH>();
                foreach (objResultTableWFH? item in TableAll)
                {
                    index++;
                    item.No = index;
                    item.sRequester = GetNameEmp(item.nCreateBy, 1);
                    item.sWaitingBy = GetNameEmp(item.nID, 2);
                    item.IsRequester = item.nCreateBy == oUser.nUserID;
                    TB_WFHFlow? oCurrentApproveFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == item.nID && !w.dApprove.HasValue).OrderBy(o => o.nLevel).FirstOrDefault();
                    if (oCurrentApproveFlow?.nApproveBy != null)
                    {
                        resultFlow = TBEmployee.Where(w => oCurrentApproveFlow.nApproveBy == w.nEmployeeID).Select(s => s.nEmployeeID + "").FirstOrDefault() ?? "";
                        item.IsApprover = resultFlow == (oUser.nUserID + "");
                    }
                    else
                    {
                        resultFlow = TBPosition.Where(w => oCurrentApproveFlow != null ? oCurrentApproveFlow.nPositionID == w.nPositionID : false).Select(s => s.sPositionName).FirstOrDefault() ?? "";
                        item.IsApprover = resultFlow == oUser.sPosition;
                    }
                    if (item.nFlowProcessID == (int)WFHStatus.Draft && listChecDraft.Contains(item.nID))
                    {
                        listfinal.Add(item);
                    }
                    else if (item.nFlowProcessID != (int)WFHStatus.Draft)
                    {
                        listfinal.Add(item);
                    }


                }


                STGrid.Pagination? dataPage = STGrid.Paging(oWFH.nPageSize, oWFH.nPageIndex, listfinal.Count());
                List<objResultTableWFH>? lstData = listfinal.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                result.lstData = lstData;
                result.nDataLength = dataPage.nDataLength;
                result.nPageIndex = dataPage.nPageIndex;
                result.nSkip = dataPage.nSkip;
                result.nTake = dataPage.nTake;
                result.nStartIndex = dataPage.nStartIndex;
                result.Status = 200;

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
        public clcResultTableWFH GetOption()
        {
            // TokenJWTSecret oUser = _authen.GetUserAccount();
            UserAccount oUser = _authen.GetUserAccount();
            clcResultTableWFH result = new clcResultTableWFH();
            try
            {
                List<string> listFlow = new List<string>();

                List<cSelectOption> listOption = new List<cSelectOption>();
                IQueryable<TM_WFHFlowProcess> TMWFHFlowProcess = _db.TM_WFHFlowProcess.Where(w => w.IsActive).AsQueryable();
                foreach (TM_WFHFlowProcess? item in TMWFHFlowProcess)
                {
                    cSelectOption a = new cSelectOption();
                    a.value = item.nFlowProcessID + "";
                    a.label = item.sProcess;
                    listOption.Add(a);
                    listFlow.Add(item.nFlowProcessID + "");
                }




                List<int> listUser = new();
                // listUser.Add(oUser.nUserID + "");
                IQueryable<TB_Employee_Report_To> TBEmployeeReportTo = _db.TB_Employee_Report_To.Where(w => !w.IsDelete).OrderBy(o => o.nEmployeeID).AsQueryable();

                // TB_Employee_Report_To oEmployeeReportTo = TBEmployeeReportTo.Where(w=> )
                listUser.Add(oUser.nUserID);
                var listEmployeeReportTo = (from a in _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive)
                                            from b in TBEmployeeReportTo.Where(w => w.nRepEmployeeID == oUser.nUserID && a.nEmployeeID == w.nEmployeeID)
                                            select new { b }).ToList();
                int idx = 0;
                foreach (var item in listEmployeeReportTo)
                {
                    if (idx == 0)
                    {
                        listUser.Add(oUser.nUserID);
                    }
                    listUser.Add(item.b.nEmployeeID);
                    idx++;
                }
                listUser = listUser.Distinct().ToList();
                List<cSelectOption> listReq = new List<cSelectOption>();
                IQueryable<TB_Employee> TBEmployeeReq = _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive && listUser.Contains(w.nEmployeeID)).OrderBy(w => w.nEmployeeID).AsQueryable();
                foreach (TB_Employee? item in TBEmployeeReq)
                {
                    cSelectOption a = new cSelectOption();
                    a.value = item.nEmployeeID + "";
                    a.label = item.sNameTH + " " + item.sSurnameTH + "( " + item.sNickname + " )";
                    listReq.Add(a);
                }
                IQueryable<TB_Employee> TBEmployee = _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive && !listUser.Contains(w.nEmployeeID)).OrderBy(w => w.nEmployeeID).AsQueryable();
                List<cSelectOption> listEmp = new List<cSelectOption>();
                foreach (TB_Employee? item in TBEmployee)
                {
                    cSelectOption a = new cSelectOption();
                    a.value = item.nEmployeeID + "";
                    a.label = item.sNameTH + " " + item.sSurnameTH + "( " + item.sNickname + " )";
                    listEmp.Add(a);
                }
                listReq.AddRange(listEmp);

                // ทำของ user ก่อน รอ Table ว่าเราอยู่กลุ่มไหน

                // var
                result.listFlow = listFlow;
                result.listUser = listUser.Select(s => s.ToString()).ToList();
                result.listEmp = listReq;
                result.listOpton = listOption;
                result.Status = 200;
            }
            catch (Exception ex)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public ResultAPI DeleteData(cReqDeleteUser param)
        {
            ResultAPI result = new ResultAPI();
            UserAccount? oUser = _authen.GetUserAccount();
            try
            {
                //ParamJWT? UserAccount = _Auth.GetUserAccount();
                //int nUserID = UserAccount.nUserID ?? 0;
                List<int> lstId = param.lstDelete.ConvertAll(item => item.DecryptParameter().ToInt()).ToList();
                List<TB_WFH>? oWFH = _db.TB_WFH.Where(w => lstId.Contains(w.nWFHID)).ToList();
                if (oWFH != null)
                {
                    foreach (var item in oWFH)
                    {
                        item.IsDelete = true;
                        item.nDeleteBy = oUser.nUserID;
                        item.dDelete = DateTime.Now;
                    }
                }
                _db.SaveChanges();
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
        public string GetNameEmp(int nWFHRequestID, int mode)
        {
            string result = " - ";
            // TokenJWTSecret oUser = _authen.GetUserAccount();
            var oUser = _authen.GetUserAccount();


            try
            {
                IQueryable<TB_Employee> TBEmployee = _db.TB_Employee.AsQueryable();
                IQueryable<TB_Position> TBPosition = _db.TB_Position.AsQueryable();
                if (mode == 2)
                {
                    TB_WFHFlow? oCurrentApproveFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == nWFHRequestID && !w.dApprove.HasValue).OrderBy(o => o.nLevel).FirstOrDefault();
                    if (oCurrentApproveFlow?.nApproveBy != null)
                    {
                        result = TBEmployee.Where(w => oCurrentApproveFlow.nApproveBy == w.nEmployeeID).Select(s => s.sNameEN + " " + s.sSurnameEN).FirstOrDefault() ?? "";
                    }
                    else
                    {
                        result = TBPosition.Where(w => oCurrentApproveFlow != null ? oCurrentApproveFlow.nPositionID == w.nPositionID : false).Select(s => s.sPositionAbbr).FirstOrDefault() ?? "";

                    }
                }
                else
                {
                    result = TBEmployee.Where(w => nWFHRequestID == w.nEmployeeID).Select(s => s.sNameEN + " " + s.sSurnameEN).FirstOrDefault() ?? "";
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }



        #endregion
        /// <summary>
        /// </summary>
        public cInitialDataWFH GetInitData()
        {
            cInitialDataWFH result = new cInitialDataWFH();
            try
            {
                ParamJWT UserAccount = _authen.GetUserAccount();
                int nUserID = UserAccount.nUserID;
                var objUser = _db.TB_Employee.Where(w => w.nEmployeeID == nUserID).FirstOrDefault();
                result.lstTypeTask = _db.TM_Data.Where(w => w.nDatatypeID == 32).Select(s => new cSelectOption
                {
                    value = s.nData_ID.ToString(),
                    label = (s.sNameTH ?? s.sNameEng)
                }).ToList();

                result.lstProject = _db.TB_Project.Where(w => w.IsDelete != true).Select(s => new cSelectOption
                {
                    value = s.nProjectID.ToString(),
                    label = !string.IsNullOrEmpty(s.sProjectAbbr) ? s.sProjectAbbr + " : " + s.sProjectName : s.sProjectName
                }).ToList();

                result.lstPosition = _db.TB_Position.Where(w => w.IsDelete != true).Select(s => new cSelectOption
                {
                    value = s.nPositionID.ToString(),
                    label = s.sPositionName
                }).ToList();

                result.lstApprover = (from a in _db.TB_Employee.Where(w => !w.IsDelete)
                                      from b in _db.TB_Employee_Position.Where(w => w.nEmployeeID == a.nEmployeeID).DefaultIfEmpty()
                                      from c in _db.TB_Position.Where(w => w.nPositionID == b.nPositionID).DefaultIfEmpty()
                                      select new cSelectOptions
                                      {
                                          value = a.nEmployeeID.ToString(),
                                          label = a.sNameTH + " " + a.sSurnameTH + " (" + a.sNickname + ")",
                                          Parent = b.nPositionID.ToString(),
                                          sPosition = c.sPositionName
                                      }).ToList();
                int nYear = DateTime.Now.Year;

                result.arrHoliday = _db.TB_HolidayDay.Where(w => w.IsActivity && !w.IsDelete && w.nYear == nYear).Select(s => s.dDate.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_dd, ST.INFRA.Enum.CultureName.en_US)).ToArray();


                if (objUser != null)
                {
                    result.sName = $"{objUser.sNameTH} {objUser.sSurnameTH} ({objUser.sNickname})";
                    var objPos = _db.TB_Employee_Position.Where(w => w.nEmployeeID == nUserID).FirstOrDefault();
                    if (objPos != null)
                    {
                        result.sPosition = _db.TB_Position.Where(w => w.nPositionID == objPos.nPositionID).Select(s => s.sPositionName).FirstOrDefault();
                    }
                }


                result.Status = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;
        }
        /// <summary>
        /// </summary>
        public cResultTaskPlanTable GetDataTableTaskPlan(cTaskTable param)
        {
            cResultTaskPlanTable result = new cResultTaskPlanTable();
            var UserAccount = _authen.GetUserAccount();
            int nUserID = UserAccount.nUserID;
            var lstID = param.lstDataTask?.Select(s => s.nTaskID).ToList();
            try
            {
                int nID = param.sWFHID.DecryptParameter().toInt();

                var lstSelect = param.lstSelect != null && param.lstSelect.Any() ? param.lstSelect.Select(s => s.toInt()).ToArray() : null;

                IQueryable<TB_Task>? lstTask = _db.TB_Task.Where(w => (w.nEmployeeID == nUserID && !w.IsDelete && w.nTypeRequest == 1) || (lstSelect != null && lstSelect.Any() && lstSelect.Contains(w.nTaskID))).AsQueryable();
                IQueryable<TB_Project> lstProject = _db.TB_Project.Where(w => !w.IsDelete).AsQueryable();
                IQueryable<TM_Data> lstData = _db.TM_Data.Where(w => !w.IsDelete).AsQueryable();
                var lstWFHTask = _db.TB_WFHTask.Where(w => !w.IsDelete && w.nCreateBy == nUserID).Select(s => s.nPlanID).ToArray();

                #region Filter
                DateTime? dStart = param.dStart != null ? Convert.ToDateTime(param.dStart, CultureInfo.InvariantCulture) : null;
                DateTime? dEnd = param.dEnd != null ? Convert.ToDateTime(param.dEnd, CultureInfo.InvariantCulture) : null;
                DateTime? nDateWFH = param.dEnd != null ? Convert.ToDateTime(param.dEnd, CultureInfo.InvariantCulture) : null;

                if (param.nProjectID.HasValue)
                {
                    lstTask = lstTask.Where(w => (w.nProjectID == param.nProjectID.Value) || (lstSelect != null && lstSelect.Any() && lstSelect.Contains(w.nTaskID))).AsQueryable();
                }

                if (dStart.HasValue && dEnd.HasValue)
                {
                    lstTask = lstTask.Where(w => (dStart.Value <= w.dTask.Date && dEnd.Value.Date >= w.dTask.Date) || (lstSelect != null && lstSelect.Any() && lstSelect.Contains(w.nTaskID))).AsQueryable();
                }
                #endregion


                #region Qeury
                List<ObjectResultTaskPlan>? qryListPlan = (from t in lstTask
                                                           from p in lstProject.Where(w => w.nProjectID == t.nProjectID)
                                                           from d in lstData.Where(w => w.nData_ID == t.nTaskTypeID)
                                                           from wt in _db.TB_WFHTask.Where(w => w.nPlanID == t.nTaskID && w.nWFHID == nID && !w.IsDelete).DefaultIfEmpty()
                                                           select new ObjectResultTaskPlan
                                                           {
                                                               sID = t.nTaskID.ToString(),
                                                               nTaskID = t.nTaskID,
                                                               nPlanID = wt != null ? wt.nTaskID : 0,
                                                               nProjectID = t.nProjectID,
                                                               sProjectName = p.sProjectName,
                                                               sDetail = t.sDescription,
                                                               nPlan = t.nPlan,
                                                               nPlanProcess = t.nPlanProcess,
                                                               nTypeRequest = 1,
                                                               nTaskTypeID = t.nTaskTypeID,
                                                               sTaskType = d.sNameEng,
                                                               dTask = t.dTask,
                                                               sDate = t.dTask.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                                                               dUpdate = t.dUpdate,
                                                               sUpdate = t.dUpdate.ToStringFromDate("dd/MM/yyyy" + " " + "HH:mm", "th-TH"),
                                                               IsNotSelect = lstWFHTask.Contains(t.nTaskID)
                                                           }).ToList();
                #endregion

                #region//SORT
                string sSortColumn = param.sSortExpression;
                switch (param.sSortExpression)
                {
                    case "sDetail": sSortColumn = "sDetail"; break;
                    case "sProjectName": sSortColumn = "sProjectName"; break;
                    case "sUpdate": sSortColumn = "sUpdate"; break;
                    case "sTaskType": sSortColumn = "sTaskType"; break;
                }
                if (param.isASC)
                {
                    qryListPlan = qryListPlan.OrderBy(sSortColumn).ToList();
                }
                else if (param.isDESC)
                {
                    qryListPlan = qryListPlan.OrderByDescending(sSortColumn).ToList();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qryListPlan.Count());
                var lstdata = qryListPlan.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();

                result.lstData = lstdata;
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
        public static string? GetPathUploadFile(string FilePath, string SysFileName)
        {
            string? sFullPath = null;
            if (!string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(SysFileName))
            {
                // string sPathWeb = STFunction.GetAppSettingJson("AppSetting:UrlSiteBackend");
                sFullPath = "UploadFile/" + FilePath + "/" + SysFileName;
            }
            return sFullPath;
        }
        public cReturnHistoryWFH GetHistoryWFH(string? sWFHID)
        {
            var oUser = _authen.GetUserAccount();
            cReturnHistoryWFH result = new cReturnHistoryWFH();
            int nID = sWFHID.DecryptParameter().toInt();
            try
            {
                List<ObjDataHis>? lstdata = (from FH in _db.TB_WFHFlowHistory.Where(w => w.nWFHID == nID && w.nFlowProcessID != 1)
                                             from EMP in _db.TB_Employee.Where(w => w.nEmployeeID == FH.nApproveBy).DefaultIfEmpty()
                                             from FPC in _db.TM_WFHFlowProcess.Where(w => w.nFlowProcessID == FH.nFlowProcessID).DefaultIfEmpty()
                                             from EPO in _db.TB_Employee_Position.Where(w => w.nEmployeeID == FH.nApproveBy).DefaultIfEmpty()
                                             from PO in _db.TB_Position.Where(w => w.nPositionID == EPO.nPositionID).DefaultIfEmpty()
                                             from EMPIMG in _db.TB_Employee_Image.Where(w => w.nEmployeeID == EMP.nEmployeeID).DefaultIfEmpty()
                                             select new ObjDataHis
                                             {
                                                 sName = EMP != null ? EMP.sNameTH + " " + EMP.sSurnameTH : "",
                                                 sPosition = PO.sPositionName,
                                                 sComment = string.IsNullOrEmpty(FH.sDescription) ? "-" : FH.sDescription,
                                                 nStatus = FH.nFlowProcessID,
                                                 sStatus = FH.nFlowProcessID == (int)WFHStatus.Completed ? FPC.sProcess : FPC.sAction,
                                                 dDateApprove = FH.dApprove,
                                                 sDateApprove = FH.dApprove.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmm, ST.INFRA.Enum.CultureName.th_TH),
                                                 sFileLink = GetPathUploadFile(EMPIMG.sFileName + "", EMPIMG.sSystemFileName + "") ?? null,
                                             }).ToList();

                result.lstDataHis = lstdata.OrderByDescending(o => o.dDateApprove).ToList();
                var dtWFH = _db.TB_WFH.FirstOrDefault(f => f.nWFHID == nID && !f.IsDelete);

                var lstStepper = new List<cStepper>();
                if (nID == 0 || (dtWFH != null && dtWFH.nFlowProcessID == 1))
                {
                    lstStepper.Add(new cStepper()
                    {
                        sLabel = "Submit",
                        sActionBy = "",
                        sDateAction = "",
                    });

                    lstStepper.Add(new cStepper()
                    {
                        sLabel = "HR Review",
                        sActionBy = "",
                        sDateAction = "",
                    });

                    lstStepper.Add(new cStepper()
                    {
                        sLabel = "Approver",
                        sActionBy = "",
                        sDateAction = "",
                    });
                }
                else
                {
                    lstStepper = (from a in _db.TB_WFHFlow.Where(w => w.nWFHID == nID)
                                  from b in _db.TB_Employee.Where(w => w.nEmployeeID == a.nApproveBy).DefaultIfEmpty()
                                  select new cStepper
                                  {
                                      sLabel = a.nLevel == 1 ? "Submit" : a.nLevel == 2 ? "HR Review" : a.nLevel == 3 ? "Approver" : "Completed",
                                      sActionBy = b != null ? b.sNameTH + " " + b.sSurnameTH : "",
                                      sDateAction = a.dApprove.HasValue ? a.dApprove.Value.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmm, ST.INFRA.Enum.CultureName.th_TH) : "",
                                  }).ToList();
                }

                lstStepper.Add(new cStepper()
                {
                    sLabel = "Completed",
                    sActionBy = "",
                    sDateAction = "",
                });

                result.lstStepper = lstStepper;

                int? nStepActive = null;
                if (dtWFH != null)
                {
                    TB_WFHFlow? oCurrentApproveFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == dtWFH.nWFHID && !w.dApprove.HasValue).OrderBy(o => o.nLevel).FirstOrDefault();

                    switch (dtWFH.nFlowProcessID)
                    {
                        case 1:
                            nStepActive = null;
                            break;
                        case 2:     //Wait for Approve (HR)
                            nStepActive = 0;
                            break;
                        case 3:     //Wait for Approve
                            // nStepActive = 1;
                            nStepActive = oCurrentApproveFlow != null ? oCurrentApproveFlow.nLevel - 1 : 0;
                            break;
                        case 4:     //Completed
                            nStepActive = 3;
                            break;
                        case 5:     //Recall
                        case 6:     //Reject
                            nStepActive = oCurrentApproveFlow != null && oCurrentApproveFlow.nLevel != 1 ? oCurrentApproveFlow.nLevel - 1 : null;
                            break;
                        case 7:     //Cancel
                            nStepActive = oCurrentApproveFlow != null ? oCurrentApproveFlow.nLevel - 1 : null;
                            break;
                    }
                }
                else
                {
                    nStepActive = null;
                }

                result.nFocusStep = nStepActive;

                result.nStatusCode = 200;
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
        public cReturnWFHApprove GetFormWFH(string sWFHID)
        {
            cReturnWFHApprove result = new cReturnWFHApprove();
            try
            {
                if (!string.IsNullOrEmpty(sWFHID))
                {
                    int nID = sWFHID.DecryptParameter().toInt();
                    IQueryable<TB_WFHFlowHistory> TBWFHFlowHistory = _db.TB_WFHFlowHistory.Where(w => !w.IsDelete).AsQueryable();
                    IQueryable<TB_WFH> TBWFH = _db.TB_WFH.Where(w => w.nWFHID == nID && !w.IsDelete).AsQueryable();
                    IQueryable<TB_WFHFlow> TBWFHFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == nID && !w.IsDelete).AsQueryable();
                    IQueryable<TB_Project_Plan> TBProjectPlan = _db.TB_Project_Plan.Where(w => !w.IsDelete).AsQueryable();
                    IQueryable<TB_Project_Action> TBProjectAction = _db.TB_Project_Action.Where(w => !w.IsDelete).AsQueryable();
                    IQueryable<TB_Project> TBProject = _db.TB_Project.Where(w => !w.IsDelete).AsQueryable();
                    IQueryable<TB_WFHTask> TBWFHTask = _db.TB_WFHTask.Where(w => w.nWFHID == nID && !w.IsDelete).AsQueryable();
                    IQueryable<TM_WFHFlowProcess> TMWFHFlowProcess = _db.TM_WFHFlowProcess.AsQueryable();
                    IQueryable<TB_Employee> TBEmployee = _db.TB_Employee.Where(w => !w.IsDelete).AsQueryable();
                    IQueryable<TB_Employee_Position> TBEmpPosition = _db.TB_Employee_Position.Where(w => !w.IsDelete).AsQueryable();
                    IQueryable<TB_Position> TBPosition = _db.TB_Position.Where(w => !w.IsDelete).AsQueryable();

                    #region DataInfo
                    cReturnWFHApprove? data = (from a in TBWFH
                                               from b in TBEmployee.Where(w => w.nEmployeeID == a.nCreateBy)
                                               from c in TBEmpPosition.Where(w => w.nEmployeeID == b.nEmployeeID).DefaultIfEmpty()
                                               from d in TBPosition.Where(w => w.nPositionID == c.nPositionID).DefaultIfEmpty()
                                               select new cReturnWFHApprove
                                               {
                                                   IsEmergency = a.IsEmergency,
                                                   nMode = a.nFlowProcessID,
                                                   ApproveByID = a.nApproveBy,
                                                   sName = b.sNameTH + " " + b.sSurnameTH,
                                                   sPosition = d != null ? d.sPositionName : "",
                                                   dWFHDate = a.dWFH,
                                                   IsOnsite = a.IsOnsite,
                                               }).FirstOrDefault();
                    if (data != null)
                    {
                        result.IsEmergency = data.IsEmergency;
                        result.nLevel = data.nLevel;
                        result.nMode = data.nMode;
                        result.sName = data.sName;
                        result.dWFHDate = data.dWFHDate;
                        result.sPosition = data.sPosition;
                        result.IsOnsite = data.IsOnsite;
                        result.ApproveByID = data.ApproveByID;
                    }
                    #endregion

                    #region LineApprove
                    if (result.nMode == 0 || result.nMode == 1)
                    {
                        var dtApprove = (from a in _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive && w.nEmployeeID == result.ApproveByID)
                                         from b in _db.TB_Employee_Position.Where(w => w.nEmployeeID == a.nEmployeeID).DefaultIfEmpty()
                                         from c in _db.TB_Position.Where(w => w.nPositionID == b.nPositionID).DefaultIfEmpty()
                                         select new cSelectOptions
                                         {
                                             value = a.nEmployeeID.ToString(),
                                             label = a.sNameTH + " " + a.sSurnameTH + " (" + a.sNickname + ")",
                                             Parent = b.nPositionID.ToString(),
                                             sPosition = c.sPositionName
                                         }).FirstOrDefault();

                        if (dtApprove != null)
                        {
                            var lstApprove = new List<ObjLineApprove>
                        {
                            new ObjLineApprove()
                            {
                                sID = 1,
                                nNo = 1,
                                sName = "",
                                sPosition = "Human Resources"
                            },
                            new ObjLineApprove()
                            {
                                sID = 2,
                                nNo = 2,
                                sName = dtApprove.label,
                                sPosition = dtApprove.sPosition
                            }
                        };
                            result.lstLineApprover = lstApprove;
                        }
                    }
                    else
                    {
                        List<ObjLineApprove>? lstDataLineApprover = (from b in _db.TB_WFHFlow.Where(w => w.nWFHID == nID && w.IsLineApprover)
                                                                     from c in _db.TB_Employee.Where(w => w.nEmployeeID == b.nApproveBy).DefaultIfEmpty()
                                                                     from d in _db.TB_Position.Where(w => w.nPositionID == b.nPositionID).DefaultIfEmpty()
                                                                     select new ObjLineApprove
                                                                     {
                                                                         nWFHID = b.nWFHID,
                                                                         nEmpID = c != null ? c.nEmployeeID : 0,
                                                                         nPositionID = d != null ? d.nPositionID : 0,
                                                                         sName = c != null ? c.sNameTH + " " + c.sSurnameTH + " (" + c.sNickname + ")" : "",
                                                                         sPosition = d.sPositionName,
                                                                         IsLineApprover = b.dApprove != null,
                                                                     }).ToList();
                        if (lstDataLineApprover.Count > 0)
                        {
                            int nIndx = 1;
                            foreach (var item in lstDataLineApprover)
                            {
                                item.sID = nIndx;
                                item.nNo = nIndx;
                                nIndx++;
                            }
                            result.lstLineApprover = lstDataLineApprover;
                        }
                    }

                    #endregion

                    #region Taskview
                    List<ObjTaskWFH>? lstDataTaskview = (from a in TBWFHTask.Where(w => w.nWFHID == nID)
                                                         from b in _db.TB_Task.Where(w => w.nTaskID == a.nPlanID)
                                                         from c in TBProject.Where(w => w.nProjectID == b.nProjectID)
                                                         from d in _db.TM_Data.Where(w => w.nData_ID == b.nTaskTypeID)
                                                         select new ObjTaskWFH
                                                         {
                                                             sProjectName = !string.IsNullOrEmpty(c.sProjectAbbr) ? c.sProjectAbbr + " : " + c.sProjectName : c.sProjectName,
                                                             sDescription = b.sDescription,
                                                             nPlan = b.nPlan,
                                                         }).ToList();


                    int nIndex = 1;
                    foreach (var item in lstDataTaskview)
                    {
                        item.sDescription = item.sDescription?.Replace("\n", "<br/>");
                        item.No = nIndex;
                        nIndex++;
                    }
                    result.lstTaskview = lstDataTaskview;
                    #endregion

                    #region TaskEdit
                    // List<ObjDataTask>? lstDataTaskEdit = (from a in TBWFHTask
                    //                                       from b in _db.TB_Task.Where(w => w.nTaskID == a.nPlanID)
                    //                                       from c in TBProject.Where(w => w.nProjectID == b.nProjectID)
                    //                                       from d in _db.TM_Data.Where(w => w.nData_ID == b.nTaskTypeID)
                    //                                       select new ObjDataTask
                    //                                       {
                    //                                           nTaskID = b.nTaskID,
                    //                                           sID = b.nTaskID.ToString(),
                    //                                           nID = b.nTaskID,
                    //                                           nPlanID = a.nTaskID,
                    //                                           nProjectID = c.nProjectID,
                    //                                           sProjectName = c.sProjectName,
                    //                                           nTaskTypeID = b.nTaskTypeID,
                    //                                           sTaskType = d.sNameEng,
                    //                                           nTypeRequest = b.nTypeRequest,
                    //                                           nPlan = b.nPlan,
                    //                                           nPlanProcess = b.nPlanProcess,
                    //                                           sDate = b.dTask.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy_HHmmss, ST.INFRA.Enum.CultureName.en_US),
                    //                                           sDetail = b.sDescription,
                    //                                       }).ToList();
                    // result.lstTaskWFH = lstDataTaskEdit;

                    List<ObjDataTask>? lstDataTaskTable = (from a in TBWFHTask
                                                           from b in _db.TB_Task.Where(w => w.nTaskID == a.nPlanID)
                                                           from c in TBProject.Where(w => w.nProjectID == b.nProjectID)
                                                           from d in _db.TM_Data.Where(w => w.nData_ID == b.nTaskTypeID)
                                                           select new ObjDataTask
                                                           {
                                                               nTaskID = b.nTaskID,
                                                               sID = b.nTaskID.ToString(),
                                                               nID = b.nTaskID,
                                                               nPlanID = a.nTaskID,
                                                               nPlanType = a.nPlanType,
                                                               nProjectID = c.nProjectID,
                                                               sProjectName = c.sProjectName,
                                                               nTaskTypeID = b.nTaskTypeID,
                                                               sTaskType = d.sNameEng,
                                                               nTypeRequest = b.nTypeRequest,
                                                               nPlan = b.nPlan,
                                                               nPlanProcess = b.nPlanProcess,
                                                               sDate = b.dTask.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH),
                                                               dTask = b.dTask,
                                                               sDetail = b.sDescription,
                                                           }).ToList();

                    var lstTaskPlan = lstDataTaskTable.Where(w => w.nPlanType == 111).ToList();
                    var lstTaskOther = lstDataTaskTable.Where(w => w.nPlanType == 112).ToList();
                    var lstTask = lstDataTaskTable;

                    result.dMinDate = lstTaskPlan.Min(s => s.dTask);
                    result.dMaxDate = lstTaskPlan.Max(s => s.dTask);
                    result.lstPlanSelect = lstTaskPlan.Select(s => s.sID ?? "").ToList();
                    result.lstTaskWFH = lstTask;
                    result.lstTaskPlan = lstTaskPlan;
                    result.lstTaskOther = lstTaskOther;
                    #endregion
                }

                result.nStatusCode = 200;
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
        public ResultAPI GetActionFlowWorkFromHome(string? Key)
        {
            var oUser = _authen.GetUserAccount();
            ResultAPI result = new();
            cActionWFH objResult = new();
            try
            {
                int? nID = !string.IsNullOrEmpty(Key) ? Key.DecryptParameter().toInt() : null;
                if (nID.HasValue)
                {
                    var dtWFH = _db.TB_WFH.FirstOrDefault(f => !f.IsDelete && f.nWFHID == nID.Value);
                    if (dtWFH != null)
                    {
                        int nCurrentProcess = dtWFH.nFlowProcessID;
                        TB_WFHFlow? oCurrentApproveFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == dtWFH.nWFHID && !w.dApprove.HasValue).OrderBy(o => o.nLevel).FirstOrDefault();

                        var IsRequester = dtWFH.nCreateBy == oUser.nUserID;
                        bool IsLineApprover = oCurrentApproveFlow?.nApproveBy == oUser.nUserID;
                        bool IsHRLineApprover = _db.TB_Employee_Position.Any(a => a.nEmployeeID == oUser.nUserID && a.nPositionID == (int)EnumWFH.LineApprover.HR);

                        objResult.IsBtnDraft = false;
                        objResult.IsBtnSubmit = false;
                        objResult.IsBtnApprove = false;
                        objResult.IsBtnReject = false;
                        objResult.IsBtnRecall = false;
                        objResult.IsBtnCancel = false;

                        switch (dtWFH.nFlowProcessID)
                        {
                            case (int)WFHStatus.Draft:
                                if (IsRequester)
                                {
                                    objResult.IsBtnDraft = true;
                                    objResult.IsBtnSubmit = true;
                                    // objResult.IsBtnCancel = true;
                                }
                                break;
                            case (int)WFHStatus.Submit:
                                if (IsHRLineApprover)
                                {
                                    objResult.IsBtnApprove = true;
                                    objResult.IsBtnReject = true;
                                }
                                else if (IsRequester)
                                {
                                    objResult.IsBtnRecall = true;
                                    objResult.IsBtnCancel = true;
                                }
                                break;
                            case (int)WFHStatus.Approve:
                                if (IsLineApprover)
                                {
                                    objResult.IsBtnApprove = true;
                                    objResult.IsBtnReject = true;
                                    objResult.IsBtnCancel = true;
                                }
                                else if (IsHRLineApprover && oCurrentApproveFlow?.nLevel == 2)
                                {

                                    objResult.IsBtnApprove = true;
                                    objResult.IsBtnReject = true;
                                    objResult.IsBtnCancel = true;
                                }
                                else if (IsRequester || (IsHRLineApprover && oCurrentApproveFlow?.nLevel == 3))
                                {
                                    objResult.IsBtnRecall = true;
                                    objResult.IsBtnCancel = true;
                                }
                                else if (IsLineApprover && oCurrentApproveFlow?.nLevel == 3)
                                {
                                    objResult.IsBtnCancel = true;
                                }
                                break;
                            case (int)WFHStatus.Reject:
                                if (IsRequester)
                                {
                                    objResult.IsBtnDraft = true;
                                    objResult.IsBtnSubmit = true;
                                    objResult.IsBtnCancel = true;
                                }
                                break;
                            case (int)WFHStatus.Recall:
                                if (IsHRLineApprover)
                                {
                                    objResult.IsBtnApprove = true;
                                    objResult.IsBtnReject = true;
                                    objResult.IsBtnCancel = true;
                                }
                                else if (IsRequester)
                                {
                                    objResult.IsBtnDraft = true;
                                    objResult.IsBtnSubmit = true;
                                }
                                break;
                            case (int)WFHStatus.Completed:
                                if (IsRequester && dtWFH.dWFH.Date > DateTime.Now.Date || IsHRLineApprover)
                                {
                                    objResult.IsBtnCancel = true;
                                }
                                break;

                            case (int)WFHStatus.Cancel:
                                break;
                            default:
                                objResult.IsBtnDraft = true;
                                objResult.IsBtnSubmit = true;
                                break;

                        }
                        result.objResult = objResult;
                        result.nStatusCode = StatusCodes.Status200OK;
                    }
                    else
                    {
                        objResult.IsBtnDraft = true;
                        objResult.IsBtnSubmit = true;
                        result.objResult = objResult;
                        result.nStatusCode = StatusCodes.Status200OK;
                    }
                }
                else
                {
                    objResult.IsBtnDraft = true;
                    objResult.IsBtnSubmit = true;
                    result.objResult = objResult;
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
        public ResultAPI UpdateFlowWorkFromHome(cUpdateFlow param)
        {
            ResultAPI result = new ResultAPI();
            var UserAccount = _authen.GetUserAccount();
            int nUserID = UserAccount.nUserID;

            try
            {
                int? nID = !string.IsNullOrEmpty(param.sWFHID) ? param.sWFHID.DecryptParameter().toInt() : null;
                if (nID.HasValue)
                {
                    TB_WFH? dtWFH = _db.TB_WFH.FirstOrDefault(w => w.nWFHID == nID.Value && w.IsDelete != true);
                    if (dtWFH != null)
                    {
                        int nPositionID = _db.TB_Employee_Position.Where(w => w.nEmployeeID == dtWFH.nCreateBy).Select(s => s.nPositionID).FirstOrDefault();

                        cWFHFlow oWFH = new cWFHFlow
                        {
                            nWFHID = dtWFH.nWFHID,
                            nMode = param.nMode,
                            sComment = param.sComment
                        };
                        cResWFHFlow Result = WFHWorkflow(oWFH);
                        if (Result.IsSuccess)
                        {
                            result.nStatusCode = StatusCodes.Status200OK;
                        }
                        else
                        {
                            result.nStatusCode = StatusCodes.Status500InternalServerError;
                            result.sMessage = Result.sMessage;
                        }
                    }
                    else
                    {
                        result.nStatusCode = StatusCodes.Status500InternalServerError;
                        result.sMessage = "Data";
                    }
                }
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }
        /// <summary>
        /// </summary>
        public cResWFHFlow WFHWorkflow(cWFHFlow oWFH)
        {
            // TokenJWTSecret oUser = _authen.GetUserAccount();
            var oUser = _authen.GetUserAccount();
            cResWFHFlow result = new cResWFHFlow();

            try
            {
                //check nWFHID has in flow
                result.IsSuccess = true;
                bool IsHasFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == oWFH.nWFHID).Any();
                TB_WFH? oWFHData = _db.TB_WFH.FirstOrDefault(w => w.nWFHID == oWFH.nWFHID);

                if (oWFHData != null)
                {
                    int nProcessWorkflow = (int)EnumWFH.WFHStatus.Draft;

                    if (oWFH.nMode == (int)EnumWFH.WFHRequestMode.Draft)
                    {
                        if (oWFHData.nFlowProcessID == (int)EnumWFH.WFHStatus.Draft || oWFHData.nFlowProcessID == (int)EnumWFH.WFHStatus.Recall || oWFHData.nFlowProcessID == (int)EnumWFH.WFHStatus.Reject)
                        {
                            nProcessWorkflow = (int)EnumWFH.WFHStatus.Draft;
                        }
                    }
                    else
                    {
                        //check permission in WFH Ticket
                        bool IsRequester = oWFHData.nCreateBy == oUser.nUserID;
                        #region Send Line Noti

                        var lstEmp = _db.TB_Employee.Where(w => w.IsActive && !w.IsDelete).ToArray();

                        var strCreate = lstEmp.FirstOrDefault(f => f.nEmployeeID == oWFHData.nCreateBy);


                        var qryListPlan = (from a in _db.TB_WFHTask.Where(w => !w.IsDelete && w.nWFHID == oWFHData.nWFHID)
                                           from b in _db.TB_Task.Where(w => w.nTaskID == a.nPlanID)
                                           from c in _db.TB_Project.Where(w => w.nProjectID == b.nProjectID)
                                           select new
                                           {
                                               Label = "- " + c.sProjectAbbr + "" + c.sProjectName + " (" + b.nPlan + " ชม.)"
                                           }).ToList();

                        var arrDetail = qryListPlan.Select(s => s.Label).ToArray();

                        var sDetailRequest = string.Join("\\n", arrDetail);
                        string sGUID = Guid.NewGuid().ToString();
                        string sWFHID = oWFHData.nWFHID.EncryptParameter();
                        string sApproveBy = oWFHData.nApproveBy.EncryptParameter();

                        cParamSendLine objParam = new()
                        {
                            sGUID = sGUID,
                            sTime = DateTime.Now.ToStringFromDate("HH:mm", "th-TH"),
                            sDate = DateTime.Now.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                            sNameAccount = strCreate != null ? strCreate.sNameTH + " " + strCreate.sSurnameTH + " (" + strCreate.sNickname + ")" : "",
                            sStartDate = oWFHData.dWFH.ToStringFromDate("dd/MM/yyyy", "th-TH"),
                            sDetailRequest = "\\n" + sDetailRequest,
                            // sStatus = dtStatus != null ? dtStatus.sProcess : "",
                            sRemark = "",      //Cancel

                            sPathApprove = "ApproveWFHLine&sWFHID=" + sWFHID + "&sUserID=" + sApproveBy + "&sMode=2&sGUID=" + sGUID,
                            sPathReject = "RejectWFHLine&sWFHID=" + sWFHID + "&sUserID=" + sApproveBy + "&sMode=3&sGUID=" + sGUID,
                            sPathCancel = "RejectWFHLine&sWFHID=" + sWFHID + "&sUserID=" + sApproveBy + "&sMode=5&sGUID=" + sGUID,
                            sPathDetail = "DetailWFHLine&sWFHID=" + sWFHID + "&sUserID=" + sApproveBy
                        };
                        // send line Last function
                        #endregion Send Line Noti 

                        if (!IsHasFlow)
                        {
                            if (oWFH.nMode == (int)EnumWFH.WFHRequestMode.Save && IsRequester)
                            {
                                //create
                                cLineApprover requester = new()
                                {
                                    nWFHID = oWFH.nWFHID,
                                    nApproveID = oWFH.nRequesterID,
                                    nPositionID = oWFH.nRequesterPositionID ?? 0,
                                    IsLineApprover = false
                                };
                                List<cLineApprover>? lstLineApprover = new()
                                {
                                    requester
                                };

                                if (oWFH.lstApprover != null && oWFH.lstApprover.Any())
                                {
                                    lstLineApprover.AddRange(oWFH.lstApprover);
                                }


                                IsHasFlow = CreateFlow(oWFH.nWFHID, lstLineApprover);
                            }

                        }

                        if (IsHasFlow)
                        {
                            TB_WFHFlow? oCurrentApproveFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == oWFH.nWFHID && !w.dApprove.HasValue).OrderBy(o => o.nLevel).FirstOrDefault();

                            //check permission in WFH Ticket
                            bool IsHRLineApprover = _db.TB_Employee_Position.Any(a => a.nEmployeeID == oUser.nUserID && a.nPositionID == (int)EnumWFH.LineApprover.HR) && (oCurrentApproveFlow?.nLevel >= 2);
                            bool IsLineApprover = oCurrentApproveFlow?.nApproveBy == oUser.nUserID || IsHRLineApprover;

                            if (IsRequester || IsLineApprover)
                            {
                                int nCurrentProcess = _db.TB_WFH.FirstOrDefault(w => w.nWFHID == oWFH.nWFHID)?.nFlowProcessID ?? 1;
                                switch (oWFH.nMode)
                                {
                                    case (int)EnumWFH.WFHRequestMode.Cancel: // not update
                                        nProcessWorkflow = (int)EnumWFH.WFHStatus.Cancel;
                                        if (!IsRequester)
                                        {
                                            objParam.nTemplateID = 39;
                                            objParam.sRemark = oWFH.sComment ?? "";
                                            objParam.lstEmpTo = new List<int> { oWFHData.nCreateBy };
                                        }
                                        break;

                                    case (int)EnumWFH.WFHRequestMode.Draft: // not update
                                        if (nCurrentProcess == (int)EnumWFH.WFHStatus.Draft || nCurrentProcess == (int)EnumWFH.WFHStatus.Recall || nCurrentProcess == (int)EnumWFH.WFHStatus.Reject)
                                        {
                                            nProcessWorkflow = (int)EnumWFH.WFHStatus.Draft;
                                        }
                                        break;
                                    case (int)EnumWFH.WFHRequestMode.Save:
                                        //db check last status WFH
                                        bool IsLastApprover = CheckIsLastApprover(oWFH.nWFHID);

                                        if (oCurrentApproveFlow != null)
                                        {
                                            if (IsLastApprover)
                                            {
                                                nProcessWorkflow = (int)EnumWFH.WFHStatus.Completed;
                                                objParam.nTemplateID = 38;
                                                objParam.lstEmpTo = new List<int> { oWFHData.nCreateBy };

                                            }
                                            else if (nCurrentProcess == (int)EnumWFH.WFHStatus.Submit)
                                            {
                                                // next is Approve HR
                                                nProcessWorkflow = (int)EnumWFH.WFHStatus.Approve;
                                                objParam.nTemplateID = 37;
                                                var lstUserHR = _db.TB_Employee_Position.Where(w => !w.IsDelete && w.nPositionID == 9).Select(s => s.nEmployeeID).ToList();
                                                objParam.lstEmpTo = lstUserHR;


                                            }
                                            else if (nCurrentProcess == (int)EnumWFH.WFHStatus.Draft)
                                            {
                                                // next is Submit
                                                nProcessWorkflow = (int)EnumWFH.WFHStatus.Submit;
                                                objParam.nTemplateID = 37;
                                                var lstUserHR = _db.TB_Employee_Position.Where(w => !w.IsDelete && w.nPositionID == 9).Select(s => s.nEmployeeID).ToList();
                                                objParam.lstEmpTo = lstUserHR;
                                            }
                                            else
                                            {
                                                nProcessWorkflow = (int)EnumWFH.WFHStatus.Approve;
                                                objParam.nTemplateID = 37;
                                                objParam.lstEmpTo = new List<int> { oWFHData.nApproveBy };
                                            }

                                            oCurrentApproveFlow.nFlowProcessID = nProcessWorkflow;
                                            oCurrentApproveFlow.dApprove = DateTime.Now;
                                            oCurrentApproveFlow.nApproveBy = oUser.nUserID;
                                            oCurrentApproveFlow.dUpdate = DateTime.Now;
                                            oCurrentApproveFlow.nUpdateBy = oUser.nUserID;

                                            _db.SaveChanges();
                                        }
                                        break;
                                    case (int)EnumWFH.WFHRequestMode.Reject:

                                        // 1. For LineApprove only
                                        // 2. Update LineApprove [delete]

                                        if (IsHRLineApprover || IsLineApprover)
                                        {
                                            List<TB_WFHFlow>? lstResetFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == oWFH.nWFHID && !w.IsDelete).ToList();

                                            if (lstResetFlow.Any())
                                            {
                                                foreach (var item in lstResetFlow)
                                                {
                                                    if (item.nPositionID == (int)EnumWFH.LineApprover.HR)
                                                    {
                                                        item.nApproveBy = null;

                                                    }
                                                    item.nFlowProcessID = 0;
                                                    item.dApprove = null;
                                                }
                                                _db.SaveChanges();

                                            }
                                            nProcessWorkflow = (int)EnumWFH.WFHStatus.Reject;
                                            objParam.nTemplateID = 40;
                                            objParam.sRemark = oWFH.sComment ?? "";
                                            objParam.lstEmpTo = new List<int> { oWFHData.nCreateBy };
                                        }
                                        else
                                        {
                                            result.IsSuccess = false;
                                            result.sMessage = "Unautorize.";
                                        }
                                        break;
                                    case (int)EnumWFH.WFHRequestMode.Recall:
                                        // 1. For Requester only
                                        // 2. check authen is requester
                                        // 3. Workflow not have anyone in LineApprover action
                                        // 4. Update LineApprove [delete]
                                        //bool IsHasFlowAction = _db.TB_WFHFlow.Where(w => w.nWFHID == oWFH.nWFHID && w.dApprove.HasValue && w.IsLineApprover).Any();

                                        if (IsRequester || IsHRLineApprover)
                                        {
                                            List<TB_WFHFlow>? lstResetFlow = new();
                                            if (IsRequester)
                                            {
                                                lstResetFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == oWFH.nWFHID && !w.IsDelete).ToList();
                                            }
                                            else if (IsHRLineApprover)
                                            {
                                                lstResetFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == oWFH.nWFHID && w.IsLineApprover && !w.IsDelete).ToList();

                                            }
                                            if (lstResetFlow.Any())
                                            {
                                                foreach (var item in lstResetFlow)
                                                {
                                                    if (item.nPositionID == (int)EnumWFH.LineApprover.HR)
                                                    {
                                                        item.nApproveBy = null;

                                                    }
                                                    item.nFlowProcessID = 0;
                                                    item.dApprove = null;
                                                }
                                                _db.SaveChanges();
                                            }

                                            nProcessWorkflow = (int)EnumWFH.WFHStatus.Recall;
                                        }
                                        else
                                        {
                                            result.IsSuccess = false;
                                            result.sMessage = "Unautorize.";
                                        }
                                        break;
                                }

                                #region update TB_WFH

                                if (result.IsSuccess)
                                {
                                    oWFHData.nFlowProcessID = nProcessWorkflow;
                                    _db.SaveChanges();
                                }

                                #endregion
                                // Send Line noti

                                var dtStatus = _db.TM_WFHFlowProcess.FirstOrDefault(f => f.nFlowProcessID == oWFHData.nFlowProcessID);
                                objParam.sStatus = dtStatus != null ? dtStatus.sProcess : "";



                                STFunction.SendToLine(objParam);

                            }
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.sMessage = "not found workflow.";
                        }
                    }

                    #region update workflow history

                    cWFHRequestHistory oHistory = new cWFHRequestHistory()
                    {
                        nWFHID = oWFH.nWFHID,
                        nFlowProcessID = nProcessWorkflow,
                        nApproveBy = oUser.nUserID,
                        dApprove = DateTime.Now,
                        sComment = oWFH.sComment
                    };

                    WFHHistory(oHistory);

                    #endregion

                }
                else
                {
                    result.IsSuccess = false;
                    result.sMessage = "Request is null";

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.sMessage = ex.Message;
            }

            return result;
        }
        /// <summary>
        /// </summary>
        public bool CreateFlow(int nWFHID, List<cLineApprover> lstApprover)
        {
            // TokenJWTSecret oUser = _authen.GetUserAccount();
            var oUser = _authen.GetUserAccount();

            bool result = false;

            try
            {
                //check nWFHID has in flow
                bool IsHasFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == nWFHID).Any();
                if (!IsHasFlow)
                {
                    int nLevel = 1;

                    lstApprover.ForEach(f =>
                    {
                        int nWFHFlow = _db.TB_WFHFlow.Any() ? _db.TB_WFHFlow.Max(m => m.nFlowID) + 1 : 1;

                        TB_WFHFlow oFlow = new TB_WFHFlow
                        {
                            nFlowID = nWFHFlow,
                            nWFHID = nWFHID,
                            nPositionID = f.nPositionID,
                            nApproveBy = f.nApproveID,
                            nLevel = nLevel++,
                            IsLineApprover = f.IsLineApprover,
                            dCreate = DateTime.Now,
                            nCreateBy = oUser.nUserID,
                            dUpdate = DateTime.Now,
                            nUpdateBy = oUser.nUserID,
                            IsDelete = false
                        };

                        _db.TB_WFHFlow.Add(oFlow);
                        _db.SaveChanges();
                    });
                }

                result = true;

            }
            catch (Exception ex)
            {
                string sMsg = ex.Message;
                result = false;
            }

            return result;
        }
        /// <summary>
        /// </summary>
        public bool CheckIsLastApprover(int nWFHRequestID)
        {
            bool result = false;
            // TokenJWTSecret oUser = _authen.GetUserAccount();
            var oUser = _authen.GetUserAccount();
            try
            {
                IOrderedQueryable<TB_WFHFlow>? lstWFHFlow = _db.TB_WFHFlow.Where(w => w.dApprove == null && w.nWFHID == nWFHRequestID).OrderBy(o => o.nLevel);
                if (lstWFHFlow.Any() && lstWFHFlow.Count() == 1)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }
        /// <summary>
        /// </summary>
        public string SendMailWFH(int nWFHID, int nFlowProcessID)
        {
            List<string>? lstTo = new List<string>();

            string result = "";

            try
            {
                TB_WFH? oWFH = _db.TB_WFH.FirstOrDefault(w => w.nWFHID == nWFHID);
                if (oWFH != null)
                {
                    TB_Employee? oRequester = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == oWFH.nCreateBy);
                    TB_WFHFlow? oNextFlow = _db.TB_WFHFlow.Where(w => w.nWFHID == nWFHID && !w.IsDelete && w.dApprove == null).OrderBy(o => o.nLevel).FirstOrDefault();
                    TB_Employee? oNextApprover = new TB_Employee();
                    int? nPositionID = null;
                    string? sStatus = _db.TM_WFHFlowProcess.FirstOrDefault(w => w.nFlowProcessID == nFlowProcessID)?.sProcess;

                    if (oNextFlow != null)
                    {
                        if (oNextFlow.nApproveBy.HasValue)
                        {
                            TB_Employee? oNext = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == oNextFlow.nApproveBy.Value);
                            if (oNext != null)
                            {
                                oNextApprover = oNext;
                            }
                        }
                        else if (oNextFlow.nPositionID > 0)
                        {
                            nPositionID = oNextFlow.nPositionID;
                        }
                    }

                    string sSubject = "";
                    string sContent = "เรียนคุณ ";
                    string sCard = "";
                    string sDateWFH = oWFH.dWFH.ToStringFromDate("dd/MM/yyyy");
                    string sTaskWFH = "ยังไม่ได้ทำค่ะ";
                    string sResult = "";
                    string sRequester = oRequester?.sNameTH + " " + oRequester?.sSurnameTH + " (" + oRequester?.sNickname + ")";
                    string sStyleChip = "background-color: #eef6ff;color: #3e97ff;";


                    sCard += @"<table style='border: 1px solid black; border-radius: 10px; padding:10px; width:70%; margin: auto'>
                               <tr>
                                    <td style='width:10px'><i class='fa fa-vcard' style='color:#78A3D4'></i></td>
                                    <td><b>{0}</b></td>
                                    <td style='text-align: right'><span style='padding: 2px 10px;border-radius: 2.7em;font-size:10pt;{1}'>{2}</span></td>
                                </tr>
                                <tr>
                                    <td style='width:10px'><i class='fa fa-calendar' style='color:#78A3D4'></i></td>
                                    <td><b>วันที่: {3}</b></td>
                                    <td colspan=2></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan=2>{4}</td>
                                </tr>
                            </table>";

                    switch (nFlowProcessID)
                    {
                        case (int)EnumWFH.WFHStatus.Submit:
                            //requester
                            if (oRequester != null)
                            {
                                lstTo = new List<string>();
                                lstTo.Add(oRequester.sEmail + "");
                                sSubject = "เอกสาร Work From Home ของคุณกำลังถูกดำเนินการ";

                                sResult = @"
                                <div style='text-align: center; margin-bottom:10px'>
                                    <span>สวัสดี <b>" + sRequester + @"</b></span>
                                    </br>
                                    <span> เอกสารของคุณ <b> กำลังถูกดำเนินการ </b></span>
                                </div></br>" + sCard;

                                sContent = string.Format(sResult, sRequester, sStyleChip, sStatus, sDateWFH, sTaskWFH);

                                EmailParameter oSendMail = new EmailParameter()
                                {
                                    lstTo = lstTo,
                                    sSubject = sSubject,
                                    sMessage = sContent
                                };

                                STEmail.Send(oSendMail);

                                result += @"</br> ================ Requester ================ </br>" + "To:" + string.Join(",", lstTo) + "</br>Subject: " + sSubject + "</br> Content" + sContent;
                            }

                            //next approver
                            sSubject = "มีเอกสารรอการพิจารณาจากคุณ";

                            if (nPositionID.HasValue)
                            {
                                //send to all in position level
                                IQueryable<int>? lstEmpPos = _db.TB_Employee_Position.Where(w => w.nPositionID == nPositionID.Value).Select(s => s.nEmployeeID);
                                if (lstEmpPos.Any())
                                {
                                    lstTo = _db.TB_Employee.Where(w => !string.IsNullOrEmpty(w.sEmail) && w.IsActive && !w.IsDelete && lstEmpPos.Contains(w.nEmployeeID)).Select(s => s.sEmail + "").ToList();
                                }

                                sResult = @"
                                <div style='text-align: center; margin-bottom:10px'>
                                    <span>สวัสดีทุกท่าน</span>
                                    </br>
                                    <span>มีเอกสาร Work From Home ใหม่รอให้คุณดำเนินการ</span>
                                </div></br>" + sCard;

                                sContent = string.Format(sResult, sRequester, sStyleChip, sStatus, sDateWFH, sTaskWFH);

                                EmailParameter oSendMail = new EmailParameter()
                                {
                                    lstTo = lstTo,
                                    sSubject = sSubject,
                                    sMessage = sContent
                                };

                                STEmail.Send(oSendMail);

                                result += @"</br> ================ Next Approver ================ </br>" + "To:" + string.Join(",", lstTo) + "</br>Subject: " + sSubject + "</br> Content" + sContent;

                            }
                            else if (oNextApprover != null)
                            {
                                lstTo = new List<string>() { oNextApprover.sEmail + "" };
                                string sNextName = oNextApprover.sNameTH + " " + oNextApprover.sSurnameTH + " (" + oNextApprover.sNickname + ")";

                                sResult = @"
                                <div style='text-align: center; margin-bottom:10px'>
                                    <span>สวัสดี " + sNextName + @"</span>
                                    </br>
                                    <span>มีเอกสาร Work From Home ใหม่รอให้คุณดำเนินการ</span>
                                </div></br>" + sCard;

                                sContent = string.Format(sResult, sRequester, sStyleChip, sStatus, sDateWFH, sTaskWFH);

                                EmailParameter oSendMail = new EmailParameter()
                                {
                                    lstTo = lstTo,
                                    sSubject = sSubject,
                                    sMessage = sContent
                                };

                                STEmail.Send(oSendMail);
                                result += @"</br> ================ Next Approver ================ </br>" + "To:" + string.Join(",", lstTo) + "</br>Subject: " + sSubject + "</br> Content" + sContent;
                            }

                            break;
                        case (int)EnumWFH.WFHStatus.Approve:
                            //next approver
                            sSubject = "มีเอกสารรอการพิจารณาจากคุณ";

                            if (nPositionID.HasValue)
                            {
                                //send to all in position level
                                IQueryable<int>? lstEmpPos = _db.TB_Employee_Position.Where(w => w.nPositionID == nPositionID.Value).Select(s => s.nEmployeeID);
                                if (lstEmpPos.Any())
                                {
                                    lstTo = _db.TB_Employee.Where(w => !string.IsNullOrEmpty(w.sEmail) && w.IsActive && !w.IsDelete && lstEmpPos.Contains(w.nEmployeeID)).Select(s => s.sEmail + "").ToList();
                                }

                                sResult = @"
                                <div style='text-align: center; margin-bottom:10px'>
                                    <span>สวัสดีทุกท่าน</span>
                                    </br>
                                    <span>มีเอกสาร Work From Home ใหม่รอให้คุณดำเนินการ</span>
                                </div></br>" + sCard;

                                sContent = string.Format(sResult, sRequester, sStyleChip, sStatus, sDateWFH, sTaskWFH);

                                EmailParameter oSendMail = new EmailParameter()
                                {
                                    lstTo = lstTo,
                                    sSubject = sSubject,
                                    sMessage = sContent
                                };

                                STEmail.Send(oSendMail);

                                result += @"</br> ================ Next Approver ================ </br>" + "To:" + string.Join(",", lstTo) + "</br>Subject: " + sSubject + "</br> Content" + sContent;

                            }
                            else if (oNextApprover != null)
                            {
                                lstTo = new List<string>() { oNextApprover.sEmail + "" };
                                string sNextName = oNextApprover.sNameTH + " " + oNextApprover.sSurnameTH + " (" + oNextApprover.sNickname + ")";

                                sResult = @"
                                <div style='text-align: center; margin-bottom:10px'>
                                    <span>สวัสดี " + sNextName + @"</span>
                                    </br>
                                    <span>มีเอกสาร Work From Home ใหม่รอให้คุณดำเนินการ</span>
                                </div></br>" + sCard;

                                sContent = string.Format(sResult, sRequester, sStyleChip, sStatus, sDateWFH, sTaskWFH);

                                EmailParameter oSendMail = new EmailParameter()
                                {
                                    lstTo = lstTo,
                                    sSubject = sSubject,
                                    sMessage = sContent
                                };

                                STEmail.Send(oSendMail);
                                result += @"</br> ================ Next Approver ================ </br>" + "To:" + string.Join(",", lstTo) + "</br>Subject: " + sSubject + "</br> Content" + sContent;
                            }
                            break;
                        case (int)EnumWFH.WFHStatus.Completed:
                        case (int)EnumWFH.WFHStatus.Reject:
                        case (int)EnumWFH.WFHStatus.Cancel:

                            if (nFlowProcessID == (int)EnumWFH.WFHStatus.Completed)
                            {
                                sStyleChip = "background-color: #e8fff3;color: #50cd89;";
                            }
                            else if (nFlowProcessID == (int)EnumWFH.WFHStatus.Reject)
                            {
                                sStyleChip = "background-color: #f9f9f9;color: #7e8299;";
                            }
                            else
                            {
                                //cancel
                                sStyleChip = "background-color: #fff5f8;color: #f1416c;";
                            }

                            //requester
                            if (oRequester != null)
                            {
                                lstTo = new List<string>();

                                lstTo.Add(oRequester.sEmail + "");
                                sSubject = "เอกสาร Work From Home ของคุณถูกดำเนินการแล้ว";

                                sResult = @"
                                <div style='text-align: center; margin-bottom:10px'>
                                    <span>สวัสดี <b>" + sRequester + @"</b></span>
                                    </br>
                                    <span> เอกสารของคุณ <b> ถูกดำเนินการแล้ว </b></span>
                                </div></br>" + sCard;

                                sContent = string.Format(sResult, sRequester, sStyleChip, sStatus, sDateWFH, sTaskWFH);

                                EmailParameter oSendMail = new EmailParameter()
                                {
                                    lstTo = lstTo,
                                    sSubject = sSubject,
                                    sMessage = sContent
                                };

                                STEmail.Send(oSendMail);

                                result += @"</br> ================ Requester ================ </br>" + "To:" + string.Join(",", lstTo) + "</br>Subject: " + sSubject + "</br> Content" + sContent;
                            }
                            break;
                        case (int)EnumWFH.WFHStatus.Recall:
                            //requester
                            //next approver
                            break;
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }


        public static int GetMonthsBetween(DateTime from, DateTime to)
        {
            if (from > to) return GetMonthsBetween(to, from);

            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                return monthDiff - 1;
            }
            else
            {
                return monthDiff;
            }
        }
    }
}

