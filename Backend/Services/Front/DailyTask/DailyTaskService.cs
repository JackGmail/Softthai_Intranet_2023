using Backend.EF.ST_Intranet;
using Backend.Enum;
using Backend.Interfaces;
using Backend.Interfaces.Authentication;
using Backend.Models;
using Backend.Models.Authentication;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.CustomUI;
using Extensions.Common.STExtension;
using ST.INFRA;
using ST.INFRA.Common;
using System.Data;
using static Backend.Enum.EnumGlobal;
using ResultAPI = Backend.Models.ResultAPI;

namespace Backend.Service
{
    public class DailyTaskService : IDailyTaskService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _auth;
        public DailyTaskService(ST_IntranetEntity db, IAuthentication auth)
        {
            _db = db;
            _auth = auth;
        }

        public ResultAPI PageLoad(bool IsFilterUserData)
        {
            ResultAPI result = new ResultAPI();
            ResultOptDailyTask objResult = new ResultOptDailyTask();
            var ua = _auth.GetUserAccount();
            int nUserID = ua.nUserID;

            var TMData = _db.TM_Data.Where(w => w.IsActive && !w.IsDelete);

            #region job type
            var lstPositionMapping = _db.TM_Task_Activity_PositionMapping.Where(w => ua.lstUserPositionID.Contains(w.nPositionID))
                .GroupBy(g => g.nMappingActivityID)
                .Select(s => new { nMappingActivityID = s.Key });

            var optJob = (from i in lstPositionMapping
                          join j in _db.TM_Task_Activity_Mapping on i.nMappingActivityID equals j.nMappingActivityID
                          join k in _db.TM_Task_Activity on j.nActivityID equals k.nActivityID
                          join l in _db.TM_Task_Activity_Type on j.nActivityTypeID equals l.nActivityTypeID
                          select new
                          {
                              label = l.sActivityTypeAbbr + " - " + k.sActivity,
                              value = i.nMappingActivityID + "",
                              nOrderActivity = k.nOrder,
                              nOrderActivityType = l.nOrder
                          }).OrderBy(w => w.nOrderActivity).ThenBy(w => w.nOrderActivityType)
                          .Select(s => new Option
                          {
                              label = s.label,
                              value = s.value
                          }).ToList();
            #endregion

            #region task status
            var optTaskStatus = TMData.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.TaskStatus).Select(s => new Option
            {
                label = s.sNameTH,
                value = s.nData_ID.ToString()
            }).ToList();
            #endregion

            #region task status
            var optTaskProgress = _db.TM_Task_Progress.Where(w => w.IsActive && !w.IsDelete).OrderBy(w => w.nOrder).Select(s => new TaskProgressOption
            {
                label = s.sProgressName,
                value = s.nProgressID.ToString(),
                IsRequiredDesc = s.IsRequiredDesc
            }).ToList();
            #endregion

            #region project
            var TB_Project = _db.TB_Project.Where(w => !w.IsDelete);
            if (IsFilterUserData)
            {
                var arrProjectID = _db.TB_Project_Person.Where(w => w.nEmployeeID == nUserID && !w.IsDelete).Select(s => s.nProjectID).ToArray();
                TB_Project = TB_Project.Where(w => arrProjectID.Contains(w.nProjectID)).AsQueryable();
            }

            var optPrj = TB_Project.OrderBy(w => w.sProjectName).Select(s => new Option
            {
                label = (!string.IsNullOrEmpty(s.sProjectAbbr) && (s.sProjectAbbr + "").Trim() != "-" ? s.sProjectAbbr + " : " : "") + s.sProjectName,
                value = s.nProjectID.ToString()
            }).ToList();
            #endregion

            #region Team
            var optTeam = _db.TB_Team.Where(w => !w.IsDelete && w.IsActive).Select(s => new Option
            {
                label = s.sTeamName,
                value = s.nTeamID.ToString()
            }).ToList();

            var lstEmployeeTeamRep = _db.TB_Employee_Report_To.Where(w => !w.IsDelete && w.nRepEmployeeID == nUserID).Select(s => s.nEmployeeID).ToList();
            var lstTeamDefault = _db.TB_Team.Where(w => !w.IsDelete && w.IsActive && w.nTeamLeaderID == nUserID || lstEmployeeTeamRep.Contains(w.nTeamLeaderID)).Select(s => s.nTeamID).ToList();
            #endregion

