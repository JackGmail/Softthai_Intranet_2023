using Extensions.Common.STExtension;
using Extensions.Common.STResultAPI;
using ST.INFRA.Common;

namespace Backend.Models
{
    /// <summary>
    /// </summary>
    public class cWFHRequest
    {
        /// <summary>
        /// WFH ID
        /// </summary>
        public int? nWFHRequestID { get; set; }
    }

    public class cActionWFH
    {

        public bool? IsBtnDraft { get; set; }
        public bool? IsBtnSubmit { get; set; }
        public bool? IsBtnApprove { get; set; }
        public bool? IsBtnReject { get; set; }
        public bool? IsBtnRecall { get; set; }
        public bool? IsBtnCancel { get; set; }
    }

    public class ParamProjectID
    {
        public List<int>? lstProjectID { get; set; }
    }


    /// <summary>
    /// </summary>
    public class cWFHRequestHistory
    {
        /// <summary>
        /// WFH ID
        /// </summary>
        public int nWFHID { get; set; }
        /// <summary>
        /// </summary>
        public int nFlowProcessID { get; set; }
        /// <summary>
        /// </summary>
        public int nApproveBy { get; set; }
        /// <summary>
        /// </summary>
        public DateTime dApprove { get; set; }
        /// <summary>
        /// </summary>
        public string? sComment { get; set; }
    }

    /// <summary>
    /// </summary>
    public class cWFHFlow
    {
        /// <summary>
        /// </summary>
        public int nWFHID { get; set; }
        /// <summary>
        /// </summary>
        public int? nRequesterID { get; set; }
        /// <summary>
        /// </summary>
        public int? nRequesterPositionID { get; set; }
        /// <summary>
        /// </summary>
        public int nMode { get; set; }
        /// <summary>
        /// </summary>
        public List<cLineApprover>? lstApprover { get; set; }
        /// <summary>
        /// </summary>
        public string? sComment { get; set; }
    }

    /// <summary>
    /// </summary>
    public class cUpdateFlow
    {
        /// <summary>
        /// </summary>
        public string? sWFHID { get; set; }
        /// <summary>
        /// </summary>
        public int nMode { get; set; }
        /// <summary>
        /// </summary>
        public string? sComment { get; set; }
    }

    /// <summary>
    /// </summary>
    public class cLineApprover
    {
        /// <summary>
        /// </summary>
        public int nWFHID { get; set; }
        /// <summary>
        /// </summary>
        public int? nApproveID { get; set; }
        /// <summary>
        /// </summary>
        public int nPositionID { get; set; }
        /// <summary>
        /// </summary>
        public bool IsLineApprover { get; set; }
    }
    /// <summary>
    /// </summary>
    public class cResWFHFlow
    {
        /// <summary>
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// </summary>
        public string? sMessage { get; set; }
    }

    /// <summary>
    /// </summary>
    public class clcFilterWFH : STGrid.PaginationData
    {
        /// <summary>
        /// </summary>
        public string sType { get; set; } = "";
        /// <summary>
        /// </summary>
        public string? sStart { get; set; }
        /// <summary>
        /// </summary>
        public string? sEnd { get; set; }
        /// <summary>
        /// </summary>
        public string? listRequest { get; set; }
        /// <summary>
        /// </summary>
        public string? listStatus { get; set; }

    }

    /// <summary>
    /// </summary>
    public class clcResultTableWFH : Pagination
    {
        /// <summary>
        /// </summary>
        public List<objResultTableWFH>? lstData { get; set; }
        /// <summary>
        /// </summary>
        public List<cSelectOption>? listOpton { get; set; }
        /// <summary>
        /// </summary>
        public List<cSelectOption>? listEmp { get; set; }
        /// <summary>
        /// </summary>
        public List<string> listUser { get; set; } = new List<string>();
        /// <summary>
        /// </summary>
        public List<string> listFlow { get; set; } = new List<string>();
    }
    /// <summary>
    /// </summary>
    public class objResultTableWFH
    {
        /// <summary>
        /// </summary>
        public int No { get; set; }
        /// <summary>
        /// </summary>
        public int nID { get; set; }
        public string sID { get; set; }
        public bool? IsCanDel { get; set; }
        /// <summary>
        /// </summary>
        public string sWFHDate { get; set; } = "";
        /// <summary>
        /// </summary>
        public List<objTaskTable>? lsitTask { get; set; }
        /// <summary>
        /// </summary>
        public string sRequestDate { get; set; } = "";
        /// <summary>
        /// </summary>
        public string sRequester { get; set; } = "";
        /// <summary>
        /// </summary>
        public string sWaitingBy { get; set; } = "";
        /// <summary>
        /// </summary>
        public string sStatus { get; set; } = "";
        /// <summary>
        /// </summary>
        public string sComment { get; set; } = "";
        /// <summary>
        /// </summary>
        public int? nFlowProcessID { get; set; }
        public DateTime dWFH { get; set; }
        public DateTime dUpdate { get; set; }
        public int nCreateBy { get; set; }
        public bool? IsRequester { get; set; }
        public bool? IsApprover { get; set; }
    }

