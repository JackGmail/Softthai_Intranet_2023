
using Extensions.Common.STResultAPI;
using ST.INFRA.Common;

namespace Backend.Models
{
    public class ResultOptDailyTask
    {
        public List<Option> lstJobType { get; set; } = new List<Option>();
        public List<Option> lstTaskStatus { get; set; } = new List<Option>();
        public List<TaskProgressOption> lstTaskProgress { get; set; } = new List<TaskProgressOption>();
        public List<Option> lstProject { get; set; } = new List<Option>();
        public List<Option> lstEmployee { get; set; } = new List<Option>();
        public List<Option> lstTeam { get; set; } = new List<Option>();
        public List<string> lstTeamDefault { get; set; } = new List<string>();
        public List<string> lstEmployeeDefault { get; set; } = new List<string>();
    }
    public class Option
    {
        public string? label { get; set; }
        public string? value { get; set; }
    }

    public class TaskProgressOption : Option
    {
        public bool IsRequiredDesc { get; set; }
    }

    public class ResultTask : Pagination
    {
        public List<TaskItem> arrData { get; set; } = new List<TaskItem>();
        // public List<TaskItem> arrRawData { get; set; } = new List<TaskItem>();
        public List<TaskItem> arrFullData { get; set; } = new List<TaskItem>();
    }

    public class ParamTask : STGrid.PaginationData
    {
        public string? sID { get; set; } = string.Empty;
        public string? sDateStart { get; set; } = string.Empty;
        public string? sDateEnd { get; set; } = string.Empty;
        public List<int> lstProject { get; set; } = new List<int>();
        public string? sTokenID { get; set; }
        public List<TaskItem> lstTaskTemp { get; set; } = new List<TaskItem>();
    }

    public class ParamSaveTask
    {
        public List<TaskItem> lstTask { get; set; } = new List<TaskItem>();
    }
    public class ParamAddTask : STGrid.PaginationData
    {
        public List<TaskItem> lstTask { get; set; } = new List<TaskItem>();
    }

    public class ParamRemoveTask : STGrid.PaginationData
    {
        public List<string> lstID { get; set; } = new List<string>();
        public List<TaskItem> lstTask { get; set; } = new List<TaskItem>();
    }

    public class ParamTaskOverAll : STGrid.PaginationData
    {
        public string? sID { get; set; } = string.Empty;
        public string? sDateStart { get; set; } = string.Empty;
        public string? sDateEnd { get; set; } = string.Empty;
        public List<int> lstProject { get; set; } = new List<int>();
        public List<int> lstStatus { get; set; } = new List<int>();
        public List<int> lstProgress { get; set; } = new List<int>();
        public List<int> lstTeam { get; set; } = new List<int>();
        public List<int> lstEmployee { get; set; } = new List<int>();
    }

    public class TaskItem
    {
        public string sID { get; set; } = string.Empty;
        public string sEncryptID { get; set; } = string.Empty;
        public int? nProjectID { get; set; }
        public DateTime dTask { get; set; }
        public string sTaskDate { get; set; } = string.Empty;
        public string sProjectName { get; set; } = string.Empty;
        public string sDescription { get; set; } = string.Empty;
        public int? nTaskTypeID { get; set; }
        public string sJobType { get; set; } = string.Empty;
        public decimal? nPlan { get; set; }
        public decimal? nPlanProcess { get; set; }
        public decimal? nActual { get; set; }
        public decimal? nActualProcess { get; set; }
        public int nTaskStatusID { get; set; }
        public string sStatus { get; set; } = string.Empty;
        public string? sProgress { get; set; } = string.Empty;
        public string sDescriptionDelay { get; set; } = string.Empty;
        public bool IsDelete { get; set; }
        public bool IsModified { get; set; }
        public bool IsRequireDelay { get; set; }
        public bool IsLock { get; set; }
        public string sEmployeeName { get; set; } = string.Empty;
        public int? nProgressID { get; set; }
    }

