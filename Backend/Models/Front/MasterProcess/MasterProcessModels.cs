using Extensions.Common.STExtension;
using Extensions.Common.STResultAPI;
using ST.INFRA.Common;


namespace ST_API.Models
{
    #region MasterProcessParam

    public class cMasterProcessData : STGrid.PaginationData
    {
        public string? sID { get; set; }
        public int? nID { get; set; }
        public int? nMasterProcessID { get; set; }
        public int? nOrder { get; set; }
        public int? nParentID { get; set; }
        public string? sMasterProcessName { get; set; } = null!;
        public string? sNote { get; set; }
        public int? nMasterProcessTypeID { get; set; }
        public string? sMasterProcessType
        {
            get { return nMasterProcessTypeID == 1 ? "Main Process" : "Sub Process"; }
        }
        public int? nSubProcess { get; set; }
        public bool? IsStandProcess { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? dLastUpdate { get; set; }
        public string? sLastUpdate
        {
            get
            {
                return dLastUpdate.ToStringFromDate();
            }
            
        }
        // public string? sLastUpdate { get; set;}
        public List<cReqMainProcess>? lstMainProcess { get; set; }

    }
    #endregion

    public class cReqMasterProcessData : ResultAPI
    {
        public string? sID { get; set; }
        public int? nID { get; set; }
        public int? nParentID { get; set; }
        public string sMasterProcessName { get; set; } = null!;
        public string? sNote { get; set; }
        public int nMasterProcessTypeID { get; set; }
        public bool IsStandProcess { get; set; }
        public bool IsActive { get; set; }
        public List<cReqMasterProcessTaskData> lstTaskData { get; set; }

    }

    public class cReqMasterProcessTaskData : STGrid.PaginationData
    {
        public int nID { get; set; }
        public string? sID { get; set; } = null!;
        public string? sMasterProcessTaskName { get; set; }
        public int nOrder { get; set; }
    }

    public class cFilterMainProcess : Pagination
    {
        public int nMasterProcessID { get; set; }
        public int nParentID { get; set; }
        public string sMasterProcessName { get; set; }
        public int nMasterProcessTypeID { get; set; }
        public List<cReqMainProcess>? lstMainProcess { get; set; }


    }
    public class cReqMainProcess
    {
        public int? nMasterProcessID { get; set; }
        public string? value { get; set; } = null!;
        public string? label { get; set; } = null!;
    }

    public class cReqOrderOptions
    {
        public int? value { get; set; } = null!;
        public string? label { get; set; } = null!;
    }

    public class cFilterMasterProcessTable : Pagination
    {
        public string? sID { get; set; }
        public int? nID { get; set; }
        public int nOrder { get; set; }
        public string sMasterProcessName { get; set; } = null!;
        public int nMasterProcessTypeID { get; set; }
        public string? sMasterProcessType
        {
            get { return nMasterProcessTypeID == 1 ? "Main Process" : "Sub Process"; }
        }
        public string? sNote { get; set; }
        public DateTime dLastUpdate { get; set; }
        public List<cMasterProcessData> lstData { get; set; }
        public List<cReqOrderOptions>? lstOrderOptions { get; set; }
    }

    public class cMasterProcessOrder {
        public string sID { get; set;}
        public string? sSubProcessID { get; set; }
        public string? sMasterProcessName { get; set; }
        public int nOrder { get; set; }

    }

    public class cMasterProcessValue : Pagination
    {
        public string? sID { get; set; }
        public int? nID { get; set; }
        public int? nMasterProcessID { get; set; }
        public int? nParentID { get; set; }
        public bool? isSubProcess { get; set; }
        public string? sMasterProcessName { get; set; } = null!;
        public string? sNote { get; set; }
        public int? nMasterProcessTypeID { get; set; }
        public string? sMasterProcessType
        {
            get { return nMasterProcessTypeID == 1 ? "Main Process" : "Sub Process"; }
        }
        public int? nSubProcess { get; set; }
        public bool? IsStandProcess { get; set; }
        public bool? IsActive { get; set; }
        public string? sMasterProcessTaskName { get; set;}
        public int? nTaskOrder { get; set;}
        public List<cReqMainProcess>? lstMainProcess { get; set; }
        public List<cReqMasterProcessTaskData>? lstTaskData { get; set; }

    }

    public class cReqMasterProcessDataValue : STGrid.PaginationData
    {
        public string? sID { get; set; }
        public int? nID { get; set; }
        public int? nParentID { get; set; }
        public string? sMasterProcessName { get; set; } = null!;
        public int? nMasterProcessTypeID { get; set; }
        public bool? IsStandProcess { get; set; }
        public bool? IsActive { get; set; }
        public List<cReqMasterProcessTaskData>? lstTaskData { get; set; }

    }

    public class cReqTask 
    {
        public string sTaskID { get; set; }
        public int nTaskID { get; set; }
        
    } 
       public class cRemoveTableMaster : STGrid.PaginationData
    {
        public List<string>? lstID { get; set; } = new List<string>();
    }

}

