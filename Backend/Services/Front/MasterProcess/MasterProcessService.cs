using Backend.EF.ST_Intranet;
using Backend.Interfaces.Authentication;
using Extensions.Common.STFunction;
using Extensions.Common.STResultAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST.INFRA;
using ST.INFRA.Common;
using ST_API.Controllers;
using ST_API.Interfaces;
using ST_API.Models;


namespace ST_API.Service
{
    public class MasterProcessService : IMasterProcessService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _Auth;
        private readonly IHostEnvironment _env;
        public MasterProcessService(ST_IntranetEntity db, IAuthentication auth, IHostEnvironment env)
        {
            _db = db;
            _Auth = auth;
            _env = env;
        }

        public cFilterMainProcess LoadMainProcessOptions()
        {
            cFilterMainProcess result = new cFilterMainProcess();
            try
            {
                var lstMainProcessOptions = _db.TB_MasterProcess.Where(w => w.nMasterProcessTypeID == 1 && !w.IsDelete && w.IsActive).ToList();

                result.lstMainProcess = lstMainProcessOptions.Select((s, Index) => new cReqMainProcess
                {
                    nMasterProcessID = s.nMasterProcessID,
                    value = s.nMasterProcessID.ToString(),
                    label = s.sMasterProcessName,

                }).ToList();

                result.Status = StatusCodes.Status200OK;
            }
            catch (System.Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;
            }
            return result;

        }

        public ResultAPI SaveProcessData(cReqMasterProcessData req)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                int nUserID = 7;
                var objMasterProcess = _db.TB_MasterProcess.FirstOrDefault(w => w.nMasterProcessID == req.sID.DecryptParameter().ToInt());
                int nNewMasterProcessID = (_db.TB_MasterProcess.Any() ? _db.TB_MasterProcess.Max(m => m.nMasterProcessID) : 0) + 1;
                int nNewMasterProcessTaskID = (_db.TB_MasterProcess_Task.Any() ? _db.TB_MasterProcess_Task.Max(m => m.nMasterProcessTaskID) : 1) + 1;

                if (objMasterProcess == null)
                {

                    int newOrder = (_db.TB_MasterProcess.Where(w => w.nParentID == req.nParentID && !w.IsDelete).Any() ? _db.TB_MasterProcess.Where(w => w.nParentID == req.nParentID && !w.IsDelete).Max(m => m.nOrder) : 0) + 1;

                    objMasterProcess = new TB_MasterProcess();

                    objMasterProcess.nMasterProcessID = nNewMasterProcessID;
                    objMasterProcess.nMasterProcessTypeID = req.nMasterProcessTypeID;
                    objMasterProcess.nParentID = req.nParentID;
                    objMasterProcess.nOrder = newOrder;

                    objMasterProcess.dCreate = DateTime.Now;
                    objMasterProcess.nCreateBy = nUserID;

                    _db.TB_MasterProcess.Add(objMasterProcess);

                }

                objMasterProcess.sMasterProcessName = req.sMasterProcessName;
                objMasterProcess.IsStandProcess = req.IsStandProcess;
                objMasterProcess.sNote = req.sNote;
                objMasterProcess.IsActive = req.IsActive;

                objMasterProcess.dUpdate = DateTime.Now;
                objMasterProcess.nUpdateBy = nUserID;
                objMasterProcess.IsDelete = false;

                //Add Task
                var lstTask = _db.TB_MasterProcess_Task.Where(w => w.nMasterProcessID == objMasterProcess.nMasterProcessID).ToList();

                //Set Delete Record ที่ไม่ได้ส่ง Param มา
                foreach (var iL in lstTask.Where(w => !w.IsDelete).ToList())
                {
                    if (!req.lstTaskData.Select(s => s.nID).Contains(iL.nMasterProcessTaskID))
                    {
                        iL.IsDelete = true;
                        iL.dDelete = DateTime.Now;
                        iL.nDeleteBy = nUserID;
                    }
                }

