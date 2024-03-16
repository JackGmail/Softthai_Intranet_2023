using Backend.EF.ST_Intranet;
using Backend.Interfaces;
using Backend.Interfaces.Authentication;
using Backend.Models;
using Backend.Models.Authentication;
using ST.INFRA;
using ST_API.Interfaces;
using ST_API.Models.ITypeleaveService;
using static Backend.Enum.EnumGlobal;

namespace Backend.Services.ISystemService
{
    public class TasksService : ITasksService
    {
        private readonly ST_IntranetEntity _db;
        private readonly IAuthentication _authen;
        private readonly IHostEnvironment _env;
        private readonly ParamJWT _user;

        public TasksService(ST_IntranetEntity db, IAuthentication authen, IHostEnvironment env)
        {
            _db = db;
            _authen = authen;
            _env = env;
            _user = _authen.GetUserAccount();
        }

        public ResultAPI getDentalRemainMoney()
        {
            ResultAPI result = new ResultAPI();
            try
            {
                DateTime dNow = DateTime.Now;
                List<TB_Employee> lstEmployee = _db.TB_Employee.Where(w => (w.dPromote != null || w.dWorkStart != null) && w.IsActive && !w.IsRetire && !w.IsDelete).ToList();

                #region เงินทันตกรรม/employee
                foreach (var item in lstEmployee)
                {
                    #region default amount/job year
                    DateTime dWorkExperience = dNow;
                    var dEmployeeStartWork = _db.TB_Employee.FirstOrDefault(w => w.nEmployeeID == item.nEmployeeID && w.IsActive && !w.IsDelete && !w.IsRetire);
                    if (dEmployeeStartWork != null)
                    {
                        dWorkExperience = dEmployeeStartWork.dWorkStart ?? default;
                    }

                    TimeSpan dEmpWorkYear = dNow.Subtract(dWorkExperience);
                    int nEmpWorkYear = new DateTime(dEmpWorkYear.Ticks).Year;
                    #endregion

                    int? nCondition = 0; //จำนวนเงินที่เบิกได้/ปี

                    #region จำนวนเงินที่เบิกได้/ปี
                    TM_Config? objConfig_MoreThreeYear = _db.TM_Config.FirstOrDefault(w => w.nConfigID == ((int)Config.WorkYear_ThreeYear));
                    TM_Config? objConfig_UnderThreeYear = _db.TM_Config.FirstOrDefault(w => w.nConfigID == ((int)Config.WorkYear_UnderThreeYear));

                    if (nEmpWorkYear < 3)
                    {
                        if (objConfig_UnderThreeYear != null)
                        {
                            nCondition = Decimal.ToInt32(objConfig_UnderThreeYear.nValue ?? 0);
                        }
                    }
                    else
                    {
                        if (objConfig_MoreThreeYear != null)
                        {
                            nCondition = Decimal.ToInt32(objConfig_MoreThreeYear.nValue ?? 0);
                        }
                    }
                    #endregion

                    var checkEmployeePerYear = _db.TB_Employee_Dental_Year.Where(w => w.nYear == dNow.Year && w.nEmpID == item.nEmployeeID).ToList();
                    if (checkEmployeePerYear.Count() == 0)
                    {
                        #region TB_Employee_Dental_Year
                        TB_Employee_Dental_Year? objDentalPerYear = _db.TB_Employee_Dental_Year.FirstOrDefault(w => w.nEmpID == item.nEmployeeID);
                        if (objDentalPerYear == null)
                        {
                            objDentalPerYear = new TB_Employee_Dental_Year();
                            objDentalPerYear.nEmpDentalID = (_db.TB_Employee_Dental_Year.Any() ? _db.TB_Employee_Dental_Year.Max(m => m.nEmpDentalID) : 0) + 1;
                            objDentalPerYear.nEmpID = item.nEmployeeID;
                            objDentalPerYear.dCreateDate = dNow;
                            objDentalPerYear.nCreateBy = 999;
                            _db.TB_Employee_Dental_Year.Add(objDentalPerYear);
                        }
                        objDentalPerYear.nMoney = nCondition ?? 0;
                        objDentalPerYear.IsDelete = false;
                        objDentalPerYear.dUpdateDate = dNow;
                        objDentalPerYear.nUpdateBy = 999;
                        if (objDentalPerYear.nEmpID == item.nEmployeeID && objDentalPerYear.nYear == 0)
                        {
                            objDentalPerYear.nYear = dNow.Year;
                        }
                        _db.SaveChanges();
                        #endregion
                    }
                }
                #endregion

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.sMessage = e.Message;
                result.nStatusCode = StatusCodes.Status500InternalServerError;
            }
            return result;
        }