    /// <summary>
    /// </summary>
    public class objTaskTable
    {
        /// <summary>
        /// </summary>
        public string Project { get; set; } = "";
        /// <summary>
        /// </summary>
        public decimal? Progress { get; set; }
        /// <summary>
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// </summary>
        public decimal Manhour { get; set; }

    }


    #region sun
    /// <summary>
    /// </summary>
    public class cReturnWFHApprove : ResultAPI
    {
        public int? nLevel { get; set; }
        public int? nMode { get; set; }
        public int? ApproveByID { get; set; }
        /// <summary>
        /// </summary>
        public string? sName { get; set; }
        /// <summary>
        /// </summary>
        public string? sPosition { get; set; }
        /// <summary>
        /// </summary>
        public DateTime? dWFHDate { get; set; }
        /// <summary>
        /// </summary>
        public bool? IsOnsite { get; set; }
        public bool? IsEmergency { get; set; }
        /// <summary>
        /// task view
        /// </summary>
        public List<ObjTaskWFH>? lstTaskview { get; set; }
        /// <summary>
        /// </summary>
        public List<ObjLineApprove>? lstLineApprover { get; set; }
        /// <summary>
        /// task edit
        /// </summary>
        public List<ObjDataTask>? lstTaskWFH { get; set; }
        public DateTime? dMinDate { get; set; }
        public DateTime? dMaxDate { get; set; }
        public List<string>? lstPlanSelect { get; set; }
        public List<ObjDataTask>? lstTaskPlan { get; set; }
        public List<ObjDataTask>? lstTaskOther { get; set; }
    }
    /// <summary>
    /// </summary>
    public class ObjTaskWFH
    {
        public int? No { get; set; }
        public string? sProjectName { get; set; }
        public string? sTaskName { get; set; }
        public string? sDescription { get; set; }
        public decimal? nPlan { get; set; }
    }
    /// <summary>
    /// </summary>
    public class ObjLineApprove
    {
        /// <summary>
        /// </summary>
        public int? nWFHID { get; set; }
        /// <summary>
        /// </summary>
        public int? nEmpID { get; set; }
        /// <summary>
        /// </summary>
        public int? nPositionID { get; set; }
        /// <summary>
        /// </summary>
        public int? sID { get; set; }
        /// <summary>
        /// </summary>
        public int? nNo { get; set; }
        /// <summary>
        /// </summary>
        public string? sName { get; set; }
        /// <summary>
        /// </summary>
        public string? sPosition { get; set; }
        /// <summary>
        /// </summary>
        public bool IsLineApprover { get; set; }
        /// <summary>
        /// </summary>
        public string? sAction
        {
            get
            {
                return IsLineApprover ? "อนุมัติ" : "ไม่อนุมัติ";
            }
        }
    }
    /// <summary>
    /// </summary>
    public class cReturnHistoryWFH : ResultAPI
    {
        /// <summary>
        /// </summary>
        public int? nFocusStep { get; set; }
        /// <summary>
        /// </summary>
        public List<ObjDataHis>? lstDataHis { get; set; }
        public List<cStepper>? lstStepper { get; set; }
    }

    /// <summary>
    /// </summary>
    public class cStepper
    {
        public string? sLabel { get; set; }
        public string? sActionBy { get; set; }
        public string? sDateAction { get; set; }
    }
    public class ObjDataHis
    {
        /// <summary>
        /// </summary>
        public string? sName { get; set; }
        /// <summary>
        /// </summary>
        public string? sPosition { get; set; }
        /// <summary>
        /// </summary>
        public string? sComment { get; set; }
        /// <summary>
        /// </summary>
        public int nStatus { get; set; }
        /// <summary>
        /// </summary>
        public string? sStatus { get; set; }
        /// <summary>
        /// </summary>
        public DateTime? dDateApprove { get; set; }
        /// <summary>
        /// </summary>
        public string? sDateApprove { get; set; }
        public string? sFileLink { get; set; }

    }
    #endregion

    #region ตี้
    /// <summary>
    /// clsFilterWorkFromHome
    /// </summary>
    public class clsFilterWorkFromHome
    {
        public string? sType { get; set; }
        public string? sStart { get; set; }
        public string? sEnd { get; set; }
        public List<string>? listRequest { get; set; }
        public List<string>? listStatus { get; set; }
    }
    /// <summary>
    /// clsResultWorkFormHome
    /// </summary>
    public class clsResultWorkFormHome : ResultAPI
    {


    }
    #endregion

