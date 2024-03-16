using Extensions.Common.STResultAPI;
using ST_API.Models.IHolidayService;

namespace ST_API.Interfaces
{
    public interface IHolidayService
    {
        ResultAPI SaveDataHoliday(cHolidayYear req);
        cFilterHolidayDayTable GetHolidayList(cHolidayData param);
        cReturnHolidayList GetHolidayYearList(cReqHolidayYear param);
        cHolidayYear GetYearOptionList();
        ResultAPI DuplicateHolidayList(cYearList req);
        Task<cExportOutput> ExportPDFHoliday(cHolidayData param);
        Task<ResultAPI> RemoveDataTable(cRemoveTableHoliday param);

    }


}