        public ResultAPI LoadDataLeaveSummaryTotalDate()
        {
            ResultAPI result = new ResultAPI();
            try
            {
                DateTime dNow = DateTime.Now;
                List<TB_Employee> lstEmployee = _db.TB_Employee.Where(w => (w.dPromote != null || w.dWorkStart != null) && w.IsActive && !w.IsRetire && !w.IsDelete).ToList();
                var lstType = _db.TB_LeaveType.Where(w => !w.IsDelete && w.IsActive).ToList();
                int nSummarryID = (_db.TB_LeaveSummary.Any() ? _db.TB_LeaveSummary.Max(m => m.nLeaveSummaryID) : 0) + 1;

                foreach (var f in lstType)
                {
                    decimal nLeaveRights = 0;
                    int nMonthSetting = 0;
                    var objInfo = _db.TB_LeaveSummary.Where(w => w.nQuantity >= 0 && w.nLeaveRemain >= 0 && w.nYear == dNow.Year && w.nLeaveTypeID == f.nLeaveTypeID && lstEmployee.Select(s => s.nEmployeeID).Contains(w.nEmployeeID) && !w.IsDelete).ToList();
                    var lstLeaveProportion = _db.TB_LeaveProportion.Where(w => w.nLeaveTypeID == f.nLeaveTypeID).ToList();
                    var LeaveSetting = _db.TB_LeaveSetting.Where(w => w.nLeaveTypeID == f.nLeaveTypeID && !w.IsDelete).ToList();

                    foreach (var item in lstEmployee.Where(w => w.sSex == f.sSex || 87 == f.sSex))
                    {
                        int nWorksYear = item.dPromote != null ? item.dPromote.Value.Year : item.dWorkStart.Value.Year;
                        int nYear = (dNow.Year - nWorksYear) * 12;
                        int nWorksMonth = item.dPromote != null ? item.dPromote.Value.Month : item.dWorkStart.Value.Month;
                        nMonthSetting = (dNow.Month - nWorksMonth) + nYear;

                        var TB_LeaveProportion = lstLeaveProportion.FirstOrDefault(w => w.nLeaveTypeID == f.nLeaveTypeID && w.nEmployeeTypeID == item.nEmployeeTypeID);
                        if (TB_LeaveProportion != null && nMonthSetting > 12 && nMonthSetting < 24)
                        {
                            switch (nWorksMonth)
                            {
                                case 12:
                                    nLeaveRights = TB_LeaveProportion.nDec;
                                    break;
                                case 11:
                                    nLeaveRights = TB_LeaveProportion.nNov;
                                    break;
                                case 10:
                                    nLeaveRights = TB_LeaveProportion.nOct;
                                    break;
                                case 9:
                                    nLeaveRights = TB_LeaveProportion.nSep;
                                    break;
                                case 8:
                                    nLeaveRights = TB_LeaveProportion.nAug;
                                    break;
                                case 7:
                                    nLeaveRights = TB_LeaveProportion.nJul;
                                    break;
                                case 6:
                                    nLeaveRights = TB_LeaveProportion.nJun;
                                    break;
                                case 5:
                                    nLeaveRights = TB_LeaveProportion.nMay;
                                    break;
                                case 4:
                                    nLeaveRights = TB_LeaveProportion.nApr;
                                    break;
                                case 3:
                                    nLeaveRights = TB_LeaveProportion.nMar;
                                    break;
                                case 2:
                                    nLeaveRights = TB_LeaveProportion.nFeb;
                                    break;
                                case 1:
                                    nLeaveRights = TB_LeaveProportion.nJan;
                                    break;
                            }
                        }
                        else
                        {
                            var objLeaveSetting = LeaveSetting.FirstOrDefault(w => w.nLeaveTypeID == f.nLeaveTypeID && nMonthSetting >= w.nWorkingAgeStart && nMonthSetting <= w.nWorkingAgeEnd && w.nEmployeeTypeID == item.nEmployeeTypeID);
                            if (objLeaveSetting != null)
                            {
                                nLeaveRights = objLeaveSetting.nLeaveRights;
                            }
                        }

                        var objInfonew = objInfo.FirstOrDefault(f => f.nEmployeeID == item.nEmployeeID && f.nYear == dNow.Year);
                        if (objInfonew == null)
                        {
                            objInfonew = new TB_LeaveSummary();
                            objInfonew.nLeaveSummaryID = nSummarryID;
                            objInfonew.nEmployeeID = item.nEmployeeID;
                            objInfonew.nLeaveTypeID = f.nLeaveTypeID;
                            objInfonew.dCreate = dNow;
                            objInfonew.nCreateBy = 999;
                            objInfonew.nYear = dNow.Year + 1;
                            objInfonew.nQuantity = nLeaveRights;
                            objInfonew.nTransferred = 0;
                            objInfonew.nLeaveUse = 0;
                            objInfonew.nIntoMoney = 0;
                            objInfonew.nLeaveRemain = nLeaveRights;
                            objInfonew.dUpdate = dNow;
                            objInfonew.nUpdateBy = 999;
                            objInfonew.IsDelete = false;
                            _db.TB_LeaveSummary.Add(objInfonew);
                        }
                        else if (objInfonew != null && objInfonew.nQuantity == 0 && objInfonew.nLeaveRemain == 0)
                        {
                            // objInfonew.nQuantity = nLeaveRights;
                            // objInfonew.nTransferred = objInfonew.nTransferred;
                            // objInfonew.nLeaveUse = objInfonew.nLeaveUse;
                            // objInfonew.nIntoMoney = objInfonew.nIntoMoney;
                            // objInfonew.nLeaveRemain = nLeaveRights;
                            // objInfonew.dUpdate = dNow;
                            // objInfonew.nUpdateBy = 999;
                            // objInfonew.IsDelete = false;


                            objInfonew.nQuantity = nLeaveRights;
                            objInfonew.nTransferred = 0;
                            objInfonew.nLeaveUse = 0;
                            objInfonew.nIntoMoney = 0;
                            objInfonew.nLeaveRemain = nLeaveRights;
                            objInfonew.dUpdate = dNow;
                            objInfonew.nUpdateBy = 999;
                            objInfonew.IsDelete = false;
                        }

                        nSummarryID += 1;
                    }
                    _db.SaveChanges();
                }
                _db.SaveChanges();

                result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                result.sMessage = e.Message;
                result.nStatusCode = StatusCodes.Status500InternalServerError;
            }
            return result;
        }
    }
}