            #region Employee
            var lstEmployee = _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive);
            var lstEmployeeDefault = _db.TB_Employee_Report_To.Where(w => (!w.IsDelete && lstTeamDefault.Contains(w.nTeamID)) || w.nEmployeeID == nUserID).Select(s => s.nEmployeeID).ToList();
            if (lstEmployeeDefault.Any())
            {
                lstEmployee = lstEmployee.Where(w => lstEmployeeDefault.Contains(w.nEmployeeID));
            }
            var optEmployee = lstEmployee.Where(w => !w.IsDelete && w.IsActive).Select(s => new Option
            {
                label = s.sNameTH + " " + s.sSurnameTH + (!string.IsNullOrEmpty(s.sNickname) ? "(" + s.sNickname + ")" : ""),
                value = s.nEmployeeID.ToString()
            }).OrderBy(w => w.label).ToList();
            #endregion

            objResult.lstJobType = optJob;
            objResult.lstTaskStatus = optTaskStatus;
            objResult.lstTaskProgress = optTaskProgress;
            objResult.lstProject = optPrj.ToList();
            objResult.lstTeam = optTeam.ToList();
            objResult.lstTeamDefault = lstTeamDefault.ConvertAll(x => x.ToString());
            objResult.lstEmployee = optEmployee.ToList();
            objResult.lstEmployeeDefault = lstEmployeeDefault.ConvertAll(x => x.ToString());
            result.objResult = objResult;

            return result;
        }

        public ResultAPI GetTask(ParamTask param)
        {
            ResultAPI result = new ResultAPI();
            ResultTask objResult = new ResultTask();
            int nUserID = _auth.GetUserAccount().nUserID;

            List<TaskItem> lstTaskTemp = new List<TaskItem>();
            lstTaskTemp = param.lstTaskTemp;


            IQueryable<TB_Task> lstTask = _db.TB_Task.Where(w => !w.IsDelete && w.nEmployeeID == nUserID).AsQueryable();
            if (!string.IsNullOrEmpty(param.sDateStart))
            {
                DateTime dCurrent = param.sDateStart.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                lstTask = lstTask.Where(w => w.dTask.Date >= dCurrent.Date).AsQueryable();

                lstTaskTemp = lstTaskTemp.Where(w => w.dTask.Date >= dCurrent.Date).ToList();
            }
            if (!string.IsNullOrEmpty(param.sDateEnd))
            {
                DateTime dCurrent = param.sDateEnd.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                lstTask = lstTask.Where(w => w.dTask.Date <= dCurrent.Date).AsQueryable();

                lstTaskTemp = lstTaskTemp.Where(w => w.dTask.Date <= dCurrent.Date).ToList();
            }
            if (param.lstProject.Any())
            {
                lstTask = lstTask.Where(w => param.lstProject.Contains(w.nProjectID)).AsQueryable();

                lstTaskTemp = lstTaskTemp.Where(w => w.nProjectID.HasValue && param.lstProject.Contains(w.nProjectID.Value)).ToList();
            }
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                string sSearch = param.sSearch.Trim();
                lstTask = lstTask.Where(w => w.sDescription.Contains(sSearch)).AsQueryable();

                lstTaskTemp = lstTaskTemp.Where(w => w.sDescription.Contains(sSearch)).ToList();
            }

            #region Task Form Database
            var lstJobType = (from i in _db.TM_Task_Activity_Mapping
                              join k in _db.TM_Task_Activity on i.nActivityID equals k.nActivityID
                              join l in _db.TM_Task_Activity_Type on i.nActivityTypeID equals l.nActivityTypeID
                              select new
                              {
                                  label = l.sActivityTypeAbbr + " - " + k.sActivity,
                                  i.nMappingActivityID
                              });
            var qry = (from task in lstTask
                       join prj in _db.TB_Project.Where(w => !w.IsDelete) on task.nProjectID equals prj.nProjectID
                       //join jobtype in _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.TaskType) on task.nTaskTypeID equals jobtype.nData_ID
                       join jobtype in lstJobType on task.nTaskTypeID equals jobtype.nMappingActivityID
                       join employee in _db.TB_Employee.Where(w => !w.IsDelete) on task.nEmployeeID equals employee.nEmployeeID
                       join status in _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.TaskStatus) on task.nTaskStatusID equals status.nData_ID
                       select new TaskItem
                       {
                           sEncryptID = task.nTaskID.EncryptParameter(),
                           dTask = task.dTask,
                           sTaskDate = task.dTask.ToString("dd/MM/yyyy"),
                           nProjectID = prj.nProjectID,
                           sProjectName = prj.sProjectName,
                           sDescription = task.sDescription,
                           nTaskTypeID = task.nTaskTypeID,
                           sJobType = jobtype.label ?? "",
                           nPlan = task.nPlan,
                           nActual = task.nActual,
                           nPlanProcess = task.nPlanProcess,
                           nActualProcess = task.nActualProcess,
                           nTaskStatusID = task.nTaskStatusID,
                           sStatus = status.sNameTH ?? "",
                           sDescriptionDelay = task.sDescriptionDelay ?? "",
                           IsDelete = false,
                           IsModified = false,
                           IsRequireDelay = false,
                           IsLock = task.dTask.AddDays(14).Date == DateTime.Now.Date
                           && (task.nTaskStatusID == (int)EnumTask.TaskStatus.Delay ||
                           task.nTaskStatusID == (int)EnumTask.TaskStatus.Completed ||
                           task.nTaskStatusID == (int)EnumTask.TaskStatus.CompletedDelay)
                       }
                ).ToList();
            #endregion
            objResult.arrFullData = qry;
            #region Task Form Client
            foreach (var item in lstTaskTemp)
            {
                if (string.IsNullOrEmpty(item.sEncryptID))
                {
                    qry.Add(item);
                }
                else
                {
                    var objDataUpdate = qry.FirstOrDefault(w => w.sEncryptID == item.sEncryptID);
                    if (objDataUpdate != null)
                    {
                        objDataUpdate.dTask = item.dTask;
                        objDataUpdate.sTaskDate = item.dTask.ToString("dd/MM/yyyy");
                        objDataUpdate.nProjectID = item.nProjectID;
                        objDataUpdate.sProjectName = item.sProjectName;
                        objDataUpdate.sDescription = item.sDescription;
                        objDataUpdate.nTaskTypeID = item.nTaskTypeID;
                        objDataUpdate.sJobType = item.sJobType;
                        objDataUpdate.nPlan = item.nPlan;
                        objDataUpdate.nActual = item.nActual;
                        objDataUpdate.nPlanProcess = item.nPlanProcess;
                        objDataUpdate.nActualProcess = item.nActualProcess;
                        objDataUpdate.nTaskStatusID = item.nTaskStatusID;
                        objDataUpdate.sStatus = item.sStatus;
                        objDataUpdate.sDescriptionDelay = item.sDescriptionDelay;
                    }
                }
            }
            qry = qry.OrderBy(w => w.dTask).ToList();
            #endregion

            #region//SORT
            string sSortColumn = param.sSortExpression;
            switch (param.sSortExpression)
            {
                case "sTaskDate": sSortColumn = "dTask"; break;
                case "sProjectName": sSortColumn = "sProjectName"; break;
                case "sDescription": sSortColumn = "sDescription"; break;
                case "sJobType": sSortColumn = "sJobType"; break;
                case "sStatus": sSortColumn = "sStatus"; break;
                case "sDescriptionDelay": sSortColumn = "sDescriptionDelay"; break;
            }
            if (param.isASC)
            {
                qry = qry.OrderBy<TaskItem>(sSortColumn).ToList();
            }
            else if (param.isDESC)
            {
                qry = qry.OrderByDescending<TaskItem>(sSortColumn).ToList();
            }
            #endregion

            #region//Final Action >> Skip , Take And Set Page
            var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qry.Count());
            objResult.arrData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
            int nKey = 1;
            objResult.arrData.ForEach(f =>
            {
                f.sID = nKey + "";
                nKey++;
            });
            objResult.nDataLength = dataPage.nDataLength;
            objResult.nPageIndex = dataPage.nPageIndex;
            objResult.nSkip = dataPage.nSkip;
            objResult.nTake = dataPage.nTake;
            objResult.nStartIndex = dataPage.nStartIndex;
            #endregion

            result.objResult = objResult;

            return result;
        }

        public ResultAPI AddTask(ParamAddTask param)
        {
            ResultAPI result = new ResultAPI();
            ResultTask objResult = new ResultTask();

            var arrData = param.lstTask.Where(w => !w.IsDelete).OrderBy(w => w.dTask).ToList();

            #region//Final Action >> Skip , Take And Set Page
            var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, arrData.Count());
            objResult.arrData = arrData.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
            objResult.nDataLength = dataPage.nDataLength;
            objResult.nPageIndex = dataPage.nPageIndex;
            objResult.nSkip = dataPage.nSkip;
            objResult.nTake = dataPage.nTake;
            objResult.nStartIndex = dataPage.nStartIndex;
            result.objResult = objResult;
            #endregion

            return result;
        }

        public ResultAPI RemoveTask(ParamRemoveTask param)
        {
            ResultAPI result = new ResultAPI();
            ResultTask objResult = new ResultTask();
            UserAccount ua = _auth.GetUserAccount();
            foreach (var item in param.lstID)
            {
                int nTaskID = item.DecryptParameter().ToInt();
                TB_Task? objTask = _db.TB_Task.FirstOrDefault(w => w.nTaskID == nTaskID);
                if (objTask != null)
                {
                    objTask.IsDelete = true;
                    objTask.dDelete = DateTime.Now;
                    objTask.nDeleteBy = ua.nUserID;
                }
                _db.SaveChanges();
            }
            return result;
        }

        public ResultAPI SaveTask(ParamSaveTask param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                var dNow = DateTime.Now;
                int nUserID = _auth.GetUserAccount().nUserID;
                List<TaskItem> lstModified = param.lstTask.Where(w => w.IsModified).ToList();
                foreach (var item in lstModified)
                {
                    int nTaskID = item.sEncryptID.DecryptParameter().ToInt();
                    TB_Task? objTask = _db.TB_Task.FirstOrDefault(w => w.nTaskID == nTaskID);
                    if (objTask == null)
                    {
                        objTask = new TB_Task();
                        objTask.nTaskID = (_db.TB_Task.Any() ? _db.TB_Task.Max(m => m.nTaskID) : 0) + 1;
                        objTask.dCreate = dNow;
                        objTask.nCreateBy = nUserID;
                        objTask.nTypeRequest = 1;
                        _db.TB_Task.Add(objTask);
                    }
                    objTask.nProjectID = item.nProjectID.HasValue ? item.nProjectID.Value : 0;
                    objTask.dTask = item.sTaskDate.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                    objTask.sDescription = item.sDescription;
                    objTask.nPlan = item.nPlan;
                    objTask.nPlanProcess = item.nPlanProcess;
                    objTask.nActual = item.nActual;
                    objTask.nActualProcess = item.nActualProcess;
                    objTask.nTaskTypeID = item.nTaskTypeID.HasValue ? item.nTaskTypeID.Value : 0;
                    objTask.nTaskStatusID = item.nTaskStatusID;
                    objTask.nEmployeeID = nUserID;
                    objTask.sDescriptionDelay = item.sDescriptionDelay;

                    objTask.dUpdate = dNow;
                    objTask.nUpdateBy = nUserID;
                    objTask.IsDelete = false;

                    _db.SaveChanges();
                }


                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }

        public ResultAPI GetTaskOverAll(ParamTaskOverAll param)
        {
            ResultAPI result = new ResultAPI();
            ResultTask objResult = new ResultTask();

            IQueryable<TB_Task> lstTask = _db.TB_Task.Where(w => !w.IsDelete).AsQueryable();
            if (!string.IsNullOrEmpty(param.sDateStart))
            {
                DateTime dCurrent = param.sDateStart.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                lstTask = lstTask.Where(w => w.dTask.Date >= dCurrent.Date).AsQueryable();
            }
            if (!string.IsNullOrEmpty(param.sDateEnd))
            {
                DateTime dCurrent = param.sDateEnd.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                lstTask = lstTask.Where(w => w.dTask.Date <= dCurrent.Date).AsQueryable();
            }
            if (param.lstProject.Any())
            {
                lstTask = lstTask.Where(w => param.lstProject.Contains(w.nProjectID)).AsQueryable();
            }
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                string sSearch = param.sSearch.Trim();
                lstTask = lstTask.Where(w => w.sDescription.Contains(sSearch)).AsQueryable();
            }

            if (param.lstStatus.Any())
            {
                lstTask = lstTask.Where(w => param.lstStatus.Contains(w.nTaskStatusID)).AsQueryable();
            }
            if (param.lstProgress.Any())
            {
                lstTask = lstTask.Where(w => param.lstProgress.Contains(w.nProgressID ?? 0)).AsQueryable();
            }
            if (param.lstTeam.Any())
            {
                var lstEmployeeDefault = _db.TB_Employee_Report_To.Where(w => !w.IsDelete && param.lstTeam.Contains(w.nTeamID)).Select(s => s.nEmployeeID).ToList();
                lstTask = lstTask.Where(w => lstEmployeeDefault.Contains(w.nEmployeeID)).AsQueryable();
            }
            if (param.lstEmployee.Any())
            {
                lstTask = lstTask.Where(w => param.lstEmployee.Contains(w.nEmployeeID)).AsQueryable();
            }

            #region Task Form Database
            var lstJobType = (from i in _db.TM_Task_Activity_Mapping
                              join k in _db.TM_Task_Activity on i.nActivityID equals k.nActivityID
                              join l in _db.TM_Task_Activity_Type on i.nActivityTypeID equals l.nActivityTypeID
                              select new
                              {
                                  label = l.sActivityTypeAbbr + " - " + k.sActivity,
                                  i.nMappingActivityID
                              });
            var qry = (from task in lstTask.ToList()
                       join prj in _db.TB_Project.Where(w => !w.IsDelete) on task.nProjectID equals prj.nProjectID
                       join jobtype in lstJobType on task.nTaskTypeID equals jobtype.nMappingActivityID into job
                       from jobtype in job.DefaultIfEmpty()
                       join employee in _db.TB_Employee.Where(w => !w.IsDelete) on task.nEmployeeID equals employee.nEmployeeID
                       join status in _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.TaskStatus) on task.nTaskStatusID equals status.nData_ID
                       select new TaskItem
                       {
                           sEncryptID = task.nTaskID.EncryptParameter(),
                           dTask = task.dTask,
                           sTaskDate = task.dTask.ToString("dd/MM/yyyy"),
                           nProjectID = prj.nProjectID,
                           sProjectName = prj.sProjectAbbr ?? prj.sProjectName,
                           sDescription = task.sDescription,
                           nTaskTypeID = task.nTaskTypeID,
                           sJobType = jobtype != null ? jobtype.label ?? "" : "",
                           nPlan = task.nPlan,
                           nActual = task.nActual,
                           nPlanProcess = task.nPlanProcess,
                           nActualProcess = task.nActualProcess,
                           nTaskStatusID = task.nTaskStatusID,
                           sStatus = status.sNameTH ?? "",
                           nProgressID = task.nProgressID,
                           sDescriptionDelay = task.sDescriptionDelay ?? "",
                           IsDelete = false,
                           IsModified = false,
                           IsRequireDelay = false,
                           IsLock = false,
                           sEmployeeName = employee.sNickname ?? employee.sNameTH + " " + employee.sSurnameTH
                       }
                ).ToList();
            #endregion

            qry.ForEach(f =>
            {
                var objProgress = _db.TM_Task_Progress.FirstOrDefault(w => w.nProgressID == f.nProgressID && !w.IsDelete);
                if (objProgress != null)
                {
                    f.sProgress = objProgress.sProgressName;
                }
            });

            objResult.arrFullData = qry;

            #region//SORT
            string sSortColumn = param.sSortExpression;
            switch (param.sSortExpression)
            {
                case "sTaskDate": sSortColumn = "dTask"; break;
                case "sProjectName": sSortColumn = "sProjectName"; break;
                case "sDescription": sSortColumn = "sDescription"; break;
                case "sJobType": sSortColumn = "sJobType"; break;
                case "sStatus": sSortColumn = "sStatus"; break;
                case "sDescriptionDelay": sSortColumn = "sDescriptionDelay"; break;
            }
            if (param.isASC)
            {
                qry = qry.OrderBy<TaskItem>(sSortColumn).ToList();
            }
            else if (param.isDESC)
            {
                qry = qry.OrderByDescending<TaskItem>(sSortColumn).ToList();
            }
            #endregion

            //raw data query
            // objResult.arrRawData = qry;

            #region//Final Action >> Skip , Take And Set Page
            var dataPage = STGrid.Paging(param.nPageSize, param.nPageIndex, qry.Count());
            objResult.arrData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
            int nKey = 1;
            objResult.arrData.ForEach(f =>
            {
                f.sID = nKey + "";
                nKey++;
            });
            objResult.nDataLength = dataPage.nDataLength;
            objResult.nPageIndex = dataPage.nPageIndex;
            objResult.nSkip = dataPage.nSkip;
            objResult.nTake = dataPage.nTake;
            objResult.nStartIndex = dataPage.nStartIndex;
            #endregion

            result.objResult = objResult;

            return result;
        }

        public ResultAPI GetTeamEmployee(ParamTeamEmployee param)
        {
            ResultAPI result = new ResultAPI();
            ResultGetTeamEmployee objResult = new ResultGetTeamEmployee();

            #region Employee
            var lstEmployee = _db.TB_Employee.Where(w => !w.IsDelete && w.IsActive);
            var lstEmployeeDefault = _db.TB_Employee_Report_To.Where(w => !w.IsDelete && param.lstTeam.Contains(w.nTeamID)).Select(s => s.nEmployeeID).ToList();
            if (lstEmployeeDefault.Any())
            {
                lstEmployee = lstEmployee.Where(w => lstEmployeeDefault.Contains(w.nEmployeeID));
            }
            var optEmployee = lstEmployee.Where(w => !w.IsDelete && w.IsActive).Select(s => new Option
            {
                label = s.sNameTH + " " + s.sSurnameTH + (!string.IsNullOrEmpty(s.sNickname) ? "(" + s.sNickname + ")" : ""),
                value = s.nEmployeeID.ToString()
            }).OrderBy(w => w.label).ToList();
            #endregion

            objResult.lstEmployee = optEmployee;
            result.objResult = objResult;

            return result;
        }


        public ResultAPI GetTaskRule()
        {
            ResultAPI result = new ResultAPI();

            List<TM_Data> lstTaskStatus = _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.TaskStatus && w.IsActive && !w.IsDelete).OrderBy(w => w.nOrder).ToList();
            List<TM_Task_Progress> lstTaskProgress = _db.TM_Task_Progress.Where(w => w.IsActive && !w.IsDelete).OrderBy(w => w.nOrder).ToList();
            List<TM_Task_Activity_Type> lstTaskActivityType = _db.TM_Task_Activity_Type.Where(w => !w.IsDelete).OrderBy(w => w.nOrder).ToList();

            result.objResult = new
            {
                lstTaskStatus,
                lstTaskProgress,
                lstTaskActivityType
            };
            return result;
        }

        #region TaskFormList
        //public ResultAPI GetTaskFormList(ParamSearchTask param)
        //{
        //    ResultAPI result = new ResultAPI();
        //    ResultTaskFormList objResult = new ResultTaskFormList();
        //    var ua = _auth.GetUserAccount();
        //    int nUserID = ua.nUserID;

        //    IQueryable<TB_Task> lstTask = _db.TB_Task.Where(w => !w.IsDelete && w.nEmployeeID == nUserID).AsQueryable();
        //    if (!string.IsNullOrEmpty(param.sDateStart))
        //    {
        //        DateTime dCurrent = param.sDateStart.ToDateTimeFromString();
        //        lstTask = lstTask.Where(w => w.dTask.Date >= dCurrent.Date).AsQueryable();
        //    }
        //    if (!string.IsNullOrEmpty(param.sDateEnd))
        //    {
        //        DateTime dCurrent = param.sDateEnd.ToDateTimeFromString();
        //        lstTask = lstTask.Where(w => w.dTask.Date <= dCurrent.Date).AsQueryable();
        //    }
        //    if (param.lstProject.Any())
        //    {
        //        lstTask = lstTask.Where(w => param.lstProject.Contains(w.nProjectID)).AsQueryable();
        //    }
        //    if (!string.IsNullOrEmpty(param.sSearch))
        //    {
        //        string sSearch = param.sSearch.Trim();
        //        lstTask = lstTask.Where(w => w.sDescription.Contains(sSearch)).AsQueryable();
        //    }
        //    var ccc = lstTask.ToList();
        //    #region Task Form Database
        //    var qry = (from task in lstTask
        //               join pro in _db.TM_Task_Progress on task.nProgressID equals pro.nProgressID into prog
        //               from pro in prog.DefaultIfEmpty()
        //               select new TaskFormListItem
        //               {
        //                   sEncryptID = task.nTaskID.EncryptParameter(),
        //                   nProjectID = task.nProjectID,
        //                   dTask = task.dTask,
        //                   sDescription = task.sDescription,
        //                   nTaskTypeID = task.nTaskTypeID,
        //                   nPlan = task.nPlan,
        //                   nActual = task.nActual,
        //                   nPlanProcess = task.nPlanProcess,
        //                   nActualProcess = task.nActualProcess,
        //                   nTaskStatusID = task.nTaskStatusID,
        //                   nProgressID = task.nProgressID,
        //                   sDescriptionDelay = task.sDescriptionDelay ?? "",
        //                   IsDelete = false,
        //                   IsModified = false,
        //                   IsRequireDelay = pro != null ? pro.IsRequiredDesc : false,
        //                   IsLock = task.dTask.AddDays(14).Date == DateTime.Now.Date
        //                   && (task.nTaskStatusID == (int)EnumTask.TaskStatus.Delay ||
        //                   task.nTaskStatusID == (int)EnumTask.TaskStatus.Completed ||
        //                   task.nTaskStatusID == (int)EnumTask.TaskStatus.CompletedDelay)
        //               }
        //        ).OrderBy(w => w.dTask).ThenBy(w => w.nProjectID).ToList();
        //    objResult.arrData = qry;
        //    #endregion

        //    result.objResult = objResult;

        //    return result;
        //}
        //public ResultAPI SaveTaskFormList(ParamSaveTaskFormList param)
        //{
        //    ResultAPI result = new ResultAPI();
        //    try
        //    {
        //        var dNow = DateTime.Now;
        //        int nUserID = _auth.GetUserAccount().nUserID;
        //        List<TaskFormListItem> lstModified = param.lstTaskDetail.Where(w => w.IsModified).ToList();
        //        foreach (var item in lstModified)
        //        {
        //            int nTaskID = item.sEncryptID.DecryptParameter().ToInt();
        //            TB_Task? objTask = _db.TB_Task.FirstOrDefault(w => w.nTaskID == nTaskID);
        //            if (objTask == null)
        //            {
        //                objTask = new TB_Task();
        //                objTask.nTaskID = (_db.TB_Task.Any() ? _db.TB_Task.Max(m => m.nTaskID) : 0) + 1;
        //                objTask.dCreate = dNow;
        //                objTask.nCreateBy = nUserID;
        //                objTask.nTypeRequest = 1;
        //                _db.TB_Task.Add(objTask);
        //            }
        //            objTask.nParentID = null;
        //            objTask.nProjectID = item.nProjectID;
        //            objTask.dTask = item.dTask;
        //            objTask.sDescription = item.sDescription;
        //            objTask.nPlan = item.nPlan;
        //            objTask.nPlanProcess = item.nPlanProcess;
        //            objTask.nActual = item.nActual;
        //            objTask.nActualProcess = item.nActualProcess;
        //            objTask.sDescriptionDelay = item.sDescriptionDelay;
        //            objTask.nTaskTypeID = item.nTaskTypeID;
        //            objTask.nTaskStatusID = item.nTaskStatusID;
        //            objTask.nEmployeeID = nUserID;
        //            objTask.nProgressID = item.nProgressID;
        //            objTask.nPlanTypeID = item.nProgressID.HasValue ? (item.nProgressID.Value == 6 ? 112 : 111) : 111;

        //            objTask.dUpdate = dNow;
        //            objTask.nUpdateBy = nUserID;
        //            objTask.IsDelete = false;
        //            objTask.IsComplete = item.nTaskStatusID == (int)EnumTask.TaskStatus.Completed;

        //            _db.SaveChanges();
        //        }


        //        result.nStatusCode = StatusCodes.Status200OK;
        //    }
        //    catch (Exception e)
        //    {
        //        result.nStatusCode = StatusCodes.Status500InternalServerError;
        //        result.sMessage = e.Message;
        //    }
        //    return result;
        //}

        public ResultAPI GetTaskFormList(ParamSearchTask param)
        {
            ResultAPI result = new ResultAPI();
            ResultTaskFormList objResult = new ResultTaskFormList();
            var ua = _auth.GetUserAccount();
            int nUserID = ua.nUserID;

            DateTime dTaskStart = DateTime.Now;
            DateTime dTaskEnd = DateTime.Now;

            IQueryable<TB_Task> lstTask = _db.TB_Task.Where(w => !w.IsDelete && w.nEmployeeID == nUserID).AsQueryable();
            if (!string.IsNullOrEmpty(param.sDateStart))
            {
                dTaskStart = param.sDateStart.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                lstTask = lstTask.Where(w => w.dTask.Date >= dTaskStart.Date).AsQueryable();
            }
            if (!string.IsNullOrEmpty(param.sDateEnd))
            {
                dTaskEnd = param.sDateEnd.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                lstTask = lstTask.Where(w => w.dTask.Date <= dTaskEnd.Date).AsQueryable();
            }
            if (param.lstProject.Any())
            {
                lstTask = lstTask.Where(w => param.lstProject.Contains(w.nProjectID)).AsQueryable();
            }
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                string sSearch = param.sSearch.Trim();
                lstTask = lstTask.Where(w => w.sDescription.Contains(sSearch)).AsQueryable();
            }

            #region Task Form Database
            var qry = (from task in lstTask
                       join pro in _db.TM_Task_Progress on task.nProgressID equals pro.nProgressID into prog
                       from pro in prog.DefaultIfEmpty()
                       select new TaskFormListItem
                       {
                           sEncryptID = task.nTaskID.EncryptParameter(),
                           nProjectID = task.nProjectID,
                           dTask = task.dTask,
                           sDescription = task.sDescription,
                           nTaskTypeID = task.nTaskTypeID,
                           nPlan = task.nPlan,
                           nActual = task.nActual,
                           nPlanProcess = task.nPlanProcess,
                           nActualProcess = task.nActualProcess,
                           nTaskStatusID = task.nTaskStatusID,
                           nProgressID = task.nProgressID,
                           sDescriptionDelay = task.sDescriptionDelay ?? "",
                           IsDelete = false,
                           IsModified = false,
                           IsRequireDelay = pro != null ? pro.IsRequiredDesc : false,
                           IsLock = task.dTask.AddDays(14).Date == DateTime.Now.Date
                           && (task.nTaskStatusID == (int)EnumTask.TaskStatus.Delay ||
                           task.nTaskStatusID == (int)EnumTask.TaskStatus.Completed ||
                           task.nTaskStatusID == (int)EnumTask.TaskStatus.CompletedDelay)
                       }
                ).OrderBy(w => w.dTask).ThenBy(w => w.nProjectID).ToList();
            objResult.arrData = qry;

            List<TaskFormList> lstTaskGroup = new List<TaskFormList>();
            for (var day = dTaskStart; day.Date <= dTaskEnd; day = day.AddDays(1))
            {
                var lstTaskItem = qry.Where(w => w.dTask.Date == day.Date).ToList();
                if (!lstTaskItem.Any())
                {
                    lstTaskItem.Add(new TaskFormListItem
                    {
                        sEncryptID = "",
                        nProjectID = null,
                        dTask = day,
                        sDescription = "",
                        nTaskTypeID = null,
                        nPlan = null,
                        nActual = null,
                        nPlanProcess = null,
                        nActualProcess = null,
                        nTaskStatusID = (int)EnumTask.TaskStatus.NotStart,
                        nProgressID = null,
                        sDescriptionDelay = null,
                        IsDelete = false,
                        IsModified = false,
                        IsRequireDelay = false,
                        IsLock = false,
                    });
                }
                lstTaskGroup.Add(new TaskFormList
                {
                    sTaskDate = day.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH),
                    nSumPlan = lstTaskItem.Sum(s => s.nPlan),
                    nSumActual = lstTaskItem.Sum(s => s.nActual),
                    lstTaskItem = lstTaskItem
                });
            }
            #endregion

            List<TaskProgressOption> optNotStart = new List<TaskProgressOption>();
            List<TaskProgressOption> optNotComplete = new List<TaskProgressOption>();
            List<TaskProgressOption> optComplete = new List<TaskProgressOption>();

            #region task status
            lstTaskGroup.ForEach(f =>
            {
                f.lstTaskItem.ForEach(item =>
                {
                    if (item.nTaskStatusID == 97)
                    {
                        optNotStart = _db.TM_Task_Progress.Where(w => (w.nProgressID == 4 || w.nProgressID == 5 || w.nProgressID == 7) && w.IsActive && !w.IsDelete).OrderBy(w => w.nOrder).Select(s => new TaskProgressOption
                        {
                            label = s.sProgressName,
                            value = s.nProgressID.ToString(),
                            IsRequiredDesc = s.IsRequiredDesc
                        }).ToList();
                    }
                    else if (item.nTaskStatusID == 98)
                    {
                        optNotComplete = _db.TM_Task_Progress.Where(w => (w.nProgressID == 2 || w.nProgressID == 4) && w.IsActive && !w.IsDelete).OrderBy(w => w.nOrder).Select(s => new TaskProgressOption
                        {
                            label = s.sProgressName,
                            value = s.nProgressID.ToString(),
                            IsRequiredDesc = s.IsRequiredDesc
                        }).ToList();
                    }
                    else if (item.nTaskStatusID == 100)
                    {
                        optComplete = _db.TM_Task_Progress.Where(w => (w.nProgressID == 1 || w.nProgressID == 3 || w.nProgressID == 6) && w.IsActive && !w.IsDelete).OrderBy(w => w.nOrder).Select(s => new TaskProgressOption
                        {
                            label = s.sProgressName,
                            value = s.nProgressID.ToString(),
                            IsRequiredDesc = s.IsRequiredDesc
                        }).ToList();
                    }
                });
            });
            #endregion

            result.objResult = new
            {
                lstTaskGroup,
                optNotStart,
                optNotComplete,
                optComplete
            };

            return result;
        }
        public ResultAPI SaveTaskFormList(ParamSaveTaskFormList param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                var dNow = DateTime.Now;
                int nUserID = _auth.GetUserAccount().nUserID;

                foreach (var itemDate in param.lstTaskDetail)
                {
                    List<TaskFormListItem> lstModified = itemDate.lstTaskItem.Where(w => w.IsModified).ToList();
                    foreach (var item in lstModified)
                    {
                        int nTaskID = item.sEncryptID.DecryptParameter().ToInt();
                        TB_Task? objTask = _db.TB_Task.FirstOrDefault(w => w.nTaskID == nTaskID);
                        if (objTask == null)
                        {
                            objTask = new TB_Task();
                            objTask.nTaskID = (_db.TB_Task.Any() ? _db.TB_Task.Max(m => m.nTaskID) : 0) + 1;
                            objTask.dCreate = dNow;
                            objTask.nCreateBy = nUserID;
                            objTask.nTypeRequest = 1;
                            _db.TB_Task.Add(objTask);
                        }
                        objTask.nParentID = null;
                        objTask.nProjectID = item.nProjectID ?? 0;
                        objTask.dTask = item.dTask;
                        objTask.sDescription = item.sDescription;
                        objTask.nPlan = item.nPlan;
                        objTask.nPlanProcess = item.nPlanProcess;
                        objTask.nActual = item.nActual;
                        objTask.nActualProcess = item.nActualProcess;
                        objTask.sDescriptionDelay = item.sDescriptionDelay;
                        objTask.nTaskTypeID = item.nTaskTypeID ?? 0;
                        objTask.nTaskStatusID = item.nTaskStatusID;
                        objTask.nEmployeeID = nUserID;
                        objTask.nProgressID = item.nProgressID;
                        objTask.nPlanTypeID = item.nProgressID.HasValue ? (item.nProgressID.Value == 6 ? 112 : 111) : 111;

                        objTask.dUpdate = dNow;
                        objTask.nUpdateBy = nUserID;
                        objTask.IsDelete = false;
                        objTask.IsComplete = item.nTaskStatusID == (int)EnumTask.TaskStatus.Completed;

                        _db.SaveChanges();
                    }
                }
                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }
        #endregion

        public ResultAPI SaveTaskPlanMultiDate(ParamSaveTaskMultiDate param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                var dNow = DateTime.Now;
                int nUserID = _auth.GetUserAccount().nUserID;
                int? nParentID = null;
                foreach (var item in param.lstTaskDetail)
                {
                    int nTaskID = item.sEncryptID.DecryptParameter().ToInt();
                    TB_Task? objTask = _db.TB_Task.FirstOrDefault(w => w.nTaskID == nTaskID);
                    if (objTask == null)
                    {
                        objTask = new TB_Task();
                        objTask.nTaskID = (_db.TB_Task.Any() ? _db.TB_Task.Max(m => m.nTaskID) : 0) + 1;
                        objTask.dCreate = dNow;
                        objTask.nCreateBy = nUserID;
                        objTask.nTypeRequest = 1;
                        if (!nParentID.HasValue)
                        {
                            nParentID = objTask.nTaskID;
                        }
                        _db.TB_Task.Add(objTask);
                    }
                    objTask.nProjectID = param.nProjectID;
                    objTask.dTask = item.dTask;
                    objTask.sDescription = param.sDescription;
                    objTask.nPlan = item.nPlan;
                    objTask.nPlanProcess = item.nPlanProcess;
                    objTask.nActual = item.nActual;
                    objTask.nActualProcess = item.nActualProcess;
                    objTask.nTaskTypeID = param.nTaskTypeID;
                    objTask.nTaskStatusID = item.nTaskStatusID;
                    objTask.nProgressID = item.nProgressID;
                    objTask.nEmployeeID = nUserID;
                    objTask.sDescriptionDelay = item.sDescriptionDelay;
                    objTask.dUpdate = dNow;
                    objTask.nUpdateBy = nUserID;
                    objTask.IsDelete = false;
                    objTask.nPlanTypeID = (int)MasterData.Plan;
                    if (nParentID != objTask.nTaskID)
                    {
                        objTask.nParentID = nParentID;
                    }
                    _db.SaveChanges();
                }


                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = e.Message;
            }
            return result;
        }

        public ExportExcel onExportTaskMonitorReport(ExportTaskMonitorReport param)
        {
            ExportExcel result = new ExportExcel();
            string sFileName = $"Task Monitor_{param.sDateForFilename}_{param.sTimeForFilename}";

            #region data
            IQueryable<TB_Task> lstTask = _db.TB_Task.Where(w => !w.IsDelete).AsQueryable();
            if (!string.IsNullOrEmpty(param.sDateStart))
            {
                DateTime dCurrent = param.sDateStart.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                lstTask = lstTask.Where(w => w.dTask.Date >= dCurrent.Date).AsQueryable();
            }
            if (!string.IsNullOrEmpty(param.sDateEnd))
            {
                DateTime dCurrent = param.sDateEnd.ToDateTimeFromString(ST.INFRA.Enum.DateTimeFormat.yyyy_MM_ddHHmm);
                lstTask = lstTask.Where(w => w.dTask.Date <= dCurrent.Date).AsQueryable();
            }
            if (param.lstProject.Any())
            {
                lstTask = lstTask.Where(w => param.lstProject.Contains(w.nProjectID)).AsQueryable();
            }
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                string sSearch = param.sSearch.Trim();
                lstTask = lstTask.Where(w => w.sDescription.Contains(sSearch)).AsQueryable();
            }

            if (param.lstStatus.Any())
            {
                lstTask = lstTask.Where(w => param.lstStatus.Contains(w.nTaskStatusID)).AsQueryable();
            }
            if (param.lstProgress.Any())
            {
                lstTask = lstTask.Where(w => param.lstProgress.Contains(w.nProgressID ?? 0)).AsQueryable();
            }
            if (param.lstTeam.Any())
            {
                var lstEmployeeDefault = _db.TB_Employee_Report_To.Where(w => !w.IsDelete && param.lstTeam.Contains(w.nTeamID)).Select(s => s.nEmployeeID).ToList();
                lstTask = lstTask.Where(w => lstEmployeeDefault.Contains(w.nEmployeeID)).AsQueryable();
            }
            if (param.lstEmployee.Any())
            {
                lstTask = lstTask.Where(w => param.lstEmployee.Contains(w.nEmployeeID)).AsQueryable();
            }

            #region Task Form Database
            var lstTaskx = lstTask.ToList();
            var lstJobType = (from i in _db.TM_Task_Activity_Mapping
                              join k in _db.TM_Task_Activity on i.nActivityID equals k.nActivityID
                              join l in _db.TM_Task_Activity_Type on i.nActivityTypeID equals l.nActivityTypeID
                              select new
                              {
                                  label = l.sActivityTypeAbbr + " - " + k.sActivity,
                                  i.nMappingActivityID
                              });
            var qry = (from task in lstTask.ToList()
                       join prj in _db.TB_Project.Where(w => !w.IsDelete) on task.nProjectID equals prj.nProjectID
                       join jobtype in lstJobType on task.nTaskTypeID equals jobtype.nMappingActivityID into job
                       from jobtype in job.DefaultIfEmpty()
                       join employee in _db.TB_Employee.Where(w => !w.IsDelete) on task.nEmployeeID equals employee.nEmployeeID
                       join status in _db.TM_Data.Where(w => w.nDatatypeID == (int)EnumGlobal.MasterDataType.TaskStatus) on task.nTaskStatusID equals status.nData_ID
                       select new TaskItem
                       {
                           sEncryptID = task.nTaskID.EncryptParameter(),
                           dTask = task.dTask,
                           sTaskDate = task.dTask.ToString("dd/MM/yyyy"),
                           nProjectID = prj.nProjectID,
                           sProjectName = prj.sProjectAbbr ?? prj.sProjectName,
                           sDescription = task.sDescription,
                           nTaskTypeID = task.nTaskTypeID,
                           sJobType = jobtype != null ? jobtype.label ?? "" : "",
                           nPlan = task.nPlan,
                           nActual = task.nActual,
                           nPlanProcess = task.nPlanProcess,
                           nActualProcess = task.nActualProcess,
                           nTaskStatusID = task.nTaskStatusID,
                           sStatus = status.sNameTH ?? "",
                           nProgressID = task.nProgressID,
                           sDescriptionDelay = task.sDescriptionDelay ?? "",
                           IsDelete = false,
                           IsModified = false,
                           IsRequireDelay = false,
                           IsLock = false,
                           sEmployeeName = employee.sNickname ?? employee.sNameTH + " " + employee.sSurnameTH
                       }
                ).ToList();

            qry.ForEach(f =>
            {
                var objProgress = _db.TM_Task_Progress.FirstOrDefault(w => w.nProgressID == f.nProgressID && !w.IsDelete);
                if (objProgress != null)
                {
                    f.sProgress = objProgress.sProgressName;
                }
            });

            #region//SORT
            string sSortColumn = param.sSortExpression;
            switch (param.sSortExpression)
            {
                case "sTaskDate": sSortColumn = "dTask"; break;
                case "sProjectName": sSortColumn = "sProjectName"; break;
                case "sDescription": sSortColumn = "sDescription"; break;
                case "sJobType": sSortColumn = "sJobType"; break;
                case "sStatus": sSortColumn = "sStatus"; break;
                case "sDescriptionDelay": sSortColumn = "sDescriptionDelay"; break;
            }
            if (param.sSortDirection == "asc")
            {
                qry = qry.OrderBy<TaskItem>(sSortColumn).ToList();
            }
            else if (param.sSortDirection == "desc")
            {
                qry = qry.OrderByDescending<TaskItem>(sSortColumn).ToList();
            }
            #endregion

            #endregion
            #endregion

            #region generate excel
            var wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("Sheet1");

            int nFontSize = 14;
            int nFontSizeHead = 14;
            string sFontName = Enum.EnumExportExcel.ExportExcelFont.FontName.GetEnumValue();

            ws.PageSetup.Margins.Top = 0.2;
            ws.PageSetup.Margins.Bottom = 0.2;
            ws.PageSetup.Margins.Left = 0.1;
            ws.PageSetup.Margins.Right = 0;
            ws.PageSetup.Margins.Footer = 0;
            ws.PageSetup.Margins.Header = 0;

            int nRow = 2;
            int nRowChild = 7;
            List<string> lstEmpHead = new List<string>() { "Date", "Employee", "Project", "Task", "Plan Hour", "Target(%)", "Actual Hour", "Actual(%)", "Status", "Progress", "Note" };
            List<int> lstwidthHeader = new List<int>() { 15, 15, 25, 55, 10, 15, 10, 15, 15, 15, 55 };

            #region head of sheet
            ws.Cell(1, 1).Value = $"Export Date: {param.sExportDate} {param.sExportTime} น.";
            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell(1, 1).Style.Font.FontSize = nFontSize;
            ws.Cell(1, 1).Style.Font.FontName = sFontName;
            ws.Cell(1, 1).Style.Font.Bold = false;
            ws.Cell(1, 1).Style.Alignment.WrapText = true;
            ws.Range(1, 1, 1, 3).Merge();

            ws.Cell(1, 4).Value = $"แบบฟอร์มบันทึกกิจกรรม Morning Talk (Everryday 09:00 - 09:30)";
            ws.Cell(1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Cell(1, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell(1, 4).Style.Font.FontSize = nFontSize;
            ws.Cell(1, 4).Style.Font.FontName = sFontName;
            ws.Cell(1, 4).Style.Font.Bold = false;
            ws.Cell(1, 4).Style.Alignment.WrapText = true;
            ws.Range(1, 4, 1, 7).Merge();
            #endregion

            int nColWidth = 1;
            int indexHead = 0;
            int indexHeadTwo = 0;

            lstwidthHeader.ForEach(itmWidth =>
            {
                ws.Column(nColWidth).Width = itmWidth;
                nColWidth++;
            });

            lstEmpHead.ForEach(item =>
            {
                indexHead += 1;
                ws.Cell(nRow, indexHead).Value = item;
                ws.Cell(nRow, indexHead).Style.Font.FontSize = nFontSizeHead;
                ws.Cell(nRow, indexHead).Style.Font.FontName = sFontName;
                ws.Cell(nRow, indexHead).Style.Font.Bold = false;
                ws.Cell(nRow, indexHead).Style.Alignment.WrapText = true; //ปัดบรรทัด
                ws.Cell(nRow, indexHead).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(nRow, indexHead).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(nRow, indexHead).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#01b0f1"));

                if (indexHead == 5 || indexHead == 6)
                {
                    ws.Cell(nRow, indexHead).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#ffe698"));
                }

                if (indexHead == 7 || indexHead == 8)
                {
                    ws.Cell(nRow, indexHead).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#c6dfb3"));
                }
            });

            //body
            if (qry.Count > 0)
            {
                qry.ForEach(f =>
                {
                    nRow++;

                    ws.Cell(nRow, 1).Value = f.sTaskDate;
                    ws.Cell(nRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(nRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, 1).Style.Font.FontSize = nFontSize;
                    ws.Cell(nRow, 1).Style.Font.FontName = sFontName;
                    ws.Cell(nRow, 1).Style.Font.Bold = false;
                    ws.Cell(nRow, 1).Style.Alignment.WrapText = true;

                    ws.Cell(nRow, 2).Value = f.sEmployeeName;
                    ws.Cell(nRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(nRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, 2).Style.Font.FontSize = nFontSize;
                    ws.Cell(nRow, 2).Style.Font.FontName = sFontName;
                    ws.Cell(nRow, 2).Style.Font.Bold = false;
                    ws.Cell(nRow, 2).Style.Alignment.WrapText = true;

                    ws.Cell(nRow, 3).Value = f.sProjectName;
                    ws.Cell(nRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Cell(nRow, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, 3).Style.Font.FontSize = nFontSize;
                    ws.Cell(nRow, 3).Style.Font.FontName = sFontName;
                    ws.Cell(nRow, 3).Style.Font.Bold = false;
                    ws.Cell(nRow, 3).Style.Alignment.WrapText = true;

                    ws.Cell(nRow, 4).Value = f.sDescription;
                    ws.Cell(nRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Cell(nRow, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, 4).Style.Font.FontSize = nFontSize;
                    ws.Cell(nRow, 4).Style.Font.FontName = sFontName;
                    ws.Cell(nRow, 4).Style.Font.Bold = false;
                    ws.Cell(nRow, 4).Style.Alignment.WrapText = true;

                    ws.Cell(nRow, 5).Value = f.nPlan == 0 ? "-" : f.nPlan;
                    ws.Cell(nRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(nRow, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, 5).Style.Font.FontSize = nFontSize;
                    ws.Cell(nRow, 5).Style.Font.FontName = sFontName;
                    ws.Cell(nRow, 5).Style.Font.Bold = false;
                    ws.Cell(nRow, 5).Style.Alignment.WrapText = true;

                    ws.Cell(nRow, 6).Value = f.nPlanProcess == 0 ? "-" : f.nPlanProcess;
                    ws.Cell(nRow, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(nRow, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, 6).Style.Font.FontSize = nFontSize;
                    ws.Cell(nRow, 6).Style.Font.FontName = sFontName;
                    ws.Cell(nRow, 6).Style.Font.Bold = false;
                    ws.Cell(nRow, 6).Style.Alignment.WrapText = true;

                    ws.Cell(nRow, 7).Value = f.nActual;
                    ws.Cell(nRow, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(nRow, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, 7).Style.Font.FontSize = nFontSize;
                    ws.Cell(nRow, 7).Style.Font.FontName = sFontName;
                    ws.Cell(nRow, 7).Style.Font.Bold = false;
                    ws.Cell(nRow, 7).Style.Alignment.WrapText = true;

                    ws.Cell(nRow, 8).Value = f.nActualProcess;
                    ws.Cell(nRow, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(nRow, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, 8).Style.Font.FontSize = nFontSize;
                    ws.Cell(nRow, 8).Style.Font.FontName = sFontName;
                    ws.Cell(nRow, 8).Style.Font.Bold = false;
                    ws.Cell(nRow, 8).Style.Alignment.WrapText = true;

                    ws.Cell(nRow, 9).Value = f.sStatus;
                    ws.Cell(nRow, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(nRow, 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, 9).Style.Font.FontSize = nFontSize;
                    ws.Cell(nRow, 9).Style.Font.FontName = sFontName;
                    ws.Cell(nRow, 9).Style.Font.Bold = false;
                    ws.Cell(nRow, 9).Style.Alignment.WrapText = true;

                    ws.Cell(nRow, 10).Value = f.sProgress;
                    ws.Cell(nRow, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(nRow, 10).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, 10).Style.Font.FontSize = nFontSize;
                    ws.Cell(nRow, 10).Style.Font.FontName = sFontName;
                    ws.Cell(nRow, 10).Style.Font.Bold = false;
                    ws.Cell(nRow, 10).Style.Alignment.WrapText = true;

                    ws.Cell(nRow, 11).Value = f.sDescriptionDelay;
                    ws.Cell(nRow, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Cell(nRow, 11).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(nRow, 11).Style.Font.FontSize = nFontSize;
                    ws.Cell(nRow, 11).Style.Font.FontName = sFontName;
                    ws.Cell(nRow, 11).Style.Font.Bold = false;
                    ws.Cell(nRow, 11).Style.Alignment.WrapText = true;

                    if (f.sProgress == "Delay")
                    {
                        ws.Cell(nRow, indexHead).Style.Fill.BackgroundColor = XLColor.FromColor(System.Drawing.ColorTranslator.FromHtml("#ffc8cf"));
                    }
                });
            }
            else
            {
                nRow = 3;
                ws.Cell(3, 11).Value = "No data";
                ws.Cell(3, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(3, 11).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(3, 11).Style.Font.FontSize = nFontSize;
                ws.Cell(3, 11).Style.Font.FontName = sFontName;
                ws.Cell(3, 11).Style.Font.Bold = false;
                ws.Cell(3, 11).Style.Alignment.WrapText = true;
                ws.Range(3, 1, 3, 11).Merge();
            }

            //ระยะเส้นขอบที่ต้องการให้แสดง
            ws.Range(2, 1, nRow, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range(2, 1, nRow, 11).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws.Range(2, 1, nRow, 11).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            ws.Range(2, 1, nRow, 11).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Range(2, 1, nRow, 11).Style.Border.TopBorder = XLBorderStyleValues.Thin;

            using (MemoryStream fs = new MemoryStream())
            {
                wb.SaveAs(fs);
                wb.Dispose();
                fs.Position = 0;
                result.objFile = fs.ToArray();
                result.sFileType = "application/vnd.ms-excel";
                result.sFileName = sFileName;
            }
            #endregion

            return result;
        }
    }
}