                //edit task
                foreach (var item in req.lstTaskData ?? new List<cReqMasterProcessTaskData>())
                {
                    var objTask = _db.TB_MasterProcess_Task.FirstOrDefault(w => w.nMasterProcessID == objMasterProcess.nMasterProcessID && w.nMasterProcessTaskID == item.nID);

                    if (objTask == null)
                    {
                        objTask = new TB_MasterProcess_Task();
                        objTask.nMasterProcessTaskID = nNewMasterProcessTaskID;
                        objTask.nMasterProcessID = objMasterProcess.nMasterProcessID;
                        objTask.dCreate = DateTime.Now;
                        objTask.nCreateBy = nUserID;
                        objTask.IsDelete = false;

                        _db.TB_MasterProcess_Task.Add(objTask);
                        nNewMasterProcessTaskID++;

                    }

                    objTask.sMasterProcessTaskName = item.sMasterProcessTaskName;
                    objTask.nOrder = item.nOrder;
                    objTask.dUpdate = DateTime.Now;
                    objTask.nUpdateBy = nUserID;

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

        public cFilterMasterProcessTable LoadMasterProcessData(cMasterProcessData req)
        {
            cFilterMasterProcessTable result = new cFilterMasterProcessTable();

            try
            {

                var lstMasterProcess = _db.TB_MasterProcess.Where(w => !w.IsDelete).ToList();

                var qry = lstMasterProcess.Where(w => !w.IsDelete && w.nMasterProcessTypeID == 1).Select((s) => new cMasterProcessData
                {
                    nID = s.nMasterProcessID,
                    sID = s.nMasterProcessID.EncryptParameter(),
                    nOrder = s.nOrder,
                    sMasterProcessName = s.sMasterProcessName,
                    nMasterProcessTypeID = s.nMasterProcessTypeID,
                    nSubProcess = lstMasterProcess.Where(w => w.nMasterProcessTypeID == 2 && w.nParentID == s.nMasterProcessID).Count(),
                    sNote = s.sNote,
                    dLastUpdate = s.dUpdate,

                }).ToArray();



                var lstOrder = qry.Select(s => s.nOrder).OrderBy(o => o).Distinct().ToList();
                List<cReqOrderOptions> lstOrderOptions = new List<cReqOrderOptions>();
                foreach (var item in lstOrder)
                {
                    var objOptions = new cReqOrderOptions();
                    objOptions.value = item;
                    objOptions.label = item.ToString();
                    lstOrderOptions.Add(objOptions);
                }

                result.lstOrderOptions = lstOrderOptions;

                #region//SORT
                string sSortColumn = req.sSortExpression;
                switch (req.sSortExpression)
                {
                    case "nOrder": sSortColumn = "nOrder"; break;
                    case "sMasterProcessName": sSortColumn = "sMasterProcessName"; break;
                    case "nSubProcess": sSortColumn = "nSubProcess"; break;
                    case "dLastUpdate": sSortColumn = "dLastUpdate"; break;
                }
                if (req.isASC)
                {
                    qry = qry.OrderBy<cMasterProcessData>(sSortColumn).ToArray();
                }
                else if (req.isDESC)
                {
                    qry = qry.OrderByDescending<cMasterProcessData>(sSortColumn).ToArray();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(req.nPageSize, req.nPageIndex, qry.Count());
                result.lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
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

        public cFilterMasterProcessTable LoadSubProcessData(cMasterProcessData req)
        {
            cFilterMasterProcessTable result = new cFilterMasterProcessTable();

            try
            {

                var lstMasterProcess = _db.TB_MasterProcess.Where(w => !w.IsDelete).ToList();

                var qry = lstMasterProcess.Where(w => !w.IsDelete && w.nMasterProcessTypeID == 2 && w.nParentID == req.sID.DecryptParameter().ToInt()).Select((s) => new cMasterProcessData
                {
                    nID = s.nMasterProcessID,
                    sID = s.nMasterProcessID.EncryptParameter(),
                    nOrder = s.nOrder,
                    sMasterProcessName = s.sMasterProcessName,
                    nParentID = s.nParentID,
                    nMasterProcessTypeID = s.nMasterProcessTypeID,
                    sNote = s.sNote,
                    dLastUpdate = s.dUpdate,

                }).ToArray();

                var lstOrder = qry.Select(s => s.nOrder).OrderBy(o => o).Distinct().ToList();
                List<cReqOrderOptions> lstOrderOptions = new List<cReqOrderOptions>();
                foreach (var item in lstOrder)
                {
                    var objOptions = new cReqOrderOptions();
                    objOptions.value = item;
                    objOptions.label = item.ToString();
                    lstOrderOptions.Add(objOptions);
                }

                result.lstOrderOptions = lstOrderOptions;

                #region//SORT
                string sSortColumn = req.sSortExpression;
                switch (req.sSortExpression)
                {
                    case "nOrder": sSortColumn = "nOrder"; break;
                    case "sMasterProcessName": sSortColumn = "sMasterProcessName"; break;
                    case "sLastUpdate": sSortColumn = "sLastUpdate"; break;
                }
                if (req.isASC)
                {
                    qry = qry.OrderBy<cMasterProcessData>(sSortColumn).ToArray();
                }
                else if (req.isDESC)
                {
                    qry = qry.OrderByDescending<cMasterProcessData>(sSortColumn).ToArray();
                }
                #endregion

                #region//Final Action >> Skip , Take And Set Page
                var dataPage = STGrid.Paging(req.nPageSize, req.nPageIndex, qry.Count());
                var lstData = qry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                result.lstData = lstData;
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

        public ResultAPI ChangeOrder(cMasterProcessOrder req)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                //main process order
                var lstMasterData = _db.TB_MasterProcess.Where(w => !w.IsDelete && w.nParentID == null).OrderBy(o => o.nOrder).ToList();
                var nID = (req.sID ?? "").DecryptParameter().ToInt();
                var nParentID = (req.sSubProcessID ?? "").DecryptParameter().ToInt();
                var objData = lstMasterData.FirstOrDefault(f => f.nMasterProcessID == nID);

                //sub process order
                var lstSubData = _db.TB_MasterProcess.Where(w => !w.IsDelete && w.nMasterProcessTypeID == 2 && w.nParentID == nParentID).OrderBy(o => o.nOrder).ToList();
                var objSubData = lstSubData.FirstOrDefault(f => f.nMasterProcessID == nID && f.nParentID == nParentID);

                if (objData != null)
                {
                    int oldOrder = objData.nOrder;  // nOrder ใน Table or เลขที่ได้รับผลกระทบ
                    int newOrder = req.nOrder;      // nOrder ที่ส่งจากหน้าบ้าน or เลขที่ต้องการเปลี่ยน
                    bool isOldMoreThanNew = oldOrder > newOrder;

                    var lstDataChange = lstMasterData.Where(w => w.nMasterProcessID != objData.nMasterProcessID && (isOldMoreThanNew ?
                    (oldOrder >= w.nOrder && newOrder <= w.nOrder) : (newOrder >= w.nOrder && oldOrder <= w.nOrder))).ToList();

                    int setOrder = isOldMoreThanNew ? newOrder : oldOrder;
                    foreach (var item in lstDataChange)
                    {
                        item.nOrder = isOldMoreThanNew ? ++setOrder : setOrder++;
                    }
                    objData.nOrder = newOrder;


                }
                else if (objSubData != null)
                {
                    int oldOrder = objSubData.nOrder;  // nOrder ใน Table or เลขที่ได้รับผลกระทบ
                    int newOrder = req.nOrder;      // nOrder ที่ส่งจากหน้าบ้าน or เลขที่ต้องการเปลี่ยน
                    bool isOldMoreThanNew = oldOrder > newOrder;

                    var lstSubDataChange = lstSubData.Where(w => w.nMasterProcessID != objSubData.nMasterProcessID && (isOldMoreThanNew ?
                    (oldOrder >= w.nOrder && newOrder <= w.nOrder) : (newOrder >= w.nOrder && oldOrder <= w.nOrder))).ToList();

                    int setOrder = isOldMoreThanNew ? newOrder : oldOrder;
                    foreach (var item in lstSubDataChange)
                    {
                        item.nOrder = isOldMoreThanNew ? ++setOrder : setOrder++;
                    }
                    objSubData.nOrder = newOrder;

                }

                _db.SaveChanges();  //nOrder ติดลบกระจาย
                result.Status = StatusCodes.Status200OK;

            }
            catch (System.Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;

            }


            return result;

        }

        // getDatatoEdit

        public cMasterProcessValue GetDataToEdit(cReqMasterProcessDataValue req)
        {
            cMasterProcessValue result = new cMasterProcessValue();
            try
            {
                int nID = req.sID.DecryptParameter().ToInt();
                var objData = _db.TB_MasterProcess.FirstOrDefault(f => f.nMasterProcessID == nID);

                if (objData != null)
                {
                    result.nMasterProcessTypeID = objData.nMasterProcessTypeID; // set radio state main || sub
                    // result.isSubProcess = isSubProcess ? true : false ;
                    result.nParentID = objData.nParentID; // set select by parent id 
                    result.sMasterProcessName = objData.sMasterProcessName;
                    result.sNote = objData.sNote;
                    result.IsStandProcess = objData.IsStandProcess;

                    result.IsActive = objData.IsActive;

                    var lstTaskQry = _db.TB_MasterProcess_Task.Where(w => w.nMasterProcessID == objData.nMasterProcessID && !w.IsDelete).Select((s) => new cReqMasterProcessTaskData
                    {
                        nID = s.nMasterProcessTaskID,
                        sID = s.nMasterProcessTaskID.ToString(),
                        sMasterProcessTaskName = s.sMasterProcessTaskName,
                        nOrder = s.nOrder,

                    }).OrderBy(o => o.nOrder).ToList();

                    #region//SORT
                    string sSortColumn = (req != null && !string.IsNullOrEmpty(req.sSortExpression) ? req.sSortExpression : "");
                    switch (req.sSortExpression)
                    {
                        case "nOrder": sSortColumn = "nOrder"; break;
                    }
                    if (req.isASC)
                    {
                        lstTaskQry = lstTaskQry.OrderBy<cReqMasterProcessTaskData>(sSortColumn).ToList();
                    }
                    else if (req.isDESC)
                    {
                        lstTaskQry = lstTaskQry.OrderByDescending<cReqMasterProcessTaskData>(sSortColumn).ToList();
                    }
                    #endregion

                    #region//Final Action >> Skip , Take And Set Page
                    var dataPage = STGrid.Paging(req.nPageSize, req.nPageIndex, lstTaskQry.Count());
                    var lstData = lstTaskQry.Skip(dataPage.nSkip).Take(dataPage.nTake).ToList();
                    result.lstTaskData = lstData;
                    result.nDataLength = dataPage.nDataLength;
                    result.nPageIndex = dataPage.nPageIndex;
                    result.nSkip = dataPage.nSkip;
                    result.nTake = dataPage.nTake;
                    result.nStartIndex = dataPage.nStartIndex;
                    #endregion


                    // result.lstTaskData = lstTaskQry;
                    result.Status = StatusCodes.Status200OK;
                }

            }
            catch (System.Exception e)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = e.Message;

            }

            return result;
        }

        public async Task<ResultAPI> RemoveProcessData(cRemoveTableMaster req)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                int nUserID = 7;

                List<int> lstID = req.lstID.ConvertAll(item => item.DecryptParameter().ToInt()).ToList();
                var lstData = _db.TB_MasterProcess.Where(w => lstID.Contains(w.nMasterProcessID)).ToList();

                foreach (var item in lstData)
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