    #region ออร์
    public class cTaskTable : STGrid.PaginationData
    {
        public int? nProjectID { get; set; }
        public string? dStart { get; set; }
        public string? dEnd { get; set; }
        public List<ObjDataTask>? lstDataTask { get; set; }
        public List<string>? lstSelect { get; set; }
        public string? sWFHID { get; set; }
    }
    public class cHR
    {
        public string sID { get; set; }
        public int nNo { get; set; }
        public string sName { get; set; }
        public string sPosition { get; set; }
        public int nPositionID { get; set; }
        public string sAction { get; set; }
    }

    public class cResultTaskTable : Pagination
    {
        public List<ObjectResultTask>? lstData { get; set; }

    }

    public class ObjectResultTask
    {
        public int? No { get; set; }
        public int nID { get; set; }
        public string? sID { get; set; }
        public int? nCustomerID { get; set; }
        public string? sDetail { get; set; }
        public int? nProjectID { get; set; }
        public int? nTypeID { get; set; }
        public string? sProjectName { get; set; }
        public string? sType { get; set; }
        public DateTime dPlanStart { get; set; }
        public string? sStartDate { get; set; }
        public decimal? nManhour { get; set; }
        public DateTime? dPlanEnd { get; set; }
        public string? sEndDate { get; set; }
        public DateTime dUpdate { get; set; }
        public string? sUpdate { get; set; }
    }
    public class cResultTaskPlanTable : Pagination
    {
        public List<ObjectResultTaskPlan>? lstData { get; set; }
    }

    public class ObjectResultTaskPlan
    {
        public int? nPlanID { get; set; }
        public int? nTypeRequest { get; set; }
        public int? nNo { get; set; }
        public int nID { get; set; }
        public string? sID { get; set; }
        public int? nTaskID { get; set; }
        public int? nProjectID { get; set; }
        public string? sProjectName { get; set; }
        public string? sDetail { get; set; }
        public decimal? nPlan { get; set; }
        public decimal? nPlanProcess { get; set; }
        public int? nTaskTypeID { get; set; }
        public string? sTaskType { get; set; }
        public DateTime? dTask { get; set; }
        public string? sDate { get; set; }
        public DateTime? dUpdate { get; set; }
        public string? sUpdate { get; set; }
        public bool? IsNotSelect { get; set; }
    }
    public class cDataInfo : Extensions.Common.STResultAPI.ResultAPI
    {
        public int? nPositionID { get; set; }
        public string? sName { get; set; }
        public string? sPosition { get; set; }
    }

    public class cInitialDataWFH : Extensions.Common.STResultAPI.ResultAPI
    {
        public List<cSelectOption>? lstTypeTask { get; set; }
        public List<cSelectOption>? lstProject { get; set; }
        public List<cSelectOption>? lstPosition { get; set; }
        public List<cSelectOptions>? lstApprover { get; set; }
        public string[]? arrHoliday { get; set; }
        public string? sName { get; set; }
        public string? sPosition { get; set; }
        public cHR? objHR { get; set; }
    }
    public class cSelectOptions
    {
        public string? label { get; set; }
        public string? value { get; set; }
        public string? Parent { get; set; }
        public string? sPosition { get; set; }

    }

    #endregion

    public class cWFHSave
    {
        public string? sWFHID { get; set; }
        public int nMode { get; set; }
        public int nApproveBy { get; set; }
        public decimal nManhour { get; set; } = 0;
        public string? dWFHDate { get; set; }
        public string? sComment { get; set; }
        public bool IsOnsite { get; set; }
        public bool IsSos { get; set; }
        public List<ObjDataTask>? lstDataTask { get; set; }
        //public List<lstDataApprovals>? lstDataApprovals { get; set; }


    }
    public class lstDataApprovals
    {
        public int nPositionID { get; set; }
        public int? ApproveByID { get; set; }
    }
    public class ObjDataTask
    {
        public int? nID { get; set; }
        public string? sID { get; set; }
        public int? nTaskID { get; set; }
        public int? nPlanType { get; set; }
        public int? nPlanID { get; set; }
        public int nProjectID { get; set; }
        public string? sProjectName { get; set; }
        public DateTime? dTask { get; set; }
        public string? sDate { get; set; }
        public string? sDetail { get; set; }
        public decimal? nPlan { get; set; }
        public decimal? nPlanProcess { get; set; }
        public string? sTaskType { get; set; }
        public int nTaskTypeID { get; set; }
        public int nTypeRequest { get; set; }
    }



}