    public class ParamTeamEmployee
    {
        public List<int> lstTeam { get; set; } = new List<int>();
    }

    public class ResultGetTeamEmployee
    {
        public List<Option> lstEmployee { get; set; } = new List<Option>();
    }


    #region TaskFormList
    public class ParamSearchTask : STGrid.PaginationData
    {
        public string? sID { get; set; } = string.Empty;
        public string? sDateStart { get; set; } = string.Empty;
        public string? sDateEnd { get; set; } = string.Empty;
        public List<int> lstProject { get; set; } = new List<int>();
    }
    public class ResultTaskFormList
    {
        public List<TaskFormListItem> arrData { get; set; } = new List<TaskFormListItem>();
    }
    public class TaskFormList
    {
        public string sTaskDate { get; set; } = string.Empty;
        public decimal? nSumPlan { get; set; }
        public decimal? nSumActual { get; set; }
        public List<TaskFormListItem> lstTaskItem { get; set; } = new List<TaskFormListItem>();
    }
    public class TaskFormListItem
    {
        public string sEncryptID { get; set; } = string.Empty;
        public int? nProjectID { get; set; }
        public int? nTaskTypeID { get; set; }
        public DateTime dTask { get; set; }
        public string sDescription { get; set; } = string.Empty;
        public decimal? nPlan { get; set; }
        public decimal? nPlanProcess { get; set; }
        public decimal? nActual { get; set; }
        public decimal? nActualProcess { get; set; }
        public int? nProgressID { get; set; }
        public int nTaskStatusID { get; set; }
        public string? sDescriptionDelay { get; set; }
        public bool IsDelete { get; set; }
        public bool IsModified { get; set; }
        public bool IsRequireDelay { get; set; }
        public bool IsLock { get; set; }
    }
    public class ParamSaveTaskFormList
    {
        public List<TaskFormList> lstTaskDetail { get; set; } = new List<TaskFormList>();
    }
    #endregion

    #region TaskPlanMultiDate
    public class ParamSaveTaskMultiDate
    {
        public int nProjectID { get; set; }
        public int nTaskTypeID { get; set; }
        public string sDescription { get; set; } = string.Empty;
        public List<TaskItemMultiDate> lstTaskDetail { get; set; } = new List<TaskItemMultiDate>();
    }
    public class TaskItemMultiDate
    {
        public string sEncryptID { get; set; } = string.Empty;
        public DateTime dTask { get; set; }
        public decimal? nPlan { get; set; }
        public decimal? nPlanProcess { get; set; }
        public decimal? nActual { get; set; }
        public decimal? nActualProcess { get; set; }
        public int nTaskStatusID { get; set; }
        public int? nProgressID { get; set; }
        public string sDescriptionDelay { get; set; } = string.Empty;
        public bool IsDelete { get; set; }
    }

    #endregion

    public class ExportExcel : ResultAPI
    {
        public byte[] objFile { get; set; } = Array.Empty<byte>();
        public string sFileType { get; set; } = String.Empty;
        public string? sFileName { get; set; }
    }

    public class ExportTaskMonitorReport : STGrid.PaginationData
    {
        public List<TaskItem> arrRawData { get; set; } = new List<TaskItem>();
        public string sSortExpression {get; set;}
        public string sSortDirection {get; set;}
        public string? sID { get; set; } = string.Empty;
        public string? sDateStart { get; set; } = string.Empty;
        public string? sDateEnd { get; set; } = string.Empty;
        public List<int> lstProject { get; set; } = new List<int>();
        public List<int> lstStatus { get; set; } = new List<int>();
        public List<int> lstProgress { get; set; } = new List<int>();
        public List<int> lstTeam { get; set; } = new List<int>();
        public List<int> lstEmployee { get; set; } = new List<int>();
        public string sExportDate { get; set; }
        public string sExportTime { get; set; }
        public string? sDateForFilename { get; set; } = string.Empty;
        public string? sTimeForFilename { get; set; } = string.Empty;
    }
}