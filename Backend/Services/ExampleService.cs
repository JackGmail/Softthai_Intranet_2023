using Extensions.Common.STResultAPI;
using Backend.Interfaces;
using Backend.Models.ExampleModel;
using Backend.EF.ST_Intranet;

namespace Backend.Services
{
    public class ExampleService : IExampleService
    {
        private readonly ST_IntranetEntity _db;

        public ExampleService(ST_IntranetEntity db)
        {
            _db = db;
        }

        public async Task<ResultAPI> SaveData(cExampleData param)
        {
            ResultAPI result = new ResultAPI();
            try
            {
                //T_Example objNew = new T_Example();
                //objNew.nFormID = _db.T_Example.Any() ? _db.T_Example.Max(m => m.nFormID) + 1 : 1;
                //objNew.sInputEmail = param.sInputEmail;
                //objNew.sInputPassword = param.sInputPassword;
                //objNew.sInputTH = param.sInputTH;
                //objNew.sInputEN = param.sInputEN;
                //objNew.nInputNumber = param.nInputNumber;
                //objNew.sInputNumberScientific = param.sInputNumberScientific;
                //objNew.nSelectID = param.nSelectID;
                //objNew.dDay = param.dDay;
                //objNew.nMonth = param.nMonth;
                //objNew.dYearMonth = param.dYearMonth;
                //objNew.nYear = param.nYear;
                //objNew.nQuarter = param.nQuarter;
                //objNew.dDayStart = param.dDayStart;
                //objNew.dDayEnd = param.dDayEnd;
                //objNew.nMonthStart = param.nMonthStart;
                //objNew.nMonthEnd = param.nMonthEnd;
                //objNew.dYearMonthStart = param.dYearMonthStart;
                //objNew.dYearMonthEnd = param.dYearMonthEnd;
                //objNew.nYearStart = param.nYearStart;
                //objNew.nQuarterStart = param.nQuarterStart;
                //objNew.nQuarterEnd = param.nQuarterEnd;
                //objNew.nYearEnd = param.nYearEnd;
                //objNew.dDateTime = param.dDateTime;
                //objNew.dDateTimeStart = param.dDateTimeStart;
                //objNew.dDateTimeEnd = param.dDateTimeEnd;

                //await _db.T_Example.AddAsync(objNew);
                //await _db.SaveChangesAsync();

                //result.Status = StatusCodes.Status200OK;
            }
            catch (System.Exception ex)
            {
                result.Status = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<cExampleData> GetData()
        {
            cExampleData result = new cExampleData();
            try
            {
                //int nMaxID = _db.T_Example.Any() ? _db.T_Example.Max(m => m.nFormID) : 0;
                //T_Example? objExam = _db.T_Example.FirstOrDefault(f => f.nFormID == nMaxID);
                //if (objExam != null)
                //{
                //    result.nFormID = objExam.nFormID;
                //    result.sInputEmail = objExam.sInputEmail;
                //    result.sInputPassword = objExam.sInputPassword;
                //    result.sInputTH = objExam.sInputTH;
                //    result.sInputEN = objExam.sInputEN;
                //    result.nInputNumber = objExam.nInputNumber;
                //    result.sInputNumberScientific = objExam.sInputNumberScientific;
                //    result.nSelectID = objExam.nSelectID;
                //    result.dDay = objExam.dDay;
                //    result.nMonth = objExam.nMonth;
                //    result.dYearMonth = objExam.dYearMonth;
                //    result.nYear = objExam.nYear;
                //    result.nQuarter = objExam.nQuarter;
                //    result.dDayStart = objExam.dDayStart;
                //    result.dDayEnd = objExam.dDayEnd;
                //    result.nMonthStart = objExam.nMonthStart;
                //    result.nMonthEnd = objExam.nMonthEnd;
                //    result.dYearMonthStart = objExam.dYearMonthStart;
                //    result.dYearMonthEnd = objExam.dYearMonthEnd;
                //    result.nYearStart = objExam.nYearStart;
                //    result.nQuarterStart = objExam.nQuarterStart;
                //    result.nQuarterEnd = objExam.nQuarterEnd;
                //    result.nYearEnd = objExam.nYearEnd;
                //    result.dDateTime = objExam.dDateTime;
                //    result.dDateTimeStart = objExam.dDateTimeStart;
                //    result.dDateTimeEnd = objExam.dDateTimeEnd;
                //}

                //result.nStatusCode = StatusCodes.Status200OK;
            }
            catch (System.Exception ex)
            {
                result.nStatusCode = StatusCodes.Status500InternalServerError;
                result.sMessage = ex.Message;
            }
            return result;
        }
    }
}

